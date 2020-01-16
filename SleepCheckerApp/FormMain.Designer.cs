namespace SleepCheckerApp
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend9 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title9 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend10 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title10 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.buttonStart = new System.Windows.Forms.Button();
            this.chartApnea = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRawData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCalculation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartAccelerometer = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_acl_z = new System.Windows.Forms.CheckBox();
            this.checkBox_acl_y = new System.Windows.Forms.CheckBox();
            this.checkBox_acl_x = new System.Windows.Forms.CheckBox();
            this.checkBox_apneapoint = new System.Windows.Forms.CheckBox();
            this.checkBox_apnearms = new System.Windows.Forms.CheckBox();
            this.groupBoxCom = new System.Windows.Forms.GroupBox();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.comboBoxComport = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonVibConf = new System.Windows.Forms.Button();
            this.chartHeartBeatRemov = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.radioButtonVibConfWeak = new System.Windows.Forms.RadioButton();
            this.radioButtonVibConfMed = new System.Windows.Forms.RadioButton();
            this.radioButtonVibConfStrong = new System.Windows.Forms.RadioButton();
            this.radioButtonVibConfGrad = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.chartApnea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCalculation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAccelerometer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBoxCom.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHeartBeatRemov)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(120, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(108, 30);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "測定開始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // chartApnea
            // 
            this.chartApnea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea6.AxisX.Interval = 1D;
            chartArea6.AxisX.Maximum = 10D;
            chartArea6.AxisX.Minimum = 0D;
            chartArea6.AxisY.Interval = 1D;
            chartArea6.AxisY.Maximum = 3D;
            chartArea6.AxisY.Minimum = 0D;
            chartArea6.Name = "ChartAreaTime";
            this.chartApnea.ChartAreas.Add(chartArea6);
            legend6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend6.Name = "Legend1";
            this.chartApnea.Legends.Add(legend6);
            this.chartApnea.Location = new System.Drawing.Point(6, 366);
            this.chartApnea.Name = "chartApnea";
            series11.BorderWidth = 3;
            series11.ChartArea = "ChartAreaTime";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Legend = "Legend1";
            series11.Name = "無呼吸";
            series12.ChartArea = "ChartAreaTime";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Legend = "Legend1";
            series12.Name = "いびき";
            this.chartApnea.Series.Add(series11);
            this.chartApnea.Series.Add(series12);
            this.chartApnea.Size = new System.Drawing.Size(1334, 290);
            this.chartApnea.TabIndex = 11;
            this.chartApnea.Text = "ステータス";
            title6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title6.Name = "Title";
            title6.Text = "いびき/無呼吸判定";
            this.chartApnea.Titles.Add(title6);
            // 
            // chartRawData
            // 
            this.chartRawData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea7.AxisX.Interval = 200D;
            chartArea7.AxisX.Maximum = 2000D;
            chartArea7.AxisX.Minimum = 0D;
            chartArea7.AxisY.Maximum = 1024D;
            chartArea7.AxisY.Minimum = 0D;
            chartArea7.Name = "ChartAreaTime";
            this.chartRawData.ChartAreas.Add(chartArea7);
            legend7.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend7.Name = "Legend1";
            this.chartRawData.Legends.Add(legend7);
            this.chartRawData.Location = new System.Drawing.Point(6, 59);
            this.chartRawData.Name = "chartRawData";
            series13.ChartArea = "ChartAreaTime";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series13.Legend = "Legend1";
            series13.Name = "呼吸音";
            series14.ChartArea = "ChartAreaTime";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Legend = "Legend1";
            series14.Name = "いびき音";
            this.chartRawData.Series.Add(series13);
            this.chartRawData.Series.Add(series14);
            this.chartRawData.Size = new System.Drawing.Size(1334, 301);
            this.chartRawData.TabIndex = 15;
            this.chartRawData.Text = "呼吸音";
            title7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title7.Name = "Title";
            title7.Text = "呼吸音";
            this.chartRawData.Titles.Add(title7);
            // 
            // chartCalculation
            // 
            chartArea8.AxisX.Interval = 10D;
            chartArea8.AxisX.Maximum = 100D;
            chartArea8.AxisX.Minimum = 0D;
            chartArea8.AxisX2.Interval = 0.1D;
            chartArea8.AxisX2.Maximum = 0.5D;
            chartArea8.AxisX2.Minimum = 0D;
            chartArea8.Name = "ChartAreaTime";
            chartArea8.Position.Auto = false;
            chartArea8.Position.Height = 85F;
            chartArea8.Position.Width = 79F;
            chartArea8.Position.X = 0.7F;
            chartArea8.Position.Y = 13F;
            this.chartCalculation.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            legend8.Position.Auto = false;
            legend8.Position.Height = 19.79167F;
            legend8.Position.Width = 18.11263F;
            legend8.Position.X = 80F;
            legend8.Position.Y = 12.98242F;
            this.chartCalculation.Legends.Add(legend8);
            this.chartCalculation.Location = new System.Drawing.Point(57, 1);
            this.chartCalculation.Name = "chartCalculation";
            series15.ChartArea = "ChartAreaTime";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series15.Legend = "Legend1";
            series15.Name = "無呼吸(rms)";
            series16.ChartArea = "ChartAreaTime";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series16.Legend = "Legend1";
            series16.Name = "無呼吸(point)";
            this.chartCalculation.Series.Add(series15);
            this.chartCalculation.Series.Add(series16);
            this.chartCalculation.Size = new System.Drawing.Size(45, 38);
            this.chartCalculation.TabIndex = 20;
            title8.Name = "Title";
            title8.Text = "演算途中データ";
            this.chartCalculation.Titles.Add(title8);
            this.chartCalculation.Visible = false;
            // 
            // chartAccelerometer
            // 
            chartArea9.AxisX.Interval = 20D;
            chartArea9.AxisX.Maximum = 200D;
            chartArea9.AxisX.Minimum = 0D;
            chartArea9.AxisY.Maximum = 127D;
            chartArea9.AxisY.Minimum = -128D;
            chartArea9.Name = "ChartAreaTime";
            chartArea9.Position.Auto = false;
            chartArea9.Position.Height = 85F;
            chartArea9.Position.Width = 79F;
            chartArea9.Position.X = 1F;
            chartArea9.Position.Y = 13F;
            this.chartAccelerometer.ChartAreas.Add(chartArea9);
            legend9.Name = "Legend1";
            legend9.Position.Auto = false;
            legend9.Position.Height = 27.08333F;
            legend9.Position.Width = 10.8067F;
            legend9.Position.X = 80F;
            legend9.Position.Y = 12.98242F;
            this.chartAccelerometer.Legends.Add(legend9);
            this.chartAccelerometer.Location = new System.Drawing.Point(984, 7);
            this.chartAccelerometer.Name = "chartAccelerometer";
            series17.BorderWidth = 2;
            series17.ChartArea = "ChartAreaTime";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series17.Legend = "Legend1";
            series17.MarkerBorderWidth = 5;
            series17.Name = "X軸";
            series18.BorderWidth = 2;
            series18.ChartArea = "ChartAreaTime";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series18.Legend = "Legend1";
            series18.MarkerBorderWidth = 5;
            series18.Name = "Y軸";
            series19.BorderWidth = 2;
            series19.ChartArea = "ChartAreaTime";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series19.Legend = "Legend1";
            series19.MarkerBorderWidth = 5;
            series19.Name = "Z軸";
            this.chartAccelerometer.Series.Add(series17);
            this.chartAccelerometer.Series.Add(series18);
            this.chartAccelerometer.Series.Add(series19);
            this.chartAccelerometer.Size = new System.Drawing.Size(45, 41);
            this.chartAccelerometer.TabIndex = 21;
            this.chartAccelerometer.Text = "加速度センサー";
            title9.Name = "Title";
            title9.Text = "加速度センサー";
            this.chartAccelerometer.Titles.Add(title9);
            this.chartAccelerometer.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_acl_z);
            this.groupBox2.Controls.Add(this.checkBox_acl_y);
            this.groupBox2.Controls.Add(this.checkBox_acl_x);
            this.groupBox2.Controls.Add(this.checkBox_apneapoint);
            this.groupBox2.Controls.Add(this.checkBox_apnearms);
            this.groupBox2.Controls.Add(this.chartCalculation);
            this.groupBox2.Controls.Add(this.chartHeartBeatRemov);
            this.groupBox2.Location = new System.Drawing.Point(1029, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 68);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "呼吸切替";
            this.groupBox2.Visible = false;
            // 
            // checkBox_acl_z
            // 
            this.checkBox_acl_z.AutoSize = true;
            this.checkBox_acl_z.Location = new System.Drawing.Point(104, 39);
            this.checkBox_acl_z.Name = "checkBox_acl_z";
            this.checkBox_acl_z.Size = new System.Drawing.Size(43, 16);
            this.checkBox_acl_z.TabIndex = 10;
            this.checkBox_acl_z.Text = "Z軸";
            this.checkBox_acl_z.UseVisualStyleBackColor = true;
            this.checkBox_acl_z.CheckedChanged += new System.EventHandler(this.checkBox_acl_z_CheckedChanged);
            // 
            // checkBox_acl_y
            // 
            this.checkBox_acl_y.AutoSize = true;
            this.checkBox_acl_y.Location = new System.Drawing.Point(55, 39);
            this.checkBox_acl_y.Name = "checkBox_acl_y";
            this.checkBox_acl_y.Size = new System.Drawing.Size(43, 16);
            this.checkBox_acl_y.TabIndex = 9;
            this.checkBox_acl_y.Text = "Y軸";
            this.checkBox_acl_y.UseVisualStyleBackColor = true;
            this.checkBox_acl_y.CheckedChanged += new System.EventHandler(this.checkBox_acl_y_CheckedChanged);
            // 
            // checkBox_acl_x
            // 
            this.checkBox_acl_x.AutoSize = true;
            this.checkBox_acl_x.Location = new System.Drawing.Point(6, 39);
            this.checkBox_acl_x.Name = "checkBox_acl_x";
            this.checkBox_acl_x.Size = new System.Drawing.Size(43, 16);
            this.checkBox_acl_x.TabIndex = 8;
            this.checkBox_acl_x.Text = "X軸";
            this.checkBox_acl_x.UseVisualStyleBackColor = true;
            this.checkBox_acl_x.CheckedChanged += new System.EventHandler(this.checkBox_acl_x_CheckedChanged);
            // 
            // checkBox_apneapoint
            // 
            this.checkBox_apneapoint.AutoSize = true;
            this.checkBox_apneapoint.Location = new System.Drawing.Point(108, 17);
            this.checkBox_apneapoint.Name = "checkBox_apneapoint";
            this.checkBox_apneapoint.Size = new System.Drawing.Size(93, 16);
            this.checkBox_apneapoint.TabIndex = 7;
            this.checkBox_apneapoint.Text = "無呼吸(point)";
            this.checkBox_apneapoint.UseVisualStyleBackColor = true;
            this.checkBox_apneapoint.CheckedChanged += new System.EventHandler(this.checkBox_apneapoint_CheckedChanged);
            // 
            // checkBox_apnearms
            // 
            this.checkBox_apnearms.AutoSize = true;
            this.checkBox_apnearms.Location = new System.Drawing.Point(6, 17);
            this.checkBox_apnearms.Name = "checkBox_apnearms";
            this.checkBox_apnearms.Size = new System.Drawing.Size(87, 16);
            this.checkBox_apnearms.TabIndex = 6;
            this.checkBox_apnearms.Text = "無呼吸(rms)";
            this.checkBox_apnearms.UseVisualStyleBackColor = true;
            this.checkBox_apnearms.CheckedChanged += new System.EventHandler(this.checkBox_apnearms_CheckedChanged);
            // 
            // groupBoxCom
            // 
            this.groupBoxCom.Controls.Add(this.buttonSetting);
            this.groupBoxCom.Controls.Add(this.comboBoxComport);
            this.groupBoxCom.Controls.Add(this.buttonStart);
            this.groupBoxCom.Location = new System.Drawing.Point(13, 6);
            this.groupBoxCom.Name = "groupBoxCom";
            this.groupBoxCom.Size = new System.Drawing.Size(338, 47);
            this.groupBoxCom.TabIndex = 8;
            this.groupBoxCom.TabStop = false;
            this.groupBoxCom.Text = "ポート設定";
            // 
            // buttonSetting
            // 
            this.buttonSetting.Location = new System.Drawing.Point(234, 12);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(98, 29);
            this.buttonSetting.TabIndex = 16;
            this.buttonSetting.Text = "設定";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // comboBoxComport
            // 
            this.comboBoxComport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxComport.FormattingEnabled = true;
            this.comboBoxComport.Location = new System.Drawing.Point(6, 18);
            this.comboBoxComport.Name = "comboBoxComport";
            this.comboBoxComport.Size = new System.Drawing.Size(108, 20);
            this.comboBoxComport.TabIndex = 15;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButtonVibConfGrad);
            this.groupBox5.Controls.Add(this.radioButtonVibConfStrong);
            this.groupBox5.Controls.Add(this.radioButtonVibConfMed);
            this.groupBox5.Controls.Add(this.radioButtonVibConfWeak);
            this.groupBox5.Controls.Add(this.buttonVibConf);
            this.groupBox5.Location = new System.Drawing.Point(579, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(385, 47);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "バイブレーション";
            // 
            // buttonVibConf
            // 
            this.buttonVibConf.Location = new System.Drawing.Point(17, 18);
            this.buttonVibConf.Name = "buttonVibConf";
            this.buttonVibConf.Size = new System.Drawing.Size(115, 20);
            this.buttonVibConf.TabIndex = 5;
            this.buttonVibConf.Text = "バイブレーション確認";
            this.buttonVibConf.UseVisualStyleBackColor = true;
            this.buttonVibConf.Click += new System.EventHandler(this.buttonVibConf_Click);
            // 
            // chartHeartBeatRemov
            // 
            chartArea10.AxisX.Interval = 200D;
            chartArea10.AxisX.Maximum = 2000D;
            chartArea10.AxisX.Minimum = 0D;
            chartArea10.AxisX2.Interval = 0.1D;
            chartArea10.AxisX2.Maximum = 0.5D;
            chartArea10.AxisX2.Minimum = 0D;
            chartArea10.AxisY.Maximum = 1024D;
            chartArea10.AxisY.Minimum = 0D;
            chartArea10.Name = "ChartAreaTime";
            chartArea10.Position.Auto = false;
            chartArea10.Position.Height = 85F;
            chartArea10.Position.Width = 80F;
            chartArea10.Position.Y = 13F;
            this.chartHeartBeatRemov.ChartAreas.Add(chartArea10);
            legend10.Name = "Legend1";
            legend10.Position.Auto = false;
            legend10.Position.Height = 19.79167F;
            legend10.Position.Width = 18.11263F;
            legend10.Position.X = 80F;
            legend10.Position.Y = 12.98242F;
            this.chartHeartBeatRemov.Legends.Add(legend10);
            this.chartHeartBeatRemov.Location = new System.Drawing.Point(6, 1);
            this.chartHeartBeatRemov.Name = "chartHeartBeatRemov";
            series20.ChartArea = "ChartAreaTime";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series20.Legend = "Legend1";
            series20.Name = "心拍除去後呼吸";
            this.chartHeartBeatRemov.Series.Add(series20);
            this.chartHeartBeatRemov.Size = new System.Drawing.Size(45, 38);
            this.chartHeartBeatRemov.TabIndex = 28;
            title10.Name = "Title";
            title10.Text = "心拍除去後の呼吸波形";
            this.chartHeartBeatRemov.Titles.Add(title10);
            this.chartHeartBeatRemov.Visible = false;
            // 
            // radioButtonVibConfWeak
            // 
            this.radioButtonVibConfWeak.AutoSize = true;
            this.radioButtonVibConfWeak.Location = new System.Drawing.Point(152, 20);
            this.radioButtonVibConfWeak.Name = "radioButtonVibConfWeak";
            this.radioButtonVibConfWeak.Size = new System.Drawing.Size(35, 16);
            this.radioButtonVibConfWeak.TabIndex = 6;
            this.radioButtonVibConfWeak.Text = "弱";
            this.radioButtonVibConfWeak.UseVisualStyleBackColor = true;
            // 
            // radioButtonVibConfMed
            // 
            this.radioButtonVibConfMed.AutoSize = true;
            this.radioButtonVibConfMed.Checked = true;
            this.radioButtonVibConfMed.Location = new System.Drawing.Point(203, 20);
            this.radioButtonVibConfMed.Name = "radioButtonVibConfMed";
            this.radioButtonVibConfMed.Size = new System.Drawing.Size(35, 16);
            this.radioButtonVibConfMed.TabIndex = 7;
            this.radioButtonVibConfMed.TabStop = true;
            this.radioButtonVibConfMed.Text = "中";
            this.radioButtonVibConfMed.UseVisualStyleBackColor = true;
            // 
            // radioButtonVibConfStrong
            // 
            this.radioButtonVibConfStrong.AutoSize = true;
            this.radioButtonVibConfStrong.Location = new System.Drawing.Point(254, 20);
            this.radioButtonVibConfStrong.Name = "radioButtonVibConfStrong";
            this.radioButtonVibConfStrong.Size = new System.Drawing.Size(35, 16);
            this.radioButtonVibConfStrong.TabIndex = 8;
            this.radioButtonVibConfStrong.Text = "強";
            this.radioButtonVibConfStrong.UseVisualStyleBackColor = true;
            // 
            // radioButtonVibConfGrad
            // 
            this.radioButtonVibConfGrad.AutoSize = true;
            this.radioButtonVibConfGrad.Location = new System.Drawing.Point(305, 20);
            this.radioButtonVibConfGrad.Name = "radioButtonVibConfGrad";
            this.radioButtonVibConfGrad.Size = new System.Drawing.Size(74, 16);
            this.radioButtonVibConfGrad.TabIndex = 9;
            this.radioButtonVibConfGrad.Text = "徐々に強く";
            this.radioButtonVibConfGrad.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1347, 660);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chartApnea);
            this.Controls.Add(this.groupBoxCom);
            this.Controls.Add(this.chartAccelerometer);
            this.Controls.Add(this.chartRawData);
            this.Name = "FormMain";
            this.Text = "Sleeim";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.chartApnea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCalculation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAccelerometer)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxCom.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHeartBeatRemov)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartApnea;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRawData;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCalculation;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAccelerometer;

        private System.Windows.Forms.ComboBox comboBoxComport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxCom;
        private System.Windows.Forms.CheckBox checkBox_apneapoint;
        private System.Windows.Forms.CheckBox checkBox_apnearms;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_acl_x;
        private System.Windows.Forms.CheckBox checkBox_acl_z;
        private System.Windows.Forms.CheckBox checkBox_acl_y;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHeartBeatRemov;
        private System.Windows.Forms.Button buttonVibConf;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.RadioButton radioButtonVibConfGrad;
        private System.Windows.Forms.RadioButton radioButtonVibConfStrong;
        private System.Windows.Forms.RadioButton radioButtonVibConfMed;
        private System.Windows.Forms.RadioButton radioButtonVibConfWeak;
    }
}

