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
        private SineSeries _sineSeries;
        private int _numberOfPoints;
        private IEnumerable<SineSeries> _sineSerieses;
        public Vm()
        {
            DataPoints = new List<DataPoint>();
            FourierPoints = new List<DataPoint>();
            Spectrum = new List<DataPoint>();
            _sineSerieses = new List<SineSeries>
            {
                new SineSeries(new SineParameters(1, 1, 0)),
                new SineSeries(new SineParameters(1, 0, 0),new SineParameters(1, 1, 0)),
            };
            _sineSeries = _sineSerieses.First();
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
        public IEnumerable<SineSeries> SineSerieses
        {
            get { return _sineSerieses; }
        }
        public SineSeries SineSeries
        {
            get { return _sineSeries; }
            private set
            {
                if (Equals(value, _sineSeries))
                {
                    return;
                }
                _sineSeries = value;
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
            DataPoints.AddRange(xs.Select(x => new DataPoint(x, SineSeries.Evaluate(x))));
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
