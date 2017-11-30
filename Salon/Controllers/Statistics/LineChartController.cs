using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models.Statistics;
using Salon.Models;

namespace Salon.Controllers.Statistics
{
    public class LineChartController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: LineChart
        public ActionResult LineChart()
        {
            var data = db.Visits.GroupBy(c => c.Created.Month).Select(g => new { Month = g.Key, Count = g.Count() });
            var dataPoints = new List<int>();
            var dataLabels = new List<string>();

            foreach (var item in data)
            {
                var nameOfMonth = new DateTime().AddMonths(item.Month).ToString("MMMM");
                dataPoints.Add(Convert.ToInt32(item.Count));
                dataLabels.Add(nameOfMonth);
            }

            var chart = new LineChart("Customers per Time", "Visits", dataLabels, dataPoints);
            return View(chart);
        }
    }
}