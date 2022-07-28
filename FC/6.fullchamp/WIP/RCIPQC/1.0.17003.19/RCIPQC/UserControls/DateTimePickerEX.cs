using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RCIPQC
{
    /// <summary>
    /// 客製 DateTimePicker，可以調整背景顏色
    /// </summary>
    public class DateTimePickerEX : System.Windows.Forms.DateTimePicker
    {
        [Browsable(true)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the background color of the control when disabled
        /// </summary>
        [Category("Appearance"), Description("The background color of the component when disabled")]
        [Browsable(true)]
        public Color BackDisabledColor { get; set; }

        private Color _BackColor { get; set; } = System.Drawing.Color.White;

        public DateTimePickerEX() : base()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            BackDisabledColor = Color.FromKnownColor(KnownColor.Control);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();

            //The dropDownRectangle defines position and size of dropdownbutton block, 
            //the width is fixed to 17 and height to 15. The dropdownbutton is aligned to right
            Rectangle dropDownRectangle = new Rectangle(ClientRectangle.Width - 17, 2, 17, ClientRectangle.Height - 4);
            Brush bkgBrush;
            ComboBoxState visualState;

            //When the control is enabled the brush is set to Backcolor, 
            //otherwise to color stored in _backDisabledColor
            if (this.Enabled)
            {
                bkgBrush = new SolidBrush(this.BackColor);
                visualState = ComboBoxState.Normal;
            }
            else
            {
                bkgBrush = new SolidBrush(this.BackDisabledColor);
                visualState = ComboBoxState.Disabled;
            }

            // Painting...in action

            //Filling the background
            g.FillRectangle(bkgBrush, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            //Drawing the datetime text
            g.DrawString(this.Text, this.Font, Brushes.Black, 0, 2);

            //Drawing the dropdownbutton using ComboBoxRenderer
            if (!this.ShowUpDown)
            {
                ComboBoxRenderer.DrawDropDownButton(g, dropDownRectangle, visualState);
            }

            g.Dispose();
            bkgBrush.Dispose();
        }
    }
}
