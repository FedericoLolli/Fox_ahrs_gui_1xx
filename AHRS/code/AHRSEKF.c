/*
 ******************************************************************************
 *
 * 						CUSTOM LIBRARY MODULES
 *
 * Copyright (c) 2013 skytech
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
 * Created      : giugno 3rd, 2014
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

//---------------------------------------------------------------------------------------------------
// Header files

#include "AHRSEKF.h"
#include <math.h>
#include <time.h>
#include <errno.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <unistd.h>
#include <getopt.h>
#include <fcntl.h>
#include <sys/ioctl.h>
#include <linux/types.h>

//#define DEBUG 1

#define  DEFV1_0 0.0f
#define  DEFV1_1 0.0f
#define DEFV1_2 1.0f

//#define DEFV2_0 0.0f
//#define DEFV2_1 1.0f
//#define DEFV2_2 0.0f
#define COUNT_CAMBIO_SOGLIA 100
#define START_GAIN 0.002f
#define START_GAIN_COUNTER 22000

#define MAX_ACCEL_CORRECTION 0.2f

#define END_BIASING_TIMEOUT_WINDOW 130000
#define TIMING_BIASING_CORRECTION 10

#define SATURATE_BIAS_GYRO 0.035f

#define GAIN_CORIOLIS_CORRECTION 0.08f
#define GAIN_linear_CORRECTION 0.25f


volatile double DEFV2_0 =-0.7;
volatile double DEFV2_1 =-0.4;
volatile double DEFV2_2 =-0.5;

volatile double sig_acc = 0.001;										
volatile double sig_m = 0.009;	
volatile double sig_quad_acc=0,sig_quad_m=0,sig_quad_tot=0,a1=0,a2=0;
static double Mk_p1_G[49];
static double x_k_p1_G[7];
static double Xk_G[7];
volatile double gain_G=0.001;
volatile double stored_gain=0.001;

static double bias[3];

static FILE *outfile_ekf_data;
static int debug_option =0;
volatile long int rowcounter=0;
volatile int avvio =0;
volatile int cnt_soglia=0;
volatile int soft_start_gain=0;


volatile	double 	altitude_stored=0,altitude_stored1=0,velocity_stored_X=0,velocity_stored1_X=0,altitude_stored2=0,velocity_stored_Y=0,velocity_stored1_Y=0;



static double tmp_bias_gyro[3];
static double gyrobiasing_stored[3];
volatile long int cnt_gyro_biasing=0;



//-----------------------------------------------------------------
/*!
\brief This routine save the file stream of computational data

\param FILE *data     : output filestream
\return nothing
*/

void set_file_data(FILE *data)
{
 outfile_ekf_data=data;
 debug_option=1;
}

//-----------------------------------------------------------------
/*!
\brief This routine computes essential datum values from basic parameters
obtained from the \e mag structure.

\param double x       : mag declination x axis   ( T )
\param double y       : mag declination Y axis   ( T )
\param double z       : mag declination Z axis   ( T )

\return nothing
*/
void set_mag_declination(double x,double y,double z )
{
	DEFV2_0=x;
	DEFV2_1=y;
	DEFV2_2=z;
	//printf("setted mag declination %f %f %f \n ",DEFV2_0,DEFV2_1,DEFV2_2);
}

//-----------------------------------------------------------------
/*!
\brief This routine inizialize the EKF paramiters

\param double dev_stad_accell   : acc dev standard   ( sigma^2 )
\param double dev_std_mag       : mag dev standard   ( sigma^2 )
\param double gain       		: EKF gain

\return nothing
*/

/***************inizialize quest and ekf data*********************/
void inizialize_quest(double dev_stad_accell, double dev_std_mag, double gain )
{
	#ifdef DEBUG 
	if(debug_option==1){
	fprintf(outfile_ekf_data,"start inizializer\n");
	}
	#endif
	int i=0;
	sig_acc=dev_stad_accell;
	sig_m=dev_std_mag;
	gain_G=gain;
	stored_gain=gain;
	sig_quad_acc=sig_acc*sig_acc;
	sig_quad_m=sig_m*sig_m;
	sig_quad_tot=1/(1/sig_quad_acc+1/sig_quad_m);
	a1=sig_quad_tot/sig_quad_acc;
	a2=sig_quad_tot/sig_quad_m;
	for( i=0; i<49;i++)
	{
		Mk_p1_G[i]=0.0;
	}
	for( i=0; i<7;i++)
	{
		x_k_p1_G[i]=0.0;
		Xk_G[i]=0.0;
	}
	Mk_p1_G[0]=0.0015;//def 0.003
	Mk_p1_G[8]=0.0015;//def 0.003
	Mk_p1_G[16]=0.0015;//def 0.003
	Mk_p1_G[24]=0.0015;//def 0.003
	x_k_p1_G[0]=1.0;
	bias[0]=0.0;
	bias[1]=0.0;
	bias[2]=0.0;
	avvio=10;
	cnt_soglia=0;
	soft_start_gain=START_GAIN_COUNTER;


	altitude_stored=0.0;
	altitude_stored1=0.0;
	velocity_stored_X=0.0;
	velocity_stored1_X=0.0;
	velocity_stored_Y=0.0;
	velocity_stored1_Y=0.0;
	altitude_stored2=0.0;
	
	tmp_bias_gyro[0]=0.0;
	tmp_bias_gyro[1]=0.0;
	tmp_bias_gyro[2]=0.0;
	gyrobiasing_stored[0]=0.0;
	gyrobiasing_stored[1]=0.0;
	gyrobiasing_stored[2]=0.0;
	cnt_gyro_biasing=0;
	//fprintf(outfile_ekf_data,"end inizializer\n");
}

/***************cross_product 3x3*********************/
//-----------------------------------------------------------------
/*!
\brief This routine compute the cross product of 3x3

\param  double a[3]   
\param  double b[3]
\return double c[3]
*/
void cross_product3(double a[3],double  b[3],double c[3])
{
	c[0]=a[1]*b[2]-a[2]*b[1];
	c[1]=a[2]*b[0]-a[0]*b[2];
	c[2]=a[0]*b[1]-a[1]*b[0];

}
/***************dot_product 3x3*********************/
double  dot_product3(double a[3],double  b[3])
{
	double c=0;
	c=a[0]*b[0]+a[1]*b[1]+a[2]*b[2];
	return c;
}
/***************normalize 3*********************/
void normalize3(double a[3],double b[3])
{
	double recipNorm;
	recipNorm =(double) (1/sqrtf(a[0] * a[0] + a[1] * a[1] + a[2] * a[2]));
	b[0] = a[0]*recipNorm;
	b[1] = a[1]*recipNorm;
	b[2] = a[2]*recipNorm;  
	
}
/***************norm 3*********************/
double norm3(double a[3])
{
return (double)sqrt(a[0]*a[0]+a[1]*a[1]+a[2]*a[2]);
}
/***************vector multiply 3x1*1x3=3x3 *********************/
void vector3x3multiplication(double a[3],double  b[3],double c[9])
{
	c[0]=a[0]*b[0];
	c[1]=a[0]*b[1];
	c[2]=a[0]*b[2];

	c[3]=a[1]*b[0];
	c[4]=a[1]*b[1];
	c[5]=a[1]*b[2];

	c[6]=a[2]*b[0];
	c[7]=a[2]*b[1];
	c[8]=a[2]*b[2];
}
/***************matrix sum 3x3+3x3=3x3 *********************/

void matrix3x3sum(double a[9],double  b[9],double c[9])
{
	c[0]=a[0]+b[0];
	c[1]=a[1]+b[1];
	c[2]=a[2]+b[2];

	c[3]=a[3]+b[3];
	c[4]=a[4]+b[4];
	c[5]=a[5]+b[5];

	c[6]=a[6]+b[6];
	c[7]=a[7]+b[7];
	c[8]=a[8]+b[8];
}
/***************matrix multiply 3x3*c=3x3 *********************/

void matrix3x3costmultiplay(double a[9],double  b,double c[9])
{
	c[0]=a[0]*b;
	c[1]=a[1]*b;
	c[2]=a[2]*b;

	c[3]=a[3]*b;
	c[4]=a[4]*b;
	c[5]=a[5]*b;

	c[6]=a[6]*b;
	c[7]=a[7]*b;
	c[8]=a[8]*b;
}
/***************matrix division element by element 3x3 ./ 3x3 =3x3 *********************/

void matrix3x3_1div(double a[9],double c[9])
{
	c[0]=1/a[0];
	c[1]=1/a[1];
	c[2]=1/a[2];

	c[3]=1/a[3];
	c[4]=1/a[4];
	c[5]=1/a[5];

	c[6]=1/a[6];
	c[7]=1/a[7];
	c[8]=1/a[8];
}
/***************matrix 3x3 determinant *********************/

double det3x3(double a[9])
{
return a[0]*a[4]*a[8]+a[1]*a[5]*a[6]+a[2]*a[3]*a[7]-a[2]*a[4]*a[6]-a[1]*a[3]*a[8]-a[0]*a[5]*a[7];
}
/***************matrix 3x3 inversion *********************/

void inv3x3(double det,double a[9],double b[9] )
{ 
	if(det==0.0)
	{
		det=0.00001;
	}
	double tmp[9];
	tmp[0]=a[0];
	tmp[1]=a[3];
	tmp[2]=a[6];
	tmp[3]=a[1];
	tmp[4]=a[4];
	tmp[5]=a[7];
	tmp[6]=a[2];
	tmp[7]=a[5];
	tmp[8]=a[8];

	b[0]=(tmp[4]*tmp[8]-tmp[7]*tmp[5])/det;
	b[1]=-(tmp[3]*tmp[8]-tmp[6]*tmp[5])/det;
	b[2]=(tmp[3]*tmp[7]-tmp[6]*tmp[4])/det;
	b[3]=-(tmp[1]*tmp[8]-tmp[7]*tmp[2])/det;
	b[4]=(tmp[0]*tmp[8]-tmp[6]*tmp[2])/det;
	b[5]=-(tmp[0]*tmp[7]-tmp[6]*tmp[1])/det;
	b[6]=(tmp[1]*tmp[5]-tmp[4]*tmp[2])/det;
	b[7]=-(tmp[0]*tmp[5]-tmp[2]*tmp[3])/det;
	b[8]=(tmp[0]*tmp[4]-tmp[1]*tmp[3])/det;




   
}
/***************matrix 3x3 inversion 2 system*********************/

int inv3x3_2(double a[9],double b[9] )
{
 double det = a[0]*a[4]*a[8] + a[1]*a[5]*a[6] + a[2]*a[3]*a[7] - a[0]*a[5]*a[7] - a[1]*a[3]*a[8] - a[2]*a[4]*a[6];
    if(det == 0)
        return 0;
	det=1/det;
     b[0] = (a[4]*a[8] - a[5]*a[7])*det;
     b[1] = (a[2]*a[7] - a[1]*a[8])*det;
     b[2] = (a[1]*a[5] - a[2]*a[4])*det;
     b[3] = (a[5]*a[6] - a[3]*a[8])*det;
     b[4] = (a[0]*a[8] - a[2]*a[6])*det;
     b[5] = (a[2]*a[3] - a[0]*a[5])*det;
     b[6] = (a[3]*a[7] - a[4]*a[6])*det;
     b[7] = (a[1]*a[6] - a[0]*a[7])*det;
     b[8] = (a[0]*a[4] - a[1]*a[3])*det;
     return 1;
}
/***************matrix 4x4 determinant *********************/

