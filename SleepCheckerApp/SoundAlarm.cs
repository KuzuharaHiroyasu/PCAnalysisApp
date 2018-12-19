using System;

namespace SleepCheckerApp
{
    class SoundAlarm
    {
        // アラーム
        private const string ALARM_SOUND = "1000Hz_intermittent.wav";

        private System.Media.SoundPlayer player = null;
        private string SoundFile = ALARM_SOUND; //デフォルト
        private Boolean playflg = false;

        public FormMain form;

        /************************************************************************/
        /* 関数名   : SoundAlarm          		                                */
        /* 機能     : コンストラクタ                                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public SoundAlarm()
        {
            player = new System.Media.SoundPlayer(SoundFile);
        }

        /************************************************************************/
        /* 関数名   : playAlarm          		                                */
        /* 機能     : アラーム再生                                              */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void playAlarm()
        {
            if (!playflg)
            {// 再生中ではない
                player.Play();
                playflg = true;
            }
        }

        /************************************************************************/
        /* 関数名   : stopAlarm          		                                */
        /* 機能     : アラーム停止                                              */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void stopAlarm()
        {
            if (playflg)
            {// 再生中
                player.Stop();
                playflg = false;
            }
        }

        /************************************************************************/
        /* 関数名   : playAlarm          		                                */
        /* 機能     : アラーム機能確認                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void confirmAlarm()
        {
            if (player != null)
            {
                //アラームON
                if (form.radio_alarmOn.Checked)
                {
                    if (form.checkBox_snore.Checked && form.snore == 1)
                    {// いびき判定ON
                        playAlarm();
                    }
                    else if (form.checkBox_apnea.Checked && form.apnea == 2)
                    {// 無呼吸判定ON
                        playAlarm();
                    }
                    else
                    {// どちらもOFF
                        stopAlarm();
                    }
                }
            }
        }

        /************************************************************************/
        /* 関数名   : alarmOffStatusChanged  	                                */
        /* 機能     : アラームOFF変更時のアラーム処理                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void alarmOffStatusChanged()
        {
            if (player != null)
            {//アラームON
                if (form.radio_alarmOn.Checked)
                {
                    if (form.checkBox_snore.Checked && form.snore == 1)
                    {// いびき判定ON
                        playAlarm();
                    }
                    else if (form.checkBox_apnea.Checked && form.apnea == 2)
                    {// 無呼吸判定ON
                        playAlarm();
                    }
                    else
                    {// どちらもOFF
                        stopAlarm();
                    }
                }
                else if (form.radio_alarmOff.Checked)
                {//アラームOFF
                    stopAlarm();
                }
            }
        }

        /************************************************************************/
        /* 関数名   : snoreCheckedChanged  		                                */
        /* 機能     : いびきチェック時のアラーム処理                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void snoreCheckedChanged()
        {
            if (player != null)
            {//アラームON
                if (form.radio_alarmOn.Checked)
                {
                    if (form.checkBox_snore.Checked && form.snore == 1)
                    {// いびき判定ON
                        playAlarm();
                    }
                    else if (!form.checkBox_apnea.Checked || form.apnea != 2)
                    {// どちらもOFF
                        stopAlarm();
                    }
                }
                else if (form.radio_alarmOff.Checked)
                {//アラームOFF
                    stopAlarm();
                }
            }
        }

        /************************************************************************/
        /* 関数名   : apneaCheckedChanged   	                                */
        /* 機能     : 無呼吸チェック時のアラーム処理                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void apneaCheckedChanged()
        {
            if (player != null)
            {//アラームON
                if (form.radio_alarmOn.Checked)
                {
                    if (form.checkBox_apnea.Checked && form.apnea == 2)
                    {// 無呼吸判定ON
                        playAlarm();
                    }
                    else if (!form.checkBox_snore.Checked || form.snore != 1)
                    {// どちらもOFF
                        stopAlarm();
                    }
                }
                else if (form.radio_alarmOff.Checked)
                {//アラームOFF
                    stopAlarm();
                }
            }
        }
    }
}
