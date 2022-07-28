using RCProcessParam.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QoSrv = RCProcessParam.Services.QCOptionService;

namespace RCProcessParam
{
    public partial class fQCOption : Form
    {
        private ProcessViewModel process_info;

        public fQCOption(ProcessViewModel model)
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            LoadComboboxItems(ref CbFpi);
            LoadComboboxItems(ref CbLpi);

            process_info = new ProcessViewModel(model);

            LbPartNo.Text = process_info.PART_NO;

            LbProcess.Text = process_info.PROCESS_NAME;

            this.Load += FQCOption_Load;

            // 一個製程可以有多種設定
            /*
            CbFpi.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            CbLpi.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            //*/

            BtnOK.Click += BtnOK_Click;

            BtnCancel.Click += BtnCancel_Click;
        }

        private void FQCOption_Load(object sender, EventArgs e)
        {
            var d = QoSrv.GetData(process_info);

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                CbFpi.Text = d.Tables[0].Rows[0]["FPI"].ToString();

                CbLpi.Text = d.Tables[0].Rows[0]["LPI"].ToString();
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var c = sender as ComboBox;

            string s = c.Text.Trim().ToUpper();

            if (s == "Y")
            {
                if (c.Name == nameof(CbFpi))
                {
                    CbLpi.Text = "N";
                }
                else
                {
                    CbFpi.Text = "N";
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var qc_options = new QCOptionModel
            {
                OPTION1 = CbFpi.Text.Trim(),
                OPTION2 = CbLpi.Text.Trim(),
            };

            QoSrv.Save(process_info, qc_options);

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 載入下選單
        /// </summary>
        /// <param name="c"></param>
        private void LoadComboboxItems(ref ComboBox c)
        {
            c.Items.Add("Y");
            c.Items.Add("N");

            c.SelectedIndex = 1;
        }
    }
}
