// PulseOximeter.cpp : �R���\�[�� �A�v���P�[�V�����̃G���g�� �|�C���g���`���܂��B
//
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include "pulse_param.h"
#include "hanning_window.h"

#ifdef __cplusplus
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#define DLLEXPORT __declspec(dllexport)
#endif

/*==============================================================================*/
DLLEXPORT void __stdcall calculator_clr(int *data, int len, char* ppath);
DLLEXPORT void __stdcall calculator_inf(int *data, int len, char* ppath);
DLLEXPORT void __stdcall get_dc(int* pdata);
DLLEXPORT void __stdcall get_fft(double* pdata);
DLLEXPORT void __stdcall get_ifft(double* pdata);
DLLEXPORT void __stdcall get_new_ifft(double* pdata);
DLLEXPORT int __stdcall get_sinpak_clr(void);
DLLEXPORT int __stdcall get_sinpak_inf(void);
DLLEXPORT int __stdcall get_spo2(void);
DLLEXPORT double __stdcall get_acdc(void);
static void	proc_spo2(const double *pdata, int len, int psnpk, double* pmk, double* pmin, int no);
static void	proc(const double *pdata, int len, int* psnpk, double* pmk, double* pmin, int no);
/*==============================================================================*/
#define TEST_DATA_NUM	(406)
#define BUF_SIZE		(128)
#define DATA_SIZE_SPO2	(128)
/*------------------------------------------------------------------------------*/
extern	double	TEST1_data1[TEST_DATA_NUM];
extern	double	TEST1_data2[TEST_DATA_NUM];
extern	double	TEST2_data1[TEST_DATA_NUM];
extern	double	TEST2_data2[TEST_DATA_NUM];
/*------------------------------------------------------------------------------*/
extern	void	debug_out	( char *f , const double d[] , int size , const char* ppath, int no );
extern	void	mado		( double in[] , double ot[] , int size , int mode );
extern	void	fft			( const double in[] , int N, double ar[] , double ai[] , double p[] );
extern	int		peak_modify	( double in_data[] , int in_res[] , double ot_data[] , double ot_hz[] , int size , double delta );	/* �� */
extern	void 	peak_vallay	( double in[] , int    ot[] , int size, int width , double th , int peak );
extern	void	cal_sp1		( double mx1 , double mx2, int *sp );
extern	void	ifft(double	ar[],double	ai[],int N, double	ot[]);
static double CalcCoeff(double dc);
/*------------------------------------------------------------------------------*/
double	temp_dbl_buf0[BUF_SIZE];
double	temp_dbl_buf1[BUF_SIZE];
double	temp_dbl_buf2[BUF_SIZE];

typedef int int16_t;
int16_t		temp_int_buf0[BUF_SIZE];

double	mx1_;
double	mx2_;
double	min1_;
double	min2_;
int dc_[BUF_SIZE];
double fft_[BUF_SIZE];
double ifft_[BUF_SIZE];
double new_ifft_[BUF_SIZE];
int	snpk1_;
int	snpk2_;
int		sp1_;
int		sp2_;
double acdc_;

char path_[256];

DLLEXPORT void __stdcall calculator_clr(int *pdata, int len, char* ppath)
{
	// �t�@�C���o�̓p�X��ۑ�
	int pos=0;
	if(ppath){
		while(ppath[pos] != '\0'){
			path_[pos] = ppath[pos];
			pos+=1;
		}
		path_[pos] = '\0';
	}
	
	double tmp;
	int i;
	
	for (i=0; i<DATA_SIZE_SPO2; i++)
	{
		tmp = (double)pdata[i];
		// -3�`3V��24bit����\AD�l
		temp_dbl_buf2[i] = tmp;
	}
//	proc(test_data, DATA_SIZE_SPO2, &snpk1_, &mx1_, &min1_, 1);
	proc(temp_dbl_buf2, DATA_SIZE_SPO2, &snpk1_, &mx1_, &min1_, 1);
	if((0 > snpk1_) || (snpk1_ > 200)){
		snpk1_ = 0;
	}
	
	for (i=0; i<DATA_SIZE_SPO2; i++)
	{
		tmp = (double)pdata[i];
		// -3�`3V��24bit����\AD�l
		temp_dbl_buf2[i] = tmp;
	}
	proc_spo2(temp_dbl_buf2, DATA_SIZE_SPO2, snpk1_, &mx1_, &min1_, 1);
}

