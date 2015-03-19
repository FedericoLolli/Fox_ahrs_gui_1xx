#include <stdint.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <getopt.h>
#include <fcntl.h>
#include <sys/ioctl.h>
#include <linux/types.h>
#include <linux/spi/spidev.h>
#include <linux/i2c-dev.h>
#include <math.h>
#include "ahrsacquire.h"

//Magnetometro HMC883L
#define HMC5883L_ADDR 		0x1E
#define HMC5883L_REGA		0x00
#define HMC5883L_REGB		0x01
#define HMC5883L_REG_MODE	0x02
#define HMC5883L_MAGX_H		0x03
#define HMC5883L_MAGX_L		0x04
#define HMC5883L_MAGZ_H		0x05
#define HMC5883L_MAGZ_L		0x06
#define HMC5883L_MAGY_H		0x07
#define HMC5883L_MAGY_L		0x08
//settaggi
#define HMC5883L_REGA_CONF 		0x78  //data output continuos 75hz media 8 valori

#define HMC5883L_REGA_CONFB 		0x1C  //data output continuos 220hz

#define HMC5883L_REGB_CONF_1_3GA 		0x20  //data output 1.3Ga
#define HMC5883L_REGB_CONF_1_9GA 		0x40  //data output 1.9Ga
#define HMC5883L_REGB_CONF_2_5GA 		0x60  //data output 2.5Ga

#define HMC5883L_REG_MODE_CONF 		0x00  //data output continuos measurement mode

//sensibilità
#define sensivity_LSB_Gauss_1_3		1090.0F
#define sensivity_LSB_Gauss_1_9		820.0F
#define sensivity_LSB_Gauss_2_5 	660.0F

#define sensivity_acc		0.25F
#define sensivity_gyro_deg_250	0.01F
#define sensivity_gyro_rad_250	0.1F
#define deg_to_rad 0.0174532925F


#define ARRAY_SIZE(a) (sizeof(a) / sizeof((a)[0]))

//int fdgpio;
//sprintf(buf, "/sys/class/gpio/pioB14/value");
//system("echo 46 > /sys/class/gpio/export");
//system("echo \"in\" > /sys/class/gpio/pioB14/direction");
//fdgpio = open(buf, O_RDONLY);	
//read(fdgpio, &value, 1);
/*
if(value == '0')
{ 
     printf("low \n");
	//qua dovrei acquisire
}
else
{
     //printf("HI \n");
}
*/
#define MAG_TIME 3
#define BARO_TIME 50


//data acquisiti
static int16_t accx, accy,accz, gyrox,gyroy, gyroz, temperature, diag_stat;
static int16_t magx,magy,magz;
static int fdspi,fdi2c;
static int magtimeout;
static int baro_tymeout;
static float acc[3],mag[3],gyro[3],temp,mag_stored[3],altitude;
static float delta_mag[3];
float hz=819.0;


//BMP085
#define BMP085_ADDRESS 0x77  // I2C address of BMP180

//pressure sensor global variable
const unsigned char OSS = 0x03;  // Oversampling Setting
static int16_t ac1;
static int16_t ac2; 
static int16_t ac3; 
static uint16_t ac4;
static uint16_t ac5;
static uint16_t ac6;
static int16_t b1; 
static int16_t b2;
static int16_t mb;
static int16_t mc;
static int16_t md;
static long b5; 
static int16_t temp_u;
static unsigned long press_u;
static int azione;

static void pabort(const char *s)
{
	perror(s);
	abort();
}

//spi
static const char *device = "/dev/spidev32766.0";
static uint32_t mode;
static uint8_t bits = 8;		//8 bit 		
static uint32_t speed = 1000000;   //velocit'a massima spi 1mhz
static uint16_t delay=00;

//i2c
static const char *devicei2c = "/dev/i2c-0";



int openi2c(void)
{
	int fd;
	char filename[20];
	//unsigned int speedic = 1;
	sprintf(filename, devicei2c);
	fd = open(filename, O_RDWR| O_NDELAY | O_NONBLOCK );
	if (fd < 0) {
		printf("Error on open i2c \n");
		exit(1);
	}
	//ret = ioctl(fd, I2C_IOC_WR_MAX_SPEED_HZ, &speedic);
	 ioctl(fd, I2C_TIMEOUT , 1);
	 ioctl(fd, I2C_RETRIES, 0);
	return fd;	
}



int setdevicei2c(int fd, char address)
{
	if (ioctl(fd, I2C_SLAVE, address) < 0) {
		printf("Error i2c on slave address\n");
		return -1;
		
	}
	return 0;
}

