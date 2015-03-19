using System;
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
    public partial class saveconfiguration : Form
    {
        public saveconfiguration(AHRS sensor)
        {
            InitializeComponent();
            this.sensor = sensor;

        }

        ~saveconfiguration()
        {

        }
        AHRS sensor; 
        StreamWriter logfile;
        SaveFileDialog saveFileDialog;

        private void browseButton_Click(object sender, EventArgs e)
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Conf file|*.conf|All Files|*.*";
            saveFileDialog.Title = "Set Configuration file name";
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Open the file
                if ((saveFileDialog.FileName != "")&&(sensor.configuration_String_r!= ""))
                {
                    logfile = new StreamWriter(saveFileDialog.FileName);

                    // Change buttons to allow logging
                    logfile.WriteLine("ahrs configuration file\n");
                    logfile.Write(sensor.configuration_String_r);
                    

                    logfile.Close();


                    filenameBox.Text = saveFileDialog.FileName;
                }
            }
        }


    }
}
