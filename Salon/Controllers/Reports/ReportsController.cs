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
        private static List<WorkPerClassViewModel> myWorkList = new List<WorkPerClassViewModel>();
        private SalonEntities db = new SalonEntities();

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
                if (cities != "all")
                {
                    var selectedCustomers = from v in customerStats.Customers
                                            join c in customerStats.Cities on v.CityId equals c.CityId
                                            where c.Title == cities
                                            select new
                                            {
                                                custID = v.CustomerId,
                                                FName = v.FName,
                                                LName = v.LName,
                                                city = c.Title,
                                            };

                    List<String> returnValue = new List<string>();
                    string headers = "Name;" + "Ort;" + "Letzte Behandlung;" + "Kontaktinformation";

                    returnValue.Add(headers);

                    foreach (var item in selectedCustomers)
                    {
                        string contactData = null;
                        List<CustomerConnections> currConnections = customerStats.GetConnections(item.custID);

                        foreach (var currentContact in currConnections)
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

                        if (contactData == null)
                        {
                            contactData = "keine Kontaktinformationen vorhanden";
                        }

                        string newLine = $"{item.FName} {item.LName};{item.city};{customerStats.LastTreatment(item.custID)};{contactData};";
                        returnValue.Add(newLine);
                    }

                    this.Export(returnValue, "Kunden-Auswertung_" + DateTime.Now.ToShortDateString().Replace(".", ""));

                }
                else if (treatments != "all")
                {
                    var selectedCustomers = from v in customerStats.Customers
                                            join c in customerStats.Cities on v.CityId equals c.CityId                                            
                                            select new
                                            {
                                                custID = v.CustomerId,
                                                FName = v.FName,
                                                LName = v.LName,
                                                city = c.Title
                                            };

                    List<String> returnValue = new List<string>();
                    string headers = "Name;" + "Ort;" + "Letzte Behandlung;" + "Kontaktinformation";

                    returnValue.Add(headers);

                    foreach (var item in selectedCustomers)
                    {
                        string contactData = null;

                        foreach (var currentContact in customerStats.GetConnections(item.custID))
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

                        if (contactData == null)
                        {
                            contactData = "keine Kontaktinformationen vorhanden";
                        }

                        string newLine = $"{item.FName}{item.LName};{item.city};{customerStats.LastTreatment(item.custID)};{contactData};";
                        if (customerStats.LastTreatment(item.custID) == treatments)
                        {
                            returnValue.Add(newLine);
                        }

                        // else: ignore the row
                    }

                    this.Export(returnValue, "Kunden-Auswertung_" + DateTime.Now.ToShortDateString().Replace(".", ""));
                }
                else if (cities == "all" && treatments == "all")
                {
                    var selectedCustomers = from v in customerStats.Customers
                                            join c in customerStats.Cities on v.CityId equals c.CityId                                            
                                            select new
                                            {
                                                custID = v.CustomerId,
                                                FName = v.FName,
                                                LName = v.LName,
                                                city = c.Title
                                            };

                    List<String> returnValue = new List<string>();
                    string headers = "Name;" + "Ort;" + "Letzte Behandlung;" + "Kontaktinformation";

                    returnValue.Add(headers);

                    foreach (var item in selectedCustomers)
                    {
                        string contactData = null;

                        foreach (var currentContact in customerStats.GetConnections(item.custID))
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

                        if (contactData == null)
                        {
                            contactData = "keine Kontaktinformationen vorhanden";
                        }

                        string newLine = $"{item.FName}{item.LName};{item.city};{customerStats.LastTreatment(item.custID)};{contactData};";
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
        public ActionResult WorkPerClass(string cl = "", string sort = "", string teacher = "", string room = "")
        {
            var visits = db.Visits;
            ViewBag.Downloaded = Download;
            ViewBag.Success = SuccessfullDownload;
            ViewBag.ErrorMessage = ErrorMessage;
            workPerClassList.Clear();

            if (Download == true)
                Download = false;
            if (SuccessfullDownload == true)
                SuccessfullDownload = false;

            string teacherLast = "";
            string teacherFirst = "";

            if (cl != "" || teacher != "" || room != "")
            {
                if (cl == "Alle")
                    cl = null;
                if (room == "Alle")
                    room = null;
                if (teacher != "Alle")
                {
                    var splitted = teacher.Split(null);
                    teacherLast = splitted[0];
                    teacherFirst = splitted[1];
                }
                else
                {
                    teacherFirst = null;
                    teacherLast = null;
                }

                IEnumerable<WorkPerClassViewModel> WorkPerClass =
                    db.GetWorkPerClass(cl, teacherFirst, teacherLast, room)
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
                    case "Schüler aufsteigend":
                        workPerClassList = WorkPerClass.OrderBy(w => w.StudentName).ThenBy(w => w.Date).ToList();
                        break;
                    case "Schüler absteigend":
                        workPerClassList = WorkPerClass.OrderByDescending(w => w.StudentName).ThenBy(w => w.Date).ToList();
                        break;
                }
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
                    }
                 );
                return View(empty.ToList());
            }   
        }

        /// <summary>
        /// Gets all treatments, assigned to a step
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
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
        public ActionResult WorkPerClassExport(string name)
        {
            List<String> returnValue = new List<string>();
            string headers = "Nr;";

            var properties = typeof(WorkPerClassViewModel).GetProperties();
            var propertiesStep = typeof(WorkPerClassViewModel.Step).GetProperties();
            int number = 1;

            if (name == "WorkPerClass")
            {
                //returnValue.Add("Arbeit pro Klasse");
                //returnValue.Add("Klasse: " + workPerClassList.First().Class);
                //returnValue.Add("");

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
                    foreach (var step in entry.StepsPerTreatment)
                    {
                        returnValue.Add(number + ";" + entry.StudentName + ";" + entry.Class + ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString() + ";" + entry.Room + ";" + step.StepTitle + ";" + step.StepDescription);
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
            else if (name == "MyWork")
            {
                //returnValue.Add("Meine Arbeit");
                //returnValue.Add("Name: " + myWorkList.First().StudentName);
                //returnValue.Add("");

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

                foreach (var entry in myWorkList)  //data
                {
                    foreach (var step in entry.StepsPerTreatment)
                    {
                        returnValue.Add(number + ";" + entry.StudentName + ";" + entry.Class + ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString() + ";" + entry.Room + ";" + step.StepTitle + ";" + step.StepDescription);
                    }
                    number++;
                }

                var cl = myWorkList.FirstOrDefault();
                if (cl != null)
                {
                    this.Export(returnValue, "MeineArbeit_" + User.Identity.Name + "_" + DateTime.Now.ToShortDateString().Replace(".", ""));
                }
                return RedirectToAction("MyWork");
            }
            return RedirectToAction("WorkPerClass");
        }

        /// <summary>
        /// Shows the work for the currently loggedon User
        /// </summary>
        /// <returns></returns>
        public ActionResult MyWork()
        {
            var user = "155e3efb-35be-44f2-b318-daf06cd1bf3a";
            var visits = db.Visits;
            ViewBag.Downloaded = Download;
            ViewBag.Success = SuccessfullDownload;
            ViewBag.ErrorMessage = ErrorMessage;
            workPerClassList.Clear();

            if (Download == true)
                Download = false;
            if (SuccessfullDownload == true)
                SuccessfullDownload = false;

            IEnumerable<WorkPerClassViewModel> MyWork =
                db.GetMyWork(user)
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

            myWorkList = MyWork.ToList();
            return View(MyWork.ToList());
        }
    }
}