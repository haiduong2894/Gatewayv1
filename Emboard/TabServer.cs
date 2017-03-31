using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace Emboard
{
    public class JSON {
        private string jsInput = null;
        DateTime time = DateTime.Now();
        private string[] cutData;
        private string[,] data;
        public string[,] Data
        {
            get{return data;}
            set{data = value;}
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jsInput"></param>
        JSON(string jsInput)
        {
            this.jsInput = jsInput;
        }
        
        private void getData()
        {
            cutData = jsInput.Split(new Char[] { ';' });
        }
    }

    public partial class Emboard : Form
    {
        private void btnConnectSql_Click(object sender, System.EventArgs e)
        {
            Database positionDatabase = new Database();
            string[] path = connection.Confix(); //path[6] de nhan du lieu vi tri cac sensor
            string urlObject = path[6] + "?table=object";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlObject);
            request.Method = "GET";
            // Get response for http web request
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());
            // Read web response into string
            string webResponseStream = responseStream.ReadToEnd();
            MessageBox.Show(webResponseStream);

            //close webresponse
            webResponse.Close();
            responseStream.Close();

        }

        private void btnInter_Click(object sender, System.EventArgs e)
        {
            //Back interplationWorker = new Thread();
        }   
    }
}

