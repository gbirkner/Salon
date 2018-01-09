using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Salon.Views.ViewModels
{
    public class WorkPerClassViewModel
    {
        public string SelectedTeacher { get; set; }

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

        [Display(Name = "Behandlung")]
        public string Treatment { get; set; }

        [Display(Name = "Datum")]
        public DateTime Date { get; set; }

        [Display(Name = "Raum")]
        public string Room { get; set; }

        public List<Step> StepsPerTreatment { get; set; }

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

        public SelectList GetTeachers()
        {
            Dictionary<string, string> teachersDict = new Dictionary<string, string>();

            using (var context = new Models.SalonEntities())
            {
                var teachers = context.GetTeachers();

                foreach (var t in teachers)
                {
                    teachersDict.Add(t.TeacherId, t.Teacher);
                }
            }
            return new SelectList(teachersDict.Select(x => new { Value = x.Key, Text = x.Value }), "Value", "Text");
        }

        //public List<Teacher> GetTeachers()
        //{
        //    List<Teacher> returnValue = new List<Teacher>();

        //    using (var context = new Models.SalonEntities())
        //    {
        //        var teachers = context.GetTeachers();

        //        foreach (var t in teachers)
        //        {
        //            returnValue.Add(new Teacher() { TeacherId = t.TeacherId, TeacherName = t.Teacher });
        //        }
        //    }
        //    return returnValue;
        //}

        public class Step
        {
            [Display(Name = "Schritt")]
            public string StepTitle { get; set; }

            [Display(Name = "Schrittbeschreibung")]
            public string StepDescription { get; set; }
        }

        public class Teacher
        {
            public string TeacherId { get; set; }
            public string TeacherName { get; set; }
        }
    }
}