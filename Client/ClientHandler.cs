
using System;
using System.Windows.Forms;
using Webcam_Server;
using System.Net.Sockets;

namespace ClientHandle
{
    public class ClientHandler
    {
        private const int LF = 10;
        private string clientIP { get; set; }
        private byte[] Data { get; set; }
        private string PartialStr { get; set; }
        ServerFrm Main { get; set; }
        TcpClient Client { get; set; }
 
        public ClientHandler(TcpClient client, ServerFrm main)
        {
            Main = main;
            Client = client;
            clientIP = client.Client.RemoteEndPoint.ToString();
            //start reading data from the client in a separate thread
            Data = new byte[client.ReceiveBufferSize];
            client.GetStream().BeginRead(Data, 0, System.Convert.ToInt32(client.ReceiveBufferSize), ReceiveMessage, null);
        }

        public void SendData(byte[] data)
        {
            try
            {
                NetworkStream ns = null;
                lock (Client.GetStream())
                {
                    ns = Client.GetStream();
                    ns.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ReceiveMessage(IAsyncResult ar)
        {
            int bytesRead = 0;
            try
            {
                lock (Client.GetStream())
                {
                    bytesRead = Client.GetStream().EndRead(ar);
                }
                //client has disconnected
                if (bytesRead < 1)
                {
                    return;
                }
                else
                {
                    string messageReceived = null;
                    int i = 0;
                    int start = 0;
                    while (Data[i] != 0)
                    {
                        if (i + 1 > bytesRead)
                        {
                            break;
                        }

                        if (Data[i] == LF)
                        {
                            messageReceived = PartialStr + System.Text.Encoding.ASCII.GetString(Data, start, i - start);

                            if (messageReceived.StartsWith("Send"))
                            {
                                //send the current bitmap via the memory stream
                                if (Main.ms != null)
                                {
                                    SendData(Main.ms.GetBuffer());
                                }
                            }
                            start = i + 1;
                        }
                        i += 1;
                    }
                    //if partial string received
                    if (start != i)
                    {
                        PartialStr = System.Text.Encoding.ASCII.GetString(Data, start, i - start);
                        MessageBox.Show(PartialStr);
                    }
                }

                lock (Client.GetStream())
                {
                    Client.GetStream().BeginRead(Data, 0, System.Convert.ToInt32(Client.ReceiveBufferSize), ReceiveMessage, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
