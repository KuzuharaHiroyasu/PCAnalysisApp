﻿using System.IO.Ports;

namespace SleepCheckerApp
{
    class LattePanda
    {
        private ComPort com = null;
        private bool LATTEPANDA = true;

        /************************************************************************/
        /* 関数名   : setComPort_Lattepanda        		                		*/
        /* 機能     : ラテパンダのCOMポート接続                                 */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void setLattepandaFuncSetting(bool setting)
        {
            LATTEPANDA = setting;
        }

        /************************************************************************/
        /* 関数名   : setComPort_Lattepanda        		                		*/
        /* 機能     : ラテパンダのCOMポート接続                                 */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void setComPort_Lattepanda()
        {
            if (LATTEPANDA)
            {
                com = new ComPort();
                com.PortName = "COM5"; //固定
                com.BaudRate = 9600;
                com.Parity = Parity.Even;
                com.DataBits = 8;
                com.StopBits = StopBits.One;

                comSerch();
                com.Start();
            }
        }

        /************************************************************************/
        /* 関数名   : closeComPort_Lattepanda      		                		*/
        /* 機能     : ラテパンダのCOMポート切断                                 */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void closeComPort_Lattepanda()
        {
            if (LATTEPANDA)
            {
                com.Close();
            }
        }

        /************************************************************************/
        /* 関数名   : requestLattepanda          		                		*/
        /* 機能     : ラテパンダにコマンドを送信する                            */
        /* 引数     : [byte] pattern - コマンド                                 */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void requestLattepanda(byte pattern)
        {
            if (LATTEPANDA)
            {
                byte[] param = new byte[1];
                param[0] = pattern;
                com.WriteData(param);
            }
        }

        /************************************************************************/
        /* 関数名   : comSerch          		                          		*/
        /* 機能     : ラテパンダと接続するCOM5を検索する                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void comSerch()
        {
            do
            {
                string[] ports = com.GetPortNames();
                foreach (string port in ports)
                {
                    if (port == "COM5")
                    {
                        return;
                    }
                }
            } while (true);
        }
    }
}
