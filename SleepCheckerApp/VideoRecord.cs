using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.IO;

namespace SleepCheckerApp
{
    class VideoRecord
    {
        private double fps = 30;
        private int width = 705;
        private int height = 590;
        public FormMain form;
        BackgroundWorker worker;
        CvVideoWriter video;

        /************************************************************************/
        /* 関数名   : VideoRecord          		                                */
        /* 機能     : コンストラクタ                                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public VideoRecord()
        {
            worker = new BackgroundWorker();

            // 非同期をキャンセルさせる
            worker.WorkerSupportsCancellation = true;

            // ProgressChangedイベントを発生させるようにする
            worker.WorkerReportsProgress = true;

            // ReportProgressメソッドで呼ばれるProgressChangedのイベントハンドラを追加
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);

            // RunWorkerAsyncメソッドで呼ばれるDoWorkに、
            // 別スレッドでUSBカメラの画像を取得し続けるイベントハンドラを追加
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        /************************************************************************/
        /* 関数名   : videoStart  		                                        */
        /* 機能     : 録画開始                                                  */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void videoStart()
        {
            // .aviファイルを開く
            video = new CvVideoWriter(getFilePath(), FourCC.MJPG, fps, new CvSize(width, height));

            // DoWorkイベントハンドラの実行を開始
            worker.RunWorkerAsync();
        }

        /************************************************************************/
        /* 関数名   : videoStop  		                                        */
        /* 機能     : 録画停止                                                  */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void videoStop()
        {
            // 動画ファイルを閉じる
            video.Dispose();
        }

        /************************************************************************/
        /* 関数名   : worker_DoWork          		                            */
        /* 機能     : カメラからの映像を受け取る                                */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // カメラからの映像を受け取る
            using (var capture = Cv.CreateCameraCapture(CaptureDevice.Any))
            {
                IplImage frame;
                while (true)
                {
                    frame = Cv.QueryFrame(capture);

                    // 新しい画像を取得したので、
                    // ReportProgressメソッドを使って、ProgressChangedイベントを発生させる
                    worker.ReportProgress(0, frame);
                }
            }
        }

        /************************************************************************/
        /* 関数名   : worker_ProgressChanged  		                            */
        /* 機能     : 動画ファイルに書き込み                                    */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  frameがe.UserStateプロパティにセットされて渡されてくる
            IplImage image = (IplImage)e.UserState;

            CvSize size = new CvSize(width, height);
            IplImage reImage = new IplImage(size, image.Depth, image.NChannels);

            Cv.Resize(image, reImage, Interpolation.NearestNeighbor);

            // 動画ファイルに書き込み
            if (!video.IsDisposed)
            {
                video.WriteFrame(reImage);
            }

            form.videoWriteFrame(reImage);
        }

        /************************************************************************/
        /* 関数名   : getFilePath                                               */
        /* 機能     : 録画保存先パスを取得                                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : [string] filePath - ファイルパス             				*/
        /************************************************************************/
        private string getFilePath()
        {
            string fileName = "video";
            string filePath = form.getVideoFilePath();
            string temp;

            temp = fileName + ".avi";
            for (int i = 1; i < 20; i++)
            {
                if (File.Exists(filePath + temp))
                {
                    temp = fileName + "(" + i + ")" + ".avi";
                }
                else
                {
                    fileName = temp;
                    break;
                }
            }
            filePath = filePath + fileName;

            return filePath;
        }
    }
}
