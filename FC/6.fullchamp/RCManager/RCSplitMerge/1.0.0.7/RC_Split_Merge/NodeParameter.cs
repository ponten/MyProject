using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Lassalle.Flow;

namespace RC_Split_Merge
{
    /// <summary>
    /// AddFlow繪圖參數
    /// </summary>
    public class AddflowZoneParameter
    {
        private int _Margin_Top = 40;
        private int _Margin_Left = 40;
        private PointF _initializeLocation = new PointF(25, -25);

        public PointF InitializeLocation { get { return _initializeLocation; } }
        public int Margin_Left { get { return _Margin_Left; } }
        public int Margin_Top { get { return _Margin_Top; } }

        public Dictionary<string, Lassalle.Flow.Node> d_LastNode;//持續記錄相同名稱的最新物件
        private NodeParameter np;

        public AddflowZoneParameter()
        {
            d_LastNode = new Dictionary<string, Node>();
            np = new NodeParameter();
        }


    }

    public class NodeParameter
    {
        public int Node_DrawWidth = 150;
        public int Node_DrawHigh = 70;
        public Color Node_DrawColor = Color.SteelBlue;
        public Color Node_FontColor = Color.White;
        public Font Node_Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
        public int LINK_DrawWidth = 2;
        public ShapeStyle Node_ShapeStyle = ShapeStyle.RoundRect;
        public bool IsFormNoteCreated = false;
        public bool IsToNoteCreated = false;
        public void reSetNoteCreateStatus()
        {
            IsFormNoteCreated = IsToNoteCreated = false;
        }
    }

}
