using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace SleepCheckerApp
{
    class ComPort
    {
        private SerialPort myPort = null;
        private Thread receiveThread = null;

        public String PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }

        public ComPort()
        {
        }

        public Boolean Start()
        {
            myPort = new SerialPort(
                 PortName, BaudRate, Parity, DataBits, StopBits);
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

        public static void ReceiveWork(object target)
        {
            ComPort my = target as ComPort;
            my.ReceiveData();
        }

        public void WriteData(byte[] buffer)
        {
            myPort.Write(buffer, 0, buffer.Length);
        }

        public delegate void DataReceivedHandler(byte[] data);
        public event DataReceivedHandler DataReceived;

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
                    int rbyte = 500;    // 500バイトまとめて取得する
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

        public void Close()
        {
            if (receiveThread != null && myPort != null)
            {
                myPort.Close();
                //receiveThread.Join();     // これだけだとブロックし続けて終わらない
                receiveThread.Join(1000);   // 1秒終了を待機して
                receiveThread.Abort();      // スレッドを強制終了
            }
        }
        public string[] GetPortNames()
        {
             return System.IO.Ports.SerialPort.GetPortNames();
        }
    }

}
