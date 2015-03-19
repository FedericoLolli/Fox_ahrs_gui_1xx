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
    public partial class AHRSupload : Form
    {
        public AHRSupload(AHRS sensor)
        {
            InitializeComponent();

            this.sensor = sensor;

        }

        ~AHRSupload()
        {

        }
        AHRS sensor;
        OpenFileDialog openFileDialog;

        DialogResult resultdialog; 

        private void browseButton_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "tar.gz file|*.tar|All Files|*.*";
            openFileDialog.Title = "Set HEX file name";
           resultdialog= openFileDialog.ShowDialog();
           // openFileDialog.OverwritePrompt = true;

        }

        private void startLoggingButton_Click(object sender, EventArgs e)
        {

           // if (resultdialog == DialogResult.OK)
            if (1 == 1)
            {
                // Open the file
                //if (openFileDialog.FileName != "")
                if(1==1)
                {

                   // byte[] caracter = File.ReadAllBytes(openFileDialog.FileName);

                    //long lungezza = caracter.LongCount();

                   // byte[] packet = new byte[20];
                   // byte[] packetlen = new byte[4];
                    //int j = 0, i = 0;
                    //byte_conversion_array ftob = new byte_conversion_array();

                    //int lungezzaint = (int)lungezza;
                    //packetlen[0] = (byte)((lungezzaint >> 24) & 0x000000FF);
                    //packetlen[1] = (byte)((lungezzaint >> 16) & 0x000000FF);
                    //packetlen[2] = (byte)((lungezzaint >> 8) & 0x000000FF);
                    //packetlen[3] = (byte)(lungezzaint & 0x000000FF);

                    sensor.updatefirmware();
                    System.Threading.Thread.Sleep(1000);

                    //sensor.sendfwPacket(packetlen, 4);
                    //System.Threading.Thread.Sleep(100);
/*
                    for (i = 0; i <= lungezza + 11; i = i + 10)
                    {
                        j = 0;
                        while ((j <= (10 - 1)) && ((i + j) <= (lungezza - 1)))
                        {
                            packet[j] = (byte)caracter[i + j];
                            j++;
                        }
                        sensor.sendfwPacket(packet, 10);
                        //  System.Threading.Thread.Sleep(1);

                    }

                    lungezza = 100;
                    for (i = 0; i <= lungezza + 11; i = i + 10)
                    {
                        j = 0;
                        while ((j <= (10 - 1)) && ((i + j) <= (lungezza - 1)))
                        {
                            packet[j] = (byte)0x00;
                            j++;
                        }
                        sensor.sendfwPacket(packet, 10);
                        //  System.Threading.Thread.Sleep(1);

                    }*/


                }
            }
     
        }
        






    }
}
