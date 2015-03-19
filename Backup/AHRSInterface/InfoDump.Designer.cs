namespace AHRSInterface
{
    partial class InfoDump
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
            this.components = new System.ComponentModel.Container();
            this.SynchButton = new System.Windows.Forms.Button();
            this.InvalidateAllButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.listenModeEnabled = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.angleGraph = new ZedGraph.ZedGraphControl();
            this.angularRateGraph = new ZedGraph.ZedGraphControl();
            this.magGraph = new ZedGraph.ZedGraphControl();
            this.accelGraph = new ZedGraph.ZedGraphControl();
            this.gyroGraph = new ZedGraph.ZedGraphControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.IMU_YAW1 = new System.Windows.Forms.TextBox();
            this.IMU_MAG_Z = new System.Windows.Forms.TextBox();
            this.IMU_MAG_Y = new System.Windows.Forms.TextBox();
            this.IMU_MAG_X = new System.Windows.Forms.TextBox();
            this.IMU_GYRO_Z = new System.Windows.Forms.TextBox();
            this.IMU_GYRO_Y = new System.Windows.Forms.TextBox();
            this.IMU_GYRO_X = new System.Windows.Forms.TextBox();
            this.IMU_ACC_Z = new System.Windows.Forms.TextBox();
            this.IMU_ACC_Y = new System.Windows.Forms.TextBox();
            this.IMU_ACC_X = new System.Windows.Forms.TextBox();
            this.IMU_ROLL = new System.Windows.Forms.TextBox();
            this.IMU_PITCH = new System.Windows.Forms.TextBox();
            this.label207 = new System.Windows.Forms.Label();
            this.activeRoll = new System.Windows.Forms.CheckBox();
            this.activePitch = new System.Windows.Forms.CheckBox();
            this.activeYaw = new System.Windows.Forms.CheckBox();
            this.activeZMag = new System.Windows.Forms.CheckBox();
            this.activeYMag = new System.Windows.Forms.CheckBox();
            this.activeYGyro = new System.Windows.Forms.CheckBox();
            this.activeZGyro = new System.Windows.Forms.CheckBox();
            this.activeXMag = new System.Windows.Forms.CheckBox();
            this.activeXGyro = new System.Windows.Forms.CheckBox();
            this.activeZAccel = new System.Windows.Forms.CheckBox();
            this.activeYAccel = new System.Windows.Forms.CheckBox();
            this.activeXAccel = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.LONGITUDE = new System.Windows.Forms.TextBox();
            this.LATITUDE = new System.Windows.Forms.TextBox();
            this.ALTITUDE = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.tabPage2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SynchButton
            // 
            this.SynchButton.Location = new System.Drawing.Point(110, 606);
            this.SynchButton.Name = "SynchButton";
            this.SynchButton.Size = new System.Drawing.Size(75, 23);
            this.SynchButton.TabIndex = 53;
            this.SynchButton.Text = "Synch";
            this.SynchButton.UseVisualStyleBackColor = true;
            this.SynchButton.Click += new System.EventHandler(this.SynchButton_Click);
            // 
            // InvalidateAllButton
            // 
            this.InvalidateAllButton.Location = new System.Drawing.Point(27, 606);
            this.InvalidateAllButton.Name = "InvalidateAllButton";
            this.InvalidateAllButton.Size = new System.Drawing.Size(75, 23);
            this.InvalidateAllButton.TabIndex = 54;
            this.InvalidateAllButton.Text = "Refresh";
            this.InvalidateAllButton.UseVisualStyleBackColor = true;
            this.InvalidateAllButton.Click += new System.EventHandler(this.InvalidateAllButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 55;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 59;
            this.label6.Text = "X";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "Z";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 63;
            this.label8.Text = "Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 62;
            this.label9.Text = "X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 67;
            this.label10.Text = "Process Variance";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 66;
            this.label11.Text = "Mag Variance";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 65;
            this.label12.Text = "Accel Variance";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 0;
            // 
            // listenModeEnabled
            // 
            this.listenModeEnabled.AutoSize = true;
            this.listenModeEnabled.Location = new System.Drawing.Point(127, 27);
            this.listenModeEnabled.Name = "listenModeEnabled";
            this.listenModeEnabled.Size = new System.Drawing.Size(83, 17);
            this.listenModeEnabled.TabIndex = 1;
            this.listenModeEnabled.TabStop = true;
            this.listenModeEnabled.Text = "Listen Mode";
            this.listenModeEnabled.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 75);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(14, 13);
            this.label17.TabIndex = 77;
            this.label17.Text = "Z";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 49);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(14, 13);
            this.label18.TabIndex = 76;
            this.label18.Text = "Y";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 24);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 13);
            this.label19.TabIndex = 75;
            this.label19.Text = "X";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 13);
            this.label16.TabIndex = 77;
            this.label16.Text = "Z";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(14, 13);
            this.label15.TabIndex = 76;
            this.label15.Text = "Y";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 13);
            this.label14.TabIndex = 75;
            this.label14.Text = "X";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(15, 29);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 13);
            this.label20.TabIndex = 62;
            this.label20.Text = "X";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(15, 83);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(14, 13);
            this.label21.TabIndex = 64;
            this.label21.Text = "Z";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(15, 55);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(14, 13);
            this.label22.TabIndex = 63;
            this.label22.Text = "Y";
            // 
            // angleGraph
            // 
            this.angleGraph.Location = new System.Drawing.Point(311, 21);
            this.angleGraph.Name = "angleGraph";
            this.angleGraph.ScrollGrace = 0;
            this.angleGraph.ScrollMaxX = 0;
            this.angleGraph.ScrollMaxY = 0;
            this.angleGraph.ScrollMaxY2 = 0;
            this.angleGraph.ScrollMinX = 0;
            this.angleGraph.ScrollMinY = 0;
            this.angleGraph.ScrollMinY2 = 0;
            this.angleGraph.Size = new System.Drawing.Size(454, 299);
            this.angleGraph.TabIndex = 79;
            // 
            // angularRateGraph
            // 
            this.angularRateGraph.Location = new System.Drawing.Point(0, 0);
            this.angularRateGraph.Name = "angularRateGraph";
            this.angularRateGraph.ScrollGrace = 0;
            this.angularRateGraph.ScrollMaxX = 0;
            this.angularRateGraph.ScrollMaxY = 0;
            this.angularRateGraph.ScrollMaxY2 = 0;
            this.angularRateGraph.ScrollMinX = 0;
            this.angularRateGraph.ScrollMinY = 0;
            this.angularRateGraph.ScrollMinY2 = 0;
            this.angularRateGraph.Size = new System.Drawing.Size(150, 150);
            this.angularRateGraph.TabIndex = 0;
            // 
            // magGraph
            // 
            this.magGraph.Location = new System.Drawing.Point(311, 337);
            this.magGraph.Name = "magGraph";
            this.magGraph.ScrollGrace = 0;
            this.magGraph.ScrollMaxX = 0;
            this.magGraph.ScrollMaxY = 0;
            this.magGraph.ScrollMaxY2 = 0;
            this.magGraph.ScrollMinX = 0;
            this.magGraph.ScrollMinY = 0;
            this.magGraph.ScrollMinY2 = 0;
            this.magGraph.Size = new System.Drawing.Size(454, 292);
            this.magGraph.TabIndex = 81;
            // 
            // accelGraph
            // 
            this.accelGraph.Location = new System.Drawing.Point(781, 21);
            this.accelGraph.Name = "accelGraph";
            this.accelGraph.ScrollGrace = 0;
            this.accelGraph.ScrollMaxX = 0;
            this.accelGraph.ScrollMaxY = 0;
            this.accelGraph.ScrollMaxY2 = 0;
            this.accelGraph.ScrollMinX = 0;
            this.accelGraph.ScrollMinY = 0;
            this.accelGraph.ScrollMinY2 = 0;
            this.accelGraph.Size = new System.Drawing.Size(454, 299);
            this.accelGraph.TabIndex = 82;
            // 
            // gyroGraph
            // 
            this.gyroGraph.Location = new System.Drawing.Point(781, 337);
            this.gyroGraph.Name = "gyroGraph";
            this.gyroGraph.ScrollGrace = 0;
            this.gyroGraph.ScrollMaxX = 0;
            this.gyroGraph.ScrollMaxY = 0;
            this.gyroGraph.ScrollMaxY2 = 0;
            this.gyroGraph.ScrollMinX = 0;
            this.gyroGraph.ScrollMinY = 0;
            this.gyroGraph.ScrollMinY2 = 0;
            this.gyroGraph.Size = new System.Drawing.Size(454, 292);
            this.gyroGraph.TabIndex = 83;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(200, 100);
            this.tabPage1.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 100);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 100);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(0, 0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(200, 100);
            this.tabPage4.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 100);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 100);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            // 
            // groupBox12
            // 
            this.groupBox12.Location = new System.Drawing.Point(0, 0);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(200, 100);
            this.groupBox12.TabIndex = 0;
            this.groupBox12.TabStop = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(0, 0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(200, 100);
            this.tabPage3.TabIndex = 0;
            // 
            // groupBox13
            // 
            this.groupBox13.Location = new System.Drawing.Point(0, 0);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(200, 100);
            this.groupBox13.TabIndex = 3;
            this.groupBox13.TabStop = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Location = new System.Drawing.Point(0, 0);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(200, 100);
            this.groupBox9.TabIndex = 2;
            this.groupBox9.TabStop = false;
            // 
            // groupBox10
            // 
            this.groupBox10.Location = new System.Drawing.Point(0, 0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(200, 100);
            this.groupBox10.TabIndex = 1;
            this.groupBox10.TabStop = false;
            // 
            // groupBox11
            // 
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(200, 100);
            this.groupBox11.TabIndex = 0;
            this.groupBox11.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(285, 562);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "COM";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label36);
            this.groupBox8.Controls.Add(this.label35);
            this.groupBox8.Controls.Add(this.label34);
            this.groupBox8.Controls.Add(this.ALTITUDE);
            this.groupBox8.Controls.Add(this.LATITUDE);
            this.groupBox8.Controls.Add(this.LONGITUDE);
            this.groupBox8.Controls.Add(this.label33);
            this.groupBox8.Controls.Add(this.label32);
            this.groupBox8.Controls.Add(this.label31);
            this.groupBox8.Controls.Add(this.label30);
            this.groupBox8.Controls.Add(this.label29);
            this.groupBox8.Controls.Add(this.label28);
            this.groupBox8.Controls.Add(this.label27);
            this.groupBox8.Controls.Add(this.label26);
            this.groupBox8.Controls.Add(this.label25);
            this.groupBox8.Controls.Add(this.label24);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Controls.Add(this.IMU_YAW1);
            this.groupBox8.Controls.Add(this.IMU_MAG_Z);
            this.groupBox8.Controls.Add(this.IMU_MAG_Y);
            this.groupBox8.Controls.Add(this.IMU_MAG_X);
            this.groupBox8.Controls.Add(this.IMU_GYRO_Z);
            this.groupBox8.Controls.Add(this.IMU_GYRO_Y);
            this.groupBox8.Controls.Add(this.IMU_GYRO_X);
            this.groupBox8.Controls.Add(this.IMU_ACC_Z);
            this.groupBox8.Controls.Add(this.IMU_ACC_Y);
            this.groupBox8.Controls.Add(this.IMU_ACC_X);
            this.groupBox8.Controls.Add(this.IMU_ROLL);
            this.groupBox8.Controls.Add(this.IMU_PITCH);
            this.groupBox8.Controls.Add(this.label207);
            this.groupBox8.Location = new System.Drawing.Point(6, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(273, 315);
            this.groupBox8.TabIndex = 51;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Data";
            // 
            // IMU_YAW1
            // 
            this.IMU_YAW1.Location = new System.Drawing.Point(197, 57);
            this.IMU_YAW1.Name = "IMU_YAW1";
            this.IMU_YAW1.Size = new System.Drawing.Size(70, 20);
            this.IMU_YAW1.TabIndex = 9;
            this.IMU_YAW1.Text = "0.0";
            this.IMU_YAW1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_MAG_Z
            // 
            this.IMU_MAG_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_MAG_Z.Location = new System.Drawing.Point(200, 154);
            this.IMU_MAG_Z.Name = "IMU_MAG_Z";
            this.IMU_MAG_Z.Size = new System.Drawing.Size(70, 20);
            this.IMU_MAG_Z.TabIndex = 8;
            this.IMU_MAG_Z.Text = "0.0";
            this.IMU_MAG_Z.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_MAG_Y
            // 
            this.IMU_MAG_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_MAG_Y.Location = new System.Drawing.Point(106, 154);
            this.IMU_MAG_Y.Name = "IMU_MAG_Y";
            this.IMU_MAG_Y.Size = new System.Drawing.Size(70, 20);
            this.IMU_MAG_Y.TabIndex = 7;
            this.IMU_MAG_Y.Text = "0.0";
            this.IMU_MAG_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_MAG_X
            // 
            this.IMU_MAG_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_MAG_X.Location = new System.Drawing.Point(10, 154);
            this.IMU_MAG_X.Name = "IMU_MAG_X";
            this.IMU_MAG_X.Size = new System.Drawing.Size(70, 20);
            this.IMU_MAG_X.TabIndex = 6;
            this.IMU_MAG_X.Text = "0.0";
            this.IMU_MAG_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_GYRO_Z
            // 
            this.IMU_GYRO_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_GYRO_Z.Location = new System.Drawing.Point(200, 99);
            this.IMU_GYRO_Z.Name = "IMU_GYRO_Z";
            this.IMU_GYRO_Z.Size = new System.Drawing.Size(70, 20);
            this.IMU_GYRO_Z.TabIndex = 5;
            this.IMU_GYRO_Z.Text = "0.0";
            this.IMU_GYRO_Z.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_GYRO_Y
            // 
            this.IMU_GYRO_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_GYRO_Y.Location = new System.Drawing.Point(106, 99);
            this.IMU_GYRO_Y.Name = "IMU_GYRO_Y";
            this.IMU_GYRO_Y.Size = new System.Drawing.Size(70, 20);
            this.IMU_GYRO_Y.TabIndex = 4;
            this.IMU_GYRO_Y.Text = "0.0";
            this.IMU_GYRO_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_GYRO_X
            // 
            this.IMU_GYRO_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_GYRO_X.Location = new System.Drawing.Point(10, 99);
            this.IMU_GYRO_X.Name = "IMU_GYRO_X";
            this.IMU_GYRO_X.Size = new System.Drawing.Size(70, 20);
            this.IMU_GYRO_X.TabIndex = 3;
            this.IMU_GYRO_X.Text = "0.0";
            this.IMU_GYRO_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_ACC_Z
            // 
            this.IMU_ACC_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_ACC_Z.Location = new System.Drawing.Point(200, 204);
            this.IMU_ACC_Z.Name = "IMU_ACC_Z";
            this.IMU_ACC_Z.Size = new System.Drawing.Size(70, 20);
            this.IMU_ACC_Z.TabIndex = 2;
            this.IMU_ACC_Z.Text = "0.0";
            this.IMU_ACC_Z.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_ACC_Y
            // 
            this.IMU_ACC_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_ACC_Y.Location = new System.Drawing.Point(106, 204);
            this.IMU_ACC_Y.Name = "IMU_ACC_Y";
            this.IMU_ACC_Y.Size = new System.Drawing.Size(70, 20);
            this.IMU_ACC_Y.TabIndex = 1;
            this.IMU_ACC_Y.Text = "0.0";
            this.IMU_ACC_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IMU_ACC_X
            // 
            this.IMU_ACC_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMU_ACC_X.Location = new System.Drawing.Point(10, 204);
            this.IMU_ACC_X.Name = "IMU_ACC_X";
            this.IMU_ACC_X.Size = new System.Drawing.Size(70, 20);
            this.IMU_ACC_X.TabIndex = 0;
            this.IMU_ACC_X.Text = "0.0";
            this.IMU_ACC_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.IMU_ACC_X.WordWrap = false;
            // 
            // IMU_ROLL
            // 
            this.IMU_ROLL.AccessibleName = "";
            this.IMU_ROLL.Location = new System.Drawing.Point(10, 57);
            this.IMU_ROLL.Name = "IMU_ROLL";
            this.IMU_ROLL.Size = new System.Drawing.Size(70, 20);
            this.IMU_ROLL.TabIndex = 0;
            this.IMU_ROLL.Tag = "";
            this.IMU_ROLL.Text = "0.0";
            this.IMU_ROLL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.IMU_ROLL.TextChanged += new System.EventHandler(this.IMU_ROLL_TextChanged);
            // 
            // IMU_PITCH
            // 
            this.IMU_PITCH.Location = new System.Drawing.Point(106, 57);
            this.IMU_PITCH.Name = "IMU_PITCH";
            this.IMU_PITCH.Size = new System.Drawing.Size(70, 20);
            this.IMU_PITCH.TabIndex = 0;
            this.IMU_PITCH.Text = "0.0";
            this.IMU_PITCH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label207
            // 
            this.label207.AutoSize = true;
            this.label207.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label207.Location = new System.Drawing.Point(23, 185);
            this.label207.Name = "label207";
            this.label207.Size = new System.Drawing.Size(46, 13);
            this.label207.TabIndex = 12;
            this.label207.Text = "ACC_X";
            this.label207.Click += new System.EventHandler(this.label207_Click);
           /* // 
            // activeRoll
            // 
            this.activeRoll.AutoSize = true;
            this.activeRoll.Location = new System.Drawing.Point(197, 26);
            this.activeRoll.Name = "activeRoll";
            this.activeRoll.Size = new System.Drawing.Size(44, 17);
            this.activeRoll.TabIndex = 72;
            this.activeRoll.Text = "Roll";
            this.activeRoll.UseVisualStyleBackColor = true;
            // 
            // activePitch
            // 
            this.activePitch.AutoSize = true;
            this.activePitch.Location = new System.Drawing.Point(113, 26);
            this.activePitch.Name = "activePitch";
            this.activePitch.Size = new System.Drawing.Size(50, 17);
            this.activePitch.TabIndex = 71;
            this.activePitch.Text = "Pitch";
            this.activePitch.UseVisualStyleBackColor = true;
            // 
            // activeYaw
            // 
            this.activeYaw.AutoSize = true;
            this.activeYaw.Location = new System.Drawing.Point(28, 26);
            this.activeYaw.Name = "activeYaw";
            this.activeYaw.Size = new System.Drawing.Size(47, 17);
            this.activeYaw.TabIndex = 70;
            this.activeYaw.Text = "Yaw";
            this.activeYaw.UseVisualStyleBackColor = true;
            // 
            // activeZMag
            // 
            this.activeZMag.AutoSize = true;
            this.activeZMag.Location = new System.Drawing.Point(197, 121);
            this.activeZMag.Name = "activeZMag";
            this.activeZMag.Size = new System.Drawing.Size(57, 17);
            this.activeZMag.TabIndex = 84;
            this.activeZMag.Text = "Z Mag";
            this.activeZMag.UseVisualStyleBackColor = true;
            // 
            // activeYMag
            // 
            this.activeYMag.AutoSize = true;
            this.activeYMag.Location = new System.Drawing.Point(112, 121);
            this.activeYMag.Name = "activeYMag";
            this.activeYMag.Size = new System.Drawing.Size(57, 17);
            this.activeYMag.TabIndex = 83;
            this.activeYMag.Text = "Y Mag";
            this.activeYMag.UseVisualStyleBackColor = true;
            // 
            // activeYGyro
            // 
            this.activeYGyro.AutoSize = true;
            this.activeYGyro.Location = new System.Drawing.Point(112, 95);
            this.activeYGyro.Name = "activeYGyro";
            this.activeYGyro.Size = new System.Drawing.Size(58, 17);
            this.activeYGyro.TabIndex = 77;
            this.activeYGyro.Text = "Y Gyro";
            this.activeYGyro.UseVisualStyleBackColor = true;
            // 
            // activeZGyro
            // 
            this.activeZGyro.AutoSize = true;
            this.activeZGyro.Location = new System.Drawing.Point(197, 95);
            this.activeZGyro.Name = "activeZGyro";
            this.activeZGyro.Size = new System.Drawing.Size(58, 17);
            this.activeZGyro.TabIndex = 78;
            this.activeZGyro.Text = "Z Gyro";
            this.activeZGyro.UseVisualStyleBackColor = true;
            // 
            // activeXMag
            // 
            this.activeXMag.AutoSize = true;
            this.activeXMag.Location = new System.Drawing.Point(28, 121);
            this.activeXMag.Name = "activeXMag";
            this.activeXMag.Size = new System.Drawing.Size(57, 17);
            this.activeXMag.TabIndex = 82;
            this.activeXMag.Text = "X Mag";
            this.activeXMag.UseVisualStyleBackColor = true;
            // 
            // activeXGyro
            // 
            this.activeXGyro.AutoSize = true;
            this.activeXGyro.Location = new System.Drawing.Point(28, 95);
            this.activeXGyro.Name = "activeXGyro";
            this.activeXGyro.Size = new System.Drawing.Size(58, 17);
            this.activeXGyro.TabIndex = 76;
            this.activeXGyro.Text = "X Gyro";
            this.activeXGyro.UseVisualStyleBackColor = true;
            // 
            // activeZAccel
            // 
            this.activeZAccel.AutoSize = true;
            this.activeZAccel.Location = new System.Drawing.Point(197, 72);
            this.activeZAccel.Name = "activeZAccel";
            this.activeZAccel.Size = new System.Drawing.Size(63, 17);
            this.activeZAccel.TabIndex = 81;
            this.activeZAccel.Text = "Z Accel";
            this.activeZAccel.UseVisualStyleBackColor = true;
            // 
            // activeYAccel
            // 
            this.activeYAccel.AutoSize = true;
            this.activeYAccel.Location = new System.Drawing.Point(112, 72);
            this.activeYAccel.Name = "activeYAccel";
            this.activeYAccel.Size = new System.Drawing.Size(63, 17);
            this.activeYAccel.TabIndex = 80;
            this.activeYAccel.Text = "Y Accel";
            this.activeYAccel.UseVisualStyleBackColor = true;
            // 
            // activeXAccel
            // 
            this.activeXAccel.AutoSize = true;
            this.activeXAccel.Location = new System.Drawing.Point(28, 72);
            this.activeXAccel.Name = "activeXAccel";
            this.activeXAccel.Size = new System.Drawing.Size(63, 17);
            this.activeXAccel.TabIndex = 79;
            this.activeXAccel.Text = "X Accel";
            this.activeXAccel.UseVisualStyleBackColor = true;*/
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(293, 588);
            this.tabControl1.TabIndex = 78;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(117, 185);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(46, 13);
            this.label23.TabIndex = 13;
            this.label23.Text = "ACC_Y";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(212, 185);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(46, 13);
            this.label24.TabIndex = 14;
            this.label24.Text = "ACC_Z";
            this.label24.Click += new System.EventHandler(this.label24_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(23, 138);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 13);
            this.label25.TabIndex = 15;
            this.label25.Text = "MAG_X";
            this.label25.Click += new System.EventHandler(this.label25_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(117, 138);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(49, 13);
            this.label26.TabIndex = 16;
            this.label26.Text = "MAG_Y";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(212, 138);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(49, 13);
            this.label27.TabIndex = 17;
            this.label27.Text = "MAG_Z";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(23, 83);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(57, 13);
            this.label28.TabIndex = 18;
            this.label28.Text = "GYRO_X";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(117, 83);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(57, 13);
            this.label29.TabIndex = 19;
            this.label29.Text = "GYRO_Y";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(212, 80);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(57, 13);
            this.label30.TabIndex = 20;
            this.label30.Text = "GYRO_Z";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(23, 41);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(39, 13);
            this.label31.TabIndex = 21;
            this.label31.Text = "ROLL";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(117, 41);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(44, 13);
            this.label32.TabIndex = 22;
            this.label32.Text = "PITCH";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(212, 41);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(35, 13);
            this.label33.TabIndex = 23;
            this.label33.Text = "YAW";
            // 
            // LONGITUDE
            // 
            this.LONGITUDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LONGITUDE.Location = new System.Drawing.Point(10, 250);
            this.LONGITUDE.Name = "LONGITUDE";
            this.LONGITUDE.Size = new System.Drawing.Size(70, 20);
            this.LONGITUDE.TabIndex = 24;
            this.LONGITUDE.Text = "0.0";
            this.LONGITUDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LONGITUDE.WordWrap = false;
            // 
            // LATITUDE
            // 
            this.LATITUDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LATITUDE.Location = new System.Drawing.Point(106, 250);
            this.LATITUDE.Name = "LATITUDE";
            this.LATITUDE.Size = new System.Drawing.Size(70, 20);
            this.LATITUDE.TabIndex = 25;
            this.LATITUDE.Text = "0.0";
            this.LATITUDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LATITUDE.WordWrap = false;
            // 
            // ALTITUDE
            // 
            this.ALTITUDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ALTITUDE.Location = new System.Drawing.Point(200, 250);
            this.ALTITUDE.Name = "ALTITUDE";
            this.ALTITUDE.Size = new System.Drawing.Size(70, 20);
            this.ALTITUDE.TabIndex = 26;
            this.ALTITUDE.Text = "0.0";
            this.ALTITUDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ALTITUDE.WordWrap = false;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(6, 234);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(79, 13);
            this.label34.TabIndex = 27;
            this.label34.Text = "LONGITUDE";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(108, 234);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(68, 13);
            this.label35.TabIndex = 28;
            this.label35.Text = "LATITUDE";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(201, 234);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(68, 13);
            this.label36.TabIndex = 29;
            this.label36.Text = "ALTITUDE";
            // 
            // InfoDump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 641);
            this.Controls.Add(this.gyroGraph);
            this.Controls.Add(this.accelGraph);
            this.Controls.Add(this.magGraph);
            this.Controls.Add(this.angleGraph);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.InvalidateAllButton);
            this.Controls.Add(this.SynchButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "InfoDump";
            this.Text = "AHRS Property Editor";
            this.Load += new System.EventHandler(this.InfoDump_Load);
            this.tabPage2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.Button SynchButton;
        private System.Windows.Forms.Button InvalidateAllButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton listenModeEnabled;

        private System.Windows.Forms.Label label13;
        private ZedGraph.ZedGraphControl angleGraph;
        private ZedGraph.ZedGraphControl angularRateGraph;
        private ZedGraph.ZedGraphControl magGraph;
        private ZedGraph.ZedGraphControl accelGraph;
        private ZedGraph.ZedGraphControl gyroGraph;
        private System.Windows.Forms.Timer timer1;

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;


        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox activeRoll;
        private System.Windows.Forms.CheckBox activePitch;
        private System.Windows.Forms.CheckBox activeYaw;
        private System.Windows.Forms.CheckBox activeZMag;
        private System.Windows.Forms.CheckBox activeYMag;
        private System.Windows.Forms.CheckBox activeYGyro;
        private System.Windows.Forms.CheckBox activeZGyro;
        private System.Windows.Forms.CheckBox activeXMag;
        private System.Windows.Forms.CheckBox activeXGyro;
        private System.Windows.Forms.CheckBox activeZAccel;
        private System.Windows.Forms.CheckBox activeYAccel;
        private System.Windows.Forms.CheckBox activeXAccel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox IMU_MAG_Z;
        private System.Windows.Forms.TextBox IMU_MAG_Y;
        private System.Windows.Forms.TextBox IMU_MAG_X;
        private System.Windows.Forms.TextBox IMU_GYRO_Z;
        private System.Windows.Forms.TextBox IMU_GYRO_Y;
        private System.Windows.Forms.TextBox IMU_GYRO_X;
        private System.Windows.Forms.TextBox IMU_ACC_Z;
        private System.Windows.Forms.TextBox IMU_ACC_Y;
        private System.Windows.Forms.TextBox IMU_ACC_X;
        private System.Windows.Forms.TextBox IMU_ROLL;
        private System.Windows.Forms.TextBox IMU_PITCH;
        private System.Windows.Forms.TextBox IMU_YAW1;
        private System.Windows.Forms.Label label207;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox ALTITUDE;
        private System.Windows.Forms.TextBox LATITUDE;
        private System.Windows.Forms.TextBox LONGITUDE;

    }
}