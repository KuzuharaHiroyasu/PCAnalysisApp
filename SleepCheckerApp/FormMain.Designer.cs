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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.buttonStart = new System.Windows.Forms.Button();
            this.chartApnea = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRawData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartAccelerometer = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_apneapoint = new System.Windows.Forms.CheckBox();
            this.checkBox_apnearms = new System.Windows.Forms.CheckBox();
            this.checkBox_dcresp = new System.Windows.Forms.CheckBox();
            this.checkBox_rawsnore = new System.Windows.Forms.CheckBox();
            this.checkBox_rawresp = new System.Windows.Forms.CheckBox();
            this.groupBoxCom = new System.Windows.Forms.GroupBox();
            this.comboBoxComport = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxAlarm = new System.Windows.Forms.ComboBox();
            this.button_alarmplay = new System.Windows.Forms.Button();
            this.checkBox_alarm_apnea = new System.Windows.Forms.CheckBox();
            this.checkBox_alarm_snore = new System.Windows.Forms.CheckBox();
            this.button_recordstop = new System.Windows.Forms.Button();
            this.button_recordstart = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_vib_apnea = new System.Windows.Forms.CheckBox();
            this.checkBox_vib_snore = new System.Windows.Forms.CheckBox();
            this.button_vibstart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartApnea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAccelerometer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBoxCom.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(175, 18);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(124, 39);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "開始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // chartApnea
            // 
            chartArea5.AxisX.Interval = 1D;
            chartArea5.AxisX.Maximum = 10D;
            chartArea5.AxisX.Minimum = 0D;
            chartArea5.AxisY.Interval = 1D;
            chartArea5.AxisY.Maximum = 4D;
            chartArea5.AxisY.Minimum = 0D;
            chartArea5.Name = "ChartAreaTime";
            this.chartApnea.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartApnea.Legends.Add(legend5);
            this.chartApnea.Location = new System.Drawing.Point(12, 324);
            this.chartApnea.Name = "chartApnea";
            series11.BorderWidth = 3;
            series11.ChartArea = "ChartAreaTime";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Legend = "Legend1";
            series11.Name = "呼吸状態";
            series12.ChartArea = "ChartAreaTime";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Legend = "Legend1";
            series12.Name = "いびき";
            this.chartApnea.Series.Add(series11);
            this.chartApnea.Series.Add(series12);
            this.chartApnea.Size = new System.Drawing.Size(658, 193);
            this.chartApnea.TabIndex = 11;
            this.chartApnea.Text = "ステータス";
            title5.Name = "Title";
            title5.Text = "無呼吸・低呼吸";
            this.chartApnea.Titles.Add(title5);
            // 
            // chartRawData
            // 
            chartArea6.AxisX.Interval = 200D;
            chartArea6.AxisX.Maximum = 2000D;
            chartArea6.AxisX.Minimum = 0D;
            chartArea6.AxisX.Title = "個数";
            chartArea6.AxisY.Maximum = 1024D;
            chartArea6.AxisY.Minimum = 0D;
            chartArea6.Name = "ChartAreaTime";
            this.chartRawData.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartRawData.Legends.Add(legend6);
            this.chartRawData.Location = new System.Drawing.Point(12, 125);
            this.chartRawData.Name = "chartRawData";
            series13.BorderWidth = 2;
            series13.ChartArea = "ChartAreaTime";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series13.Legend = "Legend1";
            series13.MarkerBorderWidth = 5;
            series13.Name = "呼吸(生データ)";
            series14.ChartArea = "ChartAreaTime";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Legend = "Legend1";
            series14.Name = "いびき(生データ)";
            series15.ChartArea = "ChartAreaTime";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series15.Legend = "Legend1";
            series15.Name = "呼吸(移動平均)";
            this.chartRawData.Series.Add(series13);
            this.chartRawData.Series.Add(series14);
            this.chartRawData.Series.Add(series15);
            this.chartRawData.Size = new System.Drawing.Size(658, 193);
            this.chartRawData.TabIndex = 15;
            this.chartRawData.Text = "生データ(呼吸)";
            title6.Name = "Title";
            title6.Text = "生データ(呼吸)";
            this.chartRawData.Titles.Add(title6);
            // 
            // chart1
            // 
            chartArea7.AxisX.Interval = 10D;
            chartArea7.AxisX.Maximum = 100D;
            chartArea7.AxisX.Minimum = 0D;
            chartArea7.AxisX.Title = "個数";
            chartArea7.AxisX2.Interval = 0.1D;
            chartArea7.AxisX2.Maximum = 0.5D;
            chartArea7.AxisX2.Minimum = 0D;
            chartArea7.Name = "ChartAreaTime";
            this.chart1.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            this.chart1.Legends.Add(legend7);
            this.chart1.Location = new System.Drawing.Point(676, 324);
            this.chart1.Name = "chart1";
            series16.ChartArea = "ChartAreaTime";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series16.Legend = "Legend1";
            series16.Name = "無呼吸(rms)";
            series17.ChartArea = "ChartAreaTime";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series17.Legend = "Legend1";
            series17.Name = "無呼吸(point)";
            this.chart1.Series.Add(series16);
            this.chart1.Series.Add(series17);
            this.chart1.Size = new System.Drawing.Size(658, 193);
            this.chart1.TabIndex = 20;
            title7.Name = "Title";
            title7.Text = "演算途中データ";
            this.chart1.Titles.Add(title7);
            // 
            // chartAccelerometer
            // 
            chartArea8.AxisX.Interval = 20D;
            chartArea8.AxisX.Maximum = 200D;
            chartArea8.AxisX.Minimum = 0D;
            chartArea8.AxisX.Title = "個数";
            chartArea8.AxisY.Maximum = 127D;
            chartArea8.AxisY.Minimum = -128D;
            chartArea8.AxisY.Title = "値";
            chartArea8.Name = "ChartAreaTime";
            this.chartAccelerometer.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.chartAccelerometer.Legends.Add(legend8);
            this.chartAccelerometer.Location = new System.Drawing.Point(676, 125);
            this.chartAccelerometer.Name = "chartAccelerometer";
            series18.BorderWidth = 2;
            series18.ChartArea = "ChartAreaTime";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series18.Legend = "Legend1";
            series18.MarkerBorderWidth = 5;
            series18.Name = "X軸";
            series19.BorderWidth = 2;
            series19.ChartArea = "ChartAreaTime";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series19.Legend = "Legend1";
            series19.MarkerBorderWidth = 5;
            series19.Name = "Y軸";
            series20.BorderWidth = 2;
            series20.ChartArea = "ChartAreaTime";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series20.Legend = "Legend1";
            series20.MarkerBorderWidth = 5;
            series20.Name = "Z軸";
            this.chartAccelerometer.Series.Add(series18);
            this.chartAccelerometer.Series.Add(series19);
            this.chartAccelerometer.Series.Add(series20);
            this.chartAccelerometer.Size = new System.Drawing.Size(658, 193);
            this.chartAccelerometer.TabIndex = 21;
            this.chartAccelerometer.Text = "加速度センサー";
            title8.Name = "Title";
            title8.Text = "加速度センサー";
            this.chartAccelerometer.Titles.Add(title8);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_apneapoint);
            this.groupBox2.Controls.Add(this.checkBox_apnearms);
            this.groupBox2.Controls.Add(this.checkBox_dcresp);
            this.groupBox2.Controls.Add(this.checkBox_rawsnore);
            this.groupBox2.Controls.Add(this.checkBox_rawresp);
            this.groupBox2.Location = new System.Drawing.Point(708, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 98);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "呼吸切替";
            // 
            // checkBox_apneapoint
            // 
            this.checkBox_apneapoint.AutoSize = true;
            this.checkBox_apneapoint.Checked = true;
            this.checkBox_apneapoint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_apneapoint.Location = new System.Drawing.Point(108, 41);
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
            this.checkBox_apnearms.Checked = true;
            this.checkBox_apnearms.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_apnearms.Location = new System.Drawing.Point(6, 41);
            this.checkBox_apnearms.Name = "checkBox_apnearms";
            this.checkBox_apnearms.Size = new System.Drawing.Size(87, 16);
            this.checkBox_apnearms.TabIndex = 6;
            this.checkBox_apnearms.Text = "無呼吸(rms)";
            this.checkBox_apnearms.UseVisualStyleBackColor = true;
            this.checkBox_apnearms.CheckedChanged += new System.EventHandler(this.checkBox_apnearms_CheckedChanged);
            // 
            // checkBox_dcresp
            // 
            this.checkBox_dcresp.AutoSize = true;
            this.checkBox_dcresp.Location = new System.Drawing.Point(215, 18);
            this.checkBox_dcresp.Name = "checkBox_dcresp";
            this.checkBox_dcresp.Size = new System.Drawing.Size(104, 16);
            this.checkBox_dcresp.TabIndex = 3;
            this.checkBox_dcresp.Text = "呼吸(移動平均)";
            this.checkBox_dcresp.UseVisualStyleBackColor = true;
            this.checkBox_dcresp.CheckedChanged += new System.EventHandler(this.checkBox_dcresp_CheckedChanged);
            // 
            // checkBox_rawsnore
            // 
            this.checkBox_rawsnore.AutoSize = true;
            this.checkBox_rawsnore.Checked = true;
            this.checkBox_rawsnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_rawsnore.Location = new System.Drawing.Point(108, 18);
            this.checkBox_rawsnore.Name = "checkBox_rawsnore";
            this.checkBox_rawsnore.Size = new System.Drawing.Size(101, 16);
            this.checkBox_rawsnore.TabIndex = 2;
            this.checkBox_rawsnore.Text = "いびき(生データ)";
            this.checkBox_rawsnore.UseVisualStyleBackColor = true;
            this.checkBox_rawsnore.CheckedChanged += new System.EventHandler(this.checkBox_rawsnore_CheckedChanged);
            // 
            // checkBox_rawresp
            // 
            this.checkBox_rawresp.AutoSize = true;
            this.checkBox_rawresp.Checked = true;
            this.checkBox_rawresp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_rawresp.Location = new System.Drawing.Point(6, 18);
            this.checkBox_rawresp.Name = "checkBox_rawresp";
            this.checkBox_rawresp.Size = new System.Drawing.Size(96, 16);
            this.checkBox_rawresp.TabIndex = 1;
            this.checkBox_rawresp.Text = "呼吸(生データ)";
            this.checkBox_rawresp.UseVisualStyleBackColor = true;
            this.checkBox_rawresp.CheckedChanged += new System.EventHandler(this.checkBox_rawresp_CheckedChanged);
            // 
            // groupBoxCom
            // 
            this.groupBoxCom.Controls.Add(this.comboBoxComport);
            this.groupBoxCom.Controls.Add(this.buttonStart);
            this.groupBoxCom.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCom.Name = "groupBoxCom";
            this.groupBoxCom.Size = new System.Drawing.Size(321, 66);
            this.groupBoxCom.TabIndex = 8;
            this.groupBoxCom.TabStop = false;
            this.groupBoxCom.Text = "データ読み込み";
            // 
            // comboBoxComport
            // 
            this.comboBoxComport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxComport.FormattingEnabled = true;
            this.comboBoxComport.Location = new System.Drawing.Point(26, 28);
            this.comboBoxComport.Name = "comboBoxComport";
            this.comboBoxComport.Size = new System.Drawing.Size(124, 20);
            this.comboBoxComport.TabIndex = 15;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxAlarm);
            this.groupBox3.Controls.Add(this.button_alarmplay);
            this.groupBox3.Controls.Add(this.checkBox_alarm_apnea);
            this.groupBox3.Controls.Add(this.checkBox_alarm_snore);
            this.groupBox3.Location = new System.Drawing.Point(340, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 100);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "アラーム";
            // 
            // comboBoxAlarm
            // 
            this.comboBoxAlarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlarm.FormattingEnabled = true;
            this.comboBoxAlarm.Location = new System.Drawing.Point(19, 23);
            this.comboBoxAlarm.Name = "comboBoxAlarm";
            this.comboBoxAlarm.Size = new System.Drawing.Size(170, 20);
            this.comboBoxAlarm.TabIndex = 5;
            this.comboBoxAlarm.SelectedIndexChanged += new System.EventHandler(this.comboBoxAlarm_SelectedIndexChanged);
            // 
            // button_alarmplay
            // 
            this.button_alarmplay.Location = new System.Drawing.Point(99, 64);
            this.button_alarmplay.Name = "button_alarmplay";
            this.button_alarmplay.Size = new System.Drawing.Size(90, 23);
            this.button_alarmplay.TabIndex = 4;
            this.button_alarmplay.Text = "テスト再生";
            this.button_alarmplay.UseVisualStyleBackColor = true;
            this.button_alarmplay.Click += new System.EventHandler(this.button_alarmplay_Click);
            // 
            // checkBox_alarm_apnea
            // 
            this.checkBox_alarm_apnea.AutoSize = true;
            this.checkBox_alarm_apnea.Location = new System.Drawing.Point(19, 74);
            this.checkBox_alarm_apnea.Name = "checkBox_alarm_apnea";
            this.checkBox_alarm_apnea.Size = new System.Drawing.Size(60, 16);
            this.checkBox_alarm_apnea.TabIndex = 3;
            this.checkBox_alarm_apnea.Text = "無呼吸";
            this.checkBox_alarm_apnea.UseVisualStyleBackColor = true;
            this.checkBox_alarm_apnea.CheckedChanged += new System.EventHandler(this.checkBox_Apnea_CheckedChanged);
            // 
            // checkBox_alarm_snore
            // 
            this.checkBox_alarm_snore.AutoSize = true;
            this.checkBox_alarm_snore.Location = new System.Drawing.Point(19, 52);
            this.checkBox_alarm_snore.Name = "checkBox_alarm_snore";
            this.checkBox_alarm_snore.Size = new System.Drawing.Size(53, 16);
            this.checkBox_alarm_snore.TabIndex = 2;
            this.checkBox_alarm_snore.Text = "いびき";
            this.checkBox_alarm_snore.UseVisualStyleBackColor = true;
            this.checkBox_alarm_snore.CheckedChanged += new System.EventHandler(this.checkBox_snore_CheckedChanged);
            // 
            // button_recordstop
            // 
            this.button_recordstop.Location = new System.Drawing.Point(174, 9);
            this.button_recordstop.Name = "button_recordstop";
            this.button_recordstop.Size = new System.Drawing.Size(75, 23);
            this.button_recordstop.TabIndex = 23;
            this.button_recordstop.Text = "停止";
            this.button_recordstop.UseVisualStyleBackColor = true;
            this.button_recordstop.Click += new System.EventHandler(this.button_recordstop_Click);
            // 
            // button_recordstart
            // 
            this.button_recordstart.Location = new System.Drawing.Point(74, 9);
            this.button_recordstart.Name = "button_recordstart";
            this.button_recordstart.Size = new System.Drawing.Size(75, 23);
            this.button_recordstart.TabIndex = 24;
            this.button_recordstart.Text = "開始";
            this.button_recordstart.UseVisualStyleBackColor = true;
            this.button_recordstart.Click += new System.EventHandler(this.button_recordstart_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_recordstop);
            this.groupBox4.Controls.Add(this.button_recordstart);
            this.groupBox4.Location = new System.Drawing.Point(13, 78);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(321, 35);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "録音";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox_vib_apnea);
            this.groupBox5.Controls.Add(this.checkBox_vib_snore);
            this.groupBox5.Controls.Add(this.button_vibstart);
            this.groupBox5.Location = new System.Drawing.Point(563, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(130, 98);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "バイブレーション";
            // 
            // checkBox_vib_apnea
            // 
            this.checkBox_vib_apnea.AutoSize = true;
            this.checkBox_vib_apnea.Location = new System.Drawing.Point(38, 69);
            this.checkBox_vib_apnea.Name = "checkBox_vib_apnea";
            this.checkBox_vib_apnea.Size = new System.Drawing.Size(60, 16);
            this.checkBox_vib_apnea.TabIndex = 4;
            this.checkBox_vib_apnea.Text = "無呼吸";
            this.checkBox_vib_apnea.UseVisualStyleBackColor = true;
            // 
            // checkBox_vib_snore
            // 
            this.checkBox_vib_snore.AutoSize = true;
            this.checkBox_vib_snore.Location = new System.Drawing.Point(38, 47);
            this.checkBox_vib_snore.Name = "checkBox_vib_snore";
            this.checkBox_vib_snore.Size = new System.Drawing.Size(53, 16);
            this.checkBox_vib_snore.TabIndex = 3;
            this.checkBox_vib_snore.Text = "いびき";
            this.checkBox_vib_snore.UseVisualStyleBackColor = true;
            // 
            // button_vibstart
            // 
            this.button_vibstart.Location = new System.Drawing.Point(17, 18);
            this.button_vibstart.Name = "button_vibstart";
            this.button_vibstart.Size = new System.Drawing.Size(90, 23);
            this.button_vibstart.TabIndex = 0;
            this.button_vibstart.Text = "テストバイブ";
            this.button_vibstart.UseVisualStyleBackColor = true;
            this.button_vibstart.Click += new System.EventHandler(this.button_vibstart_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1346, 529);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chartAccelerometer);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chartRawData);
            this.Controls.Add(this.chartApnea);
            this.Controls.Add(this.groupBoxCom);
            this.Name = "FormMain";
            this.Text = "快眠チェッカー";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.chartApnea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAccelerometer)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxCom.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartApnea;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRawData;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAccelerometer;

        private System.Windows.Forms.ComboBox comboBoxComport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxCom;
        private System.Windows.Forms.CheckBox checkBox_apneapoint;
        private System.Windows.Forms.CheckBox checkBox_apnearms;
        private System.Windows.Forms.CheckBox checkBox_dcresp;
        private System.Windows.Forms.CheckBox checkBox_rawsnore;
        private System.Windows.Forms.CheckBox checkBox_rawresp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_recordstop;
        private System.Windows.Forms.Button button_recordstart;
        public System.Windows.Forms.CheckBox checkBox_alarm_apnea;
        public System.Windows.Forms.CheckBox checkBox_alarm_snore;
        private System.Windows.Forms.Button button_alarmplay;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_vibstart;
        public System.Windows.Forms.CheckBox checkBox_vib_apnea;
        public System.Windows.Forms.CheckBox checkBox_vib_snore;
        public System.Windows.Forms.ComboBox comboBoxAlarm;
    }
}

