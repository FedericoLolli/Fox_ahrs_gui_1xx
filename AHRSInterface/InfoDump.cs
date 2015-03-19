using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

namespace AHRSInterface
{
    public partial class InfoDump : Form
    {

        delegate void displayActiveChannelsCallback();

        delegate void AppendTextCallback(string text, Color text_color);

        public InfoDump( AHRS sensor )
        {
            InitializeComponent();

            this.sensor = sensor;

            // Add new event handlers
            sensor.PacketReceivedEvent += new PacketDelegate(PacketReceivedEventHandler);
            sensor.PacketTimeoutEvent += new StateDelegate(TimeoutEventHandler);
            sensor.DataReceivedEvent += new DataReceivedDelegate(DataReceivedEventHandler);

            displayAHRSStates();

            initializeGraphs();

            timer1.Interval = 50;
            timer1.Start();            
            time = 0;
        }

        AHRS sensor;
        /*********************************************************************************
         * Variables for graph initialization
         * *******************************************************************************/
        GraphPane accelPane, gyroPane, magPane, anglePane, quaternionPane , positionPane;
        RollingPointPairList xAccelList, yAccelList, zAccelList, xGyroList, yGyroList, zGyroList;
        RollingPointPairList xMagList, yMagList, zMagList, yawList, pitchList, rollList;
        LineItem xAccelLine, yAccelLine, zAccelLine, xGyroLine, yGyroLine, zGyroLine;
        LineItem xMagLine, yMagLine, zMagLine, yawLine, pitchLine, rollLine;

        RollingPointPairList q0List, q1List, q2List, q3List, latList, lonList;
        LineItem q0Line, q1Line, q2Line, q3Line, latLine, lonLine;


        const int SENSOR_GRAPH_POINTS = 100;

        const int YAW_INDEX = 0;
        const int PITCH_INDEX = 1;
        const int ROLL_INDEX = 2;
        const int YAW_RATE_INDEX = 3;
        const int PITCH_RATE_INDEX = 4;
        const int ROLL_RATE_INDEX = 5;
        const int X_MAG_INDEX = 6;
        const int Y_MAG_INDEX = 7;
        const int Z_MAG_INDEX = 8;
        const int X_ACCEL_INDEX = 9;
        const int Y_ACCEL_INDEX = 10;
        const int Z_ACCEL_INDEX = 11;
        const int X_GYRO_INDEX = 12;
        const int Y_GYRO_INDEX = 13;
        const int Z_GYRO_INDEX = 14;

        double time;

