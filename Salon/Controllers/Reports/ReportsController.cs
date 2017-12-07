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
            

            System.IO.File.WriteAllLines("Downloads", this.ListToStrings(), System.Text.Encoding.UTF8);
            return new EmptyResult();
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
            int numberOfProperties = properties.Count();
            int counter = 1;
            foreach(var property in properties)
            {
                var display = (property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);

                if (display != null)
                    headers += display.Name + ";";

                if (counter == numberOfProperties)
                    headers = headers.Remove(headers.Length - 1);
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
    }
}