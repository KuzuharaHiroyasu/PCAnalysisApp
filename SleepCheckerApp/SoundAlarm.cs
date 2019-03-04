﻿using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace SleepCheckerApp
{
    class SoundAlarm
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint
        GetPrivateProfileString(string lpAppName,
        string lpKeyName, string lpDefault,
        StringBuilder lpReturnedString, uint nSize,
        string lpFileName);

        // アラーム
        private System.Media.SoundPlayer player = null;
        private string filePath = AppDomain.CurrentDomain.BaseDirectory;
        private string AlarmFile = ""; //デフォルト

        public FormMain form;

        /************************************************************************/
        /* 関数名   : SoundAlarm          		                                */
        /* 機能     : コンストラクタ                                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public SoundAlarm()
        {
        }

        /************************************************************************/
        /* 関数名   : setInitAlarm          		                            */
        /* 機能     : アラーム初期設定                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void setInitAlarm(string[] alarmFilesPath)
        {
            // iniファイルからアラームファイル取得
            // iniファイル名を決める（実行ファイルが置かれたフォルダと同じ場所）
            string iniFileName = filePath + "setting.ini";

            // iniファイルから文字列を取得
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(
                "SECTION",
                "KEY",
                alarmFilesPath[0],   // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                iniFileName);

            AlarmFile = Path.GetFileName(sb.ToString());

            // チェックが付いていたらインスタンス生成
            if (form.checkBox_alarm_apnea.Checked || form.checkBox_alarm_snore.Checked)
            {
                foreach(string alarmFile in alarmFilesPath)
                {
                    if(AlarmFile.Equals(alarmFile, StringComparison.Ordinal))
                    {// ファイルが存在する
                        player = new System.Media.SoundPlayer(filePath + AlarmFile);
                    }
                }
            }
        }

        /************************************************************************/
        /* 関数名   : setAlarm               		                            */
        /* 機能     : アラーム設定                                              */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void setAlarm()
        {
            //コンボボックスに表示のファイルを設定する
            player = new System.Media.SoundPlayer(filePath + form.comboBoxAlarm.SelectedItem);
        }

        /************************************************************************/
        /* 関数名   : playAlarm          		                                */
        /* 機能     : アラーム再生                                              */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void playAlarm()
        {
            if (player != null)
            {
                form.log_output("playAlarm");
                player.Play();
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
            if (player != null)
            {
                player.Stop();
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
                if (form.checkBox_alarm_snore.Checked && form.snore == 1)
                {// いびき判定ON
                    playAlarm();
                }
                else if (form.checkBox_alarm_apnea.Checked && form.apnea == 2)
                {// 無呼吸判定ON
                    playAlarm();
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
            if (form.checkBox_alarm_snore.Checked)
            {// チェックON
                if (player == null)
                {// インスタンス未生成の場合生成
                    setAlarm();
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
            if (form.checkBox_alarm_apnea.Checked)
            {// チェックON
                if (player == null)
                {// インスタンス未生成の場合生成
                    setAlarm();
                }
            }
        }

        /************************************************************************/
        /* 関数名   : apneaCheckedChanged   	                                */
        /* 機能     : 無呼吸チェック時のアラーム処理                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void changeAlarmContents()
        {
            if(player != null)
            {
                stopAlarm();
                setAlarm();
            }
        }

        /************************************************************************/
        /* 関数名   : getAlarmFile            	                                */
        /* 機能     : アラーム音のファイル名を取得する                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public string getAlarmFile()
        {
            return AlarmFile;
        }
    }
}
