using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Text.RegularExpressions;//旧㏑丁(タ玥笷Α)

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

        //﹚竡ㄧ计,ノ:耞strNumber琌计,琌计True,ぃ琌计False
        public bool IsNumeric(string strNumber)
        {
            Regex NumberPattern = new Regex("[^0-9.-]");
            return NumberPattern.IsMatch(strNumber);
        }
    }
}