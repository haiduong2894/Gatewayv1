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

#define ACTOR_BAOCHAY

using System;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Drawing;
using System.IO;

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
                    comPort.ThreadComPort();
                    //comPort.ThreadCOMSMS();
                    comPort.ThreadReceiveFromWeb();
                    comPort.ThreadSendToWeb();
                    comPort.ThreadProcessData();
                    ////////comPort.ThreadCOMGPS();

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
                Database myDatabase = new Database();
                int timenow = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                if (cbMalenh.SelectedIndex == 0 ||cbMalenh.SelectedIndex == 6)
                {
                    sensor.Mac = cbnode.Text.Substring(7, 2);
                    int now = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                    ImformationNode.timeDapUng.Remove(sensor.Mac);
                    ImformationNode.timeDapUng.Add(sensor.Mac, now);
                    if (sensor.Mac[0] > '2' && sensor.Mac[0] < '6') //20 --> 5F là sensor báo cháy
                    {
                        sensor.Ip = myDatabase.getNetworkIpSensorBC(sensor.Mac);
                    }
                    else
                    {
                        sensor.Ip = myDatabase.getNetworkIpSensor(sensor.Mac);    //01 --> 1F là sensỏ vườn lan                    
                    }
                    string cmdStr = null;
                    if (cbMalenh.SelectedIndex == 0)
                    {
                        sensor.Command = sensor.Ip + "000$";
                        cmdStr = "): Gui lenh lay nhiet do, do am tu sensor (";
                    }
                    else if (cbMalenh.SelectedIndex == 7)
                    {
                        sensor.Command = sensor.Ip + "010$";
                        cmdStr = "): Gui lenh reset sensor (";
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
                            comPort.DisplayData("(" + comPort.showTime() + "): Gui canh bao muc " + id + ":\r\n Ma lenh : " + actor.Command, tbShow);

#else
                            comPort.DisplayData("(" + comPort.showTime() + "): Bat van so " + id + ":\r\n Ma lenh : " + actor.Command, tbShow);
#endif

                        }
                    }
                    else
                    {
                        int vanoff = id - 7;
                        if (vanoff == 7)
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
                        //cbMalenh.SelectedIndex = -1;
                        //cbnode.Items.Clear();
                        //cbnode.Text = "";
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

        public byte[] stringToHexH(string data)
        {
            byte[] output = new byte[data.Length >> 1];
            string s = null;
            for (int i = 0; i < output.Length; i++)
            {
                try
                {
                    s = data.Substring(i << 1, 2);
                    output[i] = byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                }
                catch { output[i] = 0; }
            }
            return output;
        }

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
                System.Windows.Forms.MessageBox.Show("Exception caught in process:" + _Exception.ToString());
            }
            return false;
        }
        private void btTS_Click(object sender, System.EventArgs e)  //nut nay la nut view
        {
            try
            {
                //OpenFileDialog ofd = new OpenFileDialog()
                //{
                //    Filter = "File .jpeg|*.jpeg",
                //    //Filter = "File Ảnh Nhị Phân(.mli)|*.mli",
                //    //Title = "Chọn file",
                //    //Multiselect = false
                //};
                //if (ofd.ShowDialog() == DialogResult.OK)
                //{
                //    string path = ofd.FileName;
                //    Bitmap bmp = new Bitmap(path);
                //    pictureBox2.Image = (Image)bmp;
                //}   
                string data = "ffd8fffe002420067f170000000000000000000000000000007800a0001b0032120b510451040000ffdb00840007050506050407060506080707080a110b0a09090a150f100c1118161a19181618171b1e27211b1d251d1718222e222528292b2c2b1a2030332f2a33272b2b2a010708080a090a140b0b142a1c181c2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2affffa2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2afffe0005000000ffc0001108007800a003012100021101031101ffc401a200000105010ffffffffffffffffffffffffffffffffffffffffff01010101010000000000000102030405060708090a0b100002010303020403050504040000ffffffffffffffffffffffffffffffffffffffffffffff18191a25262728292a3435363738393a434445464748494a535455565758595a636465666768696a737475767778797a838485868788898a92939495969798999aa2a3a4a5a6a7a8a9ffffffffff6b7b8b9bac2c3c4c5c6c7c8c9cad2d3d4d5d6d7d8d9dae1e2e3e4e5e6e7e8e9eaf1f2f3f4f5f6f7f8f9fa110002010204040304070504040001027700010203110405213106124151076171132ffffffff4291a1b1c109233352f0156272d10a162434e125f11718191a262728292a35363738393a434445464748494a535455565758595a636465666768696a737475767778797a8283ffffffffffff5969798999aa2a3a4a5a6a7a8a9aab2b3b4b5b6b7b8b9bac2c3c4c5c6c7c8c9cad2d3d4d5d6d7d8d9dae2e3e4e5e6e7e8e9eaf2f3f4f5f6f7f8f9faffdd0004000affda000c03010002110311003f00209412055c53c54198f071448159083d28b810a44046557914c589157628e0f6a2e52456ba8c2c78518159b32ef85d7d41a451c4ed29295f438ab6ad9b6da7aab71f8d6e999d85535347c1e69817617c2715a4872a2a4cc90f149bb9c1a561a207985bb024f06a2cff00a48712000ff0d26521f380c87d6b2d8609a4338dd423f2750957fdacd119cf1ea2b7440f5ebcd4ca698993a3735b9e1dd321d4755f3a6b3ef4e6a2e6762ceec9eb4668b948ad3c22495598f0b4db882390ab7231e869143263b4a61b83c55770371a43397d7e1db76af8e1856721c63dab68bd0864dd0f4a9179aad908955856cffffffff8c6659022365493d07a5296a868f4eb4052203f90ab6327b572944aa0d4891176006493e82801d3c96f643fd3eeeded7dae2658c9fa024134c8af2ceec62c5aeb51c76b1b4771ff7d36d5fd6b58c1bd486cfffd2a77432bc75a6d9ca01c1a833468ab822941f4a458cb80cd110a707d6a18774506263bb1cd218ab3453c7b82f00f7a8a60036474a00c1f10c45ad5641ced6e7f1ae794e0d6b0d84c9c1ca834f0462aec48f538353c5863b77633dfd280d8f478fc63a2db451c06f7ed338500adbc6d2127f0156e0d7f51be3ff0012df0fdd3ae705eee45b703f3c9fd2b9f95752f725964f1195cc971a7590cffcb089a771f8b617f4a6dce9b23441b51d6b519d5bef279c218cff00c0500147325f0a0b77398bc5b2d3aed0e9f1c71b2b6776031cfd4e6bb4bbf8c1a8b4456c34db7878c02c4b91ee3a0fd2b58deda90f47a1ffd3ab28dc98aab1288e4c935999a2fa3e4715286e28290fe18533d38cd2288fccdfe644b1e303838eb4cf24a42bbb93fca8033b52844d61321ebb491f5ae3811c1ad21b0993a1ca91f8d381cd6972476ece4d4919e69580f49f87de17ff008496ce5712a0304814c649c907bfd3fc0d7a1e93e0eb7d1b5bb249774e8e64462ca02ee0a19703f3fc45428d9dc1cb4b1deaa2a851d768c0279354ed248acb4c3e7bac715b974258e02aab103f4c550ed63cd3c63e34fed8956cec109b68db3961cb9e99f6ef5daf80e5593c290aa9cf96eca7f3cfe3d68e82fb47ffd5a7b8d53bb8f71c9acccc96d0855c035714f348b43c0fe9dabc097b324518b5bb830a5a5009656c741cf73d715ea8f713be9f6f797312a324b138ff0064310adf90634897b9a17775058da49737522c50c6373331e00af17f15f8b67d72fa682cdde3b3770c686d2ed6e3cf77950e726a2fa9563aff000ff8cb52d2353b793ed329b657cbc0ac42b023078f5c7f2af71b0d5b52d77c37345a4e9b3c91b452c426964d9180412a4679247cb8e2ae3aa339ad4f37f1cfc50b9d7963b58c2476f08e446e4abb7a9cf5fe95ce697af5bbbed697cb91bb31ebf43d2a26aeb42e2fb9ad7658c2ee650be83a9fcebb8f8257e5b55d4ecdc86678164041e9b5b1ff00b3514ba84dea99f14ee846ac3e31b3b1b5fb3da5acf70beb712e3f95469f10753b627fb3a3b6b104609853923df3d6a761b773ffd0c756e29e0e78ac8c8ab36237dcd53da5c79bc0a0a45d53c54a84d05092da4734a249064af4156202767cdd8f6a062b891e742afb50751eb50dd7cbcd00715e2db629730dd28e186d623d7b57399e79381dcd2115a4d4edd38895e53ea7e5155a5d56e4afc816307d2a926c573a9d39ffffff99c9651935624b77bab69ed23728eeb98db38e7a8fd7f43576219c2bb3872242db81c1cf5069be61ec6810798dea68de7d6803fffd1c356a7eee6b33221ba505338a2c1b031800503468a9ab094863a6cf92c178245416509b640a5d9b71c9c9a0a2dbb009bb27e5e6abc8c67b50e4152c338a068c9d4ed45fe992c447cc572b9fef0e95c11849565ef82292b099cee39fa0a6b37cb8cf4ad34333a7f0c5c9938a4e407ffd2e7d5bdaa4073591904a7295520ded375c2e6819af17415650d031ecd95aa26e98dea4223600725fb5034d17c30298eb91512b970ca5700703de8194a5cac8463e515c66b307d93577551f2bfcebf8ff9352b70671922959d907382578a9edf4eba9e42440e17fda181fad697d0ccdbd174c9ec2e5e495d36b2e3683ce735bc1c34657b1e0d313d489905e5a985f779b10210e7bf1fcc7ea6b276f1d6a25a151d852169381d0546a07fffd3e683639a9158e6b2311f9c8c75aa4e4acc39c0a761dcd6b76cc62ad29e2818fcd50d4669542c56e3e790e377a0a0772ddbb6d854139238269649156e10160090700f7a417209c739ae77c496c65b58ee107311c37d0fff005e92dca7b1cfc288a72a8013d702a790b8858c401703e507bd686455b16d44dc97ba2045b7ee9c6735a8ac31f5a2c17d437f9572ae380ff2fb83d8d53bd8fcb9f28308e32bedea3f3a52d816e572714dc8c6456451ffd4e5b34f4639accc497ccc6324d433a8cef3da802dd94eae985e82af06a063b754537ddcfa5031226543e5ab64e3352c8502995c0f907534015e46575460d856edeb55a58d6786489c7cb229153d4ae870f7523d9170c8ccc8db70296cee259e12f347e5f3f283e9577d48e84462bd6be57f3b6c40e70be9e98ad10fd413de9a25b1482f1951d7b1a6c80dcda13fc43e603dfb8ff3e943d50ba99c49ff00269c07ca402722b1b9a1ffd5e5010d91f874a553f9565a99325079a271ba3e2a912c4b42211826b455f239a4343c3734e66f97a6681946d3e499e6b8216491b0a3daae4c1648195ba11cd004180d1281fc34c6ff0056a454bdcae8735af44135012ae312ae7f11c7f856703eb56999bdc7e7bd2823ad310f56ff0038a237f2e620606ef9978efde8029dcc6b1ce401f29f9978ed4c0323b7e558b3448fffd9";
                //string path = @"\Storage Card\Sigate\Image\ImageSensor02" + DateTime.Now.ToString("hhmmss") + ".jpeg";
                string path = @"C:\Users\Dương\Documents\Visual Studio 2008\Projects\Gateway-master2\Gateway-master\Gatewayv1\Gatewayv1\Emboard\bin\Debug\demo" + DateTime.Now.ToString("hhmm") + ".jpg";
                byte[] byteArr = stringToHexH(data);
                ByteArrayToFile(path, byteArr);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnC_Click(object sender, System.EventArgs e)
        {
            tbShow.Text = null;
            comPort.COMPort.Write("r");
        }
    }
}