double det4x4(double a[16])
{

	return (double)(a[0]*a[5]*a[10]*a[15] +a[1]*a[6]*a[11]*a[12] + a[2]*a[7]*a[8]*a[13] +a[3]*a[4]*a[9]*a[14] -a[3]*a[6]*a[9]*a[12] -a[13]*a[10]*a[7]*a[0] -a[14]*a[11]*a[4]*a[1] -a[15]*a[8]*a[5]*a[2]);

}


int invertColumnMajor4(double m[16], double invOut[16])
{
    double inv[16], det;
    int i;
 
    inv[ 0] =  m[5] * m[10] * m[15] - m[5] * m[11] * m[14] - m[9] * m[6] * m[15] + m[9] * m[7] * m[14] + m[13] * m[6] * m[11] - m[13] * m[7] * m[10];
    inv[ 4] = -m[4] * m[10] * m[15] + m[4] * m[11] * m[14] + m[8] * m[6] * m[15] - m[8] * m[7] * m[14] - m[12] * m[6] * m[11] + m[12] * m[7] * m[10];
    inv[ 8] =  m[4] * m[ 9] * m[15] - m[4] * m[11] * m[13] - m[8] * m[5] * m[15] + m[8] * m[7] * m[13] + m[12] * m[5] * m[11] - m[12] * m[7] * m[ 9];
    inv[12] = -m[4] * m[ 9] * m[14] + m[4] * m[10] * m[13] + m[8] * m[5] * m[14] - m[8] * m[6] * m[13] - m[12] * m[5] * m[10] + m[12] * m[6] * m[ 9];
    inv[ 1] = -m[1] * m[10] * m[15] + m[1] * m[11] * m[14] + m[9] * m[2] * m[15] - m[9] * m[3] * m[14] - m[13] * m[2] * m[11] + m[13] * m[3] * m[10];
    inv[ 5] =  m[0] * m[10] * m[15] - m[0] * m[11] * m[14] - m[8] * m[2] * m[15] + m[8] * m[3] * m[14] + m[12] * m[2] * m[11] - m[12] * m[3] * m[10];
    inv[ 9] = -m[0] * m[ 9] * m[15] + m[0] * m[11] * m[13] + m[8] * m[1] * m[15] - m[8] * m[3] * m[13] - m[12] * m[1] * m[11] + m[12] * m[3] * m[ 9];
    inv[13] =  m[0] * m[ 9] * m[14] - m[0] * m[10] * m[13] - m[8] * m[1] * m[14] + m[8] * m[2] * m[13] + m[12] * m[1] * m[10] - m[12] * m[2] * m[ 9];
    inv[ 2] =  m[1] * m[ 6] * m[15] - m[1] * m[ 7] * m[14] - m[5] * m[2] * m[15] + m[5] * m[3] * m[14] + m[13] * m[2] * m[ 7] - m[13] * m[3] * m[ 6];
    inv[ 6] = -m[0] * m[ 6] * m[15] + m[0] * m[ 7] * m[14] + m[4] * m[2] * m[15] - m[4] * m[3] * m[14] - m[12] * m[2] * m[ 7] + m[12] * m[3] * m[ 6];
    inv[10] =  m[0] * m[ 5] * m[15] - m[0] * m[ 7] * m[13] - m[4] * m[1] * m[15] + m[4] * m[3] * m[13] + m[12] * m[1] * m[ 7] - m[12] * m[3] * m[ 5];
    inv[14] = -m[0] * m[ 5] * m[14] + m[0] * m[ 6] * m[13] + m[4] * m[1] * m[14] - m[4] * m[2] * m[13] - m[12] * m[1] * m[ 6] + m[12] * m[2] * m[ 5];
    inv[ 3] = -m[1] * m[ 6] * m[11] + m[1] * m[ 7] * m[10] + m[5] * m[2] * m[11] - m[5] * m[3] * m[10] - m[ 9] * m[2] * m[ 7] + m[ 9] * m[3] * m[ 6];
    inv[ 7] =  m[0] * m[ 6] * m[11] - m[0] * m[ 7] * m[10] - m[4] * m[2] * m[11] + m[4] * m[3] * m[10] + m[ 8] * m[2] * m[ 7] - m[ 8] * m[3] * m[ 6];
    inv[11] = -m[0] * m[ 5] * m[11] + m[0] * m[ 7] * m[ 9] + m[4] * m[1] * m[11] - m[4] * m[3] * m[ 9] - m[ 8] * m[1] * m[ 7] + m[ 8] * m[3] * m[ 5];
    inv[15] =  m[0] * m[ 5] * m[10] - m[0] * m[ 6] * m[ 9] - m[4] * m[1] * m[10] + m[4] * m[2] * m[ 9] + m[ 8] * m[1] * m[ 6] - m[ 8] * m[2] * m[ 5];
 
    det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];
 
    if(det == 0)
        return 0;
 
    det = 1.f / det;
 
    for(i = 0; i < 16; i++)
        invOut[i] = inv[i] * det;
 
    return 1;
}

/***************matrix 4x4 multiplication *********************/

void matrix4x4multiplication(double a[16],double  b[16], double tmp[16])
{
	
	tmp[0]=a[0]*b[0]+a[1]*b[4]+a[2]*b[8] +a[3]*b[12];
	tmp[1]=a[0]*b[1]+a[1]*b[5]+a[2]*b[9] +a[3]*b[13];
	tmp[2]=a[0]*b[2]+a[1]*b[6]+a[2]*b[10] +a[3]*b[14];
	tmp[3]=a[0]*b[3]+a[1]*b[7]+a[2]*b[11] +a[3]*b[15];
	
	tmp[4]=a[4]*b[0]+a[5]*b[4]+a[6]*b[8] +a[7]*b[12];
	tmp[5]=a[4]*b[1]+a[5]*b[5]+a[6]*b[9] +a[7]*b[13];
	tmp[6]=a[4]*b[2]+a[5]*b[6]+a[6]*b[10] +a[7]*b[14];
	tmp[7]=a[4]*b[3]+a[5]*b[7]+a[6]*b[11] +a[7]*b[15];
	
	tmp[8]=a[8]*b[0]+a[9]*b[4]+a[10]*b[8] +a[11]*b[12];
	tmp[9]=a[8]*b[1]+a[9]*b[5]+a[10]*b[9] +a[11]*b[13];
	tmp[10]=a[8]*b[2]+a[9]*b[6]+a[10]*b[10] +a[11]*b[14];
	tmp[11]=a[8]*b[3]+a[9]*b[7]+a[10]*b[11] +a[11]*b[15];
	
	tmp[12]=a[12]*b[0]+a[13]*b[4]+a[14]*b[8] +a[15]*b[12];
	tmp[13]=a[12]*b[1]+a[13]*b[5]+a[14]*b[9] +a[15]*b[13];
	tmp[14]=a[12]*b[2]+a[13]*b[6]+a[14]*b[10] +a[15]*b[14];
	tmp[15]=a[12]*b[3]+a[13]*b[7]+a[14]*b[11] +a[15]*b[15];
	
	
}
/***************matrix 3x3 multiplication *********************/

void matrix3x3multiplication(double a[9],double  b[9], double tmp[9])
{
	
	tmp[0]=a[0]*b[0]+a[1]*b[3]+a[2]*b[6] ;
	tmp[1]=a[0]*b[1]+a[1]*b[4]+a[2]*b[7] ;
	tmp[2]=a[0]*b[2]+a[1]*b[5]+a[2]*b[8] ;

	tmp[3]=a[3]*b[0]+a[4]*b[3]+a[5]*b[6] ;
	tmp[4]=a[3]*b[1]+a[4]*b[4]+a[5]*b[7] ;
	tmp[5]=a[3]*b[2]+a[4]*b[5]+a[5]*b[8] ;

	tmp[6]=a[6]*b[0]+a[7]*b[3]+a[8]*b[6] ;
	tmp[7]=a[6]*b[1]+a[7]*b[4]+a[8]*b[7] ;
	tmp[8]=a[6]*b[2]+a[7]*b[5]+a[8]*b[8] ;
	
}
/***************matrix 7x7_7x4 multiplication 7x4 *********************/

void matrix7x7_7x4multiplication(double a[49],double  b[28], double tmp[28])//restituisce una 7x4
{
int r=0;
	tmp[0]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[1]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[2]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[3]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
r=r+7;
	tmp[4]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[5]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[6]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[7]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
r=r+7;

	tmp[8]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[9]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[10]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[11]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
r=r+7;

	tmp[12]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[13]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[14]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[15]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
r=r+7;

	tmp[16]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[17]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[18]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[19]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
r=r+7;
	
	tmp[20]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[21]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[22]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[23]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
r=r+7;
	
	tmp[24]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[25]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[26]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[27]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];

	
}
/***************matrix 4x7_7x7 multiplication 4x7 *********************/

void matrix4x7_7x7multiplication(double a[28],double  b[49], double tmp[28])//restituisce una 4x7
{
int r=0;
	tmp[0]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21]+a[r+4]*b[28]+a[r+5]*b[35]+a[r+6]*b[42];
	tmp[1]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22]+a[r+4]*b[29]+a[r+5]*b[36]+a[r+6]*b[43];
	tmp[2]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23]+a[r+4]*b[30]+a[r+5]*b[37]+a[r+6]*b[44];
	tmp[3]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24]+a[r+4]*b[31]+a[r+5]*b[38]+a[r+6]*b[45];
	tmp[4]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25]+a[r+4]*b[32]+a[r+5]*b[39]+a[r+6]*b[46];
	tmp[5]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26]+a[r+4]*b[33]+a[r+5]*b[40]+a[r+6]*b[47];
	tmp[6]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27]+a[r+4]*b[34]+a[r+5]*b[41]+a[r+6]*b[48];
r=r+7;
	tmp[7]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21]+a[r+4]*b[28]+a[r+5]*b[35]+a[r+6]*b[42];
	tmp[8]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22]+a[r+4]*b[29]+a[r+5]*b[36]+a[r+6]*b[43];
	tmp[9]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23]+a[r+4]*b[30]+a[r+5]*b[37]+a[r+6]*b[44];
	tmp[10]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24]+a[r+4]*b[31]+a[r+5]*b[38]+a[r+6]*b[45];
	tmp[11]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25]+a[r+4]*b[32]+a[r+5]*b[39]+a[r+6]*b[46];
	tmp[12]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26]+a[r+4]*b[33]+a[r+5]*b[40]+a[r+6]*b[47];
	tmp[13]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27]+a[r+4]*b[34]+a[r+5]*b[41]+a[r+6]*b[48];
