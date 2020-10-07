using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_A4_NRK3
{
	public interface ISystemOfEquations
	{
		double InitialXValue { get; }
		double InitialYValue { get; }

		double DerivativeOfY(double t, double x, double y);
		double DerivativeOfX(double t, double x, double y);
		double FunctionXofT(double t);
		double FunctionYofT(double t);
	}
}
