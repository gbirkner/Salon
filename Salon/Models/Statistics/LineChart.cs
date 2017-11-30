using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Salon.Models.Statistics
{
    public sealed class LineChart
    {
        public string ChartTitle { get; set; }
        public string DataLabel { get; set; }
        public List<string> Labels { get; set; }
        public List<int> ChartData { get; set; }

        public string GetDataLabelString()
        {
            StringBuilder concatValues = new StringBuilder();
            foreach (var item in Labels)
            {
                concatValues.Append($"'{item}',");
            }

            return concatValues.ToString();
        }

        public string GetChartValueString()
        {
            StringBuilder concatValues = new StringBuilder();
            foreach (var item in ChartData)
            {
                concatValues.Append($"'{item}',");
            }

            return concatValues.ToString();
        }

        public LineChart (string chartTitle = null, string dataLabel = null, List<string> labels = null, List<int> chartData = null)
        {
            ChartTitle = chartTitle;
            DataLabel = dataLabel;
            Labels = labels;
            ChartData = chartData;
        }
    }
}