r=r+7;

	tmp[14]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21]+a[r+4]*b[28]+a[r+5]*b[35]+a[r+6]*b[42];
	tmp[15]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22]+a[r+4]*b[29]+a[r+5]*b[36]+a[r+6]*b[43];
	tmp[16]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23]+a[r+4]*b[30]+a[r+5]*b[37]+a[r+6]*b[44];
	tmp[17]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24]+a[r+4]*b[31]+a[r+5]*b[38]+a[r+6]*b[45];
	tmp[18]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25]+a[r+4]*b[32]+a[r+5]*b[39]+a[r+6]*b[46];
	tmp[19]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26]+a[r+4]*b[33]+a[r+5]*b[40]+a[r+6]*b[47];
	tmp[20]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27]+a[r+4]*b[34]+a[r+5]*b[41]+a[r+6]*b[48];
r=r+7;
	tmp[21]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21]+a[r+4]*b[28]+a[r+5]*b[35]+a[r+6]*b[42];
	tmp[22]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22]+a[r+4]*b[29]+a[r+5]*b[36]+a[r+6]*b[43];
	tmp[23]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23]+a[r+4]*b[30]+a[r+5]*b[37]+a[r+6]*b[44];
	tmp[24]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24]+a[r+4]*b[31]+a[r+5]*b[38]+a[r+6]*b[45];
	tmp[25]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25]+a[r+4]*b[32]+a[r+5]*b[39]+a[r+6]*b[46];
	tmp[26]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26]+a[r+4]*b[33]+a[r+5]*b[40]+a[r+6]*b[47];
	tmp[27]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27]+a[r+4]*b[34]+a[r+5]*b[41]+a[r+6]*b[48];
	
}
/***************matrix 4x7_7x4 multiplication 4x4 *********************/

void matrix4x7_7x4multiplication(double a[28],double  b[28], double tmp[16])
{
int r=0;
	tmp[0]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[1]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[2]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[3]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];

r=r+7;

	tmp[4]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[5]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[6]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[7]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];

r=r+7;
	tmp[8]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[9]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[10]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[11]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];

r=r+7;
	tmp[12]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12]+a[r+4]*b[16]+a[r+5]*b[20]+a[r+6]*b[24];
	tmp[13]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13]+a[r+4]*b[17]+a[r+5]*b[21]+a[r+6]*b[25];
	tmp[14]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14]+a[r+4]*b[18]+a[r+5]*b[22]+a[r+6]*b[26];
	tmp[15]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15]+a[r+4]*b[19]+a[r+5]*b[23]+a[r+6]*b[27];
	
}
/***************matrix 7x4_4x7 multiplication 7x7 *********************/

void matrix7x4_4x7multiplication(double a[28],double  b[28], double tmp[49])//restituisce una 7x7
{
int r=0;
	tmp[0]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[1]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[2]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[3]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[4]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[5]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[6]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];
r=r+4;
	tmp[7]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[8]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[9]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[10]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[11]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[12]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[13]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];
r=r+4;
	tmp[14]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[15]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[16]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[17]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[18]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[19]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[20]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];
r=r+4;
	tmp[21]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[22]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[23]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[24]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[25]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[26]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[27]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];
r=r+4;
	tmp[28]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[29]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[30]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[31]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[32]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[33]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[34]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];
r=r+4;
	tmp[35]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[36]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[37]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[38]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[39]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[40]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[41]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];
r=r+4;
	tmp[42]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21];
	tmp[43]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22];
	tmp[44]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23];
	tmp[45]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24];
	tmp[46]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25];
	tmp[47]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26];
	tmp[48]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27];


	
}


/***************matrix 7x4_4x4 multiplication 7x4 *********************/


void matrix7x4_4x4multiplication(double a[28],double  b[16], double tmp[28])//restituisce una 7x4
{
int r=0;
	tmp[0]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[1]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[2]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[3]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];

r=r+4;
	tmp[4]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[5]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[6]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[7]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];
r=r+4;
	tmp[8]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[9]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[10]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[11]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];
r=r+4;
	tmp[12]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[13]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[14]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[15]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];

r=r+4;
	tmp[16]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[17]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[18]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[19]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];

r=r+4;
	tmp[20]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[21]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[22]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[23]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];

r=r+4;
	tmp[24]=a[r]*b[0]+a[r+1]*b[4]+a[r+2]*b[8] +a[r+3]*b[12];
	tmp[25]=a[r]*b[1]+a[r+1]*b[5]+a[r+2]*b[9] +a[r+3]*b[13];
	tmp[26]=a[r]*b[2]+a[r+1]*b[6]+a[r+2]*b[10] +a[r+3]*b[14];
	tmp[27]=a[r]*b[3]+a[r+1]*b[7]+a[r+2]*b[11] +a[r+3]*b[15];

	
}
/***************matrix 7x7_7x7 multiplication 7x7 *********************/

void matrix7x7_7x7multiplication(double a[49],double  b[49], double tmp[49])//restituisce una 7x7
{
int r=0;
int j=0;
int i=0;
	for (j=0; j<7; j++)
	{
		tmp[i+0]=a[r]*b[0]+a[r+1]*b[7]+a[r+2]*b[14] +a[r+3]*b[21]+a[r+4]*b[28]+a[r+5]*b[35]+a[r+6]*b[42];
		tmp[i+1]=a[r]*b[1]+a[r+1]*b[8]+a[r+2]*b[15] +a[r+3]*b[22]+a[r+4]*b[29]+a[r+5]*b[36]+a[r+6]*b[43];
		tmp[i+2]=a[r]*b[2]+a[r+1]*b[9]+a[r+2]*b[16] +a[r+3]*b[23]+a[r+4]*b[30]+a[r+5]*b[37]+a[r+6]*b[44];
		tmp[i+3]=a[r]*b[3]+a[r+1]*b[10]+a[r+2]*b[17] +a[r+3]*b[24]+a[r+4]*b[31]+a[r+5]*b[38]+a[r+6]*b[45];
		tmp[i+4]=a[r]*b[4]+a[r+1]*b[11]+a[r+2]*b[18] +a[r+3]*b[25]+a[r+4]*b[32]+a[r+5]*b[39]+a[r+6]*b[46];
		tmp[i+5]=a[r]*b[5]+a[r+1]*b[12]+a[r+2]*b[19] +a[r+3]*b[26]+a[r+4]*b[33]+a[r+5]*b[40]+a[r+6]*b[47];
		tmp[i+6]=a[r]*b[6]+a[r+1]*b[13]+a[r+2]*b[20] +a[r+3]*b[27]+a[r+4]*b[34]+a[r+5]*b[41]+a[r+6]*b[48];
		r=r+7;
		i=i+7;
	}

}
/***************matrix 7x7 - 7x7 + eye(7) *********************/

void eye_7_minus_mat(double a[49],double b[49])
{
	int i=0;
	for(i=0; i<49; i++)
	{
		b[i]=-a[i];
	}
	b[0]=b[0]+1;
	b[8]=b[8]+1;
	b[16]=b[16]+1;
	b[24]=b[24]+1;
	b[32]=b[32]+1;
	b[40]=b[40]+1;
	b[48]=b[48]+1;
}
/***************matrix 4x4 + 4x4 *********************/

void matrix4x4sum(double a[16],double  b[16],double c[16])
{
	c[0]=a[0]+b[0];
	c[1]=a[1]+b[1];
	c[2]=a[2]+b[2];

	c[3]=a[3]+b[3];
	c[4]=a[4]+b[4];
	c[5]=a[5]+b[5];

	c[6]=a[6]+b[6];
	c[7]=a[7]+b[7];
	c[8]=a[8]+b[8];
	c[9]=a[9]+b[9];
	c[10]=a[10]+b[10];
	c[11]=a[11]+b[11];
	c[12]=a[12]+b[12];
	c[13]=a[13]+b[13];
	c[14]=a[14]+b[14];
	c[15]=a[15]+b[15];
}
/***************matrix traspose 7x7 *********************/

void trasp7x7(double a[49],double b[49])
{
	b[0]=a[0];
	b[1]=a[7];
	b[2]=a[14];
	b[3]=a[21];
	b[4]=a[28];
	b[5]=a[35];
	b[6]=a[42];

	b[7]=a[1];
	b[8]=a[8];
	b[9]=a[15];
	b[10]=a[22];
	b[11]=a[29];
	b[12]=a[36];
	b[13]=a[43];

	b[14]=a[2];
	b[15]=a[9];
	b[16]=a[16];
	b[17]=a[23];
	b[18]=a[30];
	b[19]=a[37];
	b[20]=a[44];

	b[21]=a[3];
	b[22]=a[10];
	b[23]=a[17];
	b[24]=a[24];
	b[25]=a[31];
	b[26]=a[38];
	b[27]=a[45];

	b[28]=a[4];
	b[29]=a[11];
	b[30]=a[18];
	b[31]=a[25];
	b[32]=a[32];
	b[33]=a[39];
	b[34]=a[46];

	b[35]=a[5];
	b[36]=a[12];
	b[37]=a[19];
	b[38]=a[26];
	b[39]=a[33];
	b[40]=a[40];
	b[41]=a[47];

	b[42]=a[6];
	b[43]=a[13];
	b[44]=a[20];
	b[45]=a[27];
	b[46]=a[34];
	b[47]=a[41];
	b[48]=a[48];


}
/***************quest *********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes optimized quest for quaternion computation

\param double W1[3]					: input vector for optimizing
\param double W2[3]					: input vector for optimizing

\return double PQQout[16]  			: quaternion status covariance matrix
\return double q_opt[4]  			: quaternoon computation
*/

//w1 acc in  W2 mag in V1 acc static V2 mag static
void quest(double W1[3], double W2[3],double PQQout[16] ,double q_opt[4])
{


#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"quest\n");
		/*fprintf(outfile_ekf_data,"ACC %f %f %f \n",W1[0],W1[1],W1[2]);
		fprintf(outfile_ekf_data,"mag %f %f %f \n",W2[0],W2[1],W2[2]);*/
	}
#endif
	double V1[3];	//acc static reference
	double V2[3];	//mag static reference
	V1[0]=DEFV1_0;
	V1[1]=DEFV1_1;
	V1[2]=DEFV1_2;
	V2[0]=DEFV2_0;
	V2[1]=DEFV2_1;
	V2[2]=DEFV2_2;
	double lambda_max=0.0;
	double cteta=0.0;
	double sigma=0.0;
	double delta=0.0;
	double Z[3];
	double X[3];
	double PQQ[16];
	double q_matrix[16];
	double q_matrixT[16];
	double detS;
	double adjS[9];
	double K=0.0;
	double tmp3x1_1[3];
	double tmp3x1_2[3];
	double alpha=0,beta=0,gamma=0;
	double tmp3x3_1[9];
	double tmp3x3_2[9];
	double tmp3x3_3[9];
	double tmp3x3_4[9];
	double tmp3x3_5[9];
	double tmp3x3_6[9];
	double SS[9];
	double tmp1;
	double tmp4x4_1[16];

	
	/******************************************
	V1=V1/norm(V1);
	V2=V2/norm(V2);
	W1=W1/norm(W1);
	W2=W2/norm(W2);
	********************************************/
	// Normalise accelerometer measurement
	normalize3(W1,W1);
	normalize3(W2,W2);
	normalize3(V2,V2);
	normalize3(V1,V1);
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"W1 %f %f %f \n", W1[0],W1[1],W1[2]);
	}
