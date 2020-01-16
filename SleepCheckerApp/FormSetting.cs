using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SleepCheckerApp
{
    public partial class FormSetting : Form
    {
        public FormMain form;

        private byte[] param = new byte[1];

        public FormSetting()
        {
            InitializeComponent();

            // 現在の設定にチェック
        }

        private void radioButtonMode_CheckedChanged(object sender, EventArgs e)
        {
            if( radioButtonMonitor.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_MONITORING;
            }
            else if( radioButtonSnore.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE;
            }
            else if( radioButtonApnea.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_APNEA;
            }
            else if( radioButtonSnoreApnea.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_MODE_SUPPRESS_SNORE_APNEA;
            }

            if(!form.writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("設定できませんでした。");
            }
        }

        private void radioButtonSnoreDetect_CheckedChanged(object sender, EventArgs e)
        {
            if( radioButtonSnoreDetectWeak.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_WEAK;
            }
            else if( radioButtonSnoreDetectMed.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_MED;
            }
            else if( radioButtonSnoreDetectStrong.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_SNORE_SENS_STRONG;
            }

            if (!form.writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("設定できませんでした。");
            }

        }

        private void radioButtonVib_CheckedChanged(object sender, EventArgs e)
        {
            if( radioButtonVibWeak.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_WEAK;
            }
            else if( radioButtonVibMed.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_MED;
            }
            else if( radioButtonVibStrong.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_STRONG;
            }
            else if( radioButtonVibGrad.Checked == true )
            {
                param[0] = (int)RCV_COMMAND.Rcv_command.RCV_COM_VIB_GRAD;
            }

            if (!form.writeData(param))
            {
                System.Windows.Forms.MessageBox.Show("設定できませんでした。");
            }
        }
    }
}
