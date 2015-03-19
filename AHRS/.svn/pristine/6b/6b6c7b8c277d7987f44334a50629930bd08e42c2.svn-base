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
 * File	Name	: loadconf.h
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

#ifndef  _LOADCONF_H_
#define _LOADCONF_H_

#include <stdio.h>
#include <stdlib.h>

typedef struct _load_data_struct{
 
	float acc_alignment_parser[9];
	float acc_bias_parser[3];
	float mag_alignment_parser[9];
	float mag_bias_parser[3];
	float gyro_alignment_parser[9];
	float gyro_bias_parser[3];
	float acc_covariance_parser;
	float mag_covariance_parser;
	float quat_init_parser[4];
	float q_bias_parser[3];
	float gain_parser[4];
	float filter_type_parser;
	float convolution_time_parser;
	int output_enable_parser;
	int output_rate_parser;
	int badurate_parser; 
	int port_parser; 
	//const char ip_address_parser[24];
	}load_data_struct;
	
//-----------------------------------------------------------------
/*!
\brief This routine load configuration file

\return load_data_struct  	: file saved configuration
*/	
load_data_struct load_configuration();
//-----------------------------------------------------------------
/*!
\brief This routine write the configuration in to the file
\parm   load_data_struct writedata  : configuration to write
\return success  	
*/	
int write_configuration(load_data_struct writedata);
//-----------------------------------------------------------------
/*!
\brief This routine restore the base configuration file

\return success  	
*/	
int make_file(void);


#endif