DLLEXPORT void __stdcall calculator_inf(int *pdata, int len, char* ppath)
{
	double tmp;
	int i;
	
	for (i = 0; i<DATA_SIZE_SPO2; i++)
	{
		tmp = (double)pdata[i];
		temp_dbl_buf2[i] = tmp;
	}
	proc(temp_dbl_buf2, DATA_SIZE_SPO2, &snpk2_, &mx2_, &min2_, 2);
	if((0 > snpk2_) || (snpk2_ > 200)){
		snpk2_ = 0;
	}
	for (i = 0; i<DATA_SIZE_SPO2; i++)
	{
		tmp = (double)pdata[i];
		temp_dbl_buf2[i] = tmp;
	}
	proc_spo2(temp_dbl_buf2, DATA_SIZE_SPO2, snpk2_, &mx2_, &min2_, 2);
	
	/*- SPO2 --------------------------------------------------------------*/
	double	coeff = CalcCoeff(min2_) / CalcCoeff(min1_);
	if(mx2_ != 0){
		acdc_ = (mx1_ / mx2_) * coeff;
	}else{
		acdc_ = 0;
	}
	debug_out("acdc", &acdc_, 1, path_, 0);
	cal_sp1( mx1_, mx2_, &sp1_ );
	double test = (double)sp1_;
	debug_out("spo2", &test, 1, path_, 0);
	if((0 > sp1_) || (sp1_ > 200)){
		sp1_ = 0;
	}
}

/********************************************************************/
/* �֐�     : proc													*/
/* �֐���   : 														*/
/* ����     : �Ȃ�													*/
/* �߂�l   : �Ȃ�													*/
/* �ύX���� : 2018.01.11 Axia Soft Design �a�c	���ō쐬			*/
/********************************************************************/
/* �@�\ :															*/
/* 																	*/
/********************************************************************/
/* ���ӎ��� :														*/
/* len = 128�łȂ��Ɠ����Ȃ�										*/
/********************************************************************/
static void	proc_spo2(const double *pdata, int len, int snpk, double* pmk, double* pmin, int no)
{
	/*- fft --------------------------------------------------------------------*/
	double	ar1[BUF_SIZE];
	double	ai1[BUF_SIZE];
	fft(pdata, len, ar1, ai1, fft_);
	debug_out("new_ar1", ar1, len, path_, no);
	debug_out("new_ai1", ai1, len, path_, no);
	
	/*- F0�t�ߊO�̃f�[�^���J�b�g--------------------------------------------------*/
	double f0 = snpk / 60.0f;
	int center = (int)(f0 / 0.05);
	int min, max;
	const int width = 5;
	if(center > width){
		min = center - width;
	}else{
		min = 0;
	}
	max = center + width;
	if(max > len){
		max = len;
	}
	for(int ii=0;ii<len;++ii){
		if(ii < min || ii > max){
			ar1[ii] = 0;
			ai1[ii] = 0;
		}
	}
	debug_out("new_ar1_cut", ar1, len, path_, no);
	debug_out("new_ai1_cut", ai1, len, path_, no);
	
	/*- �tFFT --------------------------------------------------------------*/
	double* p3 = &new_ifft_[0];
	ifft(ar1, ai1, len, p3);
	debug_out("new_ifft", p3, len, path_, no);
}

