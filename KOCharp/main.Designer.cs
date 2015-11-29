namespace KOCharp
{
    partial class main
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
            this.txtGameserverPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtActiveLoginPort = new System.Windows.Forms.TextBox();
            this.btnLoginServer = new System.Windows.Forms.Button();
            this.btnGameServer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ProgressList = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGameserverPort
            // 
            this.txtGameserverPort.Enabled = false;
            this.txtGameserverPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtGameserverPort.Location = new System.Drawing.Point(727, 12);
            this.txtGameserverPort.Multiline = true;
            this.txtGameserverPort.Name = "txtGameserverPort";
            this.txtGameserverPort.Size = new System.Drawing.Size(100, 26);
            this.txtGameserverPort.TabIndex = 0;
            this.txtGameserverPort.Text = "15001";
            this.txtGameserverPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(595, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game Server Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(595, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Login Server Active Ports";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtActiveLoginPort
            // 
            this.txtActiveLoginPort.Enabled = false;
            this.txtActiveLoginPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtActiveLoginPort.Location = new System.Drawing.Point(727, 41);
            this.txtActiveLoginPort.Multiline = true;
            this.txtActiveLoginPort.Name = "txtActiveLoginPort";
            this.txtActiveLoginPort.Size = new System.Drawing.Size(100, 26);
            this.txtActiveLoginPort.TabIndex = 2;
            this.txtActiveLoginPort.Text = "0";
            this.txtActiveLoginPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLoginServer
            // 
            this.btnLoginServer.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLoginServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoginServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLoginServer.ForeColor = System.Drawing.SystemColors.Control;
            this.btnLoginServer.Location = new System.Drawing.Point(595, 70);
            this.btnLoginServer.Name = "btnLoginServer";
            this.btnLoginServer.Size = new System.Drawing.Size(232, 29);
            this.btnLoginServer.TabIndex = 4;
            this.btnLoginServer.Text = "Login Server Başlat";
            this.btnLoginServer.UseVisualStyleBackColor = false;
            this.btnLoginServer.Click += new System.EventHandler(this.btnLoginServer_Click);
            // 
            // btnGameServer
            // 
            this.btnGameServer.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGameServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGameServer.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGameServer.Location = new System.Drawing.Point(595, 105);
            this.btnGameServer.Name = "btnGameServer";
            this.btnGameServer.Size = new System.Drawing.Size(232, 29);
            this.btnGameServer.TabIndex = 5;
            this.btnGameServer.Text = "GameServer Başlat";
            this.btnGameServer.UseVisualStyleBackColor = false;
            this.btnGameServer.Click += new System.EventHandler(this.btnGameServer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.ProgressList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 313);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "İşlemler ";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox1.Location = new System.Drawing.Point(6, 276);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(449, 26);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProgressList
            // 
            this.ProgressList.FormattingEnabled = true;
            this.ProgressList.Location = new System.Drawing.Point(6, 19);
            this.ProgressList.Name = "ProgressList";
            this.ProgressList.Size = new System.Drawing.Size(539, 251);
            this.ProgressList.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(473, 288);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 26);
            this.button1.TabIndex = 7;
            this.button1.Text = "Gönder";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(598, 140);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Oto Kayıt";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(835, 337);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGameServer);
            this.Controls.Add(this.btnLoginServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtActiveLoginPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGameserverPort);
            this.Name = "main";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "KnightOnline";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGameserverPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtActiveLoginPort;
        private System.Windows.Forms.Button btnLoginServer;
        private System.Windows.Forms.Button btnGameServer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.ListBox ProgressList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

