using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChargedFriction
{
    public partial class Form1 : Form
    {
        const double tmax = 20;

        public double m, B, V, q, x, y;
        public static PictureBox[] graphs = new PictureBox[6];
        public static graphics[] gr = new graphics[6];
        string[] grNamesY = { "y(x), м", "x(t), м", "y(t), м", "Fл(√[х^2+y^2]), Н", "Fлx(x), Н", "Fлx(y), Н" };
        double[] grValuesY = { 200, 500, 500, 100, 100, 100 };
        string[] grNamesX = { "x, м", "t, c", "t, c", "√[х^2+y^2], м", "t, c", "t, c" };
        double[] grValuesX = { 500, tmax, tmax, 200, tmax, tmax };

        bool isStarted = false, drawing = true;

        point P;

        public Form1()
        {
            InitializeComponent();
            graphs[0] = graph1;
            graphs[1] = graph2;
            graphs[2] = graph3;
            graphs[3] = graph4;
            graphs[4] = graph5;
            graphs[5] = graph6;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            switch (isStarted) {
                case false:
                    m = (double)m_setter.Value;
                    B = (double)B_setter.Value;
                    V = (double)V_setter.Value;
                    q = (double)q_setter.Value;
                    x = (double)x0_setter.Value;
                    y = (double)y0_setter.Value;

                    m_setter.Enabled = false;
                    B_setter.Enabled = false;
                    V_setter.Enabled = false;
                    q_setter.Enabled = false;
                    x0_setter.Enabled = false;
                    y0_setter.Enabled = false;

                    P = new point(m, B, V, q, x, y);
                    double[] Y0 = { x, y, V, 0 };
                    P.SetInit(0, Y0);

                    for (int i = 0; i < gr.Length; i++)
                    {
                        gr[i] = new graphics(graphs[i]);
                        gr[i].Setup(grNamesX[i], grValuesX[i], grNamesY[i], grValuesY[i]);
                        if ((i == 0)) gr[i].ReCenter();
                        gr[i].MakeGrid();
                    }

                    isStarted = true;
                    timer.Start();

                    startButton.Text = "Stop";

                    break;

                case true:
                    isStarted = false;
                    timer.Stop();

                    foreach (graphics g in gr)
                    {
                        g.Clear();
                    }

                    m_setter.Enabled =  true;
                    B_setter.Enabled =  true;
                    V_setter.Enabled =  true;
                    q_setter.Enabled =  true;
                    x0_setter.Enabled = true;
                    y0_setter.Enabled = true;

                    pauseBox1.CheckState = CheckState.Unchecked;

                    startButton.Text = "Start";
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!drawing) return;

            double x, y, t, Vx, Vy;

            double[] FY = P.NextStep(0.01);

            t = P.t;
            if (t > tmax) pauseBox1.CheckState = CheckState.Indeterminate;

            B = P.get_B();

            x = P.Y[0];
            y = P.Y[1];
            Vx = FY[0];
            Vy = FY[1];

            gr[0].AddGraphDot(x,y);
            gr[1].AddGraphDot(t,x);
            gr[2].AddGraphDot(t,y);
            gr[3].AddGraphDot(Math.Sqrt(x * x + y * y), B * Math.Sqrt(Vx * Vx + Vy * Vy) * q);
            gr[4].AddGraphDot(t, B * Vx * q);
            gr[5].AddGraphDot(t, B * Vy * q);
        }

        private void pauseBox_CheckedChanged(object sender, EventArgs e)
        {
            drawing = !pauseBox1.Checked;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
