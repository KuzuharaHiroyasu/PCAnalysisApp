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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
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
            this.comboBoxComport = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_send = new System.Windows.Forms.Button();
            this.chartHeartBeatRemov = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            this.buttonStart.Location = new System.Drawing.Point(36, 41);
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
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.Maximum = 10D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Interval = 1D;
            chartArea1.AxisY.Maximum = 3D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartAreaTime";
            this.chartApnea.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chartApnea.Legends.Add(legend1);
            this.chartApnea.Location = new System.Drawing.Point(0, 443);
            this.chartApnea.Name = "chartApnea";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartAreaTime";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "無呼吸";
            series2.ChartArea = "ChartAreaTime";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "いびき";
            this.chartApnea.Series.Add(series1);
            this.chartApnea.Series.Add(series2);
            this.chartApnea.Size = new System.Drawing.Size(1344, 216);
            this.chartApnea.TabIndex = 11;
            this.chartApnea.Text = "ステータス";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title1.Name = "Title";
            title1.Text = "いびき/無呼吸判定";
            this.chartApnea.Titles.Add(title1);
            // 
            // chartRawData
            // 
            this.chartRawData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.Interval = 200D;
            chartArea2.AxisX.Maximum = 2000D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisY.Maximum = 1024D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartAreaTime";
            this.chartRawData.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chartRawData.Legends.Add(legend2);
            this.chartRawData.Location = new System.Drawing.Point(0, 125);
            this.chartRawData.Name = "chartRawData";
            series3.ChartArea = "ChartAreaTime";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "呼吸音";
            series4.ChartArea = "ChartAreaTime";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "いびき音";
            this.chartRawData.Series.Add(series3);
            this.chartRawData.Series.Add(series4);
            this.chartRawData.Size = new System.Drawing.Size(1344, 301);
            this.chartRawData.TabIndex = 15;
            this.chartRawData.Text = "呼吸音";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title2.Name = "Title";
            title2.Text = "呼吸音";
            this.chartRawData.Titles.Add(title2);
            // 
            // chartCalculation
            // 
            chartArea3.AxisX.Interval = 10D;
            chartArea3.AxisX.Maximum = 100D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.AxisX2.Interval = 0.1D;
            chartArea3.AxisX2.Maximum = 0.5D;
            chartArea3.AxisX2.Minimum = 0D;
            chartArea3.Name = "ChartAreaTime";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 85F;
            chartArea3.Position.Width = 79F;
            chartArea3.Position.X = 0.7F;
            chartArea3.Position.Y = 13F;
            this.chartCalculation.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            legend3.Position.Auto = false;
            legend3.Position.Height = 19.79167F;
            legend3.Position.Width = 18.11263F;
            legend3.Position.X = 80F;
            legend3.Position.Y = 12.98242F;
            this.chartCalculation.Legends.Add(legend3);
            this.chartCalculation.Location = new System.Drawing.Point(1289, 59);
            this.chartCalculation.Name = "chartCalculation";
            series5.ChartArea = "ChartAreaTime";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "無呼吸(rms)";
            series6.ChartArea = "ChartAreaTime";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "無呼吸(point)";
            this.chartCalculation.Series.Add(series5);
            this.chartCalculation.Series.Add(series6);
            this.chartCalculation.Size = new System.Drawing.Size(45, 38);
            this.chartCalculation.TabIndex = 20;
            title3.Name = "Title";
            title3.Text = "演算途中データ";
            this.chartCalculation.Titles.Add(title3);
            this.chartCalculation.Visible = false;
            // 
            // chartAccelerometer
            // 
            chartArea4.AxisX.Interval = 20D;
            chartArea4.AxisX.Maximum = 200D;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisY.Maximum = 127D;
            chartArea4.AxisY.Minimum = -128D;
            chartArea4.Name = "ChartAreaTime";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 85F;
            chartArea4.Position.Width = 79F;
            chartArea4.Position.X = 1F;
            chartArea4.Position.Y = 13F;
            this.chartAccelerometer.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            legend4.Position.Auto = false;
            legend4.Position.Height = 27.08333F;
            legend4.Position.Width = 10.8067F;
            legend4.Position.X = 80F;
            legend4.Position.Y = 12.98242F;
            this.chartAccelerometer.Legends.Add(legend4);
            this.chartAccelerometer.Location = new System.Drawing.Point(1238, 12);
            this.chartAccelerometer.Name = "chartAccelerometer";
            series7.BorderWidth = 2;
            series7.ChartArea = "ChartAreaTime";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Legend = "Legend1";
            series7.MarkerBorderWidth = 5;
            series7.Name = "X軸";
            series8.BorderWidth = 2;
            series8.ChartArea = "ChartAreaTime";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.MarkerBorderWidth = 5;
            series8.Name = "Y軸";
            series9.BorderWidth = 2;
            series9.ChartArea = "ChartAreaTime";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Legend = "Legend1";
            series9.MarkerBorderWidth = 5;
            series9.Name = "Z軸";
            this.chartAccelerometer.Series.Add(series7);
            this.chartAccelerometer.Series.Add(series8);
            this.chartAccelerometer.Series.Add(series9);
            this.chartAccelerometer.Size = new System.Drawing.Size(45, 41);
            this.chartAccelerometer.TabIndex = 21;
            this.chartAccelerometer.Text = "加速度センサー";
            title4.Name = "Title";
            title4.Text = "加速度センサー";
            this.chartAccelerometer.Titles.Add(title4);
            this.chartAccelerometer.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_acl_z);
            this.groupBox2.Controls.Add(this.checkBox_acl_y);
            this.groupBox2.Controls.Add(this.checkBox_acl_x);
            this.groupBox2.Controls.Add(this.checkBox_apneapoint);
            this.groupBox2.Controls.Add(this.checkBox_apnearms);
            this.groupBox2.Location = new System.Drawing.Point(1029, 9);
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
            this.groupBoxCom.Controls.Add(this.comboBoxComport);
            this.groupBoxCom.Controls.Add(this.buttonStart);
            this.groupBoxCom.Location = new System.Drawing.Point(13, 6);
            this.groupBoxCom.Name = "groupBoxCom";
            this.groupBoxCom.Size = new System.Drawing.Size(181, 82);
            this.groupBoxCom.TabIndex = 8;
            this.groupBoxCom.TabStop = false;
            this.groupBoxCom.Text = "ポート設定";
            // 
            // comboBoxComport
            // 
            this.comboBoxComport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxComport.FormattingEnabled = true;
            this.comboBoxComport.Location = new System.Drawing.Point(36, 14);
            this.comboBoxComport.Name = "comboBoxComport";
            this.comboBoxComport.Size = new System.Drawing.Size(108, 20);
            this.comboBoxComport.TabIndex = 15;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_send);
            this.groupBox5.Location = new System.Drawing.Point(213, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(147, 52);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "バイブレーション";
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(6, 21);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(90, 20);
            this.button_send.TabIndex = 5;
            this.button_send.Text = "コマンド送信";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // chartHeartBeatRemov
            // 
            chartArea5.AxisX.Interval = 200D;
            chartArea5.AxisX.Maximum = 2000D;
            chartArea5.AxisX.Minimum = 0D;
            chartArea5.AxisX2.Interval = 0.1D;
            chartArea5.AxisX2.Maximum = 0.5D;
            chartArea5.AxisX2.Minimum = 0D;
            chartArea5.AxisY.Maximum = 1024D;
            chartArea5.AxisY.Minimum = 0D;
            chartArea5.Name = "ChartAreaTime";
            chartArea5.Position.Auto = false;
            chartArea5.Position.Height = 85F;
            chartArea5.Position.Width = 80F;
            chartArea5.Position.Y = 13F;
            this.chartHeartBeatRemov.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            legend5.Position.Auto = false;
            legend5.Position.Height = 19.79167F;
            legend5.Position.Width = 18.11263F;
            legend5.Position.X = 80F;
            legend5.Position.Y = 12.98242F;
            this.chartHeartBeatRemov.Legends.Add(legend5);
            this.chartHeartBeatRemov.Location = new System.Drawing.Point(1238, 59);
            this.chartHeartBeatRemov.Name = "chartHeartBeatRemov";
            series10.ChartArea = "ChartAreaTime";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Legend = "Legend1";
            series10.Name = "心拍除去後呼吸";
            this.chartHeartBeatRemov.Series.Add(series10);
            this.chartHeartBeatRemov.Size = new System.Drawing.Size(45, 38);
            this.chartHeartBeatRemov.TabIndex = 28;
            title5.Name = "Title";
            title5.Text = "心拍除去後の呼吸波形";
            this.chartHeartBeatRemov.Titles.Add(title5);
            this.chartHeartBeatRemov.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1345, 660);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chartApnea);
            this.Controls.Add(this.groupBoxCom);
            this.Controls.Add(this.chartAccelerometer);
            this.Controls.Add(this.chartCalculation);
            this.Controls.Add(this.chartRawData);
            this.Controls.Add(this.chartHeartBeatRemov);
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
        private System.Windows.Forms.Button button_send;
    }
}

