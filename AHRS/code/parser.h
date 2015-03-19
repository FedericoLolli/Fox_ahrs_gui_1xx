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
 * File	Name	: parser.h
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
#ifndef PARSER_H_
#define PARSER_H_


//----------------command code---------------------//
#define		NO_CMD 				0x00
#define 	WRITE_TOFLASH 		0xA0
#define 	SET_OUTPUT 			0x98
#define 	SET_KALMAN 			0x97
#define 	RESET_TO_FACTORY	0x96
#define 	EKF_RESET			0x95
#define		MAG_BIAS 			0x93
#define		MAG_CAL 			0x92
#define 	ACCEL_ALIGNMENT 	0x8F
#define  	SET_GYRO_BIAS		0x83
#define 	SET_ACCEL_BIAS	 	0x84
#define 	SET_MAG_COVARIANCE	0x8B
#define 	SET_ACCEL_COVARIANCE 0x8C
#define 	SET_GYRO_ALIGNMENT 	0x8E

#define 	CALIBRATE_GYRO_BIAS	0x9B
#define 	UPDATE_FW			0x9C


#define 	GET_CONFIGURATION 	0x9A
#define 	REBOOT 				0x99

#define 	SELF_TEST 			0x88

//----------------error code---------------------//
#define 	UNKNOW_MESSAGE 		-2
#define		WRONG_LEN 			-3
#define 	WRONG_CRC 			-4
#define 	GENERIC_ERROR		-1


extern 	float acc_alignment_parser[9];
extern 	float acc_bias_parser[3];
extern 	float mag_alignment_parser[9];
extern 	float mag_bias_parser[3];
extern 	float gyro_alignment_parser[9];
extern 	float gyro_bias_parser[3];
extern  volatile	float acc_covariance_parser;
extern  volatile	float mag_covariance_parser;
extern 	float quat_init_parser[4];
extern 	float q_bias_parser[3];
extern 	float gain_parser[4];
extern  volatile	float filter_type_parser;
extern  volatile	float convolution_time_parser;
extern  volatile	int output_enable_parser;
extern  volatile	int output_rate_parser;
extern  volatile	int badurate_parser; 
extern  volatile	int port_parser; 
extern 	char ip_address_parser[50];

//-----------------------------------------------------------------
/*!
\brief This routine get serial message
\parm   int fd of serial comunication
\return code message received  	
*/
int parse_input(int fd);
//-----------------------------------------------------------------
/*!
\brief This routine parse a lineament message
\parm   char input_buf[36]
\return float alignment[9] 	
\return success 	
*/
int parse_alignment(char input_buf[36], float alignment[9]);
//-----------------------------------------------------------------
/*!
\brief This routine parse a covariance message
\parm   char input_buf[4]
\return float covariance
*/
float parse_covariance(char *input_buf);
//-----------------------------------------------------------------
/*!
\brief This routine parse a bias message
\parm   char input_buf[12]
\return float bias[9] 	
\return int success
*/
int parse_bias(char *input_buf, float *bias);
//-----------------------------------------------------------------
/*!
\brief This routine parse a output configuration message
\parm   char input_buf
\return int success
*/
int output_set(char *input_buf);
//-----------------------------------------------------------------
/*!
\brief This routine parse a parse kalman config message
\parm   char input_buf
\return int success
*/
int parse_Kalman_Parm(char *input_buf);
//-----------------------------------------------------------------
/*!
\brief This routine parse a parse new firmware message
\parm   int serial fd
\return int success
*/
int recive_new_firmware(int fd);

#endif