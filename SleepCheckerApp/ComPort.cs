using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace SleepCheckerApp
{
    class ComPort
    {
        public SerialPort myPort = null;
        private Thread receiveThread = null;

        public String PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }

        /************************************************************************/
        /* 関数名   : ComPort     	        	                                */
        /* 機能     : コンストラクタ                                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし                                                      */
        /************************************************************************/
        public ComPort()
        {
        }

        /************************************************************************/
        /* 関数名   : Start             		                                */
        /* 機能     : COM接続　                                                 */
        /* 引数     : なし                                                      */
        /* 戻り値   : [Boolean] 成功 - true                                     */
        /*          : [Boolean] 失敗 - false                  					*/
        /************************************************************************/
        public Boolean Start()
        {
            if(PortName != "")
            {
                myPort = new SerialPort(
                     PortName, BaudRate, Parity, DataBits, StopBits);
            }

            try
            {
                myPort.Open();
            }
            catch
            {
//                System.Windows.Forms.MessageBox.Show(PortName + "は使用できません。");

                return false;
            }
            receiveThread = new Thread(ComPort.ReceiveWork);
            receiveThread.Start(this);
            return true;
        }

        /************************************************************************/
        /* 関数名   : Close             		                                */
        /* 機能     : COM切断　                                                 */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし                                                      */
        /************************************************************************/
        public void Close()
        {
            if (receiveThread != null && myPort != null)
            {
                myPort.Close();
                //receiveThread.Join();     // これだけだとブロックし続けて終わらない
                receiveThread.Join(1000);   // 1秒終了を待機して
                receiveThread.Abort();      // スレッドを強制終了
                receiveThread = null;
            }
        }

        /************************************************************************/
        /* 関数名   : ReceiveWork          		                                */
        /* 機能     : データ受信                                                */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし                                                      */
        /************************************************************************/
        public static void ReceiveWork(object target)
        {
            ComPort my = target as ComPort;
            my.ReceiveData();
        }

        /************************************************************************/
        /* 関数名   : WriteData          		                                */
        /* 機能     : ポートに書き込み                                          */
        /* 引数     : [byte[]] buffer - 書き込むデータ                          */
        /* 戻り値   : なし                                                      */
        /************************************************************************/
        public void WriteData(byte[] buffer)
        {
            myPort.Write(buffer, 0, buffer.Length);
        }

        public delegate void DataReceivedHandler(byte[] data);
        public event DataReceivedHandler DataReceived;

        /************************************************************************/
        /* 関数名   : ReceiveData          		                                */
        /* 機能     : データ受信                                                */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし                                                      */
        /************************************************************************/
        public void ReceiveData()
        {
            if (myPort == null)
            {
                return;
            }
            do
            {
                try
                {
//                    int rbyte = myPort.BytesToRead;
                    int rbyte = 200;    // 500バイトまとめて取得する
                    byte[] buffer = new byte[rbyte];
                    int read = 0;
                    while (read < rbyte)
                    {
                        int length = myPort.Read(buffer, read, rbyte - read);
                        read += length;
                    }
                    if (rbyte > 0)
                    {
                        DataReceived(buffer);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (myPort.IsOpen);
        }

        /************************************************************************/
        /* 関数名   : GetPortNames         		                                */
        /* 機能     : ポート番号取得                                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : [string[]] ポート番号                                     */
        /************************************************************************/
        public string[] GetPortNames()
        {
             return System.IO.Ports.SerialPort.GetPortNames();
        }
    }
}
