using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System;

namespace Emboard
{
    public partial class Emboard : Form
    {
        const int MAX_CONNECTION = 10;
        public static string positionsendWeb;
        static TcpListener listener;
        private WebServer web_ = new WebServer();
        private ShowData showData = new ShowData();

       

        void DoWork()
        {
            while (true)
            {
                Socket soc = listener.AcceptSocket();
                ChangeTextBox("Welcome to Server\n");
                AppendTextBox("Connection received from: " + soc.RemoteEndPoint + "\n");
                try
                {
                    var stream = new NetworkStream(soc);
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    while (true)
                    {
                        string str = reader.ReadLine();
                        if (str.ToUpper() == "EXIT")
                        {
                            writer.WriteLine("bye");
                            break;
                        }
                        //positionsendWeb = "#PS:" + str;
                        //web_.sendDataToWeb(positionsendWeb);
                        //string mac = str.Substring(0, 2);
                        //string ip = str.Substring(2, 4);
                        //string lat = str.Substring(6, 9);
                        //string log = str.Substring(15, 10);
                        //AppendTextBox("Mac: " + mac + "\r\nIP: " + ip + "\r\nTọa độ: " + lat + "," + log + "\n"); //001234025.123456026.123456
                        switch (str[0])
                        {
                            case 'H':
                                dataWeather(str);
                                break;
                            case 'P':
                                dataPosition(str);
                                break;
                            default: 
                                break;
                        }
                    }
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }

                AppendTextBox("Client disconnected: " + soc.RemoteEndPoint);
                soc.Close();
            }
        }
        public void AppendTextBox(string value)
        {
            int position = 0;
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            lb_text.Text += value + "\r\n";
            position = lb_text.Text.IndexOf(value);
            lb_text.SelectionStart = position;
            lb_text.ScrollToCaret();
        }
        public void ChangeTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(ChangeTextBox), new object[] { value });
                return;
            }
            lb_status.Text = value + "\r\n";

        }

        private void btn_start_Click(object sender, System.EventArgs e)
        {
            try
            {
                text_port.Text = "9999";
                btn_start.Enabled = false;
                //  IPAddress address = IPAddress.Parse("192.168.0.109");
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress address = ipHostInfo.AddressList[0];
                Console.WriteLine("IP address:" + address);
                int PORT_NUMBER = int.Parse(text_port.Text);
                listener = new TcpListener(address, PORT_NUMBER);
                lb_status.Text = "IP Server: " + address;
                listener.Start();
                for (int i = 0; i < MAX_CONNECTION; i++)
                {
                    new Thread(DoWork).Start();
                }
            }
            catch { lb_status.Text = "Cannot connect to Server! "; }
        }

        public void dataWeather(String s)
        {
            string _dataWeather = "#" + s;
            web_.sendDataToWeb(_dataWeather);
            string mac = s.Substring(1, 2);
            string ip = s.Substring(3, 4);
            string temperature = s.Substring(7, 5);
            string humidity = s.Substring(12, 7);
            AppendTextBox("Mac: " + mac + "\r\nIP: " + ip + "\r\nTemperature: " + temperature +
                "\r\nHumidity: " + humidity);
        }

        public void dataPosition(String s)
        {
            positionsendWeb = "#" + s;
            web_.sendDataToWeb(positionsendWeb);
            string mac = s.Substring(1, 2);
            string ip = s.Substring(3, 4);
            string lat = s.Substring(7, 9);
            string log = s.Substring(16, 10);
            AppendTextBox("Mac: " + mac + "\r\nIP: " + ip + "\r\nPosition: " + lat + "," + log + "\n");
        }

        public void dataReport(String s)
        {
            string _dataWeather = "#" + s;
            web_.sendDataToWeb(_dataWeather);
            string mac = s.Substring(2, 2);
            string ip = s.Substring(4, 4);
            string temperature = s.Substring(8, 5);
            string humidity = s.Substring(13, 7);
            string lat = s.Substring(20, 9);
            string lng = s.Substring(29, 10);
            AppendTextBox("Mac: " + mac + "\r\nIP: " + ip + "\r\nTemperature: " + temperature +
                "\r\nHumidity: " + humidity + "\r\nPosition: " + lat + "," + lng + "\n");
        }
    }
}

