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
 * File	Name	: serialib.h
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
#ifndef _SERIAL_COMUNICATION_h_
#define _SERIAL_COMUNICATION_h_

#define BAUDRATE921600  B921600
#define BAUDRATE460800  B460800
#define BAUDRATE230400  B230400
#define BAUDRATE115200 	B115200
#define BAUDRATE38400	B38400
#define BAUDRATE19200	B19200
#define BAUDRATE9600 	B9600
#define BAUDRATE4800    B4800

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
#include <string.h>
#include "comunication_descriptor_data.h"
//-----------------------------------------------------------------
/*!
\brief This routine opern the comunication usb port
\parm   char serial_port[20]
\parm long int badu	
\return int fd
*/
int open_serial_port_com(char serial_port[20], long int badu);
//-----------------------------------------------------------------
/*!
\brief This routine send the compilite buffer data
\parm   int fd
\parm 	all_comunication_data buffer
\return int success
*/
int serial_send(int fd,all_comunication_data buffer);
//-----------------------------------------------------------------
/*!
\brief This routine send the configuration data
\parm   int fd
\parm 	all_comunication_get_data buffer
\return int success
*/
int serial_send_get_data (int fd,all_comunication_get_data buffer);
//-----------------------------------------------------------------
/*!
\brief This routine send the quat buffer data
\parm   int fd
\parm 	quat_comunication_data buffer
\return int success
*/
int serial_send_quat (int fd,quat_comunication_data buffer);
//-----------------------------------------------------------------
/*!
\brief This routine send the only_inertial_sender buffer data
\parm   int fd
\parm 	postion_vel_quat_sender buffer
\return int success
*/
int serial_postion_vel_quat (int fd,postion_vel_quat_sender buffer);
//-----------------------------------------------------------------
/*!
\brief This routine send the only_inertial_sender buffer data
\parm   int fd
\parm 	postion_quat_sender buffer
\return int success
*/
int serial_postion_quat (int fd,postion_quat_sender buffer);
//-----------------------------------------------------------------
/*!
\brief This routine send the only_inertial_sender buffer data
\parm   int fd
\parm 	only_inertial_sender buffer
\return int success
*/
int serial_only_inertial (int fd,only_inertial_sender buffer);
//-----------------------------------------------------------------
/*!
\brief This routine send the serial_position_vel_quat_inertial buffer data
\parm   int fd
\parm 	postion_vel_quat_inertial_sender buffer
\return int success
*/
int serial_position_vel_quat_inertial (int fd,postion_vel_quat_inertial_sender buffer);


#endif
