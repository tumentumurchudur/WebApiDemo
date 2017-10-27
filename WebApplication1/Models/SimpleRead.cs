using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    [JsonObject]
    public class SimpleRead
    {
        //[JsonProperty(PropertyName = "count")]
        //public int Count { get; private set; }

        //[JsonProperty(PropertyName = "lastReadTime")]
        //public double LastReadTime { get; private set; }

        //[JsonProperty(PropertyName = "total")]
        //public double Total { get; private set; }

        [JsonProperty(PropertyName = "delta")]
        public double Delta { get; private set; }

        //public SimpleRead(double Total, double Delta)
        //{
        //    this.Total = Total;
        //    this.Delta = Delta;
        //}

        public SimpleRead(double Delta)
        {
            this.Delta = Delta;
        }
    }
}
