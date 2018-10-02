/************************************************************************/
/* �V�X�e����   : ���ċz����											*/
/* �t�@�C����   : getwav.h												*/
/* �@�\         : input�t�@�C���̓ǂݍ���								*/
/* �ύX����     : 2017.07.12 Axia Soft Design mmura	���ō쐬			*/
/* ���ӎ���     : �Ȃ�                                                  */
/************************************************************************/

#ifdef __cplusplus
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#define DLLEXPORT __declspec(dllexport)
#endif

#ifndef		_GETWAV_H_			/* ��d��`�h�~ */
#define		_GETWAV_H_

#include "../sys.h"


/************************************************************/
/* �}�N��													*/
/************************************************************/
// ���ċz���茋��
#define APNEA_NORMAL	0	// �ُ�Ȃ�
#define APNEA_WARN		1	// ���ċz��Ԃ���
#define APNEA_ERROR		2	// ���S���ċz
#define APNEA_NONE		3	// ����G���[

// ���т����茋��
#define SNORE_OFF		0	// ���т��Ȃ�
#define SNORE_ON		1	// ���т�����


/************************************************************/
/* �^��`													*/
/************************************************************/

/************************************************************/
/* �O���Q�Ɛ錾												*/
/************************************************************/
// ���Z���s
DLLEXPORT void __stdcall getwav_init(int* data, int len, char* ppath, int* psnore);
// DC���������f�[�^
DLLEXPORT void __stdcall getwav_dc(double* pdata);

// ���ċz���Z
DLLEXPORT void __stdcall get_apnea_ave(double* pdata);
DLLEXPORT void __stdcall get_apnea_eval(int* pdata);
DLLEXPORT void __stdcall get_apnea_rms(double* pdata);
DLLEXPORT void __stdcall get_apnea_point(double* pdata);
// ���т����Z
DLLEXPORT void __stdcall get_snore_xy2(double* pdata);
DLLEXPORT void __stdcall get_snore_interval(double* pdata);
// ����
DLLEXPORT int  __stdcall get_state(void);
#endif

/************************************************************/
/* END OF TEXT												*/
/************************************************************/

