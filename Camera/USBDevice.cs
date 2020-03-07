using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Webcam_Server;

namespace USBDev
{
    public delegate void WebCameraFrameDelegate(object sender, WebCameraEventArgs e);

    public class USBDevice
    {
        int camwindowhandle = 0;
        int CaptureWidth = 0;
        int CaptureHeight = 0;
        bool fault = false;

        //the event that will be used when a frame is grabbed
        public event WebCameraFrameDelegate FrameCaptured;

        //create an array of webcam objects
        static ArrayList Webcams = new ArrayList();

        private const short WM_CAP = 1024;
        private const int WM_CAP_DRIVER_CONNECT = 1034;
        private const int WM_CAP_DRIVER_DISCONNECT = 1035;
        private const int WM_CAP_SET_PREVIEW = 1074;
        private const int WM_CAP_SET_PREVIEWRATE = 1076;
        private const int WM_CAP_SET_SCALE = 1077;
        private const int WM_CAP_EDIT_COPY = 1054;
        const int WM_CAP_SET_VIDEOFORMAT = 1069;
        const int WM_CAP_GRAB_FRAME_NOSTOP = 1085;
        const int WM_CAP_SET_CALLBACK_FRAME = 1029;
        public const int WM_CAP_GET_FRAME = 1084;

        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;

        [DllImport("avicap32.dll")]
        protected static extern int capCreateCaptureWindowA([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszWindowName,
            int dwStyle, int x, int y, int nWidth, int nHeight, int hWndParent, int nID);

        //This function enables enumerate the web cam devices
        [DllImport("avicap32.dll")]
        protected static extern bool capGetDriverDescriptionA(short wDriverIndex,
            [MarshalAs(UnmanagedType.VBByRefStr)]ref String lpszName,
           int cbName, [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszVer, int cbVer);

        [DllImport("user32", EntryPoint = "SendMessageA")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)] object lParam);
        
        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern int SendBitmapMessage(int hWnd, uint wMsg, int wParam, ref BITMAPINFO lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern int SendHeaderMessage(int hWnd, uint wMsg, int wParam, CallBackDelegate lParam);

        delegate void CallBackDelegate(IntPtr hwnd, ref VIDEOHEADER hdr);
        CallBackDelegate delegateFrameCallBack;

        [DllImport("user32")]
        protected static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32")]
        protected static extern bool DestroyWindow(int hwnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct VIDEOHEADER
        {
            public IntPtr lpData;
            public uint dwBufferLength;
            public uint dwBytesUsed;
            public uint dwTimeCaptured;
            public uint dwUser;
            public uint dwFlags;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.SafeArray)]
            byte[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public int bmiColors;
        }

        public string Name { get; set; }
        public string Version { get; set; }
        Thread captureThread = null;
        int index = 0;
        int deviceHandle;
        Bitmap bmp;

        public USBDevice()
        {
            delegateFrameCallBack = FrameCallBack;
        }

        

        public override string ToString()
        {
            return Name;
        }

        public bool WebcamConnect(int handle, int width, int height)
        {
            CaptureWidth = width;
            CaptureHeight = height;

            BITMAPINFO bInfo = new BITMAPINFO();
            bInfo.bmiHeader = new BITMAPINFOHEADER();
            bInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(bInfo.bmiHeader);
            bInfo.bmiHeader.biWidth = CaptureWidth;
            bInfo.bmiHeader.biHeight = CaptureHeight;
            bInfo.bmiHeader.biPlanes = 1;
            bInfo.bmiHeader.biBitCount = 24; // bits per frame, 24 - RGB
            camwindowhandle = handle;
            string deviceIndex = Convert.ToString(this.index);
            deviceHandle = capCreateCaptureWindowA(ref deviceIndex, WS_CHILD, 0, 0, CaptureWidth, CaptureHeight, camwindowhandle, 0);

            //connect to the device
            if (SendMessage(deviceHandle, WM_CAP_DRIVER_CONNECT, this.index, 0)>0)
            {
                //set the video format
                SendBitmapMessage(deviceHandle, WM_CAP_SET_VIDEOFORMAT, Marshal.SizeOf(bInfo), ref bInfo);
                fault = false;
                return true;
            }
            else
            {
                //if we cant connect to the device
                MessageBox.Show("Cannot Connect to USB Device");
                fault = true;
                return false;
            }
        }

        public void StartDevice()
        {
            if (!fault)
            {
                captureThread = new Thread(new ThreadStart(GetFrame));
                captureThread.Start();
            }
        }

        public void StopDevice()
        {
            captureThread.Abort();
            Stop();
        }

        public Image GetBitmap()
        {
            IDataObject data = null;
            Image img = null;
            SendMessage(deviceHandle, WM_CAP_EDIT_COPY, 0, 0);
            data = Clipboard.GetDataObject();
            if (data.GetDataPresent(typeof(Bitmap)))
            {
                img = (Image)data.GetData(typeof(Bitmap));
            }
            return img;
        }

        public void GetFrame()
        {
            while (true)
            {
                SendMessage(deviceHandle, WM_CAP_GET_FRAME, 0, 0);
                SendHeaderMessage(deviceHandle, WM_CAP_SET_CALLBACK_FRAME, 0, delegateFrameCallBack);
                Thread.Sleep(100);
            }
        }

        public void FrameCallBack(IntPtr hwnd, ref VIDEOHEADER hdr)
        {
            bmp = new Bitmap(CaptureWidth, CaptureHeight, CaptureWidth * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, hdr.lpData);
            //flip the image, the image is captured upside down
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            FrameCaptured(this, new WebCameraEventArgs(bmp));
        }

        public void Stop()
        {
            SendMessage(deviceHandle, WM_CAP_DRIVER_DISCONNECT, this.index, 0);
            DestroyWindow(deviceHandle);
            FrameCaptured = null;
        }
    }
}
