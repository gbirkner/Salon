using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using Salon.Views.ViewModels;
using System.Data.Entity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Salon.Models.Statistics;
using System.Text.RegularExpressions;

namespace Salon.Controllers.Reports
{
    public class ReportsController : Controller
    {
        private SalonEntities db = new SalonEntities();
        private static List<CustomersViewModel> customerList = new List<CustomersViewModel>();
        private static List<WorkPerClassViewModel> workPerClassList = new List<WorkPerClassViewModel>();

        // GET: CustomerViewModel
        //public ActionResult Customers(string export = "")
        //{
        //    var customer = db.Customers.Include(c => c.Cities);
        //    IEnumerable<CustomersViewModel> CustomersViewModel =
        //        (from cu in customer
        //         orderby cu.LName
        //         select new CustomersViewModel
        //         {
        //             Country = cu.Cities.CountryId,
        //             CustomerId = cu.CustomerId,
        //             Description = cu.Description,
        //             Name = cu.FName + " " + cu.LName,
        //             PostalCode = cu.Cities.PostalCode,
        //             Street = cu.Street,
        //             City = cu.Cities.Title
        //         }
        //         );
        //    customerList = CustomersViewModel.ToList();

        //    return View(customerList);
        //}

        /// <summary>
        /// Exports list to a .csv file
        /// </summary>
        /// <returns></returns>
        public void Export(List<string> exportList, string fileName)
        {
            try {
                //Gets the directory of the logged in windows user
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;

                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).ToString() + @"\downloads\";
                }
                System.IO.File.WriteAllLines(path + @"\" + fileName + ".csv", exportList, System.Text.Encoding.UTF8);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Convert list<object> to list<string> with ';' as seperator for .csv
        /// </summary>
        /// <returns></returns>
        private List<String> ListToStrings()
        {
            List<String> returnValue = new List<string>();
            string headers = string.Empty;

            var properties = typeof(CustomersViewModel).GetProperties();
            foreach(var property in properties) //headers
            {
                var display = (property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);

                if (display != null)
                    headers += display.Name + ";";
            }

            returnValue.Add(headers);
            foreach(var entry in customerList)  //data
            {
                returnValue.Add(entry.Name + ";" + entry.Description + ";" + entry.Street + ";" + entry.PostalCode + ";" + entry.City + ";" + entry.Country);
            }
            return returnValue;
        }

        /// <summary>
        /// Dispose overload
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult CustomerStatistics(string cust = null)
        {
            var customerStats = new CustomerStatistics(db);

            if (cust != null)
            {
                var connectionData = customerStats.GetConnections(Convert.ToInt32(cust));
                return View("~/Views/Reports/CustomerConnections.cshtml", connectionData);
            }
            else
            {                
                return View(customerStats);
            }            
        }

        /// <summary>
        /// Get: WorkPerClassViewModel
        /// </summary>
        /// <param name="cl">class</param>
        /// <returns></returns>
        public ActionResult WorkPerClass(string cl = "", string sort = "")
        {
            var visits = db.Visits;

            if (cl != "")
            {
                IEnumerable<WorkPerClassViewModel> WorkPerClass =
                    db.GetWorkPerClass(cl)
                        .Select(c => new WorkPerClassViewModel()
                        {
                            Class = c.Class,
                            StudentName = c.StudentName,
                            TeacherName = c.TeacherName,
                            Treatment = c.Treatement,
                            Date = c.Date ?? Convert.ToDateTime(c.Date),
                            StepsPerTreatment = this.GetStepsPerTreatment(c.TreatmentId)
                        }).ToList();

                switch(sort)
                {
                    case "Datum aufsteigend":
                        workPerClassList = WorkPerClass.OrderBy(w => w.Date).ThenBy(w => w.StudentName).ToList();
                        break;
                    case "Datum absteigend":
                        workPerClassList = WorkPerClass.OrderByDescending(w => w.Date).ThenBy(w => w.StudentName).ToList();
                        break;
                    case "Name aufsteigend":
                        workPerClassList = WorkPerClass.OrderBy(w => w.StudentName).ThenBy(w => w.Date).ToList();
                        break;
                    case "Name absteigend":
                        workPerClassList = WorkPerClass.OrderByDescending(w => w.StudentName).ThenBy(w => w.Date).ToList();
                        break;
                }

                //workPerClassList = WorkPerClass.OrderByDescending(w => w.Date).ToList();
                return View("~/Views/Reports/WorkPerClassResponse.cshtml", workPerClassList);
            }
            else
            {
                IEnumerable<WorkPerClassViewModel> empty =
                    (
                    from cu in db.Customers
                    orderby cu.LName
                    select new WorkPerClassViewModel
                    {
                        Class = "",
                        StudentName = "",
                        TeacherName = "",
                        Treatment = "",
                        Date = DateTime.Now
                        //,StepsPerTreatment = this.GetStepsPerTreatment(1)
                    }
                 );
                return View(empty.ToList());
            }   
        }

        private List<WorkPerClassViewModel.Step> GetStepsPerTreatment(int treatmentId)
        {
            List<WorkPerClassViewModel.Step> returnValue = new List<WorkPerClassViewModel.Step>();

            returnValue =
                  db.GetStepsPerTreatment(treatmentId)
                  .Select(s => new WorkPerClassViewModel.Step()
                  {
                      StepDescription = s.StepDescription,
                      StepTitle = s.Step
                  }).ToList();

            return returnValue;

        }

        /// <summary>
        /// Table to stringlist for .csv export
        /// </summary>
        /// <returns>Action</returns>
        public ActionResult WorkPerClassExport()
        {
            List<String> returnValue = new List<string>();
            string headers = string.Empty;

            var properties = typeof(WorkPerClassViewModel).GetProperties();
            var propertiesStep = typeof(WorkPerClassViewModel.Step).GetProperties();
            foreach (var property in properties) //headers
            {
                var display = (property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);
                
                if (display != null)
                    headers += display.Name + ";";
            }
            foreach (var property in propertiesStep) //headers
            {
                var display = (property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);

                if (display != null)
                    headers += display.Name + ";";
            }

            returnValue.Add(headers);
            foreach (var entry in workPerClassList)  //data
            {
                returnValue.Add(entry.StudentName + ";" + entry.Class+ ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString());

                foreach(var step in entry.StepsPerTreatment)
                {
                    returnValue.Add(";;;;;" + step.StepTitle + ";" + step.StepDescription);
                }
            }

            var cl = workPerClassList.FirstOrDefault();
            if (cl != null)
            {
                this.Export(returnValue, "Arbeit_" + workPerClassList.First().Class + "_" + DateTime.Now.ToShortDateString().Replace(".", ""));
            }
            return RedirectToAction("WorkPerClass");
        }
    }
}