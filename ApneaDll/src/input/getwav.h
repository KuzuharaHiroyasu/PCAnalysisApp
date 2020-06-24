/************************************************************************/
/* システム名   : 無呼吸判定											*/
/* ファイル名   : getwav.h												*/
/* 機能         : inputファイルの読み込み								*/
/* 変更履歴     : 2017.07.12 Axia Soft Design mmura	初版作成			*/
/* 注意事項     : なし                                                  */
/************************************************************************/

#ifdef __cplusplus
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#define DLLEXPORT __declspec(dllexport)
#endif

#ifndef		_GETWAV_H_			/* 二重定義防止 */
#define		_GETWAV_H_

#include "../sys.h"


/************************************************************/
/* マクロ													*/
/************************************************************/
#define DATA_SIZE_APNEA		(100)
#define SNORE_PARAM_SIZE	(5)
//#define SNORE_PARAM_THRE	(256)
#define SNORE_PARAM_THRE	(350)

// 無呼吸判定結果
#define APNEA_NORMAL	0	// 異常なし
#define APNEA_WARN		1	// 無呼吸区間あり
#define APNEA_ERROR		2	// 完全無呼吸
#define APNEA_NONE		3	// 判定エラー

#define APNEA_CONT_POINT	10	// APNEA_PARAM_BIN_THREを超えていない連続データ数

// いびき判定結果
#define SNORE_OFF		0	// いびきなし
#define SNORE_ON		1	// いびきあり

// OFF判定回数
#define SNORE_PARAM_OFF_CNT				(0)
// ON判定回数
#define SNORE_PARAM_ON_CNT				(4)
// いびきあり -> なしへの判定回数
//#define SNORE_PARAM_NORMAL_CNT			(290)
#define SNORE_PARAM_NORMAL_CNT			(80)
// 許容誤差 0.4s
#define SNORE_PARAM_GOSA				(8)
// 無呼吸判定カウント
#define APNEA_JUDGE_CNT					(0)

/************************************************************/
/* 型定義													*/
/************************************************************/

/************************************************************/
/* 外部参照宣言												*/
/************************************************************/
// 演算実行
DLLEXPORT void __stdcall getwav_init(int* data, int len, char* ppath, int* psnore, int* photo);
// DC成分除去データ
DLLEXPORT void __stdcall getwav_movave(double* pdata);

// 無呼吸演算
DLLEXPORT void __stdcall get_apnea_ave(double* pdata);
DLLEXPORT void __stdcall get_apnea_eval(int* pdata);
DLLEXPORT void __stdcall get_apnea_rms(double* pdata);
DLLEXPORT void __stdcall get_apnea_point(double* pdata);
// いびき演算
DLLEXPORT void __stdcall get_snore_xy2(double* pdata);
DLLEXPORT void __stdcall get_snore_interval(double* pdata);
// 結果
DLLEXPORT int  __stdcall get_state(void);
// 心拍除去後の呼吸データ
DLLEXPORT void __stdcall getwav_heartbeat_remov_dc(double* pdata);
DLLEXPORT void __stdcall getwav_dc(double* pdata);
#endif

/************************************************************/
/* END OF TEXT												*/
/************************************************************/