        private void initializeGraphs()
        {
            accelPane = accelGraph.GraphPane;
            accelPane.Title.Text = "Accelerometers";
            accelPane.XAxis.Title.Text = "Time (s)";
            accelPane.YAxis.Title.Text = "Sensor Output (g)";
            //            accelPane.YAxis.Scale.Max = accelMax;
            //            accelPane.YAxis.Scale.Min = accelMin;

            gyroPane = gyroGraph.GraphPane;
            gyroPane.Title.Text = "Rate Gyros";
            gyroPane.XAxis.Title.Text = "Time (s)";
            gyroPane.YAxis.Title.Text = "Sensor Output (deg/s)";
            //            gyroPane.YAxis.Scale.Max = gyroMax;
            //            gyroPane.YAxis.Scale.Min = gyroMin;

            magPane = magGraph.GraphPane;
            magPane.Title.Text = "Magnetic Sensors";
            magPane.XAxis.Title.Text = "Time (s)";
            magPane.YAxis.Title.Text = "Sensor Output (raw)";
            //            magPane.YAxis.Scale.Max = magMax;
            //            magPane.YAxis.Scale.Min = magMin;

            anglePane = angleGraph.GraphPane;
            anglePane.Title.Text = "Estimated Angles";
            anglePane.XAxis.Title.Text = "Time (s)";
            anglePane.YAxis.Title.Text = "Angle (degrees)";

            quaternionPane = quaternion.GraphPane;
            quaternionPane.Title.Text = "Estimated quaternion";
            quaternionPane.XAxis.Title.Text = "Time (s)";
            quaternionPane.YAxis.Title.Text = "quaternion ";

            positionPane = position.GraphPane;
            positionPane.Title.Text = "Estimated position";
            positionPane.XAxis.Title.Text = "Time (s)";
            positionPane.YAxis.Title.Text = "position ";


            xAccelList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            yAccelList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            zAccelList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            xGyroList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            yGyroList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            zGyroList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            xMagList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            yMagList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            zMagList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            yawList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            pitchList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            rollList = new RollingPointPairList(SENSOR_GRAPH_POINTS);

            q0List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            q1List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            q2List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            q3List = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            latList = new RollingPointPairList(SENSOR_GRAPH_POINTS);
            lonList = new RollingPointPairList(SENSOR_GRAPH_POINTS);


            xAccelLine = accelPane.AddCurve("X", xAccelList, Color.Blue, SymbolType.None);
            yAccelLine = accelPane.AddCurve("Y", yAccelList, Color.Red, SymbolType.None);
            zAccelLine = accelPane.AddCurve("Z", zAccelList, Color.Green, SymbolType.None);

            xGyroLine = gyroPane.AddCurve("X", xGyroList, Color.Blue, SymbolType.None);
            yGyroLine = gyroPane.AddCurve("Y", yGyroList, Color.Red, SymbolType.None);
            zGyroLine = gyroPane.AddCurve("Z", zGyroList, Color.Green, SymbolType.None);

            xMagLine = magPane.AddCurve("X", xMagList, Color.Blue, SymbolType.None);
            yMagLine = magPane.AddCurve("Y", yMagList, Color.Red, SymbolType.None);
            zMagLine = magPane.AddCurve("Z", zMagList, Color.Green, SymbolType.None);

            yawLine = anglePane.AddCurve("Yaw", yawList, Color.Blue, SymbolType.None);
            pitchLine = anglePane.AddCurve("Pitch", pitchList, Color.Red, SymbolType.None);
            rollLine = anglePane.AddCurve("Roll", rollList, Color.Green, SymbolType.None);


            q0Line = quaternionPane.AddCurve("q0", q0List, Color.Red, SymbolType.None);
            q1Line = quaternionPane.AddCurve("q1", q1List, Color.Green, SymbolType.None);
            q2Line = quaternionPane.AddCurve("q2", q2List, Color.Blue, SymbolType.None);
            q3Line = quaternionPane.AddCurve("q3", q3List, Color.Brown, SymbolType.None);
            latLine = positionPane.AddCurve("Latitude", latList, Color.Green, SymbolType.None);
            lonLine = positionPane.AddCurve("Longitude", lonList, Color.Red, SymbolType.None);


        }

        private void refreshGraphs()
        {
            
            angleGraph.AxisChange();
            angleGraph.Invalidate();

            angularRateGraph.AxisChange();
            angularRateGraph.Invalidate();
           
            gyroGraph.AxisChange();
            gyroGraph.Invalidate();

            accelGraph.AxisChange();
            accelGraph.Invalidate();

            magGraph.AxisChange();
            magGraph.Invalidate();

            quaternion.AxisChange();
            quaternion.Invalidate();

            position.AxisChange();
            position.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time += timer1.Interval / 1000.0;

            xAccelList.Add(time, sensor.accelX);
            yAccelList.Add(time, sensor.accelY);
            zAccelList.Add(time, sensor.accelZ);

            xGyroList.Add(time, sensor.gyroX);
            yGyroList.Add(time, sensor.gyroY);
            zGyroList.Add(time, sensor.gyroZ);

            xMagList.Add(time, sensor.magX);
            yMagList.Add(time, sensor.magY);
            zMagList.Add(time, sensor.magZ);

            yawList.Add(time, sensor.yawAngle);
            pitchList.Add(time, sensor.pitchAngle);
            rollList.Add(time, sensor.rollAngle);

            q0List.Add(time, sensor.q1s);
            q1List.Add(time, sensor.q2s);
            q2List.Add(time, sensor.q3s);

            q3List.Add(time, sensor.q4s);
            latList.Add(time, sensor.latitude);
            lonList.Add(time, sensor.longitude);

            refreshGraphs();

            IMU_ACC_X.Text = Convert.ToString(sensor.accelX);
            IMU_ACC_Y.Text = Convert.ToString(sensor.accelY);
            IMU_ACC_Z.Text = Convert.ToString(sensor.accelZ);

            IMU_GYRO_X.Text = Convert.ToString(sensor.gyroX);
            IMU_GYRO_Y.Text = Convert.ToString(sensor.gyroY);
            IMU_GYRO_Z.Text = Convert.ToString(sensor.gyroZ);

            IMU_MAG_X.Text = Convert.ToString(sensor.magX);
            IMU_MAG_Y.Text = Convert.ToString(sensor.magY);
            IMU_MAG_Z.Text = Convert.ToString(sensor.magZ);
            IMU_ROLL.Text  = Convert.ToString(sensor.rollAngle);
            IMU_PITCH.Text = Convert.ToString(sensor.pitchAngle);
            IMU_YAW1.Text  = Convert.ToString(sensor.yawAngle);
            ALTITUDE.Text  = Convert.ToString(sensor.altitudine);
            LATITUDE.Text  =Convert.ToString(sensor.latitude);
            LONGITUDE.Text = Convert.ToString(sensor.longitude);

            q0.Text = Convert.ToString(sensor.q1s);
            q1.Text = Convert.ToString(sensor.q2s);
            q2.Text = Convert.ToString(sensor.q3s);
            q3.Text = Convert.ToString(sensor.q4s);
            
            
        }

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

