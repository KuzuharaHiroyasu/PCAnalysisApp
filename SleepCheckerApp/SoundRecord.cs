using System;
using System.IO;
using NAudio.Wave;

namespace SleepCheckerApp
{
    class SoundRecord
    {
        // 音声録音
        private WaveIn sourceStream = null;
        private WaveFileWriter waveWriter = null;
        public FormMain form;

        public Boolean startRecordApnea()
        {
            WaveInCapabilities capabilities;
            int deviceNumber;
            Boolean ret = false;
            string fileName = "record_snore";
            string temp;
            string filePath = form.getRecordFilePath();

            for (deviceNumber = 0; deviceNumber < WaveIn.DeviceCount; deviceNumber++)
            {
                capabilities = WaveIn.GetCapabilities(deviceNumber);
                if (capabilities.ProductName.Contains("マイク") || capabilities.ProductName.Contains("Microphone"))
                {
                    ret = true;
                    break;
                }
            }

            if (!ret)
            { //マイクが見つからない
                form.log_output("[ERROR]No mic.");
                return ret;
            }
            
            // waveIn Select Recording Device
            sourceStream = new WaveIn();
            sourceStream.DeviceNumber = deviceNumber;
            sourceStream.WaveFormat = new WaveFormat(4000, WaveIn.GetCapabilities(deviceNumber).Channels);

            // 録音のコールバックkな数
            sourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(sourceStream_DataAvailable);

            temp = fileName + ".wav";
            for (int i = 1; i < 20; i++)
            {
                if (File.Exists(filePath + temp))
                {
                    temp = fileName + "(" + i + ")" + ".wav";
                }
                else
                {
                    fileName = temp;
                    break;
                }
            }
            // wave出力
            waveWriter = new WaveFileWriter(filePath + fileName, sourceStream.WaveFormat);

            // 録音開始
            sourceStream.StartRecording();

            form.log_output("[START]Record");

            return ret;
        }

        public void sourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveWriter == null) return;

            waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
            waveWriter.Flush();
        }

        public void stopRecordApnea()
        {
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
                form.log_output("[STOP]Record");
            }

            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }
        }
    }
}