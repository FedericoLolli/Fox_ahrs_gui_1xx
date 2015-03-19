/******************************************************************************
 *
 * 						CUSTOM LIBRARY MODULES
 *
 * Copyright (c) 2014 skytech italia
 *
 * All rights reserved. Protected by international copyright laws.
 *
 *  Redistribution and use in source and binary	forms, with or without
 *  modification, are not permitted	without specific prior written permission
 *  by 9D Systeml and provided that the following conditions are met:
 *
 *    Redistributions of source	code must retain the above copyright
 *    notice, this list	of conditions and the following	disclaimer.
 *
 *    Redistributions in binary	form must reproduce the	above copyright	
 *    notice, this list	of conditions and the following	disclaimer in the
 *    documentation and/or other materials provided with the
 *    distribution.
 *
 *    Neither the name of 9D System nor the names of
 *    its contributors may be used to endorse or promote products derived
 *    from this	software without specific prior	written	permission.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS	
 *  "AS	IS" AND	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 *  LIMITED TO,	THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 *  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 *  OWNER OR CONTRIBUTORS BE LIABLE FOR	ANY DIRECT, INDIRECT, INCIDENTAL,
 *  SPECIAL, EXEMPLARY,	OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 *  LIMITED TO,	PROCUREMENT OF SUBSTITUTE GOODS	OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION)	HOWEVER	CAUSED AND ON ANY
 *  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,	OR TORT	
 *  (INCLUDING NEGLIGENCE OR OTHERWISE)	ARISING	IN ANY WAY OUT OF THE USE
 *  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 ******************************************************************************
 */

/*
 ******************************************************************************
 *
 * File	Name	: ahrstest.c
 * Version		: V1.00
 * Created      : sept 3rd, 2014
 * Programmer(s): FL
 *
 ******************************************************************************
 * Description	: 
 *                
 *
 ******************************************************************************
 */

/*
 ******************************************************************************
 *                                  MODULE
 ******************************************************************************
 */
#include <stdint.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <getopt.h>
#include <fcntl.h>
#include <sys/ioctl.h>
#include <linux/types.h>
#include <time.h>
#include <signal.h>
#include <math.h>
#include "geoStars.h"


#include "ahrsacquire.h"
#include "gps.h"
#include "AHRSEKF.h"
#include "seriallib.h"
#include "parser.h"
#include "fir.h"
//#include "loadconf.h"

#define LATITUDINE_FAKE   44.4939
#define LONGITUDINE_FAKE  11.3718
#define ALTITUDINE_FAKE  15

#define GADGETPORT "/dev/ttyGS0"
#define GPSPORT "/dev/ttyS2"
#define MASTERPORT "/dev/ttyS1"

#define DEV_STD_ACC 0.0007999f
#define DEV_STD_MAG 0.0025f
#define EKF_GAIN 0.00009899f

#define QUATINI0T 0.00011f
#define QUATINI1T 0.00011f
#define QUATINI2T 0.00011f
#define QUATINI3T 0.00011f

#define SAMPLINGFREQ 819.0f

#define QBIAS0T 0.0000799f
#define QBIAS1T 0.0000799f
#define QBIAS2T 0.0000799f
#define IDDISP 200


//il sample time é 819
#define GPSTIMEOUT 20
#define SEND_TIME_TIMEOUT 8  //100hz
#define SEND_TIME_TIMEOUT_SERIAL 4 //200hz

#define ACC_BIAS_TIMEOUT  200  //4 hz

#define SAMPLE_TIME  819  //4 hz


#define COMMAND_TIME_TIMEOUT 100
#define MAGDECLINATIONTIMEOUT 80000
#define CONTROL_OUTPUT_CONSISTENCY 800

#define START_BIAS_ESTIMATION 1000


//output enable definition
#define ONLY_QUAT_DATA  					0
#define POSITION_QUAT_DATA  				1
#define ONLY_INERTIAL_DATA  				2
#define POSITION_VEL_QUAT_DATA  			3
#define POSITION_VEL_QUAT_INERTIAL_DATA  	4
#define BASE_DATA  							5


#define LOCALTIME 2014.5

#define CONFIGFILE 1

#define LOST_INERTIAL_DATA_TIMEOUT 3

#define TIME_ELAPS_SEC 0.00121F

#define FW_ACTUAL_VERSION 20

  //static long min_time_elapsed_nanos =1220000;

  static FILE *outfile_raw_data;
  static FILE *outfile_ekf;
  static int fd_serial_gadget,fd_GPS,fd_serial;
  static int debug_raw_data;
  static int fd_gpio=-1;


  void sighandlersigterm(int sig);

// call this function to start a nanosecond-resolution timer
struct timespec timer_start(){
    struct timespec start_time;
	start_time.tv_nsec=0;
	clock_settime(1, &start_time);
    clock_gettime(1, &start_time);
    return start_time;
}

// call this function to end a timer, returning nanoseconds elapsed as a long
long timer_end(struct timespec start_time){
    struct timespec end_time;
    clock_gettime(1, &end_time);
    long diffInNanos = end_time.tv_nsec - start_time.tv_nsec;
    return diffInNanos;
}

void init_correction_data(  float matrixcorrection_acc[12],  float matrixcorrection_gyro[12],  float matrixcorrection_mag[12])
{
	matrixcorrection_acc[0]=1.0;
	matrixcorrection_acc[1]=0.0;
	matrixcorrection_acc[2]=0.0;
	matrixcorrection_acc[3]=0.0;
	matrixcorrection_acc[4]=0.0;
	matrixcorrection_acc[5]=1.0;
	matrixcorrection_acc[6]=0.0;
	matrixcorrection_acc[7]=0.00;
	matrixcorrection_acc[8]=0.0;
	matrixcorrection_acc[9]=0.0;
	matrixcorrection_acc[10]=1.0;
	matrixcorrection_acc[11]=0.0;

	matrixcorrection_gyro[0]=1.0;
	matrixcorrection_gyro[1]=0.0;
	matrixcorrection_gyro[2]=0.0;
	matrixcorrection_gyro[3]=0.0;
	matrixcorrection_gyro[4]=0.0;
	matrixcorrection_gyro[5]=1.0;
	matrixcorrection_gyro[6]=0.0;
	matrixcorrection_gyro[7]=0.0;
	matrixcorrection_gyro[8]=0.0;
	matrixcorrection_gyro[9]=0.0;
	matrixcorrection_gyro[10]=1.0;
	matrixcorrection_gyro[11]=0.0;

	matrixcorrection_mag[0]=1.0;
	matrixcorrection_mag[1]=0.0;
	matrixcorrection_mag[2]=0.0;
	matrixcorrection_mag[3]=0.0;
	matrixcorrection_mag[4]=0.0;
	matrixcorrection_mag[5]=1.0;
	matrixcorrection_mag[6]=0.0;
	matrixcorrection_mag[7]=0.0;
	matrixcorrection_mag[8]=0.0;
	matrixcorrection_mag[9]=0.0;
	matrixcorrection_mag[10]=1.0;
	matrixcorrection_mag[11]=0.0;
}

