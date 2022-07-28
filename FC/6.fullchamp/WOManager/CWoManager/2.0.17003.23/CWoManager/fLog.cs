using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CWoManager
{
    public partial class fLog : Form
    {
        DataSet dsTemp;
        struct TControlData
        {
            public string sFieldName;
            public TextBox txtControl;
        }
        TControlData[] m_tControlData;

        public fLog(string sSQL, object[][] Params)
        {
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            InitializeComponent();
            int iColumn = dsTemp.Tables[0].Columns.Count - 2;
            m_tControlData = new TControlData[iColumn];
            tableLayoutPanel1.RowCount = (int)Math.Ceiling((decimal)m_tControlData.Length / 2) + 1;
            Height = (tableLayoutPanel1.RowCount - 1) * 25 + 112;
            for (int i = 2; i < dsTemp.Tables[0].Columns.Count - 1; i = i + 2)
                tableLayoutPanel1.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100 / tableLayoutPanel1.RowCount));
            int iCol = 0, iRow = 1, iCount = 0;
            Label lablTemp;
            TextBox txtTemp;
            foreach (DataColumn dc in dsTemp.Tables[0].Columns)
            {
                switch (dc.ColumnName)
                {
                    case "WORK_ORDER":
                    case "UPDATE_TIME":
                        break;
                    default:
                        lablTemp = new Label
                        {
                            Font = new Font("新細明體", 11),
                            Text = SajetCommon.SetLanguage(dc.ColumnName),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Dock = DockStyle.Fill
                        };
                        tableLayoutPanel1.Controls.Add(lablTemp, iCol, iRow);
                        txtTemp = new TextBox
                        {
                            ForeColor = Color.Maroon,
                            ReadOnly = true,
                            BackColor = SystemColors.Window,
                            Dock = DockStyle.Fill,
                            Font = new Font("新細明體", 11, FontStyle.Bold)
                        };
                        tableLayoutPanel1.Controls.Add(txtTemp, iCol + 1, iRow);
                        m_tControlData[iCount].txtControl = txtTemp;
                        m_tControlData[iCount].sFieldName = dc.ColumnName;
                        iCount++;
                        iRow++;
                        if (iRow == tableLayoutPanel1.RowCount)
                        {
                            iRow = 1;
                            iCol = 2;
                        }
                        break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < m_tControlData.Length; i++)
            {
                string sFieldName = m_tControlData[i].sFieldName;
                if (dsTemp.Tables[0].Columns.Contains(sFieldName))
                {
                    switch (sFieldName)
                    {
                        case "WO_SCHEDULE_DATE":
                        case "WO_DUE_DATE":
                            if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[comboBox1.SelectedIndex][sFieldName].ToString()))
                                m_tControlData[i].txtControl.Text = DateTime.Parse(dsTemp.Tables[0].Rows[comboBox1.SelectedIndex][sFieldName].ToString()).ToString("yyyy/MM/dd");
                            else
                                m_tControlData[i].txtControl.Text = string.Empty;
                            break;
                        default:
                            m_tControlData[i].txtControl.Text = dsTemp.Tables[0].Rows[comboBox1.SelectedIndex][sFieldName].ToString();
                            break;
                    }
                }
            }
        }

        private void fLog_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                comboBox1.Items.Add(dr["UPDATE_TIME"].ToString());
            comboBox1.SelectedIndex = 0;
        }

        private void fLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            dsTemp.Dispose();
        }
    }
}