#endif
*/
	//cteta=dot(V1,V2)*dot(W1,W2)+(norm(cross(V1,V2)))*norm(cross(W1,W2));
	cross_product3(V1,V2, tmp3x1_1);
	cross_product3(W1,W2,tmp3x1_2);
	cteta=dot_product3(V1,V2)*dot_product3(W1,W2)+norm3(tmp3x1_1)*norm3(tmp3x1_2);
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"cross prod W1 W2 %f %f %f dot %f \n",  tmp3x1_1[0], tmp3x1_1[1], tmp3x1_1[2],dot_product3(W1,W2));
		fprintf(outfile_ekf_data,"cteta %f \n",cteta);
	}
#endif
*/
	//lambda_max=sqrt(a1^2+a2^2+2*a1*a2*cteta);
	lambda_max=sqrt(a1*a1+a2*a2+2.0*a1*a2*cteta);
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"lambda_max: %f \n",lambda_max);
	}
#endif
*/
	//S=a1*(W1*V1'+V1*W1')+ a2*(W2*V2'+V2*W2');
	vector3x3multiplication(W1,V1,tmp3x3_1);
	vector3x3multiplication(V1,W1,tmp3x3_2);
	vector3x3multiplication(W2,V2,tmp3x3_3);
	vector3x3multiplication(V2,W2,tmp3x3_4);
	matrix3x3sum(tmp3x3_1,tmp3x3_2,tmp3x3_5);
	matrix3x3sum(tmp3x3_3,tmp3x3_4,tmp3x3_6);
	matrix3x3costmultiplay(tmp3x3_6,a2,tmp3x3_2);
	matrix3x3costmultiplay(tmp3x3_5,a1,tmp3x3_1);
	matrix3x3sum(tmp3x3_1,tmp3x3_2,SS);
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"SS:\n%f %f %f\n%f %f %f\n%f %f %f\n \n",
			SS[0],SS[1],SS[2],SS[3],SS[4],SS[5],SS[6],SS[7],SS[8]);	

	//S=a1*(W1*V1'+V1*W1')+ a2*(W2*V2'+V2*W2');
//	S=a1*(vector3x3multiplication(W1,V1)+vector3x3multiplication(V1,W1))+a2*(vector3x3multiplication(W2,V2)+vector3x3multiplication(V2,W2));//S é un numero singolo
	//fprintf(outfile_ekf_data,"S %f \n",S);
	}
#endif
	*/
	//adjS=(det(S)*eye(3))/S; //restituisce una matrice diagonale 3x3
	
	
		//inv3x3_2(SS,tmp3x3_2 );
		detS=det3x3(SS);
		inv3x3(detS,SS,tmp3x3_2);
		tmp3x3_1[0]=detS;
		tmp3x3_1[1]=0.0;
		tmp3x3_1[2]=0.0;
		tmp3x3_1[3]=0.0;
		tmp3x3_1[4]=detS;
		tmp3x3_1[5]=0.0;
		tmp3x3_1[6]=0.0;
		tmp3x3_1[7]=0.0;
		tmp3x3_1[8]=detS;
		matrix3x3multiplication(tmp3x3_1,tmp3x3_2, adjS);

#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"adjS:\n%f %f %f\n%f %f %f\n%f %f %f\n ",
			adjS[0],adjS[1],adjS[2],adjS[3],adjS[4],adjS[5],adjS[6],adjS[7],adjS[8]);
	}
#endif
	//sigma=0.5*sum(diag(S));
	sigma= 0.5*(SS[0]+SS[4]+SS[8]);
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"sigma: %f \n ",sigma);
	}
#endif
	*/
	//k=sum(diag(adjS)); // esce per forza tre
	K=(adjS[0]+adjS[4]+adjS[8]);
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"K: %f \n ",K);
	}
#endif
	*/
	//delta=det(S);
	delta=detS;
	//Z=a1*cross(W1,V1)+ a2*cross(W2,V2);
	cross_product3(W1,V1,tmp3x1_1);
	cross_product3(W2,V2,tmp3x1_2);
	Z[0]= a1*tmp3x1_1[0]+a2*tmp3x1_2[0];
	Z[1]= a1*tmp3x1_1[1]+a2*tmp3x1_2[1];
	Z[2]= a1*tmp3x1_1[2]+a2*tmp3x1_2[2];
	//alpha=lambda_max^2-sigma^2+k;
	alpha=lambda_max*lambda_max-sigma*sigma+K;
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"alpha: %f \n ",alpha);
	}
#endif
	*/
	//beta=lambda_max-sigma;
	beta=lambda_max-sigma;
	//gamma=(lambda_max+sigma)*alpha-delta;
	gamma=(lambda_max+sigma)*alpha-delta;

	//X=(alpha*eye(3)+beta*S+S*S)*Z;
	tmp3x3_1[0]=alpha;
	tmp3x3_1[4]=alpha;
	tmp3x3_1[8]=alpha;
	tmp3x3_1[1]=0.0;
	tmp3x3_1[2]=0.0;
	tmp3x3_1[3]=0.0;
	tmp3x3_1[5]=0.0;
	tmp3x3_1[6]=0.0;
	tmp3x3_1[7]=0.0;
	matrix3x3costmultiplay(SS,beta,tmp3x3_2);
	matrix3x3multiplication(SS,SS, tmp3x3_3);
	matrix3x3sum(tmp3x3_1,tmp3x3_2,tmp3x3_4);
	matrix3x3sum(tmp3x3_4,tmp3x3_3,tmp3x3_5);

	X[0]=tmp3x3_5[0]*Z[0]+tmp3x3_5[1]*Z[1]+tmp3x3_5[2]*Z[2];
	X[1]=tmp3x3_5[3]*Z[0]+tmp3x3_5[4]*Z[1]+tmp3x3_5[5]*Z[2];
	X[2]=tmp3x3_5[6]*Z[0]+tmp3x3_5[7]*Z[1]+tmp3x3_5[8]*Z[2];
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"X: %f %f %f\n ",X[0],X[1],X[2]);
	}
#endif
	*/
	//q_opt = 1/sqrt(gamma^2+(norm(X))^2)*[X;gamma];
	tmp1=1/sqrt(gamma*gamma + pow(norm3(X),2));
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"tmp: %f \n ",tmp1);
	}
#endif
	*/
	q_opt[0]=X[0]*tmp1;
	q_opt[1]=X[1]*tmp1;
	q_opt[2]=X[2]*tmp1;
	q_opt[3]=gamma*tmp1;
	tmp1=1/(q_opt[0]*q_opt[0]+q_opt[1]*q_opt[1]+q_opt[2]*q_opt[2]+q_opt[3]*q_opt[3]);
	q_opt[0]=q_opt[0]*tmp1;
	q_opt[1]=q_opt[1]*tmp1;
	q_opt[2]=q_opt[2]*tmp1;
	q_opt[3]=q_opt[3]*tmp1;
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"q_opt %f %f %f %f\n",q_opt[0],q_opt[1],q_opt[2],q_opt[3]);
	}
#endif

	//PQQ=1/4*(sig_quad_tot*eye(3)+(norm(cross(W1,W2)))^-2*((sig_quad_m-sig_quad_tot)*W1*W1'+(sig_quad_acc-sig_quad_tot)*W2*W2'+sig_quad_tot*dot(W1,W2)*(W1*W2'+W2*W1')));
	vector3x3multiplication(W1,W1,tmp3x3_1);
	matrix3x3costmultiplay(tmp3x3_1,(sig_quad_m-sig_quad_tot),tmp3x3_2);

	vector3x3multiplication(W2,W2,tmp3x3_1);
	matrix3x3costmultiplay(tmp3x3_1,(sig_quad_acc-sig_quad_tot),tmp3x3_3);

	matrix3x3sum(tmp3x3_2,tmp3x3_3,tmp3x3_4);

	vector3x3multiplication(W1,W2,tmp3x3_1);
	vector3x3multiplication(W2,W1,tmp3x3_2);
	matrix3x3sum(tmp3x3_1,tmp3x3_2,tmp3x3_3);
	tmp1=dot_product3(W1,W2)*sig_quad_tot;
	matrix3x3costmultiplay(tmp3x3_3,tmp1,tmp3x3_1);

	matrix3x3sum(tmp3x3_4,tmp3x3_1,tmp3x3_2);
	matrix3x3costmultiplay(tmp3x3_2,1.0,tmp3x3_1);
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"tmp3x3_1:\n%f %f %f\n%f %f %f\n%f %f %f\n ",
			tmp3x3_1[0],tmp3x3_1[1],tmp3x3_1[2],tmp3x3_1[3],tmp3x3_1[4],tmp3x3_1[5],tmp3x3_1[6],tmp3x3_1[7],tmp3x3_1[8]);
	}
#endif	
*/	
	cross_product3(W1,W2,tmp3x1_1);
	tmp1=norm3(tmp3x1_1);
	//tmp1=1;
	/*
#ifdef DEBUG
	if(debug_option==1){
	
		fprintf(outfile_ekf_data,"tmp1 %f ,sig_quad_tot %f \n", tmp1,sig_quad_tot);
	}
#endif
*/
	tmp3x3_2[0]=(sig_quad_tot+tmp1);
	tmp3x3_2[1]=tmp1;

	tmp3x3_2[2]=tmp1;
	tmp3x3_2[4]=(sig_quad_tot+tmp1);
	tmp3x3_2[3]=tmp1;
	tmp3x3_2[5]=tmp1;
	tmp3x3_2[8]=(sig_quad_tot+tmp1);
	tmp3x3_2[6]=tmp1;
	tmp3x3_2[7]=tmp1;
	/*
#ifdef DEBUG

	if(debug_option==1){
		fprintf(outfile_ekf_data,"tmp3x3_2:\n%f %f %f\n%f %f %f\n%f %f %f\n ",
			tmp3x3_2[0],tmp3x3_2[1],tmp3x3_2[2],tmp3x3_2[3],tmp3x3_2[4],tmp3x3_2[5],tmp3x3_2[6],tmp3x3_2[7],tmp3x3_2[8]);


		fprintf(outfile_ekf_data,"tmp3x3_1:\n%f %f %f\n%f %f %f\n%f %f %f\n ",
			tmp3x3_1[0],tmp3x3_1[1],tmp3x3_1[2],tmp3x3_1[3],tmp3x3_1[4],tmp3x3_1[5],tmp3x3_1[6],tmp3x3_1[7],tmp3x3_1[8]);
	}
#endif	
*/
	matrix3x3multiplication( tmp3x3_1,tmp3x3_2, tmp3x3_3);
	matrix3x3costmultiplay(tmp3x3_3,0.25,tmp3x3_2);
