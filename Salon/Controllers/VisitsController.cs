using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Salon.Models;

namespace Salon.Controllers
{
    public class VisitsController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: Visits
        public ActionResult Index()
        {
            var visits = db.Visits.Include(v => v.AspNetUsers).Include(v => v.AspNetUsers1).Include(v => v.Customers);
            IEnumerable<VisitShortViewModel> shortVisitViewModels = (from v in visits
                                                                     orderby v.Created
                                                                     select new VisitShortViewModel {
                                                                         visitId = v.VisitId,
                                                                         created = v.Created,
                                                                         customer = v.Customers,
                                                                         stylist = v.AspNetUsers1
                                                                     }).ToList();
            return View(shortVisitViewModels);
        }

        public ActionResult VisitShort() {
            var visits = db.Visits.Include(v => v.AspNetUsers).Include(v => v.AspNetUsers1).Include(v => v.Customers);
            IEnumerable<VisitShortViewModel> shortVisitViewModels = (from v in visits
                                                                     orderby v.Created
                                                                     select new VisitShortViewModel {
                                                                         visitId = v.VisitId,
                                                                         created = v.Created,
                                                                         customer = v.Customers,
                                                                         stylist = v.AspNetUsers1
                                                                     }).ToList();
            return View(shortVisitViewModels);
        }

        public ActionResult VisitDetails(int? id) {
            var visits = db.Visits.Find(id);
            return PartialView(visits);
        }

        // GET: Visits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits visits = db.Visits.Find(id);
            if (visits == null)
            {
                return HttpNotFound();
            }
            return View(visits);
        }

        // GET: Visits/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisitId,Duration,CustomerId,ModifiedBy,Modified,CreatedBy,Created")] Visits visits)
        {
            if (ModelState.IsValid)
            {
                db.Visits.Add(visits);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.ModifiedBy);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId", visits.CustomerId);
            return View(visits);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits visits = db.Visits.Find(id);
            if (visits == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.ModifiedBy);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId", visits.CustomerId);
            return View(visits);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisitId,Duration,CustomerId,ModifiedBy,Modified,CreatedBy,Created")] Visits visits)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visits).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.ModifiedBy);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId", visits.CustomerId);
            return View(visits);
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits visits = db.Visits.Find(id);
            if (visits == null)
            {
                return HttpNotFound();
            }
            return View(visits);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Visits visits = db.Visits.Find(id);
            db.Visits.Remove(visits);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
