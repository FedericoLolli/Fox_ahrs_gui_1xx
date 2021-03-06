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
    public partial class EKFset : Form
    {

        public EKFset(AHRS sensor)
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
       void confReceivedEventHandler(int active_channels)
        {
            quatinit00.Text = sensor.init_quat[0].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            quatinit01.Text = sensor.init_quat[1].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            quatinit02.Text = sensor.init_quat[2].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            quatinit03.Text = sensor.init_quat[3].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            Qbias00.Text = sensor.Q_bias[0].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            Qbias01.Text = sensor.Q_bias[1].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            Qbias02.Text = sensor.Q_bias[2].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);



            Gain00.Text = sensor.gain[0].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            Gain01.Text = sensor.gain[1].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            Gain02.Text = sensor.gain[2].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            Gain03.Text = sensor.gain[3].ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            filter_type.Text = sensor.filter_type.ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            dev_std_mag.Text = sensor.mag_covariance.ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            dev_std_acc.Text = sensor.accel_covariance.ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            conolution_time.Text = sensor.convolution_time.ToString(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
        }

        private void EKFsetAlignmentCommitButton_Click(object sender, EventArgs e)
        {


            sensor.init_quat[0] = (float)Double.Parse(quatinit00.Text,System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.init_quat[1] = (float)Double.Parse(quatinit01.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.init_quat[2] = (float)Double.Parse(quatinit02.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.init_quat[3] = (float)Double.Parse(quatinit03.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            sensor.Q_bias[0] = (float)Double.Parse(Qbias00.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.Q_bias[1] = (float)Double.Parse(Qbias01.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.Q_bias[2] = (float)Double.Parse(Qbias02.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            sensor.gain[0] = (float)Double.Parse(Gain00.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.gain[1] = (float)Double.Parse(Gain01.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.gain[2] = (float)Double.Parse(Gain02.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.gain[3] = (float)Double.Parse(Gain03.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            sensor.filter_type = (float)Double.Parse(filter_type.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.mag_covariance = (float)Double.Parse(dev_std_mag.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.accel_covariance = (float)Double.Parse(dev_std_acc.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            sensor.convolution_time = (float)Double.Parse(conolution_time.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            sensor.synch();
        }

        private void flashCommitButton_Click(object sender, EventArgs e)
        {
            sensor.WriteToFlash();
           
        }
  

    }
}
