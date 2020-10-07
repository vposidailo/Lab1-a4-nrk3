using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_A4_NRK3
{
	internal class SystemOfEquationsVariant2 : ISystemOfEquations
	{
		public double InitialXValue => 1.0;

		public double InitialYValue => 1.0;

		public double DerivativeOfX(double t, double x, double y)
		{
			return t / y;
		}

		public double DerivativeOfY(double t, double x, double y)
		{
			return -t / x;
		}

		public double FunctionXofT(double t)
		{
			return Math.Exp(Math.Pow(t, 2) / 2);
		}

		public double FunctionYofT(double t)
		{
			return Math.Exp(-Math.Pow(t, 2) / 2);
		}
	}
}