/*
#ifdef DEBUG
	fprintf(outfile_ekf_data,"tmp3:\n%f %f %f \n%f %f %f \n%f %f %f\n%f %f %f \n",
			tmp3x3_2[0],tmp3x3_2[1],tmp3x3_2[2],tmp3x3_2[3],tmp3x3_2[4],tmp3x3_2[5],tmp3x3_2[6],tmp3x3_2[7],tmp3x3_2[8]);	

#endif	
*/
	PQQ[0]=tmp3x3_2[0];
	PQQ[1]=tmp3x3_2[1];
	PQQ[2]=tmp3x3_2[2];
	PQQ[3]=0.0;

	PQQ[4]=tmp3x3_2[3];
	PQQ[5]=tmp3x3_2[4];
	PQQ[6]=tmp3x3_2[5];
	PQQ[7]=0.0;
	
	PQQ[8]=tmp3x3_2[6];
	PQQ[9]=tmp3x3_2[7];
	PQQ[10]=tmp3x3_2[8];
	PQQ[11]=0.0;
	PQQ[12]=0.0;
	PQQ[13]=0.0;
	PQQ[14]=0.0;
	PQQ[15]=0.0;
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"PQQ:\n%f %f %f %f\n%f %f %f %f\n%f %f %f %f\n%f %f %f %f \n",
				PQQ[0],PQQ[1],PQQ[2],PQQ[3],PQQ[4],PQQ[5],PQQ[6],PQQ[7],PQQ[8],PQQ[9],PQQ[10],PQQ[11],PQQ[12],PQQ[13],PQQ[14],PQQ[15]);	
	}

#endif	
	*/

	//generate q_matrix
	q_matrix[0]= q_opt[3];
	q_matrix[1]= -q_opt[2];
	q_matrix[2]= q_opt[1];
	q_matrix[3]= q_opt[0];

	q_matrix[4]= q_opt[2];
	q_matrix[5]= q_opt[3];
	q_matrix[6]= -q_opt[0];
	q_matrix[7]= q_opt[1];

	q_matrix[8]=-q_opt[1];
	q_matrix[9]= q_opt[0];
	q_matrix[10]= q_opt[3];
	q_matrix[11]= q_opt[2];

	q_matrix[12]= -q_opt[0];
	q_matrix[13]= -q_opt[1];
	q_matrix[14]= -q_opt[2];
	q_matrix[15]= q_opt[3];


	q_matrixT[0]= q_opt[3];
	q_matrixT[1]= q_opt[2];
	q_matrixT[2]= -q_opt[1];
	q_matrixT[3]= -q_opt[0];

	q_matrixT[4]= -q_opt[2];
	q_matrixT[5]= q_opt[3];
	q_matrixT[6]= q_opt[0];
	q_matrixT[7]= -q_opt[1];

	q_matrixT[8]=q_opt[1];
	q_matrixT[9]= -q_opt[0];
	q_matrixT[10]= q_opt[3];
	q_matrixT[11]= -q_opt[2];

	q_matrixT[12]= q_opt[0];
	q_matrixT[13]= q_opt[1];
	q_matrixT[14]= q_opt[2];
	q_matrixT[15]= q_opt[3];

	
	/*q_matrix={ q_opt[3],-q_opt[2], q_opt[1], q_opt[0],
			   q_opt[2],  q_opt[3], -q_opt[0],q_opt[1],
			  -q_opt[1],  q_opt[0],  q_opt[3], q_opt[2],
			  -q_opt[0], -q_opt[1], -q_opt[2], q_opt[3]};
			  
	// 	q_matrix trasposta 
	q_matrixT={	q_opt[3],q_opt[2], -q_opt[1],-q_opt[0],
				-q_opt[2], q_opt[3], q_opt[0], -q_opt[1],
				q_opt[1],-q_opt[0], q_opt[3],-q_opt[2],
				q_opt[0],q_opt[1],q_opt[2], q_opt[3]};*/
	
			  
	//concatenate matrix PQQ da 3x3 diventa 4x4 con ultima riga ed ultima colonna =0
	
	//test2
	// PQQ4x4*q_matrixT
	matrix4x4multiplication(PQQ,q_matrixT,tmp4x4_1);
	matrix4x4multiplication(q_matrix,tmp4x4_1,PQQout);
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"Pqqout:\n%f %f %f %f\n%f %f %f %f\n%f %f %f %f\n%f %f %f %f \n",
			PQQout[0],PQQout[1],PQQout[2],PQQout[3],PQQout[4],PQQout[5],PQQout[6],PQQout[7],PQQout[8],PQQout[9],PQQout[10],PQQout[11],PQQout[12],PQQout[13],PQQout[14],PQQout[15]);	
	}
#endif

}

/***************kalman  gain calculation*********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes kalman gain matrix

\param double Mk[49]  				: input status matrix 
\param double Hk[28]				: hermitian matrix
\param double Pqq[16]				: input covariance matrix
\return double Kk[28]  				: kalman gain matrix

*/

void Kalman_Gain_Martrix_Full(double Mk[49], double Hk[28],double Pqq[16],  double Kk[28])
{
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"Kalman_Gain_Martrix_Full\n");
	}
#endif
	//calculate calman gain matrix full
	//Kk=(MK*Hk')* (((Hk*Mk*HK')+Pqq)^-1)
	//MK 7x7  Pqq3x3
	double HKT[28];
	double tmp7x4_1[28];
	double tmp4x7_2[28];
	double tmp4x4_1[16];
	double tmp4x4_2[16];

	int i=0;
	for (i=0; i<28;i++)
	{
		tmp7x4_1[i]=0.0;
		tmp4x7_2[i]=0.0;

	}
	for (i=0; i<16;i++)
	{
		tmp4x4_1[i]=0.0;
		tmp4x4_2[i]=0.0;
	}
	/*
	Hk=  1     0     0     0     0     0     0
	     0     1     0     0     0     0     0
	     0     0     1     0     0     0     0
	     0     0     0     1     0     0     0
	*/
	for (i=0; i<28;i++)
	{
		Hk[i]=0.0;
	}
	Hk[0]=1.0;
	Hk[8]=1.0;
	Hk[16]=1.0;
	Hk[24]=1.0;
	/*
		Htk= 
	1	0	0	0
	0	1	0	0
	0	0	1	0
	0	0	0	1
	0	0	0	0
	0	0	0	0
	0	0	0	0
	*/
	for (i=0; i<28;i++)
	{
		HKT[i]=0.0;
	}
	HKT[0]=1.0;
	HKT[5]=1.0;
	HKT[10]=1.0;
	HKT[15]=1.0;
	/*
#ifdef DEBUG
	if(debug_option==1){
	
		fprintf(outfile_ekf_data,"HKT\n");
		for(i=0; i<28; i++)
		{
			if((i==4)||(i==8)||(i==12)||(i==16)||(i==20)||(i==24))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",HKT[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	
		fprintf(outfile_ekf_data,"Mk \n");
		for(i=0; i<49; i++)
		{
			if((i==7)||(i==14)||(i==21)||(i==28)||(i==35)||(i==42))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",Mk[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
	*/

	matrix7x7_7x4multiplication(Mk,HKT, tmp7x4_1);//7x4 ->a000
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"a000\n");
		for(i=0; i<28; i++)
		{
			if((i==4)||(i==8)||(i==12)||(i==16)||(i==20)||(i==24))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",tmp7x4_1[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
*/
	matrix4x7_7x7multiplication(Hk,Mk, tmp4x7_2);
	matrix4x7_7x4multiplication(tmp4x7_2,HKT, tmp4x4_1);
	matrix4x4sum(tmp4x4_1,Pqq,tmp4x4_2);//4x4->a005
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"a005\n");
		for(i=0; i<16; i++)
		{
			if((i==4)||(i==8)||(i==12))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",tmp4x4_2[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
*/


	invertColumnMajor4(tmp4x4_2, tmp4x4_1);
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"a001\n");
		for(i=0; i<16; i++)
		{
			if((i==4)||(i==8)||(i==12))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",tmp4x4_1[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
*/

	matrix7x4_4x4multiplication(tmp7x4_1,tmp4x4_1, Kk);//restituisce una 7x4

#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"Kk\n");
		for(i=0; i<28; i++)
		{
			if((i==4)||(i==8)||(i==12)||(i==16)||(i==20)||(i==24))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",Kk[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
	

}
/***************covariance matrix calculation*********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes the covariance matrix

\param double Mk[49]  				: input status matrix 
\param double Hk[28]				: hermitian matrix
\param double Kk[28]  				: kalman gain matrix
\return double Pk[49]				: covariance output matrix

*/
void Covariance_Matrix(double Mk[49], double Hk[28],  double Kk[28], double Pk[49])
{
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"covariance matrix\n");
	}	
#endif
*/
	int i=0;
	double tmp7x7_1[49];
	double tmp7x7_2[49];
	for (i=0; i<49;i++)
	{
		tmp7x7_1[i]=0;
		tmp7x7_2[i]=0;

	}
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"Kk cov matrix\n");
		for(i=0; i<28; i++)
		{
			if((i==4)||(i==8)||(i==12)||(i==16)||(i==20)||(i==24))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",Kk[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
*/

	//(eye(7)-(Kk*Hk))*Mk
	matrix7x4_4x7multiplication(Kk,Hk, tmp7x7_1);//restituisce una 7x7
	eye_7_minus_mat(tmp7x7_1,tmp7x7_2);
	matrix7x7_7x7multiplication(tmp7x7_2,Mk, Pk);
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"covariance matrix\n Pk \n");
		for(i=0; i<49; i++)
		{
			if((i==7)||(i==14)||(i==21)||(i==28)||(i==35)||(i==42))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",Pk[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
}
/***************state correction*********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes the state correction using the kalman gain matrix

\param double Kk[28]  				: kalman gain matrix
\param double double x_k[7]			: input the status  vector
\param double Hk[28]				: hermitian matrix
\param double zk[4]					: input estimated quaternion vector
\return double Xk[7] 				: output status vector

*/
void State_Correction(double x_k[7], double Hk[28],double zk[4],  double Kk[28], double xk[7])
{
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"State_Correction\n");
	}