int  writebytei2c(int fd,char reg, char data)
{
	char buf[2];
	buf[0] = reg;
	buf[1] = data;
	if ((write(fd,buf,2))!=2) {
		printf("Error i2c send the read command\n");
		return -1;
	}
	return 0;
}

char readbytei2c(int fd, char reg)
{
	char buf[1];
	buf[0] = reg;
	if ((write(fd,buf,1))!=1) {
		printf("Error i2c on select the High Byte\n");
		//exit(1);
	}
 
	if ((read(fd,buf,1))!=1) {
		printf("Error i2c on read the  High Byte\n");
		//exit(1);
	}
	return buf[0];
}


int readi2csixb(int fd, char reg, char *bufoutt)
{
	char buf[1];
	buf[0] = reg;
	if ((write(fd,buf,1))!=1) {
		//printf("Error i2c on select the High Byte\n");
		//exit(1);
		return -1;
	}
 
	if ((read(fd,bufoutt,6))!=6) {
		//printf("Error i2c on read the  High Byte\n");
		//exit(1);
		return -1;
	}
	return 1;
}




void set_baro_initialize(int fd)
{
	int16_t msb , lsb;
	setdevicei2c(fd, BMP085_ADDRESS);

	msb=readbytei2c(fd, 0xAA);
	lsb=readbytei2c(fd, 0xAB);
	ac1=((int16_t)((msb << 8) + lsb));

	msb=readbytei2c(fd, 0xAC);
	lsb=readbytei2c(fd, 0xAD);
	ac2=((int16_t)((msb << 8) + lsb));
	
	msb=readbytei2c(fd, 0xAE);
	lsb=readbytei2c(fd, 0xAF);
	ac3=((int16_t)((msb << 8) + lsb));
	
	msb=readbytei2c(fd, 0xB0);
	lsb=readbytei2c(fd, 0xB1);
	ac4=((int16_t)((msb << 8) + lsb));

	msb=readbytei2c(fd, 0xB2);
	lsb=readbytei2c(fd, 0xB3);
	ac5=((int16_t)((msb << 8) + lsb));
	
	msb=readbytei2c(fd, 0xB4);
	lsb=readbytei2c(fd, 0xB5);
	ac6=((int16_t)((msb << 8) + lsb));


	msb=readbytei2c(fd, 0xB6);
	lsb=readbytei2c(fd, 0xB7);
	b1=((int16_t)((msb << 8) + lsb));
	
	msb=readbytei2c(fd, 0xB8);
	lsb=readbytei2c(fd, 0xB9);
	b2=((int16_t)((msb << 8) + lsb));
	
	msb=readbytei2c(fd, 0xBA);
	lsb=readbytei2c(fd, 0xBB);
	mb=((int16_t)((msb << 8) + lsb));	

	msb=readbytei2c(fd, 0xBC);
	lsb=readbytei2c(fd, 0xBD);
	mc=((int16_t)((msb << 8) + lsb));
	
	msb=readbytei2c(fd, 0xBE);
	lsb=readbytei2c(fd, 0xBF);
	md=((int16_t)((msb << 8) + lsb));


	printf("ac1 %d ac2 %d , ac3 %d , ac4 %d , ac5 %d ,ac6 %d ,b1 %d , b2 %d , mb %d ,mc %d , md %d , b5 %d",ac1,ac2,ac3,ac4,ac5,ac6,b1,b2,mb,mc,md,b5);

}


