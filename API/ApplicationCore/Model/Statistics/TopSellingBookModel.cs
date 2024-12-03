using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Model.Statistics
{
    public class TopSellingBookModel
    {
        public int BookID { get; set; }
        public string? BookName { get; set; }
        public string? ImagePath { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