ahrs_data correction_allinment_data( ahrs_data ahrs_data_out, float matrixcorrection_acc[12],  float matrixcorrection_gyro[12],  float matrixcorrection_mag[12],float tmperature_correction)
{
	float tmp[3];
	tmp[0]=ahrs_data_out.acc[0]*matrixcorrection_acc[0]+ahrs_data_out.acc[1]*matrixcorrection_acc[1]+ahrs_data_out.acc[2]*matrixcorrection_acc[2]-matrixcorrection_acc[3];
	tmp[1]=ahrs_data_out.acc[0]*matrixcorrection_acc[4]+ahrs_data_out.acc[1]*matrixcorrection_acc[5]+ahrs_data_out.acc[2]*matrixcorrection_acc[6]-matrixcorrection_acc[7];
	tmp[2]=ahrs_data_out.acc[0]*matrixcorrection_acc[8]+ahrs_data_out.acc[1]*matrixcorrection_acc[9]+ahrs_data_out.acc[2]*matrixcorrection_acc[10]-matrixcorrection_acc[11];
	ahrs_data_out.acc[0]=tmp[0];
	ahrs_data_out.acc[1]=tmp[1];
	ahrs_data_out.acc[2]=tmp[2];
	//correct mag
	ahrs_data_out.mag[0]=ahrs_data_out.mag[0]-matrixcorrection_mag[3];
	ahrs_data_out.mag[1]=ahrs_data_out.mag[1]-matrixcorrection_mag[7];
	ahrs_data_out.mag[2]=ahrs_data_out.mag[2]-matrixcorrection_mag[11];
	tmp[0]=ahrs_data_out.mag[0]*matrixcorrection_mag[0]+ahrs_data_out.mag[1]*matrixcorrection_mag[1]+ahrs_data_out.mag[2]*matrixcorrection_mag[2];
	tmp[1]=ahrs_data_out.mag[0]*matrixcorrection_mag[4]+ahrs_data_out.mag[1]*matrixcorrection_mag[5]+ahrs_data_out.mag[2]*matrixcorrection_mag[6];
	tmp[2]=ahrs_data_out.mag[0]*matrixcorrection_mag[8]+ahrs_data_out.mag[1]*matrixcorrection_mag[9]+ahrs_data_out.mag[2]*matrixcorrection_mag[10];
	ahrs_data_out.mag[0]=tmp[0];
	ahrs_data_out.mag[1]=tmp[1];
	ahrs_data_out.mag[2]=tmp[2];

	//correct gyro
	tmp[0]=ahrs_data_out.gyro[0]*matrixcorrection_gyro[0]+ahrs_data_out.gyro[1]*matrixcorrection_gyro[1]+ahrs_data_out.gyro[2]*matrixcorrection_gyro[2]-matrixcorrection_gyro[3];
	tmp[1]=ahrs_data_out.gyro[0]*matrixcorrection_gyro[4]+ahrs_data_out.gyro[1]*matrixcorrection_gyro[5]+ahrs_data_out.gyro[2]*matrixcorrection_gyro[6]-matrixcorrection_gyro[7];
	tmp[2]=ahrs_data_out.gyro[0]*matrixcorrection_gyro[8]+ahrs_data_out.gyro[1]*matrixcorrection_gyro[9]+ahrs_data_out.gyro[2]*matrixcorrection_gyro[10]-matrixcorrection_gyro[11];
	ahrs_data_out.gyro[0]=tmp[0]*tmperature_correction;
	ahrs_data_out.gyro[1]=tmp[1]*tmperature_correction;
	ahrs_data_out.gyro[2]=tmp[2]*tmperature_correction;
	return ahrs_data_out;
}




