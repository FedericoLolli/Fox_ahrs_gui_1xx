/*
 ******************************************************************************
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
 * Created      : october 15rd, 2013
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
#include "nmeap.h"

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

#ifndef _GPS_h_
#define _GPS_h_
 
#define GPS_BUF_LEN		20

#define NMEA_STRING_LENGTH		200
#define NMEA_BUFFER_LENGTH		200		//!< maximum character buffer length for NMEA serial streaming
#define MAX_NMEA_GPSWRONG_SENTENCES 	10		//!< maximum sequential unlocked NMEA strings

// GPS Errors
//----------------------
#define ERROR_GPS_WRONG_CHECKSUM		100	//!< checksum NMEA errata.
#define ERROR_GPS_TIMEOUT_WAIT_GPS		101	//!< Timeout per: 	attesa segnale/servizio GPS
#define ERROR_GPS_TIMEOUT_NMEA_SYNCH	102	//!< Timeout per: 	mancato sincronismo con streaming NMEA
#define ERROR_GPS_SERIAL_PORT			104	//!< Impossibile aprire la porta seriale GPS
#define ERROR_GPS_GENERIC_ERR			105	//!< Errore generico. 
#define ERROR_GPS_IMPOSSIBLE_GET_POS	106	//!< Impossibile ottenere la posizione leggendo dalla seriale
#define ERROR_GPS_WAITING_FOR_GPS		110
#define ERROR_GPS_UNABLE_TO_READ_SERIAL	120
#define ERROR_GPS_PROBLEMS_DURING_PARSE	130

#define GPS_NO_ERRORS			0
#define	GPS_GET_POSITION_OK		GPS_NO_ERRORS			
#define GPS_UNABLE_TO_READ_SERIAL	ERROR_GPS_UNABLE_TO_READ_SERIAL
#define GPS_UNABLE_TO_GET_POSITION	ERROR_GPS_IMPOSSIBLE_GET_POS
#define GPS_PARSER_ERROR		ERROR_GPS_PROBLEMS_DURING_PARSE
#define GPS_WRONG_CHECKSUM		ERROR_GPS_WRONG_CHECKSUM
#define GPS_WAITING_FOR_LOCK		ERROR_GPS_WAITING_FOR_GPS



#define FALSEG 0
#define TRUEG 1

#define GPS_DEBUG			0

/*! \struct NavInputs_t
This structure contains the variable that characterize the GPS info about the vessel(user) position
 */
typedef struct {
	double 	lat;		/*!< latitude in decimanl degrees [-90° .. +90°]*/
	double 	lon;		/*!< longitude  in decimal degrees [-180° .. +180°] */
	double 	heigth;		/*!< heigth of the vessel respect the sea level [mt] */
	double	speed;
	double	trueangle;
	unsigned int	gpsFixQuality;	/*!< indicator of the presence of the GPS service (0=no service, >0 GPS available) */
}NavInputs_t;




NavInputs_t read_nav_data(void);

int readchar(char*);
void print_gga(nmeap_gga_t*);
void gpgga_callout(nmeap_context_t*, void*, void*);
void print_rmc(nmeap_rmc_t*);
void gprmc_callout(nmeap_context_t*, void*, void*);
int parseNMEAString(char*);


int getUserPosition(char nmeaBuffer_tmp[NMEA_BUFFER_LENGTH],int read_char);  	/*!< manage GPS protocol and get GPS position */
void clearNMEABuffer(void);
void clearNMEAStringBuffer(void);
int searchCompleteNMEAString (void);
void configureS1315sk(int fd_in);
void ReadGPS_data_polling(int fd_in);


#endif

