namespace AHRSInterface
{
    partial class calcVar
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
            this.computecalcVarButton = new System.Windows.Forms.Button();
            this.startDataCollectionButton = new System.Windows.Forms.Button();
            this.dataProgressBar = new System.Windows.Forms.ProgressBar();
            this.stopDataCollectionButton = new System.Windows.Forms.Button();
            this.calcVarHelpButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.calcVarStatusText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.calStatusText = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.VarAlignment11 = new System.Windows.Forms.TextBox();
            this.VarAlignment00 = new System.Windows.Forms.TextBox();
            this.VarAlignment01 = new System.Windows.Forms.TextBox();
            this.VarAlignment02 = new System.Windows.Forms.TextBox();
            this.VarAlignment22 = new System.Windows.Forms.TextBox();
            this.VarAlignment10 = new System.Windows.Forms.TextBox();
            this.VarAlignment21 = new System.Windows.Forms.TextBox();
            this.VarAlignment12 = new System.Windows.Forms.TextBox();
            this.VarAlignment20 = new System.Windows.Forms.TextBox();
            this.calcVarResetButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // computecalcVarButton
            // 
            this.computecalcVarButton.Enabled = false;
            this.computecalcVarButton.Location = new System.Drawing.Point(11, 28);
            this.computecalcVarButton.Name = "computecalcVarButton";
            this.computecalcVarButton.Size = new System.Drawing.Size(149, 23);
            this.computecalcVarButton.TabIndex = 53;
            this.computecalcVarButton.Text = "Compute Variance";
            this.computecalcVarButton.UseVisualStyleBackColor = true;
            this.computecalcVarButton.Click += new System.EventHandler(this.computecalcVarButton_Click);
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
            // calcVarHelpButton
            // 
            this.calcVarHelpButton.Location = new System.Drawing.Point(422, 12);
            this.calcVarHelpButton.Name = "calcVarHelpButton";
            this.calcVarHelpButton.Size = new System.Drawing.Size(57, 23);
            this.calcVarHelpButton.TabIndex = 58;
            this.calcVarHelpButton.Text = "Help";
            this.calcVarHelpButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.calcVarStatusText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dataProgressBar);
            this.groupBox1.Location = new System.Drawing.Point(23, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 85);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Collection Progress";
            // 
            // calcVarStatusText
            // 
            this.calcVarStatusText.AutoSize = true;
            this.calcVarStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calcVarStatusText.Location = new System.Drawing.Point(54, 57);
            this.calcVarStatusText.Name = "calcVarStatusText";
            this.calcVarStatusText.Size = new System.Drawing.Size(53, 13);
            this.calcVarStatusText.TabIndex = 62;
            this.calcVarStatusText.Text = "Inactive";
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
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.calStatusText);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.VarAlignment11);
            this.groupBox2.Controls.Add(this.VarAlignment00);
            this.groupBox2.Controls.Add(this.VarAlignment01);
            this.groupBox2.Controls.Add(this.VarAlignment02);
            this.groupBox2.Controls.Add(this.VarAlignment22);
            this.groupBox2.Controls.Add(this.VarAlignment10);
            this.groupBox2.Controls.Add(this.VarAlignment21);
            this.groupBox2.Controls.Add(this.VarAlignment12);
            this.groupBox2.Controls.Add(this.VarAlignment20);
            this.groupBox2.Controls.Add(this.computecalcVarButton);
            this.groupBox2.Location = new System.Drawing.Point(23, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(455, 240);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calibration Computation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(223, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 75;
            this.label6.Text = "Variance Gyro";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(127, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 74;
            this.label5.Text = "Variance Mag";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(36, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 73;
            this.label8.Text = "Variance Acc";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 68;
            this.label4.Text = "Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 67;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 66;
            this.label1.Text = "X";
            // 
            // VarAlignment11
            // 
            this.VarAlignment11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment11.Enabled = false;
            this.VarAlignment11.Location = new System.Drawing.Point(118, 109);
            this.VarAlignment11.Name = "VarAlignment11";
            this.VarAlignment11.Size = new System.Drawing.Size(85, 20);
            this.VarAlignment11.TabIndex = 58;
            // 
            // VarAlignment00
            // 
            this.VarAlignment00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment00.Enabled = false;
            this.VarAlignment00.Location = new System.Drawing.Point(28, 83);
            this.VarAlignment00.Name = "VarAlignment00";
            this.VarAlignment00.Size = new System.Drawing.Size(84, 20);
            this.VarAlignment00.TabIndex = 54;
            // 
            // VarAlignment01
            // 
            this.VarAlignment01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment01.Enabled = false;
            this.VarAlignment01.Location = new System.Drawing.Point(118, 83);
            this.VarAlignment01.Name = "VarAlignment01";
            this.VarAlignment01.Size = new System.Drawing.Size(85, 20);
            this.VarAlignment01.TabIndex = 55;
            // 
            // VarAlignment02
            // 
            this.VarAlignment02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment02.Enabled = false;
            this.VarAlignment02.Location = new System.Drawing.Point(209, 83);
            this.VarAlignment02.Name = "VarAlignment02";
            this.VarAlignment02.Size = new System.Drawing.Size(85, 20);
            this.VarAlignment02.TabIndex = 56;
            // 
            // VarAlignment22
            // 
            this.VarAlignment22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment22.Enabled = false;
            this.VarAlignment22.Location = new System.Drawing.Point(209, 135);
            this.VarAlignment22.Name = "VarAlignment22";
            this.VarAlignment22.Size = new System.Drawing.Size(85, 20);
            this.VarAlignment22.TabIndex = 62;
            // 
            // VarAlignment10
            // 
            this.VarAlignment10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment10.Enabled = false;
            this.VarAlignment10.Location = new System.Drawing.Point(28, 109);
            this.VarAlignment10.Name = "VarAlignment10";
            this.VarAlignment10.Size = new System.Drawing.Size(84, 20);
            this.VarAlignment10.TabIndex = 57;
            // 
            // VarAlignment21
            // 
            this.VarAlignment21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment21.Enabled = false;
            this.VarAlignment21.Location = new System.Drawing.Point(116, 135);
            this.VarAlignment21.Name = "VarAlignment21";
            this.VarAlignment21.Size = new System.Drawing.Size(87, 20);
            this.VarAlignment21.TabIndex = 61;
            // 
            // VarAlignment12
            // 
            this.VarAlignment12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment12.Enabled = false;
            this.VarAlignment12.Location = new System.Drawing.Point(209, 109);
            this.VarAlignment12.Name = "VarAlignment12";
            this.VarAlignment12.Size = new System.Drawing.Size(85, 20);
            this.VarAlignment12.TabIndex = 59;
            // 
            // VarAlignment20
            // 
            this.VarAlignment20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VarAlignment20.Enabled = false;
            this.VarAlignment20.Location = new System.Drawing.Point(28, 135);
            this.VarAlignment20.Name = "VarAlignment20";
            this.VarAlignment20.Size = new System.Drawing.Size(84, 20);
            this.VarAlignment20.TabIndex = 60;
            // 
            // calcVarResetButton
            // 
            this.calcVarResetButton.Location = new System.Drawing.Point(283, 12);
            this.calcVarResetButton.Name = "calcVarResetButton";
            this.calcVarResetButton.Size = new System.Drawing.Size(57, 23);
            this.calcVarResetButton.TabIndex = 63;
            this.calcVarResetButton.Text = "Reset";
            this.calcVarResetButton.UseVisualStyleBackColor = true;
            this.calcVarResetButton.Click += new System.EventHandler(this.calcVarResetButton_Click);
            // 
            // calcVar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 394);
            this.Controls.Add(this.calcVarResetButton);
            this.Controls.Add(this.calcVarHelpButton);
            this.Controls.Add(this.stopDataCollectionButton);
            this.Controls.Add(this.startDataCollectionButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "calcVar";
            this.Text = "Variance Calibration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button computecalcVarButton;
        private System.Windows.Forms.Button startDataCollectionButton;
        private System.Windows.Forms.ProgressBar dataProgressBar;
        private System.Windows.Forms.Button stopDataCollectionButton;
        private System.Windows.Forms.Button calcVarHelpButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label calcVarStatusText;
        private System.Windows.Forms.Button calcVarResetButton;
        private System.Windows.Forms.TextBox VarAlignment11;
        private System.Windows.Forms.TextBox VarAlignment00;
        private System.Windows.Forms.TextBox VarAlignment01;
        private System.Windows.Forms.TextBox VarAlignment02;
        private System.Windows.Forms.TextBox VarAlignment22;
        private System.Windows.Forms.TextBox VarAlignment10;
        private System.Windows.Forms.TextBox VarAlignment21;
        private System.Windows.Forms.TextBox VarAlignment12;
        private System.Windows.Forms.TextBox VarAlignment20;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label calStatusText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
    }
}