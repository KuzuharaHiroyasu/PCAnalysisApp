/************************************************************************/
/* �V�X�e����   : ���ċz����											*/
/* �t�@�C����   : getwav.c												*/
/* �@�\         : input�t�@�C���̓ǂݍ���								*/
/* �ύX����     : 2017.07.12 Axia Soft Design mmura	���ō쐬			*/
/* ���ӎ���     : �Ȃ�                                                  */
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
extern int	peak_modify	( const double in_data[] , int in_res[] , double ot_data[] , double ot_hz[] , int size , double delta, double th);	/* �� */
extern void peak_vallay	( const double in[] , int    ot[] , int size, int width , int peak );

/************************************************************/
/* �v���g�^�C�v�錾											*/
/************************************************************/
void getwav_pp(const double* pData, int DSize, double Param1);
void getwav_apnea(const double* pData, int DSize, int Param1, double Param2, double Param3, double Param4);
void getwav_snore(const double* pData, int DSize, double Param);

DLLEXPORT void __stdcall getwav_init(int* pdata, int len, char* ppath, int* psnore);
DLLEXPORT int	__stdcall get_result_snore(double* data);
DLLEXPORT int	__stdcall get_result_peak(double* data);
DLLEXPORT int	__stdcall get_result_apnea(double* data);
DLLEXPORT int   __stdcall get_result_snore_count();

/************************************************************/
/* �}�N��													*/
/************************************************************/
#define DATA_SIZE		(200)	// 10�b�ԁA50ms��1��f�[�^�擾������
#define BUF_SIZE		(256)	// DATA_SIZE + �\��
#define BUF_SIZE_APNEA	(20)	// ���ċz�E��ċz�̌��ʂ͐��f�[�^��100����1

/************************************************************/
/* �^��`													*/
/************************************************************/


/************************************************************/
/* �q�`�l��`												*/
/************************************************************/
double	result_peak[BUF_SIZE];			// ���ʃs�[�N�Ԋu
int		result_peak_size;
int		apnea_ = APNEA_NONE;	// �ċz���
int		snore_ = SNORE_OFF;		// ���т�

// ���Z�r���f�[�^
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

/************************************************************/
/* �q�n�l��`												*/
/************************************************************/
extern	double	testdata[200];

/************************************************************************/
/* �֐�     : getwav_init												*/
/* �֐���   : ����������												*/
/* ����     : �Ȃ�														*/
/* �߂�l   : �Ȃ�														*/
/* �ύX���� : 2017.07.12 Axia Soft Design mmura	���ō쐬				*/
/************************************************************************/
/* �@�\ :																*/
/************************************************************************/
/* ���ӎ��� :															*/
/* �Ȃ�																	*/
/************************************************************************/
DLLEXPORT void    __stdcall getwav_init(int* pdata, int len, char* ppath, int* psnore)
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
	
	// �ړ�����
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
	getwav_snore(ptest1, len, APNEA_PARAM_SNORE);
//	getwav_snore(testdata, 200, 0.0125f);
	
	// (57)
}

