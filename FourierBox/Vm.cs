namespace FourierBox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;
    using NUnit.Framework;
    using OxyPlot;

    public class Vm : INotifyPropertyChanged
    {
        private SampleData _selectedSample;
        private int _numberOfPoints;
        private readonly IEnumerable<SampleData> _functions;
        public Vm()
        {
            DataPoints = new List<DataPoint>();
            FourierPoints = new List<DataPoint>();
            Spectrum = new List<DataPoint>();
            MinX = 6;
            MaxX = 16;
            var onePeriod = new SineSeries(SineParameters.FromPeriod(1, MaxX - MinX, 0));
            var twoPeriods = new SineSeries(SineParameters.FromPeriod(1, (MaxX - MinX) / 2.0, 0));
            var tenPeriods = new SineSeries(SineParameters.FromPeriod(1, (MaxX - MinX) / 10.0, 0));
            var notPeriod = new SineSeries(SineParameters.FromPeriod(1, 0.7 * (MaxX - MinX), 0));

            var cos = new SineSeries(SineParameters.FromPeriod(1, (MaxX - MinX), Math.PI / 2));
            var offsetSine = new SineSeries(new SineParameters(1, 0, 0), SineParameters.FromPeriod(1, MaxX - MinX, 0));

            _functions = new List<SampleData>
            {
                new SampleData(onePeriod, "One period"),
                new SampleData(twoPeriods, "Two periods"),
                new SampleData(tenPeriods, "Ten periods"),
                new SampleData(notPeriod, "0.7 periods"),
                new SampleData(cos, "cos"),
                new SampleData(offsetSine, "offset"),
                new SampleData(new NoisySine(onePeriod, 0.1), "noisy sine"),
                new SampleData(new NoisySine(onePeriod, 2), "really noisy sine"),
                new SampleData(new SquareSeries(0,5), "Square"),
                new SampleData(new Polynom(0, 1), "y = x"),
                new SampleData(new NoiseSeries(1), "random noise"),
            };
            _selectedSample = _functions.First();

            NumberOfPoints = 10;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ChartUpdated;
        public int NumberOfPoints
        {
            get { return _numberOfPoints; }
            set
            {
                if (value == _numberOfPoints)
                {
                    return;
                }
                _numberOfPoints = value;
                OnPropertyChanged();
                Update();
            }
        }
        public int MinX { get; private set; }
        public int MaxX { get; private set; }
        public double MinY
        {
            get
            {
                return Math.Min(DataPoints.Min(x => x.Y), FourierPoints.Min(x => x.Y));
            }
        }
        public double MaxY
        {
            get
            {
                return Math.Max(DataPoints.Max(x => x.Y), FourierPoints.Max(x => x.Y));
            }
        }
        public List<DataPoint> DataPoints { get; private set; }
        public List<DataPoint> FourierPoints { get; private set; }
        public IEnumerable<SampleData> Functions
        {
            get { return _functions; }
        }
        public SampleData SelectedSample
        {
            get { return _selectedSample; }
            private set
            {
                if (Equals(value, _selectedSample))
                {
                    return;
                }
                _selectedSample = value;
                OnPropertyChanged();
                Update();
            }
        }
        public FourierSeries FourierSeries { get; private set; }
        public List<DataPoint> Spectrum { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected virtual void OnChartUpdated()
        {
            EventHandler handler = ChartUpdated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        private void Update()
        {
            DataPoints.Clear();
            double range = MaxX - MinX;
            var xs = Enumerable.Range(0, NumberOfPoints + 1)
                               .Select(x => MinX + (x * range) / (NumberOfPoints))
                               .ToArray();
            DataPoints.AddRange(xs.Select(x => new DataPoint(x, SelectedSample.Function.Evaluate(x))));
            FourierSeries = new FourierSeries(DataPoints.Take(NumberOfPoints).Select(p => p.Y));
            FourierPoints.Clear();
            FourierPoints.AddRange(xs.Select((x, i) => new DataPoint(x, FourierSeries.Evaluate(i))));
            Spectrum.Clear();
            Spectrum.AddRange(FourierSeries.Spectrum.Select(p => new DataPoint(p.X, p.Y)));
            OnPropertyChanged("MinX");
            OnPropertyChanged("MaxX");
            OnPropertyChanged("MinY");
            OnPropertyChanged("MaxY");
            OnChartUpdated();
        }
    }
}
