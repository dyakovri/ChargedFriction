using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChargedFriction
{
    public class graphics
    {
        const int grFields = 0;

        private PictureBox pb;
        private Graphics gr;

        private Point grCenter;
        private int gridStepX = 0, gridStepY = 0;
        private double grScaleX = 0, grScaleY = 0;
        private string xAxisName = "", yAxisName = "";

        public graphics(PictureBox pb)
        {
            this.pb = pb;
            gr = pb.CreateGraphics();
        }

        public void Clear()
        {
            gr.Clear(pb.BackColor);
        }

        public void Setup(String xAxisName, double xAxisMaxVal, String yAxisName, double yAxisMaxVal)
        {
            this.xAxisName = xAxisName;
            this.yAxisName = yAxisName;

            
            this.grCenter.X = 0;
            this.grCenter.Y = pb.Height / 2;

            grScaleX = (xAxisMaxVal / 10);
            grScaleY = (yAxisMaxVal / 10);

            gridStepX = pb.Width / 10;
            gridStepY = pb.Height / 10;
        }

        public void ReCenter()
        {
            this.grCenter.X = pb.Width / 2;
        }

        public void MakeGrid()
        {
            if (this.grScaleX == 0) { this.grScaleX = gridStepX; }
            if (this.grScaleY == 0) { this.grScaleY = gridStepY; }

            for (int i = grCenter.X; (i >= grFields) && (i <= (pb.Width - grFields)); i += gridStepX)
            {
                gr.DrawLine(new Pen(Color.Gray, 1), i, grFields, i, pb.Height - grFields);
            }
            for (int i = grCenter.X; (i >= grFields) && (i <= (pb.Width - grFields)); i -= gridStepX)
            {
                gr.DrawLine(new Pen(Color.Gray, 1), i, grFields, i, pb.Height - grFields);
            }

            for (int i = grCenter.Y; (i >= grFields) && (i <= (pb.Height - grFields)); i += gridStepY)
            {
                gr.DrawLine(new Pen(Color.Gray, 1), grFields, i, pb.Width - grFields, i);
            }
            for (int i = grCenter.Y; (i >= grFields) && (i <= (pb.Height - grFields)); i -= gridStepY)
            {
                gr.DrawLine(new Pen(Color.Gray, 1), grFields, i, pb.Width - grFields, i);
            }

            gr.DrawLine(new Pen(Color.Black, 1), grCenter.X, grFields, grCenter.X, pb.Height - grFields);
            gr.DrawLine(new Pen(Color.Black, 1), grFields, grCenter.Y, pb.Width - grFields, grCenter.Y);

            AddString(grCenter.X, 5, yAxisName, false);
            AddString(pb.Width - 100, grCenter.Y - 16, xAxisName, false);
            AddString(0, 0, "0");

            AddString(gridStepX, 16, Convert.ToString(grScaleX));
            AddString(0, gridStepY, Convert.ToString(grScaleY));
        }

        public void AddString(int x, int y, string s, bool fromCenter = true)
        {
            AddString(x, y, s, Color.Black, fromCenter);
        }

        public void AddString(int x, int y, string s, Color c, bool fromCenter = true)
        {
            if (fromCenter) { x += grCenter.X; y = grCenter.Y - y; }
            gr.DrawString(s, new Font("OpenSans", 8), new SolidBrush(c), x, y);
        }

        public bool AddDot(int x, int y, Color c, bool fromCenter = true)
        {
            try
            {
                if (fromCenter) { x += grCenter.X; y = grCenter.Y - y; }
                gr.DrawRectangle(new Pen(new SolidBrush(c), 3), x, y, 1, 1);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool AddGraphDot(double x, double y)
        {
            try
            {
                return AddGraphDot(x, y, Color.Red);
            }
            catch (Exception) { return false; }
        }
        public bool AddGraphDot(double x, double y, Color c)
        {
            try
            {
                return AddDot((int)((double)gridStepX / grScaleX * x), (int)((double)gridStepY / grScaleY * y), c, true);
            }
            catch (Exception) { return false; }
        }
    }
}
