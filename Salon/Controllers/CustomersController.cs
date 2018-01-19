using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using Microsoft.AspNet.Identity;

namespace Salon.Controllers
{
    public class CustomersController : Controller
    {
        private SalonEntities db = new SalonEntities();

        public ActionResult Delete (int id)
        {
            db.DeleteCustomerByID(id);

            return RedirectToAction("Index");
        }


        public ActionResult Anonymize(int id)
        {


            return RedirectToAction("Index");
        }







        // GET: Customers
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
                    GenderTitle = c.Genders.GenderTitle,
                    PostalCode = c.Cities.PostalCode,
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
                where c.FName.Contains(searchstring) || c.LName.Contains(searchstring) || c.Cities.PostalCode.Contains(searchstring) || c.Cities.Title.Contains(searchstring) || c.Cities.Countries.Title.Contains(searchstring) || c.Street.Contains(searchstring) || c.Description.Contains(searchstring)
                orderby c.LName
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                    GenderTitle = c.Genders.GenderTitle,
                    PostalCode = c.Cities.PostalCode,
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
                    Description = c.Description,
                    ConnectionId = c.ConnectionId
                }
                ).ToList();

            ViewBag.CustomerId = id;

            return PartialView("_CustomerConnection", ConViewModels);
        }

        public ActionResult _Create(int? id)
        {
            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", id);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "ConnectionId,ConnectionTypeId,CustomerId,Title,Description")] Connections connections)
        {
            if (ModelState.IsValid)
            {
                db.Connections.Add(connections);
                db.SaveChanges();
                //return CustomerConnection(connections.CustomerId);
                return RedirectToAction("Index");
            }

            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title", connections.ConnectionTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", connections.CustomerId);
            return View(connections);
        }

        public ActionResult _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Connections connections = db.Connections.Find(id);
            if (connections == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title", connections.ConnectionTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", connections.CustomerId);
            return View(connections);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Edit([Bind(Include = "ConnectionId,ConnectionTypeId,CustomerId,Title,Description")] Connections connections)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connections).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title", connections.ConnectionTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", connections.CustomerId);
            return View(connections);
        }

        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Connections connections = db.Connections.Find(id);
            if (connections == null)
            {
                return HttpNotFound();
            }
            
            return View(connections);
        }

        // POST: Connections/Delete/5
        [HttpPost, ActionName("_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteConfirmed(int id)
        {
            Connections connections = db.Connections.Find(id);
            db.Connections.Remove(connections);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle");
            return View();
        }

        // POST: Customers/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,FName,LName,Street,isActive,Description,allowImages,allowSensitive,ModifiedBy,Modified,CreatedBy,Created,GenderID,CityId")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                customers.Modified = DateTime.Now;
                customers.ModifiedBy = "0255c5b9-6cad-40a8-a5b7-bd916832519a"; //User.Identity.GetUserId();
                customers.Created = DateTime.Now;
                customers.CreatedBy = "0255c5b9-6cad-40a8-a5b7-bd916832519a"; //User.Identity.GetUserId();
                db.Customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
            return View(customers);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
            return View(customers);
        }

        // POST: Customers/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,FName,LName,Street,isActive,Description,allowImages,allowSensitive,ModifiedBy,Modified,CreatedBy,Created,GenderID,CityId")] Customers customers)
        {
            customers.Modified = DateTime.Now;
            customers.ModifiedBy = "0255c5b9-6cad-40a8-a5b7-bd916832519a"; //User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
            return View(customers);
        }

        //// GET: Customers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customers customers = db.Customers.Find(id);
        //    if (customers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customers);
        //}

        //// POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Customers customers = db.Customers.Find(id);
        //    db.Customers.Remove(customers);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
