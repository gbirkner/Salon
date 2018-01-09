using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Salon.Models;
using Salon.Views.ViewModels;
using System.IO;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Salon.Models.Statistics;

namespace Salon.Controllers.Reports
{
    public class ReportsController : Controller
    {
        public static bool Download = false;
        public static bool SuccessfullDownload = false;
        public static string ErrorMessage = string.Empty;
        private static List<CustomersViewModel> customerList = new List<CustomersViewModel>();
        private static List<WorkPerClassViewModel> workPerClassList = new List<WorkPerClassViewModel>();
        private SalonEntities db = new SalonEntities();

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
            int count = 1;
            Download = true;
            try {
                //Gets the directory of the logged in windows user
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;

                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).ToString() + @"\downloads\";
                }

                string fullPath = path + @"\" + fileName + ".csv";
                while (System.IO.File.Exists(fullPath))
                {
                    fullPath = path + @"\" + fileName + "_" + count + ".csv";
                    count++;
                }

                System.IO.File.WriteAllLines(fullPath, exportList, System.Text.Encoding.UTF8);
                SuccessfullDownload = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                SuccessfullDownload = false;
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

        public ActionResult CustomerStatistics(string cust = null, bool download = false, string cities = null, string treatments = null)
        {
            var customerStats = new CustomerStatistics(db);

            // get connections of a customer
            if (cust != null)
            {
                var connectionData = customerStats.GetConnections(Convert.ToInt32(cust));
                return View("~/Views/Reports/CustomerConnections.cshtml", connectionData);
            }
            // download a exported csv with filters
            else if (download)
            {                                
                if(cities != "all")
                {
                    var selectedCustomers = from v in customerStats.Customers
                                            join c in customerStats.Cities on v.CityId equals c.CityId
                                            where c.Title == cities
                                            select new
                                            {
                                                name = $"{v.FName} {v.LName}",
                                                city = c.Title,
                                                lastTreatment = customerStats.LastTreatment(v.CustomerId),
                                                contact = customerStats.GetConnections(v.CustomerId)
                                            };

                    List<String> returnValue = new List<string>();
                    string headers = "Name;" + "Ort;" + "Letzte Behandlung;" + "Kontaktinformation";

                    returnValue.Add(headers);

                    foreach (var item in selectedCustomers)
                    {
                        string contactData = null;

                        foreach (var currentContact in item.contact)
                        {
                            if(currentContact.ConnectionType == "Telefon" && currentContact.ConnectionValue != null)
                            {
                                contactData = currentContact.ConnectionValue;
                                break;
                            }
                            else if(currentContact.ConnectionType == "E-Mail" && currentContact.ConnectionValue != null)
                            {
                                contactData = currentContact.ConnectionValue;
                            }
                        }

                        if(contactData is null)
                        {
                            contactData = "keine Kontaktinformationen vorhanden";
                        }

                        string newLine = $"{item.name};{item.city};{item.lastTreatment};{contactData};";
                        returnValue.Add(newLine);
                    }

                    this.Export(returnValue, "Kunden-Auswertung_" + DateTime.Now.ToShortDateString().Replace(".", ""));

                }
                else if (treatments != "all")
                {
                    var selectedCustomers = from v in customerStats.Customers
                                            join c in customerStats.Cities on v.CityId equals c.CityId
                                            where customerStats.LastTreatment(v.CustomerId) == treatments
                                            select new
                                            {
                                                name = $"{v.FName} {v.LName}",
                                                city = c.Title,
                                                lastTreatment = customerStats.LastTreatment(v.CustomerId),
                                                contact = customerStats.GetConnections(v.CustomerId)
                                            };

                    List<String> returnValue = new List<string>();
                    string headers = "Name;" + "Ort;" + "Letzte Behandlung;" + "Kontaktinformation";

                    returnValue.Add(headers);

                    foreach (var item in selectedCustomers)
                    {
                        string contactData = null;

                        foreach (var currentContact in item.contact)
                        {
                            if (currentContact.ConnectionType == "Telefon" && currentContact.ConnectionValue != null)
                            {
                                contactData = currentContact.ConnectionValue;
                                break;
                            }
                            else if (currentContact.ConnectionType == "E-Mail" && currentContact.ConnectionValue != null)
                            {
                                contactData = currentContact.ConnectionValue;
                            }
                        }

                        if (contactData is null)
                        {
                            contactData = "keine Kontaktinformationen vorhanden";
                        }

                        string newLine = $"{item.name};{item.city};{item.lastTreatment};{contactData};";
                        returnValue.Add(newLine);
                    }

                    this.Export(returnValue, "Kunden-Auswertung_" + DateTime.Now.ToShortDateString().Replace(".", ""));
                }

                return RedirectToAction("CustomerStatistics");
            }
            // return the standard view
            else
            {                
                return View(customerStats);
            }            
        }

        public ActionResult CustomerStatisticsExport()
        {
            return null;
        }

        /// <summary>
        /// Get: WorkPerClassViewModel
        /// </summary>
        /// <param name="cl">class</param>
        /// <returns></returns>
        public ActionResult WorkPerClass(string cl = "", string sort = "")
        {
            var visits = db.Visits;
            ViewBag.Downloaded = Download;
            ViewBag.Success = SuccessfullDownload;
            ViewBag.ErrorMessage = ErrorMessage;
            workPerClassList.Clear();

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
                            StepsPerTreatment = this.GetStepsPerTreatment(c.TreatmentId),
                            Room = c.Room
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
                        Date = DateTime.Now,
                        Room = ""
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
            string headers = "Nr;";

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

            int number = 1;
            foreach (var entry in workPerClassList)  //data
            {
                //returnValue.Add(entry.StudentName + ";" + entry.Class+ ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString());

                foreach(var step in entry.StepsPerTreatment)
                {
                    returnValue.Add(number + ";" + entry.StudentName + ";" + entry.Class + ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString() + ";" + step.StepTitle + ";" + step.StepDescription);
                    //returnValue.Add(";;;;;" + step.StepTitle + ";" + step.StepDescription);
                }
                number++;
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