float readaltitude_press(int fd ,int azione)
{
	short temperature;
	long x1, x2, x3, b3, b6, p;
	unsigned long b4, b7;
	long x1p, x2p;
	float pressione;
	unsigned char buf;
	int16_t msb , lsb;
	if(azione==0)
	{
		//printf("azione 0 \n");
		setdevicei2c(fd, BMP085_ADDRESS);
		//richiedo aggiornamento temperature
		writebytei2c(fd,0xF4,0x2E);
		return -10000;
	}
	//leggo temperatura
	if(azione==1)
	{

		setdevicei2c(fd, BMP085_ADDRESS);
		msb=readbytei2c(fd, 0xF6);
		lsb=readbytei2c(fd, 0xF7);
		temp_u=((int16_t)((msb << 8) + lsb));
		//printf("azione 1 leggo temp  %d \n",temp_u );
		return -10000;
	}

	//richiedo aggiornamento pressione
	if(azione==2)
	{
		//printf("azione 2 \n");
		setdevicei2c(fd, BMP085_ADDRESS);
		writebytei2c(fd,0xF4,0x34 + (OSS<<6));
		return -10000;
	}
	if(azione==3)
	{

		//printf("ac1 %d ac2 %d , ac3 %d , ac4 %d , ac5 %d ,ac6 %d ,b1 %d , b2 %d , mb %d ,mc %d , md %d , b5 %d",ac1,ac2,ac3,ac4,ac5,ac6,b1,b2,mb,mc,md,b5);
		setdevicei2c(fd, BMP085_ADDRESS);
		press_u=0x00;
		buf=readbytei2c(fd, 0xF6);
		press_u = buf<<16;
		buf=readbytei2c(fd, 0xF7);
		press_u|= buf<<8;
		buf=readbytei2c(fd, 0xF8);
		press_u |= buf;
		press_u = press_u >> (8-OSS);
		//printf("azione 3 leggo press  %ld \n",press_u );
		x1p = (((long)temp_u - (long)ac6)*(long)ac5) >> 15;
		x2p = ((long)mc << 11)/(x1p + md);
		b5 = x1p + x2p;
		temperature= ((b5 + 8)>>4);
		//printf("1 temperature %d \n",temperature);

		b6 = b5 - 4000;
		// Calculate B3
		x1 = (b2 * (b6 * b6)>>12)>>11;
		x2 = (ac2 * b6)>>11;
		x3 = x1 + x2;
		b3 = (((((long)ac1)*4 + x3)<<OSS) + 2)>>2;
		// Calculate B4
		x1 = (ac3 * b6)>>13;
		x2 = (b1 * ((b6 * b6)>>12))>>16;
		x3 = ((x1 + x2) + 2)>>2;
		b4 = (ac4 * (unsigned long)(x3 + 32768))>>15;

		b7 = ((unsigned long)(press_u - b3) * (50000>>OSS));
		if (b7 < 0x80000000)
		p = (b7<<1)/b4;
		else
		p = (b7/b4)<<1;
		x1 = (p>>8) * (p>>8);
		x1 = (x1 * 3038)>>16;
		x2 = (-7357 * p)>>16;
		//printf("1\n");
		p += (x1 + x2 + 3791)>>4;
		pressione =((float)p)/100;
		//printf("1 %f  altitudine%f\n",pressione, (float)(44330*(1-pow((pressione/1013.25),(1/5.255)))));
		return (float)(44330*(1-pow((pressione/1013.25),(1/5.255))));
	}

}



void set_mag_initialize(int fd, int sens)
{
	magtimeout=10;
	setdevicei2c(fd, HMC5883L_ADDR );
	writebytei2c(fd,HMC5883L_REGA,HMC5883L_REGA_CONFB);
	if(sens==1)
	{
		writebytei2c(fd,HMC5883L_REGB,HMC5883L_REGB_CONF_1_3GA);
	}
	if(sens==2)
	{
		writebytei2c(fd,HMC5883L_REGB,HMC5883L_REGB_CONF_1_9GA);
	}
	if(sens==3)
	{
		writebytei2c(fd,HMC5883L_REGB,HMC5883L_REGB_CONF_2_5GA);
	}
	writebytei2c(fd,HMC5883L_REG_MODE,HMC5883L_REG_MODE_CONF);
}

int read_hmc883l(int fd){

	int16_t msb , lsb;
	char bufout[10];

	setdevicei2c(fd, HMC5883L_ADDR  );
	
	if( readi2csixb(fd, HMC5883L_MAGX_H,  bufout)==1)
	{
		msb=0x0000;
		lsb=0x0000;
		msb=(int16_t)bufout[0];
		lsb=(int16_t)bufout[1];
		magx=((int16_t)((msb << 8) + lsb));
		msb=0x0000;
		lsb=0x0000;
		msb=(int16_t)bufout[2];
		lsb=(int16_t)bufout[3];
		magz=((int16_t)((msb << 8) + lsb));
		msb=0x0000;
		lsb=0x0000;
		msb=(int16_t)bufout[4];
		lsb=(int16_t)bufout[5];
		magy=((int16_t)((msb << 8) + lsb));

		mag[0]=((float)magx)/sensivity_LSB_Gauss_1_9;
		mag[1]=((float)magy)/sensivity_LSB_Gauss_1_9;
		mag[2]=((float)magz)/sensivity_LSB_Gauss_1_9;
		//printf(" %x %x %x %x %x %x \n ",bufout[0],bufout[1],bufout[2],bufout[3],bufout[4],bufout[5]);
	
		return 1;
	}else
	{
		printf("i2c error \n");
		//mag[0]=0.0;
		//mag[1]=0.0;
		//mag[2]=0.0;
		return -1;
	}
/*

	msb=readbytei2c(fd, HMC5883L_MAGX_H);
	lsb=readbytei2c(fd, HMC5883L_MAGX_L);
	magx=((int16_t)((msb << 8) + lsb));
	//	msb=0;
	//	lsb=0;
	msb=readbytei2c(fd, HMC5883L_MAGY_H);
	lsb=readbytei2c(fd, HMC5883L_MAGY_L);
	magy=((int16_t)((msb << 8) + lsb));
	//	msb=0;
	//	lsb=0;
	msb=readbytei2c(fd, HMC5883L_MAGZ_H );
	lsb=readbytei2c(fd, HMC5883L_MAGZ_L );
	magz=((int16_t)((msb << 8) + lsb));
	
	mag[0]=((float)magx)/sensivity_LSB_Gauss_1_9;
	mag[1]=((float)magy)/sensivity_LSB_Gauss_1_9;
	mag[2]=((float)magz)/sensivity_LSB_Gauss_1_9;
	printf("%1.3f %1.3f %1.3f \n",mag[0],mag[1],mag[2]);
	return 1;
	*/
	

}

