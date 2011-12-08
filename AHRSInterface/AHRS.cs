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

    public delegate void COMFailedDelegate();

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
        SET_SILENT_MODE,
         GET_DATA,
        GET_ACTIVE_CHANNELS,
      
        // Packets that can be received by the AHRS class
        COMMAND_COMPLETE,
        COMMAND_FAILED,
	    BAD_CHECKSUM,
	    BAD_DATA_LENGTH,
	    UNRECOGNIZED_PACKET,
	    BUFFER_OVERFLOW,
	    STATUS_REPORT,
	    SENSOR_DATA,

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
        RESET_TO_FACTORY,
        SELF_TEST,
        EKF_RESET,
        SENSOR_DATA,
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

            // Commands that can be received by the AHRS class
            // (ie. transmitted by the device, received by the class
            // over a serial port).

            // Set AHRS class parameters so that on the next call to 'synch', the class
            // will attempt to acquire all internal states of the actual AHRS device.
            Invalidate();

            // Setup timer for keeping track of time elapsed between packet transmission and reception of response.
            PacketTimer.Interval = 100;      // 10 ms delay
            PacketTimer.Enabled = true;
            PacketTimer.Tick += new System.EventHandler(OnPacketTimerTick);

            RXbuffer = new byte[RX_BUF_SIZE];

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
        // A COMFailedEvent occurs when the AHRS class expects to receive data from the
        // AHRS device, but no data is received in the max. allowed time.
        public event StateDelegate PacketTimeoutEvent;
        
        // Data for communication
        private bool connected;
        private SerialPort serialPort;
        private Timer PacketTimer;

        const int RX_BUF_SIZE = 600;
        public byte[] RXbuffer { get; set;  }
        private int RXbufPtr;
        const int MAX_PACKET_SIZE = 300;

        const int CHANNEL_COUNT = 37;

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
        const int X_VEL_INDEX = 15;
        const int Y_VEL_INDEX = 16;
        const int Z_VEL_INDEX = 17;
        const int A_Q_INDEX = 17;
        const int B_Q_INDEX = 18;
        const int C_Q_INDEX = 19;
        const int D_Q_INDEX = 20;
        const int A_QDOT_INDEX = 21;
        const int B_QDOT_INDEX = 22;
        const int C_QDOT_INDEX = 23;
        const int D_QDOT_INDEX = 24;
        const int LATITUDE_INDEX = 25;
        const int LONGITUDE_INDEX = 26;
        const int VEL_GPS_INDEX = 27;
        const int ALTITUDINE_INDEX = 28;
        const int STATIC_PRESSURE_INDEX = 29;
        const int TIC_INDEX = 30;
        const int TEMPERATURE_INDEX = 31;
        const int GPS_INDEX = 32;
        const int SATELLITE_NUMBER_INDEX = 33;
        const int NUMBER_OF_PACKET_INDEX = 34;
        const int ID_DISPOSITIVO_INDEX = 35;
        const int CRC_INDEX = 36;


        // Array for storing the most recent data obtained from the AHRS
        private float[] m_recentData = new float[CHANNEL_COUNT];

        private double m_pitchAngle;
        private double m_rollAngle;
        private double m_yawAngle;
        private double m_magX;
        private double m_magY;
        private double m_magZ;
        private double m_gyroX;
        private double m_gyroY;
        private double m_gyroZ;
        private double m_accelX;
        private double m_accelY;
        private double m_accelZ;
        private double m_velX;
        private double m_velY;
        private double m_velZ;
        private double m_q1;
        private double m_q2;
        private double m_q3;
        private double m_q4;
        private double m_q1_dot;
        private double m_q2_dot;
        private double m_q3_dot;
        private double m_q4_dot;
        private double m_latitude;
        private double m_longitude;
        private double m_velGPS;
        private double m_altitudine;
        private double m_static_pressure;
        private double m_tic;
        private double m_temperature;
        private double m_gps;
        private double m_satellite_number;
        private double m_number_of_packet;
        private double m_id_dispositivo;
        private double m_crc;


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

        private bool m_velX_active;
        private bool m_velY_active;
        private bool m_velZ_active;
        private bool m_q1_active;
        private bool m_q2_active;
        private bool m_q3_active;
        private bool m_q4_active;
        private bool m_q1_dot_active;
        private bool m_q2_dot_active;
        private bool m_q3_dot_active;
        private bool m_q4_dot_active;
        private bool m_latitude_active;
        private bool m_longitude_active;
        private bool m_velGPS_active;
        private bool m_altitudine_active;
        private bool m_static_pressure_active;
        private bool m_tic_active;
        private bool m_temperature_active;
        private bool m_gps_active;
        private bool m_satellite_number_active;
        private bool m_number_of_packet_active;
        private bool m_id_dispositivo_active;
        private bool m_crc_active;



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

        public double velX
        {
            get { return m_velX; }
        }
        public double velY
        {
            get { return m_velY; }
        }
        public double velZ
        {
            get { return m_velZ; }
        }

        public double q1
        {
            get { return m_q1; }
        }
        public double q2
        {
            get { return m_q2; }
        }
        public double q3
        {
            get { return m_q3; }
        }
        public double q4
        {
            get { return m_q4; }
        }
       public double q1_dot
        {
            get { return m_q1_dot; }
        }
       public double q2_dot
        {
            get { return m_q2_dot; }
        }
        public double q3_dot
        {
            get { return m_q3_dot; }
        }
       public double q4_dot
        {
            get { return m_q4_dot; }
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
       public double static_pressure
        {
            get { return m_static_pressure; }
        }
       public double tic
        {
            get { return m_tic; }
        }
       public double temperature
        {
            get { return m_temperature; }
        }
       public double gps
        {
            get { return m_gps; }
        }
       public double satellite_number
        {
            get { return m_satellite_number; }
        }
       public double number_of_packet
        {
            get { return m_number_of_packet; }
        }
       public double id_dispositivo
        {
            get { return m_id_dispositivo; }
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

        public bool velX_active
        {
            get { return m_velX_active; }
            set { m_velX_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool velY_active
        {
            get { return m_velY_active; }
            set { m_velY_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool velZ_active
        {
            get { return m_velZ_active; }
            set { m_velZ_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q1_active
        {
            get { return m_q1_active; }
            set { m_q1_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q2_active
        {
            get { return m_q2_active; }
            set { m_q2_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q3_active
        {
            get { return m_q3_active; }
            set { m_q3_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q4_active
        {
            get { return m_q4_active; }
            set { m_q4_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q1_dot_active
        {
            get { return m_q1_dot_active; }
            set { m_q1_dot_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q2_dot_active
        {
            get { return m_q2_dot_active; }
            set { m_q2_dot_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q3_dot_active
        {
            get { return m_q3_dot_active; }
            set { m_q3_dot_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool q4_dot_active
        {
            get { return m_q4_dot_active; }
            set { m_q4_dot_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
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
        public bool static_pressure_active
        {
            get { return m_static_pressure_active; }
            set { m_static_pressure_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool tic_active
        {
            get { return m_tic_active; }
            set { m_tic_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool temperature_active
        {
            get { return m_temperature_active; }
            set { m_temperature_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool gps_active
        {
            get { return m_gps_active; }
            set { m_gps_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool satellite_number_active
        {
            get { return m_satellite_number_active; }
            set { m_satellite_number_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool number_of_packet_active
        {
            get { return m_number_of_packet_active; }
            set { m_number_of_packet_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool id_dispositivo_active
        {
            get { return m_id_dispositivo_active; }
            set { m_id_dispositivo_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
        }
        public bool crc_active
        {
            get { return m_crc_active; }
            set { m_crc_active = value; UpdatePending[(int)StateName.ACTIVE_CHANNELS] = true; }
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
                MaxDelay[i] = 100;
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
            serialPort.Handshake = Handshake.None;

            try
            {
                //now open the port
                serialPort.Open();

                connected = true;

                // Add event handler
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                return true;
            }
            catch
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
         * 
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

            try
            {
                bytes_to_read = serialPort.BytesToRead;

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
                serialPort.DiscardInBuffer();
                RXbufPtr += bytes_to_read;
            }
            catch
            {
                COMFailedEvent();
                return;
            }

            bool found_packet;
            int packet_start_index;
            int packet_index;

            // If there are enough bytes in the buffer to construct a full packet, then check data.
            // There are RXbufPtr bytes in the buffer at any given time
            while (RXbufPtr >= 7 && (continue_parsing == 1))
            {
                // Search for the packet start sequence
                found_packet = false;
                packet_start_index = 0;
                for (packet_index = 0; packet_index < (RXbufPtr - 2); packet_index++)
                {
                    
                    //if (RXbuffer[packet_index] == '#'  && RXbuffer[packet_index + 110] == '*')
                    if (RXbuffer[packet_index] == '#' && RXbuffer[packet_index + 1] == 's' && RXbuffer[packet_index + 2] == 'n')
                    {
                        found_packet = true;
                        packet_start_index = packet_index;
                        
                        break;
                    }
                }

                // If start sequence found, try to recover all the data in the packet
                if (found_packet && ((RXbufPtr - packet_start_index) >= 7))
                {
                    byte packet_type = 1;
                    //dimensione fissa del pacchetto
                    Int16 data_size = 148;
                    byte[] data = new byte[MAX_PACKET_SIZE];

                    // Only process packet if data_size is not too large.
                    if (data_size <= MAX_PACKET_SIZE)
                    {

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
     
                            // Copy all received bytes that weren't part of this packet into the beginning of the
                            // buffer.  Then, reset RXbufPtr.
                           // for (int index = 0; index < (buffer_length - packet_length); index++)
                           // {
                            //    RXbuffer[index] = RXbuffer[(packet_start_index + packet_length) + index];
                           // }

                           // RXbufPtr = (buffer_length - packet_length);
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
//                        AppendStatusText("\r\nBAD PACKET", Color.Red);
                    }
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

        /* **********************************************************************************
         * 
         * Function: private void updatePacketSynch
         * Inputs: PName packet
         * Outputs: None
         * Return Value: None
         * Dependencies: None
         * Description: 
         * 
         * The AHRS class keeps track of packets it has sent and received - if a packet requesting
         * data is sent to the AHRS device, then the class will watch to receive the data.
         * If the data is not received in MaxDelay milliseconds, then a PacketTimeoutEvent is
         * fired (from elsewhere in the class, not from the updatePacketSynch function).
         * 
         * Timing is tracked using several arrays:
         * The ElapsedTime[] array keeps track of how much time has elapsed since a packet
         * requesting data was sent.
         * The DataPending[] array keeps track of which channels have "pending" data - ie.
         * which channels the AHRS class is expecting to receive data on.
         * The Measured[] array keeps track of which data has been received at least once
         * from the AHRS - this allows the AHRS class to synchronize itself with the device.
         * The UpdatePending[] arrays keeps track of which internal class data has been changed,
         * but has not yet been written to the AHRS.
         * 
         * Whenever a new packet is received from the AHRS device, the DataPending array
         * needs to be updated to reflect the data that was just received - if data was received,
         * then it is no longer "pending."
         * 
         * When a packet is received, the Measured[] array should also be updated - we've
         * received data from the channel.  The ElapsedTime[] array for the received data should
         * also be set to 0.
         * 
         * The updatePacketSynch function performs the aforementioned tasks.
         * 
         * *********************************************************************************/
        private void updatePacketSynch(PName packet, byte[] data)
        {
            switch (packet)
            {


                case PName.COMMAND_COMPLETE:
                    int type_index = getTypeIndex(data[0]);
                
                    switch((PName)type_index)
                    {
                       

                        case PName.SET_ACTIVE_CHANNELS:
                            updatePacketSynchHelper(StateName.ACTIVE_CHANNELS);
                            break;

  
                        case PName.SET_SILENT_MODE:
                            updatePacketSynchHelper(StateName.BROADCAST_MODE);
                            break;


                        default:
                            break;
                    }

                    break;

  
                case PName.STATUS_REPORT:
                    updatePacketSynchHelper(StateName.SELF_TEST);
                    break;

                case PName.SENSOR_DATA:
                    updatePacketSynchHelper(StateName.SENSOR_DATA);
                    break;


                default:
                    break;
            }

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




                    int act_channels = 0xFFFF; //(data[0] << 8) | (data[1]);
                    int i = 4;
                 
                    // Roll angle
                  

                    byte[] Data_tmp=new byte[4];

                    Data_tmp[0]=data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[ROLL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_rollAngle = (float)m_recentData[ROLL_INDEX] ;
                    Console.WriteLine("roll {0}", m_rollAngle.ToString());
                    i += 4;
                    // Pitch angle
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[PITCH_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_pitchAngle = (float)m_recentData[PITCH_INDEX];
                    i += 4;
                    Console.WriteLine("pitch {0}", m_pitchAngle.ToString("f"));
                    // Yaw angle
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];

                    m_recentData[YAW_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_yawAngle = (float)m_recentData[YAW_INDEX];
                    Console.WriteLine("yaw {0}", m_yawAngle.ToString("f"));
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
                    // vel x
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[X_VEL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_velX = (float)m_recentData[Z_VEL_INDEX];
                    i += 4;
                    // vel x
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Y_VEL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_velY = (float)m_recentData[Y_VEL_INDEX];
                    i += 4;
                    // vel x
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[Z_VEL_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_velZ = (float)m_recentData[Z_VEL_INDEX];
                    i += 4;
                    // q1
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[A_Q_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q1 = (float)m_recentData[A_Q_INDEX];
                    i += 4;
                    // q2
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[B_Q_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q2 = (float)m_recentData[B_Q_INDEX];
                    i += 4;
                    // q3
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[C_Q_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q3 = (float)m_recentData[C_Q_INDEX];
                    i += 4;
                    // q4
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[D_Q_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q4 = (float)m_recentData[D_Q_INDEX];
                    i += 4;
                    // q1_dot
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[A_QDOT_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q1_dot = (float)m_recentData[A_QDOT_INDEX];
                    i += 4;
                    // q2_dot
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[B_QDOT_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q2_dot = (float)m_recentData[B_QDOT_INDEX];
                    i += 4;
                    // q3_dot
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[C_QDOT_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q3_dot = (float)m_recentData[C_QDOT_INDEX];
                    i += 4;
                    // q4_dot
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[D_QDOT_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_q4_dot = (float)m_recentData[D_QDOT_INDEX];
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
                    // static_pressure
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[STATIC_PRESSURE_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_static_pressure= (float)m_recentData[STATIC_PRESSURE_INDEX];
                    i += 4;
                    // tic
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[TIC_INDEX] = BitConverter.ToSingle(Data_tmp, 0);
                    m_tic= (float)m_recentData[TIC_INDEX];
                    i += 4;
                    // temperature
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[TEMPERATURE_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_temperature= (float)m_recentData[TEMPERATURE_INDEX];
                    i += 4;
                    // gps
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[GPS_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_gps= (float)m_recentData[GPS_INDEX];
                    i += 4;
                    // satellite_number
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[SATELLITE_NUMBER_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_satellite_number= (float)m_recentData[SATELLITE_NUMBER_INDEX];
                    i += 4;
                    // NUMBER_OF_PACKET_INDEX
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[NUMBER_OF_PACKET_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_number_of_packet= (float)m_recentData[NUMBER_OF_PACKET_INDEX];
                    i += 4;
                    // ID_DISPOSITIVO_INDEX
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[ID_DISPOSITIVO_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_id_dispositivo= (float)m_recentData[ID_DISPOSITIVO_INDEX];
                    i += 4;
                    // crc
                    Data_tmp[0] = data[i + 0];
                    Data_tmp[1] = data[i + 1];
                    Data_tmp[2] = data[i + 2];
                    Data_tmp[3] = data[i + 3];
                    m_recentData[CRC_INDEX] = (float)BitConverter.ToInt32(Data_tmp, 0);
                    m_crc= (float)m_recentData[CRC_INDEX];
                    i += 4;

                    DataReceivedEvent(act_channels);

            

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
                if (UpdatePending[i])
                {
                    DataPending[i] = true;

                    // Call UpdateAHRS to send packet to the AHRS to synchronize data
                    if (!updateAHRS(i))
                    {
                        complete = false;
                        DataPending[i] = false;
                        
                    }
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

            return complete;
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

                if (m_roll_active)
                    active_channels |= (1 << 13);
                if (m_pitch_active)
                    active_channels |= (1 << 14);
                if (m_yaw_active)
                    active_channels |= (1 << 15);

                AHRSPacket.Data[0] = (byte)((active_channels >> 8) & 0x0FF);
                AHRSPacket.Data[1] = (byte)((active_channels) & 0x0FF);

                //sendPacket(AHRSPacket);
            }
 

            return true;
        }

        /* **********************************************************************************
         * 
         * Function: private bool getAHRSState
         * Inputs: int index
         * Outputs: None
         * Return Value: bool success
         * Dependencies: None
         * Description: 
         * 
         * Causes a packet to be sent to the AHRS device requesting device information.  The
         * request packet to be sent is specified by 'index'.
         * 
         * Returns 'true' on success, 'false' otherwise
         * 
         * *********************************************************************************/
      /*  private bool getAHRSState(int index)
        {
            Packet AHRSPacket = new Packet();

            if (index == (int)StateName.ACTIVE_CHANNELS)
            {
                AHRSPacket.PacketType = PID[(int)PName.GET_ACTIVE_CHANNELS];
                AHRSPacket.DataLength = 0;
                AHRSPacket.Data = new byte[0];

                if (!sendPacket(AHRSPacket))
                    return false;                    
            }
  
            return true;
        }*/

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
       /* private bool sendPacket( Packet AHRSPacket )
        {
            int i;
            UInt16 checksum;

            if (!connected)
                return false;

            byte[] packet = new byte[AHRSPacket.DataLength + 7];

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

            // Now write the packet to the serial port
            try
            {
                serialPort.Write(packet, 0, AHRSPacket.DataLength + 7);
                
                PacketSentEvent((PName)getTypeIndex(AHRSPacket.PacketType), 0);
            }
            catch
            {
                return false;
            }

            return true;
        }*/

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




    }
}
