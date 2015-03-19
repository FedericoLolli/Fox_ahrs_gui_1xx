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
 * File	Name	: fir.c
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
#include <stdlib.h>
#include "comunication_descriptor_data.h"

#include <stdio.h>
#include <stdint.h>
 
//////////////////////////////////////////////////////////////
//  Filter Code Definitions
//////////////////////////////////////////////////////////////
 
// maximum number of inputs that can be handled
// in one function call
#define MAX_INPUT_LEN   	1
// maximum length of filter than can be handled
#define MAX_FLT_LEN     	61
#define MAX_FLT_LEN_MAG     61
#define MAX_FLT_LEN_DER		21
#define MAX_FLT_LEN_GYRO 	21
// buffer to hold all of the input samples
#define BUFFER_LEN      	(MAX_FLT_LEN - 1 + MAX_INPUT_LEN)
#define BUFFER_LEN_MAG      (MAX_FLT_LEN_MAG - 1 + MAX_INPUT_LEN)
#define BUFFER_LEN_DER      	(MAX_FLT_LEN_DER - 1 + MAX_INPUT_LEN)
#define BUFFER_LEN_GYRO      (MAX_FLT_LEN_GYRO - 1 + MAX_INPUT_LEN)



 
// array to hold input samples
double insampaccx[ BUFFER_LEN ];
double insampaccy[ BUFFER_LEN ];
double insampaccz[ BUFFER_LEN ];

double insampmagx[ BUFFER_LEN_MAG ];
double insampmagy[ BUFFER_LEN_MAG ];
double insampmagz[ BUFFER_LEN_MAG ];

double insampdx[ BUFFER_LEN_DER ];
double insampdy[ BUFFER_LEN_DER ];
double insampdz[ BUFFER_LEN_DER ];


double insamph[ BUFFER_LEN_DER ];

double insampgyrox[ BUFFER_LEN_GYRO ];
double insampgyroy[ BUFFER_LEN_GYRO ];
double insampgyroz[ BUFFER_LEN_GYRO ];




 
  		/***************FIR init all global data *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes init ALL FIR filter 

\param double *input       : acc  ( G )
\param double *input       :  mag ( T )
\return noting
*/	

void firFloatInit( void )
{
    memset( insampaccx, 0, sizeof( insampaccx ) );
    memset( insampaccy, 0, sizeof( insampaccy ) );
    memset( insampaccz, 0, sizeof( insampaccz ) );
	memset( insampmagx, 0, sizeof( insampmagx ) );
    memset( insampmagy, 0, sizeof( insampmagy ) );
    memset( insampmagz, 0, sizeof( insampmagz ) );
	
	memset( insampgyrox, 0, sizeof( insampgyrox ) );
    memset( insampgyroy, 0, sizeof( insampgyroy ) );
    memset( insampgyroz, 0, sizeof( insampgyroz ) );
	
	memset( insampdx, 0, sizeof( insampdx ) );
    memset( insampdy, 0, sizeof( insampdy ) );
    memset( insampdz, 0, sizeof( insampdz ) );
	
	memset( insamph, 0, sizeof( insamph ) );


}
  /***************the FIR filter function *********************/ 
  //-----------------------------------------------------------------
/*
\brief This routine computes the FIR filter 

\param double *coeffs       : coeff
\param double *input       	:  input
\param double *output       :  output
\param int length       	:  len of the vector
\param int filterLength     :  len of the filter
\param double *insamp     	:  local variable
/return noting
*/	


void firFloat( double *coeffs, double *input, double *output,int length, int filterLength , double *insamp)
{
    double acc;     // accumulator for MACs
    double *coeffp; // pointer to coefficients
    double *inputp; // pointer to input samples
    int n;
    int k;
 
    // put the new samples at the high end of the buffer
    memcpy( &insamp[filterLength - 1], input,
            length * sizeof(double) );
 
    // apply the filter to each input sample
    for ( n = 0; n < length; n++ ) {
        // calculate output n
        coeffp = coeffs;
        inputp = &insamp[filterLength - 1 + n];
        acc = 0;
        for ( k = 0; k < filterLength; k++ ) {
            acc += (*coeffp++) * (*inputp--);
        }
        output[n] = acc;
    }
    // shift input samples back in time for next time
    memmove( &insamp[0], &insamp[length],
            (filterLength - 1) * sizeof(double) );
 
}
 
 
   /***************accel filter coeff *********************/ 

//////////////////////////////////////////////////////////////
//  Test program
//////////////////////////////////////////////////////////////
// lowpass filter cutoff around 2 Hz
// sampling rate = 819 Hz
/*old filter
 m = [1  1  0 0];
f = [0 .003 .04 1];
 n = 300;
 b = fir2(n,f,m);
 fvtool(b,1)
*/
// lowpass filter cutoff around 5 Hz
// sampling rate = 819 Hz
	/*
	m = [6  1  0 0];
	f = [0 .003 .1 1];
	n=60;
	*/
	#define FILTER_LEN  61
