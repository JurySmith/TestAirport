using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class FlightsModel : INotifyPropertyChanged
    {
        public Flight LastFlight { get; set; } 
        public FlightsCollection ArrivalFlights { get; private set; } = new FlightsCollection();
        public FlightsCollection DepartureFlights { get; private set; } = new FlightsCollection();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        }


        public void Add(Flight flight)
        {
            switch (flight.Type)
            {
                case FlightType.Arrival:
                    ArrivalFlights.Add(flight);
                    break;

                case FlightType.Departure:
                    DepartureFlights.Add(flight);
                    break;
            }

            LastFlight = flight;
            OnPropertyChanged("LastFlight");
        }
    }
}
