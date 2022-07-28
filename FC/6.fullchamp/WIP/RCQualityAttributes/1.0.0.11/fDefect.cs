using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections;

namespace RCQualityAttributes
{
    public partial class fDefect : Form
    {

        public string s_SN;
        public string s_ProcessID;
        public string s_DefectCode;
        public string s_DefectID;
        public string s_DefectDESC;
        public string s_DefectRange;
        public string s_Count;
        //public string s_DefectType;
        //public string s_Analysis;
        //public string s_Measure;
        public bool b_HaveSN;
        public ArrayList alDefectCode;

        public fDefect()
        {
            InitializeComponent();
        }

        private void fDefect_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            if (b_HaveSN)
            {
                panelSN.Visible = true;
                txtSN.Text = s_SN;
            }
            else
                panelSN.Visible = false;
        }

        private void btnDEFECTCODE_Click(object sender, EventArgs e)
        {
            fFilter ff = new fFilter();
            ff.s_ProcessID = s_ProcessID;

            if (ff.ShowDialog() == DialogResult.OK)
            {
                txtDEFECTCODE.Text = ff.s_DefectCode;
                txtDEFECTRANGE.Text = ff.s_DefectRange;
                txtDESC.Text = ff.s_DefectDesc;
                s_DefectID = ff.s_DefectID;
                txtCOUNT.Focus();
            }
        }

        //确定按钮
        private void btnOK_Click(object sender, EventArgs e)
        {
            s_DefectCode = txtDEFECTCODE.Text;
            s_DefectRange = txtDEFECTRANGE.Text;
            s_DefectDESC = txtDESC.Text;
            s_Count = txtCOUNT.Text;
            
            if(!CheckDefectCode())
                return;

            if(!CheckDefectDuplicate())
                return;

            if (!CheckDefectQty())
                return;

            int iCnt = 0;
            if (!int.TryParse(txtCOUNT.Text, out iCnt))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Data invalid"), 0);
                txtCOUNT.SelectAll();
                txtCOUNT.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            
        }

        /// <summary>
        /// 检查Defect是否重复（区分有无SN的情况）
        /// </summary>
        /// <returns></returns>
        private bool CheckDefectDuplicate()
        {
            foreach (string sSNDefect in alDefectCode)
            {           
                //不同的测试大项，不允许相同的SN有相同的DEFECT
                if (b_HaveSN)
                {
                    string sSN = sSNDefect.Split(',')[0];
                    string sDefectCode = sSNDefect.Split(',')[1];
                    if (sSN == txtSN.Text && sDefectCode == txtDEFECTCODE.Text)
                    {
                        string sMsg = SajetCommon.SetLanguage("Defect code duplicate");
                        SajetCommon.Show_Message(sMsg, 1);
                        return false;
                    }
                }
                //不同的测试大项，不允许有相同的DEFECT
                else
                {
                    if (sSNDefect == txtDEFECTCODE.Text)
                    {
                        string sMsg = SajetCommon.SetLanguage("Defect code duplicate");
                        SajetCommon.Show_Message(sMsg, 1);
                        return false;
                    }
                }
            }
            return true;
        }

        //判断是否输入不良代码
        public bool CheckDefectCode()
        {
            bool DefectCode = true;
            if (s_DefectCode == "" || s_DefectCode == null)
            {
                string sMsg = SajetCommon.SetLanguage("please choose defectcode");
                SajetCommon.Show_Message(sMsg,1);
                DefectCode = false;
            }
            return DefectCode;
        }

        //判断是否输入不良数量
        public bool CheckDefectQty()
        {
            bool DefectQty = true;
            if (s_Count == "" || s_Count == null)
            {
                string sMsg = SajetCommon.SetLanguage("please choose defectqty");
                SajetCommon.Show_Message(sMsg, 1);
                DefectQty = false;
            }
            return DefectQty;
        }

        //取消按钮
        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
