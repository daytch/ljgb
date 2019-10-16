using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Models
{
    public partial class TrackLevel
    {
        public int Id { get; set; }
        public int TrackStatusId { get; set; }
        public int TrackTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public TrackStatus TrackStatus { get; set; }
        public TrackType TrackType { get; set; }
    }
}
