using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using System.Management;
using System.Drawing;

namespace SleepCheckerApp
{
    public partial class FormMain : Form
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint
        GetPrivateProfileString(string lpAppName,
            string lpKeyName, string lpDefault,
            StringBuilder lpReturnedString, uint nSize,
            string lpFileName);
        // For Apnea
        [DllImport("Apnea.dll")]
        static extern void setThreshold(int SnoreParamThre, int SnoreParamNormalCnt, int ApneaJudgeCnt, double ApneaParamBinThre);
        [DllImport("Apnea.dll")]
        static extern void setEdgeThreshold(int MaxEdgeThre, int MinSnoreThre, int MinBreathThre, int DiameterCenter, int DiameterNext, double DiameterEnd);
        [DllImport("Apnea.dll")]
        static extern void getwav_init(IntPtr data, int len, IntPtr path, IntPtr snore);
        [DllImport("Apnea.dll")]
        static extern void getwav_movave(IntPtr data);
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
        static extern double get_heartBeatRemoveAve();
        [DllImport("Apnea.dll")]
        static extern void get_accelerometer(double data1, double data2, double data3, IntPtr path);
        [DllImport("Apnea.dll")]
        static extern void get_photoreflector(double data, IntPtr ppath);
        [DllImport("Apnea.dll")]
        static extern void getwav_heartbeat_remov_dc(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void getwav_dc(IntPtr data);
        [DllImport("Apnea.dll")]
        static extern void set_g1d_judge_ret(int snore_g1d, int apnea_g1d);
        [DllImport("Apnea.dll")]
        static extern void calc_snore_init();
        [DllImport("Apnea.dll")]
        static extern void set_averageData(double kokyu, double ibiki, IntPtr ppath);

        private Boolean USB_OUTPUT = true;
        private Boolean C_DRIVE = true;
        private Boolean AUTO_ANALYSIS = true;
        private Boolean SOUND_RECORD = true;

        private ComPort com = null;
        private FormSetting formset = null;
        private SoundRecord record = null;
        private LattePanda panda = null;

        private const int CalcDataNumApnea = 200;           // 6秒間、50msに1回データ取得した数
        private const int CalcDataNumCalculationApnea = 10;

        enum request
        {
            LED_OFF = 0,    //解析スタート
            LED_ERROR,      //エラー
            SET_CLOCK,      //時刻設定
            VIBRATION,      //バイブレーション
        }

        private int[] CalcData1 = new int[CalcDataNumApnea];          // 1回の演算に使用するデータ
        private List<int> CalcDataList1 = new List<int>();
        private List<int> CalcDataList2 = new List<int>();
        private string amari = "";

        // グラフ用
        // 生データ
        private const int ApneaGraphDataNum = CalcDataNumApnea + 1;
        Queue<double> RawDataRespQueue = new Queue<double>();
        Queue<double> RawDataSnoreQueue = new Queue<double>();
        Queue<double> RawDataDcQueue = new Queue<double>();

        // 加速度センサー
        private const int AcceAndPhotoRef_RawDataRirekiNum = 200;       // 生データ履歴数 500ms * 200個 = 100秒
        Queue<double> AccelerometerXQueue = new Queue<double>();
        Queue<double> AccelerometerYQueue = new Queue<double>();
        Queue<double> AccelerometerZQueue = new Queue<double>();

        // 演算途中データ
        private const int ApneaGraphCalculationDataNum = CalcDataNumCalculationApnea * 10 + 1;
        Queue<double> ApneaRmsQueue = new Queue<double>();
        Queue<double> ApneaPointQueue = new Queue<double>();

        // 心拍除去後の呼吸データ
        Queue<double> ApneaHeartBeatRemovQueue = new Queue<double>();

        // 呼吸データ(プロット)
        Queue<double> ApneaGraphPlotQueue = new Queue<double>();

        // 演算結果データ
        private const int BufNumApneaGraph = 11;            // 1分(10データ)分だけ表示する // 0点も打つので+1
        Queue<double> ApneaQueue = new Queue<double>();
        Queue<double> ResultIbikiQueue = new Queue<double>();

        // G1Dでの判定結果データ
        Queue<double> ResultG1DApneaQueue = new Queue<double>();
        Queue<double> ResultG1DSnoreQueue = new Queue<double>();

        object lockData = new object();
        object lockData_Acce = new object();
        object lockData_g1d = new object();
        
        // グラフ用
        private const int SpO2_RawDataRirekiNum = 128;              // 生データ履歴数
        
        // 演算結果保存向けデータ-------
        // 保存rootパス
        private string ApneaRootPath_;
        private string AcceRootPath_;
        private string RecordRootPath_;
        private string recordFilePath;

        private int ApneaCalcCount_;
//        private int Acce_PhotoRefCalcCount_;

        private int SnoreParamThre;         // いびき閾値
        private int SnoreParamNormalCnt;    // いびき無しへのカウント
        private int ApneaJudgeCnt;          // 無呼吸判定カウント
        private double ApneaParamBinThre;   // 2値化50%閾値
        private int MaxEdgeThre;            // エッジ強調移動平均値の判定上限
        private int MinSnoreThre;           // いびき音の判定下限
        private int MinBreathThre;          // 生の呼吸音の判定下限
        private int DiameterCenter;         // エッジ強調移動平均の中心の倍率
        private int DiameterNext;           // エッジ強調移動平均の隣の倍率
        private double DiameterEnd;			// エッジ強調移動平均の端の倍率

        private StripLine stripLineSnore;

        public int snore = 0;
        public int apnea = 0;

        public int g1d_snore = 0;
        public int g1d_apnea = 0;

        private const int averageNum = 200;
        private int averageCnt = 0;
        private double kokyuSum = 0;
        private double ibikiSum = 0;
        private double kokyuAve = 0;
        private double ibikiAve = 0;
        private int elapsedTime = 0;

        // 情報取得コマンド
        static ManagementObjectSearcher MyOCS = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
        public delegate void DelegateUpdateText();
        
        // ログ出力パス
        string logPath = "C:\\log";
        string iniFileName = "setting.ini";
        static int periodCnt = 0;
        static bool stopFlg = false;
        
        public FormMain()
        {
            string icon = AppDomain.CurrentDomain.BaseDirectory + "analysis.ico";
            InitializeComponent();

            if(File.Exists(icon))
                this.Icon = new System.Drawing.Icon(icon);
        }

        /************************************************************************/
        /* 関数名   : Form1_Load          						    			*/
        /* 機能     : フォーム読み込み時のイベント	                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            logPath = logPath + "\\log.txt";

            log_output("[START-UP]App");

            // インスタンス生成
            initInstance();

            // 機能設定をiniファイルから取得
            getIniFileFuncSettingData();

            // 閾値などをiniファイルから取得
            getIniFileThresholdData();

            // グラフ初期設定
            initGraphShow();

            // COMポート取得
            string[] ports = com.GetPortNames();
            foreach (string port in ports)
            {
                comboBoxComport.Items.Add(port);
            }

            // 演算データ保存向け初期化処理
            //CreateRootDir(); //(移動)USB検索後にルート設定
            ApneaCalcCount_ = 0;
//            Acce_PhotoRefCalcCount_ = 0;

            // グラフ更新
            GraphUpdate_Apnea();

            // インターバル処理
            Timer timer = new Timer();
            timer.Tick += new EventHandler(Interval);
            timer.Interval = 10;           // ms単位
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
            Boolean ret = true;

            panda.setComPort_Lattepanda();

            // ログ出力フォルダ作成
            CreateRootDir();

            // 解析スタートでLATTEPANDAのLEDを消灯。
            panda.requestLattepanda((byte)request.LED_OFF);

            if (AUTO_ANALYSIS)
            {
                if(SOUND_RECORD)
                {
                    // 録音開始
                    ret = record.startRecordApnea();
                }

                if (ret)
                {
                    // 解析
                    ret = startAnalysis();
                    if (ret)
                    {
                        com.DataReceived += ComPort_DataReceived;   // コールバックイベント追加
                        buttonStart.Text = "データ取得中";
                        buttonStart.Enabled = false;
                        log_output("[START]Analysis_Auto");
                        buttonSetting.Enabled = true;
                    }
                    else
                    {
                        record.stopRecordApnea();
                    }
                }
            }
            else
            {
                buttonSetting.Enabled = false;
            }

            if (!ret)
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
            com.Close();
        }

        /************************************************************************/
        /* 関数名   : initInstance                    			    			*/
        /* 機能     : インスタンス生成           	                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void initInstance()
        {
            com = new ComPort();
            formset = new FormSetting();
            record = new SoundRecord();
            panda = new LattePanda();

            formset.form = this;
            record.form = this;
        }

        /************************************************************************/
        /* 関数名   : getIniFileFuncSettingData            		    			*/
        /* 機能     : iniファイルから機能設定を取得                             */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void getIniFileFuncSettingData()
        {
            // iniファイル名を決める（実行ファイルが置かれたフォルダと同じ場所）
            StringBuilder sb = new StringBuilder(1024);
            string filePath = AppDomain.CurrentDomain.BaseDirectory + iniFileName;

            // iniファイルからUSB出力設定を取得
            GetPrivateProfileString(
                "FUNCTION",
                "USB_OUTPUT",
                "ON",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                USB_OUTPUT = true;
            }
            else
            {
                USB_OUTPUT = false;
            }

            // iniファイルからCドライブ出力設定を取得
            GetPrivateProfileString(
                "FUNCTION",
                "C_DRIVE",
                "ON",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                C_DRIVE = true;
            }
            else
            {
                C_DRIVE = false;
            }

            // iniファイルから解析オートスタート設定を取得
            GetPrivateProfileString(
                "FUNCTION",
                "AUTO_ANALYSIS",
                "ON",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                AUTO_ANALYSIS = true;
            }
            else
            {
                AUTO_ANALYSIS = false;
            }

            // iniファイルから音声録音設定を取得
            GetPrivateProfileString(
                "FUNCTION",
                "SOUND_RECORD",
                "ON",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                SOUND_RECORD = true;
            }
            else
            {
                SOUND_RECORD = false;
            }

            // iniファイルからラテパンダコマンド送信設定を取得
            GetPrivateProfileString(
                "FUNCTION",
                "LATTEPANDA",
                "ON",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                panda.setLattepandaFuncSetting(true);
            }
            else
            {
                panda.setLattepandaFuncSetting(false);
            }
            // iniファイルから画面表示設定を取得
            GetPrivateProfileString(
                "WINDOW_STATE",
                "STATE",
                "MINIMIZED",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "NORMAL")
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }

        }

