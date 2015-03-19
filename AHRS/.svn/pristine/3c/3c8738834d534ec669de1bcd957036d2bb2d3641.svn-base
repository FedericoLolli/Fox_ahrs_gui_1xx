/*
 ******************************************************************************
 *
 * 						CUSTOM LIBRARY MODULES
 *
 * Copyright (c) 2013 9D System
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
 #include <stdio.h>
#ifndef AHRSEKF_h
#define AHRSEKF_h


//----------------------------------------------------------------------------------------------------
// Variable declaration

extern volatile double sig_acc;				
extern volatile double sig_m ;	
extern volatile		double sig_quad_acc,sig_quad_m,sig_quad_tot,a1,a2;
extern volatile double gain_G;


//---------------------------------------------------------------------------------------------------
// Function declarations
//-----------------------------------------------------------------

//-----------------------------------------------------------------
/*!
\brief This routine inizialize the EKF paramiters

\param double dev_stad_accell   : acc dev standard   ( sigma^2 )
\param double dev_std_mag       : mag dev standard   ( sigma^2 )
\param double gain       		: EKF gain

\return nothing
*/
void inizialize_quest(double dev_stad_accell, double dev_std_mag, double gain );
//-----------------------------------------------------------------
/*!
\brief This routine save the file stream of computational data

\param FILE *data     : output filestream
\return nothing
*/
void set_file_data(FILE *data);
//-----------------------------------------------------------------
/*!
\brief This routine computes essential datum values from basic parameters
obtained from the \e mag structure.

\param double x       : mag declination x axis   ( T )
\param double y       : mag declination Y axis   ( T )
\param double z       : mag declination Z axis   ( T )

\return nothing
*/
void set_mag_declination(double x,double y,double z );
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
void accbias_correction(double quat[4],double acc_derivated[3],double vel[3],double omega[3],double acc_bias_correction[3]);
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
void calculate_acc(double velocity,double altitude, double dt,double heading,double accvector_linear[3],double velout[3]);
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
void EKF(float acc_in[3], float mag_in[3], float gyro_in[3],float dt_in, float q_quat_in[4], float Q_bias_in[3], float quat[4], float gyro_out[3]);
//-----------------------------------------------------------------
/*!
\brief This routine computes optimized quest for quaternion computation

\param double W1[3]					: input vector for optimizing
\param double W2[3]					: input vector for optimizing

\return double PQQout[16]  			: quaternion status covariance matrix
\return double q_opt[4]  			: quaternoon computation
*/
void quest(double W1[3], double W2[3],double PQQout[16] ,double q_opt[4]);
//-----------------------------------------------------------------
/*!
\brief This routine computes the prediction of the status

\param double xk[7]					: input status vector
\param double Pk[49]				: input status matrix
\param double omega[3]				: input gyro vector
\param double q_quat[4]				: input quaternion vector
\param double Q_bias[3]				: input quaternion biasing vector
\param double dt					: input time
\return double Mk_p1[49]  			: output status matrix prediction
\return double x_k_p1[7]  			: output status vector prediction
*/
void Prediction(double xk[7] ,double Pk[49],double omega[3],double dt,double q_quat[4],double Q_bias[3],double Mk_p1[49],double x_k_p1[7]);
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
void EKF_AHRS(double zk[4], double omega[3], double Pqq[16], double dt, double q_quat[4], double Q_bias[3], double Xk[7]);
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
void correction(double Mk[49],double Pqq[16],double x_k[7],double zk[4],double gain, double Pk[49] ,double xk[7] );
//-----------------------------------------------------------------
/*!
\brief This routine computes the state correction using the kalman gain matrix

\param double Kk[28]  				: kalman gain matrix
\param double double x_k[7]			: input the status  vector
\param double Hk[28]				: hermitian matrix
\param double zk[4]					: input estimated quaternion vector
\return double Xk[7] 				: output status vector

*/
void State_Correction(double x_k[7], double Hk[28],double zk[4],  double Kk[28], double xk[7]);
//-----------------------------------------------------------------
/*!
\brief This routine computes the covariance matrix

\param double Mk[49]  				: input status matrix 
\param double Hk[28]				: hermitian matrix
\param double Kk[28]  				: kalman gain matrix
\return double Pk[49]				: covariance output matrix

*/
void Covariance_Matrix(double Mk[49], double Hk[28],  double Kk[28], double Pk[49]);
//-----------------------------------------------------------------
/*!
\brief This routine computes kalman gain matrix

\param double Mk[49]  				: input status matrix 
\param double Hk[28]				: hermitian matrix
\param double Pqq[16]				: input covariance matrix
\return double Kk[28]  				: kalman gain matrix

*/
void Kalman_Gain_Martrix_Full(double Mk[49], double Hk[28],double Pqq[16],  double Kk[28]);


/*!
\brief This is a packet of mathematical function 
*/

void matrix7x4_4x4multiplication(double a[28],double  b[16], double tmp[28]);//restituisce una 7x4
double norm3(double a[3]);
void trasp7x7(double a[49],double b[49]);
void matrix4x4sum(double a[16],double  b[16],double c[16]);
void eye_7_minus_mat(double a[49],double b[49]);
void matrix7x7_7x7multiplication(double a[49],double  b[49], double tmp[49]);//restituisce una 7x7
void matrix7x4_4x7multiplication(double a[28],double  b[28], double tmp[49]);
void matrix4x7_7x4multiplication(double a[28],double  b[28], double tmp[16]);
void matrix4x7_7x7multiplication(double a[28],double  b[49], double tmp[28]);
void matrix7x7_7x4multiplication(double a[49],double  b[28], double tmp[28]);
void matrix3x3multiplication(double a[9],double  b[9], double tmp[9]);
void matrix4x4multiplication(double a[16],double  b[16], double tmp[16]);
int invertColumnMajor4(double m[16], double invOut[16]);
double det4x4(double a[16]);
void inv3x3(double det,double a[9],double b[9] );
double det3x3(double a[9]);
void matrix3x3_1div(double a[9],double c[9]);
void matrix3x3costmultiplay(double a[9],double  b,double c[9]);
void matrix3x3sum(double a[9],double  b[9],double c[9]);
void vector3x3multiplication(double a[3],double  b[3],double c[9]);
void normalize3(double a[3],double b[3]);
double  dot_product3(double a[3],double  b[3]);
void cross_product3(double a[3],double  b[3],double c[3]);
void rotation_matrix_Z(double datain[3], double angle,double dataout[3]);
void rotation_matrix_Y(double datain[3], double angle,double dataout[3]);
void rotation_matrix_X(double datain[3], double angle,double dataout[3]);
void biasgyro_dynamics(float gyroin[3],float gyroout[3]);
	/***************compute the rotation vector from quaternion *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine computes the totation vector 3x1 from quaternion
\based on body frame system

\param double quat[4]       			: estimated quaternion body frame
\param double datain[3]       			: data in NED frame
\param double dataout[3]      			: data out in body frame
*/
void rotation_vectror_by_quaternion(double datain[3], double quat[4],double dataout[3]);

#endif
//=====================================================================================================
// End of file
//=====================================================================================================
