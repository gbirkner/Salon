using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    public class TreatmentsVM
    {
        [Key]
        public int TreatmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
    }
    public class StepsVM
    {
        [Key]
        public int StepsId { get; set; }
        public int TreatmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isSensitive { get; set; }
        public bool isActive{ get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }

    }
    public class OptionsVM
    {
        [Key]
        public int StepOptionId { get; set; }
        public int StepId { get; set; }
        public int Position{ get; set; }
        public string Option { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
    }
}