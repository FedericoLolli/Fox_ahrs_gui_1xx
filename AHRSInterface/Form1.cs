﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using x_IMU_IMU_and_AHRS_Algorithms;

using AHRSInterface;
namespace AHRSInterface
{
    public partial class AHRSInterface : Form
    {

        delegate void AppendTextCallback(string text, Color text_color);
        Form_3Dcuboid form_3DcuboidA = new Form_3Dcuboid();
        public AHRSInterface()
        {
            InitializeComponent();

            initializeSerialPort();

            sensor = new AHRS();

            // Set up event handlers
            sensor.PacketTimeoutEvent += new StateDelegate(TimeoutEventHandler);
            sensor.PacketReceivedEvent += new PacketDelegate(PacketReceivedEventHandler);
            sensor.DataReceivedEvent += new DataReceivedDelegate(DataReceivedEventHandler);
            sensor.PacketSentEvent += new PacketDelegate(PacketSentEventHandler);
            sensor.COMFailedEvent += new COMFailedDelegate(COMFailedEventHandler);

            renderTimer = new Timer();

            renderTimer.Interval = 10;
            renderTimer.Enabled = true;
            renderTimer.Tick += new System.EventHandler(OnRenderTimerTick);


            //Form_3Dcuboid form_3DcuboidA = new Form_3Dcuboid(new string[] { "RightInv.png", "LeftInv.png", "BackInv.png", "FrontInv.png", "TopInv.png", "BottomInv.png" });
            form_3DcuboidA.Text += " A";
            BackgroundWorker backgroundWorkerA = new BackgroundWorker();

            backgroundWorkerA.DoWork += new DoWorkEventHandler(delegate { form_3DcuboidA.ShowDialog(); });

            backgroundWorkerA.RunWorkerAsync();


        }

        public void OnRenderTimerTick(object source, EventArgs e)
        {
           // cube.Invalidate();
      
           //using System;

        }

        private void initializeSerialPort()
        {
            serialPort = new SerialPort();

            foreach (string s in SerialPort.GetPortNames())
            {
                serialPortCOMBox.Items.Add(s);
            }
            if (serialPortCOMBox.Items.Count == 0)
            {
                serialPortCOMBox.Items.Add("No Ports Avaliable");
                serialPortCOMBox.Enabled = false;
                serialConnectButton.Enabled = false;
            }

            serialPortCOMBox.SelectedIndex = 0;
            baudSelectBox.Items.Add(115200);
            baudSelectBox.Items.Add(38400);
            baudSelectBox.SelectedIndex = 0;
        }

        private void AHRSInterface_Load(object sender, EventArgs e)
        {
            
        }

        // Define AHRS object
        AHRS sensor;
        InfoDump data_display;

        AHRSLog dataLog;
        Timer renderTimer;

        SerialPort serialPort;

        /* **********************************************************************************
         * 
         * Function: void TimeoutEventHandler
         * Inputs: PName packet_type, int flags
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Handles timeout events generated by the AHRS class - a timeout event occurs if the
         * AHRS class attempts to communicate with the AHRS device and receives no response.
         * 
         * *********************************************************************************/
        void TimeoutEventHandler(StateName packet_type, int flags)
        {
            string message;

            /*message = "Timeout: ";
            message += System.Enum.Format(typeof(StateName), packet_type, "G");
            message += "\r\n";

            AppendStatusText(message, Color.Red);*/
        }

        void COMFailedEventHandler()
        {
//            AppendStatusText("Serial COM failed\r\n", Color.Red);
        }

        /* **********************************************************************************
        * 
        * Function: void PacketReceivedEventHandler
        * Inputs: PName packet_type, int flags
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles PacketReceived events generated by the AHRS.
        * 
        * *********************************************************************************/
        void PacketReceivedEventHandler(PName packet_type, int flags)
        {
   
        }

        /* **********************************************************************************
        * 
        * Function: void PacketSentEventHandler
        * Inputs: PName packet_type, int flags
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles PacketReceived events generated by the AHRS.
        * 
        * *********************************************************************************/
        void PacketSentEventHandler(PName packet_type, int flags)
        {
          
        }