/********************************************************************/
/* �֐�     : proc													*/
/* �֐���   : 														*/
/* ����     : �Ȃ�													*/
/* �߂�l   : �Ȃ�													*/
/* �ύX���� : 2018.01.11 Axia Soft Design �a�c	���ō쐬			*/
/********************************************************************/
/* �@�\ :															*/
/* 																	*/
/********************************************************************/
/* ���ӎ��� :														*/
/* len = 128�łȂ��Ɠ����Ȃ�										*/
/********************************************************************/
static void	proc(const double *pdata, int len, int* psnpk, double* pmk, double* pmin, int no)
{
	const double coeff	= _PULSE_PARAM_SNPK_COEF;
	const int start		= _PULSE_PARAM_START;
	const int end		= _PULSE_PARAM_END;

	double* ph11;
	double* p2;
	double* p3;
	double*	p4;
	double* ar2;
	double* ai2;
	double*	f1;
	int16_t* peak1;

	double max;
	int ii;
	
	/*- DC�������� -----------------------------------------------------------------*/
	ph11 = &temp_dbl_buf0[0];
	double min = pdata[0];
	for(int ii=1;ii<len;++ii){
		if(min > pdata[ii]){
			min = pdata[ii];
		}
	}
	*pmin = min;
	for(int ii=0;ii<len;++ii){
		ph11[ii] = (pdata[ii] - min);			//DC�����f�[�^
	}
	debug_out("raw", pdata, len, path_, no);	//���f�[�^�o��
	debug_out("dc", ph11, len, path_, no);		//DC�����f�[�^�o��
	
	/*- fft --------------------------------------------------------------------*/
	double	ar1[BUF_SIZE];		//������
	double	ai1[BUF_SIZE];		//������
	
	for(int ii=0;ii<len;++ii){
		ph11[ii] = pdata[ii] * hanning_window[ii];		//���Z�f�[�^
	}
	fft(ph11, len, ar1, ai1, fft_);
//	debug_out("fft", fft_, len, path_, no);		//FFT���Z���ʏo��

	/*- �s�v�ȃf�[�^���}�X�N����------------------------------------------------*/
	/*- �p���[�����߂�----------------------------------------------------------*/
	p2 = &temp_dbl_buf1[0];
	// start�܂ł�all 0
	for(ii=0;ii<start;++ii){
		p2[ii] = 0;
	}
	// �Q�捪����(�U���X�y�N�g��)
	for(ii=start;ii<end;++ii){
		p2[ii] = sqrt((sqrt(ar1[ii]*ar1[ii] + ai1[ii]*ai1[ii]))/len);
	}
	for(ii=end;ii<len;++ii){
		p2[ii] = 0;
	}
	debug_out("p2", p2, len, path_, no);		//�ш撊�o�o��
	
	/*- �s�[�N���o --------------------------------------------------------------*/
	peak1 = &temp_int_buf0[0];	//�s�[�N���茋��
	p4 = &temp_dbl_buf0[0];		//�p���[�X�y�N�g��
	f1 = &temp_dbl_buf2[0];		//���g��
	
	//�s�[�N����
	peak_vallay( p2 , peak1 , len, 3 , 0.1 , 1 );
	//�p���[�X�y�N�g���A���g���o��
	peak_modify( p2 , peak1 , p4 , f1 , len , 0.1);
	*pmk = p4[0];

	/*- �tFFT --------------------------------------------------------------*/
	ar2 = &temp_dbl_buf0[0];	//������
	ai2 = &temp_dbl_buf2[0];	//������
	
	memcpy(&ar2[0], &p2[0], sizeof(double)*len);	//�������ɑш撊�o�l�R�s�[
	memset(&ai2[0], 0x00, len);		//�������ɂO�Z�b�g
	
	p3 = &ifft_[0];
	ifft(ar2, ai2, len, p3);
	debug_out("ifft", p3, len, path_, no);		//�tFFT���Z���ʏo��
	
	/*- �ő�l�Ƃ̔���v�Z --------------------------------------------------------------*/
	max = 0;
	for(ii=start;ii<end;++ii){
		if(max < p3[ii]){
			max = p3[ii];
		}
	}
	for(ii=0;ii<len;++ii){
		p3[ii] /= max;
	}
	debug_out("p3hi", p3, len, path_, no);		//�ő�l�Ƃ̔�o��
	
	/*- �s�[�N���o --------------------------------------------------------------*/
	peak1 = &temp_int_buf0[0];
	p4 = &temp_dbl_buf1[0];
	f1 = &temp_dbl_buf2[0];
	peak_vallay( p3 , peak1 , len, 3 , 0.1 , 1 );
	peak_modify( p3 , peak1 , p4 , f1 , len , 1);
	debug_out("peak", f1, len, path_, no);
	
	/*- HR --------------------------------------------------------------*/
	*psnpk = (int)(60 / (f1[0] * coeff));
	double test = *psnpk;
	debug_out("snpk", &test, 1, path_, no);
}

// DC���������f�[�^
DLLEXPORT void __stdcall get_dc(int* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = dc_[ii];
	}
}

// FFT�f�[�^
DLLEXPORT void __stdcall get_fft(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = fft_[ii];
	}
}

// �tFFT�f�[�^
DLLEXPORT void __stdcall get_ifft(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = ifft_[ii];
	}
}

// �tFFT�f�[�^
DLLEXPORT void __stdcall get_new_ifft(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = new_ifft_[ii];
	}
}

// �S����(�ԐF)
DLLEXPORT int __stdcall get_sinpak_clr(void)
{
	return snpk1_;
}

// �S����(�ԊO)
DLLEXPORT int __stdcall get_sinpak_inf(void)
{
	return snpk2_;
}

// SPO2���擾
DLLEXPORT int __stdcall get_spo2(void)
{
	return sp1_;
}

// SPO2���擾
DLLEXPORT double __stdcall get_acdc(void)
{
	return acdc_;
}

static double CalcCoeff(double dc)
{
	double ret = 0.5 + ((dc*0.2) / 8388608);
	
	return ret;
}

/*==============================================================================*/
/* EOF */
