namespace FourierBox
{
    using System;

    public class SineParameters
    {
        public SineParameters(double amplitude, double frequency, double phase)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Phase = phase;
        }
        public double Amplitude { get; private set; }
        public double Frequency { get; private set; }
        public double Period
        {
            get
            {
                return 1.0 / Frequency;
            }
        }
        public double Phase { get; private set; }
        public static SineParameters FromPeriod(double amplitude, double period, double phase)
        {
            return new SineParameters(amplitude, (2*Math.PI) / period, phase);
        }
        public override string ToString()
        {
            const string Pi = "π";
            return string.Format("Amplitude: {0}, Frequency: {1}, Phase: {2}", Amplitude, Frequency, Phase);
        }
    }
}
