using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;


namespace AHRSInterface
{
    // Delegate types for interfacing with AHRS class events
    // The PacketDelegate type is used to create handlers for 'PacketReceived' events
    // and for 'COMFailed' events.
    public delegate void PacketDelegate(PName packet_type, int flags);

    public delegate void StateDelegate(StateName state_type, int flags);

    // The DataReceivedDelegate is used to create handlers for 'DataReceived' events
    public delegate void DataReceivedDelegate(int active_channels);
    public delegate void confReceivedDelegate(int active_channels);

    public delegate void COMFailedDelegate();

    public delegate  bool sendPacket( Packet AHRSPacket );

    // Define a struct for converting a 4-byte data sequences into different data types
    [StructLayout(LayoutKind.Explicit)]
    struct byte_conversion_array
    {
        // Four Bytes
        [FieldOffset(0)]
        public byte byte3;

        [FieldOffset(1)]
        public byte byte2;

        [FieldOffset(2)]
        public byte byte1;

        [FieldOffset(3)]
        public byte byte0;

        // One Float
        [FieldOffset(0)]
        public float float0;

        // One 32-bit integer
        [FieldOffset(0)]
        public Int32 int32;

        // Two 16-bit integers
        [FieldOffset(0)]
        public Int16 int16_0;

        [FieldOffset(2)]
        public Int16 int16_1;

        // Two 16-bit unsigned integers
        [FieldOffset(0)]
        public UInt16 uint16_0;

        [FieldOffset(2)]
        public UInt16 uint16_1;
    }

    public enum PName
    {
        // TX Packet names (packets that can be received by the AHRS class)
        SET_ACTIVE_CHANNELS = 0,
        SET_REBOOT,
        GET_CONFIGURATION,
        SET_SILENT_MODE,
        SET_BROADCAST_MODE,

        SET_GYRO_BIAS,
        SET_GYRO_SCALE,
        SET_GYRO_ALIGNMENT,
        ZERO_RATE_GYROS,
        SET_START_CAL,

        SET_ACCEL_BIAS,
        SET_ACCEL_ALIGNMENT,
        SET_ACCEL_REF_VECTOR,
        AUTO_SET_ACCEL_REF,

        SET_MAG_CAL,
        SET_MAG_BIAS,
        SET_MAG_REF_VECTOR,
        AUTO_SET_MAG_REF,

        EKF_RESET,
        SET_EKF_CONFIG,
        SET_PROCESS_COVARIANCE,
        SET_MAG_COVARIANCE,
        SET_ACCEL_COVARIANCE,
        SELF_TEST,

        RESET_TO_FACTORY,
        SET_KALMAN_PARM,
        SET_OUTPUT,
        WRITE_TO_FLASH,
        UPDATE_FW,
        CALIBRATE_GYRO_BIAS,

        GET_DATA,
        GET_ACTIVE_CHANNELS,
        GET_BROADCAST_MODE,
        GET_ACCEL_BIAS,
        GET_ACCEL_REF_VECTOR,
        GET_GYRO_BIAS,
        GET_GYRO_SCALE,
        GET_START_CAL,
        GET_EKF_CONFIG,
        GET_PROCESS_COVARIANCE,
        GET_MAG_COVARIANCE,
        GET_MAG_REF_VECTOR,
        GET_MAG_CAL,
        GET_MAG_BIAS,
        GET_ACCEL_COVARIANCE,
        GET_STATE_COVARIANCE,
        GET_ACCEL_ALIGNMENT,
        GET_GYRO_ALIGNMENT,
         // Packets that can be received by the AHRS class
        COMMAND_COMPLETE,
        COMMAND_FAILED,
        BAD_CHECKSUM,
        BAD_DATA_LENGTH,
        UNRECOGNIZED_PACKET,
        BUFFER_OVERFLOW,
        STATUS_REPORT,
        SENSOR_DATA,
        GYRO_BIAS_REPORT,
        GYRO_SCALE_REPORT,
        START_CAL_REPORT,
        ACCEL_BIAS_REPORT,
        ACTIVE_CHANNEL_REPORT,
        ACCEL_COVARIANCE_REPORT,
        ACCEL_REF_VECTOR_REPORT,
        MAG_REF_VECTOR_REPORT,
        MAG_COVARIANCE_REPORT,
        PROCESS_COVARIANCE_REPORT,
        STATE_COVARIANCE_REPORT,
        EKF_CONFIG_REPORT,
        GYRO_ALIGNMENT_REPORT,
        ACCEL_ALIGNMENT_REPORT,
        MAG_CAL_REPORT,
        MAG_BIAS_REPORT,
        BROADCAST_MODE_REPORT

    }

    public enum StateName
    {
        ACTIVE_CHANNELS,
        BROADCAST_MODE,
        ACCEL_BIAS,
        ACCEL_ALIGNMENT,
        ACCEL_COVARIANCE,
        ACCEL_REF,
        AUTO_SET_ACCEL_REF,
        GYRO_BIAS,
        GYRO_SCALE,
        GYRO_ALIGNMENT,
        START_CAL,
        EKF_CONFIG,
        PROCESS_COVARIANCE,
        MAG_COVARIANCE,
        MAG_CAL,
        MAG_BIAS,
        MAG_REF,
        AUTO_SET_MAG_REF,
        ZERO_GYROS,
        WRITE_TO_FLASH,
        CALIBRATE_GYRO_BIAS,
        UPDATE_FW,
        RESET_TO_FACTORY,
        SELF_TEST,
        EKF_RESET,
        GET_CONFIGURATION,
        SENSOR_DATA,
        SET_KALMAN_PARM,
        SET_OUTPUT,
        REBOOT,
        STATE_COVARIANCE
    }

    public class AHRS
    {
        // Default constructor
        public AHRS()
        {
            connected = false;

            // Fill arrays used for AHRS COM
            int packet_count = PName.GetValues( typeof( PName ) ).Length;
            PID = new byte[packet_count];

            serialPort = new SerialPort();
            PacketTimer = new Timer();
            
            int state_count = StateName.GetValues(typeof(StateName)).Length;
            UpdatePending = new bool[state_count];
            DataPending = new bool[state_count];
            Measured = new bool[state_count];
            MaxDelay = new int[state_count];
            ElapsedTime = new int[state_count];


            // Commands that can be sent to the AHRS device
            PID[(int)PName.SET_ACTIVE_CHANNELS] = 0x80;
            PID[(int)PName.SET_SILENT_MODE] = 0x81;
            PID[(int)PName.SET_BROADCAST_MODE] = 0x82;
            PID[(int)PName.SET_GYRO_BIAS] = 0x83;
            PID[(int)PName.SET_ACCEL_BIAS] = 0x84;
            PID[(int)PName.SET_ACCEL_REF_VECTOR] = 0x85;
            PID[(int)PName.AUTO_SET_ACCEL_REF] = 0x86;
            PID[(int)PName.ZERO_RATE_GYROS] = 0x87;
            PID[(int)PName.SELF_TEST] = 0x88;
            PID[(int)PName.SET_START_CAL] = 0x89;
            PID[(int)PName.SET_PROCESS_COVARIANCE] = 0x8A;
            PID[(int)PName.SET_MAG_COVARIANCE] = 0x8B;
            PID[(int)PName.SET_ACCEL_COVARIANCE] = 0x8C;
            PID[(int)PName.SET_EKF_CONFIG] = 0x8D;
            PID[(int)PName.SET_GYRO_ALIGNMENT] = 0x8E;
            PID[(int)PName.SET_ACCEL_ALIGNMENT] = 0x8F;
            PID[(int)PName.SET_MAG_REF_VECTOR] = 0x90;
            PID[(int)PName.AUTO_SET_MAG_REF] = 0x91;
            PID[(int)PName.SET_MAG_CAL] = 0x92;
            PID[(int)PName.SET_MAG_BIAS] = 0x93;
            PID[(int)PName.SET_GYRO_SCALE] = 0x94;
            PID[(int)PName.EKF_RESET] = 0x95;
            PID[(int)PName.RESET_TO_FACTORY] = 0x96;
            PID[(int)PName.SET_KALMAN_PARM] = 0x97;
            PID[(int)PName.SET_OUTPUT] = 0x98;
            PID[(int)PName.WRITE_TO_FLASH] = 0xA0;
            PID[(int)PName.UPDATE_FW] = 0x9C;
            PID[(int)PName.CALIBRATE_GYRO_BIAS] = 0x9B;
            PID[(int)PName.GET_CONFIGURATION] = 0x9A;
            PID[(int)PName.SET_REBOOT] = 0x99;

            // Commands that can be sent to the AHRS device

            // Commands that can be received by the AHRS class
            // (ie. transmitted by the device, received by the class
            // over a serial port).

            // Set AHRS class parameters so that on the next call to 'synch', the class
            // will attempt to acquire all internal states of the actual AHRS device.
            Invalidate();

            // Setup timer for keeping track of time elapsed between packet transmission and reception of response.
            PacketTimer.Interval = 1;      // 10 ms delay
            PacketTimer.Enabled = true;
            PacketTimer.Tick += new System.EventHandler(OnPacketTimerTick);
           
            RXbuffer = new byte[RX_BUF_SIZE];

            //inizialize status 
            m_state_covariance = new float[3, 3];
            m_state_covariance[0, 0] = (float)1.0;
            m_state_covariance[0, 1] = (float)0.0;
            m_state_covariance[0, 2] = (float)0.0;
            m_state_covariance[1, 0] = (float)0.0;
            m_state_covariance[1, 1] = (float)1.0;
            m_state_covariance[1, 2] = (float)0.0;
            m_state_covariance[2, 0] = (float)0.0;
            m_state_covariance[2, 1] = (float)0.0;
            m_state_covariance[2, 2] = (float)1.0;

            m_accel_alignment = new float[3, 3];
            m_accel_alignment[0, 0] = (float)1.0;
            m_accel_alignment[0, 1] = (float)0.0;
            m_accel_alignment[0, 2] = (float)0.0;
            m_accel_alignment[1, 0] = (float)0.0;
            m_accel_alignment[1, 1] = (float)1.0;
            m_accel_alignment[1, 2] = (float)0.0;
            m_accel_alignment[2, 0] = (float)0.0;
            m_accel_alignment[2, 1] = (float)0.0;
            m_accel_alignment[2, 2] = (float)1.0;

            m_gyro_alignment = new float[3, 3];
            m_gyro_alignment[0, 0] = (float)1.0;
            m_gyro_alignment[0, 1] = (float)0.0;
            m_gyro_alignment[0, 2] = (float)0.0;
            m_gyro_alignment[1, 0] = (float)0.0;
            m_gyro_alignment[1, 1] = (float)1.0;
            m_gyro_alignment[1, 2] = (float)0.0;
            m_gyro_alignment[2, 0] = (float)0.0;
            m_gyro_alignment[2, 1] = (float)0.0;
            m_gyro_alignment[2, 2] = (float)1.0;

            m_mag_cal = new float[3, 3];
            m_mag_cal[0, 0] = (float)1.0;
            m_mag_cal[0, 1] = (float)0.0;
            m_mag_cal[0, 2] = (float)0.0;
            m_mag_cal[1, 0] = (float)0.0;
            m_mag_cal[1, 1] = (float)1.0;
            m_mag_cal[1, 2] = (float)0.0;
            m_mag_cal[2, 0] = (float)0.0;
            m_mag_cal[2, 1] = (float)0.0;
            m_mag_cal[2, 2] = (float)1.0;

            m_init_quat = new float[4];
            m_init_quat[0] = (float)0.00011;
            m_init_quat[1] = (float)0.00011;
            m_init_quat[2] = (float)0.00011;
            m_init_quat[3] = (float)0.00011;

            m_Q_bias = new float[3];
            m_Q_bias[0] = (float)0.0000795;
            m_Q_bias[1] = (float)0.00007925;
            m_Q_bias[2] = (float)0.00007925;

            m_gain = new float[4];
            m_gain[0] = (float)0.00008;
            m_gain[1] = (float)0.0;
            m_gain[2] = (float)0.0;
            m_gain[3] = (float)0.0;
            m_filter_type= (float)0.0;
            m_convolution_time= (float)0.0;

            m_y_gyro_bias= (float)0.0;
            m_z_gyro_bias=  (float)0.0;
            m_x_accel_bias=  (float)0.0;
            m_y_accel_bias=  (float)0.0;
            m_z_accel_bias=  (float)0.0;
            m_x_mag_bias=  (float)0.0;
            m_y_mag_bias=  (float)0.0;
            m_z_mag_bias=  (float)0.0;
            m_mag_covariance=  (float)0.0025;
            m_accel_covariance = (float)0.000799;
          
            m_output_enable = (UInt16)0;

            m_output_rate= (UInt16)200;
            m_badurate= (UInt16)1;
            m_port= (UInt16)1200;
            m_ip = new char[30];
            String str = "192.168.010.030";
            m_ip = str.ToCharArray();
           // confReceivedEvent(1);

        }

