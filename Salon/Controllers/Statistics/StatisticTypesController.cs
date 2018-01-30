using Salon.Models.Statistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Salon.Controllers.Statistics
{
    public class StatisticTypesController : Controller
    {
        private List<StatisticTypes> definedStatistics = new List<StatisticTypes>();

        public object Streamreader { get; private set; }

        /// <summary>
        /// Controller of the statistics main menu
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            definedStatistics.Add(new StatisticTypes(
                "Besuche pro Monat", "Zeigt die Anzahl der Kundenbesuche pro Monat", "Grafik", "/Chart/LineChart?chartName=VisitsMonth"
                ));

            if (User.IsInRole("Admin") || User.IsInRole("Lehrer"))
            {
                definedStatistics.Add(new StatisticTypes(
                  "Kundenauswertung", "Zeigt eine Liste aller Kunden mit vielen Filteroptionen", "Auswertung", "/Statistics/CustomerStatistics"
                  ));
            }
            if (User.IsInRole("Admin") || User.IsInRole("Lehrer"))
            {
                definedStatistics.Add(new StatisticTypes(
                "Schülerauswertung", "Zeigt eine Liste aller Schüler mit deren Arbeitsschritten an", "Auswertung", "/Statistics/WorkPerClass"
                ));
            }
            if (User.IsInRole("Schueler"))
            {
                definedStatistics.Add(new StatisticTypes(
                    "Meine Arbeit", "Zeigt eine Liste aller Arbeiten des angemeldeten Schülers an", "Auswertung", "/Statistics/MyWork"
                    ));
            }

            return View(definedStatistics);
        }
    }
}