/*!
\file  geoStars.h

*/

/*!


*/

#define GEOSTARSLIB_VERSION 0.9

#ifndef GEO_H
#define GEO_H

#include <time.h>

#if defined(GEO_LIB) || !defined(WIN32)
#if defined(GEO_LIB)
#pragma message( "Compiling geoStarsLib.lib - static")
#endif
#define DLL_API                  //!< DLL calling convention
#define DLL_CALLCONV             //!< DLL calling convention
#else
#pragma message( "Compiling geoStarsLib.dll - Dynamic")
#define WIN32_LEAN_AND_MEAN
#define DLL_CALLCONV __stdcall
// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the GEO_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// DLL_API functions as being imported from a DLL, wheras this DLL sees symbols
// defined with this macro as being exported.
#ifdef GEO_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif // GEO_EXPORTS
#endif // GEO_LIB || !WIN32



#define GEO_DATUM_DEFAULT 0 //!< Default datum (WGS84)
#define GEO_DATUM_AA 1   //!< Datum : Airy 1830
#define GEO_DATUM_AN 2   //!< Datum : Australian National
#define GEO_DATUM_BR 3   //!< Datum : Bessel 1841
#define GEO_DATUM_BN 4   //!< Datum : Bessel 1841 (Namibia)
#define GEO_DATUM_CC 5   //!< Datum : Clarke 1866
#define GEO_DATUM_CD 6   //!< Datum : Clarke 1880
#define GEO_DATUM_EB 7   //!< Datum : Everest (Brunei, E. Malaysia (Sabah and Sarawak))
#define GEO_DATUM_EA 8   //!< Datum : Everest 1830
#define GEO_DATUM_EC 9   //!< Datum : Everest 1956 (India and Nepal)
#define GEO_DATUM_EF 10  //!< Datum : Everest (Pakistan)
#define GEO_DATUM_EE 11  //!< Datum : Everest 1948 (W. Malaysia and Singapore)
#define GEO_DATUM_ED 12  //!< Datum : Everest 1969 (W. Malaysia)
#define GEO_DATUM_RF 13  //!< Datum : Geodetic Reference System 1980 (GRS80)
#define GEO_DATUM_HE 14  //!< Datum : Helmert 1906
#define GEO_DATUM_HO 15  //!< Datum : Hough 1960
#define GEO_DATUM_ID 16  //!< Datum : Indonesian 1974
#define GEO_DATUM_IN 17  //!< Datum : International 1924
#define GEO_DATUM_KA 18  //!< Datum : Krassovsky 1940
#define GEO_DATUM_AM 19  //!< Datum : Modified Airy
#define GEO_DATUM_FA 20  //!< Datum : Modified Fischer 1960
#define GEO_DATUM_SA 21  //!< Datum : South American 1969
#define GEO_DATUM_WD 22  //!< Datum : WGS 1972
#define GEO_DATUM_WE 23  //!< Datum : WGS 1984
#define GEO_DATUM_83 24  //!< Datum : NAD 1983  (same as GRS80)


#define GEO_DATUM_MAX GEO_DATUM_83



#define GEO_OK     0      //!< Geo Library return OK
#define GEO_ERROR  1      //!< Geo Library return ERROR

#ifndef M_PI
#define M_PI            3.14159265358979323846  //!< Value of \f$\pi\f$
#endif
#ifndef TWO_PI
#define TWO_PI           (2.0*M_PI)
#endif

#define sqr(n)          (n*n) //pow(n,2.0)       //!< Squared value: \f$n^2\f$
#define cube(n)         (n*n*n)//)pow(n,3.0)       //!< Cubed value: \f$n^3\f$

#define DEG_TO_RAD              (M_PI/180.0)      //!< Degrees to Radians conversion factor
#define RAD_TO_DEG              (180.0/M_PI)      //!< Radians to Degrees conversion factor
#define MIN_TO_DEG              (1.0/60.0)        //!< Minutes to Degrees conversion factor
#define DEG_TO_MIN              (60.0)            //!< Degrees to Minutes conversion factor
#define SEC_TO_DEG              (1.0/3600.0)      //!< Seconds to Degrees conversion factor
#define SIN_1                   (sin(SEC_TO_DEG*DEG_TO_RAD))
#define CIRCLE                  (360.0)
#define HALF_CIRCLE             (CIRCLE / 2.0)
#define DELTA_LAT                (0.000000001)      //!< Delta Lat for EFG2LLH routines

//#define EARTH_RADIUS_APPROX     6400000.0

#define SOLAR_RADIUS 0.2666           // Solar Semi Diameter as per The Astromicial Almanac 2002, page C24
#define SOLAR_DIAMETER       (2.0 * SOLAR_SEMIDIAMETER)


/*!
\def GEO_B(a,f)
All geodetic datums and ellipsoids are defined by flattening \f$f\f$ and the
major axis \f$a\f$. From these parameters, the other axis and
eccentricity can be calculated. \e Minor \e Axis \f$b\f$
of the Earth is calculated from the \e Inverse \e Flattening
\f$f^{-1}\f$ and the \e Major \e Axis \f$a\f$.

Where \f[b={a (1- \frac{1}{f^{-1}})}\f]
*/

