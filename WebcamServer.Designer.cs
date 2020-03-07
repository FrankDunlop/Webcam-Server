namespace Webcam_Server
{
    partial class ServerFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerFrm));
            this.BtnStartServer = new System.Windows.Forms.Button();
            this.CameraWindow = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PortNumtxt = new System.Windows.Forms.TextBox();
            this.ServerIPcbo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.CameraWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStartServer
            // 
            this.BtnStartServer.BackColor = System.Drawing.Color.White;
            this.BtnStartServer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnStartServer.BackgroundImage")));
            this.BtnStartServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnStartServer.Location = new System.Drawing.Point(266, 285);
            this.BtnStartServer.Name = "BtnStartServer";
            this.BtnStartServer.Size = new System.Drawing.Size(81, 72);
            this.BtnStartServer.TabIndex = 0;
            this.BtnStartServer.UseVisualStyleBackColor = false;
            this.BtnStartServer.Click += new System.EventHandler(this.BtnStartServer_Click);
            this.BtnStartServer.MouseHover += new System.EventHandler(this.BtnStartServer_MouseHover);
            // 
            // CameraWindow
            // 
            this.CameraWindow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CameraWindow.BackgroundImage")));
            this.CameraWindow.Location = new System.Drawing.Point(18, 19);
            this.CameraWindow.Name = "CameraWindow";
            this.CameraWindow.Size = new System.Drawing.Size(320, 240);
            this.CameraWindow.TabIndex = 1;
            this.CameraWindow.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(49, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "SERVER IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(180, 299);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "PORT";
            // 
            // PortNumtxt
            // 
            this.PortNumtxt.Location = new System.Drawing.Point(183, 316);
            this.PortNumtxt.MaxLength = 4;
            this.PortNumtxt.Name = "PortNumtxt";
            this.PortNumtxt.Size = new System.Drawing.Size(38, 20);
            this.PortNumtxt.TabIndex = 5;
            this.PortNumtxt.Text = "8001";
            this.PortNumtxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ServerIPcbo
            // 
            this.ServerIPcbo.FormattingEnabled = true;
            this.ServerIPcbo.Location = new System.Drawing.Point(37, 315);
            this.ServerIPcbo.Name = "ServerIPcbo";
            this.ServerIPcbo.Size = new System.Drawing.Size(107, 21);
            this.ServerIPcbo.TabIndex = 4;
            // 
            // ServerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(359, 369);
            this.Controls.Add(this.BtnStartServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PortNumtxt);
            this.Controls.Add(this.CameraWindow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ServerIPcbo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ServerFrm";
            this.ShowIcon = false;
            this.Text = "WebCam Server";
            this.TransparencyKey = System.Drawing.Color.Lime;
            ((System.ComponentModel.ISupportInitialize)(this.CameraWindow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStartServer;
        private System.Windows.Forms.PictureBox CameraWindow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ServerIPcbo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PortNumtxt;
    }
}

