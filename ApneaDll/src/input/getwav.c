/************************************************************************/
/* システム名   : 無呼吸判定											*/
/* ファイル名   : getwav.c												*/
/* 機能         : inputファイルの読み込み								*/
/* 変更履歴     : 2017.07.12 Axia Soft Design mmura	初版作成			*/
/* 注意事項     : なし                                                  */
/************************************************************************/

#ifdef __cplusplus
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#define DLLEXPORT __declspec(dllexport)
#endif

/********************/
/*     include      */
/********************/
#include "getwav.h"
#include "apnea_param.h"
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

extern void	debug_out( char *f , double d[] , int size , const char* ppath );
extern int	peak_modify	( const double in_data[] , int in_res[] , double ot_data[] , double ot_hz[] , int size , double delta, double th);	/* ☆ */
extern void peak_vallay	( const double in[] , int    ot[] , int size, int width , int peak );

/************************************************************/
/* プロトタイプ宣言											*/
/************************************************************/
void getwav_pp(const double* pData, int DSize, double Param1);
void getwav_apnea(const double* pData, int DSize, int Param1, double Param2, double Param3, double Param4);
void getwav_snore(const double* pData, int DSize, double Param);

DLLEXPORT void __stdcall getwav_init(int* pdata, int len, char* ppath, int* psnore);
DLLEXPORT int	__stdcall get_result_snore(double* data);
DLLEXPORT int	__stdcall get_result_peak(double* data);
DLLEXPORT int	__stdcall get_result_apnea(double* data);
DLLEXPORT int   __stdcall get_result_snore_count();
DLLEXPORT void   __stdcall get_accelerometer(double data_x, double data_y, double data_z, char* ppath);
DLLEXPORT void __stdcall calc_snore_init(void);

static int proc_on(int Pos);
static int proc_off(int Pos);
static void Save(void);
static void Reset(void);
static void Judge(void);

/************************************************************/
/* マクロ													*/
/************************************************************/
#define DATA_SIZE		(200)	// 10秒間、50msに1回データ取得した数
#define BUF_SIZE		(256)	// DATA_SIZE + 予備
#define BUF_SIZE_APNEA	(20)	// 無呼吸・低呼吸の結果は生データの100分の1
#define RIREKI			3

/************************************************************/
/* 型定義													*/
/************************************************************/


/************************************************************/
/* ＲＡＭ定義												*/
/************************************************************/
double	result_peak[BUF_SIZE];			// 結果ピーク間隔
int		result_peak_size;
int		apnea_ = APNEA_NONE;	// 呼吸状態
int		snore_ = SNORE_OFF;		// いびき

// 演算途中データ
double raw_[DATA_SIZE];
double	dc_[DATA_SIZE];
double	movave_[DATA_SIZE];
double	ave_[DATA_SIZE];
double	eval_[DATA_SIZE];
double	rms_[DATA_SIZE];
double	point_[DATA_SIZE];

double	xy2_[DATA_SIZE];
double	interval_[DATA_SIZE];

char path_[256];
char pathAcce_[256];

static B	SnoreTime_[RIREKI];
static UB	SnoreFlg_; // ONカウント中 or OFFカウント中
static int	SnoreCnt_; // ON連続回数, OFF連続回数 兼用

int temp_int_buf0[BUF_SIZE];

/************************************************************/
/* ＲＯＭ定義												*/
/************************************************************/
extern	double	testdata[200];

