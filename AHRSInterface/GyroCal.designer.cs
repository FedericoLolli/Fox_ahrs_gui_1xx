namespace AHRSInterface
{
    partial class GyroCal
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
            this.gyroAlignmentCommitButton = new System.Windows.Forms.Button();
            this.flashCommitButton = new System.Windows.Forms.Button();
            this.computegyroCalButton = new System.Windows.Forms.Button();
            this.startDataCollectionButton = new System.Windows.Forms.Button();
            this.dataProgressBar = new System.Windows.Forms.ProgressBar();
            this.stopDataCollectionButton = new System.Windows.Forms.Button();
            this.gyroCalHelpButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.writeToFileButton = new System.Windows.Forms.Button();
            this.gyroCalStatusText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gyroautobiascorrection = new System.Windows.Forms.Button();
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
            this.gyroAlignment11 = new System.Windows.Forms.TextBox();
            this.gyroAlignment00 = new System.Windows.Forms.TextBox();
            this.gyroAlignment01 = new System.Windows.Forms.TextBox();
            this.gyroAlignment02 = new System.Windows.Forms.TextBox();
            this.gyroAlignment22 = new System.Windows.Forms.TextBox();
            this.gyroAlignment10 = new System.Windows.Forms.TextBox();
            this.gyroAlignment21 = new System.Windows.Forms.TextBox();
            this.gyroAlignment12 = new System.Windows.Forms.TextBox();
            this.gyroAlignment20 = new System.Windows.Forms.TextBox();
            this.gyroCalResetButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gyroAlignmentCommitButton
            // 
            this.gyroAlignmentCommitButton.Enabled = false;
            this.gyroAlignmentCommitButton.Location = new System.Drawing.Point(11, 179);
            this.gyroAlignmentCommitButton.Name = "gyroAlignmentCommitButton";
            this.gyroAlignmentCommitButton.Size = new System.Drawing.Size(86, 23);
            this.gyroAlignmentCommitButton.TabIndex = 2;
            this.gyroAlignmentCommitButton.Text = "RAM Commit";
            this.gyroAlignmentCommitButton.UseVisualStyleBackColor = true;
            this.gyroAlignmentCommitButton.Click += new System.EventHandler(this.gyroAlignmentCommitButton_Click);
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
            // computegyroCalButton
            // 
            this.computegyroCalButton.Enabled = false;
            this.computegyroCalButton.Location = new System.Drawing.Point(11, 28);
            this.computegyroCalButton.Name = "computegyroCalButton";
            this.computegyroCalButton.Size = new System.Drawing.Size(149, 23);
            this.computegyroCalButton.TabIndex = 53;
            this.computegyroCalButton.Text = "Compute Calibration";
            this.computegyroCalButton.UseVisualStyleBackColor = true;
            this.computegyroCalButton.Click += new System.EventHandler(this.computegyroCalButton_Click);
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
            // gyroCalHelpButton
            // 
            this.gyroCalHelpButton.Location = new System.Drawing.Point(422, 12);
            this.gyroCalHelpButton.Name = "gyroCalHelpButton";
            this.gyroCalHelpButton.Size = new System.Drawing.Size(57, 23);
            this.gyroCalHelpButton.TabIndex = 58;
            this.gyroCalHelpButton.Text = "Help";
            this.gyroCalHelpButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.writeToFileButton);
            this.groupBox1.Controls.Add(this.gyroCalStatusText);
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
            // gyroCalStatusText
            // 
            this.gyroCalStatusText.AutoSize = true;
            this.gyroCalStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gyroCalStatusText.Location = new System.Drawing.Point(54, 57);
            this.gyroCalStatusText.Name = "gyroCalStatusText";
            this.gyroCalStatusText.Size = new System.Drawing.Size(53, 13);
            this.gyroCalStatusText.TabIndex = 62;
            this.gyroCalStatusText.Text = "Inactive";
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
            this.groupBox2.Controls.Add(this.gyroautobiascorrection);
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
            this.groupBox2.Controls.Add(this.gyroAlignment11);
            this.groupBox2.Controls.Add(this.gyroAlignment00);
            this.groupBox2.Controls.Add(this.gyroAlignment01);
            this.groupBox2.Controls.Add(this.gyroAlignment02);
            this.groupBox2.Controls.Add(this.gyroAlignment22);
            this.groupBox2.Controls.Add(this.gyroAlignment10);
            this.groupBox2.Controls.Add(this.gyroAlignment21);
            this.groupBox2.Controls.Add(this.gyroAlignment12);
            this.groupBox2.Controls.Add(this.gyroAlignment20);
            this.groupBox2.Controls.Add(this.gyroAlignmentCommitButton);
            this.groupBox2.Controls.Add(this.flashCommitButton);
            this.groupBox2.Controls.Add(this.computegyroCalButton);
            this.groupBox2.Location = new System.Drawing.Point(23, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(455, 240);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calibration Computation";
            // 
            // gyroautobiascorrection
            // 
            this.gyroautobiascorrection.Location = new System.Drawing.Point(118, 179);
            this.gyroautobiascorrection.Name = "gyroautobiascorrection";
            this.gyroautobiascorrection.Size = new System.Drawing.Size(136, 23);
            this.gyroautobiascorrection.TabIndex = 72;
            this.gyroautobiascorrection.Text = "Auto Gyro correct Bias";
            this.gyroautobiascorrection.UseVisualStyleBackColor = true;
            this.gyroautobiascorrection.Click += new System.EventHandler(this.button1_Click);
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
            // gyroAlignment11
            // 
            this.gyroAlignment11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment11.Enabled = false;
            this.gyroAlignment11.Location = new System.Drawing.Point(104, 109);
            this.gyroAlignment11.Name = "gyroAlignment11";
            this.gyroAlignment11.Size = new System.Drawing.Size(85, 20);
            this.gyroAlignment11.TabIndex = 58;
            // 
            // gyroAlignment00
            // 
            this.gyroAlignment00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment00.Enabled = false;
            this.gyroAlignment00.Location = new System.Drawing.Point(12, 83);
            this.gyroAlignment00.Name = "gyroAlignment00";
            this.gyroAlignment00.Size = new System.Drawing.Size(84, 20);
            this.gyroAlignment00.TabIndex = 54;
            // 
            // gyroAlignment01
            // 
            this.gyroAlignment01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment01.Enabled = false;
            this.gyroAlignment01.Location = new System.Drawing.Point(104, 83);
            this.gyroAlignment01.Name = "gyroAlignment01";
            this.gyroAlignment01.Size = new System.Drawing.Size(85, 20);
            this.gyroAlignment01.TabIndex = 55;
            // 
            // gyroAlignment02
            // 
            this.gyroAlignment02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment02.Enabled = false;
            this.gyroAlignment02.Location = new System.Drawing.Point(195, 83);
            this.gyroAlignment02.Name = "gyroAlignment02";
            this.gyroAlignment02.Size = new System.Drawing.Size(85, 20);
            this.gyroAlignment02.TabIndex = 56;
            // 
            // gyroAlignment22
            // 
            this.gyroAlignment22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment22.Enabled = false;
            this.gyroAlignment22.Location = new System.Drawing.Point(195, 135);
            this.gyroAlignment22.Name = "gyroAlignment22";
            this.gyroAlignment22.Size = new System.Drawing.Size(85, 20);
            this.gyroAlignment22.TabIndex = 62;
            // 
            // gyroAlignment10
            // 
            this.gyroAlignment10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment10.Enabled = false;
            this.gyroAlignment10.Location = new System.Drawing.Point(12, 109);
            this.gyroAlignment10.Name = "gyroAlignment10";
            this.gyroAlignment10.Size = new System.Drawing.Size(84, 20);
            this.gyroAlignment10.TabIndex = 57;
            // 
            // gyroAlignment21
            // 
            this.gyroAlignment21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment21.Enabled = false;
            this.gyroAlignment21.Location = new System.Drawing.Point(102, 137);
            this.gyroAlignment21.Name = "gyroAlignment21";
            this.gyroAlignment21.Size = new System.Drawing.Size(87, 20);
            this.gyroAlignment21.TabIndex = 61;
            // 
            // gyroAlignment12
            // 
            this.gyroAlignment12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment12.Enabled = false;
            this.gyroAlignment12.Location = new System.Drawing.Point(195, 109);
            this.gyroAlignment12.Name = "gyroAlignment12";
            this.gyroAlignment12.Size = new System.Drawing.Size(85, 20);
            this.gyroAlignment12.TabIndex = 59;
            // 
            // gyroAlignment20
            // 
            this.gyroAlignment20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gyroAlignment20.Enabled = false;
            this.gyroAlignment20.Location = new System.Drawing.Point(12, 135);
            this.gyroAlignment20.Name = "gyroAlignment20";
            this.gyroAlignment20.Size = new System.Drawing.Size(84, 20);
            this.gyroAlignment20.TabIndex = 60;
            // 
            // gyroCalResetButton
            // 
            this.gyroCalResetButton.Location = new System.Drawing.Point(283, 12);
            this.gyroCalResetButton.Name = "gyroCalResetButton";
            this.gyroCalResetButton.Size = new System.Drawing.Size(57, 23);
            this.gyroCalResetButton.TabIndex = 63;
            this.gyroCalResetButton.Text = "Reset";
            this.gyroCalResetButton.UseVisualStyleBackColor = true;
            this.gyroCalResetButton.Click += new System.EventHandler(this.gyroCalResetButton_Click);
            // 
            // GyroCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 394);
            this.Controls.Add(this.gyroCalResetButton);
            this.Controls.Add(this.gyroCalHelpButton);
            this.Controls.Add(this.stopDataCollectionButton);
            this.Controls.Add(this.startDataCollectionButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "GyroCal";
            this.Text = "Gyroscope Calibration";
            this.Load += new System.EventHandler(this.gyroCal_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button gyroAlignmentCommitButton;
        private System.Windows.Forms.Button flashCommitButton;
        private System.Windows.Forms.Button computegyroCalButton;
        private System.Windows.Forms.Button startDataCollectionButton;
        private System.Windows.Forms.ProgressBar dataProgressBar;
        private System.Windows.Forms.Button stopDataCollectionButton;
        private System.Windows.Forms.Button gyroCalHelpButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label gyroCalStatusText;
        private System.Windows.Forms.Button gyroCalResetButton;
        private System.Windows.Forms.Button writeToFileButton;
        private System.Windows.Forms.TextBox biasX;
        private System.Windows.Forms.TextBox biasZ;
        private System.Windows.Forms.TextBox biasY;
        private System.Windows.Forms.TextBox gyroAlignment11;
        private System.Windows.Forms.TextBox gyroAlignment00;
        private System.Windows.Forms.TextBox gyroAlignment01;
        private System.Windows.Forms.TextBox gyroAlignment02;
        private System.Windows.Forms.TextBox gyroAlignment22;
        private System.Windows.Forms.TextBox gyroAlignment10;
        private System.Windows.Forms.TextBox gyroAlignment21;
        private System.Windows.Forms.TextBox gyroAlignment12;
        private System.Windows.Forms.TextBox gyroAlignment20;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label calStatusText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button gyroautobiascorrection;
    }
}