/************************************************************************/
/* �֐�     : getwav_pp													*/
/* �֐���   : �s�[�N�Ԋu���Z����										*/
/* ����     : �Ȃ�														*/
/* �߂�l   : �Ȃ�														*/
/* �ύX���� : 2017.07.12 Axia Soft Design mmura	���ō쐬				*/
/************************************************************************/
/* �@�\ :																*/
/************************************************************************/
/* ���ӎ��� :															*/
/* �Ȃ�																	*/
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
/* �֐�     : getwav_apnea												*/
/* �֐���   : ���ċz���Z����											*/
/* ����     : �Ȃ�														*/
/* �߂�l   : �Ȃ�														*/
/* �ύX���� : 2017.07.12 Axia Soft Design mmura	���ō쐬				*/
/************************************************************************/
/* �@�\ :																*/
/************************************************************************/
/* ���ӎ��� :															*/
/* �Ȃ�																	*/
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
	
	// (41) ... �g�p���Ă��Ȃ����ߏȗ�
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
	
	// ���S���ċz�̔���
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
/* �֐�     : getwav_snore												*/
/* �֐���   : ���т����Z����											*/
/* ����     : �Ȃ�														*/
/* �߂�l   : �Ȃ�														*/
/* �ύX���� : 2017.07.12 Axia Soft Design mmura	���ō쐬				*/
/************************************************************************/
/* �@�\ :																*/
/************************************************************************/
/* ���ӎ��� :															*/
/* �Ȃ�																	*/
/************************************************************************/
void getwav_snore(const double* pData, int DSize, double Param)
{
	// (48) = Param
	
	// ����[���т�]�����锠
	double* pbase_x_y2 = (double*)calloc(DSize, sizeof(double));
	
	// (49)
	for(int ii=0;ii<DSize;++ii){
		if(pData[ii] < Param){
			pbase_x_y2[ii] = 1;
		}else{
			pbase_x_y2[ii] = 0;
		}
	}
	
	// (51)
	int cnt=0;
	BOOL active = FALSE;
	for(int ii=0;ii<DSize;++ii){
		if((pbase_x_y2[ii] >= 1) && (active == FALSE)){
			active = TRUE;
			cnt += 1;
		}else if(pbase_x_y2[ii] < 1){
			active = FALSE;
		}else{
			// �Ȃɂ����Ȃ�
		}
	}
	int pos = 0;
	active = FALSE;
	memset(xy2_, 0x00, DSize);
	for(int ii=0;ii<DSize;++ii){
		if((pbase_x_y2[ii] >= 1) && (active == FALSE)){
			active = TRUE;
			xy2_[pos] = ii * 0.05;
			pos += 1;
		}else if(pbase_x_y2[ii] < 1){
			active = FALSE;
		}else{
			// �Ȃɂ����Ȃ�
		}
	}
	debug_out("x_y2", xy2_, cnt, path_);
	
	// (54) pinterval[1] - pinterval[intervalsize] �܂ł� �ċz�Ԋu
	int intervalsize = cnt;
	memset(interval_, 0x00, DSize);
	for(int ii=1;ii<intervalsize;++ii){
		interval_[ii] = xy2_[ii+1] - xy2_[ii];
	}
	debug_out("interval", interval_, intervalsize, path_);
	
	// (55)
	snore_ = SNORE_OFF;
	int intervalsize2 = 0;
	for(int ii=0;ii<intervalsize;++ii){
		// 3 <= x <= 5 �ȊO�̒l��0�ɂ���
		if((3.0f <= interval_[ii]) && (interval_[ii] <= 5.0f)){
			// ���̂܂�
			snore_ = SNORE_ON;
			intervalsize2 += 1;
		}else{
			interval_[ii] = 0.0f;
		}
	}
	double tmpsnore = (double)snore_;
	debug_out("snore", &tmpsnore, 1, path_);
	
	free(pbase_x_y2);
}

// DC���������f�[�^���擾����
DLLEXPORT void __stdcall getwav_dc(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = (double)movave_[ii];
	}
}

// ���ċz���Z�f�[�^[ave]���擾����
DLLEXPORT void __stdcall get_apnea_ave(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = ave_[ii];
	}
}

// ���ċz���Z�f�[�^[eval]���擾����
DLLEXPORT void __stdcall get_apnea_eval(int* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = (int)eval_[ii];
	}
}

// ���ċz���Z�f�[�^[rms]���擾����
DLLEXPORT void __stdcall get_apnea_rms(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = rms_[ii];
	}
}

// ���ċz���Z�f�[�^[point]���擾����
DLLEXPORT void __stdcall get_apnea_point(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = point_[ii];
	}
}

// ���т����Z�f�[�^[xy2]���擾����
DLLEXPORT void __stdcall get_snore_xy2(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = xy2_[ii];
	}
}

// ���т����Z�f�[�^[interval]���擾����
DLLEXPORT void __stdcall get_snore_interval(double* pdata)
{
	for(int ii=0;ii<DATA_SIZE;++ii){
		pdata[ii] = interval_[ii];
	}
}

// ��Ԃ��擾
DLLEXPORT int __stdcall get_state(void)
{
	int ret = 0;
	ret |= ((apnea_ << 6) & 0xC0);
	ret |= (snore_ & 0x01);
	
	return ret;
}

/************************************************************/
/* END OF TEXT												*/
/************************************************************/
