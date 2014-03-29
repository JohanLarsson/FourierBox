namespace FourierBox
{
    using System;

    public class SquareSeries : IFunction
    {
        public SquareSeries(double start, double period)
        {
            Start = start;
            Period = period;
        }
        public double Start { get; private set; }
        public double Period { get; private set; }
        public double Evaluate(double x)
        {
            double diff = Math.Abs(x - Start);
            double d = diff%Period;
            if (d > Period/2)
                return 1;
            return 0;
        }
    }
}
