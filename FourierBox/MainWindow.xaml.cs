namespace FourierBox
{
    using System.Windows;
    using MathNet.Numerics.Statistics;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vm _vm= new Vm();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm;
            _vm.ChartUpdated += (sender, args) =>
            {
                Plot.InvalidatePlot();
                Spectrum.InvalidatePlot();
            };
            Plot.InvalidatePlot();
            Spectrum.InvalidatePlot();
        }
    }
}