        // Destructor
        ~AHRS()
        {
            // If the serial port is open, close it.
            if (connected)
            {
                serialPort.Close();
            }

            PacketTimer.Stop();
            PacketTimer.Dispose();
        }

        /* **********************************************************************************
         * 
         * Private member variables
         * 
         * *********************************************************************************/
        
        // Events for interfacing with AHRS class
        // A PacketReveivedEvent occurs when a new packet is received and parsed.
        public event PacketDelegate PacketReceivedEvent;
        public event PacketDelegate PacketSentEvent;
        public event COMFailedDelegate COMFailedEvent;
        // A DataReceivedEvent occurs when new sensor data arrives from the AHRS.
        public event DataReceivedDelegate DataReceivedEvent;
        public event confReceivedDelegate confReceivedEvent;
        // A COMFailedEvent occurs when the AHRS class expects to receive data from the
        // AHRS device, but no data is received in the max. allowed time.
        public event StateDelegate PacketTimeoutEvent;
        
        // Data for communication
        private bool connected;
        private SerialPort serialPort;
        private Timer PacketTimer;

        const int RX_BUF_SIZE = 5000;//200;
        public byte[] RXbuffer { get; set;  }
        private int RXbufPtr;
        const int MAX_PACKET_SIZE = 85;

        const int CHANNEL_COUNT = 23;

        const int YAW_INDEX = 0;
        const int PITCH_INDEX = 1;
        const int ROLL_INDEX = 2;
        const int Q4_INDEX = 3;
        const int X_MAG_INDEX = 4;
        const int Y_MAG_INDEX = 5;
        const int Z_MAG_INDEX = 6;
        const int X_ACCEL_INDEX = 7;
        const int Y_ACCEL_INDEX = 8;
        const int Z_ACCEL_INDEX = 9;
        const int X_GYRO_INDEX = 10;
        const int Y_GYRO_INDEX = 11;
        const int Z_GYRO_INDEX = 12;
        const int LATITUDE_INDEX = 13;
        const int LONGITUDE_INDEX = 14;
        const int VEL_GPS_INDEX = 15;
        const int ALTITUDINE_INDEX = 16;
        const int SATELLITE_NUMBER_INDEX = 17;
        const int CRC_INDEX = 18;
        const int ID_DISP = 22;
        const int Q3_INDEX = 19;
        const int Q2_INDEX = 20;
        const int Q1_INDEX = 21;


        // Variables for storing the state of the AHRS
        private float[,] m_accel_alignment;
        private float[,] m_gyro_alignment;
        private float[,] m_mag_cal;
        private float[,] m_state_covariance;
        private float[] m_init_quat;
        private float[] m_Q_bias;
        private float[] m_gain;
        private float m_filter_type;
        private float m_convolution_time;
        private UInt16 m_output_enable;
        private UInt16 m_output_rate;
        private UInt16 m_badurate;
        private UInt16 m_port;
        private char[] m_ip;
        private float m_process_covariance;
        private float m_mag_covariance;
        private float m_accel_covariance;
        private float m_x_gyro_scale;
        private float m_y_gyro_scale;
        private float m_z_gyro_scale;
        private float m_x_gyro_bias;
        private float m_y_gyro_bias;
        private float m_z_gyro_bias;
        private float m_x_accel_bias;
        private float m_y_accel_bias;
        private float m_z_accel_bias;
        private float m_x_mag_bias;
        private float m_y_mag_bias;
        private float m_z_mag_bias;
        private float m_x_mag_ref;
        private float m_y_mag_ref;
        private float m_z_mag_ref;
        private float m_x_accel_ref;
        private float m_y_accel_ref;
        private float m_z_accel_ref;
        private bool m_mag_enabled;
        private bool m_accel_enabled;
        private bool m_mag_restriced_to_yaw;
        private bool m_broadcast_enabled;
        private UInt16 m_broadcast_rate;
       private bool m_zero_gyros_on_startup;


        // Array for storing the most recent data obtained from the AHRS
        private float[] m_recentData = new float[CHANNEL_COUNT];

        private double m_pitchAngle;
        private double m_rollAngle;
        private double m_yawAngle;
        private double m_q4;
        private double m_q3;
        private double m_q2;
        private double m_q1;

        private double m_q4s;
        private double m_q3s;
        private double m_q2s;
        private double m_q1s;
        private double m_magX;
        private double m_magY;
        private double m_magZ;
        private double m_gyroX;
        private double m_gyroY;
        private double m_gyroZ;
        private double m_accelX;
        private double m_accelY;
        private double m_accelZ;
        private double m_latitude;
        private double m_longitude;
        private double m_velGPS;
        private double m_altitudine;
        private double m_satellite_number;
        private double m_crc;
        private double m_ID_DISP;

        // Variables for storing the state of the AHRS

        private bool m_accelX_active;
        private bool m_accelY_active;
        private bool m_accelZ_active;
        private bool m_gyroX_active;
        private bool m_gyroY_active;
        private bool m_gyroZ_active;
        private bool m_magX_active;
        private bool m_magY_active;
        private bool m_magZ_active;
        private bool m_pitch_active;
        private bool m_roll_active;
        private bool m_yaw_active;
        private bool m_q4_active;
        private bool m_q3_active;
        private bool m_q2_active;
        private bool m_q1_active;
        private bool m_latitude_active;
        private bool m_longitude_active;
        private bool m_velGPS_active;
        private bool m_altitudine_active;
        private bool m_satellite_number_active;
        private bool m_crc_active;
        private bool m_ID_active;


        private string configuration_String="";

        public string configuration_String_r
        {
            get { return configuration_String; }
        }

        public double q4
        {
            get { return m_q4; }
        }

        public double q3
        {
            get { return m_q3; }
        }
        public double q2
        {
            get { return m_q2; }
        }
        public double q1
        {
            get { return m_q1; }
        }

        public double q4s
        {
            get { return m_q4s; }
        }

        public double q3s
        {
            get { return m_q3s; }
        }
        public double q2s
        {
            get { return m_q2s; }
        }
        public double q1s
        {
            get { return m_q1s; }
        }

        public double yawAngle
        {
            get { return m_yawAngle; }
        }

        public double pitchAngle
        {
            get { return m_pitchAngle; }
        }

        public double rollAngle
        {
            get { return m_rollAngle; }
        }

   
        public double magX
        {
            get { return m_magX; }
        }

        public double magY
        {
            get { return m_magY; }
        }

        public double magZ
        {
            get { return m_magZ; }
        }

        public double gyroX
        {
            get { return m_gyroX; }
        }

        public double gyroY
        {
            get { return m_gyroY; }
        }

        public double gyroZ
        {
            get { return m_gyroZ; }
        }

        public double accelX
        {
            get { return m_accelX; }
        }

        public double accelY
        {
            get { return m_accelY; }
        }

        public double accelZ
        {
            get { return m_accelZ; }
        }

        public double id
        {
            get { return m_ID_DISP; }
        }

       public double latitude
        {
            get { return m_latitude; }
        }
       public double longitude
        {
            get { return m_longitude; }
        }
       public double velGPS
        {
            get { return m_velGPS; }
        }
       public double altitudine
        {
            get { return m_altitudine; }
        }
    
       public double satellite_number
        {
            get { return m_satellite_number; }
        }
     
         public double crc
        {
            get { return m_crc; }
        }


        // Public accessor methods
        public float[] recentData
        {
            get { return m_recentData; }
        }
        
