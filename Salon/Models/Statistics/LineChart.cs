using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Salon.Models.Statistics
{
    public partial class ChartData
    {
        public string DataLabel { get; set; }
        public List<int> DataPoints { get; set; }
        public string Color { get; set; }

        public ChartData(string datalabel, List<int> datapoints, string color)
        {
            DataLabel = datalabel;
            DataPoints = datapoints;
            Color = color;
        }

        public string GetDataString()
        {
            StringBuilder concatString = new StringBuilder();
            foreach (var item in DataPoints)
            {
                concatString.Append($"'{item}',");
            }

            return concatString.ToString();
        }
    }

    public sealed class LineChart
    {
        public string ChartTitle { get; set; }        
        public List<string> Labels { get; set; }
        public List<ChartData> ChartData { get; set; }

        public string GetLabelString()
        {
            StringBuilder concatValues = new StringBuilder();
            foreach (var item in Labels)
            {
                concatValues.Append($"'{item}',");
            }

            return concatValues.ToString();
        }

        public LineChart(string chartTitle = null, List<string> labels = null, List<ChartData> chartData = null)
        {
            ChartTitle = chartTitle;
            Labels = labels;
            ChartData = chartData;
        }
    }
}