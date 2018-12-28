using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepCheckerApp
{
    class Vibration
    {
        public FormMain form;
        public LattePanda panda;

        /************************************************************************/
        /* 関数名   : confirmVib          		                                */
        /* 機能     : バイブ機能確認                                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void confirmVib(byte request)
        {
            if (form.checkBox_vib_snore.Checked && form.snore == 1)
            {// いびき判定ON
                form.log_output("[Vibration]snore");
                panda.requestLattepanda(request);
            } else if (form.checkBox_vib_apnea.Checked && form.apnea == 2)
            {// 無呼吸判定ON
                form.log_output("[Vibration]apnea");
                panda.requestLattepanda(request);
            }
        }
    }
}