/************************************************************************/
/* 関数     : getwav_init												*/
/* 関数名   : 初期化処理												*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2017.07.12 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
DLLEXPORT void    __stdcall getwav_init(int* pdata, int len, char* ppath, int* psnore)
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
	
	// 移動平均
	for(int ii=0;ii<len;++ii){
		dc_[ii] = (double)pdata[ii];
	}
	
	static const int N = APNEA_PARAM_AVE_NUM;
	for(int ii=0;ii<len;++ii){
		movave_[ii]=0;
		for(int jj=0;jj<N;++jj){
			if((ii-jj)>=0){
				movave_[ii]+=dc_[ii-jj];
			}
		}
		movave_[ii] /= (double)N;
	}
	for(int ii=0;ii<len;++ii){
		raw_[ii] = (double)psnore[ii];
	}
	debug_out("rawsnore", raw_, len, path_);
	debug_out("raw", dc_, len, path_);
	debug_out("movave", movave_, len, path_);
	double* ptest1= (double*)calloc(len, sizeof(double));
	for(int ii=0;ii<len;++ii){
		ptest1[ii] = movave_[ii] / APNEA_PARAM_RAW;
	}

	// (21) - (34)
	getwav_pp(ptest1, len, APNEA_PARAM_PEAK_THRE);
//	getwav_pp(testdata, 200, 0.003f);
	
	// (35) - (47)
	getwav_apnea(ptest1, len, APNEA_PARAM_AVE_CNT, APNEA_PARAM_AVE_THRE, APNEA_PARAM_BIN_THRE, APNEA_PARAM_APNEA_THRE);
//	getwav_apnea(testdata, 200, 450, 0.0015f, 0.002f);
	
	// (48) - (56)
	getwav_snore(raw_, len, APNEA_PARAM_SNORE);
	double tmpsnore = (double)snore_;
	debug_out("snore_", &tmpsnore, 1, path_);
//	getwav_snore(testdata, 200, 0.0125f);
	
	// (57)
}

/************************************************************************/
/* 関数     : calc_snore_init											*/
/* 関数名   : 初期化処理												*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2017.07.12 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
DLLEXPORT void    __stdcall calc_snore_init(void)
{
	int ii;

	SnoreFlg_ = OFF;
	SnoreCnt_ = 0;
	snore_ = SNORE_OFF;
	for (ii = 0; ii<RIREKI; ++ii) {
		SnoreTime_[ii] = -1;
	}
}

/************************************************************************/
/* 関数     : getwav_pp													*/
/* 関数名   : ピーク間隔演算処理										*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2017.07.12 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
void getwav_pp(const double* pData, int DSize, double Param1)
{
	// (22) = Param1
	
	// (23)
	// (24) = peak1
	int* ppeak1 = (int*)calloc(DSize, sizeof(int));
	double* ppdata1 = (double*)calloc(DSize, sizeof(double));
	double* pf1 = (double*)calloc(DSize, sizeof(double));
	peak_vallay(pData, ppeak1, DSize, 3, 1);
	int peakcnt1 = peak_modify(pData, ppeak1, ppdata1, pf1, DSize, 1.0, 0.0);
	debug_out("peak1", ppdata1, peakcnt1, path_);
	debug_out("f1", pf1, peakcnt1, path_);
	
	// (25) = peakcnt2
	// (26) = f2
	// (27) = peak2
	int* ppeak2 = (int*)calloc(peakcnt1, sizeof(int));
	double* ppdata2 = (double*)calloc(peakcnt1, sizeof(double));
	double* pf2 = (double*)calloc(peakcnt1, sizeof(double));
	peak_vallay(ppdata1, ppeak2, peakcnt1, 3, 1);
	int peakcnt2 = peak_modify(ppdata1, ppeak2, ppdata2, pf2, peakcnt1, 1.0, Param1);
	debug_out("peak2", ppdata2, peakcnt2, path_);
	debug_out("f2", pf2, peakcnt2, path_);
	
	double* ppdata3 = (double*)calloc(peakcnt2, sizeof(double));
	int peakpos = 0;
	for(int ii=0;ii<peakcnt2;++ii){
		peakpos = (int)(pf2[ii] + 0.5);
		ppdata3[ii] = pf1[peakpos] * 0.01f;
	}
	debug_out("peak3", ppdata3, peakcnt2, path_);
	
	// (28) - (30)
	double max = pData[0];
	double pos = 0;
	for(int ii=1;ii<DSize;++ii){
		if(max < pData[ii]){
			max = pData[ii];
			pos = ii;
		}
	}
	
#if 1
	int peakcnt = peakcnt2;
	double* ppos = ppdata3;
#else
	int peakcnt = 73;
	double* ppos = &testdata2[0];
#endif
	double* pinterval = (double*)calloc(peakcnt+1, sizeof(double));
	pinterval[0] = ppos[0];
	for(int ii=0;ii<peakcnt;++ii){
		pinterval[ii+1] = ppos[ii+1] - ppos[ii];
	}
	debug_out("ppinterval", pinterval, peakcnt+1, path_);
	
	// (31) - (33)
	int ppcnt = 0;
	double* pp2 = (double*)calloc(peakcnt, sizeof(double));
	double* pp5 = (double*)calloc(peakcnt, sizeof(double));
	for(int ii=0;ii<peakcnt;++ii){
		if(pinterval[ii] >= 10){
			pp2[ppcnt] = ppos[ii-1];
			pp5[ppcnt] = ppos[ii];
			ppcnt += 1;
		}
	}
	debug_out("pp2", pp2, ppcnt, path_);
	debug_out("pp5", pp5, ppcnt, path_);
	// (34)
	if(ppcnt >= 1){
		int size = (int)(pp5[ppcnt-1] + 0.5);
		double* presult = (double*)calloc(size, sizeof(double));
		int rpos = 0;
		for(int ii=0;ii<ppcnt;++ii){
			int loop2 = (int)(pp2[ii] + 0.5) -1;
			for(int jj=rpos;jj<pp2[ii];++jj){
				presult[rpos] = 0;
				rpos+=1;
			}
			int loop5 = (int)(pp5[ii] + 0.5) - 1;
			for(int jj=rpos;jj<pp5[ii];++jj){
				presult[rpos] = max;
				rpos+=1;
			}
		}
		debug_out("peakresult", presult, size, path_);
		memcpy(result_peak, presult, size * sizeof(double));
		result_peak_size = size;
		free(presult);
	}else{
		result_peak[0] = 0x00;
		result_peak_size = 0;
	}
	
	free(ppeak1);
	free(ppdata1);
	free(pf1);
	free(ppeak2);
	free(ppdata2);
	free(pf2);
	free(ppdata3);
	free(pinterval);
	free(pp2);
	free(pp5);
}

/************************************************************************/
/* 関数     : getwav_apnea												*/
/* 関数名   : 無呼吸演算処理											*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2017.07.12 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
void getwav_apnea(const double* pData, int DSize, int Param1, double Param2, double Param3, double Param4)
{
	// (35) = Param1
	// (36) = Param2
	// (40) = Param3
	
	// (37)
	for(int ii=0;ii<DSize;++ii){
		int min=0;
		int loop=0;
		if(ii <= DSize-1){
			int tmp = DSize-1 - ii;
			if(tmp > Param1){
				min = Param1;
			}else{
				min = tmp;
			}
			loop = min * 2 + 1;
			min = ii - min;
			if(min < 0){
				min = 0;
			}
		}else{
			min = 0;
			loop = ii * 2 + 1;
		}
		double ave = 0.0f;
		for(int jj=0;jj<loop;++jj){
			ave += pData[min+jj];
		}
		ave /= loop;
		ave_[ii] = ave;
	}
	debug_out("ave", ave_, DSize, path_);
	
	// (38) - (39)
	for(int ii=0;ii<DSize;++ii){
		if(ave_[ii] >= Param2){
			eval_[ii] = 1;
		}else{
			eval_[ii] = 0;
		}
	}
	debug_out("eval2", eval_, DSize, path_);
	
	// (41) ... 使用していないため省略
	// (42)
	// (43) = prms
	int datasize = DSize / 20;
	memset(rms_, 0x00, DSize);
	for(int ii=0;ii<datasize;++ii){
		rms_[ii] = 0.0f;
		double tmp = 0.0f;
		for(int jj=0;jj<20;++jj){
			tmp += (pData[ii*20 + jj]*pData[ii*20 + jj]);
		}
		tmp /= 20;
		rms_[ii] = sqrt(tmp);
	}
	debug_out("RMS", rms_, datasize, path_);
	
	// (44) = ppoint
	// (45) = rms_
	memset(point_, 0x00, DSize);
	for(int ii=0;ii<datasize;++ii){
		if(rms_[ii] >= Param3){
			point_[ii] = 1;
		}else{
			point_[ii] = 0;
		}
	}
	debug_out("point", point_, datasize, path_);
	
	// (46)
	if(datasize == 0){
		apnea_ = APNEA_NORMAL;
	}
	else if(datasize > 9){
		apnea_ = APNEA_WARN;
		int loop = datasize - 9;
		for(int ii=0;ii<loop;++ii){
			double apnea = 0;
			for(int jj=0;jj<9;++jj){
				apnea += point_[ii + jj];
			}
			if(apnea != 0){
				apnea_ = APNEA_NORMAL;
			}
		}
	}else{
		apnea_ = APNEA_NORMAL;
	}
	
	// 完全無呼吸の判定
	if(apnea_ == APNEA_WARN){
		apnea_ = APNEA_ERROR;
		for(int ii=0;ii<datasize;++ii){
			if(pData[ii] > Param4){
				apnea_ = APNEA_WARN;
				break;
			}
		}
	}
	
	double tmpapnea = (double)apnea_;
	debug_out("apnea", &tmpapnea, 1, path_);
}

/************************************************************************/
/* 関数     : getwav_snore												*/
/* 関数名   : いびき演算処理											*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2017.07.12 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
void getwav_snore(const double* pData, int DSize, double Param)
{
	int ii;
	int jj;
	int loop;
	int size;
	int pos = 0;

	// 閾値を超えた回数の移動累計をとる
	loop = DATA_SIZE_APNEA - SNORE_PARAM_SIZE;
	for (ii = 0; ii<loop; ++ii) {
		temp_int_buf0[ii] = 0;
		size = ii + SNORE_PARAM_SIZE;
		for (jj = ii; jj<size; ++jj) {
			if (pData[jj] >= SNORE_PARAM_THRE) {
				temp_int_buf0[ii] += 1;
			}
		}
	}

	while (pos < loop) {
		switch (SnoreFlg_) {
		case ON:
			pos = proc_on(pos);
			break;
		case OFF:
			pos = proc_off(pos);
			break;
		default:
			calc_snore_init();
			return;
		}
	}
}


/************************************************************************/
/* 関数     : proc_on													*/
/* 関数名   : いびきON時処理											*/
/* 引数     : int Data : 波形データ										*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2018.07.25 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
static int proc_on(int Pos)
{
	int ii;
	int loop = DATA_SIZE_APNEA - SNORE_PARAM_SIZE;
	int pos = loop;

	// OFF確定している場所を特定する
	for (ii = Pos; ii<loop; ++ii) {
		if (temp_int_buf0[ii] <= SNORE_PARAM_OFF_CNT) {
			SnoreFlg_ = OFF;
			pos = ii;
			Save();
			Judge();
			break;
		}
		else {
			SnoreCnt_ += 1;
		}
	}

	return pos;
}

/************************************************************************/
/* 関数     : proc_off													*/
/* 関数名   : いびきOFF時処理											*/
/* 引数     : int Data : 波形データ										*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2018.07.25 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
static int proc_off(int Pos)
{
	int ii;
	int loop = DATA_SIZE_APNEA - SNORE_PARAM_SIZE;
	int pos = loop;

	// ON確定している場所を特定する
	for (ii = Pos; ii<loop; ++ii) {
		if (temp_int_buf0[ii] >= SNORE_PARAM_ON_CNT) {
			SnoreFlg_ = ON;
			SnoreCnt_ = 0;
			pos = ii;
			break;
		}
		else {
			SnoreCnt_ += 1;
			if (SnoreCnt_ >= SNORE_PARAM_NORMAL_CNT) {
				Reset();
			}
		}
	}

	return pos;
}

/************************************************************************/
/* 関数     : Save														*/
/* 関数名   : いびき時間を保存											*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2018.07.25 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
static void Save(void)
{
	int ii;

	for (ii = 1; ii<RIREKI; ++ii) {
		SnoreTime_[RIREKI - ii] = SnoreTime_[RIREKI - ii - 1];
	}
	SnoreTime_[0] = SnoreCnt_;
	SnoreCnt_ = 0;
}

/************************************************************************/
/* 関数     : Judge														*/
/* 関数名   : いびき判定												*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2018.07.25 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
static void Judge(void)
{
	int ii;

	for (ii = 0; ii<RIREKI; ++ii) {
		if (SnoreTime_[ii] == -1) {
			return;
		}
	}

	for (ii = 0; ii<RIREKI - 1; ++ii) {
		if (abs(SnoreTime_[0] - SnoreTime_[ii + 1]) > SNORE_PARAM_GOSA) {
			return;
		}
	}
	snore_ = SNORE_ON;
}

/************************************************************************/
/* 関数     : Reset														*/
/* 関数名   : 通常呼吸への復帰処理										*/
/* 引数     : なし														*/
/* 戻り値   : なし														*/
/* 変更履歴 : 2018.07.25 Axia Soft Design mmura	初版作成				*/
/************************************************************************/
/* 機能 :																*/
/************************************************************************/
/* 注意事項 :															*/
/* なし																	*/
/************************************************************************/
static void Reset(void)
{
	int ii;

	for (ii = 0; ii<RIREKI; ++ii) {
		SnoreTime_[ii] = -1;
	}
	snore_ = SNORE_OFF;
}

