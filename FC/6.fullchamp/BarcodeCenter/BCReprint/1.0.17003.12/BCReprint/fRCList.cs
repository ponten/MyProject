using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using OtSrv = BCReprintDll.Services.OtherServices;
using RlSrv = Print_RCList.RCListService;

namespace BCReprintDll
{
    public partial class fRCList : Form
    {
        public fRCList()
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            BtnPrint.Click += BtnPrint_Click;

            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string work_order = TbWO.Text.Trim().ToUpper();

            if (OtSrv.CheckForWorkOrder(work_order, out string message))
            {
                bool print_success
                    = RlSrv.Print(
                    work_order: work_order,
                    direct_print: true,
                    out message);

                if (print_success)
                {
                    DialogResult = DialogResult.OK;

                    return;
                }
            }

            SajetCommon.Show_Message(SajetCommon.SetLanguage(message), 0);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

       
    }
}
