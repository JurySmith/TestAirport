using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TestApp
{
    public class ScheduleExecutor : INotifyPropertyChanged
    {
        FlightsModel model;
        ManualResetEvent resetWait = new ManualResetEvent(false);

        int speed = 1;
        public int Speed
        {
            get => speed;
            set
            {
                resetWait.Set();
                speed = value;
            }
        }

        public DateTime NowTime { get; private set; } = DateTime.Now;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        }

        public ScheduleExecutor(FlightsModel model)
        {
            this.model = model;

            var schedule = SheduleGenerator.Generate();
            schedule.Save("schedule.txt");
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var random = new Random();
                    var schedule = new Schedule();

                    while (true)
                    {
                        schedule.Load("schedule.txt");

                        schedule.Flights = new Stack<Flight>(schedule.Flights
                            .Where(f => f.Time.Day >= NowTime.Day)
                            .Where(f => (f.Time.Day != NowTime.Day) || (f.Time.Day == NowTime.Day && f.Time.Hour >= NowTime.Hour && f.Time.Minute > NowTime.Minute))
                            .OrderByDescending(f => f.Time));

                        if (schedule.Flights.Count == 0)
                            Thread.Sleep(1000);

                        while (true)
                        {
                            if (schedule.Flights.Count == 0)
                                break;

                            var flight = schedule.Flights.Peek();

                            if (flight.Time.Hour != NowTime.Hour || flight.Time.Minute != NowTime.Minute)
                            {
                                NowTime = NowTime.AddMinutes(1);
                                OnPropertyChanged("NowTime");

                                resetWait.WaitOne(60 * 1000 / Speed, false);
                                resetWait.Reset();

                                continue;
                            }

                            flight = schedule.Flights.Pop();
                            flight.PassCount = random.Next(1, flight.MaxPassCount);
                            flight.Time = flight.Time.AddMonths(NowTime.Month - 1).AddYears(NowTime.Year - 1);

                            model.Add(flight);
                        }

                        var next = new DateTime().AddDays(NowTime.Day).AddMonths(NowTime.Month - 1).AddYears(NowTime.Year - 1);
                        NowTime = next;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
