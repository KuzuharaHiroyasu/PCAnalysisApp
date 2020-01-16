using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepCheckerApp
{
    public class RCV_COMMAND
    {
        public enum Rcv_command
        {
            RCV_COM_MODE_MONITORING              = 10,  // モニタリングモード
            RCV_COM_MODE_SUPPRESS_SNORE,                // 抑制モード（いびき）
            RCV_COM_MODE_SUPPRESS_APNEA,                // 抑制モード（無呼吸）
            RCV_COM_MODE_SUPPRESS_SNORE_APNEA,          // 抑制モード（いびき＋無呼吸）
            RCV_COM_SNORE_SENS_WEAK              = 20,  // いびき検出感度（弱）
            RCV_COM_SNORE_SENS_MED,                     // いびき検出感度（中）
            RCV_COM_SNORE_SENS_STRONG,                  // いびき検出感度（強）
            RCV_COM_VIB_WEAK                     = 30,  // バイブの強さ(弱)
            RCV_COM_VIB_MED,                            // バイブの強さ(中)
            RCV_COM_VIB_STRONG,                         // バイブの強さ(強)
            RCV_COM_VIB_GRAD,                           // バイブの強さ(徐々に強く)
            RCV_COM_VIB_WEAK_CONF                = 40,  // バイブの強さ(弱)
            RCV_COM_VIB_MED_CONF,                       // バイブの強さ(中)
            RCV_COM_VIB_STRONG_CONF,                    // バイブの強さ(強)
            RCV_COM_VIB_GRAD_CONF,                      // バイブの強さ(徐々に強く)	
            RCV_COM_SNORE_SUPPRESS_TIME_FIVE     = 50,  // いびき抑制の連続時間（5分）
            RCV_COM_SNORE_SUPPRESS_TIME_TEN,            // いびき抑制の連続時間（10分）
            RCV_COM_SNORE_SUPPRESS_TIME_NON,            // いびき抑制の連続時間（設定しない）
            RCV_COM_SUPPRESS_START_TIME          = 60,  // バイブ抑制開始時間変更
            RCV_COM_MAX,							    // 最大
        }
    }
}
