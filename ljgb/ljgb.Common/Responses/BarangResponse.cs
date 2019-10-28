using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class BarangResponse : BaseResponse
    {
        public List<Car> LowestAsks { get; set; }
        public List<Car> HighestBids { get; set; }
        public List<Car> ListNormal { get; set; }
    }
    public class Car
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public Int64 price { get; set; }
    }
}
