#CC=arm-linux-gnueabi-g++
#CC=arm-skytech-eabi-gcc 
#CC= arm-linux-gnueabi-gcc
CC=arm-cortexa9_neon-linux-gnueabihf-gcc
CCS=arm-cortexa9_neon-linux-gnueabihf-strip
#CC=gcc

OBJSTATIC= gps.o AHRSEKF.o nmeap01.o ahrsacquire.o seriallib.o crc.o parser.o loadconf.o geoMag.o geoEllips.o fir.o

CFLAGS=-lm -lrt -O2 -Wall 
LIBS = 
LFLAGS= -static 




all:  static ahrstest strip

%.o: %.c $(DEPS)
	$(CC) $(CFLAGS) $(LFLAGS) -c -o $@ $< 

static: $(OBJSTATIC)
	ar rcs ahrs_core.a $^

ahrstest: ahrstest.c 
	$(CC) $(CFLAGS) $(LFLAGS) $(OBJSTATIC) -lm ./libconfig.a ahrstest.c -o  ahrs
	
strip: ahrs 
	$(CCS) ahrs


clean:
	rm -f *.o
	rm -f libekf.a
	rm -f libekf.so
	rm -f ahrs
	rm -f ahrs_core.a