/*!
\def GEO_FL(f)
Since the Geo Library \b ellips structure uses \e inverse \e flattening \f$f^{-1}\f$ then \e flattening
can be calculated by
\f[\frac{1}{f^{-1}}\f]

Duh!
*/

/*!
\def GEO_E2(a,f)
\e Eccentricity \e Squared \f$e^2\f$ is computed in the following manner:
\f[e^2 = \frac{a^2 - b^2}{a^2}\f]
*/
/*!
\def GEO_E2P(a,f)
\e Eccentricity \e Squared \e Prime \f$e^2_p\f$ is computed in the following manner:
\f[e^2_p = \frac{a^2 - b^2}{b^2}\f]
*/
#define GEO_B(a,f)                  (a*(1.0-(1.0/f)))
#define GEO_FL(f)                   (1.0/f)
#define GEO_E2(a,f)                 (((a*a) - ((GEO_B(a,f))*(GEO_B(a,f))))/(a*a))
#define GEO_E2P(a,f)                (((a*a) - ((GEO_B(a,f))*(GEO_B(a,f))))/((GEO_B(a,f))*(GEO_B(a,f))))

// These are computed at compile time to save CPU in the fast routines
#define GEO_WGS84_a     (6378137.0)
#define GEO_WGS84_b     GEO_B(GEO_WGS84_a,298.257223563)
#define GEO_WGS84_fl    GEO_FL(298.257223563)
#define GEO_WGS84_e2    GEO_E2(GEO_WGS84_a,298.257223563)
#define GEO_WGS84_ee2   GEO_E2P(GEO_WGS84_a,298.257223563)



/* Define indices for 3-element coordinate set arrays: */
#define   GEO_LAT     0         //!< Latitude
#define   GEO_LON     1         //!< Longitude
#define   GEO_HGT     2         //!< Height  ( METERS )
#define   GEO_X       0         //!< X or East coordinate of the local tangential plane ( METERS )
#define   GEO_Y       1         //!< Y or North coordinate of the local tangential plane ( METERS )
#define   GEO_Z       2         //!< Z or Up coordinate of the local tangential plane ( METERS )
#define   GEO_E       0         //!< E coordinate of Earth Fixed Geocentric coordinate ( METERS )
#define   GEO_F       1         //!< F coordinate of Earth Fixed Geocentric coordinate ( METERS )
#define   GEO_G       2         //!< G coordinate of Earth Fixed Geocentric coordinate ( METERS )
#define   GEO_RNG     0         //!< Slant range ( METERS )
#define   GEO_AZ      1         //!< Azimuth, clockwise from north
#define   GEO_EL      2         //!< Elevation, from horizon (0) up


#define GEO_SZ_ELLIPSOID_NAME 82     //!< Max size of the Ellipsoid name field

/* Define accuracy for the Efg2Llh routines */
#define GEO_EFG2LLH_MAX_ITS  10      //!< Max iterations allowed in the efg2llh routines
#define GEO_EFG2LLH_ACCURACY_METER 0.00001      //!< Use Meter Accuracy in Efg2Llh routines
#define GEO_EFG2LLH_ACCURACY_CM    0.0000001    //!< Use Centimeter Accuracy in Efg2Llh routines
#define GEO_EFG2LLH_ACCURACY_MM    0.00000001   //!< Use Millimeter Accuracy in Efg2Llh routines
#define GEO_EFG2LLH_ACCURACY_MAX    0.0         //!< Use Maximum Accuracy in Efg2Llh routines
#define GEO_EFG2LLH_ACCURACY   GEO_EFG2LLH_ACCURACY_MM //!< Millimeter accuracy is default

extern double geoAccuracy;

/*! \struct GEO_ELLIPSOID
This defines an earth ellipsoid with the least amount of information.
Additional information will be computed from these values
 */
typedef struct
{
   char name[GEO_SZ_ELLIPSOID_NAME]; /*!< Name/title of ellipsoid */
   char id[4];    /*!< ID designation of the ellipsoid */
   double a;      /*!< Major Earth axis in meters */
   double f1;     /*!< Inverse flattening value */
} GEO_ELLIPSOID;

/*! \struct GEO_DATUM
  This structure holds all of the pertinent datum and ellipsoid data associated
  with particular datum.
*/
typedef struct
{
   int datum_num;     /*!< Numeric datum value */

   /* Ellipsoid values */
   double a;      /*!< Major Earth axis in meters */
   double b;      /*!< Minor Earth axis in meters */
   double flat;   /*!< Earth flattening value */
   double e2;     /*!< Eccentricity squared */
   double ee2;    /*!< Eccentricity squared prime */
   double m1e2;   /*!< 1 - eccentricity squared */
   double bee2;   /*!< b * ee2 */
   double ae2;    /*!< a * e2 */
   double aob;    /*!< a / b */
} GEO_DATUM;


