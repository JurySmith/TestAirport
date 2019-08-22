using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public static class SheduleGenerator
    {
        static List<int> airplaneTypes;
        static List<string> cities;

        public static Schedule Generate()
        {
            airplaneTypes = new List<int>() { 50, 100, 150, 200, 250 };
            cities = TimeZoneInfo.GetSystemTimeZones().Select(t => t.DisplayName).ToList();
            
            var random = new Random();
            var list = new List<Flight>();

            for (int i = 0; i < 30000; i++)
            {
                var time = new DateTime();

                time = time.AddHours(random.Next(0, 24));
                time = time.AddMinutes(random.Next(0, 60));
                time = time.AddDays(random.Next(0, 31));

                var flight = new Flight()
                {
                    MaxPassCount = airplaneTypes[random.Next(0, airplaneTypes.Count)],
                    Time = time,
                    Type = random.Next(0, 2) % 2 == 0 ? FlightType.Arrival : FlightType.Departure,
                    City = cities[random.Next(0, cities.Count)]
                };

                list.Add(flight);
            }

            var result = new Schedule();
            result.Flights = new Stack<Flight>(list.OrderByDescending(f => f.Time));

            return result;
        }
    }
}
