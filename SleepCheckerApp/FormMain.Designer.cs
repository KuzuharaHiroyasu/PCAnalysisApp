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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend11 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series22 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title11 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend12 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series23 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series24 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title12 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea13 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend13 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series25 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series26 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title13 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea14 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend14 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series27 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series28 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series29 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title14 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea15 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend15 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series30 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title15 = new System.Windows.Forms.DataVisualization.Charting.Title();
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
            this.chartHeartBeatRemov = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBoxCom = new System.Windows.Forms.GroupBox();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.comboBoxComport = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButtonVibConfGrad = new System.Windows.Forms.RadioButton();
            this.radioButtonVibConfStrong = new System.Windows.Forms.RadioButton();
            this.radioButtonVibConfMed = new System.Windows.Forms.RadioButton();
            this.radioButtonVibConfWeak = new System.Windows.Forms.RadioButton();
            this.buttonVibConf = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartApnea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCalculation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAccelerometer)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHeartBeatRemov)).BeginInit();
            this.groupBoxCom.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 15);
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
            chartArea11.AxisX.Interval = 1D;
            chartArea11.AxisX.Maximum = 10D;
            chartArea11.AxisX.Minimum = 0D;
            chartArea11.AxisY.Interval = 1D;
            chartArea11.AxisY.Maximum = 3D;
            chartArea11.AxisY.Minimum = 0D;
            chartArea11.Name = "ChartAreaTime";
            this.chartApnea.ChartAreas.Add(chartArea11);
            legend11.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend11.Name = "Legend1";
            this.chartApnea.Legends.Add(legend11);
            this.chartApnea.Location = new System.Drawing.Point(6, 366);
            this.chartApnea.Name = "chartApnea";
            series21.BorderWidth = 3;
            series21.ChartArea = "ChartAreaTime";
            series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series21.Legend = "Legend1";
            series21.Name = "無呼吸";
            series22.ChartArea = "ChartAreaTime";
            series22.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series22.Legend = "Legend1";
            series22.Name = "いびき";
            this.chartApnea.Series.Add(series21);
            this.chartApnea.Series.Add(series22);
            this.chartApnea.Size = new System.Drawing.Size(1334, 290);
            this.chartApnea.TabIndex = 11;
            this.chartApnea.Text = "ステータス";
            title11.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title11.Name = "Title";
            title11.Text = "いびき/無呼吸判定";
            this.chartApnea.Titles.Add(title11);
            // 
            // chartRawData
            // 
            this.chartRawData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea12.AxisX.Interval = 200D;
            chartArea12.AxisX.Maximum = 2000D;
            chartArea12.AxisX.Minimum = 0D;
            chartArea12.AxisY.Maximum = 1024D;
            chartArea12.AxisY.Minimum = 0D;
            chartArea12.Name = "ChartAreaTime";
            this.chartRawData.ChartAreas.Add(chartArea12);
            legend12.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend12.Name = "Legend1";
            this.chartRawData.Legends.Add(legend12);
            this.chartRawData.Location = new System.Drawing.Point(6, 59);
            this.chartRawData.Name = "chartRawData";
            series23.ChartArea = "ChartAreaTime";
            series23.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series23.Legend = "Legend1";
            series23.Name = "呼吸音";
            series24.ChartArea = "ChartAreaTime";
            series24.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series24.Legend = "Legend1";
            series24.Name = "いびき音";
            this.chartRawData.Series.Add(series23);
            this.chartRawData.Series.Add(series24);
            this.chartRawData.Size = new System.Drawing.Size(1334, 301);
            this.chartRawData.TabIndex = 15;
            this.chartRawData.Text = "呼吸音";
            title12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title12.Name = "Title";
            title12.Text = "呼吸音";
            this.chartRawData.Titles.Add(title12);
            // 
            // chartCalculation
            // 
            chartArea13.AxisX.Interval = 10D;
            chartArea13.AxisX.Maximum = 100D;
            chartArea13.AxisX.Minimum = 0D;
            chartArea13.AxisX2.Interval = 0.1D;
            chartArea13.AxisX2.Maximum = 0.5D;
            chartArea13.AxisX2.Minimum = 0D;
            chartArea13.Name = "ChartAreaTime";
            chartArea13.Position.Auto = false;
            chartArea13.Position.Height = 85F;
            chartArea13.Position.Width = 79F;
            chartArea13.Position.X = 0.7F;
            chartArea13.Position.Y = 13F;
            this.chartCalculation.ChartAreas.Add(chartArea13);
            legend13.Name = "Legend1";
            legend13.Position.Auto = false;
            legend13.Position.Height = 19.79167F;
            legend13.Position.Width = 18.11263F;
            legend13.Position.X = 80F;
            legend13.Position.Y = 12.98242F;
            this.chartCalculation.Legends.Add(legend13);
            this.chartCalculation.Location = new System.Drawing.Point(57, 1);
            this.chartCalculation.Name = "chartCalculation";
            series25.ChartArea = "ChartAreaTime";
            series25.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series25.Legend = "Legend1";
            series25.Name = "無呼吸(rms)";
            series26.ChartArea = "ChartAreaTime";
            series26.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series26.Legend = "Legend1";
            series26.Name = "無呼吸(point)";
            this.chartCalculation.Series.Add(series25);
            this.chartCalculation.Series.Add(series26);
            this.chartCalculation.Size = new System.Drawing.Size(45, 38);
            this.chartCalculation.TabIndex = 20;
            title13.Name = "Title";
            title13.Text = "演算途中データ";
            this.chartCalculation.Titles.Add(title13);
            this.chartCalculation.Visible = false;
            // 
            // chartAccelerometer
            // 
            chartArea14.AxisX.Interval = 20D;
            chartArea14.AxisX.Maximum = 200D;
            chartArea14.AxisX.Minimum = 0D;
            chartArea14.AxisY.Maximum = 127D;
            chartArea14.AxisY.Minimum = -128D;
            chartArea14.Name = "ChartAreaTime";
            chartArea14.Position.Auto = false;
            chartArea14.Position.Height = 85F;
            chartArea14.Position.Width = 79F;
            chartArea14.Position.X = 1F;
            chartArea14.Position.Y = 13F;
            this.chartAccelerometer.ChartAreas.Add(chartArea14);
            legend14.Name = "Legend1";
            legend14.Position.Auto = false;
            legend14.Position.Height = 27.08333F;
            legend14.Position.Width = 10.8067F;
            legend14.Position.X = 80F;
            legend14.Position.Y = 12.98242F;
            this.chartAccelerometer.Legends.Add(legend14);
            this.chartAccelerometer.Location = new System.Drawing.Point(984, 7);
            this.chartAccelerometer.Name = "chartAccelerometer";
            series27.BorderWidth = 2;
            series27.ChartArea = "ChartAreaTime";
            series27.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series27.Legend = "Legend1";
            series27.MarkerBorderWidth = 5;
            series27.Name = "X軸";
            series28.BorderWidth = 2;
            series28.ChartArea = "ChartAreaTime";
            series28.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series28.Legend = "Legend1";
            series28.MarkerBorderWidth = 5;
            series28.Name = "Y軸";
            series29.BorderWidth = 2;
            series29.ChartArea = "ChartAreaTime";
            series29.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series29.Legend = "Legend1";
            series29.MarkerBorderWidth = 5;
            series29.Name = "Z軸";
            this.chartAccelerometer.Series.Add(series27);
            this.chartAccelerometer.Series.Add(series28);
            this.chartAccelerometer.Series.Add(series29);
            this.chartAccelerometer.Size = new System.Drawing.Size(45, 41);
            this.chartAccelerometer.TabIndex = 21;
            this.chartAccelerometer.Text = "加速度センサー";
            title14.Name = "Title";
            title14.Text = "加速度センサー";
            this.chartAccelerometer.Titles.Add(title14);
            this.chartAccelerometer.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_acl_z);
            this.groupBox2.Controls.Add(this.checkBox_acl_y);
            this.groupBox2.Controls.Add(this.checkBox_acl_x);
            this.groupBox2.Controls.Add(this.checkBox_apneapoint);
            this.groupBox2.Controls.Add(this.checkBox_apnearms);
            this.groupBox2.Controls.Add(this.groupBoxCom);
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
            // chartHeartBeatRemov
            // 
            chartArea15.AxisX.Interval = 200D;
            chartArea15.AxisX.Maximum = 2000D;
            chartArea15.AxisX.Minimum = 0D;
            chartArea15.AxisX2.Interval = 0.1D;
            chartArea15.AxisX2.Maximum = 0.5D;
            chartArea15.AxisX2.Minimum = 0D;
            chartArea15.AxisY.Maximum = 1024D;
            chartArea15.AxisY.Minimum = 0D;
            chartArea15.Name = "ChartAreaTime";
            chartArea15.Position.Auto = false;
            chartArea15.Position.Height = 85F;
            chartArea15.Position.Width = 80F;
            chartArea15.Position.Y = 13F;
            this.chartHeartBeatRemov.ChartAreas.Add(chartArea15);
            legend15.Name = "Legend1";
            legend15.Position.Auto = false;
            legend15.Position.Height = 19.79167F;
            legend15.Position.Width = 18.11263F;
            legend15.Position.X = 80F;
            legend15.Position.Y = 12.98242F;
            this.chartHeartBeatRemov.Legends.Add(legend15);
            this.chartHeartBeatRemov.Location = new System.Drawing.Point(6, 1);
            this.chartHeartBeatRemov.Name = "chartHeartBeatRemov";
            series30.ChartArea = "ChartAreaTime";
            series30.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series30.Legend = "Legend1";
            series30.Name = "心拍除去後呼吸";
            this.chartHeartBeatRemov.Series.Add(series30);
            this.chartHeartBeatRemov.Size = new System.Drawing.Size(45, 38);
            this.chartHeartBeatRemov.TabIndex = 28;
            title15.Name = "Title";
            title15.Text = "心拍除去後の呼吸波形";
            this.chartHeartBeatRemov.Titles.Add(title15);
            this.chartHeartBeatRemov.Visible = false;
            // 
            // groupBoxCom
            // 
            this.groupBoxCom.Controls.Add(this.comboBoxComport);
            this.groupBoxCom.Location = new System.Drawing.Point(189, 0);
            this.groupBoxCom.Name = "groupBoxCom";
            this.groupBoxCom.Size = new System.Drawing.Size(122, 47);
            this.groupBoxCom.TabIndex = 8;
            this.groupBoxCom.TabStop = false;
            this.groupBoxCom.Text = "ポート設定";
            this.groupBoxCom.Visible = false;
            // 
            // buttonSetting
            // 
            this.buttonSetting.Location = new System.Drawing.Point(455, 15);
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
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Font = new System.Drawing.Font("MS UI Gothic", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelState.ForeColor = System.Drawing.Color.Red;
            this.labelState.Location = new System.Drawing.Point(269, 15);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(93, 27);
            this.labelState.TabIndex = 27;
            this.labelState.Text = "待機中";
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(137, 15);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(108, 30);
            this.buttonStop.TabIndex = 28;
            this.buttonStop.Text = "停止";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1347, 660);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.chartRawData);
            this.Controls.Add(this.buttonSetting);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chartApnea);
            this.Controls.Add(this.chartAccelerometer);
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
            ((System.ComponentModel.ISupportInitialize)(this.chartHeartBeatRemov)).EndInit();
            this.groupBoxCom.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Button buttonStop;
    }
}

