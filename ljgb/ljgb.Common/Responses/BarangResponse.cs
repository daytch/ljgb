using ljgb.Common.ViewModel;
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
        public CarDetail CarDetail { get; set; }
        public List<Car> RelatedProducts { get; set; }
        public dynamic ListAsks { get; set; }

        public List<BarangViewModel> ListModel { get; set; }
        public BarangViewModel Model { get; set; }

        public List<SP_GetBarangByHomeParameter> sp_GetBarangByHomeParameters { get; set; }

        public BarangResponse()
        {
            ListModel = new List<BarangViewModel>();
            Model = new BarangViewModel();
        }
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class Car
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public Int64 price { get; set; }
    }
    public class CarDetail
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Int64 lowestask { get; set; }
        public Int64 highestbid { get; set; }
    }
    public class CarAsks
    {
        public Int64 ID { get; set; }
        public Int64 Price { get; set; }
        public int Quantity { get; set; }
    }
    public class Position
    {
        public Int64 ID { get; set; }
        public Int64? price_rank { get; set; }
    }

    public class SP_GetBarangByHomeParameter
    {
        public long id { get; set; }
        public string name { get; set; }
        public string photopath { get; set; }
        //public long TypeID { get; set; }
    }
}
