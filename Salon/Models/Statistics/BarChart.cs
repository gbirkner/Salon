using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Salon.Models.Statistics
{
    public sealed class BarChart
    {
        public string ChartTitle { get; set; }
        public List<string> Labels { get; set; }
        public List<ChartData> ChartData { get; set; }

        public string GetLabelString()
        {
            StringBuilder concatString = new StringBuilder();
            foreach (var item in Labels)
            {
                concatString.Append($"'{item}',");
            }

            return concatString.ToString();
        }

        public BarChart(string chartTitle = null, List<string> labels = null, List<ChartData> chartData = null)
        {
            ChartTitle = chartTitle;
            Labels = labels;
            ChartData = chartData;
        }
    }

    public partial class ChartData
    {
        public string GetBarChartString()
        {
            // Bar charts can only have one value for a bar (only one value is displayed)
            return this.DataPoints[0].ToString();
        }
    }
}