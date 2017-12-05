using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.Models.Statistics
{
    public class BarChart
    {
        public string ChartTitle { get; set; }
        public List<string> Labels { get; set; }
        public List<List<int>> ChartData { get; set; }
    }
}