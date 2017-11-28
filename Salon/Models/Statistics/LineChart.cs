using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.Models.Statistics
{
    public sealed class LineChart
    {
        public List<string> Labels { get; set; }
        public List<int> GraphData { get; set; }

        public LineChart (List<string> labels = null, List<int> graphData = null)
        {
            Labels = labels;
            GraphData = graphData;
        }
    }
}