        public bool accelX_active
        {
            get { return m_accelX_active; }
            set { m_accelX_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool accelY_active
        {
            get { return m_accelY_active; }
            set { m_accelY_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool accelZ_active
        {
            get { return m_accelZ_active; }
            set { m_accelZ_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool gyroX_active
        {
            get { return m_gyroX_active; }
            set { m_gyroX_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool gyroY_active
        {
            get { return m_gyroY_active; }
            set { m_gyroY_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool gyroZ_active
        {
            get { return m_gyroZ_active; }
            set { m_gyroZ_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool magX_active
        {
            get { return m_magX_active; }
            set { m_magX_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool magY_active
        {
            get { return m_magY_active; }
            set { m_magY_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool magZ_active
        {
            get { return m_magZ_active; }
            set { m_magZ_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool pitch_active
        {
            get { return m_pitch_active; }
            set { m_pitch_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool roll_active
        {
            get { return m_roll_active; }
            set { m_roll_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool yaw_active
        {
            get { return m_yaw_active; }
            set { m_yaw_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q4_active
        {
            get { return m_q4_active; }
            set { m_q4_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool q3_active
        {
            get { return m_q3_active; }
            set { m_q3_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool q2_active
        {
            get { return m_q2_active; }
            set { m_q2_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool q1_active
        {
            get { return m_q1_active; }
            set { m_q1_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

       
        public bool latitude_active
        {
            get { return m_latitude_active; }
            set { m_latitude_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool longitude_active
        {
            get { return m_longitude_active; }
            set { m_longitude_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool velGPS_active
        {
            get { return m_velGPS_active; }
            set { m_velGPS_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool altitudine_active
        {
            get { return m_altitudine_active; }
            set { m_altitudine_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
      
        public bool satellite_number_active
        {
            get { return m_satellite_number_active; }
            set { m_satellite_number_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool crc_active
        {
            get { return m_crc_active; }
            set { m_crc_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }

        public bool ID_active
        {
            get { return m_ID_active; }
            set { m_ID_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }


        public float[,] accel_alignment
        {
            get { return m_accel_alignment; }
            set { m_accel_alignment = value; UpdatePending[(int)StateName.ACCEL_ALIGNMENT] = true; }
        }

        public float[,] gyro_alignment
        {
            get { return m_gyro_alignment; }
            set { m_gyro_alignment = value; UpdatePending[(int)StateName.GYRO_ALIGNMENT] = true; }
        }

        public float[] init_quat
        {
            get { return m_init_quat; }
            set { m_init_quat = value; UpdatePending[(int)StateName.SET_KALMAN_PARM] = true; }
        }
        public float[] Q_bias
        {
            get { return m_Q_bias; }
            set { m_Q_bias = value; UpdatePending[(int)StateName.SET_KALMAN_PARM] = true; }
        }
        public float[] gain
        {
            get { return m_gain; }
            set { m_gain = value; UpdatePending[(int)StateName.SET_KALMAN_PARM] = true; }
        }
        public float filter_type
        {
            get { return m_filter_type; }
            set { m_filter_type = value; UpdatePending[(int)StateName.SET_KALMAN_PARM] = true; }
        }
        public float convolution_time
        {
            get { return m_convolution_time; }
            set { m_convolution_time = value; UpdatePending[(int)StateName.SET_KALMAN_PARM] = true; }
        }

        public UInt16 output_enable
        {
            get { return m_output_enable; }
            set { m_output_enable = value; UpdatePending[(int)StateName.SET_OUTPUT] = true; }
        }
        public UInt16 output_rate
        {
            get { return m_output_rate; }
            set { m_output_rate = value; UpdatePending[(int)StateName.SET_OUTPUT] = true; }
        }

        public UInt16 badurate
        {
            get { return m_badurate; }
            set { m_badurate = value; UpdatePending[(int)StateName.SET_OUTPUT] = true; }
        }


        public UInt16 port
        {
            get { return m_port; }
            set { m_port = value; UpdatePending[(int)StateName.SET_OUTPUT] = true; }
        }

        public char[] ip
        {
            get { return m_ip; }
            set { m_ip = value; UpdatePending[(int)StateName.SET_OUTPUT] = true; }
        }

        public float[,] mag_cal
        {
            get { return m_mag_cal; }
            set { m_mag_cal = value; UpdatePending[(int)StateName.MAG_CAL] = true; }
        }

        public float[,] state_covariance
        {
            get { return m_state_covariance; }
        }

        // Public accessor methods


        public float process_covariance
        {
            get { return m_process_covariance; }
            set { m_process_covariance = value; UpdatePending[(int)StateName.PROCESS_COVARIANCE] = true; }
        }

        public float mag_covariance
        {
            get { return m_mag_covariance; }
            set { m_mag_covariance = value; UpdatePending[(int)StateName.MAG_COVARIANCE] = true; }
        }

        public float accel_covariance
        {
            get { return m_accel_covariance; }
            set { m_accel_covariance = value; UpdatePending[(int)StateName.ACCEL_COVARIANCE] = true; }
        }

        public float x_gyro_scale
        {
            get { return m_x_gyro_scale; }
            set { m_x_gyro_scale = value; UpdatePending[(int)StateName.GYRO_SCALE] = true; }
        }

        public float y_gyro_scale
        {
            get { return m_y_gyro_scale; }
            set { m_y_gyro_scale = value; UpdatePending[(int)StateName.GYRO_SCALE] = true; }
        }

        public float z_gyro_scale
        {
            get { return m_z_gyro_scale; }
            set { m_z_gyro_scale = value; UpdatePending[(int)StateName.GYRO_SCALE] = true; }
        }

        public float x_gyro_bias
        {
            get { return m_x_gyro_bias; }
            set { m_x_gyro_bias = value; UpdatePending[(int)StateName.GYRO_BIAS] = true; }
        }

        public float y_gyro_bias
        {
            get { return m_y_gyro_bias; }
            set { m_y_gyro_bias = value; UpdatePending[(int)StateName.GYRO_BIAS] = true; }
        }

        public float z_gyro_bias
        {
            get { return m_z_gyro_bias; }
            set { m_z_gyro_bias = value; UpdatePending[(int)StateName.GYRO_BIAS] = true; }
        }

        public float x_accel_bias
        {
            get { return m_x_accel_bias; }
            set { m_x_accel_bias = value; UpdatePending[(int)StateName.ACCEL_BIAS] = true; }
        }

        public float y_accel_bias
        {
            get { return m_y_accel_bias; }
            set { m_y_accel_bias = value; UpdatePending[(int)StateName.ACCEL_BIAS] = true; }
        }

        public float z_accel_bias
        {
            get { return m_z_accel_bias; }
            set { m_z_accel_bias = value; UpdatePending[(int)StateName.ACCEL_BIAS] = true; }
        }

        public float x_mag_bias
        {
            get { return m_x_mag_bias; }
            set { m_x_mag_bias = value; UpdatePending[(int)StateName.MAG_BIAS] = true; }
        }

        public float y_mag_bias
        {
            get { return m_y_mag_bias; }
            set { m_y_mag_bias = value; UpdatePending[(int)StateName.MAG_BIAS] = true; }
        }

        public float z_mag_bias
        {
            get { return m_z_mag_bias; }
            set { m_z_mag_bias = value; UpdatePending[(int)StateName.MAG_BIAS] = true; }
        }

        public float x_mag_ref
        {
            get { return m_x_mag_ref; }
            set { m_x_mag_ref = value; UpdatePending[(int)StateName.MAG_REF] = true; }
        }

        public float y_mag_ref
        {
            get { return m_y_mag_ref; }
            set { m_y_mag_ref = value; UpdatePending[(int)StateName.MAG_REF] = true; }
        }

        public float z_mag_ref
        {
            get { return m_z_mag_ref; }
            set { m_z_mag_ref = value; UpdatePending[(int)StateName.MAG_REF] = true; }
        }

        public float x_accel_ref
        {
            get { return m_x_accel_ref; }
            set { m_x_accel_ref = value; UpdatePending[(int)StateName.ACCEL_REF] = true; }
        }

        public float y_accel_ref
        {
            get { return m_y_accel_ref; }
            set { m_y_accel_ref = value; UpdatePending[(int)StateName.ACCEL_REF] = true; }
        }

        public float z_accel_ref
        {
            get { return m_z_accel_ref; }
            set { m_z_accel_ref = value; UpdatePending[(int)StateName.ACCEL_REF] = true; }
        }

        public bool mag_enabled
        {
            get { return m_mag_enabled; }
            set { m_mag_enabled = value; UpdatePending[(int)StateName.EKF_CONFIG] = true; }
        }

        public bool mag_restricted_to_yaw
        {
            get { return m_mag_restriced_to_yaw; }
            set { m_mag_restriced_to_yaw = value; UpdatePending[(int)StateName.EKF_CONFIG] = true; }
        }

        public bool accel_enabled
        {
            get { return m_accel_enabled; }
            set { m_accel_enabled = value; UpdatePending[(int)StateName.EKF_CONFIG] = true; }
        }

        public bool broadcast_enabled
        {
            get { return m_broadcast_enabled; }
            set { m_broadcast_enabled = value; UpdatePending[(int)StateName.BROADCAST_MODE] = true; }
        }

        public UInt16 broadcast_rate
        {
            get { return m_broadcast_rate; }
            set { m_broadcast_rate = value; UpdatePending[(int)StateName.BROADCAST_MODE] = true; }
        }

        public bool zero_gyros_on_startup
        {
            get { return m_zero_gyros_on_startup; }
            set { m_zero_gyros_on_startup = value; UpdatePending[(int)StateName.START_CAL] = true; }
        }

  

        public bool IsConnected
        {
            get { return connected; }
        }
        
        // Arrays for definition of the AHRS COM protocol.  The enum PName references the names
        // of all possible packets that can be transmitted and received by the AHRS class.
        // The PID array stores the packet IDs associated with each packet name in PName.
        Byte[] PID;
        // The 'DataPending' flag is 'true' when a packet has been sent to the AHRS and the
        // AHRS class is awaiting a response.  This applies for both data requests and for
        // commands.
        bool[] DataPending;
        // For each packet sent, there is a MaxDelay in milliseconds that the AHRS will wait
        // to receive a response from the AHRS before assuming that the AHRS did not receive
        // the packet.  The 'ElapsedTime' array is used to store the amount of time that has
        // passed since the initiating packet was sent.
        int[] MaxDelay;
        int[] ElapsedTime;

        // The 'UpdatePending' flah is 'true' when the internal class state has been changed,
        // but the changes have not been transmitted to the AHRS itself.
        bool[] UpdatePending;

        // The 'Measured' flag is true when data has been received from AHRS - this applies to
        // AHRS configuration states that are stored internally by the AHRS class.
        bool[] Measured;


        /* **********************************************************************************
         * 
         * Function: private UInt16 Invalidate()
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS class to assume that none of its internal data is correct.
         * After Invalidate has been called, a call to 'synch' will cause the class
         * to re-acquire all data from the AHRS device.
         * 
         * *********************************************************************************/
        public void Invalidate()
        {
            // Clear 'update' and 'measured' data members.
            for (int i = 0; i < UpdatePending.Length; i++)
            {
                UpdatePending[i] = false;
                Measured[i] = false;

                // Set maximum delay in milliseconds.  Set to 200 ms for each packet - special cases (ie. packets that take longer) are
                // set later.
                MaxDelay[i] = 1;
                ElapsedTime[i] = 0;
                DataPending[i] = false;
            }

            // Set 'Measured' flag to 'true' for packets that should not be sent on synchronization.

            Measured[(int)StateName.SENSOR_DATA] = true;

        }

        /* **********************************************************************************
         * 
         * Function: public static void OnPacketTimerTick
         * Inputs: object source, EventArgs e
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * On each timer "tick," the DataPending array is checked to determine which packets
         * have been sent and are awaiting a response.  For each pending response, the amount
         * of time that has elapsed since the packet was sent is checked.  If more time has
         * elapsed than the time (in ms) specified by MaxDelay[], then assume the operation
         * has failed.  Trigger a COMFailed event for the packet.
         * 
         * *********************************************************************************/
        public void OnPacketTimerTick(object source, EventArgs e)
        {
            int i;

            for (i = 0; i < ElapsedTime.Length; i++)
            {
                if (DataPending[i])
                {
                    ElapsedTime[i] += PacketTimer.Interval;
                    if (ElapsedTime[i] >= MaxDelay[i])
                    {
                        // Set DataPending flag to false, reset elapsed time.
                        DataPending[i] = false;
                        ElapsedTime[i] = 0;

                        // Trigger a COMFailed event
                        PacketTimeoutEvent((StateName)i, 0);
                    }
                }
            }
        }

        /* **********************************************************************************
         * 
         * Function: public bool connect( String portname, String baudrate )
         * Inputs: None
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Connects to the specified serial port.  Returns true on success, false on failure
         * 
         * *********************************************************************************/
        public bool connect(String portname, String baudrate)
        {

            //set the properties of the SerialPort Object
            serialPort.PortName = portname;
            serialPort.BaudRate = Convert.ToInt32(baudrate);
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake =Handshake.None;
            serialPort.ReadTimeout = 1;
            serialPort.WriteTimeout = 100;
           // serialPort.WriteBufferSize = 50;
            try
            {
                //now open the port
                serialPort.Open();
               // serialPort.DtrEnable = serialPort.RtsEnable = true;

                connected = true;

                // Add event handler
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                serialPort.DiscardInBuffer();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /* **********************************************************************************
         * 
         * Function: public bool Disconnect( )
         * Inputs: None
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Disconnects from the serial port
         * 
         * *********************************************************************************/
        public bool Disconnect()
        {
            if (connected)
            {
                serialPort.Close();
                connected = false;
            }

            return true;
        }

        /* **********************************************************************************
         * rooo
         * Function: private void serialPort_DataReceived
         * Inputs: object sender, SerialDataReceivedEventArgs e
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Event handler for serial communication
         * 
         * *********************************************************************************/
        
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int continue_parsing = 1;
            int bytes_to_read;
            byte packet_type = 1;
            //dimensione fissa del pacchetto
            Int16 data_size = 85;//full packet struct
            Int16 data_size_onlyquat = 24;//only quat packet struct
            Int16 data_size_configuration = 240;//oconfiguration packet struct
            byte[] data = new byte[MAX_PACKET_SIZE];
            byte[] dataconfiguration = new byte[data_size_configuration];
            //serialPort.DiscardInBuffer();
            bool found_packet;
            bool found_packet_congiguration;
            bool found_packet_only_quat = false;
            int packet_start_configuration_index;
            int packet_start_index;
            int packet_start_index_only_quat;
            int packet_index;


            try
            {
                bytes_to_read =  serialPort.BytesToRead;
                if (bytes_to_read >= data_size_configuration)
                {
                    if ((RXbufPtr + bytes_to_read) >= RX_BUF_SIZE)
                    {
                        RXbufPtr = 0;
                    }

                    if (bytes_to_read >= RX_BUF_SIZE)
                    {
                        bytes_to_read = RX_BUF_SIZE - 1;
                    }

                    // Get serial data
                    serialPort.Read(RXbuffer, RXbufPtr, bytes_to_read);
                    // serialPort.DiscardInBuffer();

                    RXbufPtr += bytes_to_read;
                }
            }
            catch
            {
                COMFailedEvent();
                return;
            }

            // If there are enough bytes in the buffer to construct a full packet, then check data.
            // There are RXbufPtr bytes in the buffer at any given time
            while (RXbufPtr >= data_size && (continue_parsing == 1))
            {
                // Search for the packet start sequence
                found_packet = false;
                packet_start_index = 0;
                found_packet_congiguration = false;
                found_packet_only_quat = false;
                packet_start_configuration_index = 0;
                packet_start_index_only_quat = 0;
                for (packet_index = 0; packet_index < (RXbufPtr - 2); packet_index++)
                {
                    
                    //if (RXbuffer[packet_index] == '#'  && RXbuffer[packet_index + 110] == '*')
                    if (RXbuffer[packet_index] == '#' && RXbuffer[packet_index + 1] == 's' && RXbuffer[packet_index + 2] == 'n')
                    {
                        if (RXbuffer[packet_index + 23] == '*')//only quat data
                        {
                            found_packet_only_quat = true;
                            packet_start_index_only_quat = packet_index;
                            break;

                        }
                        else
                        {
                            found_packet = true;
                            packet_start_index = packet_index;
                            break;
                        }
                    }
                    if (RXbuffer[packet_index] == '#' && RXbuffer[packet_index + 1] == 's' && RXbuffer[packet_index + 2] == 'p' && RXbuffer[packet_index + 3] == 'p')
                    {
                        found_packet_congiguration = true;
                        packet_start_configuration_index = packet_index;

                        break;
                    }
                }

                // If start sequence found, try to recover all the data in the packet
                if (found_packet && ((RXbufPtr - packet_start_index) >= data_size))
                {

                    // Only process packet if data_size is not too large.
                    if (data_size <= MAX_PACKET_SIZE)
                    {
                        for (int i = 0; i < data_size; i++)
                        {
                            data[i] = 0;

                        }
                        // If a full packet has been received, then the full packet size should be
                        // 3 + 1 + 1 + [data_size] + 2
                        // that is, 3 bytes for the start sequence, 1 byte for type, 1 byte for data length, data_size bytes
                        // for packet data, and 2 bytes for the checksum.
                        // If enough data has been received, go ahead and recover the packet.  If not, wait until the
                        // rest of the data arrives
                        int buffer_length = (RXbufPtr - packet_start_index);
                        int packet_length = (4 + data_size);
                        if (buffer_length >= packet_length)
                        {
                            // A full packet has been received.  Retrieve the data.
                            for (int i = 0; i < data_size; i++)
                            {
                                data[i] = RXbuffer[packet_start_index  + i];
                                RXbuffer[packet_start_index + i]=0;
                                //Console.WriteLine("data {0}", data[i].ToString());
                            }
                            handle_packet(packet_type, (int)data_size, data);
                        }
                        else
                        {
                            continue_parsing = 0;
                        }
                    }
                    else
                    {
                        // data_size was too large - the packet data is invalid.  Clear the RX buffer.
                        RXbufPtr = 0;
                        continue_parsing = 0;
                    //AppendStatusText("\r\nBAD PACKET", Color.Red);
                    }
                }

                // If start sequence found, try to recover all the data in the packet
                if (found_packet_only_quat && ((RXbufPtr - packet_start_index_only_quat) >= data_size_onlyquat))
                {

                    // Only process packet if data_size is not too large.
                    if (data_size_onlyquat <= MAX_PACKET_SIZE)
                    {
                        for (int i = 0; i < data_size_onlyquat; i++)
                        {
                            data[i] = 0;

                        }
                        // If a full packet has been received, then the full packet size should be
                        // 3 + 1 + 1 + [data_size] + 2
                        // that is, 3 bytes for the start sequence, 1 byte for type, 1 byte for data length, data_size bytes
                        // for packet data, and 2 bytes for the checksum.
                        // If enough data has been received, go ahead and recover the packet.  If not, wait until the
                        // rest of the data arrives
                        int buffer_length = (RXbufPtr - packet_start_index_only_quat);
                        int packet_length = (4 + data_size_onlyquat);
                        if (buffer_length >= packet_length)
                        {
                            // A full packet has been received.  Retrieve the data.
                            for (int i = 0; i < data_size_onlyquat; i++)
                            {
                                data[i] = RXbuffer[packet_start_index_only_quat + i];
                                RXbuffer[packet_start_index_only_quat + i] = 0;
                                //Console.WriteLine("data {0}", data[i].ToString());
                            }
                            handle_packet_ony_quat(packet_type, (int)data_size, data);
                        }
                        else
                        {
                            continue_parsing = 0;
                        }
                    }
                    else
                    {
                        // data_size was too large - the packet data is invalid.  Clear the RX buffer.
                        RXbufPtr = 0;
                        continue_parsing = 0;
                        //AppendStatusText("\r\nBAD PACKET", Color.Red);
                    }
                }
                //found configuration paket
                if (found_packet_congiguration && (((RXbufPtr - packet_start_configuration_index) >= data_size_configuration)))
                {


                    for (int i = 0; i < data_size_configuration; i++)
                    {
                        dataconfiguration[i] = 0;

                    }

                    // If a full packet has been received, then the full packet size should be
                    // 3 + 1 + 1 + [data_size] + 2
                    // that is, 3 bytes for the start sequence, 1 byte for type, 1 byte for data length, data_size bytes
                    // for packet data, and 2 bytes for the checksum.
                    // If enough data has been received, go ahead and recover the packet.  If not, wait until the
                    // rest of the data arrives
                    int buffer_length = (RXbufPtr - packet_start_configuration_index);
                    int packet_length = (4 + data_size_configuration);
                    if (buffer_length >= packet_length)
                    {
                        // A full packet has been received.  Retrieve the data.
                        for (int i = 0; i < data_size_configuration; i++)
                        {
                            dataconfiguration[i] = RXbuffer[packet_start_configuration_index + i];
                            RXbuffer[packet_start_configuration_index + i] = 0;
                            //Console.WriteLine("data {0}", data[i].ToString());
                        }
                        handle_packet_configuration(packet_type, (int)data_size_configuration, dataconfiguration);
                    }
                    else
                    {
                        continue_parsing = 0;
                    }
                    
                    RXbufPtr = 0;
                    continue_parsing = 0;

                }
                else
                {
                    continue_parsing = 0;
                }
            }
        }
        
   
      
        /* **********************************************************************************
         * 
         * Function: private int getTypeIndex
         * Inputs: byte type
         * Outputs: None
         * Return Value: The index of the PID in 'byte type'
         * Dependencies: None
         * Description: 
         * 
         * Finds the index of the packet type of the packet specified by 'type'.  If no
         * packet was found, returns -1.
         * 
         * *********************************************************************************/
        private int getTypeIndex(byte type)
        {
            int type_index = -1;

            // Iterate through the PID array to determine which packet was received
            // (ie. which enum PName was received).
            for (int i = 0; i < PID.Length; i++)
            {
                if (PID[i] == type)
                {
                    type_index = i;
                    break;
                }
            }

            return type_index;
        }

       

        private void updatePacketSynchHelper(StateName state)
        {
            DataPending[(int)state] = false;
            ElapsedTime[(int)state] = 0;
            Measured[(int)state] = true;
            UpdatePending[(int)state] = false;
        }

        /* **********************************************************************************
         * 
         * Function: private void handle_packet
         * Inputs: byte type, int length, byte[] data
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Handles data packets received over the serial port.
         * 
         * *********************************************************************************/
        private void handle_packet(byte type, int length, byte[] data)
        {


                    int act_channels = 0xFFFF; 
                    int i = 4;
                 

                    byte[] Data_tmp=new byte[4];

                    Data_tmp[0]=data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Q1_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q1 = (float)m_recentData[Q1_INDEX];
                    i += 4;
                    // Pitch angle
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Q2_INDEX] = BitConverter.ToSingle(Data_tmp, 0);

                    m_q2 = (float)m_recentData[Q2_INDEX];
                    i += 4;

                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Q3_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q3 = (float)m_recentData[Q3_INDEX];
                    i += 4;

                    // quarto campo
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Q4_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q4 = (float)m_recentData[Q4_INDEX];
                    i += 4;

                    // Accel X
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[X_ACCEL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_accelX = (float)m_recentData[X_ACCEL_INDEX];
                    i += 4;


                    // Accel Y
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Y_ACCEL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_accelY = (float)m_recentData[Y_ACCEL_INDEX];
                    i += 4;

                    // Accel Z
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];

                    m_recentData[Z_ACCEL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_accelZ = (float)m_recentData[Z_ACCEL_INDEX];
                    i += 4;

                    // gyro_x
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];

                    m_recentData[X_GYRO_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_gyroX = (float)m_recentData[X_GYRO_INDEX];
                    i += 4;
                    // gyro_y
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];

                    m_recentData[Y_GYRO_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_gyroY = (float)m_recentData[Y_GYRO_INDEX];
                    i += 4;
                    // gyro_z
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];

                    m_recentData[Z_GYRO_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_gyroZ = (float)m_recentData[Z_GYRO_INDEX];
                    i += 4;

                    // Mag X
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];

                    m_recentData[X_MAG_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_magX = (float)m_recentData[X_MAG_INDEX];
                    i += 4;

                    // Mag Y
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Y_MAG_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_magY = (float)m_recentData[Y_MAG_INDEX];
                    i += 4;
                    // Mag Z
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Z_MAG_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_magZ = (float)m_recentData[Z_MAG_INDEX];
                    i += 4;
 
                    // latitude
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[LATITUDE_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_latitude = (float)m_recentData[LATITUDE_INDEX];
                    i += 4;
                    // longitude
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[LONGITUDE_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_longitude = (float)m_recentData[LONGITUDE_INDEX];
                    i += 4;
                    // vel_gps
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[VEL_GPS_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_velGPS = (float)m_recentData[VEL_GPS_INDEX];
                    i += 4;
                    // altitudine
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[ALTITUDINE_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_altitudine = (float)m_recentData[ALTITUDINE_INDEX];
                    i += 4;

                    // satellite_number
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[SATELLITE_NUMBER_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_satellite_number= (float)m_recentData[SATELLITE_NUMBER_INDEX];
                    i += 4;

                    // ID dispositivo
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[ID_DISP] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_ID_DISP = (float)m_recentData[ID_DISP];
                    i += 4;

   
                    // crc
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[CRC_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_crc= (float)m_recentData[CRC_INDEX];
                    i += 4;


                    m_q1s = m_q1;
                    m_q2s = m_q2;
                    m_q3s = m_q3;
                    m_q4s = m_q4;
                    float[] Quaternion = new float[] { (float)m_q1, (float)m_q2, (float)m_q3, (float)m_q4 };
                    
                    float pitch, roll,yaw;
                    double phi2 ;
                    double theta2;
                    double psi2 ;

                    float z=Quaternion[0];
                    float y=Quaternion[1];
                    float x=Quaternion[2];
                    float w=Quaternion[3];


                    // Used to determine if near vertical where asin() function is not
                    //defined.
	                double ASIN_LIMIT = 0.9999939;
                           
    
                    yaw = (float)Math.Atan2(2 * (x * y + z * w), 2 * w * w + 2 * x * x - 1);
	                pitch = 2 * (x * z - y * w);
                    roll = 0;   //roll has no meaning if vertical

                    if (pitch < ASIN_LIMIT)
                    {
                        if (pitch > -ASIN_LIMIT)
                        {
                            roll = (float)Math.Atan2(2 * (y * z + x * w), 2 * w * w + 2 * z * z - 1);
                            pitch = 2 * (x * z - y * w);
                            pitch = -(float)Math.Asin(pitch);
                        }
                        else
                        {
                            pitch = -(float)Math.PI / 2;
                        }
                    }
                    else
                    {
                        pitch =(float)Math.PI / 2;
                    }


                        m_rollAngle = yaw * 57.2957795130823;
                        m_pitchAngle = -pitch * 57.2957795130823; 
                        m_yawAngle = -roll *57.2957795130823;
                        roll = -roll;
                        phi2 = (+yaw * Math.Cos(roll) + pitch * Math.Sin(roll)) / 2.0;
                        theta2 = (-yaw * Math.Sin(roll) + pitch * Math.Cos(roll)) / 2.0;

                        psi2 = roll / 2.0;// yaw / 2.0;


                    double sinphi2 = Math.Sin(phi2);
                    double cosphi2 = Math.Cos(phi2);
                    double sintheta2 = Math.Sin(theta2);
                    double costheta2 = Math.Cos(theta2);
                    double sinpsi2 = Math.Sin(psi2);
                    double cospsi2 = Math.Cos(psi2);
                    double cba = 0;//m_q1;
                    double cbb = 0;//m_q2;
                    double cbc = 0;//m_q3;
                    double cbd = 0;//m_q4;

                    cba = cosphi2 * costheta2 * cospsi2 + sinphi2 * sintheta2 * sinpsi2;
                    cbb = sinphi2 * costheta2 * cospsi2 - cosphi2 * sintheta2 * sinpsi2;
                    cbc = cosphi2 * sintheta2 * cospsi2 + sinphi2 * costheta2 * sinpsi2;
                    cbd = cosphi2 * costheta2 * sinpsi2 - sinphi2 * sintheta2 * cospsi2;
                    m_q1 = cba;
                    m_q2 = cbb;
                    m_q3 = cbc;
                    m_q4 = cbd;

                    DataReceivedEvent(act_channels);
        }

        /* **********************************************************************************
        * 
        * Function: private void handle_packet_ony_quat
        * Inputs: byte type, int length, byte[] data
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Handles data packets received over the serial port.
        * 
        * *********************************************************************************/
        private void handle_packet_ony_quat(byte type, int length, byte[] data)
        {


            int act_channels = 0xFFFF;
            int i = 4;


            byte[] Data_tmp = new byte[4];

            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            m_recentData[Q1_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
            m_q1 = (float)m_recentData[Q1_INDEX];
            i += 4;
            // Pitch angle
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            m_recentData[Q2_INDEX] = BitConverter.ToSingle(Data_tmp, 0);

            m_q2 = (float)m_recentData[Q2_INDEX];
            i += 4;

            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            m_recentData[Q3_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
            m_q3 = (float)m_recentData[Q3_INDEX];
            i += 4;

            // quarto campo
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            m_recentData[Q4_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
            m_q4 = (float)m_recentData[Q4_INDEX];
            m_accelX = (float)0.0;
            m_accelY = (float)0.0;
            m_accelZ = (float)0.0;
            m_gyroX = (float)0.0;
            m_gyroY = (float)0.0;
            m_gyroZ = (float)0.0;
            m_magX = (float)0.0;
            m_magY = (float)0.0;
            m_magZ = (float)0.0;
            m_latitude = (float)0.0;
            m_longitude = (float)0.0;
            m_velGPS = (float)0.0;
            m_altitudine = (float)0.0;
            m_satellite_number = (float)0.0;
            m_ID_DISP = (float)0.0;

           // m_crc = (float)m_recentData[CRC_INDEX];

            m_q1s = m_q1;
            m_q2s = m_q2;
            m_q3s = m_q3;
            m_q4s = m_q4;
            float[] Quaternion = new float[] { (float)m_q1, (float)m_q2, (float)m_q3, (float)m_q4 };

            float pitch, roll, yaw;
            double phi2;
            double theta2;
            double psi2;

            float z = Quaternion[0];
            float y = Quaternion[1];
            float x = Quaternion[2];
            float w = Quaternion[3];


            // Used to determine if near vertical where asin() function is not
            //defined.
            double ASIN_LIMIT = 0.9999939;


            yaw = (float)Math.Atan2(2 * (x * y + z * w), 2 * w * w + 2 * x * x - 1);
            pitch = 2 * (x * z - y * w);
            roll = 0;   //roll has no meaning if vertical

            if (pitch < ASIN_LIMIT)
            {
                if (pitch > -ASIN_LIMIT)
                {
                    roll = (float)Math.Atan2(2 * (y * z + x * w), 2 * w * w + 2 * z * z - 1);
                    pitch = 2 * (x * z - y * w);
                    pitch = -(float)Math.Asin(pitch);
                }
                else
                {
                    pitch = -(float)Math.PI / 2;
                }
            }
            else
            {
                pitch = (float)Math.PI / 2;
            }


            m_rollAngle = yaw * 57.2957795130823;
            m_pitchAngle = -pitch * 57.2957795130823;
            m_yawAngle = -roll * 57.2957795130823;
            roll = -roll;
            phi2 = (+yaw * Math.Cos(roll) + pitch * Math.Sin(roll)) / 2.0;
            theta2 = (-yaw * Math.Sin(roll) + pitch * Math.Cos(roll)) / 2.0;

            psi2 = roll / 2.0;// yaw / 2.0;


            double sinphi2 = Math.Sin(phi2);
            double cosphi2 = Math.Cos(phi2);
            double sintheta2 = Math.Sin(theta2);
            double costheta2 = Math.Cos(theta2);
            double sinpsi2 = Math.Sin(psi2);
            double cospsi2 = Math.Cos(psi2);
            double cba = 0;//m_q1;
            double cbb = 0;//m_q2;
            double cbc = 0;//m_q3;
            double cbd = 0;//m_q4;

            cba = cosphi2 * costheta2 * cospsi2 + sinphi2 * sintheta2 * sinpsi2;
            cbb = sinphi2 * costheta2 * cospsi2 - cosphi2 * sintheta2 * sinpsi2;
            cbc = cosphi2 * sintheta2 * cospsi2 + sinphi2 * costheta2 * sinpsi2;
            cbd = cosphi2 * costheta2 * sinpsi2 - sinphi2 * sintheta2 * cospsi2;
            m_q1 = cba;
            m_q2 = cbb;
            m_q3 = cbc;
            m_q4 = cbd;

            DataReceivedEvent(act_channels);
        }



 /* **********************************************************************************
 * 
 * Function: private void handle_packet
 * Inputs: byte type, int length, byte[] data
 * Outputs: None
 * Return Value: None
 * Dependencies: None
 * Description: 
 * 
 * Handles data packets received over the serial port.
 * 
 * *********************************************************************************/
        private void handle_packet_configuration(byte type, int length, byte[] data)
        {

 
            int i = 4;
            // int endid=0;

            byte[] Data_tmp = new byte[4];
            float[] data_tmp_acc_alig = new float [9];
            float[] data_tmp_acc_bias = new float[3];

            float[] data_tmp_mag_alig = new float[9];
            float[] data_tmp_mag_bias = new float[3];

            float[] data_tmp_gyro_alig = new float[9];
            float[] data_tmp_gyro_bias = new float[3];

            float acc_covariance = 0;
            float mag_covariance = 0;
            float[] q_biasr = new float[4];
            float[] quat_initr = new float[4];
            float[] gainr = new float[4];
            float filter_typer = 0;
            float convolution_timer = 0;

            int output_enabler = 0;
            int output_rater = 0;
            int badurater = 0;
            int portr = 0;
            int id_dispositivo = 0;
            int FW_version = 0;

            //acc
            configuration_String="";


            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[2] = BitConverter.ToSingle(Data_tmp, 0);
             i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[3] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[4] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[5] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[6] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[7] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_alig[8] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_bias[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_bias[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_acc_bias[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //mag

            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[3] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[4] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[5] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[6] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[7] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_alig[8] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_bias[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_bias[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_mag_bias[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //gyro

            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[3] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[4] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[5] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[6] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[7] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_alig[8] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_bias[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_bias[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            data_tmp_gyro_bias[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //acc covariance
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            acc_covariance = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //mag covariance
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            mag_covariance = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //quat init
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            quat_initr[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            quat_initr[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            quat_initr[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            quat_initr[3] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //qbias init
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            q_biasr[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            q_biasr[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            q_biasr[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //gain init
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            gainr[0] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            gainr[1] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            gainr[2] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            gainr[3] = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //filter_type_parser
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            filter_typer = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;


            //convolution_time_parser
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            convolution_timer = BitConverter.ToSingle(Data_tmp, 0);
            i += 4;

            //output_enabler
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            output_enabler = BitConverter.ToInt32(Data_tmp, 0);
            i += 4;

            //output_rater
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            output_rater = BitConverter.ToInt32(Data_tmp, 0);
            i += 4;

            //badurater
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            badurater = BitConverter.ToInt32(Data_tmp, 0);
            i += 4;

            //portr
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            portr = BitConverter.ToInt32(Data_tmp, 0);
            i += 4;

            //id_dispositivo
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            id_dispositivo = BitConverter.ToInt32(Data_tmp, 0);
            i += 4;

            //FW_version
            Data_tmp[0] = data[i + 0];
            Data_tmp[1] = data[i + 1];
            Data_tmp[2] = data[i + 2];
            Data_tmp[3] = data[i + 3];
            FW_version = BitConverter.ToInt32(Data_tmp, 0);

            configuration_String += "Id ";

            configuration_String += id_dispositivo.ToString();
            configuration_String += "\n\n";

            configuration_String += "FW_version ";

            configuration_String += FW_version.ToString();
            configuration_String += "\n\n";

            configuration_String += "acc alignment \n";
            for (int j = 0; j <=8 ; j++)
            {
                configuration_String += data_tmp_acc_alig[j].ToString();
                configuration_String += " ";
                if (j == 2 || j == 5)
                {
                    configuration_String += "\n";
                }

            }
            configuration_String += "\n\n";

            configuration_String += "acc bias  \n";
            for (int j = 0; j <=2; j++)
            {
                configuration_String += data_tmp_acc_bias[j].ToString();
                configuration_String += " ";

            }
            configuration_String += "\n\n";

            configuration_String += "mag alignment \n";
            for (int j = 0; j <= 8; j++)
            {
                configuration_String += data_tmp_mag_alig[j].ToString();
                configuration_String += " ";
                if (j == 2 || j == 5)
                {
                    configuration_String += "\n";
                }

            }
            configuration_String += "\n\n";


            configuration_String += "mag bias \n";
            for (int j = 0; j <=2; j++)
            {
                configuration_String += data_tmp_mag_bias[j].ToString();
                configuration_String += " ";

            }
            configuration_String += "\n\n";


            configuration_String += "gyro alignment \n";
            for (int j = 0; j <= 8; j++)
            {
                configuration_String += data_tmp_gyro_alig[j].ToString();
                configuration_String += " ";
                if (j == 2 || j == 5)
                {
                    configuration_String += "\n";
                }

            }
            configuration_String += "\n\n";


            configuration_String += "gyro bias \n";
            for (int j = 0; j <=2; j++)
            {
                configuration_String += data_tmp_gyro_bias[j].ToString();
                configuration_String += "   ";

            }
            configuration_String += "\n\n";

            configuration_String += "q_bias \n";
            for (int j = 0; j <=2; j++)
            {
                configuration_String += q_biasr[j].ToString();
                configuration_String += "   ";

            }
            configuration_String += "\n\n";


            configuration_String += "quat_init \n";
            for (int j = 0; j <=3; j++)
            {
                configuration_String += quat_initr[j].ToString();
                configuration_String += "   ";

            }
            configuration_String += "\n\n";


            configuration_String += "gain \n";
            for (int j = 0; j <=3; j++)
            {
                configuration_String += gainr[j].ToString();
                configuration_String += "   ";

            }
            configuration_String += "\n\n";


            configuration_String += "acc_covariance ";
            configuration_String += acc_covariance.ToString();
            configuration_String += "\n";

            configuration_String += "mag_covariance ";
            configuration_String += mag_covariance.ToString();
            configuration_String += "\n";

            configuration_String += "filter_type ";
            configuration_String += filter_typer.ToString();
            configuration_String += "\n";

            configuration_String += "convolution_time ";
            configuration_String += convolution_timer.ToString();
            configuration_String += "\n";

            configuration_String += "output_enable ";
            configuration_String += output_enabler.ToString();
            configuration_String += "\n";

            configuration_String += "output_rate ";
            configuration_String += output_rater.ToString();
            configuration_String += "\n";

            configuration_String += "badurate ";
            configuration_String += badurater.ToString();
            configuration_String += "\n";

            configuration_String += "port ";
            configuration_String += portr.ToString();
            configuration_String += "\n";

        }

        /* **********************************************************************************
         * 
         * Function: public bool synch()
         * Inputs: None
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Synchronizes the stored AHRS states with actual states on  the device.  There are
         * two flags used by the AHRS class to keep track of synchronization.  For each setting,
         * the _measured flag indicates that the current AHRS state has been retrieved and stored
         * by the class.  The _updated flag indicates that the class' internal state has changed,
         * but that the AHRS itself has not yet been updated with the new data.
         * 
         * The 'synch' function first updates the AHRS by sending COM packets to set all
         * data for which the _updated flag has been set.  Then, all data that has not yet been
         * measured is requested.
         * 
         * If synchronization succeeds, the function returns 'true'.  'false' otherwise.
         * 
         * *********************************************************************************/

        public bool synch()
        {
            bool complete = true;

            if (!connected)
                return false;

            // First, iterate through the PUpdatePending array to determine which AHRS states need
            // to be updated.
            for (int i = 0; i < UpdatePending.Length; i++)
            {
                if (UpdatePending[i] == true)
                {
                    //DataPending[i] = true;

                    // Call UpdateAHRS to send packet to the AHRS to synchronize data
                    if (!updateAHRS(i))
                    {
                        complete = true;
                        DataPending[i] = false;
                        
                    }
                    complete = true;
                    DataPending[i] = false;
                }
            }

          /*  // Now, iterate through PMeasured array to determing which AHRS states have yet to be measured.
            for (int i = 0; i < Measured.Length; i++ )
            {
                if (!Measured[i])
                {
                    DataPending[i] = true;

                    if (!getAHRSState(i))
                    {
                        complete = false;
                        DataPending[i] = false;
                    }
                }
            }*/
            Invalidate();
            return complete;
        }

   


        /* **********************************************************************************
         * 
         * Function: private bool sendPacket
         * Inputs: Packet AHRSPacket - the packet to be transmitted
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Sends the specified packet to the AHRS.
         * 
         * Returns 'true' on success, 'false' otherwise
         * 
         * *********************************************************************************/
        private bool sendPacket(Packet AHRSPacket)
        {
            int i;
            UInt16 checksum;

            if (!connected)
                return false;

            byte[] packet = new byte[AHRSPacket.DataLength + 120];

            // Build packet header
            packet[0] = (byte)'s';
            packet[1] = (byte)'n';
            packet[2] = (byte)'p';
            packet[3] = AHRSPacket.PacketType;
            packet[4] = AHRSPacket.DataLength;

            // Fill data section
            for (i = 0; i < AHRSPacket.DataLength; i++)
            {
                packet[5 + i] = AHRSPacket.Data[i];
            }

            // Compute Checksum
            checksum = ComputeChecksum( packet, 5 + AHRSPacket.DataLength);

            // Add checksum to end of packet
            packet[5 + AHRSPacket.DataLength] = (byte)(checksum >> 8);
            packet[6 + AHRSPacket.DataLength] = (byte)(checksum & 0x0FF);
            packet[7+AHRSPacket.DataLength] = (byte)('\n');
            //packet[8 + AHRSPacket.DataLength] = (byte)('\r');

            // Now write the packet to the serial port
            try
            {
                serialPort.Write(packet, 0,( AHRSPacket.DataLength + 120));
                serialPort.BaseStream.Flush();
                PacketSentEvent((PName)getTypeIndex(AHRSPacket.PacketType), 0);
                System.Threading.Thread.Sleep(1000);
            }
            
            catch
            {
                return false;
            }

            return true;
        }

        public bool sendfwPacket(byte[] packet, long len)
        {
           
            if (!connected)
                return false;
            serialPort.WriteTimeout = 100;
            serialPort.ReadTimeout = 1000;

                try
                {
                    serialPort.Write(packet, 0, (int)len);
                    serialPort.BaseStream.Flush();

                }

                catch
                {
                    return false;
                }
                serialPort.ReadTimeout = 1;
                serialPort.WriteTimeout = 10;


            return true;
        }

        /* **********************************************************************************
         * 
         * Function: private UInt16 ComputeChecksum
         * Inputs: byte packet
         * Outputs: None
         * Return Value: A two-byte checksum
         * Dependencies: None
         * Description: 
         * 
         * Computes the sum of all bytes in the packet and returns a two byte value.
         * 
         * 
         * *********************************************************************************/
        private UInt16 ComputeChecksum(byte[] packet, int length)
        {
            UInt16 sum = 0;
            int i;

            for (i = 0; i < length; i++)
            {
                sum += packet[i];
            }

            return sum;
        }

        /* **********************************************************************************
         * 
         * Function: private bool updateAHRS
         * Inputs: int index
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Causes a packet to be sent to the AHRS to update its state to conform with the internal
         * state of the AHRS class.
         * 
         * Returns 'true' on success, 'false' otherwise
         * 
         * *********************************************************************************/
        private bool updateAHRS(int index)
        {
            Packet AHRSPacket = new Packet();

            byte_conversion_array ftob = new byte_conversion_array();

            if (index == (int)StateName.ACTIVE_CHANNELS)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_ACTIVE_CHANNELS];
                AHRSPacket.DataLength = 2;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                int active_channels = 0;

                if (m_accelZ_active)
                    active_channels |= (1 << 1);
                if (m_accelY_active)
                    active_channels |= (1 << 2);
                if (m_accelX_active)
                    active_channels |= (1 << 3);

                if (m_gyroZ_active)
                    active_channels |= (1 << 4);
                if (m_gyroY_active)
                    active_channels |= (1 << 5);
                if (m_gyroX_active)
                    active_channels |= (1 << 6);

                if (m_magZ_active)
                    active_channels |= (1 << 7);
                if (m_magY_active)
                    active_channels |= (1 << 8);
                if (m_magX_active)
                    active_channels |= (1 << 9);
                if (m_latitude_active)
                    active_channels |= (1 << 10);
                if (m_longitude_active)
                    active_channels |= (1 << 11);
                if (m_velGPS_active)
                    active_channels |= (1 << 12);
                if (m_altitudine_active)
                    active_channels |= (1 << 13);
                if (m_satellite_number_active)
                    active_channels |= (1 << 14);

 
                AHRSPacket.Data[0] = (byte)((active_channels >> 8) & 0x0FF);
                AHRSPacket.Data[1] = (byte)((active_channels) & 0x0FF);

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.ACCEL_ALIGNMENT)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_ACCEL_ALIGNMENT];
                AHRSPacket.DataLength = 36;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_accel_alignment[0, 0];
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_accel_alignment[0, 1];
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_accel_alignment[0, 2];
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                ftob.float0 = m_accel_alignment[1, 0];
                AHRSPacket.Data[12] = ftob.byte0;
                AHRSPacket.Data[13] = ftob.byte1;
                AHRSPacket.Data[14] = ftob.byte2;
                AHRSPacket.Data[15] = ftob.byte3;

                ftob.float0 = m_accel_alignment[1, 1];
                AHRSPacket.Data[16] = ftob.byte0;
                AHRSPacket.Data[17] = ftob.byte1;
                AHRSPacket.Data[18] = ftob.byte2;
                AHRSPacket.Data[19] = ftob.byte3;

                ftob.float0 = m_accel_alignment[1, 2];
                AHRSPacket.Data[20] = ftob.byte0;
                AHRSPacket.Data[21] = ftob.byte1;
                AHRSPacket.Data[22] = ftob.byte2;
                AHRSPacket.Data[23] = ftob.byte3;

                ftob.float0 = m_accel_alignment[2, 0];
                AHRSPacket.Data[24] = ftob.byte0;
                AHRSPacket.Data[25] = ftob.byte1;
                AHRSPacket.Data[26] = ftob.byte2;
                AHRSPacket.Data[27] = ftob.byte3;

                ftob.float0 = m_accel_alignment[2, 1];
                AHRSPacket.Data[28] = ftob.byte0;
                AHRSPacket.Data[29] = ftob.byte1;
                AHRSPacket.Data[30] = ftob.byte2;
                AHRSPacket.Data[31] = ftob.byte3;

                ftob.float0 = m_accel_alignment[2, 2];
                AHRSPacket.Data[32] = ftob.byte0;
                AHRSPacket.Data[33] = ftob.byte1;
                AHRSPacket.Data[34] = ftob.byte2;
                AHRSPacket.Data[35] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.ACCEL_COVARIANCE)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_ACCEL_COVARIANCE];
                AHRSPacket.DataLength = 4;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_accel_covariance;

                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.ACCEL_BIAS)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_ACCEL_BIAS];
                AHRSPacket.DataLength = 12;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];
                ftob.float0 = m_x_accel_bias;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_y_accel_bias;
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_z_accel_bias;
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;


                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.ACCEL_REF)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_ACCEL_REF_VECTOR];
                AHRSPacket.DataLength = 12;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_x_accel_ref;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_y_accel_ref;
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_z_accel_ref;
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.MAG_BIAS)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_MAG_BIAS];
                AHRSPacket.DataLength = 12;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_x_mag_bias;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_y_mag_bias;
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_z_mag_bias;
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.MAG_REF)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_MAG_REF_VECTOR];
                AHRSPacket.DataLength = 12;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_x_mag_ref;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_y_mag_ref;
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_z_mag_ref;
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;


                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.BROADCAST_MODE)
            {
                if (m_broadcast_enabled)
                {
                    AHRSPacket.PacketType = PID[(int)PName.SET_BROADCAST_MODE];
                    AHRSPacket.DataLength = 1;
                    AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                    AHRSPacket.Data[0] = (byte)(m_broadcast_rate);
                }
                else
                {
                    AHRSPacket.PacketType = PID[(int)PName.SET_SILENT_MODE];
                    AHRSPacket.DataLength = 0;
                    AHRSPacket.Data = new byte[AHRSPacket.DataLength];
                }

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.EKF_CONFIG)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_EKF_CONFIG];
                AHRSPacket.DataLength = 1;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                byte data = 0;

                if (m_accel_enabled)
                    data |= 0x02;
                if (m_mag_enabled)
                    data |= 0x01;
                if (m_mag_restriced_to_yaw)
                    data |= 0x04;

                AHRSPacket.Data[0] = data;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.GYRO_ALIGNMENT)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_GYRO_ALIGNMENT];
                AHRSPacket.DataLength = 36;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_gyro_alignment[0, 0];
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[0, 1];
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[0, 2];
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[1, 0];
                AHRSPacket.Data[12] = ftob.byte0;
                AHRSPacket.Data[13] = ftob.byte1;
                AHRSPacket.Data[14] = ftob.byte2;
                AHRSPacket.Data[15] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[1, 1];
                AHRSPacket.Data[16] = ftob.byte0;
                AHRSPacket.Data[17] = ftob.byte1;
                AHRSPacket.Data[18] = ftob.byte2;
                AHRSPacket.Data[19] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[1, 2];
                AHRSPacket.Data[20] = ftob.byte0;
                AHRSPacket.Data[21] = ftob.byte1;
                AHRSPacket.Data[22] = ftob.byte2;
                AHRSPacket.Data[23] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[2, 0];
                AHRSPacket.Data[24] = ftob.byte0;
                AHRSPacket.Data[25] = ftob.byte1;
                AHRSPacket.Data[26] = ftob.byte2;
                AHRSPacket.Data[27] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[2, 1];
                AHRSPacket.Data[28] = ftob.byte0;
                AHRSPacket.Data[29] = ftob.byte1;
                AHRSPacket.Data[30] = ftob.byte2;
                AHRSPacket.Data[31] = ftob.byte3;

                ftob.float0 = m_gyro_alignment[2, 2];
                AHRSPacket.Data[32] = ftob.byte0;
                AHRSPacket.Data[33] = ftob.byte1;
                AHRSPacket.Data[34] = ftob.byte2;
                AHRSPacket.Data[35] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.GYRO_BIAS)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_GYRO_BIAS];
                AHRSPacket.DataLength = 12;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_x_gyro_bias;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_y_gyro_bias;
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_z_gyro_bias;
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;


                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.GYRO_SCALE)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_GYRO_SCALE];
                AHRSPacket.DataLength = 12;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_z_gyro_scale;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_y_gyro_scale;
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_x_gyro_scale;
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.MAG_COVARIANCE)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_MAG_COVARIANCE];
                AHRSPacket.DataLength = 4;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_mag_covariance;
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.PROCESS_COVARIANCE)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_PROCESS_COVARIANCE];
                AHRSPacket.DataLength = 4;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_process_covariance;

                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.START_CAL)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_START_CAL];
                AHRSPacket.DataLength = 1;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                if (m_zero_gyros_on_startup)
                    AHRSPacket.Data[0] = 0x01;
                else
                    AHRSPacket.Data[0] = 0x00;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.ZERO_GYROS)
            {
                AHRSPacket.PacketType = PID[(int)PName.ZERO_RATE_GYROS];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.AUTO_SET_MAG_REF)
            {
                AHRSPacket.PacketType = PID[(int)PName.AUTO_SET_MAG_REF];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.AUTO_SET_ACCEL_REF)
            {
                AHRSPacket.PacketType = PID[(int)PName.AUTO_SET_ACCEL_REF];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.SELF_TEST)
            {
                AHRSPacket.PacketType = PID[(int)PName.SELF_TEST];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.RESET_TO_FACTORY)
            {
                AHRSPacket.PacketType = PID[(int)PName.RESET_TO_FACTORY];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.EKF_RESET)
            {
                AHRSPacket.PacketType = PID[(int)PName.EKF_RESET];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.REBOOT)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_REBOOT];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];
                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.MAG_CAL)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_MAG_CAL];
                AHRSPacket.DataLength = 36;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_mag_cal[0, 0];
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_mag_cal[0, 1];
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_mag_cal[0, 2];
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                ftob.float0 = m_mag_cal[1, 0];
                AHRSPacket.Data[12] = ftob.byte0;
                AHRSPacket.Data[13] = ftob.byte1;
                AHRSPacket.Data[14] = ftob.byte2;
                AHRSPacket.Data[15] = ftob.byte3;

                ftob.float0 = m_mag_cal[1, 1];
                AHRSPacket.Data[16] = ftob.byte0;
                AHRSPacket.Data[17] = ftob.byte1;
                AHRSPacket.Data[18] = ftob.byte2;
                AHRSPacket.Data[19] = ftob.byte3;

                ftob.float0 = m_mag_cal[1, 2];
                AHRSPacket.Data[20] = ftob.byte0;
                AHRSPacket.Data[21] = ftob.byte1;
                AHRSPacket.Data[22] = ftob.byte2;
                AHRSPacket.Data[23] = ftob.byte3;

                ftob.float0 = m_mag_cal[2, 0];
                AHRSPacket.Data[24] = ftob.byte0;
                AHRSPacket.Data[25] = ftob.byte1;
                AHRSPacket.Data[26] = ftob.byte2;
                AHRSPacket.Data[27] = ftob.byte3;

                ftob.float0 = m_mag_cal[2, 1];
                AHRSPacket.Data[28] = ftob.byte0;
                AHRSPacket.Data[29] = ftob.byte1;
                AHRSPacket.Data[30] = ftob.byte2;
                AHRSPacket.Data[31] = ftob.byte3;

                ftob.float0 = m_mag_cal[2, 2];
                AHRSPacket.Data[32] = ftob.byte0;
                AHRSPacket.Data[33] = ftob.byte1;
                AHRSPacket.Data[34] = ftob.byte2;
                AHRSPacket.Data[35] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.SET_KALMAN_PARM)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_KALMAN_PARM];
                AHRSPacket.DataLength = 52;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                ftob.float0 = m_init_quat[0];
                AHRSPacket.Data[0] = ftob.byte0;
                AHRSPacket.Data[1] = ftob.byte1;
                AHRSPacket.Data[2] = ftob.byte2;
                AHRSPacket.Data[3] = ftob.byte3;

                ftob.float0 = m_init_quat[1];
                AHRSPacket.Data[4] = ftob.byte0;
                AHRSPacket.Data[5] = ftob.byte1;
                AHRSPacket.Data[6] = ftob.byte2;
                AHRSPacket.Data[7] = ftob.byte3;

                ftob.float0 = m_init_quat[2];
                AHRSPacket.Data[8] = ftob.byte0;
                AHRSPacket.Data[9] = ftob.byte1;
                AHRSPacket.Data[10] = ftob.byte2;
                AHRSPacket.Data[11] = ftob.byte3;

                ftob.float0 = m_init_quat[3];
                AHRSPacket.Data[12] = ftob.byte0;
                AHRSPacket.Data[13] = ftob.byte1;
                AHRSPacket.Data[14] = ftob.byte2;
                AHRSPacket.Data[15] = ftob.byte3;

                ftob.float0 = m_Q_bias[ 0];
                AHRSPacket.Data[16] = ftob.byte0;
                AHRSPacket.Data[17] = ftob.byte1;
                AHRSPacket.Data[18] = ftob.byte2;
                AHRSPacket.Data[19] = ftob.byte3;

                ftob.float0 = m_Q_bias[1];
                AHRSPacket.Data[20] = ftob.byte0;
                AHRSPacket.Data[21] = ftob.byte1;
                AHRSPacket.Data[22] = ftob.byte2;
                AHRSPacket.Data[23] = ftob.byte3;

                ftob.float0 = m_Q_bias[2];
                AHRSPacket.Data[24] = ftob.byte0;
                AHRSPacket.Data[25] = ftob.byte1;
                AHRSPacket.Data[26] = ftob.byte2;
                AHRSPacket.Data[27] = ftob.byte3;

                ftob.float0 = m_gain[0];
                AHRSPacket.Data[28] = ftob.byte0;
                AHRSPacket.Data[29] = ftob.byte1;
                AHRSPacket.Data[30] = ftob.byte2;
                AHRSPacket.Data[31] = ftob.byte3;

                ftob.float0 = m_gain[1];
                AHRSPacket.Data[32] = ftob.byte0;
                AHRSPacket.Data[33] = ftob.byte1;
                AHRSPacket.Data[34] = ftob.byte2;
                AHRSPacket.Data[35] = ftob.byte3;

                ftob.float0 = m_gain[2];
                AHRSPacket.Data[36] = ftob.byte0;
                AHRSPacket.Data[37] = ftob.byte1;
                AHRSPacket.Data[38] = ftob.byte2;
                AHRSPacket.Data[39] = ftob.byte3;


                ftob.float0 = m_gain[3];
                AHRSPacket.Data[40] = ftob.byte0;
                AHRSPacket.Data[41] = ftob.byte1;
                AHRSPacket.Data[42] = ftob.byte2;
                AHRSPacket.Data[43] = ftob.byte3;

                ftob.float0 = m_filter_type;
                AHRSPacket.Data[44] = ftob.byte0;
                AHRSPacket.Data[45] = ftob.byte1;
                AHRSPacket.Data[46] = ftob.byte2;
                AHRSPacket.Data[47] = ftob.byte3;

                ftob.float0 = m_convolution_time;
                AHRSPacket.Data[48] = ftob.byte0;
                AHRSPacket.Data[49] = ftob.byte1;
                AHRSPacket.Data[50] = ftob.byte2;
                AHRSPacket.Data[51] = ftob.byte3;

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.SET_OUTPUT)
            {
                AHRSPacket.PacketType = PID[(int)PName.SET_OUTPUT];
                AHRSPacket.DataLength = 24;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                AHRSPacket.Data[0] = (byte)((m_output_enable >> 8) & 0x0FF);
                AHRSPacket.Data[1] = (byte)(m_output_enable & 0x0FF);

                AHRSPacket.Data[2] = (byte)((m_output_rate >> 8) & 0x0FF);
                AHRSPacket.Data[3] = (byte)(output_rate & 0x0FF);

                AHRSPacket.Data[4] = (byte)((m_badurate >> 8) & 0x0FF);
                AHRSPacket.Data[5] = (byte)(m_badurate & 0x0FF);

                AHRSPacket.Data[6] = (byte)((m_port >> 8) & 0x0FF);
                AHRSPacket.Data[7] = (byte)(m_port & 0x0FF);
               
                for(int i =0; i<=14; i ++)
                {
                    if (i <= (m_ip.Length-1))
                    AHRSPacket.Data[i+8] = (byte)m_ip[i];
                    else
                        AHRSPacket.Data[i+8]=(byte)' ' ;
                }
                                                   

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.WRITE_TO_FLASH)
            {
                AHRSPacket.PacketType = PID[(int)PName.WRITE_TO_FLASH];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.CALIBRATE_GYRO_BIAS)
            {
                AHRSPacket.PacketType = PID[(int)PName.CALIBRATE_GYRO_BIAS];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }

            else if (index == (int)StateName.UPDATE_FW)
            {
                AHRSPacket.PacketType = PID[(int)PName.UPDATE_FW];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }
            else if (index == (int)StateName.GET_CONFIGURATION)
            {
                AHRSPacket.PacketType = PID[(int)PName.GET_CONFIGURATION];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[AHRSPacket.DataLength];

                sendPacket(AHRSPacket);
            }

            return true;
        }

        /* **********************************************************************************
        * 
        * Function: public void ResetToFactory
        * Inputs: None
        * Outputs: None
        * Return Value: None
        * Dependencies: None
        * Description: 
        * 
        * Causes the AHRS to change all settings back to factory settings.
        * 
        * *********************************************************************************/
        public void ResetToFactory()
        {
            UpdatePending[(int)StateName.RESET_TO_FACTORY] = true;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void ZeroRateGyros
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to begin a ZERO_RATE_GYROS command
         * 
         * *********************************************************************************/
        public void ZeroRateGyros()
        {
            UpdatePending[(int)StateName.ZERO_GYROS] = true;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void AutoSetMagRef
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to set the current magnetic field measurement as the zero yaw
         * reference angle
         * 
         * *********************************************************************************/
        public void AutoSetMagRef()
        {
            UpdatePending[(int)StateName.AUTO_SET_MAG_REF] = true;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void AutoSetAccelRef
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to set the current accel measurement as the zero pitch and roll
         * reference angle
         * 
         * *********************************************************************************/
        public void AutoSetAccelRef()
        {
            UpdatePending[(int)StateName.AUTO_SET_ACCEL_REF] = true;

            synch();
        }


        /* **********************************************************************************
         * 
         * Function: public void SelfTest
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to execute a SELF_TEST command
         * 
         * *********************************************************************************/
        public void SelfTest()
        {
            UpdatePending[(int)StateName.SELF_TEST] = true;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void GetStateCovariance
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to send a STATE_COVARIANCE_REPORT packet
         * 
         * *********************************************************************************/
        public void GetStateCovariance()
        {
            Measured[(int)StateName.STATE_COVARIANCE] = false;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void WriteToFlash
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to write its current settings into FLASH
         * 
         * *********************************************************************************/
        public void WriteToFlash()
        {
            UpdatePending[(int)StateName.WRITE_TO_FLASH] = true;

            synch();
        }
        /* **********************************************************************************
         * 
         * Function: public void automaticgyrobiasingcorrection
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to write its current settings into FLASH
         * 
         * *********************************************************************************/

        public void automaticgyrobiasingcorrection()
        {
            UpdatePending[(int)StateName.CALIBRATE_GYRO_BIAS] = true;

            synch();
        }
        /* **********************************************************************************
         * 
         * Function: public void updatefirmware
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to write its current settings into FLASH
         * 
         * *********************************************************************************/

        public void updatefirmware()
        {
            UpdatePending[(int)StateName.UPDATE_FW] = true;

            synch();
        }

        /* **********************************************************************************
         * 
         * Function: public void EKF_Reset
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to reset the onboard EKF
         * 
         * *********************************************************************************/
        public void EKF_Reset()
        {
            UpdatePending[(int)StateName.EKF_RESET] = true;

            synch();
        }
        /* **********************************************************************************
         * 
         * Function: public void AHRS_GET_CONFIGURATION
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to write its current settings into FLASH
         * 
         * *********************************************************************************/

        public void AHRS_GET_CONFIGURATION()
        {
            UpdatePending[(int)StateName.GET_CONFIGURATION] = true;

            synch();
        }
        /* **********************************************************************************
         * 
         * Function: public void AHRS_reboot
         * Inputs: None
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * Causes the AHRS to write its current settings into FLASH
         * 
         * *********************************************************************************/

        public void AHRS_reboot()
        {
            UpdatePending[(int)StateName.REBOOT ] = true;

            synch();
        }


    }
}
