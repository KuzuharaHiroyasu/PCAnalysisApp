﻿namespace SleepCheckerApp
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
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
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.Maximum = 10D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Interval = 1D;
            chartArea1.AxisY.Maximum = 4D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartAreaTime";
            this.chartApnea.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartApnea.Legends.Add(legend1);
            this.chartApnea.Location = new System.Drawing.Point(12, 324);
            this.chartApnea.Name = "chartApnea";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartAreaTime";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "呼吸状態";
            series2.ChartArea = "ChartAreaTime";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "いびき";
            this.chartApnea.Series.Add(series1);
            this.chartApnea.Series.Add(series2);
            this.chartApnea.Size = new System.Drawing.Size(658, 193);
            this.chartApnea.TabIndex = 11;
            this.chartApnea.Text = "ステータス";
            title1.Name = "Title";
            title1.Text = "無呼吸・低呼吸";
            this.chartApnea.Titles.Add(title1);
            // 
            // chartRawData
            // 
            chartArea2.AxisX.Interval = 200D;
            chartArea2.AxisX.Maximum = 2000D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.Title = "個数";
            chartArea2.AxisY.Maximum = 1024D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartAreaTime";
            this.chartRawData.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartRawData.Legends.Add(legend2);
            this.chartRawData.Location = new System.Drawing.Point(12, 125);
            this.chartRawData.Name = "chartRawData";
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartAreaTime";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.MarkerBorderWidth = 5;
            series3.Name = "呼吸(生データ)";
            series4.ChartArea = "ChartAreaTime";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "いびき(生データ)";
            series5.ChartArea = "ChartAreaTime";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "呼吸(移動平均)";
            this.chartRawData.Series.Add(series3);
            this.chartRawData.Series.Add(series4);
            this.chartRawData.Series.Add(series5);
            this.chartRawData.Size = new System.Drawing.Size(658, 193);
            this.chartRawData.TabIndex = 15;
            this.chartRawData.Text = "生データ(呼吸)";
            title2.Name = "Title";
            title2.Text = "生データ(呼吸)";
            this.chartRawData.Titles.Add(title2);
            // 
            // chart1
            // 
            chartArea3.AxisX.Interval = 10D;
            chartArea3.AxisX.Maximum = 100D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.AxisX.Title = "個数";
            chartArea3.AxisX2.Interval = 0.1D;
            chartArea3.AxisX2.Maximum = 0.5D;
            chartArea3.AxisX2.Minimum = 0D;
            chartArea3.Name = "ChartAreaTime";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(676, 324);
            this.chart1.Name = "chart1";
            series6.ChartArea = "ChartAreaTime";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "無呼吸(rms)";
            series7.ChartArea = "ChartAreaTime";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Legend = "Legend1";
            series7.Name = "無呼吸(point)";
            this.chart1.Series.Add(series6);
            this.chart1.Series.Add(series7);
            this.chart1.Size = new System.Drawing.Size(658, 193);
            this.chart1.TabIndex = 20;
            title3.Name = "Title";
            title3.Text = "演算途中データ";
            this.chart1.Titles.Add(title3);
            // 
            // chartAccelerometer
            // 
            chartArea4.AxisX.Interval = 10D;
            chartArea4.AxisX.Maximum = 120D;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisX.Title = "個数";
            chartArea4.AxisY.Maximum = 127D;
            chartArea4.AxisY.Minimum = -128D;
            chartArea4.AxisY.Title = "値";
            chartArea4.Name = "ChartAreaTime";
            this.chartAccelerometer.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartAccelerometer.Legends.Add(legend4);
            this.chartAccelerometer.Location = new System.Drawing.Point(676, 125);
            this.chartAccelerometer.Name = "chartAccelerometer";
            series8.BorderWidth = 2;
            series8.ChartArea = "ChartAreaTime";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.MarkerBorderWidth = 5;
            series8.Name = "X軸";
            series9.BorderWidth = 2;
            series9.ChartArea = "ChartAreaTime";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Legend = "Legend1";
            series9.MarkerBorderWidth = 5;
            series9.Name = "Y軸";
            series10.BorderWidth = 2;
            series10.ChartArea = "ChartAreaTime";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Legend = "Legend1";
            series10.MarkerBorderWidth = 5;
            series10.Name = "Z軸";
            this.chartAccelerometer.Series.Add(series8);
            this.chartAccelerometer.Series.Add(series9);
            this.chartAccelerometer.Series.Add(series10);
            this.chartAccelerometer.Size = new System.Drawing.Size(658, 193);
            this.chartAccelerometer.TabIndex = 21;
            this.chartAccelerometer.Text = "加速度センサー";
            title4.Name = "Title";
            title4.Text = "加速度センサー";
            this.chartAccelerometer.Titles.Add(title4);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_apneapoint);
            this.groupBox2.Controls.Add(this.checkBox_apnearms);
            this.groupBox2.Controls.Add(this.checkBox_dcresp);
            this.groupBox2.Controls.Add(this.checkBox_rawsnore);
            this.groupBox2.Controls.Add(this.checkBox_rawresp);
            this.groupBox2.Location = new System.Drawing.Point(612, 15);
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
            this.comboBoxComport.FormattingEnabled = true;
            this.comboBoxComport.Location = new System.Drawing.Point(26, 28);
            this.comboBoxComport.Name = "comboBoxComport";
            this.comboBoxComport.Size = new System.Drawing.Size(124, 20);
            this.comboBoxComport.TabIndex = 15;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_alarmplay);
            this.groupBox3.Controls.Add(this.checkBox_alarm_apnea);
            this.groupBox3.Controls.Add(this.checkBox_alarm_snore);
            this.groupBox3.Location = new System.Drawing.Point(340, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(130, 100);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "アラーム";
            // 
            // button_alarmplay
            // 
            this.button_alarmplay.Location = new System.Drawing.Point(17, 18);
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
            this.checkBox_alarm_apnea.Location = new System.Drawing.Point(37, 71);
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
            this.checkBox_alarm_snore.Checked = true;
            this.checkBox_alarm_snore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_alarm_snore.Location = new System.Drawing.Point(37, 49);
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
            this.groupBox5.Location = new System.Drawing.Point(476, 15);
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
            this.checkBox_vib_snore.Checked = true;
            this.checkBox_vib_snore.CheckState = System.Windows.Forms.CheckState.Checked;
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
    }
}