/*! \struct GEO_LOCATION
This structure holds all of the pertinent location,
ellipsoid, and datum data associated with a location.
*/
typedef struct
{
    /* Ellipsoid values - this data is now stored in the datum structure */
   GEO_DATUM datum; /*!< Datum that this location is in. */

   /* Geodetic coordinates */
   double lat;     /*!< Site Latitude in decimal degrees */
   double lon;     /*!< Site Longitude in decimal degrees */
   double hgt;     /*!< Site Height above the ellipsoid in meters */

   double rlat;    /*!< Radian value of latitude */
   double rlon;    /*!< Radian value of longitude */

   /* Sine and Cosine values */
   double slat;    /*!< Sine of latitude */
   double clat;    /*!< Cosine of latitude */
   double slon;    /*!< Sine of longitude */
   double clon;    /*!< Cosine of longitude */
   double tlat;    /*!< Tangent of latitude */

   /* Precomputed values (for speed) */
   double clonclat; /*!< Cos(lon) * Cos(lat) */
   double slonslat; /*!< Sin(lon) * Sin(lat) */
   double clonslat; /*!< Cos(lon) * Sin(lat) */
   double slonclat; /*!< Sin(lon) * Cos(lat) */

   double n;        /*!< Radius of curvature */
   double m;        /*!< Meridional radius of curvature */

   /* Geocentric coordinates */
   double e;       /*!< X : Earth Fixed Geocentric (XYZ) Coordinates */
   double f;       /*!< Y : Earth Fixed Geocentric (XYZ) Coordinates */
   double g;       /*!< Z : Earth Fixed Geocentric (XYZ) Coordinates */
   double efg[3];  /*!< EFG: array of XYZ */

   /* Geomagentism Related information */
   /* Caution: this field's value can change daily! (but not by much) */
   double Declination;    /*!< Geomagnetic Declination */

   double timezone;  /*!< Time zone in hours from GMT (i.e Mountain STD time is +7 hours) */
   int    dst;       /*!< Daylight Savings Time (1=yes or 0=no) */

   /* Misc values */
   // int datum;     /*!< Numeric datum value */
   char name[GEO_SZ_ELLIPSOID_NAME]; /*!< Site Name */
} GEO_LOCATION;

extern GEO_ELLIPSOID ellips[];
extern int geoSunError;



/*  \struct WMM_DATA
This structure contains the WMM-20xx coefficients.
*/
typedef struct
{
    int n;          //!< n
    int m;          //!< m
    double gnm;     //!< gnm
    double hnm;     //!< hnm
    double dgnm;    //!< dgnm
    double dhnm;    //!< dhnm
} WMM_DATA;


/* geo function prototypes */

#ifdef __cplusplus
extern "C" {
#endif

#ifndef MAX
#define MAX(a,b) ((a>b)?(a):(b))
#endif

/* geoEllips function */
DLL_API void    DLL_CALLCONV geoGetEllipsoid(double *a,double *b,double *e2,double *ee2,double *f,int datum);
DLL_API int     DLL_CALLCONV geoInitDatum(GEO_DATUM* d, int datum);
DLL_API int     DLL_CALLCONV geoInitLocation(GEO_LOCATION *l, double lat, double lon, double hgt, int datum,  char *name);
DLL_API int     DLL_CALLCONV geoInitLocation2(GEO_LOCATION *l, double lat, double lon, double hgt, const GEO_DATUM* datum,  const char *name);
DLL_API void    DLL_CALLCONV geoSetTimeZone(GEO_LOCATION *l, double tz, int dst);




/* geoMag functions */
DLL_API int   DLL_CALLCONV geomg1( double alt,  double glat,  double glon,  double time,
              double *dec, double *dip,  double *ti,   double *gv);

DLL_API int   DLL_CALLCONV geoMag( double alt,  double glat,  double glon,  double time,
              double *dec, double *dip,  double *ti,   double *gv,
              double *adec,double *adip, double *ati,
              double *x,   double *y,    double *z,    double *h,
              double *ax,  double *ay,   double *az,   double *ah);

DLL_API int     DLL_CALLCONV geoMagGetDec(double lat, double lon, double hgt, int month, int day, int year, double *dec);
DLL_API double  DLL_CALLCONV geoMagGetDecRet(double lat, double lon, double hgt, int month, int day, int year);
DLL_API int     DLL_CALLCONV geoMagFillDec(GEO_LOCATION *l, double *dec);
DLL_API double  DLL_CALLCONV geoMagGetDecNow(double lat, double lon, double hgt);

DLL_API int DLL_CALLCONV geoSun(GEO_LOCATION *loc, struct tm *newtime,double *az, double *el);
DLL_API int DLL_CALLCONV geoSunM(GEO_LOCATION *loc, struct tm *newtime,double *az, double *el);
DLL_API int DLL_CALLCONV geoSunAA(GEO_LOCATION *loc, struct tm *newtime,double *az, double *el);
DLL_API int DLL_CALLCONV geoSunPosition(GEO_LOCATION *loc, double *az, double *el);

//void geoEfg2Llh(double efg[], double *lat, double *lon, double *hgt);

#ifdef __cplusplus
}
#endif

#endif // GEO_H
