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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.IO;
namespace Emboard
{
    class ConvertData
    {
        /// <summary>
        /// Tao doi tuong sensor
        /// </summary>
        private Sensor sensor = new Sensor();

        /// <summary>
        /// Tao doi tuong actor
        /// </summary>
        private Actor actor = new Actor();

        /// <summary>
        /// Tao doi tuong van
        /// </summary>
        private Van van = new Van();

        /// <summary>
        /// Chuoi loi tra ve
        /// </summary>
        private string ERR = null;

        /// <summary>
        /// Tao doi tuong co so du lieu
        /// </summary>
        private Database db;

        /// <summary>
        /// Kiem tra la sensor (true) hay actor (false)
        /// </summary>
        public bool checkSensor = false;

        /// <summary>
        /// Boc tach du lieu cua ban tin gia nhap mang
        /// </summary>
        /// <param name="data"></param>
        public void convertDataJoinNetwork(string data)
        {
            try
            {
                string mac = null;
                string ip = null;
                db = new Database();
                mac = data.Substring(8, 2);
                int mac_int = int.Parse(mac, System.Globalization.NumberStyles.HexNumber);
                ip = data.Substring(4, 4);
                if (mac_int == 0 || mac_int == 177)
                {
                    checkSensor = false;
                    actor.Mac = mac;
                    actor.Ip = ip;
                    if (db.CheckActor(mac) == "true")
                    {
                        db.setNetworkIpActor(mac, ip);
                        db.setStatusActor(mac, true);
                    }
                    else
                    {
                        db.setNodeActor(mac, ip, true);
                    } 
                }
                else if ((mac[0] > '2') && mac[0]<'6')
                {
                    checkSensor = true;
                    sensor.Mac = mac;
                    sensor.Ip = ip;
                    if (db.CheckSensorBC(mac) == "true")
                    {
                        db.setNetworkIpSensorBC(sensor.Mac, sensor.Ip);
                        db.setStatusSensorBC(sensor.Mac, true);
                    }
                    else
                    {
                        db.setSensor_bc(sensor.Mac, sensor.Ip, true);
                    }
                }
                else
                {
                    checkSensor = true;
                    sensor.Mac = mac;
                    sensor.Ip = ip;
                    if (db.CheckSensor(mac) == "true")
                    {
                        db.setNetworkIpSensor(sensor.Mac, sensor.Ip);
                        db.setActiveSensor(sensor.Mac, true);
                    }
                    else
                    {
                        db.setNodeSensor(sensor.Mac, sensor.Ip, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ERR = ex.Message;
            }
        }

        /// <summary>
        /// Boc tachs ban tin du lieu lay nhiet do, do am va nang luong
        /// </summary>
        /// <param name="data"></param>
        public void convertDataSensor(string data)
        {
            try
            {
                float humi = 0;
                float temp = 0;
                float ener = 0;
                sensor.Mac = data.Substring(8, 2);
                sensor.Ip = data.Substring(4, 4);
                string hexd = data.Substring(10, 4);
                int td = int.Parse(hexd, System.Globalization.NumberStyles.HexNumber);
                hexd = data.Substring(14, 4);
                int rhd = int.Parse(hexd, System.Globalization.NumberStyles.HexNumber);
                hexd = data.Substring(18, 4);
                int rpd = int.Parse(hexd, System.Globalization.NumberStyles.HexNumber);
                float rpd1 = ((float)rpd / (float)4096);
                float rh_lind;// rh_lin:  Humidity linear 
                temp = (float)(td * 0.01 - 39.6);                  				//calc. temperature from ticks to [deg Cel]	
                rh_lind = (float)(0.0367 * rhd - 0.0000015955 * rhd * rhd - 2.0468);     	//calc. humidity from ticks to [%RH]
                humi = (float)((temp - 25) * (0.01 + 0.00008 * rhd) + rh_lind);   		//calc. temperature compensated humidity [%RH]
                ener = (float)(0.78 / rpd1);                                 //calc. power of zigbee
                if (humi > 100) humi = 100;       				//cut if the value is outside of
                if (humi < 0.1) humi = 0;

                sensor.Temperature = temp;
                sensor.Humidity = humi;
                sensor.Energy = ener;
            }
            catch (Exception ex)
            {
                ERR = ex.Message;
            }
        }

        /// <summary>
        /// Boc tach thong tin phan hoi tu actor
        /// </summary>
        /// <param name="data"></param>
        public void convertImformationActor(string data)
        {
            try
            {
                db = new Database();
                actor.Mac = data.Substring(8, 2);
                van.VanID = int.Parse(data.Substring(11, 1), System.Globalization.NumberStyles.HexNumber);
                int check = int.Parse(data.Substring(10, 1));
                if (check == 8)
                {
                    actor.StatusActor = true;
                    if(van.VanID == 15)
                    {
                        db.setValOn();
                    }
                    else
                    {
                        db.setStateVal(van.VanID, "on");
                    }
                }
                else
                {
                    actor.StatusActor = false;
                    if(van.VanID == 15)
                    {
                        db.setValOff();
                    }
                    else
                    {
                        db.setStateVal(van.VanID,"off");
                    }
                }
            }
            catch (Exception ex)
            {
                ERR = ex.Message;
            }
        }

        /// <summary>
        /// Boc tach trang thai node sensor
        /// </summary>
        /// <param name="data"></param>
        public void convertStateNode(string data)
        {
            try
            {
                sensor.Mac = data.Substring(8, 2);
                sensor.StateSensor = data.Substring(10, 2);
            }
            catch (Exception ex)
            { ERR = ex.Message; }
        }
        /// <summary>
        /// luu anh tu mang byte
        /// </summary>
        /// <param name="_FileName"></param>
        /// <param name="_ByteArray"></param>
        /// <returns></returns>
        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);
                // close file stream
                _FileStream.Close();
                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                System.Windows.Forms.MessageBox.Show("Exception caught in process:"+ _Exception.ToString());
            }
            // error occured, return false
            return false;
        }
        /// <summary>
        /// Boc tach ban tin thuc ngu
        /// </summary>
        /// <param name="data"></param>
        public void convertImformationSleep(string data)
        {
            try
            {
                db = new Database();
                db.setActiveSensor(data.Substring(4, 2), false);
                sensor.Mac = data.Substring(4,2);
            }
            catch (Exception ex)
            {
                ERR = ex.ToString();
            }
        }
       