double coeffs[ FILTER_LEN ] =
	{
	
	0.000575483773946813, 0.000595428202166911, 0.000641683711898329, 0.000709871388553515, 0.000793404965757280, 0.000885101683785413, 0.000979623428960663, 0.00107651657684769, 0.00118347635224421, 0.00131935530449299, 
	0.00151638539328401, 0.00182110272211319, 0.00229355811090438, 0.00300456052379388, 0.00403091895864127, 0.00544889820932757, 0.00732635530175113, 0.00971424421687036, 0.0126383361323494, 0.0160920758611426, 
	0.0200314666632232, 0.0243727411081039, 0.0289933439776139, 0.0337364454986411, 0.0384188509486069, 0.0428418141634925, 0.0468039383486156, 0.0501150959799399, 0.0526101514017840, 0.0541612447445650, 0.0546875000000000, 
	0.0541612447445650, 0.0526101514017839, 0.0501150959799398, 0.0468039383486156, 0.0428418141634925, 0.0384188509486068, 0.0337364454986411, 0.0289933439776139, 0.0243727411081038, 0.0200314666632232, 0.0160920758611426, 
	0.0126383361323494, 0.00971424421687035, 0.00732635530175112, 0.00544889820932756, 0.00403091895864126, 0.00300456052379388, 0.00229355811090437, 0.00182110272211319, 0.00151638539328401, 0.00131935530449299,
	0.00118347635224421, 0.00107651657684769, 0.000979623428960663, 0.000885101683785413, 0.000793404965757281, 0.000709871388553515, 0.000641683711898329, 0.000595428202166911, 0.000575483773946813
};
	
   /***************mag filter coeff *********************/ 
	
// lowpass filter cutoff around 5 Hz
// sampling rate = 819 Hz	
	
/*
matlab code
	m = [6  1  0 0];
	f = [0 .003 .1 1];
	n=60;

 b = fir2(n,f,m);
 fvtool(b,1)
*/

	#define FILTER_MAG_LEN  61
double coeffs_fir_mag[ FILTER_MAG_LEN ] =
	{	
		0.000575483773946813, 0.000595428202166911, 0.000641683711898329, 0.000709871388553515, 0.000793404965757280, 0.000885101683785413, 0.000979623428960663, 0.00107651657684769, 0.00118347635224421, 0.00131935530449299, 
	0.00151638539328401, 0.00182110272211319, 0.00229355811090438, 0.00300456052379388, 0.00403091895864127, 0.00544889820932757, 0.00732635530175113, 0.00971424421687036, 0.0126383361323494, 0.0160920758611426, 
	0.0200314666632232, 0.0243727411081039, 0.0289933439776139, 0.0337364454986411, 0.0384188509486069, 0.0428418141634925, 0.0468039383486156, 0.0501150959799399, 0.0526101514017840, 0.0541612447445650, 0.0546875000000000, 
	0.0541612447445650, 0.0526101514017839, 0.0501150959799398, 0.0468039383486156, 0.0428418141634925, 0.0384188509486068, 0.0337364454986411, 0.0289933439776139, 0.0243727411081038, 0.0200314666632232, 0.0160920758611426, 
	0.0126383361323494, 0.00971424421687035, 0.00732635530175112, 0.00544889820932756, 0.00403091895864126, 0.00300456052379388, 0.00229355811090437, 0.00182110272211319, 0.00151638539328401, 0.00131935530449299,
	0.00118347635224421, 0.00107651657684769, 0.000979623428960663, 0.000885101683785413, 0.000793404965757281, 0.000709871388553515, 0.000641683711898329, 0.000595428202166911, 0.000575483773946813
};

	//coefficents gyro for coriolis correction
	/*
	 m = [3.5  3.5  0 0];	
	 n = 20;
	 f = [0 .005 .05 1];
	 b = fir2(n,f,m);
	  fvtool(b,1)
	*/
	#define FILTER_LEN_GYRO  21
double coeffs_Gyro[ FILTER_LEN_GYRO ] =
	{
	0.00590750251200613, 0.00784551976740138, 0.0132604058303793, 0.0219014317079051, 0.0331030081714385, 0.0458478117943395, 0.0588787953505800, 0.0708464960149962, 
	0.0804726957197683, 0.0867087443796095, 0.0888671875000000, 0.0867087443796095, 0.0804726957197683, 0.0708464960149962, 0.0588787953505800, 0.0458478117943395, 0.0331030081714385,
	0.0219014317079051, 0.0132604058303793, 0.00784551976740138, 0.00590750251200613
};	
/***************derivative filter coeff *********************/ 

/*	
	 m = [3.5  3.5  0 0];	
	 n = 20;
	 f = [0 .005 .05 1];
	 b = fir2(n,f,m);
	  fvtool(b,1)
*/
	#define FILTER_LEN_DER  21

