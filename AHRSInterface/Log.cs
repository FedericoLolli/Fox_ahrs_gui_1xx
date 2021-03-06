﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AHRSInterface
{
    public partial class AHRSLog : Form
    {
        public AHRSLog(AHRS sensor)
        {
            InitializeComponent();

            loggingEnabled = false;

            this.sensor = sensor;

            sensor.DataReceivedEvent += new DataReceivedDelegate(DataReceivedEventHandler);
        }

        ~AHRSLog()
        {
            if (loggingEnabled)
            {
                logfile.Close();
            }

            sensor.DataReceivedEvent -= new DataReceivedDelegate(DataReceivedEventHandler);
        }

        StreamWriter logfile;
        bool loggingEnabled;
        SaveFileDialog saveFileDialog;

        AHRS sensor;

        DateTime startTime;
        DateTime currentTime;

        void DataReceivedEventHandler(int active_channels)
        {
            if (loggingEnabled)
            {
                currentTime = DateTime.Now;

                TimeSpan deltaT = currentTime - startTime;

                try
                {
                    logfile.Write(deltaT.TotalMilliseconds);
                    logfile.Write("\t");
                    logfile.Write(sensor.yawAngle);
                    logfile.Write("\t");
                    logfile.Write(sensor.pitchAngle);
                    logfile.Write("\t");
                    logfile.Write(sensor.rollAngle);
                    logfile.Write("\t");
                    logfile.Write(sensor.q1s);
                    logfile.Write("\t");
                    logfile.Write(sensor.q2s);
                    logfile.Write("\t");
                    logfile.Write(sensor.q3s);
                    logfile.Write("\t");
                    logfile.Write(sensor.q4s);
                    logfile.Write("\t");
                    logfile.Write(sensor.magX);
                    logfile.Write("\t");
                    logfile.Write(sensor.magY);
                    logfile.Write("\t");
                    logfile.Write(sensor.magZ);
                    logfile.Write("\t");
                    logfile.Write(sensor.gyroX);
                    logfile.Write("\t");
                    logfile.Write(sensor.gyroY);
                    logfile.Write("\t");
                    logfile.Write(sensor.gyroZ);
                    logfile.Write("\t");
                    logfile.Write(sensor.accelX);
                    logfile.Write("\t");
                    logfile.Write(sensor.accelY);
                    logfile.Write("\t");
                    logfile.Write(sensor.accelZ);
                    logfile.Write("\t");
                    logfile.Write(sensor.latitude);
                    logfile.Write("\t");
                    logfile.Write(sensor.longitude);
                    logfile.Write("\t");
                    logfile.Write(sensor.velGPS);
                    logfile.Write("\t");
                    logfile.Write(sensor.altitudine);
                    logfile.Write("\t");
                    logfile.Write("\n");

                }
                catch
                {
                }
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "LOG file|*.log|All Files|*.*";
            saveFileDialog.Title = "Set LOG file name";
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Open the file
                if (saveFileDialog.FileName != "")
                {
                    logfile = new StreamWriter(saveFileDialog.FileName);

                    // Change buttons to allow logging
                    startLoggingButton.Enabled = true;

                    logfile.WriteLine("TIME\tYAW\tPITCH\tROLL\tq1\tq2\tq3\tq4\tMAG_X\tMAG_Y\tMAG_Z\tGYRO_X\tGYRO_Y\tGYRO_Z\tACCEL_X\tACCEL_Y\tACCEL_Z\tLATITUDE\tLONGITUDE\tVEL_GPS\tALTITUDE\t");

                    logfile.Close();

                    // Record time as start time
                    startTime = DateTime.Now;

                    filenameBox.Text = saveFileDialog.FileName;
                }
            }
        }

        private void startLoggingButton_Click(object sender, EventArgs e)
        {
            loggingEnabled = true;

            stopLoggingButton.Enabled = true;
            startLoggingButton.Enabled = false;
            browseButton.Enabled = false;

            logfile = new StreamWriter(saveFileDialog.FileName, true);        
        }

        private void stopLoggingButton_Click(object sender, EventArgs e)
        {
            loggingEnabled = false;

            stopLoggingButton.Enabled = false;
            startLoggingButton.Enabled = true;
            browseButton.Enabled = true;

            logfile.Close();
        }


    }
}
