namespace Emboard
{
    partial class Retasking
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.cbbNodeRetaking = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkAllRetasking = new System.Windows.Forms.CheckBox();
            this.chbTemHum = new System.Windows.Forms.CheckBox();
            this.chbPhoto = new System.Windows.Forms.CheckBox();
            this.chbWarning = new System.Windows.Forms.CheckBox();
            this.btnSendRetask = new System.Windows.Forms.Button();
            this.sttRetasking = new System.Windows.Forms.Label();
            this.btnGetFuntion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbbNodeRetaking
            // 
            this.cbbNodeRetaking.Location = new System.Drawing.Point(60, 12);
            this.cbbNodeRetaking.Name = "cbbNodeRetaking";
            this.cbbNodeRetaking.Size = new System.Drawing.Size(100, 23);
            this.cbbNodeRetaking.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.Text = "Sensor:";
            // 
            // checkAllRetasking
            // 
            this.checkAllRetasking.Location = new System.Drawing.Point(60, 41);
            this.checkAllRetasking.Name = "checkAllRetasking";
            this.checkAllRetasking.Size = new System.Drawing.Size(100, 46);
            this.checkAllRetasking.TabIndex = 3;
            this.checkAllRetasking.Text = "Select All ";
            // 
            // chbTemHum
            // 
            this.chbTemHum.Location = new System.Drawing.Point(187, 15);
            this.chbTemHum.Name = "chbTemHum";
            this.chbTemHum.Size = new System.Drawing.Size(167, 20);
            this.chbTemHum.TabIndex = 5;
            this.chbTemHum.Text = "Temperature/Humidity";
            // 
            // chbPhoto
            // 
            this.chbPhoto.Location = new System.Drawing.Point(187, 41);
            this.chbPhoto.Name = "chbPhoto";
            this.chbPhoto.Size = new System.Drawing.Size(167, 20);
            this.chbPhoto.TabIndex = 6;
            this.chbPhoto.Text = "Take Photo";
            // 
            // chbWarning
            // 
            this.chbWarning.Location = new System.Drawing.Point(187, 67);
            this.chbWarning.Name = "chbWarning";
            this.chbWarning.Size = new System.Drawing.Size(157, 20);
            this.chbWarning.TabIndex = 7;
            this.chbWarning.Text = "Warning";
            // 
            // btnSendRetask
            // 
            this.btnSendRetask.Location = new System.Drawing.Point(276, 93);
            this.btnSendRetask.Name = "btnSendRetask";
            this.btnSendRetask.Size = new System.Drawing.Size(78, 27);
            this.btnSendRetask.TabIndex = 8;
            this.btnSendRetask.Text = "OK";
            this.btnSendRetask.Click += new System.EventHandler(this.btnSendRetask_Click);
            // 
            // sttRetasking
            // 
            this.sttRetasking.Location = new System.Drawing.Point(3, 100);
            this.sttRetasking.Name = "sttRetasking";
            this.sttRetasking.Size = new System.Drawing.Size(188, 20);
            this.sttRetasking.Text = "Retasking Sensors!";
            // 
            // btnGetFuntion
            // 
            this.btnGetFuntion.Location = new System.Drawing.Point(189, 93);
            this.btnGetFuntion.Name = "btnGetFuntion";
            this.btnGetFuntion.Size = new System.Drawing.Size(81, 27);
            this.btnGetFuntion.TabIndex = 10;
            this.btnGetFuntion.Text = "Get Funtion";
            this.btnGetFuntion.Click += new System.EventHandler(this.btnGetFuntion_Click);
            // 
            // Retasking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(367, 127);
            this.Controls.Add(this.btnGetFuntion);
            this.Controls.Add(this.sttRetasking);
            this.Controls.Add(this.btnSendRetask);
            this.Controls.Add(this.chbWarning);
            this.Controls.Add(this.chbPhoto);
            this.Controls.Add(this.chbTemHum);
            this.Controls.Add(this.checkAllRetasking);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbNodeRetaking);
            this.Name = "Retasking";
            this.Text = "Retasking";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbNodeRetaking;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkAllRetasking;
        private System.Windows.Forms.CheckBox chbTemHum;
        private System.Windows.Forms.CheckBox chbPhoto;
        private System.Windows.Forms.CheckBox chbWarning;
        private System.Windows.Forms.Button btnSendRetask;
        private System.Windows.Forms.Label sttRetasking;
        private System.Windows.Forms.Button btnGetFuntion;
    }
}