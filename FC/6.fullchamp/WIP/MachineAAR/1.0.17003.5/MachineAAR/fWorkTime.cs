using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MachineAAR
{
    public partial class fWorkTime : Form
    {
        public DateTime? StartTime = null;

        public DateTime? EndTime = null;

        /// <summary>
        /// 預設的產出時間
        /// </summary>
        private readonly DateTime DefaultEndTime;

        public fWorkTime(DateTime defaultEndTime) : this(null, null, defaultEndTime) { }

        public fWorkTime(DateTime? startTime, DateTime? endTime, DateTime defaultEndTime)
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            DefaultEndTime = defaultEndTime;

            DtpStart.Value = startTime ?? DefaultEndTime;

            DtpEnd.Value = endTime ?? DefaultEndTime;

            CbStart.Checked = startTime != null;

            CbEnd.Checked = endTime != null;

            DtpStart.ValueChanged += DtpStart_ValueChanged;

            DtpEnd.ValueChanged += DtpEnd_ValueChanged;

            BtnOK.Click += BtnOK_Click;

            BtnCancel.Click += BtnCancel_Click;
        }

        private void DtpStart_ValueChanged(object sender, EventArgs e)
        {
            CbStart.Checked = true;
        }

        private void DtpEnd_ValueChanged(object sender, EventArgs e)
        {
            CbEnd.Checked = true;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!PreCheckWorkTime(out string message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            StartTime = CbStart.Checked ? DtpStart.Value : (DateTime?)null;

            EndTime = CbEnd.Checked ? DtpEnd.Value : (DateTime?)null;

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool PreCheckWorkTime(out string message)
        {
            message = string.Empty;

            if (CbStart.Checked)
            {
                if (CbEnd.Checked && DtpStart.Value > DtpEnd.Value)
                {
                    message = SajetCommon.SetLanguage("The start time is later than the end time");

                    return false;
                }
                else if (DtpStart.Value > DefaultEndTime)
                {
                    message = SajetCommon.SetLanguage("The start time is later than the runcard out-process time");

                    return false;
                }
            }
            
            if (CbEnd.Checked && DtpEnd.Value > DefaultEndTime)
            {
                message = SajetCommon.SetLanguage("The end time is later than the runcard out-process time");

                return false;
            }

            return true;
        }
    }
}
