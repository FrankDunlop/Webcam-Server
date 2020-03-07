using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using USBDev;
using WebCamSel;
using ClientHandle;
using System.Resources;
using Webcam_Server.Properties;

namespace Webcam_Server
{
    public partial class ServerFrm : Form
    {
        ToolTip tooltip = new ToolTip();
        USBDevice webcam = new USBDevice();
        WebCam_Select WebCamSel = new WebCam_Select();
        
        public byte[] Frame { get; set; }
        Thread CameraThread{ get; set; }
        TcpListener Listener { get; set; }
        IPAddress Address { get; set; }
        int port = 0;
        bool Connected = false;
        public MemoryStream ms = new MemoryStream();

        public ServerFrm()
        {
            InitializeComponent();
            tooltip.AutoPopDelay = 2000;
            tooltip.InitialDelay = 500;
            tooltip.ReshowDelay = 500;
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 0; i < addr.Length; i++)
            {
                ServerIPcbo.Items.Add(addr[i]);
            }
            ServerIPcbo.Items.Add("127.0.0.1");
            ServerIPcbo.SelectedIndex = 0;
        }

        private Bitmap GetImage(string image)
        {
            ResourceManager RM = new ResourceManager("Webcam_Server.Properties.Resources", typeof(Resources).Assembly);
            return new Bitmap((Bitmap)RM.GetObject(image));
        }

        private void BtnStartServer_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                CameraWindow.Image = GetImage("CamConnected");
                BtnStartServer.BackgroundImage = GetImage("BtnConnected");
                webcam.StopDevice();
                Connected = false;
                CameraThread.Abort();
                Listener.Stop();
            }
            else
            {
                ConnectCamera();

                try
                {
                    port = Convert.ToInt32(PortNumtxt.Text);
                    Address = IPAddress.Parse(ServerIPcbo.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Client IP or Port Error");
                    return;
                }

                if (PortNumtxt.Text == string.Empty)
                {
                    MessageBox.Show("Enter Port Number");
                }
                else
                {
                    CameraThread = new Thread(Listen);
                    CameraThread.Start();
                }
            }
        }

        private void Listen()
        {
            Listener = new TcpListener(Address, port);
            Listener.Start();
            while (true)
            {
                ClientHandler client = new ClientHandler(Listener.AcceptTcpClient(), this);
            }
        }

        public void ConnectCamera()
        {
            if (WebCamSel.DevicesConnected() > 1)
            {
                WebCamSel.ShowDialog();
            }
            webcam = WebCamSel.SelectedDevice;

            if (webcam != null)
            {
                webcam.FrameCaptured += new WebCameraFrameDelegate(ShowWebImage);
                int handle = CameraWindow.Handle.ToInt32();

                if (webcam.WebcamConnect(handle, 320, 240))
                {
                    Connected = true;
                    BtnStartServer.BackgroundImage = GetImage("BtnConnected");
                }
                else 
                {
                    CameraWindow.Image = GetImage("CamDisconnected");
                    BtnStartServer.BackgroundImage = GetImage("BtnDisConnect");
                    Connected = false;
                    return; 
                }

                CameraWindow.Image = GetImage("CamConnecting");
                webcam.StartDevice();
            }
        }

        private void ShowWebImage(object sender, WebCameraEventArgs e)
        {
            CameraWindow.Image = e.Frame;
            GetClipBoardBitmap();
        }

        public void GetClipBoardBitmap()
        {
            Image img = webcam.GetBitmap();
            if (img != null)
            {
                img.Save(ms, ImageFormat.Bmp);
            }
        }

        private void BtnStartServer_MouseHover(object sender, EventArgs e)
        {
            tooltip.SetToolTip(BtnStartServer, "Server Start/Stop");
        }
    }
}