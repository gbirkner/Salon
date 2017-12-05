using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using System.Data.Entity;

namespace Salon.Controllers
{
    public class CustomerVisitController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: CustomerVisit
        public ActionResult Index()
        {
            var cust = db.Customers.Include(p => p.Cities);
            IEnumerable<CustomerViewModel> CustomerViewModels = (
                from c in cust
                orderby c.LName
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                    CountryId = c.CountryId,
                    PostalCode = c.PostalCode,
                    CityName = c.Cities.Title,
                    Country = c.Cities.Countries.Title,
                    Street = c.Street
                }
                ).ToList();

            return View(CustomerViewModels);
        }



        public ActionResult CustomerVisitShort(int? id = null)
        {
            IEnumerable<VisitViewModel> Visits = (from v in db.Visits
                                                  where v.CustomerId == id
                                                  orderby v.Created
                                                  select new VisitViewModel
                                                  {
                                                      VisitId = v.VisitId,
                                                      Created = v.Created,
                                                      Duration = v.Duration
                                                  }).ToList();

            return PartialView("_CustomerVisitShort", Visits);
        }


        public ActionResult CustomerMasterdata(int? id = null)
        {
            IEnumerable<CustomerViewModel> CustomerViewModels = (
                from c in db.Customers
                where c.CustomerId == id
                orderby c.LName
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                }
                ).ToList();

            return PartialView("_CustomerMasterdata", CustomerViewModels);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }

    }
}