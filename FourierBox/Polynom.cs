using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourierBox
{
    public class Polynom : IFunction
    {
        public Polynom(params double[] coefficients)
        {
            Coefficients = coefficients;
        }
        public static Polynom Parse(string s)
        {
            var cs = new Dictionary<int, double>();
            var reader = new ExpressionReader(s);
            if (reader.HasLeftSide)
                reader.ReadLeftSide();
            while (!reader.IsEos)
            {
                var sign = reader.ReadSign();
                double c = reader.ReadCoefficient();
                var i = reader.ReadDegree();
                cs.Add(i, sign * c);
            }
            var coffs = new double[cs.Keys.Max() + 1];
            foreach (var c in cs)
            {
                coffs[c.Key] = c.Value;
            }
            return new Polynom(coffs);
        }
        public double[] Coefficients { get; private set; }
        public double Evaluate(double x)
        {
            double y = 0;
            for (int i = 0; i < Coefficients.Count(); i++)
            {
                y += Coefficients[i] * Math.Pow(x, i);
            }
            return y;
        }
        public override string ToString()
        {
            return ToString("n3");
        }
        public string ToString(int digits)
        {
            return ToString("n" + digits);
        }
        public string ToString(string format)
        {
            var sb = new StringBuilder();
            sb.Append("y = ");
            bool hasTerm = false;
            for (int i = 0; i < Coefficients.Length; i++)
            {
                var c = Coefficients[i];
                if (c == 0)
                    continue;
                if (hasTerm)
                {
                    if (c > 0)
                        sb.Append(" + ");
                    else
                    {
                        sb.Append(" - ");
                    }
                }
                else
                {
                    if (c < 0)
                        sb.Append("-");
                }
                hasTerm = true;
                if (Math.Abs(c) == 1 && i > 0)
                {
                    sb.Append("x");
                }
                else if (i > 0)
                {
                    sb.AppendFormat("{0} * x", Math.Abs(c).ToString(format));
                }

                switch (i)
                {
                    case 0:
                        sb.Append(Math.Abs(c).ToString(format));
                        break;
                    case 1:
                        break;
                    default:
                        sb.AppendFormat("^{0}", i);
                        break;

                }

            }
            return sb.ToString();
        }

    }
}
