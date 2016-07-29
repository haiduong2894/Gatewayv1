﻿/********************************************************************************
 * Tên dự án        : Emboard
 * Nhóm thực hiện   : Sigate
 * Phòng nghiên cứu : Hệ nhúng nối mạng
 * Trường           : Đại Học Bách Khoa Hà Nội
 * Mô tả chung      : 1. Chương trình thu thập dữ liệu nhiệt độ, độ ẩm từ các sensor 
 *                    2. Ra quyết định điều khiển đến các actor phục vụ chăm sóc lan và cảnh báo cháy rừng
 *                    3. Chuyển tiếp dữ liệu về Web server để quản lý và theo dõi qua internet
 * IDE              : Microsoft Visual Studio 2008
 * Target Platform  : Window CE Device
 * *****************************************************************************/

#define ACTOR_BAOCHAY

using System;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Drawing;

namespace Emboard
{
    public partial class Emboard : Form
    {
        /// <summary>
        /// Tao doi tuong Process (lop process ke thua tu lop cong com)
        /// </summary>
        private Process comPort = new Process();

        /// <summary>
        /// Tao doi tuong sensor
        /// </summary>
        private Sensor sensor = new Sensor();

        /// <summary>
        /// Tao doi tuong actor
        /// </summary>
        private Actor actor = new Actor();

        /// <summary>
        /// Tao doi tuong webserver
        /// </summary>
        WebServer web = new WebServer();

        /// <summary>
        /// Su kien khi an nut connect tren giao dien
        /// Mo cac thread web va com
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        
        private void btConnect_Click(object sender, EventArgs e)
        {
            if (btConnect.Text == "Connect")
            {
                try
                {
                    comPort.openComPort();
                    //comPort.ThreadCOMGPS();
                    comPort.ThreadComPort();
                    comPort.ThreadCOMSMS();
                    comPort.ThreadReceiveFromWeb();
                    comPort.ThreadSendToWeb();
                    comPort.ThreadProcessData();
                    //

                    btSend.Enabled = true;
                    cbnode.Enabled = true;
                    cbMalenh.Enabled = true;
                    btConnect.Text = "Disconnect";
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString() + "loi");
                }
            }
            else 
            {
                try
                {
                    try
                    {
                        comPort.closeCOM();
                        comPort.closeCOMSMS();
                    }
                    catch { }
                    cbMalenh.Enabled = false;
                    cbnode.Enabled = false;
                    btSend.Enabled = false;
                    btConnect.Text = "Connect";
                }
                catch { }
            }
        }