double coeffsder[ FILTER_LEN_DER ] =
	{
	0.00590750251200613, 0.00784551976740138, 0.0132604058303793, 0.0219014317079051, 0.0331030081714385, 0.0458478117943395, 0.0588787953505800, 0.0708464960149962, 
	0.0804726957197683, 0.0867087443796095, 0.0888671875000000, 0.0867087443796095, 0.0804726957197683, 0.0708464960149962, 0.0588787953505800, 0.0458478117943395, 0.0331030081714385,
	0.0219014317079051, 0.0132604058303793, 0.00784551976740138, 0.00590750251200613
	};
	


	/***************mag and acc filter function *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the mag and acc vector

\param double *input       : acc  ( G )
\param double *input       :  mag ( T )
\return noting
*/	
void fir_filter_acc(float input[3], float magimput[3])
{
    int size=1;

    double floatInputx[1];
    double floatOutputx[1];
	double floatInputy[1];
    double floatOutputy[1];
	double floatInputz[1];
    double floatOutputz[1];
	
	double floatInputmx[1];
    double floatOutputmx[1];
	double floatInputmy[1];
    double floatOutputmy[1];
	double floatInputmz[1];
    double floatOutputmz[1];
	
	
	floatInputx[0]=input[0];
	floatInputy[0]=input[1];
	floatInputz[0]=input[2];
	
		
	floatInputmx[0]=magimput[0];
	floatInputmy[0]=magimput[1];
	floatInputmz[0]=magimput[2];

	firFloat( coeffs, floatInputx, floatOutputx, size,FILTER_LEN ,insampaccx);
	firFloat( coeffs, floatInputy, floatOutputy, size,FILTER_LEN ,insampaccy);
	firFloat( coeffs, floatInputz, floatOutputz, size,FILTER_LEN ,insampaccz);
	
	firFloat( coeffs_fir_mag, floatInputmx, floatOutputmx, size,FILTER_MAG_LEN ,insampmagx);
	firFloat( coeffs_fir_mag, floatInputmy, floatOutputmy, size,FILTER_MAG_LEN ,insampmagy);
	firFloat( coeffs_fir_mag, floatInputmz, floatOutputmz, size,FILTER_MAG_LEN ,insampmagz);
	
	magimput[0]=floatOutputmx[0];
	magimput[1]=floatOutputmy[0];
	magimput[2]=floatOutputmz[0];
	
	input[0]=floatOutputx[0];
	input[1]=floatOutputy[0];
	input[2]=floatOutputz[0];
	return 0;
	
}

	/***************derivative filter function *********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the derivative vector

\param double *input       : dv/dt  ( Meters/S^2 )
\return nothing
*/	

void fir_filter_corilis_and_linear(double input[3])
{
    int size=1;

    double floatInputx[1];
    double floatOutputx[1];
	double floatInputy[1];
    double floatOutputy[1];
	double floatInputz[1];
    double floatOutputz[1];
	
	floatInputx[0]=input[0];
	floatInputy[0]=input[1];
	floatInputz[0]=input[2];
	

	firFloat( coeffsder, floatInputx, floatOutputx, size,FILTER_LEN_DER ,insampdx);
	firFloat( coeffsder, floatInputy, floatOutputy, size,FILTER_LEN_DER ,insampdy);
	firFloat( coeffsder, floatInputz, floatOutputz, size,FILTER_LEN_DER ,insampdz);

	input[0]=floatOutputx[0];
	input[1]=floatOutputy[0];
	input[2]=floatOutputz[0];
	return 0;
	
}

//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the gyro coriolis correction

\param double *input       : dw/dt  ( rad/S )
\return nothing
*/	

void fir_filter_gyro(double input[3])
{
    int size=1;

    double floatInputx[1];
    double floatOutputx[1];
	double floatInputy[1];
    double floatOutputy[1];
	double floatInputz[1];
    double floatOutputz[1];
	
	floatInputx[0]=input[0];
	floatInputy[0]=input[1];
	floatInputz[0]=input[2];
	

	firFloat( coeffs_Gyro, floatInputx, floatOutputx, size,FILTER_LEN_GYRO ,insampgyrox);
	firFloat( coeffs_Gyro, floatInputy, floatOutputy, size,FILTER_LEN_GYRO ,insampgyroy);
	firFloat( coeffs_Gyro, floatInputz, floatOutputz, size,FILTER_LEN_GYRO ,insampgyroz);

	input[0]=floatOutputx[0];
	input[1]=floatOutputy[0];
	input[2]=floatOutputz[0];
	return 0;
	
}


	/***************filter altitude from baro*********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the altitude

\param double *input       : altitude  ( Meters )
\return nothing
*/

void fir_filter_height(double input[1])
{
    int size=1;

    double floatInputx[1];
    double floatOutputx[1];
	
	floatInputx[0]=input[0];

	firFloat( coeffsder, floatInputx, floatOutputx, size,FILTER_LEN_DER ,insamph);
	input[0]=floatOutputx[0];
	return 0;
	
}