#endif
 */
	double tmp4x1_1[4];
	double tmp4x1_2[4];
	double tmp7x1_1[7];	
	
	double soglia_abs=4.0;
	
	double soglia_cambio_quat=(fabs(x_k[0]-zk[0])+fabs(x_k[1]-zk[1])+fabs(x_k[2]-zk[2])+fabs(x_k[3]-zk[3]));
	
	if((x_k[0]-zk[0])>=1.0)
	{
		//printf("caso 1a\n");
		zk[0]=zk[0]+2.0;
		zk[2]=-zk[2];
		zk[1]=-zk[1];
		zk[3]=-zk[3];
	}
	if((x_k[0]-zk[0])<=-1.0)
	{
		//printf("caso 2a\n");

		zk[0]=zk[0]-2.0;
		zk[2]=-zk[2];
		zk[1]=-zk[1];
		zk[3]=-zk[3];
	}
	
	if((x_k[1]-zk[1])>=1.0)
	{
		//printf("caso 1a\n");
		zk[1]=zk[1]+2.0;
		zk[2]=-zk[2];
		zk[0]=-zk[0];
		zk[3]=-zk[3];
	}
	if((x_k[1]-zk[1])<=-1.0)
	{
		//printf("caso 2a\n");

		zk[1]=zk[1]-2.0;
		zk[2]=-zk[2];
		zk[0]=-zk[0];
		zk[3]=-zk[3];
	}
	
	
	if((x_k[2]-zk[2])>=1.0)
	{
		//printf("caso 1a\n");
		zk[2]=zk[2]+2.0;
		zk[0]=-zk[0];
		zk[1]=-zk[1];
		zk[3]=-zk[3];
	}
	if((x_k[2]-zk[2])<=-1.0)
	{
		//printf("caso 2a\n");

		zk[2]=zk[2]-2.0;
		zk[0]=-zk[0];
		zk[1]=-zk[1];
		zk[3]=-zk[3];
	}
	
	if((x_k[3]-zk[3])>=1.0)
	{
		//printf("caso 3a\n");

		zk[3]=zk[3]+2.0;
		zk[0]=-zk[0];
		zk[1]=-zk[1];
		zk[2]=-zk[2];
	}
	if((x_k[3]-zk[3])<=-1.0)
	{
		//printf("caso 4a\n");

		zk[3]=zk[3]-2.0;
		zk[0]=-zk[0];
		zk[1]=-zk[1];
		zk[2]=-zk[2];
	}
	
	// Kk*(zk - x_k*Hk) + x_k 
	//tmp1_3 matlab
	tmp4x1_1[0]=x_k[0];// viene moltiplicato tutto per uno
	tmp4x1_1[1]=x_k[1];
	tmp4x1_1[2]=x_k[2];
	tmp4x1_1[3]=x_k[3];
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"State_Correction\n soglia_cambio_quat %f  \n",soglia_cambio_quat);
		//fprintf(outfile_ekf_data,"tmp_1_3 %f %f %f %f \n",tmp4x1_1[0],tmp4x1_1[1],tmp4x1_1[2],tmp4x1_1[3]);
	}
#endif
	*/
	
	tmp4x1_2[0]= zk[0]-tmp4x1_1[0];
	tmp4x1_2[1]=zk[1]-tmp4x1_1[1];
	tmp4x1_2[2]=zk[2]-tmp4x1_1[2];
	tmp4x1_2[3]=zk[3]-tmp4x1_1[3];
	

	tmp7x1_1[0]=Kk[0]*tmp4x1_2[0]+Kk[1]*tmp4x1_2[1]+Kk[2]*tmp4x1_2[2]+Kk[3]*tmp4x1_2[3];
	tmp7x1_1[1]=Kk[4]*tmp4x1_2[0]+Kk[5]*tmp4x1_2[1]+Kk[6]*tmp4x1_2[2]+Kk[7]*tmp4x1_2[3];
	tmp7x1_1[2]=Kk[8]*tmp4x1_2[0]+Kk[9]*tmp4x1_2[1]+Kk[10]*tmp4x1_2[2]+Kk[11]*tmp4x1_2[3];
	tmp7x1_1[3]=Kk[12]*tmp4x1_2[0]+Kk[13]*tmp4x1_2[1]+Kk[14]*tmp4x1_2[2]+Kk[15]*tmp4x1_2[3];

	tmp7x1_1[4]=Kk[16]*tmp4x1_2[0]+Kk[17]*tmp4x1_2[1]+Kk[18]*tmp4x1_2[2]+Kk[19]*tmp4x1_2[3];
	tmp7x1_1[5]=Kk[20]*tmp4x1_2[0]+Kk[21]*tmp4x1_2[1]+Kk[22]*tmp4x1_2[2]+Kk[23]*tmp4x1_2[3];
	tmp7x1_1[6]=Kk[24]*tmp4x1_2[0]+Kk[25]*tmp4x1_2[1]+Kk[26]*tmp4x1_2[2]+Kk[27]*tmp4x1_2[3];
	


	xk[0]=x_k[0]+tmp7x1_1[0];
	xk[1]=x_k[1]+tmp7x1_1[1];
	xk[2]=x_k[2]+tmp7x1_1[2];
	xk[3]=x_k[3]+tmp7x1_1[3];
	xk[4]=x_k[4]+tmp7x1_1[4];
	xk[5]=x_k[5]+tmp7x1_1[5];
	xk[6]=x_k[6]+tmp7x1_1[6];
	if((soglia_cambio_quat>=soglia_abs)||(avvio>=1))
	{	
		
		if((cnt_soglia>COUNT_CAMBIO_SOGLIA)||(avvio>=1))
		{
			printf("cambio soglia_cambio_quat\n");
			
			
			/*xk[0]=-xk[0];
			xk[1]=-xk[1];
			xk[2]=zk[2];
			xk[3]=zk[3];
			
			xk[4]=xk[4];
			xk[5]=xk[5];
			xk[6]=xk[6];*/
			
			if(avvio>=1)
			{
				xk[0]=zk[0];
				xk[1]=zk[1];
				xk[2]=zk[2];
				xk[3]=zk[3];
				xk[4]=xk[4];
				xk[5]=xk[5];
				xk[6]=xk[6];
				avvio --;
			}
			cnt_soglia=0;
		}
		cnt_soglia ++;
	}else
	{
		cnt_soglia=0;
	}
	//aggiunte
	
	if(xk[1]>=1.0)
	{
		//printf("caso 1\n");
		xk[0]=-xk[0];
		xk[1]=xk[1]-2.0;
		xk[2]=-xk[2];
		xk[3]=-xk[3];

	}
	if(xk[1]<=-1.0)
	{
	//printf("caso 2\n");
		xk[0]=-xk[0];
		xk[1]=xk[1]+2.0;
		xk[2]=-xk[2];
		xk[3]=-xk[3];

	}
	if(xk[0]>=1.0)
	{
		//printf("caso 1\n");
		xk[0]=xk[0]-2.0;
		xk[1]=-xk[1];
		xk[2]=-xk[2];
		xk[3]=-xk[3];

	}
	if(xk[0]<=-1.0)
	{
	//printf("caso 2\n");
		xk[0]=xk[0]+2.0;
		xk[1]=-xk[1];
		xk[2]=-xk[2];
		xk[3]=-xk[3];

	}
		
	if(xk[2]>=1.0)
	{
		//printf("caso 1\n");
		xk[0]=-xk[0];
		xk[1]=-xk[1];
		xk[2]=xk[2]-2.0;
		xk[3]=-xk[3];

	}
	if(xk[2]<=-1.0)
	{
	//printf("caso 2\n");
		xk[0]=-xk[0];
		xk[1]=-xk[1];
		xk[2]=xk[2]+2.0;
		xk[3]=-xk[3];

	}
	if(xk[3]<=-1.0)
	{
	//printf("caso 4\n");
		xk[0]=-xk[0];
		xk[1]=-xk[1];
		xk[3]=xk[3]+2.0;
		xk[2]=-xk[2];

	}

	if(xk[3]>=1.0)
	{
	//printf("caso 3\n");
		xk[0]=-xk[0];
		xk[1]=-xk[1];
		xk[3]=xk[3]-2.0;
		xk[2]=-xk[2];

	}

	/*tmpa=1/(xk[0]*xk[0]+xk[1]*xk[1]+xk[2]*xk[2]+xk[3]*xk[3]);
	xk[0]=xk[0]*tmpa;
	xk[1]=xk[1]*tmpa;
	xk[2]=xk[2]*tmpa;
	xk[3]=xk[3]*tmpa;*/

#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"XK %f %f %f %f %f %f %f \n",xk[0],xk[1],xk[2],xk[3],xk[4],xk[5],xk[6]);
	}
#endif
}

/*************** correction*********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes the correction based on the kalman gain

\param double Mk[49]  				: input status matrix 
\param double double x_k[7]			: input the status  vector
\param double Pqq[16]				: input covariance matrix
\param double zk[4]					: input estimated quaternion vector
\param double gain					: system EKF gain
\param double dt					: input time
\return double Pk[49]				: covariance output matrix
\return double Xk[7] 				: output status vector

*/
void correction(double Mk[49],double Pqq[16],double x_k[7],double zk[4],double gain, double Pk[49] ,double xk[7] )
{
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"correction\n gain %f\n",gain);
	}
#endif
	double Hk[28];
	double Kk[28];

	int i=0;
	for(i=0;i<28;i++)
	{
		Hk[i]=0.0;
		Kk[i]=0.0;
	}
	Kalman_Gain_Martrix_Full(Mk, Hk,Pqq, Kk);
	for (i=0; i<28; i++)
	{
		Kk[i]=Kk[i]*gain;
	}
	Covariance_Matrix(Mk, Hk, Kk,Pk);
	State_Correction( x_k,  Hk, zk,   Kk, xk);

}


/***************prediction*********************/
//-----------------------------------------------------------------
/*!
\brief This routine computes the prediction of the status

\param double xk[7]					: input status vector
\param double Pk[49]				: input status matrix
\param double omega[3]				: input gyro vector
\param double q_quat[4]				: input quaternion vector
\param double Q_bias[3]				: input quaternion biasing vector
\param double dt					: input time
\return double Mk_p1[49  			: output status matrix prediction
\return double x_k_p1[7]  			: output status vector prediction
*/

void Prediction(double xk[7] ,double Pk[49],double omega[3],double dt,double q_quat[4],double Q_bias[3],double Mk_p1[49],double x_k_p1[7])
{
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"\nprediction\n");
	}
#endif
*/
	double tmp7x7_1[49]; 
	double PHT[49];
	double PH[49] ;
	int i=0;
	for(i=0;i<=48;i++)
	{
		tmp7x7_1[i]=0.0;
		PHT[i]=0.0;
		PH[i]=0.0;
	}
	if(xk[4]>=SATURATE_BIAS_GYRO)
	{
		xk[4]=SATURATE_BIAS_GYRO;
	}
	if(xk[4]<=-SATURATE_BIAS_GYRO)
	{
		xk[4]=-SATURATE_BIAS_GYRO;
	}
	
	if(xk[5]>=SATURATE_BIAS_GYRO)
	{
		xk[5]=SATURATE_BIAS_GYRO;
	}
	if(xk[5]<=-SATURATE_BIAS_GYRO)
	{
		xk[5]=-SATURATE_BIAS_GYRO;
	}
	
	if(xk[6]>=SATURATE_BIAS_GYRO)
	{
		xk[6]=SATURATE_BIAS_GYRO;
	}
	if(xk[6]<=-SATURATE_BIAS_GYRO)
	{
		xk[6]=-SATURATE_BIAS_GYRO;
	}
	
	//double Q_matrix[49];
	//f(x_k)
	double q1=xk[0];
	double q2=xk[1];
	double q3=xk[2];
	double q0=xk[3];
	double bp=xk[4];
	double bq=xk[5];
	double br=xk[6];
	double p= omega[0];
	double q=omega[1];
	double r=omega[2];


	double dotxk[7];
	for(i=0;i<7;i++)
	{
		dotxk[i]=0.0;

	}
	i=0;
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"prediction\n q1 %f q2 %f q3 %f q0 %f \n",q1,q2,q3,q0);
		fprintf(outfile_ekf_data,"bp %f bq %f br %f \n",bp,bq,br);
	}
