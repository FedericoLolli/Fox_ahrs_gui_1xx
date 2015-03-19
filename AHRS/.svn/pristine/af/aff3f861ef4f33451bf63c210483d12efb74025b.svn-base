
 /******************************************************************************
 *
 * 						CUSTOM LIBRARY MODULES
 *
 * Copyright (c) 2013 skytech italia
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
 * File	Name	: 
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

#ifndef COMUNICATION_DESCRIPTOR_DATA_H_
#define COMUNICATION_DESCRIPTOR_DATA_H_


#include "loadconf.h"

           


typedef unsigned char uint8_t;
typedef unsigned int uint32_t;
//typedef  char int8_t;
//typedef  int int32_t;
  		/***************sender sender all data usb mode *********************/ 

typedef struct all_comunication_data_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t gpsfix;
	float q1;
	float q2;
	float q3;
	float q4;	
	float accx;
	float accy;
	float accz;
	float gyrx;
	float gyry;
	float gyrz;		
	float magx;
	float magy;
	float magz;		
	float latitude;
	float longitude;
	float vel_gps;//m/s
	float altitudine;//m
	uint32_t satellite_number;
	uint32_t id_dispositivo;
	uint32_t crc;
	uint8_t i1;
	uint8_t i2;
	uint8_t i3;
	uint8_t end_f;

}all_comunication_data;

  		/***************sender only quaternion in serial master *********************/ 

typedef struct quat_comunication_data_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t gpsfix;
	float q1;
	float q2;
	float q3;
	float q4;	
	uint8_t i1;
	uint8_t i2;
	uint8_t i3;
	uint8_t end_f;

}quat_comunication_data;



 /***************sender  POSITION_QUAT_DATA in serial master *********************/ 

typedef struct postion_quat_sender_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t gpsfix;
	float q1;
	float q2;
	float q3;
	float q4;
	float latitude;
	float longitude;
	float altitudine;	
	uint8_t i1;
	uint8_t i2;
	uint8_t i3;
	uint8_t end_f;

}postion_quat_sender;

 /***************sender  ONLY_INERTIAL_DATA in serial master *********************/ 

typedef struct only_inertial_sender_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t gpsfix;
	float accx;
	float accy;
	float accz;
	float gyrx;
	float gyry;
	float gyrz;		
	float magx;
	float magy;
	float magz;		
	uint8_t i1;
	uint8_t i2;
	uint8_t i3;
	uint8_t end_f;

}only_inertial_sender;


 /***************sender  POSITION_VEL_QUAT_DATA in serial master *********************/ 

typedef struct postion_vel_quat_sender_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t gpsfix;
	float q1;
	float q2;
	float q3;
	float q4;
	float Vx;
	float Vy;
	float Vz;
	float latitude;
	float longitude;
	float altitudine;	
	uint8_t i1;
	uint8_t i2;
	uint8_t i3;
	uint8_t end_f;

}postion_vel_quat_sender;


 /***************sender  POSITION_VEL_QUAT_INERTIAL_DATA in serial master *********************/ 

typedef struct postion_vel_quat_inertial_sender_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t gpsfix;
	float q1;
	float q2;
	float q3;
	float q4;
	float accx;
	float accy;
	float accz;
	float gyrx;
	float gyry;
	float gyrz;		
	float magx;
	float magy;
	float magz;
	float Vx;
	float Vy;
	float Vz;
	float latitude;
	float longitude;
	float altitudine;	
	uint8_t i1;
	uint8_t i2;
	uint8_t i3;
	uint8_t end_f;

}postion_vel_quat_inertial_sender;

  		/***************sender sender all configuration *********************/ 

typedef struct all_comunication_get_data_{

	uint8_t start;
	uint8_t start1;
	uint8_t start2;
	uint8_t ia;
	load_data_struct data;
	uint32_t id_dispositivo;
	uint32_t FW_version;
	uint32_t crc;
	uint8_t ib;
	uint8_t ic;
	uint8_t id;
	uint8_t end_f;

}all_comunication_get_data;


#endif
