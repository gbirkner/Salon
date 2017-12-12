using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Salon.Views.ViewModels
{
    public class WorkPerStudentViewModel
    {
        [Display(Name = "Schüler")]
        public string StudentName { get; set; }

        [Display(Name = "Klasse")]
        public string Class { get; set; }

        [Display(Name = "Lehrer")]
        public string TeacherName { get; set; }

        [Display(Name = "Arbeit")]
        public string Treatment { get; set; }

        [Display(Name = "Datum")]
        public DateTime Date { get; set; }
    }


    public class ClassList
    {
        public string Class { get; set; }
    }
}