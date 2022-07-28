using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Text.RegularExpressions;//�ɤJ�R�W�Ŷ�(���h��F��)

namespace PartAQL
{
    public partial class fAddItem : Form
    {
        public string sItem, sInputType;
        public fAddItem()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            sItem = editItem.Text;

            if (sInputType == "1")
            {
                if (IsNumeric(sItem))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Item Value in Number"), 0);
                    return;
                }
            }
            DialogResult = DialogResult.Yes;
        }

        private void fAddItem_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        //�w�q�@�Ө��,�@��:�P�_strNumber�O�_���Ʀr,�O�Ʀr��^True,���O�Ʀr��^False
        public bool IsNumeric(string strNumber)
        {
            Regex NumberPattern = new Regex("[^0-9.-]");
            return NumberPattern.IsMatch(strNumber);
        }
    }
}