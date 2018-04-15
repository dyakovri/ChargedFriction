using System;
using System.Collections.Generic;
using System.Text;

namespace ChargedFriction
{
    class point : RungeKutta
    {
        private double m, B, B0, V, q, x, y, r, r0, x0, y0;

        public point(double m, double B, double V, double q, double x, double y) : base(4)
        {
            this.m = m;
            this.B = B;
            this.V = V;
            this.q = q;
            this.x = x;
            this.y = y;

            B0 = B;

            r = Math.Sqrt(x * x + y * y);

            x0 = x;
            y0 = y;
            r0 = Math.Sqrt(x0 * x0 + y0 * y0);
        }

        public double get_B() { return B;  }

        public override double[] F(double time, double[] coordinates)
        {
            // x - Y[0], y - Y[1], Vx - Y[2], Vy - Y[3]
            // dx/dt - FY[0] , dy/dt - FY[1] , dVx/dt - FY[2] , dVy/dt - FY[3]

            r = Math.Sqrt(Y[0] * Y[0] + Y[1] * Y[1]);
            if (r > r0)
                B = B0 * Math.Exp((r - r0) / 100);
            else
                B = B0;

            FY[0] = Y[2];
            FY[1] = Y[3];
            FY[2] =  B * Y[3] * q / m; //ax = B*Vy*q/m
            FY[3] =  - B * Y[2] * q / m; //ay = -B*Vx*q/m

            return FY;
        }
    }
}