            message = "Timeout: ";
            message += System.Enum.Format(typeof(StateName), packet_type, "G");
            message += "\r\n";

           
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
        }

        void PacketReceivedEventHandler(PName packet_type, int flags)
        {
            string message;

            if (packet_type == PName.COMMAND_FAILED)
            {
                message = "Command Failed: ";
                message += System.Enum.Format(typeof(PName), (PName)flags, "G");
                message += "\r\n";
            }
            else if (packet_type == PName.COMMAND_COMPLETE)
            {
                message = "Command Complete: ";
                message += System.Enum.Format(typeof(PName), (PName)flags, "G");
                message += "\r\n";
            }
            else
            {
                message = "Received ";
                message += System.Enum.Format(typeof(PName), packet_type, "G");
                message += " packet\r\n";
            }

           
        }

        

        private void displayAHRSStates()
        {
            displayActiveChannels();

        }
       
       
       


        private void displayActiveChannels()
        {
            if (this.activeXAccel.InvokeRequired)
            {
                displayActiveChannelsCallback d = new displayActiveChannelsCallback(displayActiveChannels);
                this.Invoke(d, new object[] {  });
            }
            else
            {
                if (sensor.accelZ_active)
                    activeZAccel.Checked = true;
                else
                    activeZAccel.Checked = false;
                if (sensor.accelY_active)
                    activeYAccel.Checked = true;
                else
                    activeYAccel.Checked = false;
                if (sensor.accelX_active)
                    activeXAccel.Checked = true;
                else
                    activeXAccel.Checked = false;

                if (sensor.gyroZ_active)
                    activeZGyro.Checked = true;
                else
                    activeZGyro.Checked = false;
                if (sensor.gyroY_active)
                    activeYGyro.Checked = true;
                else
                    activeYGyro.Checked = false;
                if (sensor.gyroX_active)
                    activeXGyro.Checked = true;
                else
                    activeXGyro.Checked = false;

                if (sensor.magZ_active)
                    activeZMag.Checked = true;
                else
                    activeZMag.Checked = false;
                if (sensor.magY_active)
                    activeYMag.Checked = true;
                else
                    activeYMag.Checked = false;
                if (sensor.magX_active)
                    activeXMag.Checked = true;
                else
                    activeXMag.Checked = false;


                if (sensor.roll_active)
                    activeRoll.Checked = true;
                else
                    activeRoll.Checked = false;
                if (sensor.pitch_active)
                    activePitch.Checked = true;
                else
                    activePitch.Checked = false;
                if (sensor.yaw_active)
                    activeYaw.Checked = true;
                else
                    activeYaw.Checked = false;

            }

        }

      

        private void InfoDump_Load(object sender, EventArgs e)
        {

        }

        private void SynchButton_Click(object sender, EventArgs e)
        {
            sensor.synch();
        }


        private void InvalidateAllButton_Click(object sender, EventArgs e)
        {
            sensor.Invalidate();
            sensor.synch();
        }



        private void activeChannelsCommitButton_Click(object sender, EventArgs e)
        {
            sensor.yaw_active = activeYaw.Checked;
            sensor.pitch_active = activePitch.Checked;
            sensor.roll_active = activeRoll.Checked;
            sensor.magX_active = activeXMag.Checked;
            sensor.magY_active = activeYMag.Checked;
            sensor.magZ_active = activeZMag.Checked;
            sensor.gyroX_active = activeXGyro.Checked;
            sensor.gyroY_active = activeYGyro.Checked;
            sensor.gyroZ_active = activeZGyro.Checked;
            sensor.accelX_active = activeXAccel.Checked;
            sensor.accelY_active = activeYAccel.Checked;
            sensor.accelZ_active = activeZAccel.Checked;

            sensor.synch();
        }

        private void IMU_ROLL_TextChanged(object sender, EventArgs e)
        {

        }
        private void IMU_YAW_TextChanged(object sender, EventArgs e)
        {

        }

        private void label207_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }



    }
}
