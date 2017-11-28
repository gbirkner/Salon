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
        public ActionResult Index()
        {
            var chart = new LineChart();
            return View(chart);
        }
    }
}