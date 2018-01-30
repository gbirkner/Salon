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
using Microsoft.AspNet.Identity;

namespace Salon.Controllers.Statistics
{
    public class StatisticsController : Controller
    {
        //static variable that shows if the file was downloaded
        public static bool Download = false;
        //static variable that shows if the file was downloaded successfully
        public static bool SuccessfullDownload = false;
        //static variable that shows if there was an error during download
        public static string ErrorMessage = string.Empty;
        public static string Path = string.Empty;
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

                if (Environment.OSVersion.Version.Major >= 6) //checks the OSVersion
                {
                    path = Directory.GetParent(path).ToString() + @"\downloads\"; //adds download to the path
                }

                string fullPath = path + @"\" + fileName + ".csv"; //adds filename and extension to the path
                while (System.IO.File.Exists(fullPath)) //checks if a file with that name allready exists
                {
                    fullPath = path + @"\" + fileName + "_" + count + ".csv"; //adds a number to the filename (like a version)
                    Path = fullPath;
                    count++;
                }

                System.IO.File.WriteAllLines(fullPath, exportList, System.Text.Encoding.UTF8); //saves the file
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

        public ActionResult CustomerStatistics(string cust = null, string result=null,  bool download = false, string cities = null, string treatments = null)
        {
            var customerStats = new CustomerStatistics(db);

            // get connections of a customer
            if (cust != null && result == null)
            {
                var connectionData = customerStats.GetConnections(Convert.ToInt32(cust));
                return View("~/Views/Statistics/CustomerConnections.cshtml", connectionData);
            }
            else if (cust != null && result == "getVisit")
            {
                var visitData = customerStats.GetLastVisit(Convert.ToInt32(cust));                                
                return View("~/Views/Statistics/CustomerVisits.cshtml", visitData);
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

                        string newLine = $"{item.FName} {item.LName};{item.city};{string.Join(", ", customerStats.GetLastVisit(item.custID))};{contactData};";
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

                        string newLine = $"{item.FName} {item.LName};{item.city};{string.Join(", ", customerStats.GetLastVisit(item.custID))};{contactData};";
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

                        string newLine = $"{item.FName} {item.LName};{item.city};{string.Join(", ", customerStats.GetLastVisit(item.custID))};{contactData};";
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
        /// Shows the work per class
        /// </summary>
        /// <param name="cl">class</param>
        /// <param name="sort">sort-options</param>
        /// <param name="teacher">teacher</param>
        /// <param name="room">room</param>
        /// <returns></returns>
        public ActionResult WorkPerClass(string cl = "", string sort = "", string teacher = "", string room = "")
        {
            var visits = db.Visits;
            ViewBag.Downloaded = Download;
            ViewBag.Success = SuccessfullDownload;
            ViewBag.ErrorMessage = ErrorMessage;
            ViewBag.Path = Path;
            workPerClassList.Clear();

            if (Download == true)
                Download = false; //reset static variable
            if (SuccessfullDownload == true)
                SuccessfullDownload = false; //reset static variable

            string teacherLast = ""; //teacher lastname
            string teacherFirst = ""; //teacher firstname

            if (cl != "" || teacher != "" || room != "") //if there is something filterd
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

                //Get WorkPerClass from DB with the selected filter-options
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

                switch(sort) //switch on sort-options
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
                return View("~/Views/Statistics/WorkPerClassResponse.cshtml", workPerClassList);
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
        /// Get all steps of a treatment
        /// </summary>
        /// <param name="treatmentId">Id of the treatment</param>
        /// <returns>List of steps</returns>
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
        /// Creates a list of strings from the data for the .csv export
        /// </summary>
        /// <param name="name">Name of the statistic</param>
        /// <returns>Action</returns>
        public ActionResult WorkPerClassExport(string name)
        {
            List<String> returnValue = new List<string>();
            string headers = "Nr;";

            var properties = typeof(WorkPerClassViewModel).GetProperties();
            var propertiesStep = typeof(WorkPerClassViewModel.Step).GetProperties();
            int number = 1;

            if (name == "WorkPerClass") //if name of the statistic is WorkPerClass
            {
                foreach (var property in properties) //creates header of the document
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

                foreach (var entry in workPerClassList)  //loops through all treatments
                {
                    foreach (var step in entry.StepsPerTreatment) //loops through all steps for a treatment
                    {
                        //connects all variables in string seperated by ';'
                        returnValue.Add(number + ";" + entry.StudentName + ";" + entry.Class + ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString() + ";" + entry.Room + ";" + step.StepTitle + ";" + step.StepDescription);
                    }
                    number++;
                }

                var cl = workPerClassList.FirstOrDefault();
                if (cl != null) //if there is a item in exportlist
                {
                    this.Export(returnValue, "Arbeit_" + workPerClassList.First().Class + "_" + DateTime.Now.ToShortDateString().Replace(".", "")); //calls the export method
                }
                return RedirectToAction("WorkPerClass");
            }
            else if (name == "MyWork") //if name of the statistic is MyWork
            {
                foreach (var property in properties) //creates headers of the document
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

                foreach (var entry in myWorkList)  //loops through all treatments
                {
                    foreach (var step in entry.StepsPerTreatment) //loops through all steps for a treatment
                    {
                        //connects all variables in string seperated by ';'
                        returnValue.Add(number + ";" + entry.StudentName + ";" + entry.Class + ";" + entry.TeacherName + ";" + entry.Treatment + ";" + entry.Date.ToShortDateString() + ";" + entry.Room + ";" + step.StepTitle + ";" + step.StepDescription);
                    }
                    number++;
                }

                var cl = myWorkList.FirstOrDefault();
                if (cl != null)//if there is a item in exportlist
                {
                    this.Export(returnValue, "MeineArbeit_" + User.Identity.Name + "_" + DateTime.Now.ToShortDateString().Replace(".", "")); //calls the export method
                }
                return RedirectToAction("MyWork");
            }
            return RedirectToAction("WorkPerClass");
        }

        /// <summary>
        /// Shows the work for the User that is currently logged in
        /// </summary>
        /// <returns></returns>
        public ActionResult MyWork()
        {
            var userId = User.Identity.GetUserId(); //Get id of logged in user
            var visits = db.Visits;
            ViewBag.Downloaded = Download;
            ViewBag.Success = SuccessfullDownload;
            ViewBag.ErrorMessage = ErrorMessage;
            workPerClassList.Clear();

            if (Download == true)
                Download = false; //reset variables
            if (SuccessfullDownload == true)
                SuccessfullDownload = false; //reset variables

            //get data from database
            IEnumerable<WorkPerClassViewModel> MyWork =
                db.GetMyWork(userId)
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