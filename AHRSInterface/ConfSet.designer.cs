namespace AHRSInterface
{
    partial class ConfSet
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
            this.confsetAlignmentCommitButton = new System.Windows.Forms.Button();
            this.flashCommitButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.output_enable = new System.Windows.Forms.TextBox();
            this.output_rate = new System.Windows.Forms.TextBox();
            this.badurate = new System.Windows.Forms.TextBox();
            this.ip = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.rebootButton = new System.Windows.Forms.Button();
            this.ResetEkFbutton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // confsetAlignmentCommitButton
            // 
            this.confsetAlignmentCommitButton.Location = new System.Drawing.Point(14, 200);
            this.confsetAlignmentCommitButton.Name = "confsetAlignmentCommitButton";
            this.confsetAlignmentCommitButton.Size = new System.Drawing.Size(86, 23);
            this.confsetAlignmentCommitButton.TabIndex = 2;
            this.confsetAlignmentCommitButton.Text = "RAM Commit";
            this.confsetAlignmentCommitButton.UseVisualStyleBackColor = true;
            this.confsetAlignmentCommitButton.Click += new System.EventHandler(this.confsetAlignmentCommitButton_Click);
            // 
            // flashCommitButton
            // 
            this.flashCommitButton.Location = new System.Drawing.Point(130, 200);
            this.flashCommitButton.Name = "flashCommitButton";
            this.flashCommitButton.Size = new System.Drawing.Size(86, 23);
            this.flashCommitButton.TabIndex = 52;
            this.flashCommitButton.Text = "FLASH Commit";
            this.flashCommitButton.UseVisualStyleBackColor = true;
            this.flashCommitButton.Click += new System.EventHandler(this.flashCommitButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ResetEkFbutton);
            this.groupBox2.Controls.Add(this.rebootButton);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.output_enable);
            this.groupBox2.Controls.Add(this.output_rate);
            this.groupBox2.Controls.Add(this.badurate);
            this.groupBox2.Controls.Add(this.ip);
            this.groupBox2.Controls.Add(this.port);
            this.groupBox2.Controls.Add(this.confsetAlignmentCommitButton);
            this.groupBox2.Controls.Add(this.flashCommitButton);
            this.groupBox2.Location = new System.Drawing.Point(23, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(391, 304);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CONF SET";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(255, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 82;
            this.button1.Text = "Reset to Factrory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 81;
            this.label8.Text = "Output Rate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 73;
            this.label1.Text = "Port";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 72;
            this.label7.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 71;
            this.label3.Text = "Badurate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 70;
            this.label6.Text = "Output enable";
            // 
            // output_enable
            // 
            this.output_enable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.output_enable.Location = new System.Drawing.Point(84, 19);
            this.output_enable.Name = "output_enable";
            this.output_enable.Size = new System.Drawing.Size(84, 20);
            this.output_enable.TabIndex = 54;
            // 
            // output_rate
            // 
            this.output_rate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.output_rate.Location = new System.Drawing.Point(84, 48);
            this.output_rate.Name = "output_rate";
            this.output_rate.Size = new System.Drawing.Size(85, 20);
            this.output_rate.TabIndex = 55;
            // 
            // badurate
            // 
            this.badurate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.badurate.Location = new System.Drawing.Point(84, 81);
            this.badurate.Name = "badurate";
            this.badurate.Size = new System.Drawing.Size(85, 20);
            this.badurate.TabIndex = 56;
            // 
            // ip
            // 
            this.ip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ip.Location = new System.Drawing.Point(84, 114);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(141, 20);
            this.ip.TabIndex = 57;
            // 
            // port
            // 
            this.port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.port.Location = new System.Drawing.Point(81, 143);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(87, 20);
            this.port.TabIndex = 61;
            // 
            // rebootButton
            // 
            this.rebootButton.Location = new System.Drawing.Point(14, 235);
            this.rebootButton.Name = "rebootButton";
            this.rebootButton.Size = new System.Drawing.Size(86, 23);
            this.rebootButton.TabIndex = 83;
            this.rebootButton.Text = "Reboot";
            this.rebootButton.UseVisualStyleBackColor = true;
            this.rebootButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // ResetEkFbutton
            // 
            this.ResetEkFbutton.Location = new System.Drawing.Point(130, 235);
            this.ResetEkFbutton.Name = "ResetEkFbutton";
            this.ResetEkFbutton.Size = new System.Drawing.Size(86, 23);
            this.ResetEkFbutton.TabIndex = 84;
            this.ResetEkFbutton.Text = "Reset EKF";
            this.ResetEkFbutton.UseVisualStyleBackColor = true;
            this.ResetEkFbutton.Click += new System.EventHandler(this.button3_Click);
            // 
            // ConfSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 282);
            this.Controls.Add(this.groupBox2);
            this.Name = "ConfSet";
            this.Text = "CONF SET";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button confsetAlignmentCommitButton;
        private System.Windows.Forms.Button flashCommitButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox output_enable;
        private System.Windows.Forms.TextBox output_rate;
        private System.Windows.Forms.TextBox badurate;
        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ResetEkFbutton;
        private System.Windows.Forms.Button rebootButton;
    }
}