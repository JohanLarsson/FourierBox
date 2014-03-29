namespace FourierBox
{
    using System;

    public class SquareSeries : IFunction
    {
        public double Evaluate(double x)
        {
            if (x < Math.PI/2)
                return 0;
            if(x<1.5*Math.PI)
                return 1;
            return 0;
        }
    }
}
