using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using Salon.Views.ViewModels;
using System.Data.Entity;
using System.IO;

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
            if (export == "")
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
                return View(this.customerList);
            }
            if (export == "excel")
            {
                //System.IO.File.WriteAllLines(@"C:\tmp\Salon\test.csv", this.customerList.ToArray());

                var x = 0;
                return View();
            }
            else if (export == "pdf")
            {
                var x = 0;
                return View();
            }
            return View();
        }

        public ActionResult CustomersDownload()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/Images/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return View(items);
        }

        [ActionName("ExportExcel")]
        public ActionResult ExportExcel()
        {
            var x = 0;

            return new EmptyResult();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}