#define USB_OUTPUT      // 無効化するとCドライブ直下に出力する
#define C_DRIVE         // Cドライブ直下にログ出力
#define AUTO_ANALYSIS   // 解析自動スタート
#define SOUND_RECORD    // 音声録音

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using System.Management;

namespace SleepCheckerApp
{
    public partial class FormMain : Form
    {
        // For Apnea
        [DllImport("Apnea.dll")]
        static extern void getwav_init(IntPtr data, int len, IntPtr path, IntPtr snore);
        [DllImport("Apnea.dll")]
        static extern void getwav_dc(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void get_apnea_ave(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void get_apnea_eval(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void get_apnea_rms(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void get_apnea_point(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void get_snore_xy2(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void get_snore_interval(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern int get_state();
        [DllImport("Apnea.dll")]
        static extern void get_accelerometer(double data1, double data2, double data3, IntPtr path);
        [DllImport("Apnea.dll")]
        static extern void calc_snore_init();
        [DllImport("Apnea.dll")]
        static extern void get_general_result(double snore, double apnea, IntPtr path);

        // For PulseOximeter
        [DllImport("PulseOximeter.dll")]
        static extern void calculator_clr(IntPtr data, int len, IntPtr path);
        [DllImport("PulseOximeter.dll")]
        static extern void calculator_inf(IntPtr data, int len, IntPtr path);
        [DllImport("PulseOximeter.dll")]
        static extern int get_dc(IntPtr data);
        [DllImport("PulseOximeter.dll")]
        static extern int get_fft(IntPtr data);
        [DllImport("PulseOximeter.dll")]
        static extern int get_ifft(IntPtr data);
        [DllImport("PulseOximeter.dll")]
        static extern int get_new_ifft(IntPtr data);
        [DllImport("PulseOximeter.dll")]
        static extern int get_sinpak_clr();
        [DllImport("PulseOximeter.dll")]
        static extern int get_sinpak_inf();
        [DllImport("PulseOximeter.dll")]
        static extern int get_spo2();
        [DllImport("PulseOximeter.dll")]
        static extern double get_acdc();
        [DllImport("PulseOximeter.dll")]
        static extern double get_acavg_clr();
        [DllImport("PulseOximeter.dll")]
        static extern double get_acavg_inf();
        [DllImport("PulseOximeter.dll")]
        static extern double get_dcavg_clr();
        [DllImport("PulseOximeter.dll")]
        static extern double get_dcavg_inf();
        [DllImport("PulseOximeter.dll")]
        static extern double get_acavg_ratio();

        private ComPort com_left = null;
        private ComPort com_right = null;
        private SoundRecord record = null;
        private SoundAlarm alarm = null;
        private LattePanda panda = null;
        private Vibration vib = null;

        private const int CalcDataNumApnea = 200;           // 6秒間、50msに1回データ取得した数
        private const int CalcDataNumSpO2 = 128;            // 4秒間、50msに1回データ取得した数

        enum request
        {
            LED_OFF = 0,    //解析スタート
            LED_ERROR,      //エラー
            SET_CLOCK,      //時刻設定
            VIBRATION,      //バイブレーション
        }

        private int[] CalcData1 = new int[CalcDataNumApnea];          // 1回の演算に使用するデータ
        // 左
        private List<int> CalcDataList_left1 = new List<int>();
        private List<int> CalcDataList_left2 = new List<int>();
        // 右
        private List<int> CalcDataList_right1 = new List<int>();
        private List<int> CalcDataList_right2 = new List<int>();
        private string amari_left = "";
        private string amari_right = "";

        // グラフ用
        // 生データ
        private const int ApneaGraphDataNum = CalcDataNumApnea * 10 + 1;
        // 左
        Queue<double> RawDataRespQueue_left = new Queue<double>();
        Queue<double> RawDataSnoreQueue_left = new Queue<double>();
        Queue<double> RawDataDcQueue_left = new Queue<double>();
        // 右
        Queue<double> RawDataRespQueue_right = new Queue<double>();
        Queue<double> RawDataSnoreQueue_right = new Queue<double>();
        Queue<double> RawDataDcQueue_right = new Queue<double>();

        // 加速度センサー
        private const int Acc_RawDataRirekiNum = 120;       // 生データ履歴数 500ms * 120個 = 60秒
        Queue<double> AccelerometerXQueue = new Queue<double>();
        Queue<double> AccelerometerYQueue = new Queue<double>();
        Queue<double> AccelerometerZQueue = new Queue<double>();

        // 演算途中データ
        // 左
        Queue<double> ApneaAveQueue_left = new Queue<double>();
        Queue<int> ApneaEvalQueue_left = new Queue<int>();
        Queue<double> ApneaRmsQueue_left = new Queue<double>();
        Queue<double> ApneaPointQueue_left = new Queue<double>();
        Queue<double> SnoreXy2Queue_left = new Queue<double>();
        Queue<double> SnoreIntervalQueue_left = new Queue<double>();
        // 右
        Queue<double> ApneaAveQueue_right = new Queue<double>();
        Queue<int> ApneaEvalQueue_right = new Queue<int>();
        Queue<double> ApneaRmsQueue_right = new Queue<double>();
        Queue<double> ApneaPointQueue_right = new Queue<double>();
        Queue<double> SnoreXy2Queue_right = new Queue<double>();
        Queue<double> SnoreIntervalQueue_right = new Queue<double>();

        // 演算結果データ
        private const int BufNumApneaGraph = 11;            // 1分(10データ)分だけ表示する // 0点も打つので+1

        Queue<double> ApneaQueue = new Queue<double>();
        Queue<double> ResultIbikiQueue = new Queue<double>();

        // 左
        Queue<double> ApneaQueue_left = new Queue<double>();
        Queue<double> ResultIbikiQueue_left = new Queue<double>();
        // 右
        Queue<double> ApneaQueue_right = new Queue<double>();
        Queue<double> ResultIbikiQueue_right = new Queue<double>();
        
        object lockData = new object();
        object lockData_Acc = new object();

        // For PulseOximeter
        private List<int> SpO2_CalcDataList1 = new List<int>();
        private List<int> SpO2_CalcDataList2 = new List<int>();
        
        // グラフ用
        private const int SpO2_RawDataRirekiNum = 128;              // 生データ履歴数
        
        // 生データ
        Queue<int> RawDataSekisyokuQueue = new Queue<int>();
        Queue<int> RawDataSekigaiQueue = new Queue<int>();
        Queue<int> DcSekisyokuDataQueue = new Queue<int>();
        Queue<int> DcSekigaiDataQueue = new Queue<int>();
        
        // 演算途中データ
        Queue<double> FftSekisyokuDataQueue = new Queue<double>();
        Queue<double> FftSekigaiDataQueue = new Queue<double>();
        Queue<double> IfftSekisyokuDataQueue = new Queue<double>();
        Queue<double> IfftSekigaiDataQueue = new Queue<double>();
        Queue<double> NewIfftSekisyokuDataQueue = new Queue<double>();
        Queue<double> NewIfftSekigaiDataQueue = new Queue<double>();
        
        // 演算結果データ
        private const int BufNumSpO2Graph = 31;             // 1分(15データ)分だけ表示する // 0点も打つので+1
        private const int ShipakuDataRirekiNum = BufNumSpO2Graph;    // 心拍数データ履歴数
        private const int SpDataRirekiNum = BufNumSpO2Graph;         // SpO2データ履歴数
        Queue<double> ShinpakuSekisyokuDataQueue = new Queue<double>();
        Queue<double> ShinpakuSekigaiDataQueue = new Queue<double>();
        Queue<double> SpNormalDataQueue = new Queue<double>();
        Queue<double> SpAcdcDataQueue = new Queue<double>();
        object lockData_SpO2 = new object();

        // 演算結果保存向けデータ-------
        // 保存rootパス
        private string ApneaRootPathGeneral_;
        private string ApneaRootPathLeft_;
        private string ApneaRootPathRight_;
//        private string PulseRootPath_;
        private string AcceRootPath_;
        private string RecordRootPath_;
        private string recordFilePath;

        private int ApneaCalcCountGeneral_;
        private int ApneaCalcCountLeft_;
        private int ApneaCalcCountRight_;
//        private int PulseCalcCount_;
        private int AcceCalcCount_;

        public int snore = 0;
        public int apnea = 0;
        // 左
        public int snore_left = -1;
        public int apnea_left = -1;
        // 右
        public int snore_right = -1;
        public int apnea_right = -1;

        // 情報取得コマンド
        static ManagementObjectSearcher MyOCS = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");

        // ログ出力パス
        string logPath = "C:\\log";
        string tempPortname = "";

        public FormMain()
        {
            string icon = "analysis.ico";
            InitializeComponent();

            if(File.Exists(icon))
                this.Icon = new System.Drawing.Icon(icon);

            //CalcDataList1 = new List<int>(CalcData1);
            //CalcDataList2 = new List<int>(CalcData2);
        }

        /************************************************************************/
        /* 関数名   : Form1_Load          						    			*/
        /* 機能     : フォーム読み込み時のイベント	                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            com_left = new ComPort();
            com_right = new ComPort();
            record = new SoundRecord();
            alarm = new SoundAlarm();
            panda = new LattePanda();
            vib = new Vibration();

            record.form = this;
            alarm.form = this;
            vib.form = this;
            vib.panda = panda;

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            logPath = logPath + "\\log.txt";

            log_output("[START-UP]App");
            log_output("AlarmFile:" + alarm.getAlarmFile());

            string[] ports = com_left.GetPortNames();   //片方からのみ取得
            foreach (string port in ports)
            {
                comboBoxComport.Items.Add(port);
                //Console.WriteLine(port);
            }

            // 演算データ保存向け初期化処理
            //CreateRootDir(); //(移動)USB検索後にルート設定
            ApneaCalcCountGeneral_ = 0;
            ApneaCalcCountLeft_ = 0;
            ApneaCalcCountRight_ = 0;
//            PulseCalcCount_ = 0;
            AcceCalcCount_  = 0;

            // グラフ表示初期化
            // For Apnea
            // 左
            RawDataRespQueue_left.Clear();
            RawDataSnoreQueue_left.Clear();
            RawDataDcQueue_left.Clear();
            ApneaAveQueue_left.Clear();
            ApneaEvalQueue_left.Clear();
            ApneaRmsQueue_left.Clear();
            ApneaPointQueue_left.Clear();
            SnoreXy2Queue_left.Clear();
            SnoreIntervalQueue_left.Clear();
            // 右
            RawDataRespQueue_right.Clear();
            RawDataSnoreQueue_right.Clear();
            RawDataDcQueue_right.Clear();
            ApneaAveQueue_right.Clear();
            ApneaEvalQueue_right.Clear();
            ApneaRmsQueue_right.Clear();
            ApneaPointQueue_right.Clear();
            SnoreXy2Queue_right.Clear();
            SnoreIntervalQueue_right.Clear();

            AccelerometerXQueue.Clear();
            AccelerometerYQueue.Clear();
            AccelerometerZQueue.Clear();

            for (int i = 0; i < ApneaGraphDataNum; i++)
            {
                // 左
                RawDataRespQueue_left.Enqueue(0);
                RawDataSnoreQueue_left.Enqueue(0);
                RawDataDcQueue_left.Enqueue(0);
                ApneaAveQueue_left.Enqueue(0);
                ApneaEvalQueue_left.Enqueue(0);
                ApneaRmsQueue_left.Enqueue(0);
                ApneaPointQueue_left.Enqueue(0);
                SnoreXy2Queue_left.Enqueue(0);
                SnoreIntervalQueue_left.Enqueue(0);
                // 右
                RawDataRespQueue_right.Enqueue(0);
                RawDataSnoreQueue_right.Enqueue(0);
                RawDataDcQueue_right.Enqueue(0);
                ApneaAveQueue_right.Enqueue(0);
                ApneaEvalQueue_right.Enqueue(0);
                ApneaRmsQueue_right.Enqueue(0);
                ApneaPointQueue_right.Enqueue(0);
                SnoreXy2Queue_right.Enqueue(0);
                SnoreIntervalQueue_right.Enqueue(0);
            }

            ApneaQueue.Clear();
            ResultIbikiQueue.Clear();
            ApneaQueue_left.Clear();
            ResultIbikiQueue_left.Clear();
            ApneaQueue_right.Clear();
            ResultIbikiQueue_right.Clear();
            for (int i = 0; i < BufNumApneaGraph; i++)
            {
                ApneaQueue.Enqueue(0);
                ResultIbikiQueue.Enqueue(0);
                ApneaQueue_left.Enqueue(0);
                ResultIbikiQueue_left.Enqueue(0);
                ApneaQueue_right.Enqueue(0);
                ResultIbikiQueue_right.Enqueue(0);
            }
            // For PulseOximeter
            for (int i = 0; i < SpO2_RawDataRirekiNum; i++)
            {
                DcSekisyokuDataQueue.Enqueue(0);
                DcSekigaiDataQueue.Enqueue(0);
                FftSekisyokuDataQueue.Enqueue(0);
                FftSekigaiDataQueue.Enqueue(0);
                IfftSekisyokuDataQueue.Enqueue(0);
                IfftSekigaiDataQueue.Enqueue(0);
                NewIfftSekisyokuDataQueue.Enqueue(0);
                NewIfftSekigaiDataQueue.Enqueue(0);
                RawDataSekisyokuQueue.Enqueue(0);
                RawDataSekigaiQueue.Enqueue(0);
            }
            for (int i = 0; i < Acc_RawDataRirekiNum; i++)
            {
                AccelerometerXQueue.Enqueue(0);
                AccelerometerYQueue.Enqueue(0);
                AccelerometerZQueue.Enqueue(0);
            }
            for (int i = 0; i < ShipakuDataRirekiNum; i++)
            {
                ShinpakuSekisyokuDataQueue.Enqueue(0);
                ShinpakuSekigaiDataQueue.Enqueue(0);
            }
            for (int i = 0; i < SpDataRirekiNum; i++)
            {
                SpNormalDataQueue.Enqueue(0);
                SpAcdcDataQueue.Enqueue(0);
            }
            GraphUpdate_Apnea();
//            GraphUpdate_SpO2();
            GraphUpdate_Acc();

            // インターバル処理
            Timer timer = new Timer();
            timer.Tick += new EventHandler(Interval);
            timer.Interval = 500;           // ms単位
            timer.Start();

            calc_snore_init();
        }

        /************************************************************************/
        /* 関数名   : Form1_Shown          						    			*/
        /* 機能     : フォーム表示時のイベント		                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Form1_Shown(object sender, EventArgs e)
        {
            Boolean ret = false;
            Boolean ret_left = false;
            Boolean ret_right = false;

            initGraphShow();

            panda.setComPort_Lattepanda();

            // ログ出力フォルダ作成
            CreateRootDir();

            // 解析スタートでLATTEPANDAのLEDを消灯。
            panda.requestLattepanda((byte)request.LED_OFF);
#if AUTO_ANALYSIS
#if SOUND_RECORD
            // 録音開始
            ret = record.startRecordApnea();
#else
            ret = true;
#endif
            if (ret)
            {
                // 解析
                ret_left = startAnalysis(com_left);      // 左
                ret_right = startAnalysis(com_right);    // 右

                if (ret_left && ret_right)
                {
                    com_left.micPosition = true;    // 左は１
                    com_right.micPosition = false;  // 右は０
                    com_left.DataReceived += ComPort_DataReceived_left;   // コールバックイベント追加
                    com_right.DataReceived += ComPort_DataReceived_right;   // コールバックイベント追加
                    buttonStart.Text = "データ取得中";
                    buttonStart.Enabled = false;
                    log_output("[START]Analysis_Auto");
                }
                else
                {
                    record.stopRecordApnea();
                }
            }
#else
            ret_left = true;
            ret_right = true;
#endif

            if (!ret || !ret_left || !ret_right)
            { //エラー処理
                panda.requestLattepanda((byte)request.LED_ERROR); // LATTEPANDAのLEDを点灯（エラー）。
                panda.closeComPort_Lattepanda();
                log_output("[ERROR]");
                Application.Exit();
            }
        }

        /************************************************************************/
        /* 関数名   : FormMain_FormClosing             			    			*/
        /* 機能     : フォームを閉じる時のイベント	                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            record.stopRecordApnea();
            panda.closeComPort_Lattepanda();
            com_left.Close();
            com_right.Close();
        }

        /************************************************************************/
        /* 関数名   : startAnalysis             		                		*/
        /* 機能     : 解析スタート処理                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : Boolean : 成功 - true                                     */
        /*                      失敗 - false                  					*/
        /************************************************************************/
        private Boolean startAnalysis(ComPort com)
        {
            Boolean ret = false;
            com.BaudRate = 76800;
            com.Parity = Parity.Even;
            com.DataBits = 8;
            com.StopBits = StopBits.One;

            for (int i = 0; i < comboBoxComport.Items.Count; i++)
            {
                com.PortName = comboBoxComport.Items[i].ToString();
                if (com.PortName != "COM1" && com.PortName != "COM5")
                {
                    if (String.IsNullOrWhiteSpace(com.PortName) == false && com.PortName != tempPortname)
                    {
                        if(String.IsNullOrEmpty(tempPortname))
                        {
                            label_portname1.Text = com.PortName;
                        }
                        else
                        {
                            label_portname2.Text = com.PortName;
                        }

                        tempPortname = com.PortName;
                        ret = com.Start();
                        break;
                    }
                }
            }
            log_output("startAnalysis:" + ret);

            return ret;
        }

        /************************************************************************/
        /* 関数名   : USBConnectConf             		                		*/
        /* 機能     : USBストレージ挿入確認                                     */
        /* 引数     : なし                                                      */
        /* 戻り値   : [Boolean] 成功 - true                                     */
        /*          : [Boolean] 失敗 - false                  					*/
        /************************************************************************/
        private Boolean USBConnectConf()
        {
            Boolean ret = false;
#if USB_OUTPUT
            string[] Array_DeviceID;//取得ID分解用配列
            ManagementObjectCollection MyCollection;

            // USBストレージが挿さっているか確認
            MyCollection = MyOCS.Get();
            foreach (ManagementObject MyObject in MyCollection)
            {
                Array_DeviceID = MyObject["DeviceID"].ToString().Split('\\');
                if (Array_DeviceID[0].Contains("USBSTOR"))
                {
                    ret = true;
                    break;
                }
            }
#else
            ret = true; // 無条件でtrue
#endif
            log_output("USBConnectConf:" + ret);

            return ret;
        }

        /************************************************************************/
        /* 関数名   : getRecordFilePath    		                          		*/
        /* 機能     : 録音ファイルのファイルパスを返す                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : [string] recordFilePath - ファイルパス         			*/
        /************************************************************************/
        public string getRecordFilePath()
        {
            return recordFilePath;
        }

        /************************************************************************/
        /* 関数名   : log_output                     			    			*/
        /* 機能     : ログ出力                                                  */
        /* 引数     : [string] msg - ログ文言                                   */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void log_output(string msg)
        {
            Logging(logPath, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "    " + msg + Environment.NewLine);
        }

        /************************************************************************/
        /* 関数名   : Logging          			    		          			*/
        /* 機能     : ログ書き込み処理                                          */
        /* 引数     : [string] logFullPath - ログ出力先パス                     */
        /*          : [string] logstr - ログ文言                                */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Logging(string logFullPath, string logstr)
        {
            FileStream fs = new FileStream(logFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("Shift_JIS"));
            TextWriter tw = TextWriter.Synchronized(sw);
            tw.Write(logstr);
            tw.Flush();
            fs.Close();
        }

        /************************************************************************/
        /* 関数名   : ComPort_DataReceived_left        			    			*/
        /* 機能     : データ受信イベント                                        */
        /* 引数     : [byte[]] buffer - 受信データ                              */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void ComPort_DataReceived_left(byte[] buffer, Boolean micPosition)
        {
            string text = "";

            //Console.WriteLine("ComPort_DataReceived");
            try
            {
                text = amari_left + Encoding.ASCII.GetString(buffer);
                amari_left = "";
                //string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //for (int i = 0; i < lines.Length; i++)
                for (int i = 0; i < lines.Length - 1; i++)  // 最後のデータは切れている可能性があるので、次のデータと結合して処理する
                {
                    //空行チェック
                    if (lines[i].Length == 0)
                    {
                        continue;
                    }

                    //異常値チェック
                    string[] datas = lines[i].Split(new string[] { "," }, StringSplitOptions.None);
                    if (datas.Length == 7)
                    {
                        //測定データ表示
                        //SetTextInput(lines[i] + "\r\n");
                        //演算
                        int result;
                        if (!int.TryParse(datas[0], out result)) continue;      // 赤色AD値
                        if (!int.TryParse(datas[1], out result)) continue;      // 赤外AD値
                        if (!int.TryParse(datas[2], out result)) continue;      // マイク(呼吸)
                        if (!int.TryParse(datas[3], out result)) continue;      // マイク(いびき)
                        if (!int.TryParse(datas[4], out result)) continue;      // 加速度センサ(X)
                        if (!int.TryParse(datas[5], out result)) continue;      // 加速度センサ(Y)
                        if (!int.TryParse(datas[6], out result)) continue;      // 加速度センサ(Z)

                        // For Apnea
                        SetCalcData_Apnea(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]), micPosition);
                        // For PulseOximeter
                        //SetCalcData_SpO2(Convert.ToInt32(datas[0]), Convert.ToInt32(datas[1]));
                        if (Convert.ToInt32(datas[4]) < 200)
                        {
                            // For 加速度
                            SetCalcData_Acc(Convert.ToInt32(datas[4]), Convert.ToInt32(datas[5]), Convert.ToInt32(datas[6]));
                            //log_output("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[3]) + " X軸:" + Convert.ToInt32(datas[4]) + " Y軸:" + Convert.ToInt32(datas[5]) + " Z軸:" + Convert.ToInt32(datas[6]));
                            //Console.WriteLine("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[1]) + " X軸:" + Convert.ToInt32(datas[4]) + " Y軸:" + Convert.ToInt32(datas[5]) + " Z軸:" + Convert.ToInt32(datas[6]));
                        } else
                        {
                            //log_output("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[3]));
                            //Console.WriteLine("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[1]));
                        }
                    }
                    else
                    {
                        Console.WriteLine("受信異常データ演算破棄(" + lines[i] + " i:" + i + "/" + lines.Length + ")");
                    }
                }
                amari_left = lines[lines.Length - 1];
            }
            catch (Exception ex)
            {
                //■例外多発
                //MessageBox.Show(ex.Message, "内部エラー(ComPort_DataReceived)");
                Console.WriteLine(ex.Message + "text:" + text);
            }
        }

        /************************************************************************/
        /* 関数名   : ComPort_DataReceived_right            			    	*/
        /* 機能     : データ受信イベント                                        */
        /* 引数     : [byte[]] buffer - 受信データ                              */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void ComPort_DataReceived_right(byte[] buffer, Boolean micPosition)
        {
            string text = "";

            //Console.WriteLine("ComPort_DataReceived");
            try
            {
                text = amari_right + Encoding.ASCII.GetString(buffer);
                amari_right = "";
                //string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //for (int i = 0; i < lines.Length; i++)
                for (int i = 0; i < lines.Length - 1; i++)  // 最後のデータは切れている可能性があるので、次のデータと結合して処理する
                {
                    //空行チェック
                    if (lines[i].Length == 0)
                    {
                        continue;
                    }

                    //異常値チェック
                    string[] datas = lines[i].Split(new string[] { "," }, StringSplitOptions.None);
                    if (datas.Length == 7)
                    {
                        //測定データ表示
                        //SetTextInput(lines[i] + "\r\n");
                        //演算
                        int result;
                        if (!int.TryParse(datas[0], out result)) continue;      // 赤色AD値
                        if (!int.TryParse(datas[1], out result)) continue;      // 赤外AD値
                        if (!int.TryParse(datas[2], out result)) continue;      // マイク(呼吸)
                        if (!int.TryParse(datas[3], out result)) continue;      // マイク(いびき)
                        if (!int.TryParse(datas[4], out result)) continue;      // 加速度センサ(X)
                        if (!int.TryParse(datas[5], out result)) continue;      // 加速度センサ(Y)
                        if (!int.TryParse(datas[6], out result)) continue;      // 加速度センサ(Z)

                        // For Apnea
                        SetCalcData_Apnea(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]), micPosition);
                        // For PulseOximeter
                        //SetCalcData_SpO2(Convert.ToInt32(datas[0]), Convert.ToInt32(datas[1]));
/*
                        if (Convert.ToInt32(datas[4]) < 200)
                        {
                            // For 加速度
                            SetCalcData_Acc(Convert.ToInt32(datas[4]), Convert.ToInt32(datas[5]), Convert.ToInt32(datas[6]));
                            //log_output("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[3]) + " X軸:" + Convert.ToInt32(datas[4]) + " Y軸:" + Convert.ToInt32(datas[5]) + " Z軸:" + Convert.ToInt32(datas[6]));
                            //Console.WriteLine("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[1]) + " X軸:" + Convert.ToInt32(datas[4]) + " Y軸:" + Convert.ToInt32(datas[5]) + " Z軸:" + Convert.ToInt32(datas[6]));
                        }
                        else
                        {
                            //log_output("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[3]));
                            //Console.WriteLine("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[1]));
                        }
*/
                    }
                    else
                    {
                        Console.WriteLine("受信異常データ演算破棄(" + lines[i] + " i:" + i + "/" + lines.Length + ")");
                    }
                }
                amari_right = lines[lines.Length - 1];
            }
            catch (Exception ex)
            {
                //■例外多発
                //MessageBox.Show(ex.Message, "内部エラー(ComPort_DataReceived)");
                Console.WriteLine(ex.Message + "text:" + text);
            }
        }
        public delegate void SetTextDelegate(string str);

        /************************************************************************/
        /* 関数名   : SetCalcData_Apnea               			    			*/
        /* 機能     : 呼吸・いびきのデータをセット                              */
        /* 引数     : [int] data1 - 呼吸の生データ                              */
        /*          : [int] data2 - いびきの生データ                            */
        /* 戻り値   : なし														*/
        /************************************************************************/
        // For Apnea
        private void SetCalcData_Apnea(int data1, int data2, Boolean micPosition)
        {
            if(micPosition)
            {// 左
             //計算用データ
                CalcDataList_left1.Add(data1);
                CalcDataList_left2.Add(data2);

                //グラフ用データ追加
                lock (lockData)
                {
                    // 呼吸データ
                    if (RawDataRespQueue_left.Count >= ApneaGraphDataNum)
                    {
                        RawDataRespQueue_left.Dequeue();
                    }
                    RawDataRespQueue_left.Enqueue(data1);

                    // いびきデータ
                    if (RawDataSnoreQueue_left.Count >= ApneaGraphDataNum)
                    {
                        RawDataSnoreQueue_left.Dequeue();
                    }
                    RawDataSnoreQueue_left.Enqueue(data2);
                }

                if (CalcDataList_left1.Count >= CalcDataNumApnea)
                {
                    //演算
                    Calc_Apnea(micPosition);

                    //データクリア
                    CalcDataList_left1.Clear();
                    CalcDataList_left2.Clear();
                }
            }
            else
            {// 右
             //計算用データ
                CalcDataList_right1.Add(data1);
                CalcDataList_right2.Add(data2);

                //グラフ用データ追加
                lock (lockData)
                {
                    // 呼吸データ
                    if (RawDataRespQueue_right.Count >= ApneaGraphDataNum)
                    {
                        RawDataRespQueue_right.Dequeue();
                    }
                    RawDataRespQueue_right.Enqueue(data1);

                    // いびきデータ
                    if (RawDataSnoreQueue_right.Count >= ApneaGraphDataNum)
                    {
                        RawDataSnoreQueue_right.Dequeue();
                    }
                    RawDataSnoreQueue_right.Enqueue(data2);
                }

                if (CalcDataList_right1.Count >= CalcDataNumApnea)
                {
                    //演算
                    Calc_Apnea(micPosition);

                    //データクリア
                    CalcDataList_right1.Clear();
                    CalcDataList_right2.Clear();
                }

            }
        }

        /************************************************************************/
        /* 関数名   : SetCalcData_SpO2               			    			*/
        /* 機能     : 赤色・赤外のデータをセット                                */
        /* 引数     : [int] data1 - 赤色の生データ                              */
        /*          : [int] data2 - 赤外の生データ                              */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void SetCalcData_SpO2(int data1, int data2)
        {

            //計算用データ
            SpO2_CalcDataList1.Add(data1);
            SpO2_CalcDataList2.Add(data2);
            lock (lockData_SpO2)
            {
                //グラフ用データ追加
                if (RawDataSekisyokuQueue.Count >= SpO2_RawDataRirekiNum)
                {
                    RawDataSekisyokuQueue.Dequeue();
                }
                RawDataSekisyokuQueue.Enqueue(data1);
                if (RawDataSekigaiQueue.Count >= SpO2_RawDataRirekiNum)
                {
                    RawDataSekigaiQueue.Dequeue();
                }
                RawDataSekigaiQueue.Enqueue(data2);
            }

            if (SpO2_CalcDataList1.Count >= CalcDataNumSpO2)     // 1と2は同じ個数が入る前提
            {
                //演算
//                Calc_SpO2();
                //結果表示
                double shinpaku_sekisyoku = get_sinpak_clr();
                double shinpaku_sekigai   = get_sinpak_inf();
                double sp_normal = get_spo2();
                double sp_acdc = get_acavg_ratio();

                //グラフ用データ追加
                lock (lockData_SpO2)
                {
                    if (ShinpakuSekisyokuDataQueue.Count >= ShipakuDataRirekiNum)
                    {
                        ShinpakuSekisyokuDataQueue.Dequeue();
                    }
                    ShinpakuSekisyokuDataQueue.Enqueue(shinpaku_sekisyoku);

                    if (ShinpakuSekigaiDataQueue.Count >= ShipakuDataRirekiNum)
                    {
                        ShinpakuSekigaiDataQueue.Dequeue();
                    }
                    ShinpakuSekigaiDataQueue.Enqueue(shinpaku_sekigai);

                    if (SpNormalDataQueue.Count >= SpDataRirekiNum)
                    {
                        SpNormalDataQueue.Dequeue();
                    }
                    SpNormalDataQueue.Enqueue(sp_normal);

                    if (SpAcdcDataQueue.Count >= SpDataRirekiNum)
                    {
                        SpAcdcDataQueue.Dequeue();
                    }
                    SpAcdcDataQueue.Enqueue(sp_acdc);
                }
                //データクリア
                SpO2_CalcDataList1.Clear();
                SpO2_CalcDataList2.Clear();
            }
        }

        /************************************************************************/
        /* 関数名   : SetCalcData_Acc               			    			*/
        /* 機能     : 加速度センサーのデータをセット                            */
        /* 引数     : [int] data1 - X軸                                         */
        /*          : [int] data2 - Y軸                                         */
        /*          : [int] data3 - Z軸                                         */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void SetCalcData_Acc(int data1, int data2, int data3)
        {
            lock (lockData_Acc)
            {
                //グラフ用データ追加
                // X軸
                if (AccelerometerXQueue.Count >= Acc_RawDataRirekiNum)
                {
                    AccelerometerXQueue.Dequeue();
                }
                AccelerometerXQueue.Enqueue(data1);
                // Y軸
                if (AccelerometerYQueue.Count >= Acc_RawDataRirekiNum)
                {
                    AccelerometerYQueue.Dequeue();
                }
                AccelerometerYQueue.Enqueue(data2);
                // Z軸
                if (AccelerometerZQueue.Count >= Acc_RawDataRirekiNum)
                {
                    AccelerometerZQueue.Dequeue();
                }
                AccelerometerZQueue.Enqueue(data3);

                //10回に1回テキスト出力する(500ms毎)（暫定）
                //50msごとに出力すると約1時間で１フォルダ内のフォルダ数の限界がくるため
                //理想は1テキスト内に出力し続ける
                string path = CreateAcceDir(AcceCalcCount_);
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                get_accelerometer((double)data1, (double)data2, (double)data3, pathptr);
                AcceCalcCount_++;
            }
        }

        /************************************************************************/
        /* 関数名   : CreateRootDir                     			   			*/
        /* 機能     : 演算結果保存用パスの作成                                  */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void CreateRootDir()
        {
            string datestr = DateTime.Now.ToString("yyyyMMddHHmm");
            string temp;
            string drivePath;
            int i;
#if USB_OUTPUT
            drivePath = "C:\\"; //初期値
#else
#if C_DRIVE
            drivePath = "C:\\";
#else
            drivePath = "."; //exeと同ディレクトリ
#endif
#endif

#if USB_OUTPUT
            char path_char = 'A';
            System.IO.DriveInfo drive;

            if (USBConnectConf())
            { //USBが挿さっていたらAドライブからZドライブまで検索(ただし、Cは除く)
                do
                {
                    temp = path_char.ToString();
                    if (temp != "C")
                    {
                        drive = new System.IO.DriveInfo(temp);
                        if (drive.IsReady && drive.DriveType == System.IO.DriveType.Removable)
                        {
                            drivePath = temp + ":\\";
                            break;
                        }
                        else
                        {
                            path_char++;
                            if (path_char > 'Z')
                            { //USBは挿しているがZドライブまで検索したが見つからなかった場合の救済措置として、強制的にCドライブに出力する。
                                break;
                            }
                        }
                    }
                    else
                    {
                        path_char++;
                    }
                } while (path_char <= 'Z');
            }
#endif
            // rootパス
            ApneaRootPathGeneral_ = drivePath + "\\ax\\apnea\\general\\" + datestr + "\\";
            temp = ApneaRootPathGeneral_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = ApneaRootPathGeneral_ + "(" + i + ")";
                }
                else
                {
                    temp = temp + "\\";
                    Directory.CreateDirectory(temp);
                    break;
                }
            }

            // 左
            ApneaRootPathLeft_ = drivePath + "\\ax\\apnea\\left\\" + datestr + "\\";
            temp = ApneaRootPathLeft_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = ApneaRootPathLeft_ + "(" + i + ")";
                }
                else
                {
                    temp = temp + "\\";
                    Directory.CreateDirectory(temp);
                    break;
                }
            }
            // rootパス
            // 右
            ApneaRootPathRight_ = drivePath + "\\ax\\apnea\\right\\" + datestr + "\\";
            temp = ApneaRootPathRight_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = ApneaRootPathRight_ + "(" + i + ")";
                }
                else
                {
                    temp = temp + "\\";
                    Directory.CreateDirectory(temp);
                    break;
                }
            }