// DC成分除去データを取得する
DLLEXPORT void __stdcall getwav_dc(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = (double)movave_[ii];
	}
}

// 無呼吸演算データ[ave]を取得する
DLLEXPORT void __stdcall get_apnea_ave(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = ave_[ii];
	}
}

// 無呼吸演算データ[eval]を取得する
DLLEXPORT void __stdcall get_apnea_eval(int* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = (int)eval_[ii];
	}
}

// 無呼吸演算データ[rms]を取得する
DLLEXPORT void __stdcall get_apnea_rms(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = rms_[ii];
	}
}

// 無呼吸演算データ[point]を取得する
DLLEXPORT void __stdcall get_apnea_point(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = point_[ii];
	}
}

// いびき演算データ[xy2]を取得する
DLLEXPORT void __stdcall get_snore_xy2(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = xy2_[ii];
	}
}

// いびき演算データ[interval]を取得する
DLLEXPORT void __stdcall get_snore_interval(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = interval_[ii];
	}
}

// 状態を取得
DLLEXPORT int __stdcall get_state(void)
{
	int ret = 0;
	ret |= ((apnea_ << 6) & 0xC0);
	ret |= (snore_ & 0x01);
	
	return ret;
}

// 加速度センサーの値を取得
DLLEXPORT void __stdcall get_accelerometer(double data_x, double data_y, double data_z, char* ppath)
{
	// ファイル出力パスを保存
	int pos = 0;
	if (ppath) {
		while (ppath[pos] != '\0') {
			pathAcce_[pos] = ppath[pos];
			pos += 1;
		}
		pathAcce_[pos] = '\0';
	}

	debug_out("acce_x", &data_x, 1, pathAcce_);
	debug_out("acce_y", &data_y, 1, pathAcce_);
	debug_out("acce_z", &data_z, 1, pathAcce_);
}

/************************************************************/
/* END OF TEXT												*/
/************************************************************/
