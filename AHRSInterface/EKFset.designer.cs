namespace AHRSInterface
{
    partial class EKFset
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
            this.EKFsetAlignmentCommitButton = new System.Windows.Forms.Button();
            this.flashCommitButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dev_std_mag = new System.Windows.Forms.TextBox();
            this.dev_std_acc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.conolution_time = new System.Windows.Forms.TextBox();
            this.filter_type = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.quatinit03 = new System.Windows.Forms.TextBox();
            this.Gain03 = new System.Windows.Forms.TextBox();
            this.Qbias01 = new System.Windows.Forms.TextBox();
            this.quatinit00 = new System.Windows.Forms.TextBox();
            this.quatinit01 = new System.Windows.Forms.TextBox();
            this.quatinit02 = new System.Windows.Forms.TextBox();
            this.Gain02 = new System.Windows.Forms.TextBox();
            this.Qbias00 = new System.Windows.Forms.TextBox();
            this.Gain01 = new System.Windows.Forms.TextBox();
            this.Qbias02 = new System.Windows.Forms.TextBox();
            this.Gain00 = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // EKFsetAlignmentCommitButton
            // 
            this.EKFsetAlignmentCommitButton.Location = new System.Drawing.Point(14, 255);
            this.EKFsetAlignmentCommitButton.Name = "EKFsetAlignmentCommitButton";
            this.EKFsetAlignmentCommitButton.Size = new System.Drawing.Size(86, 23);
            this.EKFsetAlignmentCommitButton.TabIndex = 2;
            this.EKFsetAlignmentCommitButton.Text = "RAM Commit";
            this.EKFsetAlignmentCommitButton.UseVisualStyleBackColor = true;
            this.EKFsetAlignmentCommitButton.Click += new System.EventHandler(this.EKFsetAlignmentCommitButton_Click);
            // 
            // flashCommitButton
            // 
            this.flashCommitButton.Location = new System.Drawing.Point(284, 245);
            this.flashCommitButton.Name = "flashCommitButton";
            this.flashCommitButton.Size = new System.Drawing.Size(86, 23);
            this.flashCommitButton.TabIndex = 52;
            this.flashCommitButton.Text = "FLASH Commit";
            this.flashCommitButton.UseVisualStyleBackColor = true;
            this.flashCommitButton.Click += new System.EventHandler(this.flashCommitButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dev_std_mag);
            this.groupBox2.Controls.Add(this.dev_std_acc);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.conolution_time);
            this.groupBox2.Controls.Add(this.filter_type);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.quatinit03);
            this.groupBox2.Controls.Add(this.Gain03);
            this.groupBox2.Controls.Add(this.Qbias01);
            this.groupBox2.Controls.Add(this.quatinit00);
            this.groupBox2.Controls.Add(this.quatinit01);
            this.groupBox2.Controls.Add(this.quatinit02);
            this.groupBox2.Controls.Add(this.Gain02);
            this.groupBox2.Controls.Add(this.Qbias00);
            this.groupBox2.Controls.Add(this.Gain01);
            this.groupBox2.Controls.Add(this.Qbias02);
            this.groupBox2.Controls.Add(this.Gain00);
            this.groupBox2.Controls.Add(this.EKFsetAlignmentCommitButton);
            this.groupBox2.Controls.Add(this.flashCommitButton);
            this.groupBox2.Location = new System.Drawing.Point(23, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(391, 304);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EKF SET";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 80;
            this.label5.Text = "Dev Std MAG";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 79;
            this.label4.Text = "Dev Std ACC";
            // 
            // dev_std_mag
            // 
            this.dev_std_mag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dev_std_mag.Location = new System.Drawing.Point(286, 209);
            this.dev_std_mag.Name = "dev_std_mag";
            this.dev_std_mag.Size = new System.Drawing.Size(84, 20);
            this.dev_std_mag.TabIndex = 78;
            // 
            // dev_std_acc
            // 
            this.dev_std_acc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dev_std_acc.Location = new System.Drawing.Point(196, 209);
            this.dev_std_acc.Name = "dev_std_acc";
            this.dev_std_acc.Size = new System.Drawing.Size(84, 20);
            this.dev_std_acc.TabIndex = 77;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Convolution Time";
            // 
            // conolution_time
            // 
            this.conolution_time.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.conolution_time.Location = new System.Drawing.Point(105, 209);
            this.conolution_time.Name = "conolution_time";
            this.conolution_time.Size = new System.Drawing.Size(84, 20);
            this.conolution_time.TabIndex = 75;
            // 
            // filter_type
            // 
            this.filter_type.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filter_type.Location = new System.Drawing.Point(14, 209);
            this.filter_type.Name = "filter_type";
            this.filter_type.Size = new System.Drawing.Size(84, 20);
            this.filter_type.TabIndex = 74;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 73;
            this.label1.Text = "Filter Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 72;
            this.label7.Text = "GAIN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 71;
            this.label3.Text = "Q BIAS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 70;
            this.label6.Text = "QUAT INIT";
            // 
            // quatinit03
            // 
            this.quatinit03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quatinit03.Location = new System.Drawing.Point(286, 83);
            this.quatinit03.Name = "quatinit03";
            this.quatinit03.Size = new System.Drawing.Size(74, 20);
            this.quatinit03.TabIndex = 63;
            // 
            // Gain03
            // 
            this.Gain03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Gain03.Location = new System.Drawing.Point(286, 161);
            this.Gain03.Name = "Gain03";
            this.Gain03.Size = new System.Drawing.Size(74, 20);
            this.Gain03.TabIndex = 65;
            // 
            // Qbias01
            // 
            this.Qbias01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Qbias01.Location = new System.Drawing.Point(104, 122);
            this.Qbias01.Name = "Qbias01";
            this.Qbias01.Size = new System.Drawing.Size(85, 20);
            this.Qbias01.TabIndex = 58;
            // 
            // quatinit00
            // 
            this.quatinit00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quatinit00.Location = new System.Drawing.Point(12, 83);
            this.quatinit00.Name = "quatinit00";
            this.quatinit00.Size = new System.Drawing.Size(84, 20);
            this.quatinit00.TabIndex = 54;
            // 
            // quatinit01
            // 
            this.quatinit01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quatinit01.Location = new System.Drawing.Point(104, 83);
            this.quatinit01.Name = "quatinit01";
            this.quatinit01.Size = new System.Drawing.Size(85, 20);
            this.quatinit01.TabIndex = 55;
            // 
            // quatinit02
            // 
            this.quatinit02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quatinit02.Location = new System.Drawing.Point(195, 83);
            this.quatinit02.Name = "quatinit02";
            this.quatinit02.Size = new System.Drawing.Size(85, 20);
            this.quatinit02.TabIndex = 56;
            // 
            // Gain02
            // 
            this.Gain02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Gain02.Location = new System.Drawing.Point(196, 161);
            this.Gain02.Name = "Gain02";
            this.Gain02.Size = new System.Drawing.Size(85, 20);
            this.Gain02.TabIndex = 62;
            // 
            // Qbias00
            // 
            this.Qbias00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Qbias00.Location = new System.Drawing.Point(14, 122);
            this.Qbias00.Name = "Qbias00";
            this.Qbias00.Size = new System.Drawing.Size(84, 20);
            this.Qbias00.TabIndex = 57;
            // 
            // Gain01
            // 
            this.Gain01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Gain01.Location = new System.Drawing.Point(105, 161);
            this.Gain01.Name = "Gain01";
            this.Gain01.Size = new System.Drawing.Size(87, 20);
            this.Gain01.TabIndex = 61;
            // 
            // Qbias02
            // 
            this.Qbias02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Qbias02.Location = new System.Drawing.Point(196, 122);
            this.Qbias02.Name = "Qbias02";
            this.Qbias02.Size = new System.Drawing.Size(85, 20);
            this.Qbias02.TabIndex = 59;
            // 
            // Gain00
            // 
            this.Gain00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Gain00.Location = new System.Drawing.Point(14, 161);
            this.Gain00.Name = "Gain00";
            this.Gain00.Size = new System.Drawing.Size(84, 20);
            this.Gain00.TabIndex = 60;
            // 
            // EKFset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 325);
            this.Controls.Add(this.groupBox2);
            this.Name = "EKFset";
            this.Text = "EKF SET";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button EKFsetAlignmentCommitButton;
        private System.Windows.Forms.Button flashCommitButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox quatinit03;
        private System.Windows.Forms.TextBox Gain03;
        private System.Windows.Forms.TextBox Qbias01;
        private System.Windows.Forms.TextBox quatinit00;
        private System.Windows.Forms.TextBox quatinit01;
        private System.Windows.Forms.TextBox quatinit02;
        private System.Windows.Forms.TextBox Gain02;
        private System.Windows.Forms.TextBox Qbias00;
        private System.Windows.Forms.TextBox Gain01;
        private System.Windows.Forms.TextBox Qbias02;
        private System.Windows.Forms.TextBox Gain00;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox conolution_time;
        private System.Windows.Forms.TextBox filter_type;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox dev_std_mag;
        private System.Windows.Forms.TextBox dev_std_acc;
    }
}