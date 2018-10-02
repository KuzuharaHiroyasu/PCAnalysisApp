// PulseOximeter.cpp : コンソール アプリケーションのエントリ ポイントを定義します。
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
extern	int		peak_modify	( double in_data[] , int in_res[] , double ot_data[] , double ot_hz[] , int size , double delta );	/* ☆ */
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
	// ファイル出力パスを保存
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
		// -3～3Vの24bit分解能AD値
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
		// -3～3Vの24bit分解能AD値
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
/* 関数     : proc													*/
/* 関数名   : 														*/
/* 引数     : なし													*/
/* 戻り値   : なし													*/
/* 変更履歴 : 2018.01.11 Axia Soft Design 和田	初版作成			*/
/********************************************************************/
/* 機能 :															*/
/* 																	*/
/********************************************************************/
/* 注意事項 :														*/
/* len = 128でないと動かない										*/
/********************************************************************/
static void	proc_spo2(const double *pdata, int len, int snpk, double* pmk, double* pmin, int no)
{
	/*- fft --------------------------------------------------------------------*/
	double	ar1[BUF_SIZE];
	double	ai1[BUF_SIZE];
	fft(pdata, len, ar1, ai1, fft_);
	debug_out("new_ar1", ar1, len, path_, no);
	debug_out("new_ai1", ai1, len, path_, no);
	
	/*- F0付近外のデータをカット--------------------------------------------------*/
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
	
	/*- 逆FFT --------------------------------------------------------------*/
	double* p3 = &new_ifft_[0];
	ifft(ar1, ai1, len, p3);
	debug_out("new_ifft", p3, len, path_, no);
}

/********************************************************************/
/* 関数     : proc													*/
/* 関数名   : 														*/
/* 引数     : なし													*/
/* 戻り値   : なし													*/
/* 変更履歴 : 2018.01.11 Axia Soft Design 和田	初版作成			*/
/********************************************************************/
/* 機能 :															*/
/* 																	*/
/********************************************************************/
/* 注意事項 :														*/
/* len = 128でないと動かない										*/
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
	
	/*- DC成分除去 -----------------------------------------------------------------*/
	ph11 = &temp_dbl_buf0[0];
	double min = pdata[0];
	for(int ii=1;ii<len;++ii){
		if(min > pdata[ii]){
			min = pdata[ii];
		}
	}
	*pmin = min;
	for(int ii=0;ii<len;++ii){
		ph11[ii] = (pdata[ii] - min);			//DC抜きデータ
	}
	debug_out("raw", pdata, len, path_, no);	//生データ出力
	debug_out("dc", ph11, len, path_, no);		//DC抜きデータ出力
	
	/*- fft --------------------------------------------------------------------*/
	double	ar1[BUF_SIZE];		//実数部
	double	ai1[BUF_SIZE];		//虚数部
	
	for(int ii=0;ii<len;++ii){
		ph11[ii] = pdata[ii] * hanning_window[ii];		//演算データ
	}
	fft(ph11, len, ar1, ai1, fft_);
//	debug_out("fft", fft_, len, path_, no);		//FFT演算結果出力

	/*- 不要なデータをマスクする------------------------------------------------*/
	/*- パワーを求める----------------------------------------------------------*/
	p2 = &temp_dbl_buf1[0];
	// startまではall 0
	for(ii=0;ii<start;++ii){
		p2[ii] = 0;
	}
	// ２乗根処理(振幅スペクトル)
	for(ii=start;ii<end;++ii){
		p2[ii] = sqrt((sqrt(ar1[ii]*ar1[ii] + ai1[ii]*ai1[ii]))/len);
	}
	for(ii=end;ii<len;++ii){
		p2[ii] = 0;
	}
	debug_out("p2", p2, len, path_, no);		//帯域抽出出力
	
	/*- ピーク検出 --------------------------------------------------------------*/
	peak1 = &temp_int_buf0[0];	//ピーク判定結果
	p4 = &temp_dbl_buf0[0];		//パワースペクトル
	f1 = &temp_dbl_buf2[0];		//周波数
	
	//ピーク判定
	peak_vallay( p2 , peak1 , len, 3 , 0.1 , 1 );
	//パワースペクトル、周波数出力
	peak_modify( p2 , peak1 , p4 , f1 , len , 0.1);
	*pmk = p4[0];

	/*- 逆FFT --------------------------------------------------------------*/
	ar2 = &temp_dbl_buf0[0];	//実数部
	ai2 = &temp_dbl_buf2[0];	//虚数部
	
	memcpy(&ar2[0], &p2[0], sizeof(double)*len);	//実数部に帯域抽出値コピー
	memset(&ai2[0], 0x00, len);		//虚数部に０セット
	
	p3 = &ifft_[0];
	ifft(ar2, ai2, len, p3);
	debug_out("ifft", p3, len, path_, no);		//逆FFT演算結果出力
	
	/*- 最大値との比を計算 --------------------------------------------------------------*/
	max = 0;
	for(ii=start;ii<end;++ii){
		if(max < p3[ii]){
			max = p3[ii];
		}
	}
	for(ii=0;ii<len;++ii){
		p3[ii] /= max;
	}
	debug_out("p3hi", p3, len, path_, no);		//最大値との比出力
	
	/*- ピーク検出 --------------------------------------------------------------*/
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

// DC成分除去データ
DLLEXPORT void __stdcall get_dc(int* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = dc_[ii];
	}
}

// FFTデータ
DLLEXPORT void __stdcall get_fft(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = fft_[ii];
	}
}

// 逆FFTデータ
DLLEXPORT void __stdcall get_ifft(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = ifft_[ii];
	}
}

// 逆FFTデータ
DLLEXPORT void __stdcall get_new_ifft(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE_SPO2;++ii){
		pdata[ii] = new_ifft_[ii];
	}
}

// 心拍数(赤色)
DLLEXPORT int __stdcall get_sinpak_clr(void)
{
	return snpk1_;
}

// 心拍数(赤外)
DLLEXPORT int __stdcall get_sinpak_inf(void)
{
	return snpk2_;
}

// SPO2を取得
DLLEXPORT int __stdcall get_spo2(void)
{
	return sp1_;
}

// SPO2を取得
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
