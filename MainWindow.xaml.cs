using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ScheduleExecutor ScheduleExecutor { get; private set; } 
        public FlightsModel Model { get; } = new FlightsModel();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, a) =>
            {

                departureControl.DataContext = Model.DepartureFlights;
                arrivalControl.DataContext = Model.ArrivalFlights;
                lastDayStat.DataContext = Model;
                DataContext = Model;

                ScheduleExecutor = new ScheduleExecutor(Model);
                ScheduleExecutor.Start();

                timeBox.DataContext = ScheduleExecutor;
            };

        }

        int tumbValue = 1;

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tumbValue = (int)e.NewValue;
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            ScheduleExecutor.Speed = tumbValue;
        }
    }
}
