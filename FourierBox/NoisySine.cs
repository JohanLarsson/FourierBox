namespace FourierBox
{
    using System;

    public class NoisySine : IFunction
    {
        private readonly SineSeries _sineSeries;
        private readonly double _stddev;
        private readonly Random _random = new Random();
        public NoisySine(SineSeries sineSeries, double stddev)
        {
            _sineSeries = sineSeries;
            _stddev = stddev;
        }
        public double Evaluate(double x)
        {
            return _sineSeries.Evaluate(x) + MathNet.Numerics.Distributions.Normal.Sample(_random, 0, _stddev);
        }
    }
}
