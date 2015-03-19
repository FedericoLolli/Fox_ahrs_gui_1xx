﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using DotNetMatrix;

namespace AHRSInterface
{
    public partial class ConfSet : Form
    {

        public ConfSet(AHRS sensor)
        {
            int i=0;

            InitializeComponent();

            this.sensor = sensor;
            // Add DataReceived event handler.
            sensor.confReceivedEvent += new confReceivedDelegate(confReceivedEventHandler);
            confReceivedEventHandler(i);
           
        }

       private AHRS sensor;


       /* **********************************************************************************
       * 
       * Function: void confReceivedEventHandler
       * Inputs: None
       * Outputs: None
       * Return Value: None
       * Dependencies: None
       * Description: 
       * 
       * Handles DataReceived events generated by the AHRS object
       * 
       * *********************************************************************************/
       void confReceivedEventHandler(int active_channels)
        {
           
            output_enable.Text =  sensor.output_enable.ToString();
            output_rate.Text =  sensor.output_rate.ToString();
            badurate.Text =  sensor.badurate.ToString();
            port.Text =  sensor.port.ToString();
            String str = new String(sensor.ip);
            ip.Text = str;

        }

        private void computegyroCalButton_Click(object sender, EventArgs e)
        {

            flashCommitButton.Enabled = true;
            confsetAlignmentCommitButton.Enabled = true;
        }


        private void confsetAlignmentCommitButton_Click(object sender, EventArgs e)
        {
           // string tmpstring new tmpstring;

            sensor.output_enable = (UInt16)  int.Parse(output_enable.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.port = (UInt16)int.Parse(port.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.output_rate = (UInt16)int.Parse(output_rate.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.badurate = (UInt16)int.Parse(badurate.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            String str = ip.Text;
            sensor.ip = str.ToCharArray();
          

            sensor.synch();
        }

        private void flashCommitButton_Click(object sender, EventArgs e)
        {
            
            sensor.WriteToFlash();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sensor.ResetToFactory();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sensor.AHRS_reboot();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sensor.EKF_Reset();
        }

    

    }
}
