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

namespace Salon.Controllers.Reports
{
    public class ReportsController : Controller
    {
        private SalonEntities db = new SalonEntities();
        private static List<CustomersViewModel> customerList = new List<CustomersViewModel>();

        // GET: CustomerViewModel
        public ActionResult Customers(string export = "")
        {
            var customer = db.Customers.Include(c => c.Cities);
            IEnumerable<CustomersViewModel> CustomersViewModel =
                (from cu in customer
                 orderby cu.LName
                 select new CustomersViewModel
                 {
                     Country = cu.Cities.CountryId,
                     CustomerId = cu.CustomerId,
                     Description = cu.Description,
                     Name = cu.FName + " " + cu.LName,
                     PostalCode = cu.Cities.PostalCode,
                     Street = cu.Street,
                     City = cu.Cities.Title
                 }
                 );
            customerList = CustomersViewModel.ToList();

            return View(customerList);
        }

        /// <summary>
        /// Exports list to a .csv file
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            //Gets the directory of the logged in windows user
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString() + @"\downloads\";
            }
            System.IO.File.WriteAllLines(path + @"\Kundenliste.csv", this.ListToStrings(), System.Text.Encoding.UTF8);
            return RedirectToAction("Customers");   //redirect to action customer, otherwise site will load up empty        
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

        public ActionResult CustomerStatistics()
        {
            var customerStats = new CustomerStatistics(db.Customers);

            return View(customerStats);
        }

        /// <summary>
        /// Get: WorkPerClassViewModel
        /// </summary>
        /// <param name="cl">class</param>
        /// <returns></returns>
        public ActionResult WorkPerClass(string cl = "")
        {
            var visits = db.Visits;
            //IEnumerable<WorkPerStudentViewModel> WorkPerStudent =
            //    (from v in visits
            //     select new WorkPerStudentViewModel
            //     {
            //         StudentName = "Matthias Feurstein",
            //         Class = "4aINF",
            //         TeacherName = "Gert Birkner",
            //         Treatment = "Nix",
            //         Date = DateTime.Now
            //     }
            //     );

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
                            Date = c.Date
                        }).ToList();

                return View("~/Views/Reports/WorkPerClassResponse.cshtml", WorkPerClass.ToList());
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
                    }
                 );

                return View(empty.ToList());
            }
                
        }
    }
}