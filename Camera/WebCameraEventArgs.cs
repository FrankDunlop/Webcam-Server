using System;
using System.Drawing;

namespace Webcam_Server
{
    public class WebCameraEventArgs : EventArgs
    {
        public Bitmap Frame { get; set; }

        public WebCameraEventArgs(Bitmap frame)
        {
            this.Frame = frame;
        }
    }
}
