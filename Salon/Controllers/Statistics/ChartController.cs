using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models.Statistics;
using Salon.Models;

namespace Salon.Controllers.Statistics
{
    public class ChartController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: LineChart
        public ActionResult LineChart(string chartName = null)
        {
            if (chartName == "VisitsMonth")
            {
                var data = db.Visits.GroupBy(c => c.Created.Month).Select(g => new { Month = g.Key, Count = g.Count() });
                var Labels = new List<string>();
                var dataPoints = new List<ChartData>();

                var visitCount = new List<int>();
                foreach (var item in data)
                {
                    var nameOfMonth = new DateTime().AddMonths(item.Month).ToString("MMMM");
                    visitCount.Add(Convert.ToInt32(item.Count));
                    Labels.Add(nameOfMonth);
                }

                dataPoints.Add(new ChartData("Visits", visitCount, "#57ab26"));
                var chart = new LineChart("Customers per Month", Labels, dataPoints);

                return View(chart);
            }

            // empty data for empty view if url is called without parameter
            var emptyPoints = new List<ChartData>();
            var emptyLabels = new List<String>();
            emptyPoints.Add(new ChartData("", new List<int>(), "#ccc"));
            emptyLabels.Add("");

            var EmptyChart = new LineChart("", emptyLabels, emptyPoints);
            return View(EmptyChart);
        }

        // GET: BarChart
        public ActionResult BarChart(string chartName = null)
        {
            if (chartName == "TopThreatments")
            {
                var Labels = new List<string>();
                var dataPoints = new List<ChartData>();

                Labels.Add("Kurzhaar Frisur");
                Labels.Add("Dauerwelle");
                Labels.Add("Faerben");

                var random = new Random();

                dataPoints.Add(new ChartData("Kurzhaar", new List<int>() { 13 }, string.Format("#{0:X6}", random.Next(0x1000000))));
                dataPoints.Add(new ChartData("Dauerwelle", new List<int>() { 4 }, string.Format("#{0:X6}", random.Next(0x1000000))));
                dataPoints.Add(new ChartData("Faerben", new List<int>() { 7 }, string.Format("#{0:X6}", random.Next(0x1000000))));

                var chart = new BarChart("Gefragteste Behandlungen", Labels, dataPoints);

                return View(chart);

                // linq to get data
                // var data = db.Visits.GroupBy(c => c.VisitTasks)
            }

            return View();
        }
    }
}