        /* **********************************************************************************
        * 
        * Function: void DataReceivedEventHandler
        * Inputs: None
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles DataReceived events generated by the AHRS object
        * 
        * *********************************************************************************/
        void DataReceivedEventHandler(int active_channels)
        {
//            AppendStatusText("Got Data\r\n", Color.Green);
            // Convert from NED to XYZ
            float[] num = new float[9] { 
                  /*(1 - 2 * (float)sensor.q2 * (float)sensor.q2 - 2 * (float)sensor.q3 * (float)sensor.q3), (2 * (float)sensor.q1 * (float)sensor.q2 - 2 * (float)sensor.q3 * (float)sensor.q4), (2 * (float)sensor.q1 * (float)sensor.q3 + 2 * (float)sensor.q2 * (float)sensor.q4),
                 (2*(float)sensor.q1*(float)sensor.q2+2*(float)sensor.q3*(float)sensor.q4), (1-2*(float)sensor.q1*(float)sensor.q1-2*(float)sensor.q3*(float)sensor.q3), (2*(float)sensor.q2*(float)sensor.q3-2*(float)sensor.q1*(float)sensor.q4),
                 (2*(float)sensor.q1*(float)sensor.q3-2*(float)sensor.q2*(float)sensor.q4), (2*(float)sensor.q2*(float)sensor.q3+2*(float)sensor.q1*(float)sensor.q4),(1-2*(float)sensor.q1*(float)sensor.q1-2*(float)sensor.q2*(float)sensor.q2) };*/


                 /*   (1-2*(float)sensor.q1*(float)sensor.q1-2*(float)sensor.q2*(float)sensor.q2) , (2*(float)sensor.q1*(float)sensor.q3-2*(float)sensor.q2*(float)sensor.q4),(2*(float)sensor.q2*(float)sensor.q3+2*(float)sensor.q1*(float)sensor.q4),
              (2 * (float)sensor.q1 * (float)sensor.q3 + 2 * (float)sensor.q2 * (float)sensor.q4),   (1 - 2 * (float)sensor.q2 * (float)sensor.q2 - 2 * (float)sensor.q3 * (float)sensor.q3), (2 * (float)sensor.q1 * (float)sensor.q2 - 2 * (float)sensor.q3 * (float)sensor.q4),  
                (2*(float)sensor.q2*(float)sensor.q3-2*(float)sensor.q1*(float)sensor.q4), (2*(float)sensor.q1*(float)sensor.q2+2*(float)sensor.q3*(float)sensor.q4),  (1-2*(float)sensor.q1*(float)sensor.q1-2*(float)sensor.q3*(float)sensor.q3),*/
              -  (1-2*(float)sensor.q1*(float)sensor.q1-2*(float)sensor.q2*(float)sensor.q2) ,-(2*(float)sensor.q2*(float)sensor.q3+2*(float)sensor.q1*(float)sensor.q4), (-2*(float)sensor.q1*(float)sensor.q3-2*(float)sensor.q2*(float)sensor.q4),
              (2 * (float)sensor.q1 * (float)sensor.q3 + 2 * (float)sensor.q2 * (float)sensor.q4),  (2 * (float)sensor.q1 * (float)sensor.q2 - 2 * (float)sensor.q3 * (float)sensor.q4), (1 - 2 * (float)sensor.q2 * (float)sensor.q2 - 2 * (float)sensor.q3 * (float)sensor.q3),  
              (2*(float)sensor.q2*(float)sensor.q3-2*(float)sensor.q1*(float)sensor.q4),(1-2*(float)sensor.q1*(float)sensor.q1-2*(float)sensor.q3*(float)sensor.q3),(2*(float)sensor.q1*(float)sensor.q2+2*(float)sensor.q3*(float)sensor.q4),  
            };
                
               
              
                  
            form_3DcuboidA.RotationMatrix = num;

            //cube.Yaw = (float)sensor.yawAngle * 3.14159f / 180f;
           // cube.Pitch = (float)sensor.pitchAngle * 3.14159f / 180f;
            //cube.Roll = (float)sensor.rollAngle * 3.14159f / 180f;
 
        }

        private void AppendStatusText(string text, Color text_color)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusBox.InvokeRequired)
            {
                AppendTextCallback d = new AppendTextCallback(AppendStatusText);
                this.Invoke(d, new object[] { text, text_color });
            }
            else
            {
                this.statusBox.SelectionColor = text_color;
                this.statusBox.AppendText(text);
                this.statusBox.ScrollToCaret();
            }
        }

        private void SynchButton_Click(object sender, EventArgs e)
        {
            sensor.synch();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            data_display = new InfoDump(sensor);

            data_display.Show();
        }

        // "Connect" button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            // Connect to the serial port
            if (!sensor.connect(serialPortCOMBox.SelectedItem.ToString(), baudSelectBox.SelectedItem.ToString()))
            {
                AppendStatusText("Failed to connect to serial port\r\n", Color.Red);
            }
            else
            {
                AppendStatusText("Connected to " + serialPortCOMBox.SelectedItem.ToString() + "\r\n", Color.Blue);

                serialDisconnectButton.Enabled = true;
                serialConnectButton.Enabled = false;
                opend3dcube.Enabled = true;
                configToolStripMenuItem.Enabled = true;
                logDataToolstripItem.Enabled = true;

                sensor.synch();
            }
        }

        private void serialDisconnectButton_Click(object sender, EventArgs e)
        {
            sensor.Disconnect();
            sensor.Invalidate();

            serialDisconnectButton.Enabled = false;
            serialConnectButton.Enabled = true;
            opend3dcube.Enabled = false;
            configToolStripMenuItem.Enabled = false;
            logDataToolstripItem.Enabled = false;

            AppendStatusText("Disconnected from serial port\r\n", Color.Blue);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void logDataToolstripItem_Click(object sender, EventArgs e)
        {
            dataLog = new AHRSLog(sensor);

            dataLog.Show();
        }

        private void directXCube1_Load(object sender, EventArgs e)
        {

        }

        private void baudSelectBox_Click(object sender, EventArgs e)
        {

        }

        private void statusBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void opend3dcube_Click(object sender, EventArgs e)
        {
            if (!form_3DcuboidA.Visible)
            {
                //form_3DcuboidA = null;
                form_3DcuboidA.Refresh();
                form_3DcuboidA.Text += " A";
                BackgroundWorker backgroundWorkerA = new BackgroundWorker();
                backgroundWorkerA.DoWork += new DoWorkEventHandler(delegate { form_3DcuboidA.ShowDialog(); });
                backgroundWorkerA.RunWorkerAsync();
            }
            
        }

      
    }
}
