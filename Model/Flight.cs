using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public enum FlightType : int
    {
        Arrival,
        Departure
    }

    [DataContract]
    public class Flight
    {
        public int PassCount { get; set; }

        [DataMember]
        public int MaxPassCount { get; set; }

        [DataMember]
        public FlightType Type { get; set; }

        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public string City { get; set; }

        public override string ToString()
        {
            return Type + " " + Time.ToString("HH:mm") + " " + City + " " + PassCount;
        }
    }
}
