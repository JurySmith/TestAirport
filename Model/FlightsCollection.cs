using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace TestApp
{
    public class ChartDataValue
    {
        public string Time { get; set; }
        public int Count { get; set; }
    }

    public class FlightsCollection : INotifyPropertyChanged
    {
        readonly ObservableCollection<Flight> source = new ObservableCollection<Flight>();
        public int LastCount { get => source.Count == 0 ? 0 : source.Last().PassCount; }
        public int TotalCount { get => source.Sum(f => f.PassCount); }
        public int LastDayCount
        {
            get
            {
                if (source.Count > 0)
                {
                    var lastTime = source.Max(f => f.Time);
                    return source.Where(f => f.Time <= lastTime && f.Time >= lastTime.AddHours(-24)).Sum(f => f.PassCount);
                }
                else
                    return 0;
            }
        }

        public List<KeyValuePair<string, int>> LastDayStat
        {
            get
            {
                if (source.Count > 0)
                {
                    var lastTime = source.Max(f => f.Time);
                    var beginDate = new DateTime().AddYears(lastTime.Year - 1).AddMonths(lastTime.Month - 1).AddDays(lastTime.Day - 1).AddHours(lastTime.Hour - 23);

                    var res = source
                        .Where(f => f.Time <= lastTime && f.Time >= beginDate)
                        .Select(f => new KeyValuePair<string, int>(f.Time.ToString("HH:00"), f.PassCount))
                        .GroupBy(p => p.Key)
                        .Select(g => new KeyValuePair<string, int>(g.Key, g.Sum(x => x.Value)))
                        .ToList();

                    return res;
                }
                else
                    return new List<KeyValuePair<string, int>>();
            }
        } 


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Add(Flight item)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                source.Add(item);

                OnPropertyChanged("LastCount");
                OnPropertyChanged("LastDayCount");
                OnPropertyChanged("TotalCount");
                OnPropertyChanged("LastDayStat");
            }));
        }

    }
}
