namespace FourierBox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SineSeries : IFunction
    {
        private readonly SineParameters[] _sines;
        public SineSeries(params SineParameters[] sines)
            : this((IEnumerable<SineParameters>)sines)
        {
        }
        public SineSeries(IEnumerable<SineParameters> sines)
        {
            _sines = sines.OrderBy(x => x.Frequency).ToArray();
        }
        public IEnumerable<SineParameters> Parameters
        {
            get
            {
                return _sines;
            }
        }
        public double Evaluate(double x)
        {
            double sum = 0;
            foreach (var sine in _sines)
            {
                if (sine.Frequency == 0)
                {
                    sum += sine.Amplitude;
                }
                else
                {
                    sum += sine.Amplitude * Math.Sin(x * sine.Frequency + sine.Phase);
                }
            }
            return sum;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _sines.Length; i++)
            {
                var sine = _sines[i];
                if (sine.Frequency == 0)
                {
                    sb.Append(sine.Amplitude);
                }
                else
                {
                    var a = sine.Amplitude == 1 ? "" : sine.Amplitude.ToString("F");
                    var f = sine.Frequency == 1 ? "" : sine.Frequency.ToString("F");
                    var p = sine.Phase == 0 ? "" : " + " + sine.Phase.ToString("F2");
                    sb.AppendFormat("{0}sin({1}x/(2Pi){2})", a, f, p);
                }
                if (i < _sines.Length - 1)
                {
                    sb.Append(" + ");
                }
            }
            return sb.ToString();
        }
    }
}