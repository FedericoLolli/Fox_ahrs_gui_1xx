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
 * File	Name	: parser.c
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


#include <unistd.h>
#include <stdio.h>		
#include <stdlib.h>
#include <fcntl.h>
#include <errno.h>
#include <fcntl.h>
#include <sys/wait.h>
#include <signal.h>
#include <stdio.h> 
#include <termios.h>
#include "parser.h"

#include "comunication_descriptor_data.h"

//#define DEBUGPARSER 1

#define 	WRITE_TOFLASH_LEN 			0
#define 	SET_OUTPUT_LEN  			24
#define 	SET_KALMAN_LEN  			52
#define 	RESET_TO_FACTORY_LEN 		0
#define 	EKF_RESET_LEN 				0
#define		MAG_BIAS_LEN  				12
#define		MAG_CAL_LEN  				36
#define 	ACCEL_ALIGNMENT_LEN  		36
#define  	SET_GYRO_BIAS_LEN 			12
#define 	SET_ACCEL_BIAS_LEN 	 		12
#define 	SET_MAG_COVARIANCE_LEN 		4
#define 	SET_ACCEL_COVARIANCE_LEN  	4
#define 	SET_GYRO_ALIGNMENT_LEN  	36

#define BUF_DIMENSION_UPDATE 500000
#define MAX_DIMENSION_UPDATE 5500000
#define MAX_iterations 5500000


typedef union Data_float
{
   char i[4];
   float f;
   int a;
}data_float ;  

float acc_alignment_parser[9];
float acc_bias_parser[3];
float mag_alignment_parser[9];
float mag_bias_parser[3];
float gyro_alignment_parser[9];
float gyro_bias_parser[3];

volatile	float acc_covariance_parser;
volatile	float mag_covariance_parser;
float quat_init_parser[4];
float q_bias_parser[3];
float gain_parser[4];
volatile	float filter_type_parser;
volatile	float convolution_time_parser;
volatile	int output_enable_parser;
volatile	int output_rate_parser;
volatile	int badurate_parser; 
volatile	int port_parser; 
char ip_address_parser[50];


		/***************serial parsing main function*********************/\
