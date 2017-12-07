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

namespace Salon.Controllers.Reports
{
    //public class CustomerViewModelController : Controller
    public class ReportsController : Controller
    {
        private SalonEntities db = new SalonEntities();
        private List<CustomersViewModel> customerList = new List<CustomersViewModel>();

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

            this.customerList = CustomersViewModel.ToList();

            if (export == "excel")
            {
                
            }
            //else if (export == "pdf")
            //{
            //    var x = 0;
            //}
            return View(this.customerList);
        }

        public ActionResult Export()
        {
            string path = @"C:\";

            System.IO.File.WriteAllLines("Downloads", this.ListToStrings(), System.Text.Encoding.UTF8);
            return new EmptyResult();
        }


        private List<String> ListToStrings()
        {
            List<String> returnValue = new List<string>();

            returnValue.Add("Name;Beschreibung;Straße;PLZ;Stadt;Land");
            foreach(var entry in this.customerList)
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