        /************************************************************************/
        /* 関数名   : getIniFileData                   			    			*/
        /* 機能     : iniファイルから閾値データを取得                           */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void getIniFileThresholdData()
        {
            // iniファイル名を決める（実行ファイルが置かれたフォルダと同じ場所）
            StringBuilder sb = new StringBuilder(1024);
            string filePath = AppDomain.CurrentDomain.BaseDirectory + iniFileName;

            // iniファイルからいびきの閾値を取得
            GetPrivateProfileString(
                "SNORE",
                "THRESHOLD",
                "245",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            SnoreParamThre = int.Parse(sb.ToString());

            // iniファイルからいびき無しへのカウントを取得
            GetPrivateProfileString(
                "SNORE",
                "NORMAL_COUNT",
                "80",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            SnoreParamNormalCnt = int.Parse(sb.ToString());

            // iniファイルから無呼吸判定カウントを取得
            GetPrivateProfileString(
                "APNEA",
                "APNEA_JUDGE_CNT",
                "0",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            ApneaJudgeCnt = int.Parse(sb.ToString());

            // iniファイルから 2値化50%閾値を取得
            GetPrivateProfileString(
                "APNEA",
                "APNEA_PARAM_BIN_THRE",
                "0.002",       // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            ApneaParamBinThre = double.Parse(sb.ToString());

            // iniファイルから取得した閾値をApneaにセットする
            setThreshold(SnoreParamThre, SnoreParamNormalCnt, ApneaJudgeCnt, ApneaParamBinThre);

            // iniファイルからエッジ強調移動平均値の判定上限を取得
            GetPrivateProfileString(
                "APNEA",
                "MAX_EDGE_THRESHOLD",
                "1000",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            MaxEdgeThre = int.Parse(sb.ToString());

            // iniファイルからいびき音の判定下限を取得
            GetPrivateProfileString(
                "APNEA",
                "MIN_SNORE_THRESHOLD",
                "100",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            MinSnoreThre = int.Parse(sb.ToString());

            // iniファイルから生の呼吸音の判定下限を取得
            GetPrivateProfileString(
                "APNEA",
                "MIN_BREATH_THRESHOLD",
                "50",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            MinBreathThre = int.Parse(sb.ToString());

            // iniファイルからエッジ強調移動平均の中心の倍率を取得
            GetPrivateProfileString(
                "APNEA",
                "DIAMETER_CENTER",
                "20",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            DiameterCenter = int.Parse(sb.ToString());

            // iniファイルからエッジ強調移動平均の隣の倍率を取得
            GetPrivateProfileString(
                "APNEA",
                "DIAMETER_NEXT",
                "10",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            DiameterNext = int.Parse(sb.ToString());

            // iniファイルからエッジ強調移動平均の端の倍率を取得
            GetPrivateProfileString(
                "APNEA",
                "DIAMETER_END",
                "0.1",       // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            DiameterEnd = double.Parse(sb.ToString());

            // iniファイルから取得したエッジ強調処理の閾値をApneaにセットする
            setEdgeThreshold(MaxEdgeThre, MinSnoreThre, MinBreathThre, DiameterCenter, DiameterNext, DiameterEnd);
        }

        /************************************************************************/
        /* 関数名   : initGraphShow                    			    			*/
        /* 機能     : グラフ表示の初期化           	                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void initGraphShow()
        {
            // いびきの閾値表示
            stripLineSnore = new StripLine
            {
                Interval = 0,
                IntervalOffset = SnoreParamThre,    // いびきの閾値(SNORE_PARAM_THRE)
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderColor = Color.Red,
            };
            chart_hertBeatRemoveRawData.ChartAreas[0].AxisY.StripLines.Add(stripLineSnore);

            // 無呼吸の閾値表示
            StripLine stripLineApnea = new StripLine
            {
                Interval = 0,
                IntervalOffset = ApneaParamBinThre,    // 無呼吸の閾値(APNEA_PARAM_BIN_THRE)
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderColor = Color.Blue,
            };

            // グラフ表示初期化
            // For Apnea
            RawDataRespQueue.Clear();
            RawDataSnoreQueue.Clear();
            RawDataDcQueue.Clear();
            ApneaRmsQueue.Clear();
            ApneaPointQueue.Clear();
            ApneaHeartBeatRemovQueue.Clear();
            AccelerometerXQueue.Clear();
            AccelerometerYQueue.Clear();
            AccelerometerZQueue.Clear();
            ApneaQueue.Clear();
            ResultIbikiQueue.Clear();
            ResultG1DApneaQueue.Clear();
            ResultG1DSnoreQueue.Clear();

            for (int i = 0; i < ApneaGraphDataNum; i++)
            {
                RawDataRespQueue.Enqueue(0);
                RawDataSnoreQueue.Enqueue(0);
                RawDataDcQueue.Enqueue(0);
                ApneaHeartBeatRemovQueue.Enqueue(0);
                ApneaGraphPlotQueue.Enqueue(0);
            }

            for (int i = 0; i < ApneaGraphCalculationDataNum; i++)
            {
                ApneaRmsQueue.Enqueue(0);
                ApneaPointQueue.Enqueue(0);
            }

            for (int i = 0; i < BufNumApneaGraph; i++)
            {
                ApneaQueue.Enqueue(0);
                ResultIbikiQueue.Enqueue(0);
                ResultG1DApneaQueue.Enqueue(0);
                ResultG1DSnoreQueue.Enqueue(0);
            }

            for (int i = 0; i < AcceAndPhotoRef_RawDataRirekiNum; i++)
            {
                AccelerometerXQueue.Enqueue(0);
                AccelerometerYQueue.Enqueue(0);
                AccelerometerZQueue.Enqueue(0);
            }
        }

        /************************************************************************/
        /* 関数名   : startAnalysis             		                		*/
        /* 機能     : 解析スタート処理                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : Boolean : 成功 - true                                     */
        /*                      失敗 - false                  					*/
        /************************************************************************/
        private Boolean startAnalysis()
        {
            Boolean ret = false;
            com.BaudRate = 19200;
            com.Parity = Parity.Even;
            com.DataBits = 8;
            com.StopBits = StopBits.One;

            for (int i = 0; i < comboBoxComport.Items.Count; i++)
            {
                com.PortName = comboBoxComport.Items[i].ToString();
                if (com.PortName != "COM1" && com.PortName != "COM5")
                {
                    comboBoxComport.SelectedIndex = i;
                    if (String.IsNullOrWhiteSpace(com.PortName) == false)
                    {
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

            if (USB_OUTPUT)
            {
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
            }
            else
            {
                ret = true; // 無条件でtrue
            }
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
        /* 関数名   : ComPort_DataReceived            			    			*/
        /* 機能     : データ受信イベント                                        */
        /* 引数     : [byte[]] buffer - 受信データ                              */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void ComPort_DataReceived(byte[] buffer)
        {
            string text = "";

            //Console.WriteLine("ComPort_DataReceived");
            try
            {
                text = amari + Encoding.ASCII.GetString(buffer);
                amari = "";
                //string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //for (int i = 0; i < lines.Length; i++)
//                Console.WriteLine("[Data]:" + text);
                for (int i = 0; i < lines.Length - 1; i++)  // 最後のデータは切れている可能性があるので、次のデータと結合して処理する
                {
                    //空行チェック
                    if (lines[i].Length == 0)
                    {
                        continue;
                    }

                    //異常値チェック
                    string[] datas = lines[i].Split(new string[] { "," }, StringSplitOptions.None);
                    if (datas.Length == 2)
                    {
                        //測定データ表示
                        //SetTextInput(lines[i] + "\r\n");
                        //演算
                        int result;
                        if (!int.TryParse(datas[0], out result)) continue;      // マイク(呼吸)
                        if (!int.TryParse(datas[1], out result)) continue;      // マイク(いびき)

                        // For Apnea
                        if (labelState.Text == "検査中")
                        {
                            if (periodCnt == 5)
                            {
                                SetCalcData_Apnea(Convert.ToInt32(datas[0]), Convert.ToInt32(datas[1]));
                                periodCnt = 0;
                            }
                            else
                            {
                                periodCnt++;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("受信異常データ演算破棄(" + lines[i] + " i:" + i + "/" + lines.Length + ")");
                    }
                }
                amari = lines[lines.Length - 1];
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

            if (USB_OUTPUT)
            {
                char path_char = 'A';
                System.IO.DriveInfo drive;

                drivePath = "C:\\"; //初期値

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
            }
            else
            {
                if (C_DRIVE)
                {
                    drivePath = "C:\\";
                }
                else
                {
                    drivePath = AppDomain.CurrentDomain.BaseDirectory; //exeと同ディレクトリ
                }
            }

            // rootパス
            ApneaRootPath_ = drivePath + "\\ax\\apnea\\" + datestr + "\\";
            temp = ApneaRootPath_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = ApneaRootPath_ + "(" + i + ")";
                }
                else
                {
                    temp = temp + "\\";
                    Directory.CreateDirectory(temp);
                    break;
                }
            }

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
            if (SOUND_RECORD)
            {
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
            else
            {
                RecordRootPath_ = null;
                recordFilePath = RecordRootPath_;
            }
        }

        /************************************************************************/
        /* 関数名   : CreateApneaDir                     		    			*/
        /* 機能     : 無呼吸演算結果保存用パスの作成                            */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private string CreateApneaDir(int Count)
        {
            string path = ApneaRootPath_ + Count.ToString("D");
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
//                Directory.CreateDirectory(path);
//                Directory.CreateDirectory(path);
            }
            return path;
        }

        /************************************************************************/
        /* 関数名   : SetCalcData_Apnea               			    			*/
        /* 機能     : 呼吸・いびきのデータをセット                              */
        /* 引数     : [int] data1 - 呼吸の生データ                              */
        /*          : [int] data2 - いびきの生データ                            */
        /* 戻り値   : なし														*/
        /************************************************************************/
        // For Apnea
        private void SetCalcData_Apnea(int data1, int data2)
        {

            //計算用データ
            CalcDataList1.Add(data1);
            CalcDataList2.Add(data2);

            //グラフ用データ追加
            lock (lockData)
            {
                // 呼吸データ
                if (RawDataRespQueue.Count >= ApneaGraphDataNum)
                {
                    RawDataRespQueue.Dequeue();
                }
                RawDataRespQueue.Enqueue(data1);

                // いびきデータ
                if (RawDataSnoreQueue.Count >= ApneaGraphDataNum)
                {
                    RawDataSnoreQueue.Dequeue();
                }
                RawDataSnoreQueue.Enqueue(data2);

            }

            kokyuSum += data1;
            ibikiSum += data2;
            averageCnt++;
            if (averageNum == averageCnt)
            {
                kokyuAve = kokyuSum / averageNum;
                ibikiAve = ibikiSum / averageNum;
                averageCnt = 0;
                kokyuSum = 0;
                ibikiSum = 0;
                Invoke(new DelegateUpdateText(averageTextUpDate));
            }

            if (CalcDataList1.Count >= CalcDataNumApnea)
            {
                //演算
                Calc_Apnea();

                //データクリア
                CalcDataList1.Clear();
                CalcDataList2.Clear();

                Invoke(new DelegateUpdateText(apneaRetShow));

                Invoke(new DelegateUpdateText(measStop));

                
            }
        }

        private void averageTextUpDate()
        {
            IntPtr pathptr = Marshal.StringToHGlobalAnsi(ApneaRootPath_);

            ibikiAve = get_heartBeatRemoveAve();

            Math.Round(kokyuAve, 2, MidpointRounding.AwayFromZero);
            Math.Round(ibikiAve, 2, MidpointRounding.AwayFromZero);

            set_averageData(kokyuAve, ibikiAve, pathptr);

            switch (elapsedTime)
            {
                case 0:
                    label_kokyuAve_1.Text = kokyuAve.ToString();
                    label_ibikiAve_1.Text = ibikiAve.ToString();
                    elapsedTime = 0;
                    break;
/*
                case 1:
                    label_kokyuAve_2.Text = kokyuAve.ToString();
                    label_ibikiAve_2.Text = ibikiAve.ToString();
                    elapsedTime++;
                    break;
                case 2:
                    label_kokyuAve_3.Text = kokyuAve.ToString();
                    label_ibikiAve_3.Text = ibikiAve.ToString();
                    elapsedTime++;
                    break;
                case 3:
                    label_kokyuAve_4.Text = kokyuAve.ToString();
                    label_ibikiAve_4.Text = ibikiAve.ToString();
                    elapsedTime++;
                    break;
                case 4:
                    label_kokyuAve_5.Text = kokyuAve.ToString();
                    label_ibikiAve_5.Text = ibikiAve.ToString();
                    elapsedTime = 0;
                    break;
*/
            }
        }

        /************************************************************************/
        /* 関数名   : SetCalcData_Acce                        			    	*/
        /* 機能     : 加速度センサーのデータをセット                            */
        /* 引数     : [int] data1 - X軸                                         */
        /*          : [int] data2 - Y軸                                         */
        /*          : [int] data3 - Z軸                                         */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void SetCalcData_Acce(int data1, int data2, int data3)
        {
            lock (lockData_Acce)
            {
                //グラフ用データ追加
                // X軸
                if (AccelerometerXQueue.Count >= AcceAndPhotoRef_RawDataRirekiNum)
                {
                    AccelerometerXQueue.Dequeue();
                }
                AccelerometerXQueue.Enqueue(data1);
                // Y軸
                if (AccelerometerYQueue.Count >= AcceAndPhotoRef_RawDataRirekiNum)
                {
                    AccelerometerYQueue.Dequeue();
                }
                AccelerometerYQueue.Enqueue(data2);
                // Z軸
                if (AccelerometerZQueue.Count >= AcceAndPhotoRef_RawDataRirekiNum)
                {
                    AccelerometerZQueue.Dequeue();
                }
                AccelerometerZQueue.Enqueue(data3);

                //10回に1回テキスト出力する(500ms毎)（暫定）
                //50msごとに出力すると約1時間で１フォルダ内のフォルダ数の限界がくるため
                //理想は1テキスト内に出力し続ける
//                string path = CreateAcceDir(Acce_PhotoRefCalcCount_);
//                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
//                get_accelerometer((double)data1, (double)data2, (double)data3, pathptr);
            }
        }

        /************************************************************************/
        /* 関数名   : SetJudgeResult               			                   	*/
        /* 機能     : 判定結果のデータをセット                                  */
        /* 引数     : [int] data1 - いびきの判定結果                            */
        /*          : [int] data2 - 無呼吸の判定結果                            */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void SetJudgeResult(int data1, int data2)
        {
            lock (lockData_g1d)
            {
                if (ResultG1DSnoreQueue.Count >= BufNumApneaGraph)
                {
                    ResultG1DSnoreQueue.Dequeue();
                }
                ResultG1DSnoreQueue.Enqueue(data1);
                if (ResultG1DApneaQueue.Count >= BufNumApneaGraph)
                {
                    ResultG1DApneaQueue.Dequeue();
                }
                ResultG1DApneaQueue.Enqueue(data2);
                g1d_snore = data1;
                g1d_apnea = data2;
            }
        }

        /************************************************************************/
        /* 関数名   : Calc_Apnea                    			    			*/
        /* 機能     : 呼吸データの演算                                          */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void Calc_Apnea()
        {
            try
            {
                string path = CreateApneaDir(ApneaCalcCount_);
                ApneaCalcCount_ += 1;

                int num = CalcDataNumApnea;
                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr ptr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr pd = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(double)) * num);
                int[] arrayi = new int[num];
                double[] arrayd = new double[num];
                Marshal.Copy(CalcDataList1.ToArray(), 0, ptr, num);
                Marshal.Copy(CalcDataList2.ToArray(), 0, ptr2, num);
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                getwav_init(ptr, num, pathptr, ptr2);
                lock (lockData)
                {
                    // 心拍除去後のデータをQueueに置く
                    getwav_heartbeat_remov_dc(pd);
                    Marshal.Copy(pd, arrayd, 0, CalcDataNumApnea);
                    for (int ii = 0; ii < CalcDataNumApnea; ++ii)
                    {
                        ApneaHeartBeatRemovQueue.Dequeue();
                        ApneaHeartBeatRemovQueue.Enqueue(arrayd[ii]);
                    }
                }
                Marshal.FreeCoTaskMem(ptr);
                Marshal.FreeCoTaskMem(ptr2);
                Marshal.FreeCoTaskMem(pd);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "内部エラー(Calc)");
                Console.WriteLine(ex.Message);
            }
        }

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
                // 生データグラフを更新
                Series srs_rawresp = chart_kokyuRowData.Series["呼吸音"]; //■
                Series srs_rawhertBeatRemove = chart_hertBeatRemoveRawData.Series["心拍除去後の呼吸音"]; //■
                Series srs_rawsnore = chart_snoreRow.Series["いびき音"]; //■
                srs_rawresp.Points.Clear();
                srs_rawhertBeatRemove.Points.Clear();
                srs_rawsnore.Points.Clear();
                srs_rawsnore.Color = Color.GreenYellow;
                cnt = 0;
                foreach (double data in RawDataRespQueue)
                {
                    srs_rawresp.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaHeartBeatRemovQueue)
                {
                    srs_rawhertBeatRemove.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataSnoreQueue)
                {
                    srs_rawsnore.Points.AddXY(cnt, data);
                    cnt++;
                }
            }
            // 更新実行
            chart_kokyuRowData.Invalidate();
            chart_hertBeatRemoveRawData.Invalidate();
            chart_snoreRow.Invalidate();
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
            byte[] param = new byte[1];

            if (buttonStart.Text == "測定開始")
            {
                if (stopFlg == false)
                {
                    com.PortName = comboBoxComport.Text;
                    com.BaudRate = 19200;
                    com.Parity = Parity.Even;
                    com.DataBits = 8;
                    com.StopBits = StopBits.One;
                    Boolean ret = com.Start();
                    if (ret)
                    {

                        com.DataReceived += ComPort_DataReceived;   // コールバックイベント追加
                        buttonStart.Enabled = false;
                        buttonSetting.Enabled = true;
                        labelState.Text = "検査中";
                        log_output("[START]Analysis(button)");

                        param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MIC_DIAG_MODE_START;
                        writeData(param);
                    }
                }
                else
                {
                    initGraphShow();

                    buttonStart.Enabled = false;
                    buttonSetting.Enabled = true;
                    labelState.Text = "検査中";
                    log_output("[START]Analysis(button)");

                    param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MIC_DIAG_MODE_START;
                    writeData(param);

                    stopFlg = false;
                }
            }
        }

/* コマンド送信 */
        /************************************************************************/
        /* 関数名   : buttonSetting_Click              		    		        */
        /* 機能     : 設定ボタンクリック時のイベント                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            if (com.myPort != null)
            {
                if (com.myPort.IsOpen)
                {
                    formset.Show();
                }
            }
        }

        /************************************************************************/
        /* 関数名   : writeData                      		    		        */
        /* 機能     : コマンド書き込み                                          */
        /* 引数     : [byte[]] buffer - 書き込むデータ                          */
        /* 戻り値   : true  - 成功　　　　　　 　　　　　　　　　　　　　　　　 */
        /*            false - 失敗                                            　*/
        /************************************************************************/
        public bool writeData(byte[] buffer)
        {
            bool ret = false;

            if (com.myPort != null)
            {
                if (com.myPort.IsOpen)
                {
                    com.WriteData(buffer);
                    ret = true;
                }
            }
            return ret;
        }

        /************************************************************************/
        /* 関数名   : buttonVibConf_Click                         		        */
        /* 機能     : バイブ確認ボタンクリック時のイベント                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void buttonVibConf_Click(object sender, EventArgs e)
        {
            byte[] param = new byte[1];

            if (radioButtonVibConfWeak.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_WEAK_CONF;
            }
            else if (radioButtonVibConfMed.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_MED_CONF;
            }
            else if (radioButtonVibConfStrong.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_STRONG_CONF;
            }
            else if (radioButtonVibConfGrad.Checked == true)
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_GRAD_CONF;
            }

            if (!writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("エラー");
            }
        }

        /************************************************************************/
        /* 関数名   : buttonEnd_Click                        	    	        */
        /* 機能     : 測定終了ボタンクリック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            byte[] param = new byte[1];

            param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MIC_DIAG_MODE_END;
            writeData(param);

            com.Close();

            labelState.Text = "待機中";
            buttonStart.Enabled = true;
        }

        /************************************************************************/
        /* 関数名   : setSnoreThreUpdate                      	    	        */
        /* 機能     : いびき閾値ライン更新                                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        public void setSnoreThreUpdate(int snoreSens)
        {
            if ((int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_WEAK == snoreSens)
            {
                SnoreParamThre = 300;
            }
            else if ((int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_MED == snoreSens)
            {
                SnoreParamThre = 245;
            }
            else if ((int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_STRONG == snoreSens)
            {
                SnoreParamThre = 200;
            }
            setThreshold(SnoreParamThre, SnoreParamNormalCnt, ApneaJudgeCnt, ApneaParamBinThre);

            // いびきの閾値表示
            chart_hertBeatRemoveRawData.ChartAreas[0].AxisY.StripLines.Remove(stripLineSnore);
            stripLineSnore.IntervalOffset = SnoreParamThre;
            chart_hertBeatRemoveRawData.ChartAreas[0].AxisY.StripLines.Add(stripLineSnore);
        }

        /************************************************************************/
        /* 関数名   : measStop                               	    	        */
        /* 機能     : 測定停止                                                  */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void measStop()
        {
            byte[] param = new byte[1];

            param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MIC_DIAG_MODE_END;
            writeData(param);

            //            com.Close();

            stopFlg = true;

            labelState.Text = "待機中";
            buttonStart.Enabled = true;
            buttonEnd.Enabled = false;

        }

        /************************************************************************/
        /* 関数名   : apneaRetShow                            	    	        */
        /* 機能     : 無呼吸判定結果を表示する                                  */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void apneaRetShow()
        {
            int state = get_state();
            int snore;
            int apnea;

            snore = state & 0x01;
            apnea = (state & 0xC0) >> 6;

//            rmsRetShow();

            if (apnea == 2)
            {
                label_apnea.Text = "無呼吸";
            }
            else if(snore == 1)
            {
                label_apnea.Text = "いびき";
            }
            else
            {
                label_apnea.Text = "通常呼吸";
            }
        }

        /************************************************************************/
        /* 関数名   : rmsRetShow                            	    	        */
        /* 機能     : rmsを表示する                                             */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void rmsRetShow()
        {
            try
            {
                double[] arrayRmsData = new double[10];
                IntPtr rmsData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(double)) * 10);

                get_apnea_rms(rmsData);
                Marshal.Copy(rmsData, arrayRmsData, 0, 10);

                lock (lockData)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Math.Round(arrayRmsData[i], 3, MidpointRounding.AwayFromZero);
                    }
                }
                Marshal.FreeCoTaskMem(rmsData);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "内部エラー(Calc)");
                Console.WriteLine(ex.Message);
            }

    /*
                label_rms1.Text = arrayRmsData[0].ToString();
                if(arrayRmsData[0] >= 0.002)
                {
                    label_rms1.ForeColor = Color.Red;
                }

                label_rms2.Text = arrayRmsData[1].ToString();
                if (arrayRmsData[1] >= 0.002)
                {
                    label_rms2.ForeColor = Color.Red;
                }

                label_rms3.Text = arrayRmsData[2].ToString();
                if (arrayRmsData[2] >= 0.002)
                {
                    label_rms3.ForeColor = Color.Red;
                }

                label_rms4.Text = arrayRmsData[3].ToString();
                if (arrayRmsData[3] >= 0.002)
                {
                    label_rms4.ForeColor = Color.Red;
                }

                label_rms5.Text = arrayRmsData[4].ToString();
                if (arrayRmsData[4] >= 0.002)
                {
                    label_rms5.ForeColor = Color.Red;
                }

                label_rms6.Text = arrayRmsData[5].ToString();
                if (arrayRmsData[5] >= 0.002)
                {
                    label_rms6.ForeColor = Color.Red;
                }

                label_rms7.Text = arrayRmsData[6].ToString();
                if (arrayRmsData[6] >= 0.002)
                {
                    label_rms7.ForeColor = Color.Red;
                }

                label_rms8.Text = arrayRmsData[7].ToString();
                if (arrayRmsData[7] >= 0.002)
                {
                    label_rms8.ForeColor = Color.Red;
                }

                label_rms9.Text = arrayRmsData[8].ToString();
                if (arrayRmsData[8] >= 0.002)
                {
                    label_rms9.ForeColor = Color.Red;
                }

                label_rms10.Text = arrayRmsData[9].ToString();
                if (arrayRmsData[9] >= 0.002)
                {
                    label_rms10.ForeColor = Color.Red;
                }
                */
}
    }
}
