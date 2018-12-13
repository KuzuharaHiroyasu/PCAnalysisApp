using System;

namespace SleepCheckerApp
{
    class SoundAlarm
    {
        // アラーム
        private System.Media.SoundPlayer player = null;
        private string SoundFile = SOUND_1000HZ; //デフォルト
        private Boolean playflg = false;

        private const string SOUND_1000HZ = "1000Hz.wav";
        private const string SOUND_5000HZ = "5000Hz.wav";
        private const string SOUND_10000HZ = "10000Hz.wav";

        public FormMain form;

        enum alarmIndex
        {
            SOUND_1000HZ = 0,
            SOUND_5000HZ,
            SOUND_10000HZ,
        }

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
                player.PlayLooping();
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

        /************************************************************************/
        /* 関数名   : alarmComboboxTextChanged                                  */
        /* 機能     : 選択(変更)されたアラーム音を再生する                      */
        /* 引数     : [int] index - コンボボックス(アラーム音)のアイテム番号    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void alarmComboboxTextChanged(int index)
        {
            if (player != null)
            {
                if (playflg)
                {//再生中なら停止
                    player.Stop();
                }
                // 一度破棄
                player.Dispose();
                player = null;
            }

            // アラーム音セット
            switch (index)
            {
                case (int)alarmIndex.SOUND_1000HZ:
                    SoundFile = SOUND_1000HZ;
                    break;
                case (int)alarmIndex.SOUND_5000HZ:
                    SoundFile = SOUND_5000HZ;
                    break;
                case (int)alarmIndex.SOUND_10000HZ:
                    SoundFile = SOUND_10000HZ;
                    break;
                default:
                    SoundFile = SOUND_1000HZ;
                    break;
            }

            // インスタンス生成
            player = new System.Media.SoundPlayer(SoundFile);

            if (player != null)
            {
                if (playflg)
                {
                    // アラーム再生
                    player.PlayLooping();
                }
            }
        }
    }
}
