namespace AHRSInterface
{
    partial class AccCal
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
            this.accAlignmentCommitButton = new System.Windows.Forms.Button();
            this.flashCommitButton = new System.Windows.Forms.Button();
            this.computeaccCalButton = new System.Windows.Forms.Button();
            this.startDataCollectionButton = new System.Windows.Forms.Button();
            this.dataProgressBar = new System.Windows.Forms.ProgressBar();
            this.stopDataCollectionButton = new System.Windows.Forms.Button();
            this.accCalHelpButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.writeToFileButton = new System.Windows.Forms.Button();
            this.accCalStatusText = new System.Windows.Forms.Label();
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
            this.accAlignment11 = new System.Windows.Forms.TextBox();
            this.accAlignment00 = new System.Windows.Forms.TextBox();
            this.accAlignment01 = new System.Windows.Forms.TextBox();
            this.accAlignment02 = new System.Windows.Forms.TextBox();
            this.accAlignment22 = new System.Windows.Forms.TextBox();
            this.accAlignment10 = new System.Windows.Forms.TextBox();
            this.accAlignment21 = new System.Windows.Forms.TextBox();
            this.accAlignment12 = new System.Windows.Forms.TextBox();
            this.accAlignment20 = new System.Windows.Forms.TextBox();
            this.accCalResetButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // accAlignmentCommitButton
            // 
            this.accAlignmentCommitButton.Enabled = false;
            this.accAlignmentCommitButton.Location = new System.Drawing.Point(11, 179);
            this.accAlignmentCommitButton.Name = "accAlignmentCommitButton";
            this.accAlignmentCommitButton.Size = new System.Drawing.Size(86, 23);
            this.accAlignmentCommitButton.TabIndex = 2;
            this.accAlignmentCommitButton.Text = "RAM Commit";
            this.accAlignmentCommitButton.UseVisualStyleBackColor = true;
            this.accAlignmentCommitButton.Click += new System.EventHandler(this.accAlignmentCommitButton_Click);
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
            // computeaccCalButton
            // 
            this.computeaccCalButton.Enabled = false;
            this.computeaccCalButton.Location = new System.Drawing.Point(11, 28);
            this.computeaccCalButton.Name = "computeaccCalButton";
            this.computeaccCalButton.Size = new System.Drawing.Size(149, 23);
            this.computeaccCalButton.TabIndex = 53;
            this.computeaccCalButton.Text = "Compute Calibration";
            this.computeaccCalButton.UseVisualStyleBackColor = true;
            this.computeaccCalButton.Click += new System.EventHandler(this.computeaccCalButton_Click);
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
            // accCalHelpButton
            // 
            this.accCalHelpButton.Location = new System.Drawing.Point(422, 12);
            this.accCalHelpButton.Name = "accCalHelpButton";
            this.accCalHelpButton.Size = new System.Drawing.Size(57, 23);
            this.accCalHelpButton.TabIndex = 58;
            this.accCalHelpButton.Text = "Help";
            this.accCalHelpButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.writeToFileButton);
            this.groupBox1.Controls.Add(this.accCalStatusText);
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
            // accCalStatusText
            // 
            this.accCalStatusText.AutoSize = true;
            this.accCalStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accCalStatusText.Location = new System.Drawing.Point(54, 57);
            this.accCalStatusText.Name = "accCalStatusText";
            this.accCalStatusText.Size = new System.Drawing.Size(53, 13);
            this.accCalStatusText.TabIndex = 62;
            this.accCalStatusText.Text = "Inactive";
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
            this.groupBox2.Controls.Add(this.accAlignment11);
            this.groupBox2.Controls.Add(this.accAlignment00);
            this.groupBox2.Controls.Add(this.accAlignment01);
            this.groupBox2.Controls.Add(this.accAlignment02);
            this.groupBox2.Controls.Add(this.accAlignment22);
            this.groupBox2.Controls.Add(this.accAlignment10);
            this.groupBox2.Controls.Add(this.accAlignment21);
            this.groupBox2.Controls.Add(this.accAlignment12);
            this.groupBox2.Controls.Add(this.accAlignment20);
            this.groupBox2.Controls.Add(this.accAlignmentCommitButton);
            this.groupBox2.Controls.Add(this.flashCommitButton);
            this.groupBox2.Controls.Add(this.computeaccCalButton);
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
            // accAlignment11
            // 
            this.accAlignment11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment11.Enabled = false;
            this.accAlignment11.Location = new System.Drawing.Point(104, 109);
            this.accAlignment11.Name = "accAlignment11";
            this.accAlignment11.Size = new System.Drawing.Size(85, 20);
            this.accAlignment11.TabIndex = 58;
            // 
            // accAlignment00
            // 
            this.accAlignment00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment00.Enabled = false;
            this.accAlignment00.Location = new System.Drawing.Point(12, 83);
            this.accAlignment00.Name = "accAlignment00";
            this.accAlignment00.Size = new System.Drawing.Size(84, 20);
            this.accAlignment00.TabIndex = 54;
            // 
            // accAlignment01
            // 
            this.accAlignment01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment01.Enabled = false;
            this.accAlignment01.Location = new System.Drawing.Point(104, 83);
            this.accAlignment01.Name = "accAlignment01";
            this.accAlignment01.Size = new System.Drawing.Size(85, 20);
            this.accAlignment01.TabIndex = 55;
            // 
            // accAlignment02
            // 
            this.accAlignment02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment02.Enabled = false;
            this.accAlignment02.Location = new System.Drawing.Point(195, 83);
            this.accAlignment02.Name = "accAlignment02";
            this.accAlignment02.Size = new System.Drawing.Size(85, 20);
            this.accAlignment02.TabIndex = 56;
            // 
            // accAlignment22
            // 
            this.accAlignment22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment22.Enabled = false;
            this.accAlignment22.Location = new System.Drawing.Point(195, 135);
            this.accAlignment22.Name = "accAlignment22";
            this.accAlignment22.Size = new System.Drawing.Size(85, 20);
            this.accAlignment22.TabIndex = 62;
            // 
            // accAlignment10
            // 
            this.accAlignment10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment10.Enabled = false;
            this.accAlignment10.Location = new System.Drawing.Point(12, 109);
            this.accAlignment10.Name = "accAlignment10";
            this.accAlignment10.Size = new System.Drawing.Size(84, 20);
            this.accAlignment10.TabIndex = 57;
            // 
            // accAlignment21
            // 
            this.accAlignment21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment21.Enabled = false;
            this.accAlignment21.Location = new System.Drawing.Point(102, 137);
            this.accAlignment21.Name = "accAlignment21";
            this.accAlignment21.Size = new System.Drawing.Size(87, 20);
            this.accAlignment21.TabIndex = 61;
            // 
            // accAlignment12
            // 
            this.accAlignment12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment12.Enabled = false;
            this.accAlignment12.Location = new System.Drawing.Point(195, 109);
            this.accAlignment12.Name = "accAlignment12";
            this.accAlignment12.Size = new System.Drawing.Size(85, 20);
            this.accAlignment12.TabIndex = 59;
            // 
            // accAlignment20
            // 
            this.accAlignment20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accAlignment20.Enabled = false;
            this.accAlignment20.Location = new System.Drawing.Point(12, 135);
            this.accAlignment20.Name = "accAlignment20";
            this.accAlignment20.Size = new System.Drawing.Size(84, 20);
            this.accAlignment20.TabIndex = 60;
            // 
            // accCalResetButton
            // 
            this.accCalResetButton.Location = new System.Drawing.Point(283, 12);
            this.accCalResetButton.Name = "accCalResetButton";
            this.accCalResetButton.Size = new System.Drawing.Size(57, 23);
            this.accCalResetButton.TabIndex = 63;
            this.accCalResetButton.Text = "Reset";
            this.accCalResetButton.UseVisualStyleBackColor = true;
            this.accCalResetButton.Click += new System.EventHandler(this.accCalResetButton_Click);
            // 
            // AccCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 394);
            this.Controls.Add(this.accCalResetButton);
            this.Controls.Add(this.accCalHelpButton);
            this.Controls.Add(this.stopDataCollectionButton);
            this.Controls.Add(this.startDataCollectionButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "AccCal";
            this.Text = "Acceleromiter Calibration";
            this.Load += new System.EventHandler(this.accCal_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button accAlignmentCommitButton;
        private System.Windows.Forms.Button flashCommitButton;
        private System.Windows.Forms.Button computeaccCalButton;
        private System.Windows.Forms.Button startDataCollectionButton;
        private System.Windows.Forms.ProgressBar dataProgressBar;
        private System.Windows.Forms.Button stopDataCollectionButton;
        private System.Windows.Forms.Button accCalHelpButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label accCalStatusText;
        private System.Windows.Forms.Button accCalResetButton;
        private System.Windows.Forms.Button writeToFileButton;
        private System.Windows.Forms.TextBox biasX;
        private System.Windows.Forms.TextBox biasZ;
        private System.Windows.Forms.TextBox biasY;
        private System.Windows.Forms.TextBox accAlignment11;
        private System.Windows.Forms.TextBox accAlignment00;
        private System.Windows.Forms.TextBox accAlignment01;
        private System.Windows.Forms.TextBox accAlignment02;
        private System.Windows.Forms.TextBox accAlignment22;
        private System.Windows.Forms.TextBox accAlignment10;
        private System.Windows.Forms.TextBox accAlignment21;
        private System.Windows.Forms.TextBox accAlignment12;
        private System.Windows.Forms.TextBox accAlignment20;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label calStatusText;
        private System.Windows.Forms.Label label7;
    }
}