#endif
	*/

	dotxk[0]=		0.5*q0*(p-bp)-0.5*q3*(q-bq)+0.5*q2*(r-br);
	dotxk[1]= 		0.5*q3*(p-bp)+0.5*q0*(q-bq)-0.5*q1*(r-br);
	dotxk[2]=       -0.5*q2*(p-bp)+0.5*q1*(q-bq)+0.5*q0*(r-br);
	dotxk[3]=       -0.5*q1*(p-bp)-0.5*q2*(q-bq)-0.5*q3*(r-br);
	dotxk[4]=       0.0;
	dotxk[5]=       0.0;
	dotxk[6]=       0.0;
	/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"dotxk %f %f %f %f %f %f %f \n",dotxk[0],dotxk[1],dotxk[2],dotxk[3],dotxk[4],dotxk[5],dotxk[6]);
	}
#endif
	*/
	for(i=0;i<7; i++)
	{
		dotxk[i]=dotxk[i]*dt;
		x_k_p1[i]=dotxk[i]+xk[i];
	}
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"dotxkdt %f %f %f %f %f %f %f \n",dotxk[0],dotxk[1],dotxk[2],dotxk[3],dotxk[4],dotxk[5],dotxk[6]);

		fprintf(outfile_ekf_data,"x_k_p1 %f %f %f %f %f %f %f \n",x_k_p1[0],x_k_p1[1],x_k_p1[2],x_k_p1[3],x_k_p1[4],x_k_p1[5],x_k_p1[6]);
	}
#endif
	//d f(x) / dx

	PH[0]=  0;
	PH[1]=  0.5*(r-br);
	PH[2]=  0.5*(-q+bq);
	PH[3]=  0.5*(p-bp);
	PH[4]=  -0.5*q0;
	PH[5]=  0.5*q3;
	PH[6]=  -0.5*q2;

	PH[7]=  0.5*(-r+br);
	PH[8]=  0;
	PH[9]=  0.5*(p-bp);
	PH[10]= 0.5*(q-bq);
	PH[11]= -0.5*q3;
	PH[12]= -0.5*q0;
	PH[13]=  0.5*q1;


	PH[14]= 0.5*(q-bq);
	PH[15]= 0.5*(-p+bp);
	PH[16]= 0;
	PH[17]= 0.5*(r-br);
	PH[18]= 0.5*q2;
	PH[19]= -0.5*q1;
	PH[20]= -0.5*q0;

	PH[21]= 0.5*(-p+bp);
	PH[22]= 0.5*(-q+bq);
	PH[23]= 0.5*(-r+br);
	PH[24]= 0;
	PH[25]= 0.5*q1;
	PH[26]= 0.5*q2;
	PH[27]= 0.5*q3;

/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"PH\n");
		for(i=0; i<49; i++)
		{
			if((i==7)||(i==14)||(i==21)||(i==28)||(i==35)||(i==42))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",PH[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
*/
	for(i=0;i<49; i++)
	{
		PH[i]=PH[i]*dt;
	}
	//F+eye(7)
	PH[0]=PH[0]+1.0;
	PH[8]=PH[8]+1.0;
	PH[16]=PH[16]+1.0;
	PH[24]=PH[24]+1.0;
	PH[32]=PH[32]+1.0;
	PH[40]=PH[40]+1.0;
	PH[48]=PH[48]+1.0;
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"PH\n");
		for(i=0; i<49; i++)
		{
			if((i==7)||(i==14)||(i==21)||(i==28)||(i==35)||(i==42))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",PH[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif

	trasp7x7(PH,PHT);
	matrix7x7_7x7multiplication(PH,Pk, tmp7x7_1);
	matrix7x7_7x7multiplication(tmp7x7_1,PHT, Mk_p1);
/*
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"a4001\n");
		for(i=0; i<49; i++)
		{
			if((i==7)||(i==14)||(i==21)||(i==28)||(i==35)||(i==42))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",Mk_p1[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif
*/	
	/*for(i=0;i<49; i++)
	{
		Q_matrix[i]=0;
	}
	//F+eye(7)
	Q_matrix[0]=q_quat[0];
	Q_matrix[8]=q_quat[0];
	Q_matrix[16]=q_quat[0];
	Q_matrix[24]=q_quat[0];
	Q_matrix[32]=Q_bias;
	Q_matrix[40]=Q_bias;
	Q_matrix[48]=Q_bias;
	*/

	Mk_p1[0]=Mk_p1[0]+q_quat[0];
	Mk_p1[8]=Mk_p1[8]+q_quat[1];
	Mk_p1[16]=Mk_p1[16]+q_quat[2];
	Mk_p1[24]=Mk_p1[24]+q_quat[3];
	Mk_p1[32]=Mk_p1[32]+Q_bias[0];
	Mk_p1[40]=Mk_p1[40]+Q_bias[1];
	Mk_p1[48]=Mk_p1[48]+Q_bias[2];
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"x_k_p1\n %f %f %f %f %f %f %f \n",x_k_p1[0],x_k_p1[1],x_k_p1[2],x_k_p1[3],x_k_p1[4],x_k_p1[5],x_k_p1[6]);
		fprintf(outfile_ekf_data,"Mk_p1\n");
		for(i=0; i<49; i++)
		{
			if((i==7)||(i==14)||(i==21)||(i==28)||(i==35)||(i==42))
			fprintf(outfile_ekf_data,"\n");
			fprintf(outfile_ekf_data,"%f ",Mk_p1[i]);
		}
		fprintf(outfile_ekf_data,"\n");
	}
#endif

}
//-----------------------------------------------------------------
/*!
\brief This routine computes the EKF based of quest computation

\param double zk[4]					: input quaternion stimated vector
\param double omega[3]				: input gyro vector
\param double Pqq[16]				: input covariance matrix
\param double q_quat[4]				: input quaternion vector
\param double Q_bias[3]				: input quaternion biasing vector
\param double dt					: input time
\return double Xk[7] 				: output status vector

*/
void EKF_AHRS(double zk[4], double omega[3], double Pqq[16], double dt, double q_quat[4], double Q_bias[3], double Xk[7])
{
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"EKF_AHRS\n");
	}
#endif
	double Pk[49] ;
	int i;
	for(i=0; i<49; i++)
	{
		Pk[i]=0.0;
	}
	if(soft_start_gain>=1)
	{
		gain_G=(double)START_GAIN;
		soft_start_gain --;
	}
	else
	{
		gain_G=stored_gain;
	}

	correction(Mk_p1_G, Pqq,x_k_p1_G,zk,gain_G,  Pk , Xk);
	Prediction( Xk , Pk, omega, dt, q_quat, Q_bias, Mk_p1_G, x_k_p1_G);
}
//-----------------------------------------------------------------
/*!
\brief This routine computes the EKF

\param float acc_in[3]					: acceleration vector [m/s^2] body frame
\param float mag_in[3]      			: mag vector [T] body frame
\param  float gyro_in[3]      			: angular rate vector [rad/sec] body frame
\param float dt_in      				: time of integration [s]
\param  float q_quat_in[4]				: quaternion vector [q]
\param float Q_bias_in[3]      			: quaternion biasing system

\return float quat[4]  					: quaternion output
\return float gyro_out[3]  				: gyro output bias corrected [rad/sec] body frame
*/
void EKF(float acc_in[3], float mag_in[3], float gyro_in[3],float dt_in, float q_quat_in[4], float Q_bias_in[3], float quat[4], float gyro_out[3])
{

	double acc[3];
	double mag[3];
	double gyro[3];
	double dt;
	double q_quat[4]; 
	double Q_bias[3];
	acc[0]=(double)acc_in[0];
	acc[1]=(double)acc_in[1];
	acc[2]=(double)acc_in[2];
	
	mag[0]=(double)mag_in[0];
	mag[1]=(double)mag_in[1];
	mag[2]=(double)mag_in[2];
	
	gyro[0]=(double)gyro_in[0];
	gyro[1]=(double)gyro_in[1];
	gyro[2]=(double)gyro_in[2];
	
	q_quat[0]=(double)q_quat_in[0];
	q_quat[1]=(double)q_quat_in[1];
	q_quat[2]=(double)q_quat_in[2];
	q_quat[3]=(double)q_quat_in[3];
	
	Q_bias[0]=(double)Q_bias_in[0];
	Q_bias[1]=(double)Q_bias_in[1];
	Q_bias[2]=(double)Q_bias_in[2];
	
	
	dt=(double)dt_in;

	
	#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"row %ld dt %f \n",rowcounter,dt);
	}
	rowcounter++;
	#endif

	int i=0;
	double PQQout[16];
	double q_opt[4];
	double omega[3];
	for (i=0; i<16; i++)
	{
		PQQout[i]=0.0;
		if(i<=3)
		{
			q_opt[i]=0.0;
		}
		if(i<=2)
		{
		omega[i]=0.0;
		}
	}
	//omega[0]=+gyro[0]-bias[0];
	//omega[1]=+gyro[1]-bias[1];
	//omega[2]=+gyro[2]-bias[2];
	
	omega[0]=gyro[0];
	omega[1]=gyro[1];
	omega[2]=gyro[2];

	quest(acc, mag, PQQout ,q_opt);

	 EKF_AHRS( q_opt,  omega,  PQQout,  dt,  q_quat,  Q_bias,  Xk_G);
#ifdef DEBUG
	if(debug_option==1){
		fprintf(outfile_ekf_data,"EKF\n Xk_G %f %f %f %f \n",Xk_G[0],Xk_G[1],Xk_G[2],Xk_G[3]);
		fprintf(outfile_ekf_data,"Xk_G %f %f %f \n",Xk_G[4],Xk_G[5],Xk_G[6]);
	}
#endif
	quat[0]=(float)Xk_G[0];
	quat[1]=(float)Xk_G[1];
	quat[2]=(float)Xk_G[2];
	quat[3]=(float)Xk_G[3];
	bias[0]=Xk_G[4];
	bias[1]=Xk_G[5];
	bias[2]=Xk_G[6];
	gyro_out[0]=(float)omega[0];
	gyro_out[1]=(float)omega[1];
	gyro_out[2]=(float)omega[2];

}


		/***************derivate for acc calculation*********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes linear acceleration of the system

\param double heading					: heading of velocity [rad] NED frame
\param double dt       					: integration time [s]
\param double velocity      			: velocity estimated [m/s] NED frame
\param double altitude       			: altitude of the system [m] body frame
\return double accvector_linear[3]  	: undesired acceleration [m/s^2] NED frame
\return double velout[3]  				: vel out [m/s] NED frame
*/

