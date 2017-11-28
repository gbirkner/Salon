using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models.Statistics
{
    public sealed class StatisticTypes
    {
        [Display(Name = "Statistic name")]
        public string StatisticName { get; set; }
        [Display(Name = "Statistic description")]
        public string StatisticDescription { get; set; }
        [Display(Name = "Statistic type")]
        public string StatisticType { get; set; }
        [Display(Name = "Link to statistic")]
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