using System;

namespace Lab1_A4_NRK3
{
	internal class SystemOfEquationsVariant1 : ISystemOfEquations
	{
		public double InitialXValue => 1.0;

		public double InitialYValue => 0.0;

		public double DerivativeOfX(double t, double x, double y)
		{
			return 2 * x - y + Math.Pow(t, 2) - 2 * (Math.Sin(t) + 1) + Math.Cos(t);
		}

		public double DerivativeOfY(double t, double x, double y)
		{
			return x + 2 * y - Math.Sin(t) - 2 * Math.Pow(t, 2) + 2 * t - 1;
		}

		public double FunctionXofT(double t)
		{
			return Math.Sin(t) + 1;
		}

		public double FunctionYofT(double t)
		{
			return Math.Pow(t, 2);
		}
	}
}