        /// <summary>
        /// ghep du lieu anh trong 1 mang
        /// </summary>
        /// <param name="stringIn"></param>
        /// <returns></returns>
        public string addStringImage(int mac, string[,] strImg)
        {
            string strOut = null;
            //int mac = int.Parse(Mac, System.Globalization.NumberStyles.HexNumber);
            try
            {
                for( int i = 0; i<strImg.GetLength(0);i++){
                    strOut += strImg[mac,i];
                }
            }
            catch { MessageBox.Show("Cannot mux image data"); }
            return strOut;
            //strImg[mac] = strImg[mac] + stringIn;
        }
        /// <summary>
        /// convert from string to hex array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
        /// Su ly du lieu anh gui ve tu router
        /// </summary>
        /// <param111 name="data"></param>
        public void convertDataPicture(string[,] StringImage, string data) //data: #RI:FFFF D0D1D2D3 D4D5D6D7...
        {
            try
            {
                ///#RC:000132000F0012345678  #RC:IP Mac Dodai STT DATA
                const int DataLen = 80;
                int[] lengthImage = new int[100];
                string dodai, STT, temp;
                sensor.Ip = data.Substring(4, 4);
                sensor.Mac = data.Substring(8, 2);
                dodai = data.Substring(10, 4);
                STT = data.Substring(14, 2);
                int mac = int.Parse(sensor.Mac, System.Globalization.NumberStyles.HexNumber);
                lengthImage[mac] = int.Parse(dodai, System.Globalization.NumberStyles.HexNumber);
                int i = int.Parse(STT, System.Globalization.NumberStyles.HexNumber);
                //if (i == 0) { StringImage[mac] = null; }
                temp = data.Substring(16, data.Length - 17);  //Bo 16 ky tu dau va 1 ky tu \n o cuoi data
                sensor.ArrayStringImage[mac, i] = temp;
                if (i >= (lengthImage[mac] / DataLen))
                {
                    try
                    {
                        sensor.Img_path = @"\Storage Card\Sigate\Image\ImageSensor"+sensor.Mac.ToString()+DateTime.Now.ToString("hhmmss")+".jpeg";
                        temp = addStringImage(mac, sensor.ArrayStringImage);
                        byte[] byteArr = stringToHex(temp);
                        ByteArrayToFile(sensor.Img_path, byteArr);
                        //Stream strm = new MemoryStream(byteArr);
                        
                        //temp = "#RC:"+ sensor.Ip + sensor.Mac + StringImage[mac];
                        MessageBox.Show("Take photo sucessfull!!! do dai: "+lengthImage[mac].ToString());
                        sensor.TakePhotoDone = true;                      
                    }
                    catch (Exception ex) { MessageBox.Show("Save file error" + ex.ToString()); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error convertDataPicture:\r" + ex.Message);
            }
        }
    }
}
