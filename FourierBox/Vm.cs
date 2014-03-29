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
            var sine = new SineSeries(new SineParameters(1, 1, 0));
            var cos = new SineSeries(new SineParameters(1, 1, Math.PI/2));
            var offsetSine = new SineSeries(new SineParameters(1, 0, 0), new SineParameters(1, 1, 0));
            var hfSine = new SineSeries(new SineParameters(1, 5, 0));
            _functions = new List<SampleData>
            {
                new SampleData(sine, sine.ToString()),
                new SampleData(cos, cos.ToString()),
                new SampleData(offsetSine, offsetSine.ToString()),
                new SampleData(hfSine, hfSine.ToString()),
                new SampleData(new NoisySine(sine, 0.1), "noisy " + hfSine.ToString()),
                new SampleData(new SquareSeries(), "Square"),
                new SampleData(new Polynom(0, 1), "y = x"),
                new SampleData(new NoiseSeries(1), "noise"),
            };
            _selectedSample = _functions.First();
            NumberOfPoints = 32;
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
        public double MinX
        {
            get
            {
                return DataPoints.Min(x => x.X);
            }
        }
        public double MaxX
        {
            get
            {
                return DataPoints.Max(x => x.X);
            }
        }
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
            var xs = Enumerable.Range(0, NumberOfPoints)
                               .Select(x => 2 * Math.PI * (((double)x) / NumberOfPoints))
                               .ToArray();
            DataPoints.AddRange(xs.Select(x => new DataPoint(x, SelectedSample.Function.Evaluate(x))));
            FourierSeries = new FourierSeries(DataPoints.Select(p => p.Y));
            FourierPoints.Clear();
            FourierPoints.AddRange(xs.Select(x => new DataPoint(x, FourierSeries.Evaluate(x))));
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
