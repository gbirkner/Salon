using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models.Statistics
{
    public sealed class StatisticTypes
    {
        [Display(Name = "Name der Statistik")]
        public string StatisticName { get; set; }
        [Display(Name = "Beschreibung")]
        public string StatisticDescription { get; set; }
        [Display(Name = "Typ")]
        public string StatisticType { get; set; }
        [Display(Name = "Zur Statistik")]
        public string StatisticUrl { get; set; }

        public StatisticTypes(string statisticName, string  statisticDescription, string statisticType, string statisticUrl)
        {
            StatisticName = statisticName;
            StatisticDescription = statisticDescription;
            StatisticType = statisticType;
            StatisticUrl = statisticUrl;
        }
    }
}