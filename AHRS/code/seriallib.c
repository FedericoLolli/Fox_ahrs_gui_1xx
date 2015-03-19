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
 * File	Name	: serialib.c
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
#include"seriallib.h"
//-----------------------------------------------------------------
/*!
\brief This routine opern the comunication usb port
\parm   char serial_port[20]
\parm long int badu	
\return int fd
*/
int open_serial_port_com(char serial_port[20], long int badu){
//serial port opening
	int fd;
	struct termios oldtio, newtio;
  	fd=open(serial_port, O_RDWR | O_NOCTTY | O_NDELAY | O_NONBLOCK);

  	if (fd<0) {
		perror ("FD opened\n");
	     perror (serial_port);
	   //exit(-1);
  	}
	if (fd>0)
	{
		tcgetattr(fd,&oldtio);
		bzero(&newtio,sizeof(newtio));
		if(badu==921600){newtio.c_cflag=(BAUDRATE921600 | CS8 | CLOCAL | CREAD);}
		if(badu==460800){newtio.c_cflag=(BAUDRATE460800 | CS8 | CLOCAL | CREAD);}
		if(badu==230400){newtio.c_cflag=(BAUDRATE230400 | CS8 | CLOCAL | CREAD);}
		if(badu==115200){newtio.c_cflag=(BAUDRATE115200 | CS8 | CLOCAL | CREAD);}
		if(badu==38400){newtio.c_cflag=(BAUDRATE38400 | CS8 | CLOCAL | CREAD);}
		if(badu==19200){newtio.c_cflag=(BAUDRATE19200 | CS8 | CLOCAL | CREAD);}
		if(badu==9600){newtio.c_cflag=(BAUDRATE9600 | CS8 | CLOCAL | CREAD);}
		if(badu==4800){newtio.c_cflag=(BAUDRATE4800 | CS8 | CLOCAL | CREAD);}
		if((badu!=4800)&&(badu!=9600)&&(badu!=19200)&&(badu!=38400)&&(badu!=115200)&&(badu!=460800))
		{
			perror("worning error on badurate serial device set slowat 4800");
			newtio.c_cflag=(BAUDRATE4800 | CS8 | CLOCAL | CREAD);
		}
		newtio.c_iflag= IGNPAR | ICRNL;
		newtio.c_oflag=0;
		newtio.c_lflag=ICANON;
		tcflush(fd,TCIFLUSH);
		tcsetattr(fd,TCSANOW,&newtio);
	}

	return fd;
	//end setting serial port
}

//-----------------------------------------------------------------
/*!
\brief This routine send the compilite buffer data
\parm   int fd
\parm 	all_comunication_data buffer
\return int success
*/
int serial_send (int fd,all_comunication_data buffer)
{
	int writed;
	writed=write(fd,(void*)&buffer, sizeof(buffer));
	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
    if(writed==sizeof(buffer))
    {
	   return 1;
	}
    else
	{
	  return 0;
	}
}
//-----------------------------------------------------------------
/*!
\brief This routine send the quat buffer data
\parm   int fd
\parm 	quat_comunication_data buffer
\return int success
*/
int serial_send_quat (int fd,quat_comunication_data buffer)
{
	int writed;
	//tcflush(fd, TCOFLUSH);
	writed=write(fd,(void*)&buffer, sizeof(buffer));

	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
    if(writed==sizeof(buffer))
    {
		return 1;
	}
	else{
       return 0;
	}
}

//-----------------------------------------------------------------
/*!
\brief This routine send the serial_position_vel_quat_inertial buffer data
\parm   int fd
\parm 	postion_vel_quat_inertial_sender buffer
\return int success
*/
int serial_position_vel_quat_inertial (int fd,postion_vel_quat_inertial_sender buffer)
{
	int writed;
	//tcflush(fd, TCOFLUSH);
	writed=write(fd,(void*)&buffer, sizeof(buffer));

	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
    if(writed==sizeof(buffer))
    {
		return 1;
	}
	else{
       return 0;
	}
}

//-----------------------------------------------------------------
/*!
\brief This routine send the only_inertial_sender buffer data
\parm   int fd
\parm 	only_inertial_sender buffer
\return int success
*/
int serial_only_inertial (int fd,only_inertial_sender buffer)
{
	int writed;
	//tcflush(fd, TCOFLUSH);
	writed=write(fd,(void*)&buffer, sizeof(buffer));

	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
    if(writed==sizeof(buffer))
    {
		return 1;
	}
	else{
       return 0;
	}
}

//-----------------------------------------------------------------
/*!
\brief This routine send the only_inertial_sender buffer data
\parm   int fd
\parm 	postion_quat_sender buffer
\return int success
*/
int serial_postion_quat (int fd,postion_quat_sender buffer)
{
	int writed;
	//tcflush(fd, TCOFLUSH);
	writed=write(fd,(void*)&buffer, sizeof(buffer));

	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
    if(writed==sizeof(buffer))
    {
		return 1;
	}
	else{
       return 0;
	}
}

//-----------------------------------------------------------------
/*!
\brief This routine send the only_inertial_sender buffer data
\parm   int fd
\parm 	postion_vel_quat_sender buffer
\return int success
*/
int serial_postion_vel_quat (int fd,postion_vel_quat_sender buffer)
{
	int writed;
	//tcflush(fd, TCOFLUSH);
	writed=write(fd,(void*)&buffer, sizeof(buffer));

	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
    if(writed==sizeof(buffer))
    {
		return 1;
	}
	else{
       return 0;
	}
}
//-----------------------------------------------------------------
/*!
\brief This routine send the configuration data
\parm   int fd
\parm 	all_comunication_get_data buffer
\return int success
*/
int serial_send_get_data (int fd,all_comunication_get_data buffer)
{
	int writed;
	//tcflush(fd, TCOFLUSH);
	writed=write(fd,(void*)&buffer, sizeof(buffer));
	//tcflush(fd,TCOFLUSH);
	//printf("writed %d \n",writed);
	if(writed==sizeof(buffer))
    {
		return 1;
	}  else
	{
		return 0;
	}
}