        /// <summary>
        /// Su kien khi kich nut gui (send) lenh tren giao dien xuong actor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSend_Click(object sender, EventArgs e)
        {
            try
            {
                string cmdStr;
                Database myDatabase = new Database();
                int timenow = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                if (cbMalenh.SelectedIndex == 0 ||cbMalenh.SelectedIndex == 6)
                {
                    sensor.Mac = cbnode.Text.Substring(7, 2);
                    int now = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                    ImformationNode.timeDapUng.Remove(sensor.Mac);
                    ImformationNode.timeDapUng.Add(sensor.Mac,now);
                    if (sensor.Mac[0] > '2' && sensor.Mac[0] < '6')
                    {
                        sensor.Ip = myDatabase.getNetworkIpSensorBC(sensor.Mac);
                    }
                    else
                    {
                        sensor.Ip = myDatabase.getNetworkIpSensor(sensor.Mac);                        
                    }
                    if (cbMalenh.SelectedIndex == 0)
                    {
                        sensor.Command = sensor.Ip + "000$";
                        cmdStr = "): Gui lenh lay nhiet do, do am tu sensor (";
                    }
                    else
                    {
                        sensor.Command = sensor.Ip + "400$";
                        cmdStr = "): Gui lenh lay anh tu sensor (";
                    }
                    comPort.DisplayData("(" + comPort.showTime()+ cmdStr + sensor.Mac + "):\r\n Ma lenh : " + sensor.Command, tbShow);
                    if (sensor.Command.Length == 8)
                    {
                        cbMalenh.SelectedIndex = -1;
                        cbnode.Items.Clear();
                        cbnode.Text = "";
                        byte[] commandbyte = comPort.ConvertTobyte(sensor.Command);
                        comPort.writeByteData(commandbyte);
                    }
                }
                else
                {
                    actor.Ip = myDatabase.getNetworkIpActor(actor.Mac);
                    int now = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                    ImformationNode.timeDapUng.Remove(actor.Mac);
                    ImformationNode.timeDapUng.Add(actor.Mac, now);
                    int id = cbMalenh.SelectedIndex;
                    if (id < 8)
                    {
                        if (id == 7)
                        {
                            actor.Command = actor.commandOnActor(15, "0000");
                            comPort.DisplayData("(" + comPort.showTime() + "): Bat tat ca cac van:\r\n Ma lenh : " + actor.Command, tbShow);
                        }
                        else
                        {
                            actor.Command = actor.commandOnActor(id, "0000");
#if ACTOR_BAOCHAY
                            comPort.DisplayData("(" + comPort.showTime() + "): Gui canh bao muc " + id +":\r\n Ma lenh : " + actor.Command, tbShow);
                            
#else
                            comPort.DisplayData("(" + comPort.showTime() + "): Bat van so " + id + ":\r\n Ma lenh : " + actor.Command, tbShow);
#endif

                        }
                    }
                    else
                    {
                        int vanoff = id - 7;
                        if (vanoff== 7)
                        {
                            actor.Command = actor.commandOffActor(15, "0000");
                            comPort.DisplayData("(" + comPort.showTime() + "): Tat tat ca cac van:\r\n Ma lenh : " + actor.Command, tbShow);
                        }
                        else
                        {
                            actor.Command = actor.commandOffActor(vanoff, "0000");
                            comPort.DisplayData("(" + comPort.showTime() + "): Tat van so " + vanoff + ":\r\n Ma lenh : " + actor.Command, tbShow);

                        }

                    }
                    if (actor.Command.Length == 8)
                    {
                        cbMalenh.SelectedIndex = -1;
                        cbnode.Items.Clear();
                        cbnode.Text = "";
                        byte[] commandbyte = comPort.ConvertTobyte(actor.Command);
                        comPort.writeByteData(commandbyte);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ban chua chon du thong tin o Commnad hoac Node", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }


        /// <summary>
        /// Bat su kien lua chon combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMalenh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index_malenh = cbMalenh.SelectedIndex;
                if (index_malenh == 0||index_malenh == 6)
                {
                    cbnode.Items.Clear();
                    Database my_Database = new Database();
                    //Hien thi danh sach sensor khu vuon lan
                    XmlNodeList nodeSensor = ((XmlElement)my_Database.sensor).GetElementsByTagName("node");
                    foreach (XmlNode node in nodeSensor)
                    {
                        if (node.Attributes["status"].Value == "true" || node.Attributes["status"].Value == "True")
                        {
                            string str = "Sensor " + node.Attributes["mac"].Value;
                            cbnode.Items.Add(str);
                        }
                    }
                    if (index_malenh == 0)
                    {
                        XmlNodeList nodeSensor_BC = ((XmlElement)my_Database.sensor_bc).GetElementsByTagName("node");
                        foreach (XmlNode node_BC in nodeSensor_BC)
                        {
                            if (node_BC.Attributes["status"].Value == "true" || node_BC.Attributes["status"].Value == "True")
                            {
                                string str = "Sensor " + node_BC.Attributes["mac"].Value;
                                cbnode.Items.Add(str);
                            }
                        }
                    }
                }
                else
                {
                    cbnode.Items.Clear();
                    Database my_Database = new Database();
                    XmlNodeList nodeActor = ((XmlElement)my_Database.actor).GetElementsByTagName("node");
                    foreach (XmlNode node in nodeActor)
                    {
                        if (node.Attributes["status"].Value == "true" || node.Attributes["status"].Value == "True")
                        {

                            string str = "";
#if ACTOR_BAOCHAY
                            str = "Actor bao chay";
#else
                            str = "Actor bom tuoi";
#endif
                            cbnode.Items.Add(str);
                            actor.Mac = "00";
                            break;

                        }
                    }
                }
            }
            catch { comPort.DisplayData("Select comboBox error:\r\n", tbShow); }
        }

        /// <summary>
        /// Bat su kien khi click nut exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btexit_Click(object sender, EventArgs e)
        {
            try
            {
                comPort.comPort.Abort();
                web.threadReceiveFromWeb.Abort();
                web.threadSendToWeb.Abort();
                comPort.threadProcess.Abort();
                comPort.closeCOM();
                comPort.closeCOMSMS();
            }
            catch { }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Su kien khi dong phan mem o goc phai
        /// Dong tat ca cac thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Emboard_Closed(object sender, System.EventArgs e)
        {
            try
            {
                web.threadReceiveFromWeb.Abort();
                web.threadSendToWeb.Abort();
                comPort.comPort.Abort();
                comPort.threadProcess.Abort();
                comPort.clearDataCOM();
                comPort.closeCOM();
                comPort.closeCOMSMS();
            }
            catch { }
        }
        public byte[] stringToHex(string data)
        {
            byte[] output = new byte[data.Length >> 1];
            string s = null;
            for (int i = 0; i < output.Length; i++)
            {
                s = data.Substring(i << 1, 2);
                output[i] = byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
            }
            return output;
        }
        /// <summary>
        /// Demo gui du lieu anh len web
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSendToWeb_Click(object sender, System.EventArgs e)
        {

            try
            {
                Form2 f2 = new Form2();
                f2.ShowDialog();
               
            }
            catch (Exception ex) { MessageBox.Show("Cannot access: " + ex.Message); }
        }
        private void btTS_Click(object sender, System.EventArgs e)  //nut nay la nut view
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "File .jpeg|*.jpeg",
                    //Filter = "File Ảnh Nhị Phân(.mli)|*.mli",
                    //Title = "Chọn file",
                    //Multiselect = false
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    Bitmap bmp = new Bitmap(path);
                    pictureBox2.Image = (Image)bmp;
                }                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnC_Click(object sender, System.EventArgs e)
        {
            tbShow.Text = null;
        }
    }
}