/*
            // rootパス
            PulseRootPath_ = drivePath + "\\ax\\pulse\\" + datestr + "\\";
            temp = PulseRootPath_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = PulseRootPath_ + "(" + i + ")";
                }
                else
                {
                    temp = temp + "\\";
                    Directory.CreateDirectory(temp);
                    break;
                }
            }
*/
            // rootパス
            AcceRootPath_ = drivePath + "\\ax\\acce\\" + datestr + "\\";
            temp = AcceRootPath_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = AcceRootPath_ + "(" + i + ")";
                }
                else
                {
                    temp = temp + "\\";
                    Directory.CreateDirectory(temp);
                    break;
                }
            }

            // rootパス
            RecordRootPath_ = drivePath + "\\ax\\record\\" + datestr + "\\";
            temp = RecordRootPath_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = RecordRootPath_ + "(" + i + ")";
                }
                else
                {
                    Directory.CreateDirectory(temp);
                    recordFilePath = temp;
                    break;
                }
            }
        }

        /************************************************************************/
        /* 関数名   : CreateApneaDir                     		    			*/
        /* 機能     : 無呼吸演算結果保存用パスの作成                            */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private string CreateApneaDir(int Count, Boolean micPosition)
        {
            string path = "";
            
            if(micPosition)
            { //左
                path = ApneaRootPathLeft_ + Count.ToString("D");
            }
            else
            { //右
                path = ApneaRootPathRight_ + Count.ToString("D");
            }

            if (Directory.Exists(path)){
            }else{
                Directory.CreateDirectory(path);
            }
            
            return path;
        }

        /************************************************************************/
        /* 関数名   : CreateApneaDirGeneral                     		    	*/
        /* 機能     : 無呼吸演算結果保存用パスの作成                            */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private string CreateApneaDirGeneral(int Count)
        {
            string path = ApneaRootPathGeneral_ + Count.ToString("D");

            if (Directory.Exists(path)){
            }else{
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /************************************************************************/
        /* 関数名   : CreatePulseDir                     		    			*/
        /* 機能     : SpO2演算結果保存用パスの作成                              */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        /*
                private string CreatePulseDir(int Count)
                {
                   string path = PulseRootPath_ + Count.ToString("D");
                    if(Directory.Exists(path)){
                    }else{
                        Directory.CreateDirectory(path);
                    }

                    return path;
                }
        */
        /************************************************************************/
        /* 関数名   : CreateAcceDir                     		    			*/
        /* 機能     : 加速度センサー結果保存用パスの作成                        */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private string CreateAcceDir(int Count)
        {

            string path = AcceRootPath_ + Count.ToString("D");
            if (Directory.Exists(path))
            {
            }
            else
            {
                 Directory.CreateDirectory(path);
            }
            return path;
        }

        /************************************************************************/
        /* 関数名   : Calc_Apnea                    			    			*/
        /* 機能     : 呼吸データの演算                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Calc_Apnea(Boolean micPosition)
        {
            try
            {
                string path = ""; 
                int num = CalcDataNumApnea;
                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr ptr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr pi = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr pd = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(double)) * num);
                int[] arrayi = new int[num];
                double[] arrayd = new double[num];
                int state;

                if (micPosition)
                { //左
                    path = CreateApneaDir(ApneaCalcCountLeft_, micPosition);
                    ApneaCalcCountLeft_ += 1;
                    Marshal.Copy(CalcDataList_left1.ToArray(), 0, ptr, num);
                    Marshal.Copy(CalcDataList_left2.ToArray(), 0, ptr2, num);
                }
                else
                { //右
                    path = CreateApneaDir(ApneaCalcCountRight_, micPosition);
                    ApneaCalcCountRight_ += 1;
                    Marshal.Copy(CalcDataList_right1.ToArray(), 0, ptr, num);
                    Marshal.Copy(CalcDataList_right2.ToArray(), 0, ptr2, num);
                }
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                lock (lockData)
                {
                    getwav_init(ptr, num, pathptr, ptr2);
                    if (micPosition)
                    {// 左
                        // DC成分除去データをQueueに置く
                        getwav_dc(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            RawDataDcQueue_left.Dequeue();
                            RawDataDcQueue_left.Enqueue(arrayd[ii]);
                        }

                        // 無呼吸(ave)データをQueueに置く
                        get_apnea_ave(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaAveQueue_left.Dequeue();
                            ApneaAveQueue_left.Enqueue(arrayd[ii]);
                        }

                        // 無呼吸(eval)データをQueueに置く
                        get_apnea_eval(pi);
                        Marshal.Copy(pi, arrayi, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaEvalQueue_left.Dequeue();
                            ApneaEvalQueue_left.Enqueue(arrayi[ii]);
                        }
                        // 無呼吸(rms)データをQueueに置く
                        get_apnea_rms(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaRmsQueue_left.Dequeue();
                            ApneaRmsQueue_left.Enqueue(arrayd[ii]);
                        }

                        // 無呼吸(point)データをQueueに置く
                        get_apnea_point(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaPointQueue_left.Dequeue();
                            ApneaPointQueue_left.Enqueue(arrayd[ii]);
                        }

                        // いびき(xy2)データをQueueに置く
                        get_snore_xy2(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            SnoreXy2Queue_left.Dequeue();
                            SnoreXy2Queue_left.Enqueue(arrayd[ii]);
                        }

                        // いびき(interval)データをQueueに置く
                        get_snore_interval(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            SnoreIntervalQueue_left.Dequeue();
                            SnoreIntervalQueue_left.Enqueue(arrayd[ii]);
                        }

                        // 演算結果データ
                        state = get_state();
                        snore_left = state & 0x01;
                        apnea_left = (state & 0xC0) >> 6;
                        if (ResultIbikiQueue_left.Count >= BufNumApneaGraph)
                        {
                            ResultIbikiQueue_left.Dequeue();
                        }
                        ResultIbikiQueue_left.Enqueue(snore_left);
                        if (ApneaQueue_left.Count >= BufNumApneaGraph)
                        {
                            ApneaQueue_left.Dequeue();
                        }
                        ApneaQueue_left.Enqueue(apnea_left);
                    }
                    else
                    {// 右
                        // DC成分除去データをQueueに置く
                        getwav_dc(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            RawDataDcQueue_right.Dequeue();
                            RawDataDcQueue_right.Enqueue(arrayd[ii]);
                        }

                        // 無呼吸(ave)データをQueueに置く
                        get_apnea_ave(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaAveQueue_right.Dequeue();
                            ApneaAveQueue_right.Enqueue(arrayd[ii]);
                        }

                        // 無呼吸(eval)データをQueueに置く
                        get_apnea_eval(pi);
                        Marshal.Copy(pi, arrayi, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaEvalQueue_right.Dequeue();
                            ApneaEvalQueue_right.Enqueue(arrayi[ii]);
                        }
                        // 無呼吸(rms)データをQueueに置く
                        get_apnea_rms(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaRmsQueue_right.Dequeue();
                            ApneaRmsQueue_right.Enqueue(arrayd[ii]);
                        }

                        // 無呼吸(point)データをQueueに置く
                        get_apnea_point(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            ApneaPointQueue_right.Dequeue();
                            ApneaPointQueue_right.Enqueue(arrayd[ii]);
                        }

                        // いびき(xy2)データをQueueに置く
                        get_snore_xy2(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            SnoreXy2Queue_right.Dequeue();
                            SnoreXy2Queue_right.Enqueue(arrayd[ii]);
                        }

                        // いびき(interval)データをQueueに置く
                        get_snore_interval(pd);
                        Marshal.Copy(pd, arrayd, 0, num);
                        for (int ii = 0; ii < num; ++ii)
                        {
                            SnoreIntervalQueue_right.Dequeue();
                            SnoreIntervalQueue_right.Enqueue(arrayd[ii]);
                        }

                        // 演算結果データ
                        state = get_state();
                        snore_right = state & 0x01;
                        apnea_right = (state & 0xC0) >> 6;
                        if (ResultIbikiQueue_right.Count >= BufNumApneaGraph)
                        {
                            ResultIbikiQueue_right.Dequeue();
                        }
                        ResultIbikiQueue_right.Enqueue(snore_right);
                        if (ApneaQueue_right.Count >= BufNumApneaGraph)
                        {
                            ApneaQueue_right.Dequeue();
                        }
                        ApneaQueue_right.Enqueue(apnea_right);
                    }
                    
                    if(snore_left != -1 && snore_right != -1 && apnea_left != -1 && apnea_right != -1)
                    { //左右の判定済み
                        if(snore_left == 1 || snore_right == 1)
                        { //どちらかがいびき判定
                            snore = 1;
                        }
                        else if(apnea_left != 0 && apnea_right != 0)
                        { //どちらも無呼吸判定
                            apnea = 2;
                        }

                        if(snore == 1 || apnea == 2)
                        {
                            // アラーム鳴動
                            alarm.confirmAlarm();

                            // バイブレーション
                            vib.confirmVib((byte)request.VIBRATION);
                        }

                        // ログ出力
                        path = CreateApneaDirGeneral(ApneaCalcCountGeneral_);
                        ApneaCalcCountGeneral_ += 1;
                        pathptr = Marshal.StringToHGlobalAnsi(path);
                        get_general_result((double)snore, (double)apnea, pathptr);

                        if (ResultIbikiQueue.Count >= BufNumApneaGraph)
                        {
                            ResultIbikiQueue.Dequeue();
                        }
                        ResultIbikiQueue.Enqueue(snore);
                        if (ApneaQueue.Count >= BufNumApneaGraph)
                        {
                            ApneaQueue.Dequeue();
                        }
                        ApneaQueue.Enqueue(apnea);

                        snore_left = -1;
                        apnea_left = -1;
                        snore_right = -1;
                        apnea_right = -1;
                        snore = 0;
                        apnea = 0;
                    }
                }
                Marshal.FreeCoTaskMem(ptr);
                Marshal.FreeCoTaskMem(ptr2);
                Marshal.FreeCoTaskMem(pi);
                Marshal.FreeCoTaskMem(pd);
                Marshal.FreeHGlobal(pathptr);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "内部エラー(Calc)");
                Console.WriteLine(ex.Message);
            }
        }

        /************************************************************************/
        /* 関数名   : Calc_SpO2                     			    			*/
        /* 機能     : SpO2データの演算                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void Calc_SpO2()
        {
            try
            {
                string path = CreatePulseDir(PulseCalcCount_);
                PulseCalcCount_+=1;
                
                int num = SpO2_CalcDataList1.Count;
                IntPtr ptr1 = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr ptr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr pi = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr pd = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(double)) * num);
                int[] arrayi = new int[num];
                double[] arrayd = new double[num];
                Marshal.Copy(SpO2_CalcDataList1.ToArray(), 0, ptr1, num);
                Marshal.Copy(SpO2_CalcDataList2.ToArray(), 0, ptr2, num);
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                calculator_clr(ptr1, num, pathptr);
                lock (lockData_SpO2)
                {
                    // DC成分除去データをQueueに置く
                    get_dc(pi);
                    Marshal.Copy(pi, arrayi, 0, num);
                    for(int ii=0;ii<num;++ii){
                        DcSekisyokuDataQueue.Dequeue();
                        DcSekisyokuDataQueue.Enqueue(arrayi[ii]);
                    }
                    // FFTデータをQueueに置く
                    get_fft(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for(int ii=0;ii<num;++ii){
                        FftSekisyokuDataQueue.Dequeue();
                        FftSekisyokuDataQueue.Enqueue(arrayd[ii]);
                    }
                    // iFFTデータをQueueに置く
                    get_ifft(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for(int ii=0;ii<num;++ii){
                        IfftSekisyokuDataQueue.Dequeue();
                        IfftSekisyokuDataQueue.Enqueue(arrayd[ii]);
                    }
                    // new_iFFTデータをQueueに置く
                    get_new_ifft(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for(int ii=0;ii<num;++ii){
                        NewIfftSekisyokuDataQueue.Dequeue();
                        NewIfftSekisyokuDataQueue.Enqueue(arrayd[ii]);
                    }
                }
                calculator_inf(ptr2, num, pathptr);
                lock (lockData_SpO2)
                {
                    // DC成分除去データをQueueに置く
                    get_dc(pi);
                    Marshal.Copy(pi, arrayi, 0, num);
                    for(int ii=0;ii<num;++ii){
                        DcSekigaiDataQueue.Dequeue();
                        DcSekigaiDataQueue.Enqueue(arrayi[ii]);
                    }
                    // FFTデータをQueueに置く
                    get_fft(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for(int ii=0;ii<num;++ii){
                        FftSekigaiDataQueue.Dequeue();
                        FftSekigaiDataQueue.Enqueue(arrayd[ii]);
                    }
                    // iFFTデータをQueueに置く
                    get_ifft(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for(int ii=0;ii<num;++ii){
                        IfftSekigaiDataQueue.Dequeue();
                        IfftSekigaiDataQueue.Enqueue(arrayd[ii]);
                    }
                    // new_iFFTデータをQueueに置く
                    get_new_ifft(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for(int ii=0;ii<num;++ii){
                        NewIfftSekigaiDataQueue.Dequeue();
                        NewIfftSekigaiDataQueue.Enqueue(arrayd[ii]);
                    }
                }
                Marshal.FreeCoTaskMem(ptr1);
                Marshal.FreeCoTaskMem(ptr2);
                Marshal.FreeCoTaskMem(pi);
                Marshal.FreeCoTaskMem(pd);
                Marshal.FreeHGlobal(pathptr);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "内部エラー(Calc)");
                Console.WriteLine(ex.Message);
            }
        }
*/
        /************************************************************************/
        /* 関数名   : GraphUpdate_Apnea                			    			*/
        /* 機能     : 呼吸グラフを更新                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void GraphUpdate_Apnea()
        {
            int cnt = 0;

            lock (lockData)
            {
                // 無呼吸・低呼吸グラフを更新
                // 左
                Series srs_apnea = chartApnea_left.Series["呼吸状態"]; //■
                Series srs_snore = chartApnea_left.Series["いびき"]; //■
                srs_apnea.Points.Clear();
                srs_snore.Points.Clear();
                cnt = 0;
                foreach (int data in ApneaQueue_left)
                {
                    srs_apnea.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ResultIbikiQueue_left)
                {
                    srs_snore.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 右
                srs_apnea = chartApnea_right.Series["呼吸状態"]; //■
                srs_snore = chartApnea_right.Series["いびき"]; //■
                srs_apnea.Points.Clear();
                srs_snore.Points.Clear();
                cnt = 0;
                foreach (int data in ApneaQueue_right)
                {
                    srs_apnea.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ResultIbikiQueue_right)
                {
                    srs_snore.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 総合
                srs_apnea = chartApnea.Series["呼吸状態"]; //■
                srs_snore = chartApnea.Series["いびき"]; //■
                srs_apnea.Points.Clear();
                srs_snore.Points.Clear();
                cnt = 0;
                foreach (int data in ApneaQueue)
                {
                    srs_apnea.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ResultIbikiQueue)
                {
                    srs_snore.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 生データグラフを更新
                // 左
                Series srs_rawresp = chartRawData_left.Series["呼吸(生データ)"]; //■
                Series srs_rawsnore = chartRawData_left.Series["いびき(生データ)"]; //■
                Series srs_dcresp = chartRawData_left.Series["呼吸(移動平均)"]; //■
                srs_rawresp.Points.Clear();
                srs_rawsnore.Points.Clear();
                srs_dcresp.Points.Clear();
                cnt = 0;
                foreach (double data in RawDataRespQueue_left)
                {
                    srs_rawresp.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataSnoreQueue_left)
                {
                    srs_rawsnore.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataDcQueue_left)
                {
                    srs_dcresp.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 右
                srs_rawresp = chartRawData_right.Series["呼吸(生データ)"]; //■
                srs_rawsnore = chartRawData_right.Series["いびき(生データ)"]; //■
                srs_dcresp = chartRawData_right.Series["呼吸(移動平均)"]; //■
                srs_rawresp.Points.Clear();
                srs_rawsnore.Points.Clear();
                srs_dcresp.Points.Clear();
                cnt = 0;
                foreach (double data in RawDataRespQueue_right)
                {
                    srs_rawresp.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataSnoreQueue_right)
                {
                    srs_rawsnore.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataDcQueue_right)
                {
                    srs_dcresp.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 演算途中データグラフを更新
                // 左
                Series srs_apnea_ave = chartOperation_left.Series["無呼吸(ave)"];
                Series srs_apnea_eval = chartOperation_left.Series["無呼吸(eval)"];
                Series srs_apnea_rms = chartOperation_left.Series["無呼吸(rms)"];
                Series srs_apnea_point = chartOperation_left.Series["無呼吸(point)"];
                Series srs_snore_xy2 = chartOperation_left.Series["いびき(xy2)"];
                Series srs_snore_interval = chartOperation_left.Series["いびき(interval)"];
                srs_apnea_ave.Points.Clear();
                srs_apnea_eval.Points.Clear();
                srs_apnea_rms.Points.Clear();
                srs_apnea_point.Points.Clear();
                srs_snore_xy2.Points.Clear();
                srs_snore_interval.Points.Clear();
                cnt = 0;
                foreach (double data in ApneaAveQueue_left)
                {
                    srs_apnea_ave.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaEvalQueue_left)
                {
                    srs_apnea_eval.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaRmsQueue_left)
                {
                    srs_apnea_rms.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaPointQueue_left)
                {
                    srs_apnea_point.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in SnoreXy2Queue_left)
                {
                    srs_snore_xy2.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in SnoreIntervalQueue_left)
                {
                    srs_snore_interval.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 右
                srs_apnea_ave = chartOperation_right.Series["無呼吸(ave)"];
                srs_apnea_eval = chartOperation_right.Series["無呼吸(eval)"];
                srs_apnea_rms = chartOperation_right.Series["無呼吸(rms)"];
                srs_apnea_point = chartOperation_right.Series["無呼吸(point)"];
                srs_snore_xy2 = chartOperation_right.Series["いびき(xy2)"];
                srs_snore_interval = chartOperation_right.Series["いびき(interval)"];
                srs_apnea_ave.Points.Clear();
                srs_apnea_eval.Points.Clear();
                srs_apnea_rms.Points.Clear();
                srs_apnea_point.Points.Clear();
                srs_snore_xy2.Points.Clear();
                srs_snore_interval.Points.Clear();
                cnt = 0;
                foreach (double data in ApneaAveQueue_right)
                {
                    srs_apnea_ave.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaEvalQueue_right)
                {
                    srs_apnea_eval.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaRmsQueue_right)
                {
                    srs_apnea_rms.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaPointQueue_right)
                {
                    srs_apnea_point.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in SnoreXy2Queue_right)
                {
                    srs_snore_xy2.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in SnoreIntervalQueue_right)
                {
                    srs_snore_interval.Points.AddXY(cnt, data);
                    cnt++;
                }
            }
            // 更新実行
            chartApnea.Invalidate();
            chartApnea_left.Invalidate();
            chartApnea_right.Invalidate();
            chartRawData_left.Invalidate();
            chartRawData_right.Invalidate();
            chartOperation_left.Invalidate();
            chartOperation_right.Invalidate();
        }

        /************************************************************************/
        /* 関数名   : GraphUpdate_SpO2                			    			*/
        /* 機能     : SpO2グラフを更新                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void GraphUpdate_SpO2()
        {
            int cnt = 0;

            lock (lockData_SpO2)
            {
                // 脈拍数グラフを更新
                Series srs_sekisyoku = chartSinpaku.Series["赤色"];
                Series srs_sekigai = chartSinpaku.Series["赤外"];
                srs_sekisyoku.Points.Clear();
                cnt = 0;
                foreach (double data in ShinpakuSekisyokuDataQueue)
                {
                    srs_sekisyoku.Points.AddXY(cnt, data);
                    cnt++;
                }
                srs_sekigai.Points.Clear();
                cnt = 0;
                foreach (double data in ShinpakuSekigaiDataQueue)
                {
                    srs_sekigai.Points.AddXY(cnt, data);
                    cnt++;
                }

                // SpO2グラフを更新
                Series srs_normal = chartSpO2.Series["SpO2"];
                srs_normal.Points.Clear();
                cnt = 0;
                foreach (double data in SpNormalDataQueue)
                {
                    srs_normal.Points.AddXY(cnt, data);
                    cnt++;
                }

                // Acdcグラフを更新
                Series srs_acdc = chartSpO2.Series["AC比"];
                srs_acdc.Points.Clear();
                cnt = 0;
                foreach (double data in SpAcdcDataQueue)
                {
                    srs_acdc.Points.AddXY(cnt, data);
                    cnt++;
                }

                // 生データSpO2グラフを更新
                Series srs_rawsekisyoku = chartRawData_SpO2.Series["赤色(生データ)"];
                Series srs_rawsekigai = chartRawData_SpO2.Series["赤外(生データ)"];
                Series srs_dcsekisyoku = chartRawData_SpO2.Series["赤色(DC抜)"];
                Series srs_dcsekigai = chartRawData_SpO2.Series["赤外(DC抜)"];
                Series srs_cutsekisyoku = chartRawData_SpO2.Series["赤色(CUT)"];
                Series srs_cutsekigai = chartRawData_SpO2.Series["赤外(CUT)"];
                srs_rawsekisyoku.Points.Clear();
                cnt = 0;
                foreach (double data in RawDataSekisyokuQueue)
                {
                    srs_rawsekisyoku.Points.AddXY(cnt, data);  // 個数表記
                    cnt++;
                }
                srs_rawsekigai.Points.Clear();
                cnt = 0;
                foreach (double data in RawDataSekigaiQueue)
                {
                    srs_rawsekigai.Points.AddXY(cnt, data);  // 個数表記
                    cnt++;
                }
                srs_dcsekisyoku.Points.Clear();
                cnt = 0;
                foreach (double data in DcSekisyokuDataQueue)
                {
                    srs_dcsekisyoku.Points.AddXY(cnt, data);  // 個数表記
                    cnt++;
                }
                srs_dcsekigai.Points.Clear();
                cnt = 0;
                foreach (double data in DcSekigaiDataQueue)
                {
                    srs_dcsekigai.Points.AddXY(cnt, data);  // 個数表記
                    cnt++;
                }
                srs_cutsekisyoku.Points.Clear();
                cnt = 0;
                foreach (double data in NewIfftSekisyokuDataQueue)
                {
                    srs_cutsekisyoku.Points.AddXY(cnt, data);  // 個数表記
                    cnt++;
                }
                srs_cutsekigai.Points.Clear();
                cnt = 0;
                foreach (double data in NewIfftSekigaiDataQueue)
                {
                    srs_cutsekigai.Points.AddXY(cnt, data);  // 個数表記
                    cnt++;
                }
                // 演算途中データグラフを更新
                Series srs_fftclr = chartRawData_Acc.Series["赤色(FFT)"];
                Series srs_fftinf = chartRawData_Acc.Series["赤外(FFT)"];
                Series srs_ifftclr = chartRawData_Acc.Series["赤色(IFFT)"];
                Series srs_ifftinf = chartRawData_Acc.Series["赤外(IFFT)"];
                srs_fftclr.Points.Clear();
                cnt = 0;
                foreach (double data in FftSekisyokuDataQueue)
                {
                    double tmp = (double)cnt;
                    srs_fftclr.Points.AddXY(cnt, data);
                    cnt++;
                }
                srs_fftinf.Points.Clear();
                cnt = 0;
                foreach (double data in FftSekigaiDataQueue)
                {
                    double tmp = (double)cnt;
                    srs_fftinf.Points.AddXY(cnt, data);
                    cnt++;
                }
                srs_ifftclr.Points.Clear();
                cnt = 0;
                foreach (double data in IfftSekisyokuDataQueue)
                {
                    double tmp = (double)cnt;
                    srs_ifftclr.Points.AddXY(cnt, data);
                    cnt++;
                }
                srs_ifftinf.Points.Clear();
                cnt = 0;
                foreach (double data in IfftSekigaiDataQueue)
                {
                    double tmp = (double)cnt;
                    srs_ifftinf.Points.AddXY(cnt, data);
                    cnt++;
                }
            }
            // 更新実行
            chartSinpaku.Invalidate();
            chartSpO2.Invalidate();
            chartRawData_SpO2.Invalidate();
        }
*/
        /************************************************************************/
        /* 関数名   : GraphUpdate_Acc                			    			*/
        /* 機能     : 加速度センサーのグラフを更新                              */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void GraphUpdate_Acc()
        {
            int cnt = 0;

            lock (lockData_Acc)
            {
                // 加速度センサーのグラフを更新
                Series accelerometer_x = chartAccelerometer.Series["X軸"]; //■
                Series accelerometer_y = chartAccelerometer.Series["Y軸"]; //■
                Series accelerometer_z = chartAccelerometer.Series["Z軸"]; //■
                accelerometer_x.Points.Clear();
                accelerometer_y.Points.Clear();
                accelerometer_z.Points.Clear();
                foreach (int data in AccelerometerXQueue)
                {
                    accelerometer_x.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in AccelerometerYQueue)
                {
                    accelerometer_y.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in AccelerometerZQueue)
                {
                    accelerometer_z.Points.AddXY(cnt, data);
                    cnt++;
                }
            }
            chartAccelerometer.Invalidate();
        }

        /************************************************************************/
        /* 関数名   : Interval                      			    			*/
        /* 機能     : グラフ更新イベント                                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void Interval(object sender, EventArgs e)
        {
            GraphUpdate_Apnea();
//            GraphUpdate_SpO2();
            GraphUpdate_Acc();
        }

        /************************************************************************/
        /* 関数名   : CalculateAll                   			    			*/
        /* 機能     : CSVファイル読み込み                                       */
        /* 引数     : [string] FolderPath - フォルダパス                        */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void CalculateAll(String FolderPath)
        {
        	string[] files = System.IO.Directory.GetFiles(FolderPath, "*.csv", System.IO.SearchOption.AllDirectories);
        	foreach(string filename in files){
        		Calculate(filename);
        	}
        }

        /************************************************************************/
        /* 関数名   : Calculate                     			    			*/
        /* 機能     : CSVファイルに書き出し                                     */
        /* 引数     : [string] FolderPath - ファイルパス                        */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Calculate(String FilePath)
        {
    		using(StreamReader r = new StreamReader(FilePath))
    		{
    			string strline;
  				while( (strline = r.ReadLine()) != null){
                    string[] datas = strline.Split(new string[] { "," }, StringSplitOptions.None);
                    if (datas.Length == 4)
                    {
                        //測定データ表示
                        //SetTextInput(lines[i] + "\r\n");
                        //演算
                        int result;
                        if (!int.TryParse(datas[0], out result)) continue;      // 赤色AD値
                        if (!int.TryParse(datas[1], out result)) continue;      // 赤外AD値
                        if (!int.TryParse(datas[2], out result)) continue;      // マイク(呼吸)
                        if (!int.TryParse(datas[3], out result)) continue;      // マイク(いびき)

                        // For Apnea
                        //SetCalcData_Apnea(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]));
                        // For PulseOximeter
                        SetCalcData_SpO2(Convert.ToInt32(datas[0]), Convert.ToInt32(datas[1]));
                    }
  				}
  			}
        }

        /************************************************************************/
        /* 関数名   : initGraphShow                  			    			*/
        /* 機能     : グラフ表示の初期設定                                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void initGraphShow()
        {
            Series srs = chartRawData_left.Series["呼吸(生データ)"];
            Series srs2 = chartRawData_right.Series["呼吸(生データ)"];
            if (checkBox_rawresp.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartRawData_left.Series["いびき(生データ)"];
            srs2 = chartRawData_right.Series["いびき(生データ)"];
            if (checkBox_rawsnore.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartRawData_left.Series["呼吸(移動平均)"];
            srs2 = chartRawData_right.Series["呼吸(移動平均)"];
            if (checkBox_dcresp.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["無呼吸(ave)"];
            srs2 = chartOperation_right.Series["無呼吸(ave)"];
            if (checkBox_apneaave.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["無呼吸(eval)"];
            srs2 = chartOperation_right.Series["無呼吸(eval)"];
            if (checkBox_apneaeval.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["無呼吸(rms)"];
            srs2 = chartOperation_right.Series["無呼吸(rms)"];
            if (checkBox_apnearms.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["無呼吸(rms)"];
            srs2 = chartOperation_right.Series["無呼吸(rms)"];
            if (checkBox_apneapoint.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["無呼吸(point)"];
            srs2 = chartOperation_right.Series["無呼吸(point)"];
            if (checkBox_apneapoint.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["いびき(xy2)"];
            srs2 = chartOperation_right.Series["いびき(xy2)"];
            if (checkBox_snorexy2.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }

            srs = chartOperation_left.Series["いびき(interval)"];
            srs2 = chartOperation_right.Series["いびき(interval)"];
            if (checkBox_snore_interval.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /* ボタンクリックイベント */
        /************************************************************************/
        /* 関数名   : buttonStart_Click          					    		*/
        /* 機能     : 開始ボタンクリック時のイベント                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "開始")
            {
                com_left.PortName = comboBoxComport.Text;
                com_left.BaudRate = 76800;
                com_left.Parity = Parity.Even;
                com_left.DataBits = 8;
                com_left.StopBits = StopBits.One;
                if (String.IsNullOrWhiteSpace(com_left.PortName) == false)
                {
                    Boolean ret = com_left.Start();
                    if (ret)
                    {
                        com_left.DataReceived += ComPort_DataReceived_left;   // コールバックイベント追加
                        buttonStart.Text = "データ取得中";
                        buttonStart.Enabled = false;
                        log_output("[START]Analysis(button)");
                    }
                }
            }
        }

        /************************************************************************/
        /* 関数名   : button_recordstart_Click          				   		*/
        /* 機能     : 録音開始ボタンクリック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void button_recordstart_Click(object sender, EventArgs e)
        {
            log_output("[BUTTON]Record Start");
            record.startRecordApnea();
        }

        /************************************************************************/
        /* 関数名   : button_recordstop_Click          		    		   		*/
        /* 機能     : 録音停止ボタンクリック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void button_recordstop_Click(object sender, EventArgs e)
        {
            log_output("[BUTTON]Record Stop");
            record.stopRecordApnea();
        }

/* チェックボックスイベント */
/* 呼吸切替 */
        /************************************************************************/
        /* 関数名   : checkBox_rawresp_CheckedChanged				    		*/
        /* 機能     : 呼吸(生データ)チェック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_rawresp_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartRawData_left.Series["呼吸(生データ)"];
            Series srs2 = chartRawData_right.Series["呼吸(生データ)"];
            if (checkBox_rawresp.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_rawsnore_CheckedChanged     			    		*/
        /* 機能     : いびき(生データ)チェック時のイベント                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_rawsnore_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartRawData_left.Series["いびき(生データ)"];
            Series srs2 = chartRawData_right.Series["いびき(生データ)"];
            if (checkBox_rawsnore.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_dcresp_CheckedChanged     			    		*/
        /* 機能     : 呼吸(移動平均)チェック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_dcresp_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartRawData_left.Series["呼吸(移動平均)"];
            Series srs2 = chartRawData_right.Series["呼吸(移動平均)"];
            if (checkBox_dcresp.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_apneaave_CheckedChanged  			    		*/
        /* 機能     : 無呼吸(ave)チェック時のイベント                           */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_apneaave_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartOperation_left.Series["無呼吸(ave)"];
            Series srs2 = chartOperation_right.Series["無呼吸(ave)"];
            if (checkBox_apneaave.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_apneaeval_CheckedChanged    			    		*/
        /* 機能     : 無呼吸(eval)チェック時のイベント                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_apneaeval_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartOperation_left.Series["無呼吸(eval)"];
            Series srs2 = chartOperation_right.Series["無呼吸(eval)"];
            if (checkBox_apneaeval.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_apnearms_CheckedChanged        		    		*/
        /* 機能     : 無呼吸(rms)チェック時のイベント                           */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_apnearms_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartOperation_left.Series["無呼吸(rms)"];
            Series srs2 = chartOperation_right.Series["無呼吸(rms)"];
            if (checkBox_apnearms.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_apneapoint_CheckedChanged     	        		*/
        /* 機能     : 無呼吸(point)チェック時のイベント                         */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_apneapoint_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartOperation_left.Series["無呼吸(point)"];
            Series srs2 = chartOperation_right.Series["無呼吸(point)"];
            if (checkBox_apneapoint.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_snorexy2_CheckedChanged   			    		*/
        /* 機能     : いびき(xy2)チェック時のイベント                           */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_snorexy2_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartOperation_left.Series["いびき(xy2)"];
            Series srs2 = chartOperation_right.Series["いびき(xy2)"];
            if (checkBox_snorexy2.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_snore_interval_CheckedChanged      	    		*/
        /* 機能     : いびき(interval)チェック時のイベント                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_snore_interval_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartOperation_left.Series["いびき(interval)"];
            Series srs2 = chartOperation_right.Series["いびき(interval)"];
            if (checkBox_snore_interval.Checked)
            {
                srs.Enabled = true;
                srs2.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
                srs2.Enabled = false;
            }
        }

        /* SpO2切替 */
        /************************************************************************/
        /* 関数名   : checkBox_rawclr_CheckedChanged       			    		*/
        /* 機能     : 赤色(生データ)チェック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_rawclr_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_rawsekisyoku = chartRawData_SpO2.Series["赤色(生データ)"];
            if (checkBox_rawclr.Checked)
            {
                srs_rawsekisyoku.Enabled = true;
            }
            else
            {
                srs_rawsekisyoku.Enabled = false;
            }
        }
*/
        /************************************************************************/
        /* 関数名   : checkBox_rawinf_CheckedChanged   			        		*/
        /* 機能     : 赤外(生データ)チェック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_rawinf_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_rawsekigai = chartRawData_SpO2.Series["赤外(生データ)"];
            if (checkBox_rawinf.Checked)
            {
                srs_rawsekigai.Enabled = true;
            }
            else
            {
                srs_rawsekigai.Enabled = false;
            }
        }
*/

        /************************************************************************/
        /* 関数名   : checkBox_dcclr_CheckedChanged   				    		*/
        /* 機能     : 赤色(DC抜)チェック時のイベント                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_dcclr_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_dcsekisyoku = chartRawData_SpO2.Series["赤色(DC抜)"];
            if (checkBox_dcclr.Checked)
            {
                srs_dcsekisyoku.Enabled = true;
            }
            else
            {
                srs_dcsekisyoku.Enabled = false;
            }
        }
*/
        /************************************************************************/
        /* 関数名   : checkBox_dcinf_CheckedChanged    				    		*/
        /* 機能     : 赤外(DC抜)チェック時のイベント                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_dcinf_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_dcsekigai = chartRawData_SpO2.Series["赤外(DC抜)"];
            if (checkBox_dcinf.Checked)
            {
                srs_dcsekigai.Enabled = true;
            }
            else
            {
                srs_dcsekigai.Enabled = false;
            }
        }
*/
        /************************************************************************/
        /* 関数名   : checkBox_fftclr_CheckedChanged          		    		*/
        /* 機能     : 赤色(FFT)チェック時のイベント                             */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_fftclr_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_fftclr = chartRawData_Acc.Series["赤色(FFT)"];
            if (checkBox_fftclr.Checked)
            {
                srs_fftclr.Enabled = true;
            }
            else
            {
                srs_fftclr.Enabled = false;
            }
        }
*/
        /************************************************************************/
        /* 関数名   : checkBox_fftinf_CheckedChanged    			    		*/
        /* 機能     : 赤外(FFT)チェック時のイベント                             */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_fftinf_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_fftinf = chartRawData_Acc.Series["赤外(FFT)"];
            if (checkBox_fftinf.Checked)
            {
                srs_fftinf.Enabled = true;
            }
            else
            {
                srs_fftinf.Enabled = false;
            }
        }
*/
        /************************************************************************/
        /* 関数名   : checkBox_ifftclr_CheckedChanged          		    		*/
        /* 機能     : 赤色(IFFT)チェック時のイベント                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_ifftclr_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_ifftclr = chartRawData_Acc.Series["赤色(IFFT)"];
            if (checkBox_ifftclr.Checked)
            {
                srs_ifftclr.Enabled = true;
            }
            else
            {
                srs_ifftclr.Enabled = false;
            }
        }
*/
        /************************************************************************/
        /* 関数名   : checkBox_ifftinf_CheckedChanged       		    		*/
        /* 機能     : 赤外(IFFT)チェック時のイベント                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
/*
        private void checkBox_ifftinf_CheckedChanged(object sender, EventArgs e)
        {
            Series srs_ifftinf = chartRawData_Acc.Series["赤外(IFFT)"];
            if (checkBox_ifftinf.Checked)
            {
                srs_ifftinf.Enabled = true;
            }
            else
            {
                srs_ifftinf.Enabled = false;
            }
        }
*/
/* アラーム */
        /************************************************************************/
        /* 関数名   : checkBox_snore_CheckedChanged          		    		*/
        /* 機能     : いびきチェック時のイベント                                */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_snore_CheckedChanged(object sender, EventArgs e)
        {
            alarm.snoreCheckedChanged();
        }

        /************************************************************************/
        /* 関数名   : checkBox_Apnea_CheckedChanged          		    		*/
        /* 機能     : 無呼吸チェック時のイベント                                */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_Apnea_CheckedChanged(object sender, EventArgs e)
        {
            alarm.apneaCheckedChanged();
        }

        /************************************************************************/
        /* 関数名   : button_alarmplay_Click          		    		        */
        /* 機能     : アラーム再生ボタンクリック時のイベント                    */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void button_alarmplay_Click(object sender, EventArgs e)
        {
            alarm.playAlarm();
        }

/* バイブレーション */
        /************************************************************************/
        /* 関数名   : button_vibstart_Click          		    		        */
        /* 機能     : バイブボタンクリック時のイベント                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void button_vibstart_Click(object sender, EventArgs e)
        {
            panda.requestLattepanda((byte)request.VIBRATION);
        }
    }
}
