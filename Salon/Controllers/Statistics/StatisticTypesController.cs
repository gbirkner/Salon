using Salon.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salon.Controllers.Statistics
{
    public class StatisticTypesController : Controller
    {
        // GET: StatisticTypes
        public ActionResult Index()
        {
            List<StatisticTypes> definedStatistics = new List<StatisticTypes>();
            definedStatistics.Add(new StatisticTypes(
                "Customers per TIME", "Shows the customers for a specific time span", "Report", "/"
                ));

            definedStatistics.Add(new StatisticTypes(
                "Visits per TIME", "Shows the visits for a specific time span", "Report", "/"
                ));

            return View(definedStatistics);
        }
    }
}