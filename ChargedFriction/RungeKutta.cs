using System;
using System.Collections.Generic;
using System.Text;

namespace ChargedFriction
{
    abstract class RungeKutta
    {
        /// <summary>
        /// Текущее время
        /// </summary>
        public double t;

        /// <summary>
        /// Искомое решение Y[0] - само решение, Y[i] - i-тая производная решения
        /// </summary>
        /// 
        public double[] Y;
        /// <summary>
        /// Внутренние переменные 
        /// </summary>
        /// 
        double[] YY, Y1, Y2, Y3, Y4;
        protected double[] FY;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="N">размерность системы</param>
        public RungeKutta(uint N)
        {
            Init(N);
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public RungeKutta() { }

        /// <summary>
        /// Выделение памяти под рабочие массивы
        /// </summary>
        /// <param name="N">Размерность массивов</param>
        protected void Init(uint N)
        {
            Y = new double[N];
            YY = new double[N];
            Y1 = new double[N];
            Y2 = new double[N];
            Y3 = new double[N];
            Y4 = new double[N];
            FY = new double[N];
        }

        /// <summary>
        /// Установка начальных условий
        /// </summary>
        /// <param name="t0">Начальное время</param>
        /// <param name="Y0">Начальное условие</param>
        public void SetInit(double t0, double[] Y0)
        {
            t = t0;
            if (Y == null)
                Init((uint)Y0.Length);
            for (int i = 0; i < Y.Length; i++)
                Y[i] = Y0[i];
        }

        /// <summary>
        /// Расчет правых частей системы
        /// </summary>
        /// <param name="t">текущее время</param>
        /// <param name="Y">вектор решения</param>
        /// <returns>правая часть</returns>
        abstract public double[] F(double t, double[] Y);

        /// <summary>
        /// Следующий шаг метода Рунге-Кутта
        /// </summary>
        /// <param name="dt">текущий шаг по времени (может быть переменным)</param>
        public double[] NextStep(double dt)
        {
            int i;

            if (dt < 0) throw new Exception();

            // рассчитать Y1
            Y1 = F(t, Y);

            for (i = 0; i < Y.Length; i++)
                YY[i] = Y[i] + Y1[i] * (dt / 2.0);

            // рассчитать Y2
            Y2 = F(t + dt / 2.0, YY);

            for (i = 0; i < Y.Length; i++)
                YY[i] = Y[i] + Y2[i] * (dt / 2.0);

            // рассчитать Y3
            Y3 = F(t + dt / 2.0, YY);

            for (i = 0; i < Y.Length; i++)
                YY[i] = Y[i] + Y3[i] * dt;

            // рассчитать Y4
            Y4 = F(t + dt, YY);

            // рассчитать решение на новом шаге
            for (i = 0; i < Y.Length; i++)
                Y[i] = Y[i] + dt / 6.0 * (Y1[i] + 2.0 * Y2[i] + 2.0 * Y3[i] + Y4[i]);

            // рассчитать текущее время
            t = t + dt;

            return FY;
        }
    }
}
