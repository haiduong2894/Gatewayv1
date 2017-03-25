using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emboard
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnNode1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "File .jpeg|*.jpeg",
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tbNode1.Text = ofd.FileName;
                    Bitmap bmp = new Bitmap(tbNode1.Text);
                    pbNode1.Image = (Image)bmp;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnNode2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "File .jpeg|*.jpeg",
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tbNode2.Text = ofd.FileName;
                    Bitmap bmp = new Bitmap(tbNode2.Text);
                    pbNode2.Image = (Image)bmp;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                int hour1 = int.Parse(tbNode1.Text.Substring(42, 2));
                int min1 = int.Parse(tbNode1.Text.Substring(44, 2));
                int sec1 = int.Parse(tbNode1.Text.Substring(46, 2));

                int hour2 = int.Parse(tbNode2.Text.Substring(42, 2));
                int min2 = int.Parse(tbNode2.Text.Substring(44, 2));
                int sec2 = int.Parse(tbNode2.Text.Substring(46, 2));
                float t = (hour2 - hour1) * 3600 + (min2 - min1) * 60 + (sec2 - sec1);
                int d;
                try
                {
                    string[] url = connection.Confix();
                    d = int.Parse(url[6]);
                }
                catch (Exception ex) { MessageBox.Show("Khong lay duoc khoang cach!"+ex.Message); d = 22; }
                
                float v = d / t;
                MessageBox.Show("Van toc di chuyen: " + v.ToString()+"m/s");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the tinh toan!"+ex.Message);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pbNode1.Image;
            Sdk.ToGray(bmp);
        }
    }
}