namespace SleepCheckerApp
{
    partial class FormSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox_mode = new System.Windows.Forms.GroupBox();
            this.radioButtonSnoreApnea = new System.Windows.Forms.RadioButton();
            this.radioButtonApnea = new System.Windows.Forms.RadioButton();
            this.radioButtonSnore = new System.Windows.Forms.RadioButton();
            this.radioButtonMonitor = new System.Windows.Forms.RadioButton();
            this.groupBox_snore_sens = new System.Windows.Forms.GroupBox();
            this.radioButtonSnoreDetectStrong = new System.Windows.Forms.RadioButton();
            this.radioButtonSnoreDetectMed = new System.Windows.Forms.RadioButton();
            this.radioButtonSnoreDetectWeak = new System.Windows.Forms.RadioButton();
            this.groupBox_vib = new System.Windows.Forms.GroupBox();
            this.radioButtonVibGrad = new System.Windows.Forms.RadioButton();
            this.radioButtonVibStrong = new System.Windows.Forms.RadioButton();
            this.radioButtonVibMed = new System.Windows.Forms.RadioButton();
            this.radioButtonVibWeak = new System.Windows.Forms.RadioButton();
            this.groupBox_mode.SuspendLayout();
            this.groupBox_snore_sens.SuspendLayout();
            this.groupBox_vib.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_mode
            // 
            this.groupBox_mode.Controls.Add(this.radioButtonSnoreApnea);
            this.groupBox_mode.Controls.Add(this.radioButtonApnea);
            this.groupBox_mode.Controls.Add(this.radioButtonSnore);
            this.groupBox_mode.Controls.Add(this.radioButtonMonitor);
            this.groupBox_mode.Location = new System.Drawing.Point(12, 12);
            this.groupBox_mode.Name = "groupBox_mode";
            this.groupBox_mode.Size = new System.Drawing.Size(119, 130);
            this.groupBox_mode.TabIndex = 0;
            this.groupBox_mode.TabStop = false;
            this.groupBox_mode.Text = "モード";
            // 
            // radioButtonSnoreApnea
            // 
            this.radioButtonSnoreApnea.AutoSize = true;
            this.radioButtonSnoreApnea.Location = new System.Drawing.Point(15, 85);
            this.radioButtonSnoreApnea.Name = "radioButtonSnoreApnea";
            this.radioButtonSnoreApnea.Size = new System.Drawing.Size(94, 16);
            this.radioButtonSnoreApnea.TabIndex = 3;
            this.radioButtonSnoreApnea.Text = "いびき/無呼吸";
            this.radioButtonSnoreApnea.UseVisualStyleBackColor = true;
            this.radioButtonSnoreApnea.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // radioButtonApnea
            // 
            this.radioButtonApnea.AutoSize = true;
            this.radioButtonApnea.Location = new System.Drawing.Point(15, 63);
            this.radioButtonApnea.Name = "radioButtonApnea";
            this.radioButtonApnea.Size = new System.Drawing.Size(59, 16);
            this.radioButtonApnea.TabIndex = 2;
            this.radioButtonApnea.Text = "無呼吸";
            this.radioButtonApnea.UseVisualStyleBackColor = true;
            this.radioButtonApnea.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // radioButtonSnore
            // 
            this.radioButtonSnore.AutoSize = true;
            this.radioButtonSnore.Location = new System.Drawing.Point(15, 41);
            this.radioButtonSnore.Name = "radioButtonSnore";
            this.radioButtonSnore.Size = new System.Drawing.Size(52, 16);
            this.radioButtonSnore.TabIndex = 1;
            this.radioButtonSnore.Text = "いびき";
            this.radioButtonSnore.UseVisualStyleBackColor = true;
            this.radioButtonSnore.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // radioButtonMonitor
            // 
            this.radioButtonMonitor.AutoSize = true;
            this.radioButtonMonitor.Checked = true;
            this.radioButtonMonitor.Location = new System.Drawing.Point(15, 18);
            this.radioButtonMonitor.Name = "radioButtonMonitor";
            this.radioButtonMonitor.Size = new System.Drawing.Size(74, 16);
            this.radioButtonMonitor.TabIndex = 0;
            this.radioButtonMonitor.TabStop = true;
            this.radioButtonMonitor.Text = "モニタリング";
            this.radioButtonMonitor.UseVisualStyleBackColor = true;
            this.radioButtonMonitor.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // groupBox_snore_sens
            // 
            this.groupBox_snore_sens.Controls.Add(this.radioButtonSnoreDetectStrong);
            this.groupBox_snore_sens.Controls.Add(this.radioButtonSnoreDetectMed);
            this.groupBox_snore_sens.Controls.Add(this.radioButtonSnoreDetectWeak);
            this.groupBox_snore_sens.Location = new System.Drawing.Point(137, 12);
            this.groupBox_snore_sens.Name = "groupBox_snore_sens";
            this.groupBox_snore_sens.Size = new System.Drawing.Size(96, 130);
            this.groupBox_snore_sens.TabIndex = 1;
            this.groupBox_snore_sens.TabStop = false;
            this.groupBox_snore_sens.Text = "いびき検出感度";
            // 
            // radioButtonSnoreDetectStrong
            // 
            this.radioButtonSnoreDetectStrong.AutoSize = true;
            this.radioButtonSnoreDetectStrong.Location = new System.Drawing.Point(18, 63);
            this.radioButtonSnoreDetectStrong.Name = "radioButtonSnoreDetectStrong";
            this.radioButtonSnoreDetectStrong.Size = new System.Drawing.Size(35, 16);
            this.radioButtonSnoreDetectStrong.TabIndex = 2;
            this.radioButtonSnoreDetectStrong.Text = "強";
            this.radioButtonSnoreDetectStrong.UseVisualStyleBackColor = true;
            this.radioButtonSnoreDetectStrong.CheckedChanged += new System.EventHandler(this.radioButtonSnoreDetect_CheckedChanged);
            // 
            // radioButtonSnoreDetectMed
            // 
            this.radioButtonSnoreDetectMed.AutoSize = true;
            this.radioButtonSnoreDetectMed.Checked = true;
            this.radioButtonSnoreDetectMed.Location = new System.Drawing.Point(18, 41);
            this.radioButtonSnoreDetectMed.Name = "radioButtonSnoreDetectMed";
            this.radioButtonSnoreDetectMed.Size = new System.Drawing.Size(35, 16);
            this.radioButtonSnoreDetectMed.TabIndex = 1;
            this.radioButtonSnoreDetectMed.TabStop = true;
            this.radioButtonSnoreDetectMed.Text = "中";
            this.radioButtonSnoreDetectMed.UseVisualStyleBackColor = true;
            this.radioButtonSnoreDetectMed.CheckedChanged += new System.EventHandler(this.radioButtonSnoreDetect_CheckedChanged);
            // 
            // radioButtonSnoreDetectWeak
            // 
            this.radioButtonSnoreDetectWeak.AutoSize = true;
            this.radioButtonSnoreDetectWeak.Location = new System.Drawing.Point(18, 19);
            this.radioButtonSnoreDetectWeak.Name = "radioButtonSnoreDetectWeak";
            this.radioButtonSnoreDetectWeak.Size = new System.Drawing.Size(35, 16);
            this.radioButtonSnoreDetectWeak.TabIndex = 0;
            this.radioButtonSnoreDetectWeak.Text = "弱";
            this.radioButtonSnoreDetectWeak.UseVisualStyleBackColor = true;
            this.radioButtonSnoreDetectWeak.CheckedChanged += new System.EventHandler(this.radioButtonSnoreDetect_CheckedChanged);
            // 
            // groupBox_vib
            // 
            this.groupBox_vib.Controls.Add(this.radioButtonVibGrad);
            this.groupBox_vib.Controls.Add(this.radioButtonVibStrong);
            this.groupBox_vib.Controls.Add(this.radioButtonVibMed);
            this.groupBox_vib.Controls.Add(this.radioButtonVibWeak);
            this.groupBox_vib.Location = new System.Drawing.Point(239, 12);
            this.groupBox_vib.Name = "groupBox_vib";
            this.groupBox_vib.Size = new System.Drawing.Size(114, 130);
            this.groupBox_vib.TabIndex = 2;
            this.groupBox_vib.TabStop = false;
            this.groupBox_vib.Text = "バイブレーション";
            // 
            // radioButtonVibGrad
            // 
            this.radioButtonVibGrad.AutoSize = true;
            this.radioButtonVibGrad.Location = new System.Drawing.Point(22, 85);
            this.radioButtonVibGrad.Name = "radioButtonVibGrad";
            this.radioButtonVibGrad.Size = new System.Drawing.Size(74, 16);
            this.radioButtonVibGrad.TabIndex = 3;
            this.radioButtonVibGrad.Text = "徐々に強く";
            this.radioButtonVibGrad.UseVisualStyleBackColor = true;
            this.radioButtonVibGrad.CheckedChanged += new System.EventHandler(this.radioButtonVib_CheckedChanged);
            // 
            // radioButtonVibStrong
            // 
            this.radioButtonVibStrong.AutoSize = true;
            this.radioButtonVibStrong.Location = new System.Drawing.Point(22, 63);
            this.radioButtonVibStrong.Name = "radioButtonVibStrong";
            this.radioButtonVibStrong.Size = new System.Drawing.Size(35, 16);
            this.radioButtonVibStrong.TabIndex = 2;
            this.radioButtonVibStrong.Text = "強";
            this.radioButtonVibStrong.UseVisualStyleBackColor = true;
            this.radioButtonVibStrong.CheckedChanged += new System.EventHandler(this.radioButtonVib_CheckedChanged);
            // 
            // radioButtonVibMed
            // 
            this.radioButtonVibMed.AutoSize = true;
            this.radioButtonVibMed.Checked = true;
            this.radioButtonVibMed.Location = new System.Drawing.Point(22, 41);
            this.radioButtonVibMed.Name = "radioButtonVibMed";
            this.radioButtonVibMed.Size = new System.Drawing.Size(35, 16);
            this.radioButtonVibMed.TabIndex = 1;
            this.radioButtonVibMed.TabStop = true;
            this.radioButtonVibMed.Text = "中";
            this.radioButtonVibMed.UseVisualStyleBackColor = true;
            this.radioButtonVibMed.CheckedChanged += new System.EventHandler(this.radioButtonVib_CheckedChanged);
            // 
            // radioButtonVibWeak
            // 
            this.radioButtonVibWeak.AutoSize = true;
            this.radioButtonVibWeak.Location = new System.Drawing.Point(22, 19);
            this.radioButtonVibWeak.Name = "radioButtonVibWeak";
            this.radioButtonVibWeak.Size = new System.Drawing.Size(35, 16);
            this.radioButtonVibWeak.TabIndex = 0;
            this.radioButtonVibWeak.Text = "弱";
            this.radioButtonVibWeak.UseVisualStyleBackColor = true;
            this.radioButtonVibWeak.CheckedChanged += new System.EventHandler(this.radioButtonVib_CheckedChanged);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 157);
            this.Controls.Add(this.groupBox_vib);
            this.Controls.Add(this.groupBox_snore_sens);
            this.Controls.Add(this.groupBox_mode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSetting";
            this.Text = "設定";
            this.groupBox_mode.ResumeLayout(false);
            this.groupBox_mode.PerformLayout();
            this.groupBox_snore_sens.ResumeLayout(false);
            this.groupBox_snore_sens.PerformLayout();
            this.groupBox_vib.ResumeLayout(false);
            this.groupBox_vib.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_mode;
        private System.Windows.Forms.RadioButton radioButtonSnoreApnea;
        private System.Windows.Forms.RadioButton radioButtonApnea;
        private System.Windows.Forms.RadioButton radioButtonSnore;
        private System.Windows.Forms.RadioButton radioButtonMonitor;
        private System.Windows.Forms.GroupBox groupBox_snore_sens;
        private System.Windows.Forms.GroupBox groupBox_vib;
        private System.Windows.Forms.RadioButton radioButtonSnoreDetectStrong;
        private System.Windows.Forms.RadioButton radioButtonSnoreDetectMed;
        private System.Windows.Forms.RadioButton radioButtonSnoreDetectWeak;
        private System.Windows.Forms.RadioButton radioButtonVibGrad;
        private System.Windows.Forms.RadioButton radioButtonVibStrong;
        private System.Windows.Forms.RadioButton radioButtonVibMed;
        private System.Windows.Forms.RadioButton radioButtonVibWeak;
    }
}