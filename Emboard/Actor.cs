/********************************************************************************
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Emboard
{
    class Actor : ImformationNode
    {
        /// <summary>
        /// web phuc vu lay du lieu thoitiet tu yahoo
        /// </summary>
        private WebServer web = new WebServer();

        /// <summary>
        /// Trang thai actor
        /// </summary>
        private static bool statusActor;
        public bool StatusActor{
            set { statusActor = value; }
            get { return statusActor; }
        }

        private string ERR;
        /// <summary>
        /// Tao lenh bat van
        /// </summary>
        /// <param name="van"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string commandOnActor(int van, string ip)
        {
            try
            {
                string command = null;
                if (van == 15)
                    command = ip + van.ToString() + "1$";
                else
                    command = ip + "0" + van.ToString() + "1$";
                return command;
            }
            catch (Exception ex)
            {
                ERR = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Ham tao lenh tat van
        /// </summary>
        /// <param name="van"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string commandOffActor(int van, string ip)
        {
            try
            {
                string command = null;
                if (van == 15)
                    command = ip + van.ToString() + "0$";
                else
                    command = ip + "0" + van.ToString() + "0$";
                return command;
            }
            catch (Exception ex)
            {
                ERR = ex.Message;
                return null;
            }
        }

        public int GetLevelSendCanhBao(string dataReceived)///chua hoan thanh
        {
            try
            {
                int lv = 0;
                Database mydatabase = new Database();
                float nhietdotb = mydatabase.SumTemp();
                float doamtb = mydatabase.SumHumi();
                float level = doamtb / (float)20 - (float)(27 - nhietdotb) / (float)10;
                mydatabase.DeleteData();
                if (level > 4)
                    lv = 4;
                if ((2.5 < level) && (level < 4))
                    lv = 3;
                if ((2 < level) && (level < 2.5))
                    lv = 2;
                if (level < 2)
                    lv = 1;
                return lv;
                //DisplayData("Nguyen hai Duong", txtShowData);
                /*string[] url = connection.Confix();
                string uriCom = url[5];
                //MessageBox.Show("uri[5]: " + uriCom);
                string[] data_weather = null;
                float diff = 0;
                float level_web = 0;
                web.DataReceiveFromWeb = web.receiveDataFromWeb(uriCom);
                dataReceived = web.DataReceiveFromWeb;
                //MessageBox.Show("DataReceive from web: " + web.DataReceiveFromWeb);
                data_weather = web.DataReceiveFromWeb.Split(new char[] { ' ' });

                Database mydatabase = new Database();
                float nhietdotb = mydatabase.SumTemp();
                float doamtb = mydatabase.SumHumi();
                //MessageBox.Show("Nhiet do tb: " + nhietdotb.ToString() + "\tDo am tb:");
                
                float level = doamtb / (float)20 - (float)(27 - nhietdotb) / (float)10;
                //MessageBox.Show("\n\rLevel tinh duoc tu du lieu sensor:" + level);
                mydatabase.DeleteData();
                nhietdotb = float.Parse(data_weather[0]);
                doamtb = float.Parse(data_weather[1]);
                level_web = doamtb / (float)20 - (float)(27 - nhietdotb) / (float)10;
                //MessageBox.Show("\n\rLevel tinh duoc tu du lieu sensor:" + level_web);
                diff = Math.Abs(level - level_web);
                if (diff > 3 && level > 3 && level < 6)
                    return (int)level;
                else
                    return (int)level_web;*/
            }
            catch 
            {
                //MessageBox.Show("Weather error get:"+EX.Message);
                return 0;
            }
        }
    }
}
