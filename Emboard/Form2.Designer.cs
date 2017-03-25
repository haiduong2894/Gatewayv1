namespace Emboard
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.pbNode1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbNode2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNode1 = new System.Windows.Forms.Button();
            this.btnNode2 = new System.Windows.Forms.Button();
            this.tbNode1 = new System.Windows.Forms.TextBox();
            this.tbNode2 = new System.Windows.Forms.TextBox();
            this.btnCal = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pbNode1
            // 
            this.pbNode1.Image = ((System.Drawing.Image)(resources.GetObject("pbNode1.Image")));
            this.pbNode1.Location = new System.Drawing.Point(3, 3);
            this.pbNode1.Name = "pbNode1";
            this.pbNode1.Size = new System.Drawing.Size(205, 131);
            this.pbNode1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Node 1";
            // 
            // pbNode2
            // 
            this.pbNode2.Image = ((System.Drawing.Image)(resources.GetObject("pbNode2.Image")));
            this.pbNode2.Location = new System.Drawing.Point(214, 3);
            this.pbNode2.Name = "pbNode2";
            this.pbNode2.Size = new System.Drawing.Size(203, 131);
            this.pbNode2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(226, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 23);
            this.label2.Text = "Node 2";
            // 
            // btnNode1
            // 
            this.btnNode1.Location = new System.Drawing.Point(3, 160);
            this.btnNode1.Name = "btnNode1";
            this.btnNode1.Size = new System.Drawing.Size(72, 20);
            this.btnNode1.TabIndex = 4;
            this.btnNode1.Text = "Image 1";
            this.btnNode1.Click += new System.EventHandler(this.btnNode1_Click);
            // 
            // btnNode2
            // 
            this.btnNode2.Location = new System.Drawing.Point(356, 163);
            this.btnNode2.Name = "btnNode2";
            this.btnNode2.Size = new System.Drawing.Size(61, 20);
            this.btnNode2.TabIndex = 5;
            this.btnNode2.Text = "Image 2";
            this.btnNode2.Click += new System.EventHandler(this.btnNode2_Click);
            // 
            // tbNode1
            // 
            this.tbNode1.Location = new System.Drawing.Point(62, 134);
            this.tbNode1.Name = "tbNode1";
            this.tbNode1.Size = new System.Drawing.Size(146, 23);
            this.tbNode1.TabIndex = 6;
            // 
            // tbNode2
            // 
            this.tbNode2.Location = new System.Drawing.Point(282, 134);
            this.tbNode2.Name = "tbNode2";
            this.tbNode2.Size = new System.Drawing.Size(136, 23);
            this.tbNode2.TabIndex = 7;
            // 
            // btnCal
            // 
            this.btnCal.Location = new System.Drawing.Point(104, 163);
            this.btnCal.Name = "btnCal";
            this.btnCal.Size = new System.Drawing.Size(123, 20);
            this.btnCal.TabIndex = 8;
            this.btnCal.Text = "Calculate";
            this.btnCal.Click += new System.EventHandler(this.btnCal_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(262, 163);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(72, 20);
            this.btnTest.TabIndex = 13;
            this.btnTest.Text = "Test";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(423, 190);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCal);
            this.Controls.Add(this.tbNode2);
            this.Controls.Add(this.tbNode1);
            this.Controls.Add(this.btnNode2);
            this.Controls.Add(this.btnNode1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbNode2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbNode1);
            this.Menu = this.mainMenu1;
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbNode1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbNode2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnNode1;
        private System.Windows.Forms.Button btnNode2;
        private System.Windows.Forms.TextBox tbNode1;
        private System.Windows.Forms.TextBox tbNode2;
        private System.Windows.Forms.Button btnCal;
        private System.Windows.Forms.Button btnTest;
    }
}