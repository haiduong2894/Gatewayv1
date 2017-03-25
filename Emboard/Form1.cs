using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Emboard
{
    public partial class Emboard : Form
    {
        public Emboard()
        {
            InitializeComponent();
            btSend.Enabled = false;
            cbMalenh.Enabled = false;
            cbnode.Enabled = false;
            btexit.Enabled = false;
            pnGeneral.Visible = true;
            pnNode.Visible = false;
            pnGeneral.Location = new Point(0, 0);
            panel2.Visible = true;
            panel2.Controls.Add(pnGeneral);
        }

        public string[] messageList;
        public string strmsg;

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(sensor.Img_path);
                pictureBox2.Image = (Image)bmp;
            }
            catch { }
        }

        
    }
}