void  main(int argc ,char *argv[] )
{
  ahrs_data ahrs_data_out,ahrs_data_outa;
  NavInputs_t gpsData1;

  all_comunication_data sender;
  quat_comunication_data sender_quat;
  
  postion_vel_quat_inertial_sender postion_vel_quat_inertial;
  postion_vel_quat_sender postion_vel_quat;
  only_inertial_sender only_inertial;
  postion_quat_sender postion_quat;
  
  int32_t i=0;
  uint32_t id_dispositivo=IDDISP;
	
  		/***************counter *********************/ 
  int	counter_send =0;
  int	counter_send_serial =50;

  int	counter_gps =0;
  int   counter_command =0;
  unsigned int counter_magdeclination=0;
  int counter_correct_acc_biasing=20;
  
  int magfirstattemp_gps=1;  //correct mag vector if GPS is fixed at first time
  int max_int=0;
  
  		/***************parser data*********************/	
  unsigned int type_p;
  
  int output_enable;
  int output_rate=200;
  int badurate=115200;; 
  int port=0; 
  char ip_address[50]="";
  int alternate=1;
  int maincomunicationportoutput=SEND_TIME_TIMEOUT_SERIAL;
  
  		/***************matrix correction*********************/	
  float matrixcorrection_acc[12];
  float matrixcorrection_gyro[12];
  float matrixcorrection_mag[12];
  
  float gyro_start_bias[3];
  
    	/***************timer data*********************/	
  struct timespec vartime ;  // begin a timer called 'vartime'
  long time_elapsed_nanos=1250000;
  int cnt=0;
  
  double time_elapsed_second =0.00122;
  float difftime=0;
  
      	/***************mag computation data*********************/
   double dlat, dlon;
   double ati, adec, adip;
   double alt, gv;
   double time, dec, dip, ti;
   double x,y,z,h;
   double ax,ay,az,ah;
   /***************acceleration bias correction*********************/

   double velocity=0,trueangle=0, altitude=0;
   double acc_derivation_linear[3],acc_bias_correction[3], velout[3],omega[3];
   double quat_double[4];
   acc_derivation_linear[0]=0.0;
   acc_derivation_linear[1]=0.0;
   acc_derivation_linear[2]=0.0;
   velout[0]=0.0;
   velout[1]=0.0;
   velout[2]=0.0;
   omega[0]=0.0;
   omega[1]=0.0;
   omega[2]=0.0;
   
  //debug definition
  debug_raw_data =0;
  
  #ifdef CONFIGFILE
  load_data_struct configuration_data;
  #endif
  all_comunication_get_data get_data_buf;

  gpsData1.lat=LATITUDINE_FAKE ;
  gpsData1.lon=LONGITUDINE_FAKE;
  gpsData1.heigth=ALTITUDINE_FAKE;
  gpsData1.speed=0.0;
  gpsData1.gpsFixQuality=0;
  sender.gpsfix=0;
  
    	/*************** ekf data*********************/	
	float dev_stad_accell=DEV_STD_ACC;
	float dev_std_mag=DEV_STD_MAG;
	float gain=EKF_GAIN;
	
	float dt=1/SAMPLINGFREQ;
	float q_quat[4];
	float quat[4];
	float Q_bias[3];
	float gyro_out[3];
	float gyro_biased_out[3];

	
	/*************** control data*********************/	
	unsigned int data_acceleromiter_lost=0;
	float		tmp_accx=0;
	float		tmp_accy=0;
	float		tmp_accz=0;
	
	printf("s.w. version %d date 16/10/2014\n",FW_ACTUAL_VERSION);
	
	system("echo 32 > /sys/class/gpio/export");
	system("echo \"out\" > /sys/class/gpio/pioB0/direction");
	system("echo 1 > /sys/class/gpio/pioB0/value");

	
		/***************configure gadget*********************/	
  printf("inizialize control gadget serial port send  %s \n",GADGETPORT);

  printf("configure gadget\n");
  system("modprobe g_serial");


  printf("open serial gadget port send \n");
  fd_serial_gadget=open_serial_port_com(GADGETPORT, 115200);//115200);
  if( fd_serial_gadget<=-1)
	{
	printf("error to open send serial gadget port\n");
	}

		/***************open GPS*********************/	
  printf("open serial GPS port  \n");
  fd_GPS=open_serial_port_com(GPSPORT, 115200);
  if( fd_GPS<=-1)
	{
	printf("error to open send serial gps port\n");
	}

	usleep(50000);
	
  		/***************init correction data*********************/	

  init_correction_data(   matrixcorrection_acc,  matrixcorrection_gyro,  matrixcorrection_mag);
  
    	/*************** init ekf data*********************/	
	gyro_out[0]=0.0;
	gyro_out[1]=0.0;
	gyro_out[2]=0.0;
	q_quat[0]=QUATINI0T;
	q_quat[1]=QUATINI1T;
	q_quat[2]=QUATINI2T;
	q_quat[3]=QUATINI3T;
	Q_bias[0]=QBIAS0T;
	Q_bias[1]=QBIAS1T;
	Q_bias[2]=QBIAS2T;
	
	/***************read configuration*********************/
	configuration_data.badurate_parser=115200;
		#ifdef CONFIGFILE
	printf("load configuration \n");
	configuration_data=load_configuration();
	if(configuration_data.filter_type_parser==-1)
	{
		printf("configuration unable to read \n");
		
		make_file();
		configuration_data=load_configuration();
		if(configuration_data.filter_type_parser==-1)
		{
			printf("error ren conf file\n ");
			exit(1);
		}
		
	}else
	{
		printf("configuration ok \n");
		matrixcorrection_acc[0]=	configuration_data.acc_alignment_parser[0];
		matrixcorrection_acc[1]=	configuration_data.acc_alignment_parser[1];
		matrixcorrection_acc[2]=	configuration_data.acc_alignment_parser[2];
		matrixcorrection_acc[4]=	configuration_data.acc_alignment_parser[3];
		matrixcorrection_acc[5]=	configuration_data.acc_alignment_parser[4];
		matrixcorrection_acc[6]=	configuration_data.acc_alignment_parser[5];
		matrixcorrection_acc[8]=	configuration_data.acc_alignment_parser[6];
		matrixcorrection_acc[9]=	configuration_data.acc_alignment_parser[7];
		matrixcorrection_acc[10]=	configuration_data.acc_alignment_parser[8];
		
		matrixcorrection_acc[3]=	configuration_data.acc_bias_parser[0];
		matrixcorrection_acc[7]=	configuration_data.acc_bias_parser[1];
		matrixcorrection_acc[11]=	configuration_data.acc_bias_parser[2];
		
		matrixcorrection_mag[0]=configuration_data.mag_alignment_parser[0];
		matrixcorrection_mag[1]=configuration_data.mag_alignment_parser[1];
		matrixcorrection_mag[2]=configuration_data.mag_alignment_parser[2];
		matrixcorrection_mag[4]=configuration_data.mag_alignment_parser[3];
		matrixcorrection_mag[5]=configuration_data.mag_alignment_parser[4];
		matrixcorrection_mag[6]=configuration_data.mag_alignment_parser[5];
		matrixcorrection_mag[8]=configuration_data.mag_alignment_parser[6];
		matrixcorrection_mag[9]=configuration_data.mag_alignment_parser[7];
		matrixcorrection_mag[10]=configuration_data.mag_alignment_parser[8];
		
		matrixcorrection_mag[3]=configuration_data.mag_bias_parser[0];
		matrixcorrection_mag[7]=configuration_data.mag_bias_parser[1];
		matrixcorrection_mag[11]=configuration_data.mag_bias_parser[2];
		
		matrixcorrection_gyro[0]=configuration_data.gyro_alignment_parser[0];
		matrixcorrection_gyro[1]=configuration_data.gyro_alignment_parser[1];
		matrixcorrection_gyro[2]=configuration_data.gyro_alignment_parser[2];
		matrixcorrection_gyro[4]=configuration_data.gyro_alignment_parser[3];
		matrixcorrection_gyro[5]=configuration_data.gyro_alignment_parser[4];
		matrixcorrection_gyro[6]=configuration_data.gyro_alignment_parser[5];
		matrixcorrection_gyro[8]=configuration_data.gyro_alignment_parser[6];
		matrixcorrection_gyro[9]=configuration_data.gyro_alignment_parser[7];
		matrixcorrection_gyro[10]=configuration_data.gyro_alignment_parser[8];
		
		matrixcorrection_gyro[3]=configuration_data.gyro_bias_parser[0];
		matrixcorrection_gyro[7]=configuration_data.gyro_bias_parser[1];
		matrixcorrection_gyro[11]=configuration_data.gyro_bias_parser[2];
		
		dev_stad_accell=configuration_data.acc_covariance_parser;
		dev_std_mag=configuration_data.mag_covariance_parser;
		
		q_quat[0]=configuration_data.quat_init_parser[0];
		q_quat[1]=configuration_data.quat_init_parser[1];
		q_quat[2]=configuration_data.quat_init_parser[2];
		q_quat[3]=configuration_data.quat_init_parser[3];
		
		Q_bias[0]=configuration_data.q_bias_parser[0];
		Q_bias[1]=configuration_data.q_bias_parser[1];
		Q_bias[2]=configuration_data.q_bias_parser[2];

		gain=configuration_data.gain_parser[0];

		
		configuration_data.filter_type_parser=0.0;
		configuration_data.convolution_time_parser=0.0;
		output_enable=configuration_data.output_enable_parser;
		configuration_data.port_parser=0;
		badurate=configuration_data.badurate_parser;

		output_rate=configuration_data.output_rate_parser;
		//strcpy(ip_address,configuration_data.ip_address_parser);//non serve
		
		//advanced debug
		debug_raw_data=(int)configuration_data.gain_parser[1];
	}
		
	#endif
	signal(SIGTERM,sighandlersigterm);
	signal(SIGINT,sighandlersigterm);
	
	/*************** configure master serial port *********************/	
	printf("open serial master port send badu %d\n",badurate);
	if(badurate==0)
	{
		fd_serial=open_serial_port_com(MASTERPORT, 115200);
	}else
	{
		fd_serial=open_serial_port_com(MASTERPORT, 115200);
	}
	if( fd_serial<=-1)
	{
		printf("error to open send serial gadget port\n");
	}
	
	if(output_rate==200)
	{
		printf("set master port output rate 200hz \n");
		maincomunicationportoutput=SEND_TIME_TIMEOUT_SERIAL;
	}
	else	if(output_rate==100)
	{
		printf("set master port output rate 100hz \n");
		maincomunicationportoutput=SEND_TIME_TIMEOUT;
	}
	else
	{
		printf("set master port output rate force 200hz \n");

		maincomunicationportoutput=SEND_TIME_TIMEOUT_SERIAL;
	}
	

	output_enable= configuration_data.output_enable_parser;
	//check if messag is too long for hi speed output rate
	if(output_rate==200)
	{
		if(configuration_data.output_enable_parser>=POSITION_VEL_QUAT_DATA)
		{
			output_enable= ONLY_QUAT_DATA;
		}
	}
	if(output_enable==ONLY_QUAT_DATA)
		printf("master port configured in ONLY_QUAT_DATA\n");
	if(output_enable==POSITION_QUAT_DATA)
		printf("master port configured in POSITION_QUAT_DATA\n");
	if(output_enable==ONLY_INERTIAL_DATA)
		printf("master port configured in ONLY_INERTIAL_DATA\n");
	if(output_enable==POSITION_VEL_QUAT_DATA)
		printf("master port configured in POSITION_VEL_QUAT_DATA\n");
	if(output_enable==POSITION_VEL_QUAT_INERTIAL_DATA)
		printf("master port configured in POSITION_VEL_QUAT_INERTIAL_DATA\n");
	if(output_enable==BASE_DATA)
		printf("master port configured in BASE_DATA\n");		
	
	/*************** init mag declination *********************/
	printf("init mag declination \n");

	dlat=(double) LATITUDINE_FAKE ;
	dlon=(double)LONGITUDINE_FAKE;
	alt= (double)ALTITUDINE_FAKE;
	time=LOCALTIME;
	geoMag(alt,dlat,dlon,time,&dec,&dip,&ti,&gv,&adec,&adip,&ati,&x, &y, &z, &h, &ax, &ay, &az, &ah);
	x=x/ti;
	y=-y/ti;
	z=-z/ti;
	printf("mag output %f %f %f \n",x,y,z);
	
    	/*************** init ekf *********************/
	printf("set mag declination \n");
	set_mag_declination(x,y,z )	;
	if(debug_raw_data==1)
	{
		printf("enter debug mode \n file : /tmp/raw_data.log /tmp/ekf_raw_data.log \n ");
		outfile_raw_data = fopen("/tmp/raw_data.log", "w+");
		fprintf (outfile_raw_data, " q1 , q2 , q3 , q4 , accx , accy , accz , magx , magy , magz , gyrox , gyroy , gyroz , velocity , altitude , trueangle , dt \n ");
		outfile_ekf = fopen("/tmp/ekf_raw_data.log", "w+");
		fprintf (outfile_ekf, " ekf row data out\n ");
		set_file_data(outfile_ekf);
	}
	printf("inizialize quest \n");

	inizialize_quest((double)dev_stad_accell, (double)dev_std_mag, (double)gain );
	

	/***************init device ekf*********************/
	printf("inizialize fir \n");
	
	firFloatInit();
	printf("inizialize acquisition \n");

	init_device_ahrs();

	/*char value_gpio;
	system("echo 46 > /sys/class/gpio/export");
	system("echo \"in\" > /sys/class/gpio/pioB14/direction");
	char buf[100]="/sys/class/gpio/pioB14/value";
	fd_gpio = open(buf, O_RDONLY);*/
	
	/***************init gps*********************/		
	configureS1315sk(fd_GPS);
	printf("start main loop \n");
	while(1)
	{
		//usleep(1);
	    vartime = timer_start(); 
	/***************GPS*********************/		
		if(counter_gps>=GPSTIMEOUT)
		{
			counter_gps =0;
			//usleep(1);
			ReadGPS_data_polling(fd_GPS);
			gpsData1=read_nav_data();
			if(gpsData1.gpsFixQuality>=0)
			{
				sender.latitude=gpsData1.lat;
				sender.longitude=gpsData1.lon;
				sender.vel_gps=gpsData1.speed;
				velocity=gpsData1.speed;
				trueangle=gpsData1.trueangle;
				//sender.altitudine=gpsData1.heigth;
				sender.gpsfix=gpsData1.gpsFixQuality;
			}
  			
		}
		if((gpsData1.lat==0.0)||(gpsData1.lon==0.0))
		{
			gpsData1.lat=LATITUDINE_FAKE ;
			gpsData1.lon=LONGITUDINE_FAKE;
			gpsData1.heigth=ALTITUDINE_FAKE;
			gpsData1.speed=0.0;
			gpsData1.gpsFixQuality=0;
			sender.gpsfix=0;
			sender.latitude=LATITUDINE_FAKE;
			sender.longitude=LONGITUDINE_FAKE;
			sender.vel_gps=0.0;
			sender.altitudine=0;
			sender.gpsfix=0;
		}
	/****************CORRECT MAG DECLINATION*********************/		
	if((counter_magdeclination>=MAGDECLINATIONTIMEOUT)||(magfirstattemp_gps==1))
	{
		counter_magdeclination=0;
		if(sender.gpsfix>=1)
		{
			magfirstattemp_gps=0;
			dlat=(double) gpsData1.lat ;
			dlon=(double)gpsData1.lon;
			alt= (double)gpsData1.heigth;
			time=LOCALTIME;
			//printf("calculate mag lat %f lon %f hi %f \n",gpsData1.lat,gpsData1.lon,gpsData1.heigth);
			geoMag(alt,dlat,dlon,time,&dec,&dip,&ti,&gv,&adec,&adip,&ati,&x, &y, &z, &h, &ax, &ay, &az, &ah);
			x=x/ti;
			y=-y/ti;
			z=-z/ti;
			//printf("mag output recalculated %f %f %f \n",x,y,z);
			set_mag_declination(x,y,z )	;
			
		}
	}
	/****************CALCULATE THE FORCE EFFECT ON ACCELEROMITERS*********************/			
	if(counter_correct_acc_biasing>=ACC_BIAS_TIMEOUT)
	{
		counter_correct_acc_biasing=0;
		if(gpsData1.gpsFixQuality>=1)
		//if(1)
		{
			altitude=(double)get_altitude();
			fir_filter_height(&altitude);

			calculate_acc( (double)velocity, (double)altitude,(double)1/(( SAMPLE_TIME/ACC_BIAS_TIMEOUT)),(double)(trueangle*0.0174532925),acc_derivation_linear,velout);
			//printf("acc derivation %f %f %f \n",acc_derivation_linear[0],acc_derivation_linear[1],acc_derivation_linear[2]);
			//fir_filter_derviate(acc_derivation_linear);
			//printf("acc derivation filtred %f %f %f \n",acc_derivation_linear[0],acc_derivation_linear[1],acc_derivation_linear[2]);
			// non uso le accelerazioni lineari perché di breve durata e troppo poco precise
			//acc_derivation_linear[0]=0.0;
			//acc_derivation_linear[1]=0.0;
			//acc_derivation_linear[2]=0.0;

			//printf("vel %f alt %f trueangle %f \n",velocity,altitude,trueangle);	
			//printf("velout %f %f %f \n",velout[0],velout[1],velout[2]);
			//printf("acc_derivation_linear %f %f %f \n",acc_derivation_linear[0],acc_derivation_linear[1],acc_derivation_linear[2]);
			//printf("acc bias correction %f %f %f \n",acc_bias_correction[0],acc_bias_correction[1],acc_bias_correction[2]);
		}else
		{
			acc_derivation_linear[0]=0.0;
			acc_derivation_linear[1]=0.0;
			acc_derivation_linear[2]=0.0;
			velout[0]=0.0;
			velout[1]=0.0;
			velout[2]=0.0;
		}
	}
	

	/***************ACQUIRE IMU DATA*********************/	
		ahrs_data_outa =acquire_device();
	/***************CORRECT DATA*********************/
		ahrs_data_out =correction_allinment_data(  ahrs_data_outa,  matrixcorrection_acc,   matrixcorrection_gyro,   matrixcorrection_mag,1.0);
		quat_double[0]=(double)quat[0];
		quat_double[1]=(double)quat[1];
		quat_double[2]=(double)quat[2];
		quat_double[3]=(double)quat[3];
		
		omega[0]=(double)gyro_out[0];
		omega[1]=(double)gyro_out[1];
		omega[2]=(double)gyro_out[2];
		fir_filter_gyro(omega);
		accbias_correction(quat_double,acc_derivation_linear,velout,omega,acc_bias_correction);
		fir_filter_corilis_and_linear(acc_bias_correction);
		ahrs_data_out.acc[0]=ahrs_data_out.acc[0]-acc_bias_correction[0];
		ahrs_data_out.acc[1]=ahrs_data_out.acc[1]-acc_bias_correction[1];
		ahrs_data_out.acc[2]=ahrs_data_out.acc[2]-acc_bias_correction[2];
	/***************CALCULATE ELAPSED TIME*********************/			
			
		dt=(float)((double)time_elapsed_nanos)/1000000000.0;
		if(dt>=(0.004))
		{
			dt=0.00122;
			//printf("time error 1\n");
		}
		if(dt<=(0.0002))
		{
			dt=0.00122;
			//printf("time error 2 \n");
		}
		/*if(dt>=(0.0013))
		{
		printf("%f \n",dt);
		}*/
	/***************calculate kalman*********************/
		fir_filter_acc(ahrs_data_out.acc, ahrs_data_out.mag);
		biasgyro_dynamics( ahrs_data_out.gyro,gyro_biased_out);
		EKF(ahrs_data_out.acc, ahrs_data_out.mag,  gyro_biased_out,dt,  q_quat,  Q_bias,  quat,  gyro_out);

		//EKF(ahrs_data_out.acc, ahrs_data_out.mag,  ahrs_data_out.gyro,dt,  q_quat,  Q_bias,  quat,  gyro_out);
		if(debug_raw_data==1)
		{
			fprintf (outfile_raw_data, "%f , %f , %f , %f , %f , %f , %f , %f , %f , %f , %f , %f , %f , %f , %f , %f ,%f\n ",quat[0],quat[1],quat[2],quat[3],ahrs_data_outa.acc[0],ahrs_data_outa.acc[1],ahrs_data_outa.acc[2],ahrs_data_outa.mag[0],ahrs_data_outa.mag[1],ahrs_data_outa.mag[2],ahrs_data_outa.gyro[0],ahrs_data_outa.gyro[1],ahrs_data_outa.gyro[2],velocity,altitude,trueangle,dt);
		}
		//printf("acc %f %f %f \n",ahrs_data_out.acc[0],ahrs_data_out.acc[1],ahrs_data_out.acc[2]);
		//printf("mag %f %f %f \n",ahrs_data_out.mag[0],ahrs_data_out.mag[1],ahrs_data_out.mag[2]);
		//printf("gyro %f %f %f \n",ahrs_data_out.gyro[0],ahrs_data_out.gyro[1],ahrs_data_out.gyro[2]);
		//printf("quat %f %f %f %f \n",quat[0],quat[1],quat[2],quat[3]);
	/***************send MASTER serial connection*********************/	
		if(counter_send_serial>=maincomunicationportoutput)
		{
			if(output_enable==ONLY_QUAT_DATA)
			{
				sender_quat.start='#';
				sender_quat.start1='s';
				sender_quat.start2='n';
				sender_quat.i1=0x00;
				sender_quat.i2=0x00;
				sender_quat.i3=0x00;
				sender_quat.end_f='*';
				sender_quat.q1=quat[0];
				sender_quat.q2=quat[1];
				sender_quat.q3=quat[2];
				sender_quat.q4=quat[3];
				//aggiungere calcolo crc
				serial_send_quat( fd_serial,sender_quat);
			}
			else if(output_enable==POSITION_VEL_QUAT_DATA)
			{
				postion_vel_quat.start='#';
				postion_vel_quat.start1='s';
				postion_vel_quat.start2='n';
				postion_vel_quat.i1=0x03;
				postion_vel_quat.i2=0x00;
				postion_vel_quat.i3=0x00;
				postion_vel_quat.end_f='*';
				postion_vel_quat.q1=quat[0];
				postion_vel_quat.q2=quat[1];
				postion_vel_quat.q3=quat[2];
				postion_vel_quat.q4=quat[3];
				postion_vel_quat.Vx=(float)velout[0];
				postion_vel_quat.Vy=(float)velout[1];
				postion_vel_quat.Vz=(float)velout[2];
				postion_vel_quat.latitude=sender.latitude;
				postion_vel_quat.longitude=sender.longitude;
				postion_vel_quat.altitudine=sender.altitudine;
				//aggiungere calcolo crc
				serial_postion_vel_quat( fd_serial,postion_vel_quat);
			}
			else if(output_enable==POSITION_QUAT_DATA)
			{
				postion_quat.start='#';
				postion_quat.start1='s';
				postion_quat.start2='n';
				postion_quat.i1=0x01;
				postion_quat.i2=0x00;
				postion_quat.i3=0x00;
				postion_quat.end_f='*';
				postion_quat.q1=quat[0];
				postion_quat.q2=quat[1];
				postion_quat.q3=quat[2];
				postion_quat.q4=quat[3];
				postion_quat.latitude=sender.latitude;
				postion_quat.longitude=sender.longitude;
				postion_quat.altitudine=sender.altitudine;
				//aggiungere calcolo crc
				serial_postion_quat( fd_serial,postion_quat);
			}
			
			else if(output_enable==ONLY_INERTIAL_DATA)
			{
				only_inertial.start='#';
				only_inertial.start1='s';
				only_inertial.start2='n';
				only_inertial.i1=0x02;
				only_inertial.i2=0x00;
				only_inertial.i3=0x00;
				only_inertial.end_f='*';
				only_inertial.gyrx=gyro_biased_out[0];
				only_inertial.gyry=gyro_biased_out[1];
				only_inertial.gyrz=gyro_biased_out[2];
				only_inertial.accx=ahrs_data_out.acc[0];
				only_inertial.accy=ahrs_data_out.acc[1];
				only_inertial.accz=ahrs_data_out.acc[2];
				only_inertial.magx=ahrs_data_out.mag[0];
				only_inertial.magy=ahrs_data_out.mag[1];
				only_inertial.magz=ahrs_data_out.mag[2];

				//aggiungere calcolo crc
				serial_only_inertial( fd_serial,only_inertial);
			}
			else if(output_enable==POSITION_VEL_QUAT_INERTIAL_DATA)
			{
				postion_vel_quat_inertial.start='#';
				postion_vel_quat_inertial.start1='s';
				postion_vel_quat_inertial.start2='n';
				postion_vel_quat_inertial.i1=0x04;
				postion_vel_quat_inertial.i2=0x00;
				postion_vel_quat_inertial.i3=0x00;
				postion_vel_quat_inertial.end_f='*';
				postion_vel_quat_inertial.gyrx=gyro_biased_out[0];
				postion_vel_quat_inertial.gyry=gyro_biased_out[1];
				postion_vel_quat_inertial.gyrz=gyro_biased_out[2];
				postion_vel_quat_inertial.accx=ahrs_data_out.acc[0];
				postion_vel_quat_inertial.accy=ahrs_data_out.acc[1];
				postion_vel_quat_inertial.accz=ahrs_data_out.acc[2];
				postion_vel_quat_inertial.magx=ahrs_data_out.mag[0];
				postion_vel_quat_inertial.magy=ahrs_data_out.mag[1];
				postion_vel_quat_inertial.magz=ahrs_data_out.mag[2];
				postion_vel_quat_inertial.q1=quat[0];
				postion_vel_quat_inertial.q2=quat[1];
				postion_vel_quat_inertial.q3=quat[2];
				postion_vel_quat_inertial.q4=quat[3];
				postion_vel_quat_inertial.Vx=(float)velout[0];
				postion_vel_quat_inertial.Vy=(float)velout[1];
				postion_vel_quat_inertial.Vz=(float)velout[2];
				postion_vel_quat_inertial.latitude=sender.latitude;
				postion_vel_quat_inertial.longitude=sender.longitude;
				postion_vel_quat_inertial.altitudine=sender.altitudine;
				//aggiungere calcolo crc
				serial_position_vel_quat_inertial( fd_serial,postion_vel_quat_inertial);
			}
			else if(output_enable==BASE_DATA)
			{
				sender.start='#';
				sender.start1='s';
				sender.start2='n';
				sender.id_dispositivo=id_dispositivo;
				sender.i1=0x00;
				sender.i2=0x00;
				sender.i3=0x00;
				sender.end_f='*';
				sender.q1=quat[0];
				sender.q2=quat[1];
				sender.q3=quat[2];
				sender.q4=quat[3];
				sender.gyrx=gyro_biased_out[0];
				sender.gyry=gyro_biased_out[1];
				sender.gyrz=gyro_biased_out[2];
				
				sender.accx=ahrs_data_out.acc[0];
				sender.accy=ahrs_data_out.acc[1];
				sender.accz=ahrs_data_out.acc[2];
				sender.magx=ahrs_data_out.mag[0];
				sender.magy=ahrs_data_out.mag[1];
				sender.magz=ahrs_data_out.mag[2];
				sender.altitudine=altitude;
			
				/***************calcolo crc*********************/
				sender.crc= calculate_crc_data_sender( sender);
				/***************invio*********************/
				serial_send( fd_serial,sender);
			}
			counter_send_serial=0;
			
			
		}
	/***************send usb connection*********************/	
		if(counter_send>=SEND_TIME_TIMEOUT)
		{
								
			sender.start='#';
			sender.start1='s';
			sender.start2='n';
			sender.id_dispositivo=id_dispositivo;
			sender.i1=0x00;
			sender.i2=0x00;
			sender.i3=0x00;
			sender.end_f='*';
			sender.q1=quat[0];
			sender.q2=quat[1];
			sender.q3=quat[2];
			sender.q4=quat[3];
			sender.gyrx=gyro_biased_out[0];
			sender.gyry=gyro_biased_out[1];
			sender.gyrz=gyro_biased_out[2];
			
			sender.accx=ahrs_data_out.acc[0];
			sender.accy=ahrs_data_out.acc[1];
			sender.accz=ahrs_data_out.acc[2];
			sender.magx=ahrs_data_out.mag[0];
			sender.magy=ahrs_data_out.mag[1];
			sender.magz=ahrs_data_out.mag[2];
			sender.altitudine=altitude;
		
			/***************calcolo crc*********************/
			sender.crc= calculate_crc_data_sender( sender);
			/***************invio*********************/
			serial_send( fd_serial_gadget,sender);

			counter_send=0;
			
		}

		if(counter_command>=COMMAND_TIME_TIMEOUT)
		{

			/***************parsing command*********************/
			if(alternate==1)
			{
				alternate=0;
				type_p=parse_input(fd_serial_gadget );
			}
			else
			{
				alternate=1;
				type_p=parse_input(fd_serial );
			}

			if(type_p==SET_GYRO_ALIGNMENT)
			{
				printf("recived command SET_GYRO_ALIGNMENT %d \n" ,type_p);
				matrixcorrection_gyro[0]=gyro_alignment_parser[0];
				matrixcorrection_gyro[1]=gyro_alignment_parser[1];
				matrixcorrection_gyro[2]=gyro_alignment_parser[2];
				matrixcorrection_gyro[4]=gyro_alignment_parser[3];
				matrixcorrection_gyro[5]=gyro_alignment_parser[4];
				matrixcorrection_gyro[6]=gyro_alignment_parser[5];
				matrixcorrection_gyro[8]=gyro_alignment_parser[6];
				matrixcorrection_gyro[9]=gyro_alignment_parser[7];
				matrixcorrection_gyro[10]=gyro_alignment_parser[8];
				printf("matrixcorrection_gyro\n ");
			    for(i=0; i<11; i++)
			    {
					if((i!=3)&&(i!=7)&&(i!=11))
					{
						printf("%f ",matrixcorrection_gyro[i]);
					}
			    }	
				printf("\n");			   
		   }
			if(type_p==ACCEL_ALIGNMENT)
			{
				printf("recived command ACCEL_ALIGNMENT %d \n" ,type_p);
				matrixcorrection_acc[0]=acc_alignment_parser[0];
				matrixcorrection_acc[1]=acc_alignment_parser[1];
				matrixcorrection_acc[2]=acc_alignment_parser[2];
				matrixcorrection_acc[4]=acc_alignment_parser[3];
				matrixcorrection_acc[5]=acc_alignment_parser[4];
				matrixcorrection_acc[6]=acc_alignment_parser[5];
				matrixcorrection_acc[8]=acc_alignment_parser[6];
				matrixcorrection_acc[9]=acc_alignment_parser[7];
				matrixcorrection_acc[10]=acc_alignment_parser[8];
				printf("matrixcorrection_acc\n ");
			    for(i=0; i<11; i++)
			    {
					if((i!=3)&&(i!=7)&&(i!=11))
					{
					printf("%f ",matrixcorrection_acc[i]);
					}
			    }	
				printf("\n");
			}
			if(type_p==MAG_CAL)
			{
				printf("recived command MAG_CAL %d \n" ,type_p);
				matrixcorrection_mag[0]=mag_alignment_parser[0];
				matrixcorrection_mag[1]=mag_alignment_parser[1];
				matrixcorrection_mag[2]=mag_alignment_parser[2];
				matrixcorrection_mag[4]=mag_alignment_parser[3];
				matrixcorrection_mag[5]=mag_alignment_parser[4];
				matrixcorrection_mag[6]=mag_alignment_parser[5];
				matrixcorrection_mag[8]=mag_alignment_parser[6];
				matrixcorrection_mag[9]=mag_alignment_parser[7];
				matrixcorrection_mag[10]=mag_alignment_parser[8];
				printf("matrixcorrection_mag\n ");
			    for(i=0; i<11; i++)
			    {
					if((i!=3)&&(i!=7)&&(i!=11))
					{
						printf("%f ",matrixcorrection_mag[i]);
					}
			    }	
				printf("\n");
			}
			if(type_p==MAG_BIAS)
			{
				printf("recived command MAG_BIAS %d \n" ,type_p);
				matrixcorrection_mag[3]=mag_bias_parser[0];
				matrixcorrection_mag[7]=mag_bias_parser[1];
				matrixcorrection_mag[11]=mag_bias_parser[2];
				printf("matrixcorrection_mag bias\n ");
			    for(i=3; i<12; i=i+4)
			    {
					printf("%f ",matrixcorrection_mag[i]);
			    }	
				printf("\n");
			}
			if(type_p==SET_GYRO_BIAS)
			{
				printf("recived command SET_GYRO_BIAS %d \n" ,type_p);
				matrixcorrection_gyro[3]=gyro_bias_parser[0];
				matrixcorrection_gyro[7]=gyro_bias_parser[1];
				matrixcorrection_gyro[11]=gyro_bias_parser[2];
				printf("matrixcorrection_gyro bias\n ");
			    for(i=3; i<12; i=i+4)
			    {
					printf("%f ",matrixcorrection_gyro[i]);
			    }	
				printf("\n");
			}
			if(type_p==SET_ACCEL_BIAS)
			{
				printf("recived command SET_ACCEL_BIAS %d \n" ,type_p);
				matrixcorrection_acc[3]=acc_bias_parser[0];
				matrixcorrection_acc[7]=acc_bias_parser[1];
				matrixcorrection_acc[11]=acc_bias_parser[2];
				printf("matrixcorrection_acc bias\n ");
			    for(i=3; i<12; i=i+4)
			    {
					printf("%f ",matrixcorrection_acc[i]);
			    }	
				printf("\n");
			}
			if(type_p==WRITE_TOFLASH)
			{
				printf("recived command WRITE_TOFLASH %d \n" ,type_p);
				#ifdef CONFIGFILE
					configuration_data.acc_alignment_parser[0]=matrixcorrection_acc[0];
					configuration_data.acc_alignment_parser[1]=matrixcorrection_acc[1];
					configuration_data.acc_alignment_parser[2]=matrixcorrection_acc[2];
					configuration_data.acc_alignment_parser[3]=matrixcorrection_acc[4];
					configuration_data.acc_alignment_parser[4]=matrixcorrection_acc[5];
					configuration_data.acc_alignment_parser[5]=matrixcorrection_acc[6];
					configuration_data.acc_alignment_parser[6]=matrixcorrection_acc[8];
					configuration_data.acc_alignment_parser[7]=matrixcorrection_acc[9];
					configuration_data.acc_alignment_parser[8]=matrixcorrection_acc[10];
					
					configuration_data.acc_bias_parser[0]=matrixcorrection_acc[3];
					configuration_data.acc_bias_parser[1]=matrixcorrection_acc[7];
					configuration_data.acc_bias_parser[2]=matrixcorrection_acc[11];
					
					configuration_data.mag_alignment_parser[0]=matrixcorrection_mag[0];
					configuration_data.mag_alignment_parser[1]=matrixcorrection_mag[1];
					configuration_data.mag_alignment_parser[2]=matrixcorrection_mag[2];
					configuration_data.mag_alignment_parser[3]=matrixcorrection_mag[4];
					configuration_data.mag_alignment_parser[4]=matrixcorrection_mag[5];
					configuration_data.mag_alignment_parser[5]=matrixcorrection_mag[6];
					configuration_data.mag_alignment_parser[6]=matrixcorrection_mag[8];
					configuration_data.mag_alignment_parser[7]=matrixcorrection_mag[9];
					configuration_data.mag_alignment_parser[8]=matrixcorrection_mag[10];
					
					configuration_data.mag_bias_parser[0]=matrixcorrection_mag[3];
					configuration_data.mag_bias_parser[1]=matrixcorrection_mag[7];
					configuration_data.mag_bias_parser[2]=matrixcorrection_mag[11];
					
					configuration_data.gyro_alignment_parser[0]=matrixcorrection_gyro[0];
					configuration_data.gyro_alignment_parser[1]=matrixcorrection_gyro[1];
					configuration_data.gyro_alignment_parser[2]=matrixcorrection_gyro[2];
					configuration_data.gyro_alignment_parser[3]=matrixcorrection_gyro[4];
					configuration_data.gyro_alignment_parser[4]=matrixcorrection_gyro[5];
					configuration_data.gyro_alignment_parser[5]=matrixcorrection_gyro[6];
					configuration_data.gyro_alignment_parser[6]=matrixcorrection_gyro[8];
					configuration_data.gyro_alignment_parser[7]=matrixcorrection_gyro[9];
					configuration_data.gyro_alignment_parser[8]=matrixcorrection_gyro[10];
					
					configuration_data.gyro_bias_parser[0]=matrixcorrection_gyro[3];
					configuration_data.gyro_bias_parser[1]=matrixcorrection_gyro[7];
					configuration_data.gyro_bias_parser[2]=matrixcorrection_gyro[11];
					configuration_data.acc_covariance_parser=dev_stad_accell;
					configuration_data.mag_covariance_parser=dev_std_mag;
					
					configuration_data.quat_init_parser[0]=q_quat[0];
					configuration_data.quat_init_parser[1]=q_quat[1];
					configuration_data.quat_init_parser[2]=q_quat[2];
					configuration_data.quat_init_parser[3]=q_quat[3];
					
					configuration_data.q_bias_parser[0]=Q_bias[0];
					configuration_data.q_bias_parser[1]=Q_bias[1];
					configuration_data.q_bias_parser[2]=Q_bias[2];

					configuration_data.gain_parser[0]=gain;
					configuration_data.gain_parser[1]=0.0;
					configuration_data.gain_parser[2]=0.0;
					configuration_data.gain_parser[3]=0.0;
					
					configuration_data.filter_type_parser=0.0;
					configuration_data.convolution_time_parser=0.0;
					
					configuration_data.output_enable_parser=output_enable;
					configuration_data.badurate_parser=badurate_parser;
					configuration_data.port_parser=port_parser;
					//strcpy(configuration_data.ip_address_parser,ip_address);
					write_configuration(configuration_data);
				#endif

			}
			
			if(type_p==CALIBRATE_GYRO_BIAS)
			{
				printf("recived command CALIBRATE_GYRO_BIAS %d \n" ,type_p);
					/* estimate start biasing gyro*/
				printf("inizialize gyro bias \n");

				gyro_start_bias[0]=0;
				gyro_start_bias[1]=0;
				gyro_start_bias[2]=0;
				
				for (i=1; i <= START_BIAS_ESTIMATION; i ++)
				{

					ahrs_data_outa =acquire_device();
					ahrs_data_out =correction_allinment_data(  ahrs_data_outa,  matrixcorrection_acc,   matrixcorrection_gyro,   matrixcorrection_mag,1.0);

					gyro_start_bias[0] =gyro_start_bias[0]+ahrs_data_out.gyro[0];
					gyro_start_bias[1] =gyro_start_bias[1]+ahrs_data_out.gyro[1];
					gyro_start_bias[2] =gyro_start_bias[2]+ahrs_data_out.gyro[2];
					usleep(10000);

				}
				gyro_start_bias[0] =gyro_start_bias[0]/START_BIAS_ESTIMATION;
				gyro_start_bias[1] =gyro_start_bias[1]/START_BIAS_ESTIMATION;
				gyro_start_bias[2] =gyro_start_bias[2]/START_BIAS_ESTIMATION;
				printf("estimated biasing %f %f %f \n",gyro_start_bias[0],gyro_start_bias[1],gyro_start_bias[2]);
				matrixcorrection_gyro[3]=matrixcorrection_gyro[3]+gyro_start_bias[0];
				matrixcorrection_gyro[7]=matrixcorrection_gyro[7]+gyro_start_bias[1];
				matrixcorrection_gyro[11]=matrixcorrection_gyro[11]+gyro_start_bias[2];
				printf("correcting biasing %f %f %f \n",matrixcorrection_gyro[3],matrixcorrection_gyro[7],matrixcorrection_gyro[11]);
				inizialize_quest((double)dev_stad_accell,(double) dev_std_mag, (double)gain );

			}
			if(type_p==RESET_TO_FACTORY)
			{
				printf("recived command RESET_TO_FACTORY %d \n" ,type_p);
				make_file();
				q_quat[0]=QUATINI0T;
				q_quat[1]=QUATINI1T;
				q_quat[2]=QUATINI2T;
				q_quat[3]=QUATINI3T;
				Q_bias[0]=QBIAS0T;
				Q_bias[1]=QBIAS1T;
				Q_bias[2]=QBIAS2T;

				dev_stad_accell=DEV_STD_ACC;
				dev_std_mag=DEV_STD_MAG;
				gain=EKF_GAIN;
				inizialize_quest((double)dev_stad_accell, (double)dev_std_mag, (double)gain );


			}
			if(type_p==EKF_RESET)
			{
				close_device();
				init_device_ahrs();
				printf("recived command EKF_RESET %d \n" ,type_p);
				inizialize_quest((double)dev_stad_accell, (double)dev_std_mag, (double)gain );

			}
			if(type_p==SET_OUTPUT)
			{
				printf("recived command SET_OUTPUT %d \n" ,type_p);
				output_enable= output_enable_parser;
				output_rate= output_rate_parser;
				badurate=badurate_parser; 
				port= port_parser; 
				if(output_enable==ONLY_QUAT_DATA)
					printf("master port configured in ONLY_QUAT_DATA\n");
				else if(output_enable==POSITION_QUAT_DATA)
					printf("master port configured in POSITION_QUAT_DATA\n");
				else if(output_enable==ONLY_INERTIAL_DATA)
					printf("master port configured in ONLY_INERTIAL_DATA\n");
				else if(output_enable==POSITION_VEL_QUAT_DATA)
					printf("master port configured in POSITION_VEL_QUAT_DATA\n");
				else if(output_enable==POSITION_VEL_QUAT_INERTIAL_DATA)
					printf("master port configured in POSITION_VEL_QUAT_INERTIAL_DATA\n");
				else {
					output_enable=ONLY_QUAT_DATA;
					printf("wrong output enable force set to ONLY_QUAT_DATA\n");
				}
				if(output_rate==200)
					printf("output_rate %d \n",output_rate);
				else if(output_rate==100)
					printf("output_rate %d \n",output_rate);
				else{
					output_rate=200;
					printf("wrong output_rate force set to 200\n");
				}
				if(badurate==0)
				{
					printf("badurate %d = 115200\n",badurate);
				}else
				{
					badurate=0;
					printf("wrong badurate force to %d = 115200\n",badurate);
				}
				//memcpy (ip_address ,ip_address_parser,50);
				printf("port %d \n",port);
				//printf("port %s \n",ip_address);

			}
			if(type_p==SET_ACCEL_COVARIANCE)
			{
				printf("recived command SET_ACCEL_COVARIANCE %d \n" ,type_p);
				dev_stad_accell=acc_covariance_parser;
				printf("dev_stad_accell %f \n",dev_stad_accell);
				inizialize_quest((double)dev_stad_accell,(double) dev_std_mag, (double)gain );
			}
			
			if(type_p==UPDATE_FW)
			{
				printf("recived command UPDATE_FW %d failed \n" ,type_p);
			}
			
			if(type_p==SET_MAG_COVARIANCE)
			{
				printf("recived command SET_MAG_COVARIANCE %d \n" ,type_p);
				dev_std_mag=mag_covariance_parser;
				printf("dev_std_mag %f \n",dev_std_mag);
				inizialize_quest((double)dev_stad_accell, (double)dev_std_mag, (double)gain );
			}
			if(type_p==SET_KALMAN)
			{
				printf("recived command SET_KALMAN %d \n" ,type_p);
				q_quat[0]=quat_init_parser[0];
				q_quat[1]=quat_init_parser[1];
				q_quat[2]=quat_init_parser[2];
				q_quat[3]=quat_init_parser[3];
				Q_bias[0]=q_bias_parser[0];
				Q_bias[1]=q_bias_parser[1];
				Q_bias[2]=q_bias_parser[2];
				gain=gain_parser[0];
				printf("Q_bias\n ");
			    for(i=0; i<3; i++)
			    {
					printf("%f ",Q_bias[i]);
			    }	
				printf("\n");
				printf("q_quat\n ");
			    for(i=0; i<4; i++)
			    {
					printf("%f ",q_quat[i]);
			    }	
				printf("\n");
				printf ("gain %f \n",gain);
				inizialize_quest((double)dev_stad_accell,(double) dev_std_mag, (double)gain );
			}
			if(type_p==GET_CONFIGURATION)
			{
			
				printf("recived command GET_CONFIGURATION %d \n" ,type_p);
				
				get_data_buf.start='#';
				get_data_buf.start1='s';
				get_data_buf.start2='p';
				get_data_buf.ia='p';

				get_data_buf.data.acc_alignment_parser[0]=matrixcorrection_acc[0];
				get_data_buf.data.acc_alignment_parser[1]=matrixcorrection_acc[1];
				get_data_buf.data.acc_alignment_parser[2]=matrixcorrection_acc[2];
				get_data_buf.data.acc_alignment_parser[3]=matrixcorrection_acc[4];
				get_data_buf.data.acc_alignment_parser[4]=matrixcorrection_acc[5];
				get_data_buf.data.acc_alignment_parser[5]=matrixcorrection_acc[6];
				get_data_buf.data.acc_alignment_parser[6]=matrixcorrection_acc[8];
				get_data_buf.data.acc_alignment_parser[7]=matrixcorrection_acc[9];
				get_data_buf.data.acc_alignment_parser[8]=matrixcorrection_acc[10];
				
				get_data_buf.data.acc_bias_parser[0]=matrixcorrection_acc[3];
				get_data_buf.data.acc_bias_parser[1]=matrixcorrection_acc[7];
				get_data_buf.data.acc_bias_parser[2]=matrixcorrection_acc[11];
				
				get_data_buf.data.mag_alignment_parser[0]=matrixcorrection_mag[0];
				get_data_buf.data.mag_alignment_parser[1]=matrixcorrection_mag[1];
				get_data_buf.data.mag_alignment_parser[2]=matrixcorrection_mag[2];
				get_data_buf.data.mag_alignment_parser[3]=matrixcorrection_mag[4];
				get_data_buf.data.mag_alignment_parser[4]=matrixcorrection_mag[5];
				get_data_buf.data.mag_alignment_parser[5]=matrixcorrection_mag[6];
				get_data_buf.data.mag_alignment_parser[6]=matrixcorrection_mag[8];
				get_data_buf.data.mag_alignment_parser[7]=matrixcorrection_mag[9];
				get_data_buf.data.mag_alignment_parser[8]=matrixcorrection_mag[10];
				
				get_data_buf.data.mag_bias_parser[0]=matrixcorrection_mag[3];
				get_data_buf.data.mag_bias_parser[1]=matrixcorrection_mag[7];
				get_data_buf.data.mag_bias_parser[2]=matrixcorrection_mag[11];
				
				get_data_buf.data.gyro_alignment_parser[0]=matrixcorrection_gyro[0];
				get_data_buf.data.gyro_alignment_parser[1]=matrixcorrection_gyro[1];
				get_data_buf.data.gyro_alignment_parser[2]=matrixcorrection_gyro[2];
				get_data_buf.data.gyro_alignment_parser[3]=matrixcorrection_gyro[4];
				get_data_buf.data.gyro_alignment_parser[4]=matrixcorrection_gyro[5];
				get_data_buf.data.gyro_alignment_parser[5]=matrixcorrection_gyro[6];
				get_data_buf.data.gyro_alignment_parser[6]=matrixcorrection_gyro[8];
				get_data_buf.data.gyro_alignment_parser[7]=matrixcorrection_gyro[9];
				get_data_buf.data.gyro_alignment_parser[8]=matrixcorrection_gyro[10];
				
				get_data_buf.data.gyro_bias_parser[0]=matrixcorrection_gyro[3];
				get_data_buf.data.gyro_bias_parser[1]=matrixcorrection_gyro[7];
				get_data_buf.data.gyro_bias_parser[2]=matrixcorrection_gyro[11];
				get_data_buf.data.acc_covariance_parser=dev_stad_accell;
				get_data_buf.data.mag_covariance_parser=dev_std_mag;
				
				get_data_buf.data.quat_init_parser[0]=QUATINI0T;
				get_data_buf.data.quat_init_parser[1]=QUATINI1T;
				get_data_buf.data.quat_init_parser[2]=QUATINI2T;
				get_data_buf.data.quat_init_parser[3]=QUATINI3T;
				
				get_data_buf.data.q_bias_parser[0]=QBIAS0T;
				get_data_buf.data.q_bias_parser[1]=QBIAS1T;
				get_data_buf.data.q_bias_parser[2]=QBIAS2T;

				get_data_buf.data.gain_parser[0]=gain;
				get_data_buf.data.gain_parser[1]=0.0;
				get_data_buf.data.gain_parser[2]=0.0;
				get_data_buf.data.gain_parser[3]=0.0;
				
				get_data_buf.data.filter_type_parser=0.0;
				get_data_buf.data.convolution_time_parser=0.0;
				get_data_buf.data.output_enable_parser=output_enable;
				get_data_buf.data.badurate_parser=badurate;
				get_data_buf.data.output_rate_parser=output_rate;
								
				get_data_buf.data.port_parser=port_parser;
				//strcpy(get_data_buf.data.ip_address_parser,ip_address);
				
				get_data_buf.id_dispositivo=id_dispositivo;
				get_data_buf.FW_version=FW_ACTUAL_VERSION;				
				get_data_buf.crc=0;
				
				get_data_buf.ib=0x00;
				get_data_buf.ic=0x00;
				get_data_buf.id=0x01;
				get_data_buf.end_f='*';
				serial_send_get_data (fd_serial_gadget,get_data_buf);
				printf("send %d byte \n", sizeof(get_data_buf));

			}
			if(type_p==REBOOT)
			{
				printf("recived command REBOOT %d \n" ,type_p);
				printf("rebooting\n");
				system("reboot -f");
				exit(1);

			}
			if(type_p==UNKNOW_MESSAGE)
			{
				printf("recived command UNKNOW_MESSAGE %d \n" ,type_p);
			}
			if(type_p==WRONG_LEN)
			{
				printf("recived command WRONG_LEN %d \n" ,type_p);
			}
			if(type_p==WRONG_CRC)
			{
				printf("recived command WRONG_CRC %d \n" ,type_p);
			}
			type_p=NO_CMD;
			counter_command =0;;
			
		}
		counter_magdeclination ++;		
		counter_command ++;
		counter_send ++;
		counter_gps ++;
		cnt ++;
		counter_send_serial ++;
		counter_correct_acc_biasing ++;
		
		/***************reset main counter*********************/
		if(cnt==CONTROL_OUTPUT_CONSISTENCY)
		{
			//printf("Time taken (nanoseconds): %ld\n", time_elapsed_nanos);
			//printf("processati 800 dati dt %f \n",dt);
			cnt=0;
			if((quat[0]==FP_NAN)||(quat[1]==FP_NAN)||(quat[2]==FP_NAN)||(quat[3]==FP_NAN)||(quat[0]==FP_INFINITE)||(quat[1]==FP_INFINITE)||(quat[2]==FP_INFINITE)||(quat[3]==FP_INFINITE))
			{
				printf("output NAN  \n");
				inizialize_quest((double)dev_stad_accell, (double)dev_std_mag, (double)gain );
			}
			if((ahrs_data_outa.acc[0]==tmp_accx)&&(ahrs_data_outa.acc[1]==tmp_accy)&&(ahrs_data_outa.acc[2]==tmp_accz))
			{
				data_acceleromiter_lost ++;
				if(data_acceleromiter_lost>LOST_INERTIAL_DATA_TIMEOUT)
				{
					printf("reset device inertial data lost\n");
					close_device();
					init_device_ahrs();
					inizialize_quest((double)dev_stad_accell, (double)dev_std_mag, (double)gain );
					data_acceleromiter_lost=0;
				}
			}
			else
			{
				data_acceleromiter_lost=0;
			}
			tmp_accx=ahrs_data_outa.acc[0];
			tmp_accy=ahrs_data_outa.acc[1];
			tmp_accz=ahrs_data_outa.acc[2];
		}
		
		/***************timer *********************/
		time_elapsed_nanos = timer_end(vartime);
		time_elapsed_second=((double)time_elapsed_nanos)/1000000000.0;
		difftime=time_elapsed_second-TIME_ELAPS_SEC;
		max_int=0;
		while((difftime<=0.0))
		{
			//usleep(1);
			max_int++;
			ReadGPS_data_polling(fd_GPS);
			time_elapsed_nanos = timer_end(vartime);
			time_elapsed_second=((double)time_elapsed_nanos)/1000000000.0;
			difftime=time_elapsed_second-TIME_ELAPS_SEC;
			if(difftime>=0.0001)
			{
				difftime=0.1;
			}
			if(difftime<=-0.003)
			{
				difftime=0.1;
			}
		}

	}
	printf("closing n \n");
	if(debug_raw_data==1)
	{
		fclose(outfile_raw_data);
		fclose(outfile_ekf);
	}
	close( fd_serial_gadget);
	close( fd_serial);

	close(fd_GPS);
	close(fd_gpio);
	close_device();
	exit(1);
}

void sighandlersigterm(int sig){
	printf("closing \n");
	if(debug_raw_data==1)
	{
		fclose(outfile_raw_data);
		fclose(outfile_ekf);
	}
	close( fd_serial_gadget);
	close( fd_serial);

	close(fd_GPS);
	close(fd_gpio);
	close_device();
	exit(1);
}
