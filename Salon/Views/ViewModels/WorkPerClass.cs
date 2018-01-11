using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

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

        [Display(Name = "Behandlung")]
        public string Treatment { get; set; }

        [Display(Name = "Datum")]
        public DateTime Date { get; set; }

        [Display(Name = "Raum")]
        public string Room { get; set; }

        public List<Step> StepsPerTreatment { get; set; }

        /// <summary>
        /// Get all classes
        /// </summary>
        /// <returns></returns>
        public List<string> GetClasses()
        {
            List<string> returnValue = new List<string>();
            returnValue.Add("Alle");

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

        /// <summary>
        /// Get all teachers
        /// </summary>
        /// <returns></returns>
        public SelectList GetTeachers()
        {
            List<string> teacherList = new List<string>();
            teacherList.Add("Alle");

            using (var context = new Models.SalonEntities())
            {
                var teachers = context.GetTeachers();

                foreach (var t in teachers)
                {
                    teacherList.Add(t.Teacher);
                }
            }
            return new SelectList(teacherList);
        }

        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <returns></returns>
        public SelectList GetRooms()
        {
            List<string> roomList = new List<string>();
            roomList.Add("Alle");

            using (var context = new Models.SalonEntities())
            {
                var rooms = context.Rooms.Select(r => r.Title);

                roomList.AddRange(rooms.ToList());
            }
            return new SelectList(roomList);
        }

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