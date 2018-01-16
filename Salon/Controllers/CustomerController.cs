using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using System.Data.Entity;
using System.Net;

namespace Salon.Controllers
{
    public class CustomerController : Controller
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
                    GenderID = c.GenderID,
                    //PostalCode = c.PostalCode,
                    CityName = c.Cities.Title,
                    Country = c.Cities.Countries.Title,
                    Street = c.Street,
                    Description = c.Description
                }
                ).ToList();

            return View(CustomerViewModels);
        }


        public ActionResult CustomerOverview(string searchstring = null)
        {
            var cust = db.Customers.Include(p => p.Cities);
            IEnumerable<CustomerViewModel> CustomerViewModels = (
                from c in cust
                //where c.FName.Contains(searchstring) || c.LName.Contains(searchstring) || c.PostalCode.Contains(searchstring) || c.Cities.Title.Contains(searchstring) || c.Cities.Countries.Title.Contains(searchstring) || c.Street.Contains(searchstring) || c.Description.Contains(searchstring)
                orderby c.LName
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                    GenderID = c.GenderID,
                    //PostalCode = c.PostalCode,
                    CityName = c.Cities.Title,
                    Country = c.Cities.Countries.Title,
                    Street = c.Street,
                    Description = c.Description
                }
                ).ToList();

            return PartialView("_CustomerOverview", CustomerViewModels);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Customers Customers = db.Customers.Find(id);
            if (Customers == null)
            {
                return HttpNotFound();
            }
            return View(Customers);
        }



        public ActionResult VisitShort(int? id = null)
        {
            IEnumerable<VisitShortViewModel> Visits = (from v in db.Visits
                                                       where v.CustomerId == id
                                                       orderby v.Created
                                                       select new VisitShortViewModel
                                                       {
                                                           visitId = v.VisitId,
                                                           created = v.Created,
                                                           customer = v.Customers,
                                                           stylist = v.AspNetUsers1
                                                       }).ToList();

            return PartialView("_VisitShort", Visits);
        }

        public ActionResult CustomerConnection(int? id = null)
        {
            var con = db.Connections.Include(c => c.ConnectionTypes);
            IEnumerable<ConnectionViewModel> ConViewModels = (
                from c in con
                where c.CustomerId == id
                orderby c.Title
                select new ConnectionViewModel
                {
                    Title = c.ConnectionTypes.Title,
                    Value = c.Title
                }
                ).ToList();

            return PartialView("_CustomerConnection", ConViewModels);
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
