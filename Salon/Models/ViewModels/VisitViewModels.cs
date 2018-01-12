using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    public class VisitShortViewModel
    {
        [Key]
        public int visitId { get; set; }
        [Display(Name = "Datum")]
        public DateTime created { get; set; }
        [Display(Name = "Kunde")]
        public Customers customer { get; set; }
        [Display(Name = "Stylist")]
        public AspNetUsers stylist { get; set; }

    }

    public class VisitDetailViewModel
    {
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
        [Display(Name = "Behandlungen")]
        public List<VisitTreatment> treatments { get; set; }

    }

    public class VisitCreateViewModel
    {
        [Key]
        public int visitId { get; set; }
        [Display(Name = "Datum")]
        public DateTime created { get; set; }
        [Display(Name = "Kunde")]
        public Customers customer { get; set; }
        [Display(Name = "Stylist")]
        public AspNetUsers stylist { get; set; }
        [Display(Name = "mögliche Behandlungen")]
        public List<Treatments> availableTreatments { get; set; }
        [Display(Name = "ausgewählte Behandlungen")]
        public List<Treatments> selectedTreatments { get; set; }

    }

    public class VisitTreatment
    {
        [Key]
        public int treatmentID { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Schritte")]
        public List<VisitTasks> tasks { get; set; }

        public VisitTreatment()
        {
            this.tasks = new List<VisitTasks>();
        }
    }
}