//funzione di trasferimento e parsing ADIS16445
static void transfer(int fd)
{
	int ret;
	uint8_t tx[] = {
		0x3E,0x00 ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00 ,0x00,0x00,0x00,0x00,0x00,0x00
	};
	uint8_t rx[18] = {0x00,0x00 ,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00 ,0x00,0x00,0x00,0x00,0x00,0x00};
	struct spi_ioc_transfer tr = {
		.tx_buf = (unsigned long)tx,
		.rx_buf = (unsigned long)rx,
		.len = 18,
		.delay_usecs = delay,
		.speed_hz = speed,
		.bits_per_word = bits,
	};

	ret = ioctl(fd, SPI_IOC_MESSAGE(1), &tr);
	if(ret<0)
	{
		printf("SPI error read");
	}
	diag_stat=((int16_t)((rx[2] << 8) + rx[3]));
	gyrox=((int16_t)((rx[4] << 8) + rx[5]));
	gyroy=((int16_t)((rx[6] << 8) + rx[7]));
	gyroz=((int16_t)((rx[8] << 8) + rx[9]));
	accx=((int16_t)((rx[10] << 8) + rx[11]));
	accy=((int16_t)((rx[12] << 8) + rx[13]));
	accz=((int16_t)((rx[14] << 8) + rx[15]));
	temperature=((int16_t)((rx[16] << 8) + rx[17]));
	
	acc[0]=(((float)accx)*sensivity_acc)/1000;
	acc[1]=(((float)accy)*sensivity_acc)/1000;
	acc[2]=(((float)accz)*sensivity_acc)/1000;

	gyro[0]=(((float)gyrox)*sensivity_gyro_deg_250)*deg_to_rad;
	gyro[1]=(((float)gyroy)*sensivity_gyro_deg_250)*deg_to_rad;
	gyro[2]=(((float)gyroz)*sensivity_gyro_deg_250)*deg_to_rad;
	
	//printf("%f %f %f \n",gyro[0],gyro[1],gyro[2]);
	

}


