using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.API.Data.Models
{
    public class PeanutSpecification
    {
        // To acces the database property by by common class
        public string? ComId { get; set; }
        public decimal? Cropyear { get; set; }
        public string? BuyingPoint { get; set; }
        public string PnutVarietyId { get; set; }
        public string PnutTypeId { get; set; }
        public string SeedInd { get; set; }
        public string SegType { get; set; }
        public string EdibleOilInd { get; set; }
        public string? errormessage { get; set; }

    }
}
