namespace AHRSInterface
{
    partial class MagCal
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
            this.magAlignmentCommitButton = new System.Windows.Forms.Button();
            this.flashCommitButton = new System.Windows.Forms.Button();
            this.computeMagCalButton = new System.Windows.Forms.Button();
            this.startDataCollectionButton = new System.Windows.Forms.Button();
            this.dataProgressBar = new System.Windows.Forms.ProgressBar();
            this.stopDataCollectionButton = new System.Windows.Forms.Button();
            this.magCalHelpButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.writeToFileButton = new System.Windows.Forms.Button();
            this.magCalStatusText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.calStatusText = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.biasX = new System.Windows.Forms.TextBox();
            this.biasZ = new System.Windows.Forms.TextBox();
            this.biasY = new System.Windows.Forms.TextBox();
            this.magAlignment11 = new System.Windows.Forms.TextBox();
            this.magAlignment00 = new System.Windows.Forms.TextBox();
            this.magAlignment01 = new System.Windows.Forms.TextBox();
            this.magAlignment02 = new System.Windows.Forms.TextBox();
            this.magAlignment22 = new System.Windows.Forms.TextBox();
            this.magAlignment10 = new System.Windows.Forms.TextBox();
            this.magAlignment21 = new System.Windows.Forms.TextBox();
            this.magAlignment12 = new System.Windows.Forms.TextBox();
            this.magAlignment20 = new System.Windows.Forms.TextBox();
            this.magCalResetButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // magAlignmentCommitButton
            // 
            this.magAlignmentCommitButton.Enabled = false;
            this.magAlignmentCommitButton.Location = new System.Drawing.Point(11, 179);
            this.magAlignmentCommitButton.Name = "magAlignmentCommitButton";
            this.magAlignmentCommitButton.Size = new System.Drawing.Size(86, 23);
            this.magAlignmentCommitButton.TabIndex = 2;
            this.magAlignmentCommitButton.Text = "RAM Commit";
            this.magAlignmentCommitButton.UseVisualStyleBackColor = true;
            this.magAlignmentCommitButton.Click += new System.EventHandler(this.magAlignmentCommitButton_Click);
            // 
            // flashCommitButton
            // 
            this.flashCommitButton.Enabled = false;
            this.flashCommitButton.Location = new System.Drawing.Point(363, 179);
            this.flashCommitButton.Name = "flashCommitButton";
            this.flashCommitButton.Size = new System.Drawing.Size(86, 23);
            this.flashCommitButton.TabIndex = 52;
            this.flashCommitButton.Text = "FLASH Commit";
            this.flashCommitButton.UseVisualStyleBackColor = true;
            this.flashCommitButton.Click += new System.EventHandler(this.flashCommitButton_Click);
            // 
            // computeMagCalButton
            // 
            this.computeMagCalButton.Enabled = false;
            this.computeMagCalButton.Location = new System.Drawing.Point(11, 28);
            this.computeMagCalButton.Name = "computeMagCalButton";
            this.computeMagCalButton.Size = new System.Drawing.Size(149, 23);
            this.computeMagCalButton.TabIndex = 53;
            this.computeMagCalButton.Text = "Compute Calibration";
            this.computeMagCalButton.UseVisualStyleBackColor = true;
            this.computeMagCalButton.Click += new System.EventHandler(this.computeMagCalButton_Click);
            // 
            // startDataCollectionButton
            // 
            this.startDataCollectionButton.Location = new System.Drawing.Point(23, 12);
            this.startDataCollectionButton.Name = "startDataCollectionButton";
            this.startDataCollectionButton.Size = new System.Drawing.Size(124, 23);
            this.startDataCollectionButton.TabIndex = 54;
            this.startDataCollectionButton.Text = "Start Data Collection";
            this.startDataCollectionButton.UseVisualStyleBackColor = true;
            this.startDataCollectionButton.Click += new System.EventHandler(this.startDataCollectionButton_Click);
            // 
            // dataProgressBar
            // 
            this.dataProgressBar.Location = new System.Drawing.Point(11, 23);
            this.dataProgressBar.Name = "dataProgressBar";
            this.dataProgressBar.Size = new System.Drawing.Size(439, 18);
            this.dataProgressBar.TabIndex = 55;
            // 
            // stopDataCollectionButton
            // 
            this.stopDataCollectionButton.Enabled = false;
            this.stopDataCollectionButton.Location = new System.Drawing.Point(153, 12);
            this.stopDataCollectionButton.Name = "stopDataCollectionButton";
            this.stopDataCollectionButton.Size = new System.Drawing.Size(124, 23);
            this.stopDataCollectionButton.TabIndex = 57;
            this.stopDataCollectionButton.Text = "Stop Data Collection";
            this.stopDataCollectionButton.UseVisualStyleBackColor = true;
            this.stopDataCollectionButton.Click += new System.EventHandler(this.stopDataCollectionButton_Click);
            // 
            // magCalHelpButton
            // 
            this.magCalHelpButton.Location = new System.Drawing.Point(422, 12);
            this.magCalHelpButton.Name = "magCalHelpButton";
            this.magCalHelpButton.Size = new System.Drawing.Size(57, 23);
            this.magCalHelpButton.TabIndex = 58;
            this.magCalHelpButton.Text = "Help";
            this.magCalHelpButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.writeToFileButton);
            this.groupBox1.Controls.Add(this.magCalStatusText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dataProgressBar);
            this.groupBox1.Location = new System.Drawing.Point(23, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 85);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Collection Progress";
            // 
            // writeToFileButton
            // 
            this.writeToFileButton.Enabled = false;
            this.writeToFileButton.Location = new System.Drawing.Point(375, 52);
            this.writeToFileButton.Name = "writeToFileButton";
            this.writeToFileButton.Size = new System.Drawing.Size(75, 23);
            this.writeToFileButton.TabIndex = 64;
            this.writeToFileButton.Text = "Write to File";
            this.writeToFileButton.UseVisualStyleBackColor = true;
            this.writeToFileButton.Click += new System.EventHandler(this.writeToFileButton_Click);
            // 
            // magCalStatusText
            // 
            this.magCalStatusText.AutoSize = true;
            this.magCalStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.magCalStatusText.Location = new System.Drawing.Point(54, 57);
            this.magCalStatusText.Name = "magCalStatusText";
            this.magCalStatusText.Size = new System.Drawing.Size(53, 13);
            this.magCalStatusText.TabIndex = 62;
            this.magCalStatusText.Text = "Inactive";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "Status:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.calStatusText);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.biasX);
            this.groupBox2.Controls.Add(this.biasZ);
            this.groupBox2.Controls.Add(this.biasY);
            this.groupBox2.Controls.Add(this.magAlignment11);
            this.groupBox2.Controls.Add(this.magAlignment00);
            this.groupBox2.Controls.Add(this.magAlignment01);
            this.groupBox2.Controls.Add(this.magAlignment02);
            this.groupBox2.Controls.Add(this.magAlignment22);
            this.groupBox2.Controls.Add(this.magAlignment10);
            this.groupBox2.Controls.Add(this.magAlignment21);
            this.groupBox2.Controls.Add(this.magAlignment12);
            this.groupBox2.Controls.Add(this.magAlignment20);
            this.groupBox2.Controls.Add(this.magAlignmentCommitButton);
            this.groupBox2.Controls.Add(this.flashCommitButton);
            this.groupBox2.Controls.Add(this.computeMagCalButton);
            this.groupBox2.Location = new System.Drawing.Point(23, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(455, 240);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calibration Computation";
            // 
            // calStatusText
            // 
            this.calStatusText.AutoSize = true;
            this.calStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calStatusText.Location = new System.Drawing.Point(54, 214);
            this.calStatusText.Name = "calStatusText";
            this.calStatusText.Size = new System.Drawing.Size(53, 13);
            this.calStatusText.TabIndex = 71;
            this.calStatusText.Text = "Inactive";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 65;
            this.label7.Text = "Status:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 70;
            this.label6.Text = "Calibration Matrix";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(330, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 69;
            this.label5.Text = "Bias";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(310, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 68;
            this.label4.Text = "Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(310, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 67;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(310, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 66;
            this.label1.Text = "X";
            // 
            // biasX
            // 
            this.biasX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.biasX.Enabled = false;
            this.biasX.Location = new System.Drawing.Point(330, 83);
            this.biasX.Name = "biasX";
            this.biasX.Size = new System.Drawing.Size(74, 20);
            this.biasX.TabIndex = 63;
            // 
            // biasZ
            // 
            this.biasZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.biasZ.Enabled = false;
            this.biasZ.Location = new System.Drawing.Point(330, 135);
            this.biasZ.Name = "biasZ";
            this.biasZ.Size = new System.Drawing.Size(74, 20);
            this.biasZ.TabIndex = 65;
            // 
            // biasY
            // 
            this.biasY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.biasY.Enabled = false;
            this.biasY.Location = new System.Drawing.Point(330, 109);
            this.biasY.Name = "biasY";
            this.biasY.Size = new System.Drawing.Size(74, 20);
            this.biasY.TabIndex = 64;
            // 
            // magAlignment11
            // 
            this.magAlignment11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment11.Enabled = false;
            this.magAlignment11.Location = new System.Drawing.Point(104, 109);
            this.magAlignment11.Name = "magAlignment11";
            this.magAlignment11.Size = new System.Drawing.Size(85, 20);
            this.magAlignment11.TabIndex = 58;
            // 
            // magAlignment00
            // 
            this.magAlignment00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment00.Enabled = false;
            this.magAlignment00.Location = new System.Drawing.Point(12, 83);
            this.magAlignment00.Name = "magAlignment00";
            this.magAlignment00.Size = new System.Drawing.Size(84, 20);
            this.magAlignment00.TabIndex = 54;
            // 
            // magAlignment01
            // 
            this.magAlignment01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment01.Enabled = false;
            this.magAlignment01.Location = new System.Drawing.Point(104, 83);
            this.magAlignment01.Name = "magAlignment01";
            this.magAlignment01.Size = new System.Drawing.Size(85, 20);
            this.magAlignment01.TabIndex = 55;
            // 
            // magAlignment02
            // 
            this.magAlignment02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment02.Enabled = false;
            this.magAlignment02.Location = new System.Drawing.Point(195, 83);
            this.magAlignment02.Name = "magAlignment02";
            this.magAlignment02.Size = new System.Drawing.Size(85, 20);
            this.magAlignment02.TabIndex = 56;
            // 
            // magAlignment22
            // 
            this.magAlignment22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment22.Enabled = false;
            this.magAlignment22.Location = new System.Drawing.Point(195, 135);
            this.magAlignment22.Name = "magAlignment22";
            this.magAlignment22.Size = new System.Drawing.Size(85, 20);
            this.magAlignment22.TabIndex = 62;
            // 
            // magAlignment10
            // 
            this.magAlignment10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment10.Enabled = false;
            this.magAlignment10.Location = new System.Drawing.Point(12, 109);
            this.magAlignment10.Name = "magAlignment10";
            this.magAlignment10.Size = new System.Drawing.Size(84, 20);
            this.magAlignment10.TabIndex = 57;
            // 
            // magAlignment21
            // 
            this.magAlignment21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment21.Enabled = false;
            this.magAlignment21.Location = new System.Drawing.Point(102, 137);
            this.magAlignment21.Name = "magAlignment21";
            this.magAlignment21.Size = new System.Drawing.Size(87, 20);
            this.magAlignment21.TabIndex = 61;
            // 
            // magAlignment12
            // 
            this.magAlignment12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment12.Enabled = false;
            this.magAlignment12.Location = new System.Drawing.Point(195, 109);
            this.magAlignment12.Name = "magAlignment12";
            this.magAlignment12.Size = new System.Drawing.Size(85, 20);
            this.magAlignment12.TabIndex = 59;
            // 
            // magAlignment20
            // 
            this.magAlignment20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magAlignment20.Enabled = false;
            this.magAlignment20.Location = new System.Drawing.Point(12, 135);
            this.magAlignment20.Name = "magAlignment20";
            this.magAlignment20.Size = new System.Drawing.Size(84, 20);
            this.magAlignment20.TabIndex = 60;
            // 
            // magCalResetButton
            // 
            this.magCalResetButton.Location = new System.Drawing.Point(283, 12);
            this.magCalResetButton.Name = "magCalResetButton";
            this.magCalResetButton.Size = new System.Drawing.Size(57, 23);
            this.magCalResetButton.TabIndex = 63;
            this.magCalResetButton.Text = "Reset";
            this.magCalResetButton.UseVisualStyleBackColor = true;
            this.magCalResetButton.Click += new System.EventHandler(this.magCalResetButton_Click);
            // 
            // MagCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 394);
            this.Controls.Add(this.magCalResetButton);
            this.Controls.Add(this.magCalHelpButton);
            this.Controls.Add(this.stopDataCollectionButton);
            this.Controls.Add(this.startDataCollectionButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "MagCal";
            this.Text = "Magnetometer Calibration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button magAlignmentCommitButton;
        private System.Windows.Forms.Button flashCommitButton;
        private System.Windows.Forms.Button computeMagCalButton;
        private System.Windows.Forms.Button startDataCollectionButton;
        private System.Windows.Forms.ProgressBar dataProgressBar;
        private System.Windows.Forms.Button stopDataCollectionButton;
        private System.Windows.Forms.Button magCalHelpButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label magCalStatusText;
        private System.Windows.Forms.Button magCalResetButton;
        private System.Windows.Forms.Button writeToFileButton;
        private System.Windows.Forms.TextBox biasX;
        private System.Windows.Forms.TextBox biasZ;
        private System.Windows.Forms.TextBox biasY;
        private System.Windows.Forms.TextBox magAlignment11;
        private System.Windows.Forms.TextBox magAlignment00;
        private System.Windows.Forms.TextBox magAlignment01;
        private System.Windows.Forms.TextBox magAlignment02;
        private System.Windows.Forms.TextBox magAlignment22;
        private System.Windows.Forms.TextBox magAlignment10;
        private System.Windows.Forms.TextBox magAlignment21;
        private System.Windows.Forms.TextBox magAlignment12;
        private System.Windows.Forms.TextBox magAlignment20;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label calStatusText;
        private System.Windows.Forms.Label label7;
    }
}