//-----------------------------------------------------------------
/*!
\brief This routine init the device hardware and configure it
return succes code
*/
int init_device_ahrs(void)
{

	magtimeout=0;
	baro_tymeout=0;
	int ret = 0;
	
	//adis 81hz -3db configuration
	//uint8_t tx[] = {
	//	0x38,0x04 ,0x04,0x00
	//};
	
	//adis 30hz -3db configuration
	//uint8_t tx[] = {
	//	0x38,0x04 ,0x06,0x00
	//};
	//adis 200hz -3db configuration
	uint8_t tx[] = {
		0x38,0x04 ,0x02,0x00
	};
	uint8_t rx[4] = {0x00,0x00 ,0x00,0x00};
	struct spi_ioc_transfer tr = {
		.tx_buf = (unsigned long)tx,
		.rx_buf = (unsigned long)rx,
		.len = 3,
		.delay_usecs = delay,
		.speed_hz = speed,
		.bits_per_word = bits,
	};
	
	
	fdi2c =openi2c();

	//setto il modo SPI
	mode=0;
	mode |= SPI_CPHA;
	mode |= SPI_CPOL;
	fdspi = open(device, O_RDWR| O_NDELAY | O_NONBLOCK);
	if (fdspi < 0)
		pabort("can't open device");
		

	/*
	 * set spi mode
	 */

	ret = ioctl(fdspi, SPI_IOC_WR_MODE, &mode);
	if (ret == -1)
		pabort("can't set spi mode");

	ret = ioctl(fdspi, SPI_IOC_RD_MODE, &mode);
	if (ret == -1)
		pabort("can't get spi mode");

	
	 // bits per word
	
	ret = ioctl(fdspi, SPI_IOC_WR_BITS_PER_WORD, &bits);
	if (ret == -1)
		pabort("can't set bits per word");

	ret = ioctl(fdspi, SPI_IOC_RD_BITS_PER_WORD, &bits);
	if (ret == -1)
		pabort("can't get bits per word");
	
	 // max speed hz
	 
	ret = ioctl(fdspi, SPI_IOC_WR_MAX_SPEED_HZ, &speed);
	if (ret == -1)
		pabort("can't set max speed hz");

	ret = ioctl(fdspi, SPI_IOC_RD_MAX_SPEED_HZ, &speed);
	if (ret == -1)
		pabort("can't get max speed hz");
		

	printf("spi mode: 0x%x\n", mode);
	printf("bits per word: %d\n", bits);
	printf("max speed: %d Hz (%d KHz)\n", speed, speed/1000);

	//send adis configuration message
	ret = ioctl(fdspi, SPI_IOC_MESSAGE(1), &tr);
	//inizialize mag
	set_mag_initialize(fdi2c, 2);
	
	//inizializze pressure
	set_baro_initialize( fdi2c);
	
	
	return 1;
}

//-----------------------------------------------------------------
/*!
\brief This routine return the inertial data e

\return ahrs_data
*/
ahrs_data acquire_device(void)
{
	float tmp_altitude=0;
	ahrs_data ahrs_data_out;
	transfer(fdspi);
	magtimeout++;
	if(magtimeout>MAG_TIME)
	{
		mag_stored[0]=mag[0];
		mag_stored[1]=mag[1];
		mag_stored[2]=mag[2];
		read_hmc883l(fdi2c);
		//printf("%1.4f %1.4f %1.4f\n",mag[0],mag[1],mag[2]);

		delta_mag[0]=(mag[0]-mag_stored[0]);
		if(delta_mag[0]>=0.05)
			delta_mag[0]=0.05;
		if(delta_mag[0]<=-0.05)
			delta_mag[0]=-0.05;
			
		delta_mag[1]=(mag[1]-mag_stored[1]);
		if(delta_mag[1]>=0.05)
			delta_mag[1]=0.05;
		if(delta_mag[1]<=-0.05)
			delta_mag[1]=-0.05;
			
		delta_mag[2]=(mag[2]-mag_stored[2]);
		if(delta_mag[2]>=0.05)
			delta_mag[2]=0.05;
		if(delta_mag[2]<=-0.05)
			delta_mag[2]=-0.05;
		
		magtimeout=0;

	}
	else
	{
		mag[0]=mag[0]+(delta_mag[0])/hz ;
		mag[1]=mag[1]+(delta_mag[1])/hz ;
		mag[2]=mag[2]+(delta_mag[2])/hz ;
	
	}
	baro_tymeout ++;
	if(baro_tymeout>BARO_TIME)
	{
		baro_tymeout=0;
		tmp_altitude=readaltitude_press(fdi2c ,azione);
		if(tmp_altitude>=(-1000.0))
		{
			altitude=tmp_altitude;
		}
		azione ++;
		if (azione>=4)
		{
			azione =0;
		}
	}
	//printf("%1.4f %1.4f %1.4f %1.4f %1.4f %1.4f \n",acc[0],acc[1],acc[2],gyro[0],gyro[1],gyro[2]);
	ahrs_data_out.mag[0]=mag[0];
	ahrs_data_out.mag[1]=mag[1];
	ahrs_data_out.mag[2]=mag[2];
	ahrs_data_out.acc[0]=acc[0];
	ahrs_data_out.acc[1]=acc[1];
	ahrs_data_out.acc[2]=acc[2];
	ahrs_data_out.gyro[0]=gyro[0];
	ahrs_data_out.gyro[1]=gyro[1];
	ahrs_data_out.gyro[2]=gyro[2];
	ahrs_data_out.temp=temp;
	return ahrs_data_out;
}
//-----------------------------------------------------------------
/*!
\brief This routine close the divice

*/
void close_device(void)
{
	close(fdspi);
	close(fdi2c);
	
}
//-----------------------------------------------------------------
/*!
\brief This routine return the altitude from the barometer
return meters
*/
float get_altitude(void){
return  altitude;
}












