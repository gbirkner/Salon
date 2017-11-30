using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using Salon.Views.ViewModels;
using System.Data.Entity;

namespace Salon.Controllers.Reports
{
    //public class CustomerViewModelController : Controller
    public class ReportsController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: CustomerViewModel
        public ActionResult Customers()
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

            return View(CustomersViewModel.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}