//-----------------------------------------------------------------
/*!
\brief This routine get serial message
\parm   int fd of serial comunication
\return code message received  	
*/	
int parse_input(int fd )
{
    int i=0,nr_read=0;
    char crc[2];
    char c=0x00,type;
	int success_fw=-1;
	char input_buf[100];

	uint8_t  data_lenght;
    nr_read = read(fd, &c, 1);
	
    if( (c == 's')&&(nr_read==1))
	{
		//printf("read s ");
		nr_read = read(fd, &c, 1);
		if ((c == 'n') &&(nr_read==1))
		{
			//printf("read n ");
			nr_read = read(fd, &c, 1);
			if ((c == 'p')&&(nr_read==1)) 
			{
				//printf("read p \n");
				nr_read = read(fd, &c, 1);
				if(nr_read!=1)
				{
					return GENERIC_ERROR;
				}
				type=0x00;
				type=c;
				#ifdef DEBUGPARSER
				printf("tipo %X ",c);
				#endif 
				nr_read = read(fd, &c, 1);
				if(nr_read!=1)
				{
					return GENERIC_ERROR;
				}
				data_lenght=0x00;
				data_lenght=c;
				#ifdef DEBUGPARSER
				printf("len %d ",c);
				#endif
				nr_read = read(fd, input_buf, (int)data_lenght);
				#ifdef DEBUGPARSER
				printf("data read %d \n",nr_read);
				#endif
				//for(i=0; i<nr_read; i++)
			   //{
				//printf("%X \n",input_buf[i]);
			   //}
				if(nr_read<=(data_lenght-1))
				{
					
					return WRONG_LEN;
				}
				nr_read = read(fd, crc, 2);
				//printf("crc read %d \n",nr_read);
				if(nr_read<=0)
				{
					
					return WRONG_LEN;
				}else
				{
					//inserire controllo cecksum
					//acc
					if(type==ACCEL_ALIGNMENT)
					{
					   #ifdef DEBUGPARSER
					   printf("parsing acc allinment \n");
					   #endif
					   i=parse_alignment(input_buf, acc_alignment_parser);
					   #ifdef DEBUGPARSER
					   for(i=0; i<9; i++)
					   {
						printf("%f \n",acc_alignment_parser[i]);
					   }
					   #endif
					   return ACCEL_ALIGNMENT;
					}
					if(type==SET_ACCEL_BIAS)
					{
					   i=parse_bias(input_buf, acc_bias_parser);
					   #ifdef DEBUGPARSER
					   printf("parsing acc allinment \n");
					   for(i=0; i<3; i++)
					   {
						printf("%f \n",acc_bias_parser[i]);
					   }
					   #endif
					   return SET_ACCEL_BIAS;
					}
					//mag
					if(type==MAG_CAL)
					{
					   i=parse_alignment(input_buf, mag_alignment_parser);
					   #ifdef DEBUGPARSER
					   printf("parsing mag allinment \n");
					   for(i=0; i<9; i++)
					   {
						printf("%f \n",mag_alignment_parser[i]);
					   }
					   #endif
					   return MAG_CAL;
					}
					if(type==MAG_BIAS)
					{
					   i=parse_bias(input_buf, mag_bias_parser);
					   #ifdef DEBUGPARSER
					   printf("parsing mag allinment \n");
					   for(i=0; i<3; i++)
					   {
						printf("%f \n",mag_bias_parser[i]);
					   }
					   #endif
					   	return MAG_BIAS;
					}
					//gyro
					if(type==SET_GYRO_ALIGNMENT)
					{
					   i=parse_alignment(input_buf, gyro_alignment_parser);
					   #ifdef DEBUGPARSER
					   printf("parsing goyr allinment \n");
					   for(i=0; i<9; i++)
					   {
						printf("%f \n",gyro_alignment_parser[i]);
					   }
					   #endif
					   	return SET_GYRO_ALIGNMENT;

					}
					if(type==SET_GYRO_BIAS)
					{
					   i=parse_bias(input_buf, gyro_bias_parser);
					   #ifdef DEBUGPARSER
					   printf("parsing gyro bias \n");
					   for(i=0; i<3; i++)
					   {
						printf("%f \n",gyro_bias_parser[i]);
					   }
					   #endif
					   return SET_GYRO_BIAS;
					}
					if(type==RESET_TO_FACTORY)
					{
						#ifdef DEBUGPARSER
						printf("reset to factory \n");
						#endif
					   return RESET_TO_FACTORY;

					}
					if(type==EKF_RESET)
					{
						#ifdef DEBUGPARSER
						printf("ekf reset \n");
						#endif
					   return EKF_RESET;
					}
					if(type==WRITE_TOFLASH)
					{
						#ifdef DEBUGPARSER
						printf("WRITE_TOFLASH \n");
						#endif
					   return WRITE_TOFLASH;
					}
					if(type==CALIBRATE_GYRO_BIAS)
					{
						#ifdef DEBUGPARSER
						printf("CALIBRATE_GYRO_BIAS \n");
						#endif
					   return CALIBRATE_GYRO_BIAS;
					}
					if(type==UPDATE_FW)
					{
						#ifdef DEBUGPARSER
						printf("UPDATE_FW \n");
						#endif
						printf("UPDATE_FW \n");
						success_fw=recive_new_firmware(fd);
						if(success_fw==1)
						{
						//reboot
							printf("going to reboot \n");
							sleep(1);
							system("reboot -f");

						}

					   return UPDATE_FW;
					}
					
					if(type==SET_MAG_COVARIANCE)
					{
						mag_covariance_parser=parse_covariance(input_buf);
						#ifdef DEBUGPARSER
						printf("parsing SET_MAG_COVARIANCE ");
						printf("%f \n",mag_covariance_parser);
						#endif
					   return SET_MAG_COVARIANCE;
					}
					
					if(type==SET_ACCEL_COVARIANCE)
					{
						acc_covariance_parser=parse_covariance(input_buf);
						#ifdef DEBUGPARSER
						printf("parsing SET_ACCEL_COVARIANCE ");
						printf("%f \n",acc_covariance_parser);
						#endif
					   return SET_ACCEL_COVARIANCE;
					}
					if(type==SET_KALMAN)
					{
						i=parse_Kalman_Parm(input_buf);
						#ifdef DEBUGPARSER
						printf("parsing SET_KALMAN ");
					   for(i=0; i<4; i++)
					   {
						printf("quat_init %f \n",quat_init_parser[i]);
					   }
					   	for(i=0; i<4; i++)
					   {
						printf("gain %f \n",gain_parser[i]);
					   }
					   for(i=0; i<3; i++)
					   {
						printf("q_bias %f \n",q_bias_parser[i]);
						
					   }
					   	printf("filter_type %f \n",filter_type_parser);
						printf("convolution_time %f \n",convolution_time_parser);
						#endif
					   return SET_KALMAN;
					}
					if(type==SET_OUTPUT)
					{
						i=output_set(input_buf);
						#ifdef DEBUGPARSER
						printf("parsing SET_OUTPUT ");
					   	printf("output_enable %d \n",output_enable_parser);
						printf("output_rate %d \n",output_rate_parser);
						printf("badurate %d \n",badurate_parser);
						printf("port %d \n",port_parser);
						printf("port %s \n",ip_address_parser);
						#endif
					   return SET_OUTPUT;
					}
					if(type==GET_CONFIGURATION)
					{
						#ifdef DEBUGPARSER
						printf("parsing GET_CONFIGURATION ");
						#endif
					   return GET_CONFIGURATION;
					}
					if(type==REBOOT)
					{
						#ifdef DEBUGPARSER
						printf("parsing REBOOT ");
						#endif
					   return REBOOT;
					}
					
					return UNKNOW_MESSAGE;
				}

			}
		}
	}
	else
	{
		//tcflush(fd,TCIFLUSH);
	}
	return GENERIC_ERROR;
    /* check for string "boot" in input buffer */
    /* maybe string compare? */
}

		/***************prende in ingresso 36 char e li trasforma in 9 float*********************/
