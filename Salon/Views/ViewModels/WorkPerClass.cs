using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Salon.Views.ViewModels
{
    public class WorkPerClassViewModel
    {
        public WorkPerClassViewModel()
        {
            StudentName = "";
            Class = "";
            TeacherName = "";
            Treatment = "";
            Date = new DateTime();
        }

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

        public List<string> GetClasses()
        {
            List<string> returnValue = new List<string>();

            using (var context = new Models.SalonEntities())
            {
                var classes = context.GetClasses();

                foreach(var cl in classes)
                {
                    returnValue.Add(cl.Class);
                }
            }
            return returnValue;
        }
    }
}