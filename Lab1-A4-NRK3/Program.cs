using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lab1_A4_NRK3
{
	class Program
	{
		private static List<double> _xPoints = new List<double>();
		private static List<double> _yPoints = new List<double>();
		private static List<double> _tPoints = new List<double>();

		private static double CalculateOptimalStepInterative(
			double precition,
			double initialStep_H,
			Func<double, double, double, double> functionOfX,
			Func<double, double, double, double> functionOfY)
		{
			bool shouldContinue = true;
			double x11;
			double y11;
			do
			{
				double x1 = _xPoints[0] + initialStep_H * functionOfX(_tPoints[0], _xPoints[0], _yPoints[0]);
				double y1 = _yPoints[0] + initialStep_H * functionOfY(_tPoints[0], _xPoints[0], _yPoints[0]);

				initialStep_H /= 2.0;

				double xc = _xPoints[0] + initialStep_H * functionOfX(_tPoints[0], _xPoints[0], _yPoints[0]);
				double yc = _yPoints[0] + initialStep_H * functionOfY(_tPoints[0], _xPoints[0], _yPoints[0]);
				double tc = _tPoints[0] + initialStep_H;

				x11 = xc + initialStep_H * functionOfX(tc, xc, yc);
				y11 = yc + initialStep_H * functionOfY(tc, xc, yc);

				shouldContinue = Math.Abs(x1 - x11) + Math.Abs(y1 - y11) >= precition;
			}
			while (shouldContinue);

			_xPoints.Add(x11);
			_yPoints.Add(y11);

			return initialStep_H;
		}

		private static double NRK3(double initialStep_H, double tPoint, double ownPoint, double xCalculatedPoint, double yCalculatedPoint, Func<double, double, double, double> function)
		{
			double tPoint_nextValue = tPoint + initialStep_H;
			double T1 = function(tPoint_nextValue, xCalculatedPoint, yCalculatedPoint);
			double T2 = function(tPoint_nextValue - initialStep_H / 3, xCalculatedPoint - initialStep_H / 3 * T1, yCalculatedPoint - initialStep_H / 3 * T1);
			double T3 = function(tPoint_nextValue - initialStep_H * 2 / 3, xCalculatedPoint - initialStep_H * 2 / 3 * T2, yCalculatedPoint - initialStep_H * 2 / 3 * T2);

			return ownPoint + initialStep_H / 4 * (T1 + T3);
		}

		private static double Adams4Rang(
			int startIndex,
			double initialStep_H,
			double ownPoint,
			List<double> tPoints,
			List<double> xPoints,
			List<double> yPoints,
			Func<double, double, double, double> function)
		{
			return ownPoint + initialStep_H / 24 * (55 * function(_tPoints[startIndex + 3], xPoints[startIndex + 3], yPoints[startIndex + 3]) - 59 * function(_tPoints[startIndex + 2], xPoints[startIndex + 2], yPoints[startIndex + 2]) + 37 * function(_tPoints[startIndex + 1], xPoints[startIndex + 1], yPoints[startIndex + 1]) - 9 * function(_tPoints[startIndex], xPoints[startIndex], yPoints[startIndex]));
		}

		private static void CalculationSubsidiarySchema(
			double initialStep_H,
			double eps,
			Func<double, double, double, double> functionOfX,
			Func<double, double, double, double> functionOfY)
		{
			_tPoints.Add(_tPoints.Last() + initialStep_H);
			var x_correction = NRK3(initialStep_H, _tPoints.Last(), _xPoints.Last(), _xPoints.Last(), _yPoints.Last(), functionOfX);
			var y_correction = NRK3(initialStep_H, _tPoints.Last(), _yPoints.Last(), _xPoints.Last(), _yPoints.Last(), functionOfY);
			var x_potential = NRK3(initialStep_H, _tPoints.Last(), _xPoints.Last(), x_correction, y_correction, functionOfX);
			var y_potential = NRK3(initialStep_H, _tPoints.Last(), _yPoints.Last(), x_correction, y_correction, functionOfY);

			while (Math.Abs(x_potential - x_correction) + Math.Abs(y_potential - y_potential) > eps) {
				x_correction = x_potential;
				y_correction = y_potential;

				x_potential = NRK3(initialStep_H, _tPoints.Last(), _xPoints.Last(), x_correction, y_correction, functionOfX);
				y_potential = NRK3(initialStep_H, _tPoints.Last(), _yPoints.Last(), x_correction, y_correction, functionOfY);
			}

			_xPoints.Add(x_potential);
			_yPoints.Add(y_potential);
		}

		static int Main(string[] args)
		{

			Console.WriteLine("Please choose variant of system: ");
			Console.WriteLine("1)\t\t\t\t\t\t\t\t2)");
			Console.WriteLine($"x'(t) = 2x - y + t^2 -2(sin(t) + 1) + cos(t)\t\t\tx'(t) = t / y");
			Console.WriteLine($"y'(t) = x - 2y + sin(t) - 2t^2 + 2t - 1     \t\t\ty'(t) = -t / y");
			Console.WriteLine($"x(0) = 1, y(0) = 0 \t\t\t\t\t\tx(0) = 1, y(0) = 1");
			Console.WriteLine($"x(t) = sin(t) + 1\t\t\t\t\t\tx(t) = e^t^2/2");
			Console.WriteLine($"y(t) = t^2\t\t\t\t\t\t\tx(t) = e^-t^2/2");
			Console.WriteLine("System number: ");

			ISystemOfEquations systemOfEquations;
			switch (Console.ReadLine())
			{
				case "1":
					systemOfEquations = new SystemOfEquationsVariant1();
					break;
				case "2":
					systemOfEquations = new SystemOfEquationsVariant2();
					break;
				default:
					Console.WriteLine("System which you selected doesn't exists.");
					return -1;
			}

			Console.WriteLine("Please enter precition (e): ");
			if(!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double eps) || eps <= 0)
			{
				Console.WriteLine("Incorrect precition (e) format. Precision must be a decimal number great than zero.");
				return -1;
			}

			Console.WriteLine("Please enter initial step (h): ");
			if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double initialStep) || initialStep <= 0)
			{
				Console.WriteLine("Incorrect initial step (h) format. Initial step must be a decimal number great than zero.");
				return -1;
			}

			_xPoints.Add(systemOfEquations.InitialXValue);
			_yPoints.Add(systemOfEquations.InitialYValue);
			_tPoints.Add(0.0);

			// Рахуємо оптимальний крок
			double optimalStep = CalculateOptimalStepInterative(eps, initialStep, systemOfEquations.DerivativeOfX, systemOfEquations.DerivativeOfY);
			Console.WriteLine($"The optimal step is: {optimalStep}");

			// За допомогою НРК3 рахуємо точки x та y в 2 точці оскільки перша точка буде знайдена на кроці визначення оптимального кроку
			CalculationSubsidiarySchema(optimalStep, eps, systemOfEquations.DerivativeOfX, systemOfEquations.DerivativeOfY);
			
			// За допомогою НРК3 рахуємо точки x та y в 3 точці оскільки перша точка буде знайдена на кроці визначення оптимального кроку а друга на попередньому кроці
			CalculationSubsidiarySchema(optimalStep, eps, systemOfEquations.DerivativeOfX, systemOfEquations.DerivativeOfY);

			int index = 0;
			// Використовуємо схему ЯА4;
			for (double i = _tPoints.Last(); i < 1; i += optimalStep)
			{
				_tPoints.Add(_tPoints.Last() + optimalStep);
				_xPoints.Add(Adams4Rang(startIndex: index, optimalStep, _xPoints.Last(), _tPoints, _xPoints, _yPoints, systemOfEquations.DerivativeOfX));
				_yPoints.Add(Adams4Rang(startIndex: index, optimalStep, _yPoints.Last(), _tPoints, _xPoints, _yPoints, systemOfEquations.DerivativeOfY));

				index++;
			}

			for(int i = 0; i < _tPoints.Count; i++)
			{
				Console.WriteLine($"Initial step: { i }");
				Console.WriteLine($"Actual x: { _xPoints[i] }, Expected: { systemOfEquations.FunctionXofT(_tPoints[i]) }, Eps: {_xPoints[i] - systemOfEquations.FunctionXofT(_tPoints[i])}");
				Console.WriteLine($"Actual y: { _yPoints[i] }, Expected: { systemOfEquations.FunctionYofT(_tPoints[i]) }, Eps: {_yPoints[i] - systemOfEquations.FunctionYofT(_tPoints[i])}");
			}

			return 0;
		}
	}
}
