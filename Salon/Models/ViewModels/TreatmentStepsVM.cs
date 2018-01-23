using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    public class StepsVM
    {
        [Key]
        public int StepsId { get; set; }
        public int TreatmentId { get; set; }
        [Display(Name = "Titel")]
        public string Title { get; set; }
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
        [Display(Name = "Sensibel")]
        public bool isSensitive { get; set; }
        [Display(Name = "Aktiv")]
        public bool isActive{ get; set; }
        [Display(Name = "Dauer in Min")]
        public int Duration { get; set; }
        [Display(Name = "Reihenfolge")]
        public int Order { get; set; }
    }
}