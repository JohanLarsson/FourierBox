namespace FourierBox
{
    using System;

    public class NoiseSeries : IFunction
    {
        private readonly double _stddev;
        private readonly Random _random = new Random();
        public NoiseSeries(double stddev)
        {
            _stddev = stddev;
        }
        public double Evaluate(double x)
        {
            return MathNet.Numerics.Distributions.Normal.Sample(_random, 0, _stddev);
        }
    }
}