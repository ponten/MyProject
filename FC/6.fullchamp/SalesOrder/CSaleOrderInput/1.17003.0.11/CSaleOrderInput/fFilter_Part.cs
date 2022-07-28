using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSaleOrderInput
{
    public partial class fFilter_Part : Form
    {
        public string firstFilter;
        public string sPART_NO;
        public string sUnitWeight;

        private List<string> fieldList = new List<string>();

        public fFilter_Part()
        {
            InitializeComponent();
        }

        private void fFilter_Part_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in gvData.Columns)
            {
                combField.Items.Add(column.Name);
                fieldList.Add(column.Name);
            }

            if (combField.Items.Count > 0)
            {
                combField.SelectedIndex = 0;
            }

            showData();
            SajetCommon.SetLanguageControl(this);
            gvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }
            buttonSearch.PerformClick();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            showData();
            gvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void gvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            sPART_NO = gvData.CurrentRow.Cells["PART_NO"].Value.ToString();
            sUnitWeight = gvData.CurrentRow.Cells["OPTION5"].Value.ToString();
            DialogResult = DialogResult.OK;
        }

        private void showData()
        {
            string SQL = "";
            string secondFilter = editValue.Text;
            switch (combField.SelectedIndex)
            {
                case 0: // PART_ID
                case 1: // SPEC1
                case 2: // OPTION5
                    SQL = @"
SELECT PART_NO
      ,SPEC1
      ,OPTION5
      ,'' CUSTOMER_ID
      ,'' CUSTOMER_NAME
FROM SAJET.SYS_PART
WHERE 1 = 1
AND UPPER(PART_NO) LIKE 'F%'
{0}
";
                    break;
                case 3: // CUSTOMER_ID
                case 4: // CUSTOMER_NAME
                    SQL = @"


WITH A AS
(
    SELECT A.PART_NO
          ,A.SPEC1
          ,A.OPTION5
          ,C.CUSTOMER_ID
    FROM SAJET.SYS_PART A
        ,SAJET.SYS_ORD_H C
        ,SAJET.SYS_ORD_D D
    WHERE C.NUMBER1 = D.NUMBER1
    AND C.NUMBER2 = D.NUMBER2
    AND A.PART_NO = TRIM(D.PRODUCE_NUMBER)
    AND UPPER(A.PART_NO) LIKE 'F%'
    AND  C.CUSTOMER_ID IS NOT NULL
    {0}
    GROUP BY A.PART_NO
            ,A.SPEC1
            ,A.OPTION5
            ,C.CUSTOMER_ID
)
SELECT A.*
      ,B.CUSTOMER_NAME
FROM A
    ,SAJET.SYS_CUSTOMER B
WHERE A.CUSTOMER_ID = B.CUSTOMER_CODE(+)
ORDER BY A.PART_NO
";
                    break;
                default:
                    break;
            }

            string SQL_WHERE = "";
            if (!string.IsNullOrWhiteSpace(firstFilter))
            {
                SQL_WHERE += @"
AND PART_NO LIKE :PART_NO || '%'
";
            }

            if (!string.IsNullOrWhiteSpace(editValue.Text))
            {
                SQL_WHERE += $@"
AND {fieldList[combField.SelectedIndex]} LIKE :SEARCH_VALUE || '%'
";
            }

            SQL = string.Format(SQL, SQL_WHERE);

            if (!string.IsNullOrEmpty(SQL))
            {
                object[][] param = new object[][]
                {
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "PART_NO", firstFilter },
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "SEARCH_VALUE", secondFilter },
                };
                DataSet ds = ClientUtils.ExecuteSQL(SQL, param);

                gvData.DataSource = ds;
                gvData.DataMember = ds.Tables[0].ToString();

                if (gvData.Rows.Count > 0)
                    gvData.CurrentCell = gvData.Rows[0].Cells[0];

                editValue.Focus();
            }
        }
    }
}
