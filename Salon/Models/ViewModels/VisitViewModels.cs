using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    public class VisitShortViewModel {
        [Key]
        public int visitId { get; set; }
        [Display(Name = "Datum")]
        public DateTime created { get; set; }
        [Display(Name = "Kunde")]
        public Customers customer { get; set; }
        [Display(Name = "Stylist")]
        public AspNetUsers stylist { get; set; }
        
    }

    public class VisitDetailViewModel {
        [Key]
        public int visitId { get; set; }
        [Display(Name = "Datum")]
        public DateTime created { get; set; }
        [Display(Name = "Kunde")]
        public Customers customer { get; set; }
        [Display(Name = "Stylist")]
        public AspNetUsers stylist { get; set; }
        [Display(Name = "Dauer")]
        public int duration { get; set; }
        [Display(Name = "Bearbeitet von")]
        public AspNetUsers modifiedBy { get; set; }
        [Display(Name = "Bearbeitet am")]
        public DateTime modified { get; set; }

    }
}