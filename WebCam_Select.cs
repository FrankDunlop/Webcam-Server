using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using USBDev;

namespace WebCamSel
{
    public partial class WebCam_Select : Form
    {
        [DllImport("avicap32.dll")]
        static extern bool capGetDriverDescription(int wDriverIndex,
        [MarshalAs(UnmanagedType.VBByRefStr)]ref String lpszName, int cbName, [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszVer, int cbVer);

        public USBDevice SelectedDevice { get; set; }

        static ArrayList Webcams = new ArrayList();
        USBDevice WebCamDevice = new USBDevice();
        int NumDevices = 0;
        
        public WebCam_Select()
        {
            InitializeComponent();
            ListUSBDevices();
        }

        public void ListUSBDevices()
        {
            string name = "".PadRight(40);
            string version = "".PadRight(25);
            cboUSBDevices.Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                if (capGetDriverDescription(i, ref name, 40, ref version, 25))
                {
                    USBDevice webcam = new USBDevice();
                    webcam.Name = name.Trim();
                    webcam.Version = version.Trim();
                    Webcams.Add(webcam);
                    cboUSBDevices.Items.Add(webcam);
                    NumDevices += 1;
                }
            }
            cboUSBDevices.SelectedIndex = 0;
        }

        public int DevicesConnected()
        {
            if (NumDevices == 1)
            {
                SelectedDevice = (USBDevice)Webcams[0];
            }
            return NumDevices;
        }

        public void btnConnect_Click(object sender, EventArgs e)
        {
            if (cboUSBDevices.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select A Device");
            }
            else
            {
                SelectedDevice = (USBDevice)Webcams[cboUSBDevices.SelectedIndex];
                Close();
            }
        }
    }
}
