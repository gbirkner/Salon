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

        // GET: StatisticTypes
        public ActionResult Index()
        {
            definedStatistics.Add(new StatisticTypes(
                "Besuche pro Monat", "Zeigt die Anzahl der Kundenbesuche pro Monat", "Grafik", "/Chart/LineChart?chartName=VisitsMonth"
                ));
            
            definedStatistics.Add(new StatisticTypes(
                "Kundenauswertung", "Zeigt eine Liste aller Kunden mit vielen Filteroptionen", "Auswertung", "/Reports/CustomerStatistics"
                ));

            definedStatistics.Add(new StatisticTypes(
                "Schülerauswertung", "Zeigt eine Liste aller Schüler mit deren Arbeitsschritten an", "Auswertung", "/Reports/WorkPerClass"
                ));

            definedStatistics.Add(new StatisticTypes(
                "Meine Arbeit", "Zeigt eine Liste aller Arbeiten des angemeldeten Schülers an", "Auswertung", "/Reports/MyWork"
                ));

            return View(definedStatistics);
        }
    }
}