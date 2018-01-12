using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;

namespace Salon.Controllers
{
    public class CustomerController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: CustomerVisit
        public ActionResult Index()
        {
            var cust = db.Customers.Include(p => p.Cities).Include(k => k.Cities.Countries);
            IEnumerable<CustomerViewModel> CustomerViewModels = (
                from c in cust
                orderby c.LName
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                    GenderID = c.GenderID,
                    PostalCode = c.PostalCode,
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
            var cust = db.Customers.Include(p => p.Cities).Include(g => g.Genders).Include(k => k.Cities.Countries);
            IEnumerable<CustomerViewModel> CustomerViewModels = (
                from c in cust
                where c.FName.Contains(searchstring) || c.LName.Contains(searchstring) || c.PostalCode.Contains(searchstring) || c.Cities.Title.Contains(searchstring) || c.Cities.Countries.Title.Contains(searchstring) || c.Street.Contains(searchstring) || c.Description.Contains(searchstring)
                orderby c.LName
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                    GenderID = c.GenderID,
                    PostalCode = c.PostalCode,
                    CityName = c.Cities.Title,
                    Country = c.Cities.Countries.Title,
                    Street = c.Street,
                    Description = c.Description
                }
                ).ToList();

            return PartialView("_CustomerOverview", CustomerViewModels);
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
                    Value = c.Title,
                    Description = c.Description
                }
                ).ToList();

            return PartialView("_CustomerConnection", ConViewModels);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers cust = db.Customers.Find(id);

            if (cust == null)
            {
                return HttpNotFound();
            }

            CustomerViewModel Customer = new CustomerViewModel
            {
                CustomerId = cust.CustomerId,
                FName = cust.FName,
                LName = cust.LName,
                GenderID = cust.GenderID,
                // ALT GenderTitle = "Test",
                CityId = cust.CityId,
                Street = cust.Street,
                Description = cust.Description,
                allowSensitive = cust.allowSensitive,
                allowImages = cust.allowImages,
                isActive = cust.isActive,
                CreatedBy = cust.CreatedBy,
                Created = cust.Created
            };

            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", cust.GenderID);
            ViewBag.CityID = new SelectList(db.Cities, "CityId", "Title", cust.CityId);

            return View(Customer);
         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "CustomerId, FName, LName, GenderID, PostalCode, CountryId, Street, Description, allowSensitive, allowImages, isActive")]
        public ActionResult Edit(Customers Customer)
        {
            //string CountryID = db.Cities.First(x => x.PostalCode == cvm.PostalCode).CountryId;

            //Customers Customer = new Customers
            //{
            //    CustomerId = cvm.CustomerId,
            //    FName = cvm.FName,
            //    LName = cvm.LName,
            //    GenderID = cvm.GenderID,
            //    CityId = cvm.CityId,
            //    Street = cvm.Street,
            //    Description = cvm.Description,
            //    allowSensitive = cvm.allowSensitive,
            //    allowImages = cvm.allowImages,
            //    isActive = cvm.isActive,
            //    Modified = DateTime.Now,
            //    ModifiedBy = User.Identity.GetUserId(),
            //    Created = cvm.Created,
            //    CreatedBy = cvm.CreatedBy
            //};

            if (ModelState.IsValid)
            {
                db.Entry(Customer).State = EntityState.Modified;
                db.Entry(Customer).Property(a => a.Created).IsModified = false;
                db.Entry(Customer).Property(a => a.CreatedBy).IsModified = false;

                bool SaveFailed = false;
                do
                {
                    SaveFailed = false;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        SaveFailed = true;

                        //Update original values from the database
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }
                } while (SaveFailed);

                return RedirectToAction("Index");
            }

            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", Customer.GenderID);
            ViewBag.CityID = new SelectList(db.Cities, "CityId", "Title", Customer.CityId);
            return View(Customer);
        }




        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.Gender = new SelectList(db.Genders, "GenderID", "GenderTitle");
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title");
            return View();
        }

        // POST: Customer/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId, FName, LName, GenderID, PostalCode, Street, Description, allowSensitive, allowImages, isActive, CityId")] CustomerViewModel cvm)
        {
            //string CountryID = db.Cities.First(x => x.PostalCode == cvm.PostalCode).CountryId;

            Customers Customer = new Customers
            {
                FName = cvm.FName,
                LName = cvm.LName,
                GenderID = cvm.GenderID,
                CityId = cvm.CityId,
                Street = cvm.Street,
                Description = cvm.Description,
                allowSensitive = cvm.allowSensitive,
                allowImages = cvm.allowImages,
                isActive = cvm.isActive,
                CreatedBy  = User.Identity.GetUserId(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                ModifiedBy = User.Identity.GetUserId()
            };


            if (ModelState.IsValid)
            {
                db.Customers.Add(Customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gender = new SelectList(db.Genders, "GenderID", "GenderTitle", cvm.GenderID);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", cvm.PostalCode);

            return View(Customer);
        }


        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customers cust = db.Customers.Find(id);

            CustomerViewModel Customer = new CustomerViewModel
            {
                CustomerId = cust.CustomerId,
                FName = cust.FName,
                LName = cust.LName,
                GenderTitle = cust.Genders.GenderTitle,
                PostalCode = cust.Cities.PostalCode,
                CityName = cust.Cities.Title,
                Country = cust.Cities.Countries.Title,
                Street = cust.Street,
                Description = cust.Description,
                isActive = cust.isActive,
                allowImages = cust.allowImages,
                allowSensitive = cust.allowSensitive
            };

            if (cust == null)
            {
                return HttpNotFound();
            }
            return View(Customer);
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
