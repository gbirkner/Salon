using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models.Statistics;

namespace Salon.Controllers.Statistics
{
    public class LineChartController : Controller
    {
        // GET: LineChart
        public ActionResult LineChart()
        {
            var intList = new List<int>();
            intList.Add(3);
            intList.Add(5);
            intList.Add(2);

            var stringList = new List<string>();
            stringList.Add("Jannuary");
            stringList.Add("February");
            stringList.Add("March");

            var chart = new LineChart("Customers per month", "Visits", stringList, intList);
            return View(chart);
        }
    }
}