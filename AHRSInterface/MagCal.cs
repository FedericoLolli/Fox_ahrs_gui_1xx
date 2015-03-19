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
    public partial class MagCal : Form
    {
        delegate void displayLoggingPercentCallback(float percent_complete);
        
        public MagCal(AHRS sensor)
        {
            int i=0;

            InitializeComponent();

            this.sensor = sensor;

            // Add DataReceived event handler.
            sensor.DataReceivedEvent += new DataReceivedDelegate(DataReceivedEventHandler);
            sensor.confReceivedEvent += new confReceivedDelegate(confReceivedEventHandler);
            confReceivedEventHandler(i);

            data_collection_enabled = false;
            next_data_index = 0;

            loggedData = new double[SAMPLES, 3];
            threshold = 1.5 * 3.14159 / 180;

            bias = new double[3];

            calMat = new double[3, 3];
            calMat[0, 0] = 1.0;
            calMat[1, 1] = 1.0;
            calMat[2, 2] = 1.0;

            D = new GeneralMatrix(SAMPLES, 10);

            for (i = 0; i < SAMPLES; i++)
            {
                loggedData[i,0] = 0;
                loggedData[i, 1] = 0;
                loggedData[i, 2] = 0;
            }
        }

        int next_data_index;
        private AHRS sensor;

        private bool data_collection_enabled;
        
        StreamWriter logfile;
        SaveFileDialog saveFileDialog;

        const int SAMPLES = 1000;
        
        double[,] loggedData;
        double[] bias;
        double[,] calMat;
        GeneralMatrix D;
        double threshold;


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
            float percent_complete;

            if (data_collection_enabled)
            {
                // Get the yaw, pitch, and roll angle of the most recently logged data.
                // Determine whether the angle is sufficiently different to justify logging it.
                // If so, log it.  Update the data collection progress bars as neccessary.
                if (isUniqueVector(sensor.magX, sensor.magY, sensor.magZ))
                {
                    loggedData[next_data_index, 0] = sensor.magX;
                    loggedData[next_data_index, 1] = sensor.magY;
                    loggedData[next_data_index, 2] = sensor.magZ;

                    next_data_index++;

                    if (next_data_index == SAMPLES)
                    {
                        data_collection_enabled = false;

                        percent_complete = 100;
                        displayLoggingPercent(percent_complete);
                    }
                    else
                    {
                        // Update data collection bar
                        percent_complete = ((float)dataProgressBar.Maximum / (float)SAMPLES) * (float)next_data_index;

                        displayLoggingPercent(percent_complete);
                    }
                }
            }
            
        }

        private void displayLoggingPercent(float percent_complete)
        {
            if (this.magCalStatusText.InvokeRequired)
            {
                displayLoggingPercentCallback d = new displayLoggingPercentCallback(displayLoggingPercent);
                this.Invoke(d, new object[] { percent_complete });
            }
            else
            {
                if (percent_complete == 100)
                {
                    dataProgressBar.Value = (int)percent_complete;
                    magCalStatusText.Text = "Done.";
                    computeMagCalButton.Enabled = true;
                    startDataCollectionButton.Enabled = false;
                    stopDataCollectionButton.Enabled = false;
                    writeToFileButton.Enabled = true;
                }
                else
                {
                    dataProgressBar.Value = (int)percent_complete;
                    magCalStatusText.Text = "Collecting Data: " + percent_complete.ToString() + " %";
                }
            }
        }

        private bool isUniqueVector(double magX, double magY, double magZ)
        {
            int i;

            bool unique_data = true;

            for (i = 0; i < next_data_index; i++)
            {
                double dot_product = loggedData[i, 0] * magX + loggedData[i, 1] * magY + loggedData[i, 2] * magZ;
                double norm1 = Math.Sqrt(magX * magX + magY * magY + magZ * magZ);
                double norm2 = Math.Sqrt(loggedData[i, 0] * loggedData[i, 0] + loggedData[i, 1] * loggedData[i, 1] + loggedData[i, 2] * loggedData[i, 2]);
                double angle = Math.Acos(dot_product / (norm1 * norm2));

                if (Math.Abs(angle) < threshold)
                {
                    unique_data = false;
                }
            }

            return unique_data;
        }

        private void startDataCollectionButton_Click(object sender, EventArgs e)
        {
            data_collection_enabled = true;
            startDataCollectionButton.Enabled = false;
            stopDataCollectionButton.Enabled = true;

            magCalStatusText.Text = "Collecting Data";
        }

        private void stopDataCollectionButton_Click(object sender, EventArgs e)
        {
            data_collection_enabled = false;
            startDataCollectionButton.Enabled = true;
            stopDataCollectionButton.Enabled = false;

            magCalStatusText.Text = "Inactive";
        }

        private void magCalResetButton_Click(object sender, EventArgs e)
        {
            data_collection_enabled = false;
            startDataCollectionButton.Enabled = true;
            stopDataCollectionButton.Enabled = false;
            computeMagCalButton.Enabled = false;
            writeToFileButton.Enabled = false;

            calStatusText.Text = "Inactive";
            flashCommitButton.Enabled = false;
            magAlignmentCommitButton.Enabled = false;

            dataProgressBar.Value = 0;
            next_data_index = 0;

            magCalStatusText.Text = "Inactive";
        }

        private void computeMagCalButton_Click(object sender, EventArgs e)
        {
            int i,j;

            calStatusText.Text = "Computing Calibration...";

            // Construct D matrix
            // D = [x.^2, y.^2, z.^2, x.*y, x.*z, y.*z, x, y, z, ones(N,1)];
            for (i = 0; i < SAMPLES; i++ )
            {
                // x^2 term
                D.SetElement(i,0, loggedData[i,0]*loggedData[i,0]);

                // y^2 term
                D.SetElement(i,1,loggedData[i,1]*loggedData[i,1]);

                // z^2 term
                D.SetElement(i, 2, loggedData[i, 2] * loggedData[i, 2]);

                // x*y term
                D.SetElement(i,3,loggedData[i,0]*loggedData[i,1]);

                // x*z term
                D.SetElement(i,4,loggedData[i,0]*loggedData[i,2]);

                // y*z term
                D.SetElement(i,5,loggedData[i,1]*loggedData[i,2]);

                // x term
                D.SetElement(i,6,loggedData[i,0]);

                // y term
                D.SetElement(i,7,loggedData[i,1]);

                // z term
                D.SetElement(i,8,loggedData[i,2]);

                // Constant term
                D.SetElement(i,9,1);
            }

            // QR=triu(qr(D))
            QRDecomposition QR = new QRDecomposition(D);
            // [U,S,V] = svd(D)
            SingularValueDecomposition SVD = new SingularValueDecomposition(QR.R);
            GeneralMatrix V = SVD.GetV();

            GeneralMatrix A = new GeneralMatrix(3, 3);

            double[] p = new double[V.RowDimension];
            
            for (i = 0; i < V.RowDimension; i++ )
            {
                p[i] = V.GetElement(i,V.ColumnDimension-1);
            }

            /*
            A = [p(1) p(4)/2 p(5)/2;
            p(4)/2 p(2) p(6)/2; 
            p(5)/2 p(6)/2 p(3)];
             */

            if (p[0] < 0)
            {
                for (i = 0; i < V.RowDimension; i++)
                {
                    p[i] = -p[i];
                }
            }

            A.SetElement(0,0,p[0]);
            A.SetElement(0,1,p[3]/2);
            A.SetElement(1,2,p[4]/2);

            A.SetElement(1,0,p[3]/2);
            A.SetElement(1,1,p[1]);
            A.SetElement(1,2,p[5]/2);

            A.SetElement(2,0,p[4]/2);
            A.SetElement(2,1,p[5]/2);
            A.SetElement(2,2,p[2]);
            
            CholeskyDecomposition Chol = new CholeskyDecomposition(A);
            GeneralMatrix Ut = Chol.GetL();
            GeneralMatrix U = Ut.Transpose();

            double[] bvect = {p[6]/2,p[7]/2,p[8]/2};
            double d = p[9];
            GeneralMatrix b = new GeneralMatrix(bvect,3);
            
            GeneralMatrix v = Ut.Solve(b);
            
            double vnorm_sqrd = v.GetElement(0,0)*v.GetElement(0,0) + v.GetElement(1,0)*v.GetElement(1,0) + v.GetElement(2,0)*v.GetElement(2,0);
            double s = 1/Math.Sqrt(vnorm_sqrd - d);

            GeneralMatrix c = U.Solve(v);
            for (i = 0; i < 3; i++)
            {
                c.SetElement(i, 0, -c.GetElement(i, 0));
            }

            U = U.Multiply(s);

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    calMat[i, j] = U.GetElement(i, j);
                }
            }

            for (i = 0; i < 3; i++)
            {
                bias[i] = c.GetElement(i, 0);
            }

            magAlignment00.Text = calMat[0, 0].ToString();
            magAlignment01.Text = calMat[0, 1].ToString();
            magAlignment02.Text = calMat[0, 2].ToString();

            magAlignment10.Text = calMat[1, 0].ToString();
            magAlignment11.Text = calMat[1, 1].ToString();
            magAlignment12.Text = calMat[1, 2].ToString();

            magAlignment20.Text = calMat[2, 0].ToString();
            magAlignment21.Text = calMat[2, 1].ToString();
            magAlignment22.Text = calMat[2, 2].ToString();

            biasX.Text = bias[0].ToString();
            biasY.Text = bias[1].ToString();
            biasZ.Text = bias[2].ToString();

            calStatusText.Text = "Done";
            flashCommitButton.Enabled = true;
            magAlignmentCommitButton.Enabled = true;
        }

        private void confReceivedEventHandler(int i)
        {

            float[,] floatCalMat = new float[3, 3];
            floatCalMat = sensor.mag_cal;




            magAlignment00.Text = floatCalMat[0, 0].ToString();
            magAlignment01.Text = floatCalMat[0, 1].ToString();
            magAlignment02.Text = floatCalMat[0, 2].ToString();

            magAlignment10.Text = floatCalMat[1, 0].ToString();
            magAlignment11.Text = floatCalMat[1, 1].ToString();
            magAlignment12.Text = floatCalMat[1, 2].ToString();

            magAlignment20.Text = floatCalMat[2, 0].ToString();
            magAlignment21.Text = floatCalMat[2, 1].ToString();
            magAlignment22.Text = floatCalMat[2, 2].ToString();

            biasX.Text = sensor.x_mag_bias.ToString();
            biasY.Text = sensor.y_mag_bias.ToString();
            biasZ.Text = sensor.z_mag_bias.ToString();
            calStatusText.Text = "Done";
            flashCommitButton.Enabled = true;
            magAlignmentCommitButton.Enabled = true;

        }

        private void writeToFileButton_Click(object sender, EventArgs e)
        {
            saveFileDialog = new SaveFileDialog();
            
            saveFileDialog.Filter = "LOG file|*.log|All Files|*.*";
            saveFileDialog.Title = "Set LOG file name";
            saveFileDialog.OverwritePrompt = true;

            saveFileDialog.FileOk += new CancelEventHandler(saveFileDialog_FileOK);

            saveFileDialog.ShowDialog();

        }

        private void saveFileDialog_FileOK(Object sender, EventArgs e)
        {
            int i;
            // Open the file
            if (saveFileDialog.FileName != "")
            {
                logfile = new StreamWriter(saveFileDialog.FileName);

                logfile.WriteLine("MAG_X\tMAG_Y\tMAG_Z");

                for (i = 0; i < SAMPLES; i++)
                {
                    logfile.WriteLine(loggedData[i, 0].ToString() + "\t" + loggedData[i, 1].ToString() + "\t" + loggedData[i, 2].ToString());
                }

                logfile.Close();

            }
        }

        private void magAlignmentCommitButton_Click(object sender, EventArgs e)
        {
            float[,] floatCalMat = new float[3, 3];
            floatCalMat[0, 0] = (float)calMat[0, 0];
            floatCalMat[0, 1] = (float)calMat[0, 1];
            floatCalMat[0, 2] = (float)calMat[0, 2];

            floatCalMat[1, 0] = (float)calMat[1, 0];
            floatCalMat[1, 1] = (float)calMat[1, 1];
            floatCalMat[1, 2] = (float)calMat[1, 2];

            floatCalMat[2, 0] = (float)calMat[2, 0];
            floatCalMat[2, 1] = (float)calMat[2, 1];
            floatCalMat[2, 2] = (float)calMat[2, 2];

            sensor.mag_cal = floatCalMat;
            sensor.x_mag_bias = (float)bias[0];
            sensor.y_mag_bias = (float)bias[1];
            sensor.z_mag_bias = (float)bias[2];

            sensor.synch();
        }

        private void flashCommitButton_Click(object sender, EventArgs e)
        {
            
            sensor.WriteToFlash();
           
        }

    }
}
