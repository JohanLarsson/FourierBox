namespace FourierBox
{
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
        public double Phase { get; private set; }
        public override string ToString()
        {
            return string.Format("Amplitude: {0}, Frequency: {1}, Phase: {2}", Amplitude, Frequency, Phase);
        }
    }
}
