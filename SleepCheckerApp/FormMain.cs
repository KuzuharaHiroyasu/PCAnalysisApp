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
        static extern void get_photoreflector(double data, IntPtr ppath);
        [DllImport("Apnea.dll")]
        static extern void calc_snore_init();

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

        private Boolean USB_OUTPUT = true;
        private Boolean C_DRIVE = true;
        private Boolean AUTO_ANALYSIS = true;
        private Boolean SOUND_RECORD = true;

        private ComPort com = null;
        private SoundRecord record = null;
        private SoundAlarm alarm = null;
        private LattePanda panda = null;
        private Vibration vib = null;

        private const int CalcDataNumApnea = 200;           // 6秒間、50msに1回データ取得した数
        private const int CalcDataNumSpO2 = 128;            // 4秒間、50msに1回データ取得した数
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
        private const int ApneaGraphDataNum = CalcDataNumApnea * 10 + 1;
        Queue<double> RawDataRespQueue = new Queue<double>();
        Queue<double> RawDataSnoreQueue = new Queue<double>();
        Queue<double> RawDataDcQueue = new Queue<double>();

        // 加速度センサー
        private const int AcceAndPhotoRef_RawDataRirekiNum = 200;       // 生データ履歴数 500ms * 200個 = 100秒
        Queue<double> AccelerometerXQueue = new Queue<double>();
        Queue<double> AccelerometerYQueue = new Queue<double>();
        Queue<double> AccelerometerZQueue = new Queue<double>();

        // フォトリフレクタ
        Queue<double> PhotoRefQueue = new Queue<double>();

        // 演算途中データ
        private const int ApneaGraphCalculationDataNum = CalcDataNumCalculationApnea * 10 + 1;
        Queue<double> ApneaRmsQueue = new Queue<double>();
        Queue<double> ApneaPointQueue = new Queue<double>();

        // 演算結果データ
        private const int BufNumApneaGraph = 11;            // 1分(10データ)分だけ表示する // 0点も打つので+1
        Queue<double> ApneaQueue = new Queue<double>();
        Queue<double> ResultIbikiQueue = new Queue<double>();

        object lockData = new object();
        object lockData_Acce = new object();
        object lockData_PhotoRef = new object();

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
        private string ApneaRootPath_;
        private string PulseRootPath_ = null;
        private string AcceRootPath_;
        private string PhotoRefRootPath_;
        private string RecordRootPath_;
        private string recordFilePath;

        private int ApneaCalcCount_;
        private int PulseCalcCount_;
        private int Acce_PhotoRefCalcCount_;

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

        public int snore = 0;
        public int apnea = 0;

        // 情報取得コマンド
        static ManagementObjectSearcher MyOCS = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");

        // ログ出力パス
        string logPath = "C:\\log";
        string iniFileName = "setting.ini";

        public FormMain()
        {
            string icon = AppDomain.CurrentDomain.BaseDirectory + "analysis.ico";
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

            // アラーム音初期設定
            initAlarm();

            // COMポート取得
            string[] ports = com.GetPortNames();
            foreach (string port in ports)
            {
                comboBoxComport.Items.Add(port);
            }

            // 演算データ保存向け初期化処理
            //CreateRootDir(); //(移動)USB検索後にルート設定
            ApneaCalcCount_ = 0;
            PulseCalcCount_ = 0;
            Acce_PhotoRefCalcCount_ = 0;

            // グラフ更新
            GraphUpdate_Apnea();
            GraphUpdate_Acce();
            GraphUpdate_PhotoRef();

            // インターバル処理
            Timer timer = new Timer();
            timer.Tick += new EventHandler(Interval);
            timer.Interval = 100;           // ms単位
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
                    }
                    else
                    {
                        record.stopRecordApnea();
                    }
                }
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
            record = new SoundRecord();
            alarm = new SoundAlarm();
            panda = new LattePanda();
            vib = new Vibration();

            record.form = this;
            alarm.form = this;
            vib.form = this;
            vib.panda = panda;
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

            // iniファイルからいびきのバイブ設定を取得
            GetPrivateProfileString(
                "VIBRATION",
                "SNORE",
                "OFF",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                checkBox_vib_snore.Checked = true;
            }
            else
            {
                checkBox_vib_snore.Checked = false;
            }

            // iniファイルから無呼吸のバイブ設定を取得
            GetPrivateProfileString(
                "VIBRATION",
                "APNEA",
                "OFF",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                checkBox_vib_apnea.Checked = true;
            }
            else
            {
                checkBox_vib_apnea.Checked = false;
            }

            // iniファイルからいびきのアラーム設定を取得
            GetPrivateProfileString(
                "ALARM",
                "SNORE",
                "OFF",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                checkBox_alarm_snore.Checked = true;
            }
            else
            {
                checkBox_alarm_snore.Checked = false;
            }

            // iniファイルから無呼吸のアラーム設定を取得
            GetPrivateProfileString(
                "ALARM",
                "APNEA",
                "OFF",            // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            if (Path.GetFileName(sb.ToString()) == "ON")
            {
                checkBox_alarm_apnea.Checked = true;
            }
            else
            {
                checkBox_alarm_apnea.Checked = false;
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
                "350",            // 値が取得できなかった場合に返される初期値
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
                "2",            // 値が取得できなかった場合に返される初期値
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
            StripLine stripLine = new StripLine
            {
                Interval = 0,
                IntervalOffset = SnoreParamThre,    // いびきの閾値(SNORE_PARAM_THRE)
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderColor = Color.Green,
            };
            chartRawData.ChartAreas[0].AxisY.StripLines.Add(stripLine);
            
            // 無呼吸の閾値表示
            stripLine = new StripLine
            {
                Interval = 0,
                IntervalOffset = ApneaParamBinThre,    // 無呼吸の閾値(APNEA_PARAM_BIN_THRE)
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderColor = Color.Blue,
            };
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripLine);

            // グラフ表示初期化
            // For Apnea
            RawDataRespQueue.Clear();
            RawDataSnoreQueue.Clear();
            RawDataDcQueue.Clear();
            ApneaRmsQueue.Clear();
            ApneaPointQueue.Clear();
            AccelerometerXQueue.Clear();
            AccelerometerYQueue.Clear();
            AccelerometerZQueue.Clear();
            PhotoRefQueue.Clear();
            ApneaQueue.Clear();
            ResultIbikiQueue.Clear();

            for (int i = 0; i < ApneaGraphDataNum; i++)
            {
                RawDataRespQueue.Enqueue(0);
                RawDataSnoreQueue.Enqueue(0);
                RawDataDcQueue.Enqueue(0);
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
            }

            for (int i = 0; i < AcceAndPhotoRef_RawDataRirekiNum; i++)
            {
                AccelerometerXQueue.Enqueue(0);
                AccelerometerYQueue.Enqueue(0);
                AccelerometerZQueue.Enqueue(0);
                PhotoRefQueue.Enqueue(0);
            }

            // 表示設定
            Series srs = chartRawData.Series["呼吸(移動平均)"];
            srs.Enabled = false;
            srs = chartPhotoReflector.Series["フォトリフレクタ"];
            srs.Enabled = false;
        }

        /************************************************************************/
        /* 関数名   : initAlarm                       			    			*/
        /* 機能     : アラーム設定の初期処理       	                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void initAlarm()
        {
            // アラーム音取得
            StringBuilder sb = new StringBuilder(1024);
            string[] alarmFilesPath = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.wav", System.IO.SearchOption.TopDirectoryOnly);
            string filePath = AppDomain.CurrentDomain.BaseDirectory + iniFileName;
            string alarmFile;

            foreach (string alarmFilePath in alarmFilesPath)
            {
                alarmFile = Path.GetFileName(alarmFilePath);
                comboBoxAlarm.Items.Add(alarmFile);
            }
            // iniファイルからいびきの閾値を取得
            GetPrivateProfileString(
                "ALARM",
                "FILE_NAME",
                alarmFilesPath[0],   // 値が取得できなかった場合に返される初期値
                sb,
                Convert.ToUInt32(sb.Capacity),
                filePath);
            alarmFile = Path.GetFileName(sb.ToString());

            alarm.setInitAlarm(alarmFile);
            comboBoxAlarm.SelectedItem = alarm.getAlarmFile();
            log_output("AlarmFile:" + alarm.getAlarmFile());
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
                    if (datas.Length == 6)
                    {
                        //測定データ表示
                        //SetTextInput(lines[i] + "\r\n");
                        //演算
                        int result;
                        if (!int.TryParse(datas[0], out result)) continue;      // マイク(呼吸)
                        if (!int.TryParse(datas[1], out result)) continue;      // マイク(いびき)
                        if (!int.TryParse(datas[2], out result)) continue;      // 加速度センサ(X)
                        if (!int.TryParse(datas[3], out result)) continue;      // 加速度センサ(Y)
                        if (!int.TryParse(datas[4], out result)) continue;      // 加速度センサ(Z)
                        if (!int.TryParse(datas[5], out result)) continue;      // フォトセンサ

                        // For Apnea
                        SetCalcData_Apnea(Convert.ToInt32(datas[0]), Convert.ToInt32(datas[1]));

                        if (Convert.ToInt32(datas[2]) == 99 && Convert.ToInt32(datas[3]) == 99 && Convert.ToInt32(datas[4]) == 99 && Convert.ToInt32(datas[5]) == 0)
                        {
                            //log_output("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[3]) + " X軸:" + Convert.ToInt32(datas[4]) + " Y軸:" + Convert.ToInt32(datas[5]) + " Z軸:" + Convert.ToInt32(datas[6]));
                            //Console.WriteLine("[DataReceived]呼吸:" + Convert.ToInt32(datas[2]) + " いびき:" + Convert.ToInt32(datas[1]) + " X軸:" + Convert.ToInt32(datas[4]) + " Y軸:" + Convert.ToInt32(datas[5]) + " Z軸:" + Convert.ToInt32(datas[6]));
                        } else
                        {
                            // For 加速度
                            SetCalcData_Acce(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]), Convert.ToInt32(datas[4]));
                            //Console.WriteLine("[DataReceived] X軸:" + Convert.ToInt32(datas[2]) + " Y軸:" + Convert.ToInt32(datas[3]) + " Z軸:" + Convert.ToInt32(datas[4]));
                            // For フォトリフレクタ
                            SetCalcData_PhotoRef(Convert.ToInt32(datas[5]));
                            //Console.WriteLine("[DataReceived] フォトリフレクタ:" + Convert.ToInt32(datas[5]));
                            Acce_PhotoRefCalcCount_++;
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

            PhotoRefRootPath_ = drivePath + "\\ax\\photoref\\" + datestr + "\\";
            temp = PhotoRefRootPath_;
            for (i = 1; i < 20; i++)
            {
                if (Directory.Exists(temp))
                {
                    temp = PhotoRefRootPath_ + "(" + i + ")";
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
        /* 関数名   : CreatePulseDir                     		    			*/
        /* 機能     : SpO2演算結果保存用パスの作成                              */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private string CreatePulseDir(int Count)
        {
            string path = PulseRootPath_ + Count.ToString("D");
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
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /************************************************************************/
        /* 関数名   : CreatePhotoRefDir                		    			    */
        /* 機能     : フォトリフレクタ結果保存用パスの作成                      */
        /* 引数     : [int] Count - データ数                                    */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private string CreatePhotoRefDir(int Count)
        {

            string path = PhotoRefRootPath_ + Count.ToString("D");
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

            if (CalcDataList1.Count >= CalcDataNumApnea)
            {
                //演算
                Calc_Apnea();

                //データクリア
                CalcDataList1.Clear();
                CalcDataList2.Clear();
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
                Calc_SpO2();
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
                string path = CreateAcceDir(Acce_PhotoRefCalcCount_);
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                get_accelerometer((double)data1, (double)data2, (double)data3, pathptr);
            }
        }

        /************************************************************************/
        /* 関数名   : SetCalcData_PhotoRef               			          	*/
        /* 機能     : フォトリフレクタのデータをセット                          */
        /* 引数     : [int] data1 - フォトリフレクタ値                          */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void SetCalcData_PhotoRef(int data1)
        {
            lock (lockData_PhotoRef)
            {
                //グラフ用データ追加
                // フォトリフレクタ
                if (PhotoRefQueue.Count >= AcceAndPhotoRef_RawDataRirekiNum)
                {
                    PhotoRefQueue.Dequeue();
                }
                PhotoRefQueue.Enqueue(data1);

                //10回に1回テキスト出力する(500ms毎)（暫定）
                //50msごとに出力すると約1時間で１フォルダ内のフォルダ数の限界がくるため
                //理想は1テキスト内に出力し続ける
                string path = CreatePhotoRefDir(Acce_PhotoRefCalcCount_);
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                get_photoreflector((double)data1, pathptr);
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
                IntPtr pi = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * num);
                IntPtr pd = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(double)) * num);
                int[] arrayi = new int[num];
                double[] arrayd = new double[num];
                Marshal.Copy(CalcDataList1.ToArray(), 0, ptr, num);
                Marshal.Copy(CalcDataList2.ToArray(), 0, ptr2, num);
                IntPtr pathptr = Marshal.StringToHGlobalAnsi(path);
                getwav_init(ptr, num, pathptr, ptr2);
                lock (lockData)
                {
                    // DC成分除去データをQueueに置く
                    getwav_dc(pd);
                    Marshal.Copy(pd, arrayd, 0, num);
                    for (int ii = 0; ii < num; ++ii)
                    {
                        RawDataDcQueue.Dequeue();
                        RawDataDcQueue.Enqueue(arrayd[ii]);
                    }

                    // 無呼吸(rms)データをQueueに置く
                    get_apnea_rms(pd);
                    Marshal.Copy(pd, arrayd, 0, CalcDataNumCalculationApnea);
                    for (int ii = 0; ii < CalcDataNumCalculationApnea; ++ii)
                    {
                        ApneaRmsQueue.Dequeue();
                        ApneaRmsQueue.Enqueue(arrayd[ii]);
                    }

                    // 無呼吸(point)データをQueueに置く
                    get_apnea_point(pd);
                    Marshal.Copy(pd, arrayd, 0, CalcDataNumCalculationApnea);
                    for (int ii = 0; ii < CalcDataNumCalculationApnea; ++ii)
                    {
                        ApneaPointQueue.Dequeue();
                        ApneaPointQueue.Enqueue(arrayd[ii]);
                    }

                    // 演算結果データ
                    int state = get_state();
                    snore = state & 0x01;
                    apnea = (state & 0xC0) >> 6;
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

                    // アラーム鳴動
                    alarm.confirmAlarm();

                    // バイブレーション
                    vib.confirmVib((byte)request.VIBRATION);
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
                // いびき、呼吸状態グラフを更新
                Series srs_apnea = chartApnea.Series["呼吸状態"]; //■
                Series srs_snore = chartApnea.Series["いびき"]; //■
                srs_apnea.Points.Clear();
                srs_snore.Points.Clear();
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
                Series srs_rawresp = chartRawData.Series["呼吸(生データ)"]; //■
                Series srs_rawsnore = chartRawData.Series["いびき(生データ)"]; //■
                Series srs_dcresp = chartRawData.Series["呼吸(移動平均)"]; //■
                srs_rawresp.Points.Clear();
                srs_rawsnore.Points.Clear();
                srs_dcresp.Points.Clear();
                cnt = 0;
                foreach (double data in RawDataRespQueue)
                {
                    srs_rawresp.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataSnoreQueue)
                {
                    srs_rawsnore.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in RawDataDcQueue)
                {
                    srs_dcresp.Points.AddXY(cnt, data);
                    cnt++;
                }
                
                // 演算途中データグラフを更新
                Series srs_apnea_rms = chart1.Series["無呼吸(rms)"];
                Series srs_apnea_point = chart1.Series["無呼吸(point)"];
                srs_apnea_rms.Points.Clear();
                srs_apnea_point.Points.Clear();
                cnt = 0;
                foreach (double data in ApneaRmsQueue)
                {
                    srs_apnea_rms.Points.AddXY(cnt, data);
                    cnt++;
                }
                cnt = 0;
                foreach (double data in ApneaPointQueue)
                {
                    srs_apnea_point.Points.AddXY(cnt, data);
                    cnt++;
                }
            }
            // 更新実行
            chartApnea.Invalidate();
            chartRawData.Invalidate();
            chart1.Invalidate();
        }

        /************************************************************************/
        /* 関数名   : GraphUpdate_Acce               			    			*/
        /* 機能     : 加速度センサーのグラフを更新                              */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void GraphUpdate_Acce()
        {
            int cnt = 0;

            lock (lockData_Acce)
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
        /* 関数名   : GraphUpdate_PhotoRef          			    			*/
        /* 機能     : フォトリフレクタのグラフを更新                            */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void GraphUpdate_PhotoRef()
        {
            int cnt = 0;

            lock (lockData_PhotoRef)
            {
                // フォトリフレクタのグラフを更新
                Series photoRef = chartPhotoReflector.Series["フォトリフレクタ"]; //■

                photoRef.Points.Clear();

                foreach (double data in PhotoRefQueue)
                {
                    photoRef.Points.AddXY(cnt, data);
                    cnt++;
                }
            }
            chartPhotoReflector.Invalidate();
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
            GraphUpdate_Acce();
            GraphUpdate_PhotoRef();
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
                        SetCalcData_Apnea(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]));
                        // For PulseOximeter
                        SetCalcData_SpO2(Convert.ToInt32(datas[0]), Convert.ToInt32(datas[1]));
                    }
  				}
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
                com.PortName = comboBoxComport.Text;
                com.BaudRate = 76800;
                com.Parity = Parity.Even;
                com.DataBits = 8;
                com.StopBits = StopBits.One;
                if (String.IsNullOrWhiteSpace(com.PortName) == false)
                {
                    Boolean ret = com.Start();
                    if (ret)
                    {
                        com.DataReceived += ComPort_DataReceived;   // コールバックイベント追加
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
            if (SOUND_RECORD)
            {
                log_output("[BUTTON]Record Start");

                record.startRecordApnea();
            }
        }

        /************************************************************************/
        /* 関数名   : button_recordstop_Click          		    		   		*/
        /* 機能     : 録音停止ボタンクリック時のイベント                        */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void button_recordstop_Click(object sender, EventArgs e)
        {
            if (SOUND_RECORD)
            {
                log_output("[BUTTON]Record Stop");
                record.stopRecordApnea();
            }
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
            Series srs = chartRawData.Series["呼吸(生データ)"];
            if (checkBox_rawresp.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
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
            Series srs = chartRawData.Series["いびき(生データ)"];
            if (checkBox_rawsnore.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
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
            Series srs = chartRawData.Series["呼吸(移動平均)"];
            if (checkBox_dcresp.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
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
            Series srs = chart1.Series["無呼吸(rms)"];
            if (checkBox_apnearms.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
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
            Series srs = chart1.Series["無呼吸(point)"];
            if (checkBox_apneapoint.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_acl_x_CheckedChanged     	        		    */
        /* 機能     : X軸チェック時のイベント                                   */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_acl_x_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartAccelerometer.Series["X軸"];
            if (checkBox_acl_x.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_acl_y_CheckedChanged           	        		*/
        /* 機能     : Y軸チェック時のイベント                                   */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_acl_y_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartAccelerometer.Series["Y軸"];
            if (checkBox_acl_y.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_acl_z_CheckedChanged         	        		*/
        /* 機能     : Z軸チェック時のイベント                                   */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_acl_z_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartAccelerometer.Series["Z軸"];
            if (checkBox_acl_z.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
            }
        }

        /************************************************************************/
        /* 関数名   : checkBox_photo_CheckedChanged     	            		*/
        /* 機能     : フォトリフレクタチェック時のイベント                      */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void checkBox_photo_CheckedChanged(object sender, EventArgs e)
        {
            Series srs = chartPhotoReflector.Series["フォトリフレクタ"];
            if (checkBox_photo.Checked)
            {
                srs.Enabled = true;
            }
            else
            {
                srs.Enabled = false;
            }
        }

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
        /* 関数名   : comboBoxAlarm_SelectedIndexChanged      		    		*/
        /* 機能     : アラーム音変更時のイベント                                */
        /* 引数     : なし                                                      */
        /* 戻り値   : なし														*/
        /************************************************************************/
        private void comboBoxAlarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            alarm.changeAlarmContents();
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