//-----------------------------------------------------------------
/*!
\brief This routine parse a lineament message
\parm   char input_buf[36]
\return float alignment[9] 	
\return success 	
*/
int parse_alignment(char input_buf[36], float alignment[9])
{
	int i;
	data_float tmp;
	for(i=0; i<=35; i=i+4)
	{
		tmp.i[3]=input_buf[i];
		tmp.i[2]=input_buf[i+1];
		tmp.i[1]=input_buf[i+2];
		tmp.i[0]=input_buf[i+3];
		alignment[(i/4)]=tmp.f;
	}
	
	return 1;
}
		/***************prende in ingresso 4 char e li trasforma in 1 float*********************/
//-----------------------------------------------------------------
/*!
\brief This routine parse a covariance message
\parm   char input_buf[4]
\return float covariance
*/
float parse_covariance(char *input_buf)
{
	
	data_float tmp;
	tmp.i[3]=input_buf[0];
	tmp.i[2]=input_buf[1];
	tmp.i[1]=input_buf[2];
	tmp.i[0]=input_buf[3];
	return tmp.f;
	
}
		/***************prende in ingresso 12 char e li trasforma in 3 float*********************/
//-----------------------------------------------------------------
/*!
\brief This routine parse a bias message
\parm   char input_buf[12]
\return float bias[9] 	
\return int covariance
*/
int parse_bias(char *input_buf, float *bias)
{
	int i;
	data_float tmp;
	for(i=0; i<=11; i=i+4)
	{
		tmp.i[3]=input_buf[i];
		tmp.i[2]=input_buf[i+1];
		tmp.i[1]=input_buf[i+2];
		tmp.i[0]=input_buf[i+3];
		bias[(i/4)]=tmp.f;
	}
	
	return 1;
}
		/***************calman par parsing*********************/
//-----------------------------------------------------------------
/*!
\brief This routine parse a parse kalman config message
\parm   char input_buf
\return int covariance
*/
int parse_Kalman_Parm(char *input_buf)
{
	int i;
	int cnt_int=0;
	data_float tmp;
	for(i=0; i<=12; i=i+4)
	{
		tmp.i[3]=input_buf[i];
		tmp.i[2]=input_buf[i+1];
		tmp.i[1]=input_buf[i+2];
		tmp.i[0]=input_buf[i+3];
		quat_init_parser[cnt_int]=tmp.f;
		cnt_int++;
	}
	cnt_int=0;
	for(i=16; i<=24; i=i+4)
	{
		tmp.i[3]=input_buf[i];
		tmp.i[2]=input_buf[i+1];
		tmp.i[1]=input_buf[i+2];
		tmp.i[0]=input_buf[i+3];
		q_bias_parser[cnt_int]=tmp.f;
		cnt_int++;
	}
	cnt_int=0;
	for(i=28; i<=40; i=i+4)
	{
		tmp.i[3]=input_buf[i];
		tmp.i[2]=input_buf[i+1];
		tmp.i[1]=input_buf[i+2];
		tmp.i[0]=input_buf[i+3];
		gain_parser[cnt_int]=tmp.f;
		cnt_int++;
	}
		tmp.i[3]=input_buf[44];
		tmp.i[2]=input_buf[45];
		tmp.i[1]=input_buf[46];
		tmp.i[0]=input_buf[47];
		filter_type_parser=tmp.f;
		tmp.i[3]=input_buf[48];
		tmp.i[2]=input_buf[49];
		tmp.i[1]=input_buf[50];
		tmp.i[0]=input_buf[51];
		convolution_time_parser=tmp.f;
	
	
	return 1;
}
		/***************output set parsing*********************/
