using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Requests
{
    public class MerkRequest : RequestBase
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public string cmd { get; set; }
        public string[] columns { get; set; }
        public int draw { get; set; }
        public string from { get; set; }
        public int length { get; set; }
        public List<Order> order { get; set; }
        public string start { get; set; }
        public string to { get; set; }
    }

    public class Order {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
