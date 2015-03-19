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
 * File	Name	: fir.h
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


#ifndef FIR_H_
#define FIR_H_
#include "comunication_descriptor_data.h"
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the mag and acc vector

\param double *input       :  acc  ( G )
\param double *input       :  mag ( T )
\return noting
*/	
void fir_filter_acc(float input[3], float magimput[3]);
//-----------------------------------------------------------------
/*!
\brief This routine computes init ALL FIR filter 

\param double *input       : acc  ( G )
\param double *input       : mag ( T )
\return noting
*/	
void firFloatInit( void );
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the derivative vector

\param double *input       : dv/dt  ( Meters/S^2 )
\return nothing
*/
void fir_filter_corilis_and_linear(double input[3]);
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the altitude

\param double *input       : altitude  ( Meters )
\return nothing
*/
void fir_filter_height(double input[1]);
//-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter for the gyro coriolis correction

\param double *input       : dw/dt  ( rad/S )
\return nothing
*/	

void fir_filter_gyro(double input[3]);
  //-----------------------------------------------------------------
/*!
\brief This routine computes the FIR filter 

\param double *coeffs       : coeff
\param double *input       	:  input
\param double *output       :  output
\param int length       	:  len of the vector
\param int filterLength     :  len of the filter
\param double *insamp     	:  local variable
/return noting
*/	

void firFloat( double *coeffs, double *input, double *output,int length, int filterLength , double *insamp);


#endif /* CRC_H_ */
