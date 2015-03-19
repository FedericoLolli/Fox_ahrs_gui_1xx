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
 * File	Name	: loadconf.c
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

	
#include "loadconf.h"
#include <stdio.h>
#include <stdlib.h>
//#include <libconfig.h>

typedef union config_value_t
{
  long ival;
  long long llval;
  double fval;
  char *sval;
  struct config_list_t *list;
} config_value_t;

typedef struct config_setting_t
{
  char *name;
  short type;
  short format;
  config_value_t value;
  struct config_setting_t *parent;
  struct config_t *config;
  void *hook;
  unsigned int line;
} config_setting_t;

typedef struct config_list_t
{
  unsigned int length;
  unsigned int capacity;
  config_setting_t **elements;
} config_list_t;

typedef struct config_t
{
  config_setting_t *root;
  void (*destructor)(void *);
  int flags;
  const char *error_text;
  int error_line;
} config_t;

	/***************load configuration file *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine load configuration file

\return load_data_struct  	: file saved configuration
*/
 load_data_struct load_configuration()
{
  config_t cfg;

  load_data_struct loaddata;

  config_init(&cfg);
  int errorparser=0;
  /* Read the file. If there is an error, report it and exit. */
  if(!config_read_file(&cfg, "/home/config.cfg"))
  {
    printf("error configuration file \n");
    config_destroy(&cfg);
	loaddata.filter_type_parser=-1;
    return loaddata;
  }
  /*read string*/
  double tmp;
	
    if(config_lookup_float(&cfg, "acc_alignment_0", &tmp))
	{
		loaddata.acc_alignment_parser[0]=(float)tmp;
		printf("acc_alignment_0: %f\n", loaddata.acc_alignment_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_1", &tmp))
	{
		loaddata.acc_alignment_parser[1]=(float)tmp;
		printf("acc_alignment_1: %f\n", loaddata.acc_alignment_parser[1]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_2", &tmp))
	{
		loaddata.acc_alignment_parser[2]=(float)tmp;
		printf("acc_alignment_2: %f\n", loaddata.acc_alignment_parser[2]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_3", &tmp))
	{
		loaddata.acc_alignment_parser[3]=(float)tmp;
		printf("acc_alignment_3: %f\n", loaddata.acc_alignment_parser[3]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_4", &tmp))
	{
		loaddata.acc_alignment_parser[4]=(float)tmp;
		printf("acc_alignment_4: %f\n", loaddata.acc_alignment_parser[4]);
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	if(config_lookup_float(&cfg, "acc_alignment_5", &tmp))
	{
		loaddata.acc_alignment_parser[5]=(float)tmp;
		printf("acc_alignment_5: %f\n", loaddata.acc_alignment_parser[5]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_6", &tmp))
	{
		loaddata.acc_alignment_parser[6]=(float)tmp;
		printf("acc_alignment_6: %f\n", loaddata.acc_alignment_parser[6]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_7", &tmp))
	{
		loaddata.acc_alignment_parser[7]=(float)tmp;
		printf("acc_alignment_7: %f\n", loaddata.acc_alignment_parser[7]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "acc_alignment_8", &tmp))
	{
		loaddata.acc_alignment_parser[8]=(float)tmp;
		printf("acc_alignment_8: %f\n", loaddata.acc_alignment_parser[8]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	
    if(config_lookup_float(&cfg, "mag_alignment_0", &tmp))
	{
		loaddata.mag_alignment_parser[0]=(float)tmp;
		printf("mag_alignment_0: %f\n", loaddata.mag_alignment_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_1", &tmp))
	{
		loaddata.mag_alignment_parser[1]=(float)tmp;
		printf("mag_alignment_1: %f\n", loaddata.mag_alignment_parser[1]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_2", &tmp))
	{
		loaddata.mag_alignment_parser[2]=(float)tmp;
		printf("mag_alignment_2: %f\n", loaddata.mag_alignment_parser[2]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_3", &tmp))
	{
		loaddata.mag_alignment_parser[3]=(float)tmp;
		printf("mag_alignment_3: %f\n", loaddata.mag_alignment_parser[3]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_4", &tmp))
	{
		loaddata.mag_alignment_parser[4]=(float)tmp;
		printf("mag_alignment_4: %f\n", loaddata.mag_alignment_parser[4]);
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	if(config_lookup_float(&cfg, "mag_alignment_5", &tmp))
	{
		loaddata.mag_alignment_parser[5]=(float)tmp;
		printf("mag_alignment_5: %f\n", loaddata.mag_alignment_parser[5]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_6", &tmp))
	{
		loaddata.mag_alignment_parser[6]=(float)tmp;
		printf("mag_alignment_6: %f\n", loaddata.mag_alignment_parser[6]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_7", &tmp))
	{
		loaddata.mag_alignment_parser[7]=(float)tmp;
		printf("mag_alignment_7: %f\n", loaddata.mag_alignment_parser[7]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_alignment_8", &tmp))
	{
		loaddata.mag_alignment_parser[8]=(float)tmp;
		printf("mag_alignment_8: %f\n", loaddata.mag_alignment_parser[8]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	
	
    if(config_lookup_float(&cfg, "gyro_alignment_0", &tmp))
	{
		loaddata.gyro_alignment_parser[0]=(float)tmp;
		printf("gyro_alignment_0: %f\n", loaddata.gyro_alignment_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_1", &tmp))
	{
		loaddata.gyro_alignment_parser[1]=(float)tmp;
		printf("gyro_alignment_1: %f\n", loaddata.gyro_alignment_parser[1]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_2", &tmp))
	{
		loaddata.gyro_alignment_parser[2]=(float)tmp;
		printf("gyro_alignment_2: %f\n", loaddata.gyro_alignment_parser[2]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_3", &tmp))
	{
		loaddata.gyro_alignment_parser[3]=(float)tmp;
		printf("gyro_alignment_3: %f\n", loaddata.gyro_alignment_parser[3]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_4", &tmp))
	{
		loaddata.gyro_alignment_parser[4]=(float)tmp;
		printf("gyro_alignment_4: %f\n", loaddata.gyro_alignment_parser[4]);
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	if(config_lookup_float(&cfg, "gyro_alignment_5", &tmp))
	{
		loaddata.gyro_alignment_parser[5]=(float)tmp;
		printf("gyro_alignment_5: %f\n", loaddata.gyro_alignment_parser[5]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_6", &tmp))
	{
		loaddata.gyro_alignment_parser[6]=(float)tmp;
		printf("gyro_alignment_6: %f\n", loaddata.gyro_alignment_parser[6]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_7", &tmp))
	{
		loaddata.gyro_alignment_parser[7]=(float)tmp;
		printf("gyro_alignment_7: %f\n", loaddata.gyro_alignment_parser[7]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gyro_alignment_8", &tmp))
	{
		loaddata.gyro_alignment_parser[8]=(float)tmp;
		printf("gyro_alignment_8: %f\n", loaddata.gyro_alignment_parser[8]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	
	if(config_lookup_float(&cfg, "acc_bias_parser_0", &tmp))
	{
		loaddata.acc_bias_parser[0]=(float)tmp;
		printf("acc_bias_parser_0: %f\n", loaddata.acc_bias_parser[0]);
	}else
	{
			printf("error parser");
			errorparser=-1;
	}		
	if(config_lookup_float(&cfg, "acc_bias_parser_1", &tmp))
 	{
		loaddata.acc_bias_parser[1]=(float)tmp;
		printf("acc_bias_parser_1: %f\n", loaddata.acc_bias_parser[1]);
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	 
	if(config_lookup_float(&cfg, "acc_bias_parser_2", &tmp))
	{
		loaddata.acc_bias_parser[2]=(float)tmp;
		printf("acc_bias_parser_2: %f\n", loaddata.acc_bias_parser[2]);
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	
	if(config_lookup_float(&cfg, "mag_bias_parser_0", &tmp))
	{
		loaddata.mag_bias_parser[0]=(float)tmp;
		printf("mag_bias_parser_0: %f\n", loaddata.mag_bias_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_bias_parser_1", &tmp))
	{
		loaddata.mag_bias_parser[1]=(float)tmp;
		printf("mag_bias_parser_1: %f\n", loaddata.mag_bias_parser[1]);  
	} else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "mag_bias_parser_2", &tmp))
	{
		loaddata.mag_bias_parser[2]=(float)tmp;
		printf("mag_bias_parser_2: %f\n", loaddata.mag_bias_parser[2]);  
	} else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	
	if(config_lookup_float(&cfg, "gyro_bias_parser_0", &tmp))
	{
		loaddata.gyro_bias_parser[0]=(float)tmp;	
		printf("gyro_bias_parser_0: %f\n", loaddata.gyro_bias_parser[0]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}		
	if(config_lookup_float(&cfg, "gyro_bias_parser_1", &tmp))
	{
		loaddata.gyro_bias_parser[1]=(float)tmp;	
		printf("gyro_bias_parser_1: %f\n", loaddata.gyro_bias_parser[1]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	if(config_lookup_float(&cfg, "gyro_bias_parser_2", &tmp))
	{
		loaddata.gyro_bias_parser[2]=(float)tmp;	
		printf("gyro_bias_parser_2: %f\n", loaddata.gyro_bias_parser[2]); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	
	if(config_lookup_float(&cfg, "acc_covariance_parser", &tmp))
	{
		loaddata.acc_covariance_parser=(float)tmp;	
		printf("acc_covariance_parser: %f\n", loaddata.acc_covariance_parser); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
 
	
	if(config_lookup_float(&cfg, "mag_covariance_parser", &tmp))
	{
		loaddata.mag_covariance_parser=(float)tmp;	
		printf("mag_covariance_parser: %f\n", loaddata.mag_covariance_parser); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	
	if(config_lookup_float(&cfg, "quat_init_parser_0", &tmp))
	{
		loaddata.quat_init_parser[0]=(float)tmp;
		printf("quat_init_parser_0: %f\n", loaddata.quat_init_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "quat_init_parser_1", &tmp))
	{
		loaddata.quat_init_parser[1]=(float)tmp;
		printf("quat_init_parser_1: %f\n", loaddata.quat_init_parser[1]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	if(config_lookup_float(&cfg, "quat_init_parser_2", &tmp))
	{
		loaddata.quat_init_parser[2]=(float)tmp;
		printf("quat_init_parser_2: %f\n", loaddata.quat_init_parser[2]);  
	}else	
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "quat_init_parser_3", &tmp))
	{
		loaddata.quat_init_parser[3]=(float)tmp;
		printf("quat_init_parser_3: %f\n", loaddata.quat_init_parser[3]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	if(config_lookup_float(&cfg, "q_bias_parser_0", &tmp))
	{
		loaddata.q_bias_parser[0]=(float)tmp;
		printf("q_bias_parser_0: %f\n", loaddata.q_bias_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "q_bias_parser_1", &tmp))
	{
		loaddata.q_bias_parser[1]=(float)tmp;
		printf("q_bias_parser_1: %f\n", loaddata.q_bias_parser[1]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "q_bias_parser_2", &tmp))
	{
		loaddata.q_bias_parser[2]=(float)tmp;
		printf("q_bias_parser_2: %f\n", loaddata.q_bias_parser[2]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	
	if(config_lookup_float(&cfg, "gain_parser_0", &tmp))
	{
		loaddata.gain_parser[0]=(float)tmp;
		printf("gain_parser_0: %f\n", loaddata.gain_parser[0]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "gain_parser_1", &tmp))
	{
		loaddata.gain_parser[1]=(float)tmp;
		printf("gain_parser_1: %f\n", loaddata.gain_parser[1]);  
	}
	else
	{
			printf("error parser");
			errorparser=-1;
	}		
	if(config_lookup_float(&cfg, "gain_parser_2", &tmp))
	{
		loaddata.gain_parser[2]=(float)tmp;
		printf("gain_parser_2: %f\n", loaddata.gain_parser[2]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	if(config_lookup_float(&cfg, "gain_parser_3", &tmp))
	{
		loaddata.gain_parser[3]=(float)tmp;
		printf("gain_parser_3: %f\n", loaddata.gain_parser[3]);  
	}else
	{
			printf("error parser");
			errorparser=-1;
	}	
	
	if(config_lookup_float(&cfg, "filter_type_parser", &tmp))
	{
		loaddata.filter_type_parser=(float)tmp;	
		printf("filter_type_parser: %f\n", loaddata.filter_type_parser); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	if(config_lookup_float(&cfg, "convolution_time_parser", &tmp))
	{
		loaddata.convolution_time_parser=(float)tmp;	
		printf("convolution_time_parser: %f\n", loaddata.convolution_time_parser); 
	}else
	{
			printf("error parser");
			errorparser=-1;
	}
	
	if(config_lookup_int(&cfg, "output_enable_parser", &loaddata.output_enable_parser))
    printf("output_enable_parser: %d\n",(int)  loaddata.output_enable_parser);
	
	if(config_lookup_int(&cfg, "output_rate_parser", &loaddata.output_rate_parser))
    printf("output_rate_parser: %d\n",(int)  loaddata.output_rate_parser);
	
	if(config_lookup_int(&cfg, "badurate_parser", &loaddata.badurate_parser))
    printf("badurate_parser: %d\n",(int)  loaddata.badurate_parser);
	
	if(config_lookup_int(&cfg, "port_parser", &loaddata.port_parser))
    printf("port_parser: %d\n",(int)  loaddata.port_parser);
	
	
	//if(config_lookup_string(&cfg, "ip_address_parser", &loaddata.ip_address_parser))
    //printf("ip_address_parser: %s\n", loaddata.ip_address_parser);
	
	if(errorparser==-1)
	{
		loaddata.filter_type_parser=-1;
	}
	
	/*	
  if(config_lookup_string(&cfg, "ip_dest_addrs", &loaddata.ip_dest_addrs))
    printf("ip dest name: %s\n", loaddata.ip_dest_addrs);
	  if(config_lookup_int(&cfg, "internal_port", &loaddata.internal_port))
    printf("internal_port: %d\n",(int)  loaddata.internal_port);
	
	*/

	return loaddata;
}

	/***************write configuration file *********************/ 
//-----------------------------------------------------------------
/*!
\brief This routine write the configuration in to the file
\parm   load_data_struct writedata  : configuration to write
\return success  	
*/	
 int write_configuration(load_data_struct writedata)
{
	config_t cfg;
	double tmp;

	config_init(&cfg);
  /* Read the file. If there is an error, report it and exit. */
  if(!config_read_file(&cfg, "/home/config.cfg"))
  {
    printf("error configuration file \n");
    config_destroy(&cfg);
    return -1;
  }
  config_setting_t *setting;


  	tmp=(double)writedata.acc_alignment_parser[0];
	setting= config_lookup (&cfg, "acc_alignment_0");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_0: %f\n", writedata.acc_alignment_parser[0]);  
	
	
	tmp=(double)writedata.acc_alignment_parser[1];
	setting= config_lookup (&cfg, "acc_alignment_1");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_1: %f\n", writedata.acc_alignment_parser[1]); 
	
	tmp=(double)writedata.acc_alignment_parser[2];
	setting= config_lookup (&cfg, "acc_alignment_2");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_2: %f\n", writedata.acc_alignment_parser[2]); 
	
	
	tmp=(double)writedata.acc_alignment_parser[3];
	setting= config_lookup (&cfg, "acc_alignment_3");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_3: %f\n", writedata.acc_alignment_parser[3]); 
	
	
	tmp=(double)writedata.acc_alignment_parser[4];
	setting= config_lookup (&cfg, "acc_alignment_4");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_4: %f\n", writedata.acc_alignment_parser[4]);
	
	
	tmp=(double)writedata.acc_alignment_parser[5];
	setting= config_lookup (&cfg, "acc_alignment_5");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_5: %f\n", writedata.acc_alignment_parser[5]); 
	
	
	tmp=(double)writedata.acc_alignment_parser[6];
	setting= config_lookup (&cfg, "acc_alignment_6");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_6: %f\n", writedata.acc_alignment_parser[6]); 
	
	tmp=(double)writedata.acc_alignment_parser[7];
	setting= config_lookup (&cfg, "acc_alignment_7");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_7: %f\n", writedata.acc_alignment_parser[7]); 
	
	
	tmp=(double)writedata.acc_alignment_parser[8];
	setting= config_lookup (&cfg, "acc_alignment_8");
	config_setting_set_float(setting,tmp);
	printf("acc_alignment_8: %f\n", writedata.acc_alignment_parser[8]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[0];
	setting= config_lookup (&cfg, "mag_alignment_0");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_0: %f\n", writedata.mag_alignment_parser[0]);  
	
	
	tmp=(double)writedata.mag_alignment_parser[1];
	setting= config_lookup (&cfg, "mag_alignment_1");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_1: %f\n", writedata.mag_alignment_parser[1]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[2];
	setting= config_lookup (&cfg, "mag_alignment_2");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_2: %f\n", writedata.mag_alignment_parser[2]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[3];
	setting= config_lookup (&cfg, "mag_alignment_3");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_3: %f\n", writedata.mag_alignment_parser[3]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[4];
	setting= config_lookup (&cfg, "mag_alignment_4");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_4: %f\n", writedata.mag_alignment_parser[4]);
	
	
	tmp=(double)writedata.mag_alignment_parser[5];
	setting= config_lookup (&cfg, "mag_alignment_5");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_5: %f\n", writedata.mag_alignment_parser[5]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[6];
	setting= config_lookup (&cfg, "mag_alignment_6");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_6: %f\n", writedata.mag_alignment_parser[6]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[7];
	setting= config_lookup (&cfg, "mag_alignment_7");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_7: %f\n", writedata.mag_alignment_parser[7]); 
	
	
	tmp=(double)writedata.mag_alignment_parser[8];
	setting= config_lookup (&cfg, "mag_alignment_8");
    config_setting_set_float(setting,tmp);
	printf("mag_alignment_8: %f\n", writedata.mag_alignment_parser[8]); 
	
	
	
	tmp=(double)writedata.gyro_alignment_parser[0];
	setting= config_lookup (&cfg, "gyro_alignment_0");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_0: %f\n", writedata.gyro_alignment_parser[0]);  
	
	
	tmp=(double)writedata.gyro_alignment_parser[1];
	setting= config_lookup (&cfg, "gyro_alignment_1");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_1: %f\n", writedata.gyro_alignment_parser[1]); 
	
	
	tmp=(double)writedata.gyro_alignment_parser[2];
	setting= config_lookup (&cfg, "gyro_alignment_2");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_2: %f\n", writedata.gyro_alignment_parser[2]); 
	
	
	tmp=(double)writedata.gyro_alignment_parser[3];
	setting= config_lookup (&cfg, "gyro_alignment_3");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_3: %f\n", writedata.gyro_alignment_parser[3]); 
	
	tmp=(double)writedata.gyro_alignment_parser[4];
	setting= config_lookup (&cfg, "gyro_alignment_4");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_4: %f\n", writedata.gyro_alignment_parser[4]);
	
	
	tmp=(double)writedata.gyro_alignment_parser[5];	
	setting= config_lookup (&cfg, "gyro_alignment_5");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_5: %f\n", writedata.gyro_alignment_parser[5]); 
	
	
	tmp=(double)writedata.gyro_alignment_parser[6];
	setting= config_lookup (&cfg, "gyro_alignment_6");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_6: %f\n", writedata.gyro_alignment_parser[6]); 
	
	
	tmp=(double)writedata.gyro_alignment_parser[7];
	setting= config_lookup (&cfg, "gyro_alignment_7");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_7: %f\n", writedata.gyro_alignment_parser[7]); 
	
	
	tmp=(double)writedata.gyro_alignment_parser[8];
	setting= config_lookup (&cfg, "gyro_alignment_8");
    config_setting_set_float(setting,tmp);
	printf("gyro_alignment_8: %f\n", writedata.gyro_alignment_parser[8]); 
	
	
	tmp=(double)writedata.acc_bias_parser[0];
	setting= config_lookup (&cfg, "acc_bias_parser_0");
    config_setting_set_float(setting,tmp);
	printf("acc_bias_parser_0: %f\n", writedata.acc_bias_parser[0]);
	
	tmp=(double)writedata.acc_bias_parser[1];
	setting= config_lookup (&cfg, "acc_bias_parser_1");
    config_setting_set_float(setting,tmp);	
	printf("acc_bias_parser_1: %f\n", writedata.acc_bias_parser[1]);
		
	tmp=(double)writedata.acc_bias_parser[2];
	setting= config_lookup (&cfg, "acc_bias_parser_2");
    config_setting_set_float(setting,tmp);	
	printf("acc_bias_parser_2: %f\n", writedata.acc_bias_parser[2]);
	
	
	tmp=(double)writedata.mag_bias_parser[0];
	setting= config_lookup (&cfg, "mag_bias_parser_0");
    config_setting_set_float(setting,tmp);		
	printf("mag_bias_parser_0: %f\n", writedata.mag_bias_parser[0]);  
	
	
	tmp=(double)writedata.mag_bias_parser[1];
	setting= config_lookup (&cfg, "mag_bias_parser_1");
    config_setting_set_float(setting,tmp);	
	printf("mag_bias_parser_1: %f\n", writedata.mag_bias_parser[1]);  
	 
	tmp=(double)writedata.mag_bias_parser[2];
	setting= config_lookup (&cfg, "mag_bias_parser_2");
    config_setting_set_float(setting,tmp);	
	printf("mag_bias_parser_2: %f\n", writedata.mag_bias_parser[2]);  
	 
	
	tmp=(double)writedata.gyro_bias_parser[0];
	setting= config_lookup (&cfg, "gyro_bias_parser_0");
    config_setting_set_float(setting,tmp);
	printf("gyro_bias_parser_0: %f\n", writedata.gyro_bias_parser[0]); 
	
	tmp=(double)writedata.gyro_bias_parser[1];	
	setting= config_lookup (&cfg, "gyro_bias_parser_1");
    config_setting_set_float(setting,tmp);
	printf("gyro_bias_parser_1: %f\n", writedata.gyro_bias_parser[1]); 
	
	tmp=(double)writedata.gyro_bias_parser[2];
	setting= config_lookup (&cfg, "gyro_bias_parser_2");
    config_setting_set_float(setting,tmp);	
	printf("gyro_bias_parser_2: %f\n", writedata.gyro_bias_parser[2]); 
		
	
	tmp=(double)writedata.acc_covariance_parser;	
	setting= config_lookup (&cfg, "acc_covariance_parser");
    config_setting_set_float(setting,tmp);
	printf("acc_covariance_parser: %f\n", writedata.acc_covariance_parser); 
		
 
	tmp=(double)writedata.mag_covariance_parser;
	setting= config_lookup (&cfg, "mag_covariance_parser");
    config_setting_set_float(setting,tmp);	
	printf("mag_covariance_parser: %f\n", writedata.mag_covariance_parser); 
	
	
	tmp=(double)writedata.quat_init_parser[0];
	setting= config_lookup (&cfg, "quat_init_parser_0");
    config_setting_set_float(setting,tmp);
	printf("quat_init_parser_0: %f\n", writedata.quat_init_parser[0]);  
	
	tmp=(double)writedata.quat_init_parser[1];
	setting= config_lookup (&cfg, "quat_init_parser_1");
    config_setting_set_float(setting,tmp);
	printf("quat_init_parser_1: %f\n", writedata.quat_init_parser[1]);  
	
	tmp=(double)writedata.quat_init_parser[2];
	setting= config_lookup (&cfg, "quat_init_parser_2");
    config_setting_set_float(setting,tmp);	
	printf("quat_init_parser_2: %f\n", writedata.quat_init_parser[2]);  
	
	tmp=(double)writedata.quat_init_parser[3];
	setting= config_lookup (&cfg, "quat_init_parser_3");
    config_setting_set_float(setting,tmp);	
	printf("quat_init_parser_3: %f\n", writedata.quat_init_parser[3]);  
	
	
	tmp=(double)writedata.q_bias_parser[0];
	setting= config_lookup (&cfg, "q_bias_parser_0");
    config_setting_set_float(setting,tmp);
	printf("q_bias_parser_0: %f\n", writedata.q_bias_parser[0]);  
	
	tmp=(double)writedata.q_bias_parser[1];
	setting= config_lookup (&cfg, "q_bias_parser_1");
    config_setting_set_float(setting,tmp);
	printf("q_bias_parser_1: %f\n", writedata.q_bias_parser[1]);  
	
	tmp=(double)writedata.q_bias_parser[2];
	setting= config_lookup (&cfg, "q_bias_parser_2");
    config_setting_set_float(setting,tmp);
	printf("q_bias_parser_2: %f\n", writedata.q_bias_parser[2]);  
	
	
	tmp=(double)writedata.gain_parser[0];
	setting= config_lookup (&cfg, "gain_parser_0");
    config_setting_set_float(setting,tmp);
	printf("gain_parser_0: %f\n", writedata.gain_parser[0]);  
	
	tmp=(double)writedata.gain_parser[1];
	setting= config_lookup (&cfg, "gain_parser_1");
    config_setting_set_float(setting,tmp);
	printf("gain_parser_1: %f\n", writedata.gain_parser[1]);  
	
	tmp=(double)writedata.gain_parser[2];
	setting= config_lookup (&cfg, "gain_parser_2");
    config_setting_set_float(setting,tmp);	
	printf("gain_parser_2: %f\n", writedata.gain_parser[2]);  
	
	tmp=(double)writedata.gain_parser[3];
	setting= config_lookup (&cfg, "gain_parser_3");
    config_setting_set_float(setting,tmp);	
	printf("gain_parser_3: %f\n", writedata.gain_parser[3]);  
		
	
	tmp=(double)writedata.filter_type_parser;
	setting= config_lookup (&cfg, "filter_type_parser");
    config_setting_set_float(setting,tmp);
	printf("filter_type_parser: %f\n", writedata.filter_type_parser); 
	
	tmp=(double)writedata.convolution_time_parser;
	setting= config_lookup (&cfg, "convolution_time_parser");
    config_setting_set_float(setting,tmp);
	printf("convolution_time_parser: %f\n", writedata.convolution_time_parser); 
	
	setting= config_lookup (&cfg, "output_enable_parser");
    config_setting_set_int(setting,(int)  writedata.output_enable_parser);
	printf("output_enable_parser: %d\n",(int)  writedata.output_enable_parser);
	
	setting= config_lookup (&cfg, "output_rate_parser");
    config_setting_set_int(setting,(int)  writedata.output_rate_parser);
	printf("output_rate_parser: %d\n",(int)   writedata.output_rate_parser);
	
	setting= config_lookup (&cfg, "badurate_parser");
    config_setting_set_int(setting,(int)  writedata.badurate_parser);
	printf("badurate_parser: %d\n",(int)   writedata.badurate_parser);

	setting= config_lookup (&cfg, "port_parser");
    config_setting_set_int(setting,(int)  writedata.port_parser);
	printf("port_parser: %d\n",(int)   writedata.port_parser);	

	/*
	setting= config_lookup (&cfg, "ip_address_parser");
    config_setting_set_string (setting,  writedata.ip_address_parser);
	printf("ip_address_parser: %s\n", writedata.ip_address_parser);
	*/
	
  
  if(!config_write_file    (&cfg, "config.cfg"))
  {
    printf("error configuration file \n");
    config_destroy(&cfg);
    return -1;
  }
  //config_destroy(&cfg);
  printf("end write to flash \n");
  return 1;
}
//-----------------------------------------------------------------
/*!
\brief This routine restore the base configuration file

\return success  	
*/	
int make_file(void)
{
	/*FILE *fp;
	char stringtowrite[]= "acc_alignment_0 = 1.0; \r\nacc_alignment_1 = 0.0; \r\nacc_alignment_2 = 0.0; \r\nacc_alignment_3 = 0.0; \r\nacc_alignment_4 = 1.0; \r\nacc_alignment_5 = 0.0; \r\nacc_alignment_6 = 0.0; \r\nacc_alignment_7 = 0.0; \r\nacc_alignment_8 = 1.0; \r\nmag_alignment_0 = 1.0; \r\nmag_alignment_1 = 0.0; \r\nmag_alignment_2 = 0.0; \r\nmag_alignment_3 = 0.0; \r\nmag_alignment_4 = 1.0; \r\nmag_alignment_5 = 0.0; \r\nmag_alignment_6 = 0.0; \r\nmag_alignment_7 = 0.0; \r\nmag_alignment_8 = 1.0; \r\ngyro_alignment_0 = 1.0; \r\ngyro_alignment_1 = 0.0; \r\ngyro_alignment_2 = 0.0; \r\ngyro_alignment_3 = 0.0; \r\ngyro_alignment_4 = 1.0; \r\ngyro_alignment_5 = 0.0; \r\ngyro_alignment_6 = 0.0; \r\ngyro_alignment_7 = 0.0; \r\ngyro_alignment_8 = 1.0; \r\nacc_bias_parser_0 = 0.0; \r\nacc_bias_parser_1 = 0.0; \r\nacc_bias_parser_2 = 0.0; \r\nmag_bias_parser_0 = 0.0; \r\nmag_bias_parser_1 = 0.0; \r\nmag_bias_parser_2 = 0.0; \r\ngyro_bias_parser_0 = 0.0; \r\ngyro_bias_parser_1 = 0.0; \r\ngyro_bias_parser_2 = 0.0; \r\nacc_covariance_parser = 0.0010; \r\nmag_covariance_parser = 0.00899; \r\nquat_init_parser_0 = 0.009999999776; \r\nquat_init_parser_1 = 0.009999999776; \r\nquat_init_parser_2 = 0.9900000095; \r\nquat_init_parser_3 = 0.001000000047; \r\nq_bias_parser_0 = 0.001000000047; \r\nq_bias_parser_1 = 0.001000000047; \r\nq_bias_parser_2 = 0.001000000047; \r\ngain_parser_0 = 0.0003000000142; \r\ngain_parser_1 = 0.0; \r\ngain_parser_2 = 0.0; \r\ngain_parser_3 = 0.0; \r\nfilter_type_parser = 0.0; \r\nconvolution_time_parser = 0.0; \r\noutput_enable_parser = 0; \r\noutput_rate_parser = 100; \r\nbadurate_parser = 1; \r\nport_parser = 1200;";
	
	printf("file config ren. \n");
	system ("rm config.cfg" );
	fp=fopen("config.cfg", "wb");
	//fwrite (fd,stringtowrite, sizeof(data_basic_file));
	fwrite(stringtowrite,sizeof(stringtowrite),1,fp);
	fclose(fp);*/
	system ("rm /home/config.cfg" );
	system ("cp /home/config.cfg.bk /home/config.cfg" );
	system ("sync" );
	sleep(1);
	return 1;
}