//-----------------------------------------------------------------
/*!
\brief This routine parse a output configuration message
\parm   char input_buf
\return int covariance
*/
int output_set(char *input_buf)
{
	int i;
	data_float tmp;

	tmp.i[1]=input_buf[0];
	tmp.i[0]=input_buf[1];
	tmp.i[2]=0x00;
	tmp.i[3]=0x00;
	output_enable_parser=tmp.a;
	printf("%d \n",tmp.a);
	tmp.i[1]=input_buf[2];
	tmp.i[0]=input_buf[3];
	tmp.i[2]=0x00;
	tmp.i[3]=0x00;
	output_rate_parser=tmp.a;
	printf("%d \n",tmp.a);

	tmp.i[1]=input_buf[4];
	tmp.i[0]=input_buf[5];
	tmp.i[2]=0x00;
	tmp.i[3]=0x00;
	badurate_parser=tmp.a;
	printf("%d \n",tmp.a);

	tmp.i[1]=input_buf[6];
	tmp.i[0]=input_buf[7];
	tmp.i[2]=0x00;
	tmp.i[3]=0x00;
	port_parser=tmp.a;
	printf("%d \n",tmp.a);

	for(i=0; i<=14; i ++)
	{
		ip_address_parser[i]=input_buf[i+8];
	}
	ip_address_parser[15]='\0';
	

	return 1;
}

//-----------------------------------------------------------------
/*! \brief This routine read the serial data for update the system

 \param  [GLOBAL] fd	:  COM port handle
 \return :
			1 ok crate the /home/ahrsupdate.tar.gz
			-1 update failed
			
\note - .

*/
// questa funzione salva in memoria il file tar.gz per l'update
//-----------------------------------------------------------------
/*!
\brief This routine parse a parse new firmware message
\parm   int serial fd
\return int success
*/
int recive_new_firmware(int fd)
{
	char input_buf[BUF_DIMENSION_UPDATE+10];
	char input_tmp[MAX_DIMENSION_UPDATE];
	char new_byte;
	data_float tmp;
	unsigned int len=0,i=0,j=0, k=0,maxnuminteration=0;
    int nr_read=0;;
	FILE *outfile_fw;
	i=0;
	usleep(100000);
  	tcflush(fd,TCIFLUSH);
	printf("start download new firmware \n");
	while(i<=3)
	{
		nr_read = read(fd, &new_byte,1);
		//printf(" %X , %d \n",input_buf[0],nr_read);
		if(nr_read==1)
		{
			input_buf[i]=new_byte;
			i++;
			maxnuminteration ++;
		}else
		{
			maxnuminteration ++;
		}
		if (maxnuminteration>=MAX_DIMENSION_UPDATE)
		{
			printf("error download fw version len \n");
			return -1;
		}
		//usleep(1);
	}
	//printf(" %d %X %X %X %X \n",nr_read,input_buf[0],input_buf[1],input_buf[2],input_buf[3]);

	if(i>=3)
	{
		tmp.i[3]=input_buf[0];
		tmp.i[2]=input_buf[1];
		tmp.i[1]=input_buf[2];
		tmp.i[0]=input_buf[3];
		len=tmp.a;
		printf("create new file len %d \n",len);
		if((len<=0)||(len>=MAX_DIMENSION_UPDATE))
		{
			printf("error download fw version dimension  \n");
			return -1;
		}
		i=0;
		i=len;
		while((i>=1)&&(maxnuminteration<=MAX_iterations))
		{
			nr_read = read(fd, input_buf, BUF_DIMENSION_UPDATE);
			if(nr_read>=1)
			{
				i=i-nr_read;
				for(j=0;j<=(nr_read-1); j++)
				{

					input_tmp[k]=input_buf[j];
					k ++;
				}
				//printf(" r %d , w %d ",i, k);
			}

			maxnuminteration ++;
		}
	}
	else
	{
		printf("error download fw version dimension  \n");
		return -1;
	}
	//aggiungere controllo CS
	system("rm /home/update.tar");
	system("sync");
	usleep(5000);

	printf("new fw downloaded \n");
	outfile_fw = fopen("/home/update.tar", "w+");
	fwrite ( input_tmp, 1, len, outfile_fw);
	fclose(outfile_fw);
	system("sync");

	return 1;
}

