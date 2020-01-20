using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SleepCheckerApp
{
    public partial class FormSetting : Form
    {
        public FormMain form;

        private byte[] param = new byte[1];

        private int mode = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE;
        private int snoreSens = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_MED;
        private int vib = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_MED;

        public FormSetting()
        {
            InitializeComponent();

            // 現在の設定にチェック
            modeInitCheck();        // 動作モードの初期チェック

            snoreSensInitCheck();   // いびき検出感度の初期チェック

            vibInitCheck();         // バイブの強さの初期チェック
        }

        /************************************************************************/
        /* 関数名   : radioButtonMode_CheckedChanged        		    		*/
        /* 機能     : 動作モードのチェック変更時のイベント                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void radioButtonMode_CheckedChanged(object sender, EventArgs e)
        {
/*
            if( radioButtonMonitor.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_MONITORING;
            }
            else if( radioButtonSnore.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE;
            }
            else if( radioButtonApnea.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_APNEA;
            }
            else if( radioButtonSnoreApnea.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE_APNEA;
            }

            if(!form.writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("設定できませんでした。");
            }
*/
        }

        /************************************************************************/
        /* 関数名   : radioButtonSnoreDetect_CheckedChanged                		*/
        /* 機能     : いびき検出感度のチェック変更時のイベント                  */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void radioButtonSnoreDetect_CheckedChanged(object sender, EventArgs e)
        {
/*
            if( radioButtonSnoreDetectWeak.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_WEAK;
            }
            else if( radioButtonSnoreDetectMed.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_MED;
            }
            else if( radioButtonSnoreDetectStrong.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_STRONG;
            }

            if (!form.writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("設定できませんでした。");
            }
*/
        }

        /************************************************************************/
        /* 関数名   : radioButtonVib_CheckedChanged                        		*/
        /* 機能     : バイブの強さのチェック変更時のイベント      　            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void radioButtonVib_CheckedChanged(object sender, EventArgs e)
        {
/*
            if( radioButtonVibWeak.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_WEAK;
            }
            else if( radioButtonVibMed.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_MED;
            }
            else if( radioButtonVibStrong.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_STRONG;
            }
            else if( radioButtonVibGrad.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_GRAD;
            }

            if (!form.writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("設定できませんでした。");
            }
*/
        }

        /************************************************************************/
        /* 関数名   : buttonClose_Click                                   		*/
        /* 機能     : 閉じるボタンクリック時のイベント            　            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void buttonClose_Click(object sender, EventArgs e)
        {
            // 動作モード
            if (radioButtonMonitor.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_MONITORING;
            }
            else if (radioButtonSnore.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE;
            }
            else if (radioButtonApnea.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_APNEA;
            }
            else if (radioButtonSnoreApnea.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE_APNEA;
            }

            if (form.writeData(param))
            {
                mode = param[0];
            }

            // いびき検出感度
            if (radioButtonSnoreDetectWeak.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_WEAK;
            }
            else if (radioButtonSnoreDetectMed.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_MED;
            }
            else if (radioButtonSnoreDetectStrong.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_STRONG;
            }

            if (form.writeData(param))
            {
                snoreSens = param[0];
            }

            // バイブレーションの強さ
            if (radioButtonVibWeak.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_WEAK;
            }
            else if (radioButtonVibMed.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_MED;
            }
            else if (radioButtonVibStrong.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_STRONG;
            }
            else if (radioButtonVibGrad.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_GRAD;
            }

            if (form.writeData(param))
            {
                vib = param[0];
            }

            this.Hide();
        }

        /************************************************************************/
        /* 関数名   : modeInitCheck                                       		*/
        /* 機能     : 動作モードのチェック          　            　            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void modeInitCheck()
        {
            if( mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_MONITORING)
            {
                radioButtonMonitor.Checked = true;
            }
            else if( mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE)
            {
                radioButtonSnore.Checked = true;
            }
            else if (mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_APNEA)
            {
                radioButtonApnea.Checked = true;
            }
            else if (mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE_APNEA)
            {
                radioButtonSnoreApnea.Checked = true;
            }
        }

        /************************************************************************/
        /* 関数名   : snoreSensInitCheck                                   		*/
        /* 機能     : いびき検出感度のチェック      　            　            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void snoreSensInitCheck()
        {
            if (snoreSens == (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_WEAK)
            {
                radioButtonSnoreDetectWeak.Checked = true;
            }
            else if (snoreSens == (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_MED)
            {
                radioButtonSnoreDetectMed.Checked = true;
            }
            else if (snoreSens == (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_STRONG)
            {
                radioButtonSnoreDetectStrong.Checked = true;
            }
        }

        /************************************************************************/
        /* 関数名   : vibInitCheck                                       		*/
        /* 機能     : バイブの強さのチェック        　            　            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void vibInitCheck()
        {
            if (mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_WEAK)
            {
                radioButtonVibWeak.Checked = true;
            }
            else if (mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_MED)
            {
                radioButtonVibMed.Checked = true;
            }
            else if (mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_STRONG)
            {
                radioButtonVibStrong.Checked = true;
            }
            else if (mode == (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_GRAD)
            {
                radioButtonVibGrad.Checked = true;
            }
        }
    }
}