void calculate_acc(double velocity,double altitude, double dt,double heading,double accvector_linear[3],double velout[3])
{
	double velocity_X=velocity*cos(heading);
	double velocity_Y=velocity*sin(heading);
	double dt_GPS=1.0;
	if(velocity_X>=20.0)
	{
		velocity_X=20.0;
	}
	if(velocity_X<=-20.0)
	{
		velocity_X=-20.0;
	}
	if(velocity_Y>=20.0)
	{
		velocity_Y=20.0;
	}
	if(velocity_Y<=-20.0)
	{
		velocity_Y=-20.0;
	}
	altitude=altitude/100;
	accvector_linear[0]=((((velocity_X-velocity_stored_X)/dt_GPS)+(velocity_X-velocity_stored1_X)/(2*dt_GPS)))/2;
	accvector_linear[1]=((((velocity_Y-velocity_stored_Y)/dt_GPS)+(velocity_Y-velocity_stored1_Y)/(2*dt_GPS)))/2;

	accvector_linear[2]=(((altitude-altitude_stored)/(dt_GPS*dt_GPS))+((altitude-altitude_stored1)/(2*dt_GPS*dt_GPS))+((altitude-altitude_stored2)/(6*dt_GPS*dt_GPS)))/3;//((velz-velz_stored)/dt+(velz-velz_stored1)/(2*dt));
	velout[0]=velocity_X;
	velout[1]=velocity_Y;
	velout[2]=(((altitude-altitude_stored)/(dt_GPS))+((altitude-altitude_stored1)/(2*dt_GPS))+((altitude-altitude_stored2)/(6*dt_GPS)))/3;

	altitude_stored=altitude;
	altitude_stored1=altitude_stored;
	altitude_stored2=altitude_stored1;
	velocity_stored_X=velocity_X;
	velocity_stored1_X=velocity_stored_X;
	velocity_stored_Y=velocity_Y;
	velocity_stored1_Y=velocity_stored_Y;
	
	if(accvector_linear[2]>=MAX_ACCEL_CORRECTION)
	{
		accvector_linear[2]=MAX_ACCEL_CORRECTION;
	}
	if(accvector_linear[2]<=-MAX_ACCEL_CORRECTION)
	{
		accvector_linear[2]=-MAX_ACCEL_CORRECTION;
	}
	
	if(accvector_linear[1]>=MAX_ACCEL_CORRECTION)
	{
		accvector_linear[1]=MAX_ACCEL_CORRECTION;
	}
	if(accvector_linear[1]<=-MAX_ACCEL_CORRECTION)
	{
		accvector_linear[1]=-MAX_ACCEL_CORRECTION;
	}
	if(accvector_linear[0]>=MAX_ACCEL_CORRECTION)
	{
		accvector_linear[0]=MAX_ACCEL_CORRECTION;
	}
	if(accvector_linear[0]<=-MAX_ACCEL_CORRECTION)
	{
		accvector_linear[0]=-MAX_ACCEL_CORRECTION;
	}
}

		/***************rotation matrix*********************/ 

void rotation_matrix_Z(double datain[3], double angle,double dataout[3])
{
	dataout[0]=datain[0]*cos(angle)-  datain[1]*sin(angle);
	dataout[1]=datain[0]*sin(angle)+  datain[1]*cos(angle);
	dataout[2]=datain[2];

}

void rotation_matrix_Y(double datain[3], double angle,double dataout[3])
{
	dataout[0]=datain[0]*cos(angle)+ datain[2]*sin(angle);
	dataout[2]=-datain[0]*sin(angle)+ datain[2]*cos(angle);
	dataout[1]=datain[1];

}
void rotation_matrix_X(double datain[3], double angle,double dataout[3])
{
	dataout[1]=datain[1]*cos(angle)-  datain[2]*sin(angle);
	dataout[2]=datain[1]*sin(angle)+  datain[2]*cos(angle);
	dataout[0]=datain[0];

}
	/***************compute the rotation vector from quaternion *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes the totation vector 3x1 from quaternion
\based on body frame system

\param double quat[4]       			: estimated quaternion body frame
\param double datain[3]       			: data in NED frame
\param double dataout[3]      			: data out in body frame
*/
void rotation_vectror_by_quaternion(double datain[3], double quat[4],double dataout[3])
{
double q0=quat[2];
double q1=quat[1];
double q2=quat[0];
double q3=quat[3];


	dataout[0]=-datain[0]*(1-2*q2*q2-2*q3*q3)-datain[1]*2*(q1*q2+q0*q3)+datain[2]*2*(q1*q3-q0*q2);
	dataout[1]=-datain[0]*2*(q1*q2-q0*q3)-datain[1]*(1-2*q1*q1-2*q3*q3)+datain[2]*2*(q2*q3+q0*q1);
	dataout[2]=-datain[0]*2*(q1*q3+q0*q2)-datain[1]*2*(q2*q3-q0*q1)+datain[2]*(1-2*q1*q1-2*q2*q2);

}

	/***************compute accelerometer correction in body frame *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes the correction of centripetal and linear acceleration of accelerometers 
\based on body frame system

\param double quat[4]       			: estimated quaternion body frame
\param double acc_derivated[3]       	: linear acceleration input based on velocity derivation [m/s^2] NED frame
\param double vel[3]      				: velocity estimated [m/s] NED frame
\param double omega[3]       			: angular turn estimated [rad/s] body frame
\return double acc_bias_correction[3]  	: undesired acceleration [m/s^2] body frame
*/

void accbias_correction(double quat[4],double acc_derivated[3],double vel[3],double omega[3],double acc_bias_correction[3])
{

	double acclinearbody[3];
	double velbody[3];
	double coriolisacc[3];
	//printf("yaw %f pitch %f  roll %f\n",yaw_t,roll_t,pitch_t);
	//calculate body linear acceleratrion linear
	rotation_vectror_by_quaternion(acc_derivated, quat,acclinearbody);
	//calculate body coriolis acceleration
	rotation_vectror_by_quaternion(vel, quat,velbody);
	cross_product3(omega,velbody,coriolisacc);
	acc_bias_correction[0]=GAIN_linear_CORRECTION*acclinearbody[0]+coriolisacc[0]*GAIN_CORIOLIS_CORRECTION;//pitch
	acc_bias_correction[1]=GAIN_linear_CORRECTION*acclinearbody[1]+coriolisacc[1]*GAIN_CORIOLIS_CORRECTION;//roll
	acc_bias_correction[2]=GAIN_linear_CORRECTION*acclinearbody[2]+coriolisacc[2]*GAIN_CORIOLIS_CORRECTION;//yaw
		
	if(acc_bias_correction[2]>=MAX_ACCEL_CORRECTION)
	{
		acc_bias_correction[2]=MAX_ACCEL_CORRECTION;
	}
	if(acc_bias_correction[2]<=-MAX_ACCEL_CORRECTION)
	{
		acc_bias_correction[2]=-MAX_ACCEL_CORRECTION;
	}
	
	if(acc_bias_correction[1]>=MAX_ACCEL_CORRECTION)
	{
		acc_bias_correction[1]=MAX_ACCEL_CORRECTION;
	}
	if(acc_bias_correction[1]<=-MAX_ACCEL_CORRECTION)
	{
		acc_bias_correction[1]=-MAX_ACCEL_CORRECTION;
	}
	if(acc_bias_correction[0]>=MAX_ACCEL_CORRECTION)
	{
		acc_bias_correction[0]=MAX_ACCEL_CORRECTION;
	}
	if(acc_bias_correction[0]<=-MAX_ACCEL_CORRECTION)
	{
		acc_bias_correction[0]=-MAX_ACCEL_CORRECTION;
	}

}

void biasgyro_dynamics(float gyroin[3],float gyroout[3])
{
	double tmp_data[3];
	if(cnt_gyro_biasing>=(END_BIASING_TIMEOUT_WINDOW-1))
	{
		cnt_gyro_biasing=0;
		tmp_bias_gyro[0]=tmp_bias_gyro[0]/END_BIASING_TIMEOUT_WINDOW;
		tmp_bias_gyro[1]=tmp_bias_gyro[1]/END_BIASING_TIMEOUT_WINDOW;
		tmp_bias_gyro[2]=tmp_bias_gyro[2]/END_BIASING_TIMEOUT_WINDOW;
		gyrobiasing_stored[0]=gyrobiasing_stored[0]+tmp_bias_gyro[0]/TIMING_BIASING_CORRECTION;
		gyrobiasing_stored[1]=gyrobiasing_stored[1]+tmp_bias_gyro[1]/TIMING_BIASING_CORRECTION;
		gyrobiasing_stored[2]=gyrobiasing_stored[2]+tmp_bias_gyro[2]/TIMING_BIASING_CORRECTION;
		//printf(" tmp %f %f %f ",tmp_bias_gyro[0],tmp_bias_gyro[1],tmp_bias_gyro[2]);
		//printf("stored %f %f %f \n",gyrobiasing_stored[0],gyrobiasing_stored[1],gyrobiasing_stored[2]);

		tmp_bias_gyro[0]=0.0;
		tmp_bias_gyro[1]=0.0;
		tmp_bias_gyro[2]=0.0;
	}
	gyroout[0]=gyroin[0]-(float)gyrobiasing_stored[0];
	gyroout[1]=gyroin[1]-(float)gyrobiasing_stored[1];
	gyroout[2]=gyroin[2]-(float)gyrobiasing_stored[2];
	
	tmp_data[0]=(double)gyroin[0]-gyrobiasing_stored[0];
	tmp_data[1]=(double)gyroin[1]-gyrobiasing_stored[1];
	tmp_data[2]=(double)gyroin[2]-gyrobiasing_stored[2];
	
	tmp_bias_gyro[0]=tmp_bias_gyro[0]+tmp_data[0];
	tmp_bias_gyro[1]=tmp_bias_gyro[1]+tmp_data[1];
	tmp_bias_gyro[2]=tmp_bias_gyro[2]+tmp_data[2];
	cnt_gyro_biasing ++;

}
//====================================================================================================
// END OF CODE
//====================================================================================================

/*
     
int main(int argc, char *argv[])
{
int i=0;
float acc[3];
float mag[3];
mag[0]=0.5;
mag[1]=0.5;
mag[2]=0.2;
acc[0]=0.5;
acc[1]=0.2;
acc[2]=0.50;
float dt=0.001;
float q_quat[4];
float quat[4];
 float Q_bias[3];
float gyro_out[3];
float gyro[3];
q_quat[0]=0.3;
q_quat[1]=0.3;
q_quat[2]=0.3;
q_quat[3]=0.7;
Q_bias[0]=0.1;
Q_bias[1]=0.2;
Q_bias[2]=0.3;
gyro[0]=0.0;
gyro[1]=0.0;
gyro[2]=0.0;
#ifdef DEBUG
	printf("start \n");
#endif 
inizialize_quest(0.01, 0.01, 0.001 );
for(i=0; i<1000;i++)
 EKF(acc, mag,  gyro,dt,  q_quat,  Q_bias,  quat,  gyro_out);


printf("\n quat %f %f %f %f \n",quat[0],quat[1],quat[2],quat[3]);
//quest(acc, mag);
return 0;
}
*/
