namespace FourierBox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Windows;
    using MathNet.Numerics.IntegralTransforms;

    public class FourierSeries
    {
        private readonly Complex[] _complexes;
        private SineSeries _sineSeries;
        public FourierSeries(IEnumerable<double> samples)
        {
            _complexes = samples.Select(x => new Complex(x, 0))
                                .ToArray();
            var dft = new MathNet.Numerics.IntegralTransforms.Algorithms.DiscreteFourierTransform();
            dft.BluesteinForward(_complexes, FourierOptions.NoScaling);
        }
        public FourierSeries(IEnumerable<double> samples, int i)
            : this(samples)
        {
            if (i <= 0)
                i = 1;
            for (int j = i; j < _complexes.Length; j++)
            {
                _complexes[j] = 0;
            }
        }
        public SineSeries SineSeries
        {
            get
            {
                if (_sineSeries == null)
                {
                    var sineParameterses = new List<SineParameters>();
                    for (int i = 0; i < _complexes.Length / 2; i++)
                    {
                        var complex = _complexes[i];
                        double a = Amplitude(_complexes, i);
                        double phase = complex.Phase + Math.PI / 2;
                        sineParameterses.Add(new SineParameters(a, i, phase));
                    }
                    _sineSeries = new SineSeries(sineParameterses);
                }
                return _sineSeries;
            }
        }
        public double Evaluate(double x)
        {
            return SineSeries.Evaluate(x);
        }
        public Point[] Spectrum
        {
            get
            {
                return Enumerable.Range(0, _complexes.Length / 2)
                              .Select(x => new Point(x, Amplitude(_complexes, x)))
                              .ToArray();
            }
        }

        private static double Amplitude(Complex[] complexes, int index)
        {
            return (index + 1) * complexes[index].Magnitude / complexes.Length;
        }
    }
}