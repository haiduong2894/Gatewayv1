using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

namespace Emboard
{
    public partial class Retasking : Form
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
        /// Lay cac node dang hoat dong trong mang
        /// </summary>
        Database my_Database = new Database();

        string mac = null, ip = null;
        public string cmdReturn { set; get; }

        public Retasking()
        {
            InitializeComponent();
            try
            {
                cbbNodeRetaking.Items.Clear();
                //Hien thi danh sach sensor khu vuon lan
                XmlNodeList nodeSensor = ((XmlElement)my_Database.sensor).GetElementsByTagName("node");
                foreach (XmlNode node in nodeSensor)
                {
                    if (node.Attributes["status"].Value == "true" || node.Attributes["status"].Value == "True")
                    {
                        string str = "Sensor " + node.Attributes["mac"].Value;
                        cbbNodeRetaking.Items.Add(str);
                    }
                }
                //Hien thi danh sach cac sensor bao chay
                XmlNodeList nodeSensor_BC = ((XmlElement)my_Database.sensor_bc).GetElementsByTagName("node");
                foreach (XmlNode node_BC in nodeSensor_BC)
                {
                    if (node_BC.Attributes["status"].Value == "true" || node_BC.Attributes["status"].Value == "True")
                    {
                        string str = "Sensor " + node_BC.Attributes["mac"].Value;
                        cbbNodeRetaking.Items.Add(str);
                    }
                }
            }
            catch { MessageBox.Show("Select comboBox error:\r\n"); }
        }

        private void btnSendRetask_Click(object sender, EventArgs e)
        {
            ip = null;
            this.mac = null;
            int stt = (int)chbTemHum.CheckState + (int)chbPhoto.CheckState * 2 + (int)chbWarning.CheckState * 4;
            if (cbbNodeRetaking.SelectedIndex > -1)
            {
                try { this.mac = cbbNodeRetaking.Text.Substring(7, 2); }
                catch { }
                if (mac[0] > '2' && mac[0] < '6')
                {
                    ip = my_Database.getNetworkIpSensorBC(mac);
                }
                else
                {
                    ip = my_Database.getNetworkIpSensor(mac);
                }
                this.cmdReturn = ip + "5" + stt.ToString() + "0$";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else MessageBox.Show("Ban chua chon node");
        }
        public void waitsensor()
        {
            while (sensor.FunctionRetask == "00")
            {
                sttRetasking.Text = "Dang cap nhat trang thai...";
                System.Threading.Thread.Sleep(50);
            }
        }

        private void btnGetFuntion_Click(object sender, EventArgs e)
        {
            ip = null;
            this.mac = null;
            //int stt = (int)chbTemHum.CheckState + (int)chbPhoto.CheckState * 2 + (int)chbWarning.CheckState * 4;
            if (cbbNodeRetaking.SelectedIndex > -1) 
            {
                try { this.mac = cbbNodeRetaking.Text.Substring(7, 2); }
                catch { }
                if (mac[0] > '2' && mac[0] < '6')
                {
                    ip = my_Database.getNetworkIpSensorBC(mac);
                }
                else
                {
                    ip = my_Database.getNetworkIpSensor(mac);
                }
                this.cmdReturn = ip + "580$";
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else MessageBox.Show("Ban chua chon node");
        }
    }
}