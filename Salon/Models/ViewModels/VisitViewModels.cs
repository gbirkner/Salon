using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models {
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
        [Display(Name = "Behandlungen")]
        public List<VisitTreatment> treatments { get; set; }
        public List<byte[]> images { get; set; }

        public VisitDetailViewModel() {
            images = new List<byte[]>();
        }

    }

    public class VisitCreateViewModel {
        [Key]
        public int? visitId { get; set; }
        [Display(Name = "Datum")]
        public DateTime created { get; set; }
        [Display(Name = "Kunde")]
        public Customers customer { get; set; }
        [Display(Name = "Stylist")]
        public AspNetUsers stylist { get; set; }
        [Display(Name = "mögliche Behandlungen")]
        public List<Treatments> availableTreatments { get; set; }
        [Display(Name = "ausgewählte Behandlungen")]
        public List<VisitTreatment> selectedTreatments { get; set; }
        public List<System.Web.Mvc.ActionResult> treatmentViews { get; set; }
        [Display(Name = "Lehrer")]
        public KeyValuePair<string, string> teacher { get; set; }
        [Display(Name = "Raum")]
        public KeyValuePair<int, string> room { get; set; }
        [Display(Name = "Dauer Approx [min]")]
        public int duration { get; set; }

        public VisitCreateViewModel() {
            this.availableTreatments = new List<Treatments>();
            this.selectedTreatments = new List<VisitTreatment>();
            this.treatmentViews = new List<System.Web.Mvc.ActionResult>();
        }

    }

    public class VisitTreatment {
        [Key]
        public int treatmentID { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Schritte")]
        public List<VisitTasks> tasks { get; set; }
        public List<TreatmentSteps> possibleTasks { get; set; }
        public int ran { get; set; }
        public bool allowSensitive { get; set; }

        public VisitTreatment() {
            this.tasks = new List<VisitTasks>();
            this.possibleTasks = new List<TreatmentSteps>();
            this.ran = new Random().Next();
        }

        public List<TreatmentSteps> getTasksWithoutSensitive() {
            List<TreatmentSteps> steps = new List<TreatmentSteps>();
            foreach(TreatmentSteps s in possibleTasks) {
                if (!s.Steps.isSensitive) {
                    steps.Add(s);
                }
            }
            return steps;
        }

        public List<TreatmentSteps> getSensitiveTasks() {
            List<TreatmentSteps> steps = new List<TreatmentSteps>();
            foreach (TreatmentSteps s in possibleTasks) {
                if (s.Steps.isSensitive) {
                    steps.Add(s);
                }
            }
            return steps;
        }
    }

    public class CustomerPicker {
        [Key]
        public int customerId { get; set; }
        [Display(Name = "Vorname")]
        public string fName { get; set; }
        [Display(Name = "Nachname")]
        public string lName { get; set; }
    }
}