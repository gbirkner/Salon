using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using System.Data.Entity.Infrastructure;

namespace Salon.Controllers
{
    public class GendersController : Controller
    {
        private SalonEntities db = new SalonEntities();

        /// <summary>
        /// Index view Gender
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Genders.ToList());
        }

        /// <summary>
        /// Detail View Gender
        /// </summary>
        /// <param name="id">GenderID</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genders genders = db.Genders.Find(id);
            if (genders == null)
            {
                return HttpNotFound();
            }
            return View(genders);
        }

        /// <summary>
        /// Create View Gender
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create Gender in DB
        /// </summary>
        /// <param name="genders"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenderID,GenderTitle")] Genders genders)
        {
            if (ModelState.IsValid)
            {
                db.Genders.Add(genders);
                try {
                    db.SaveChanges();

                } catch (DbUpdateException ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("GenderID", "Es ist ein Fehler aufgetreten!");
                    return View(genders);
                }
                return RedirectToAction("Index");
            }

            return View(genders);
        }

        /// <summary>
        /// Edit Gender View
        /// </summary>
        /// <param name="id">GenderID</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genders genders = db.Genders.Find(id);
            if (genders == null)
            {
                return HttpNotFound();
            }
            return View(genders);
        }

        /// <summary>
        /// Edit Gender in DB
        /// </summary>
        /// <param name="genders"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GenderID,GenderTitle")] Genders genders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genders).State = EntityState.Modified;
                try {
                    db.SaveChanges();

                } catch (DbUpdateException ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("GenderID", "Es ist ein Fehler aufgetreten!");
                    return View(genders);
                }
                return RedirectToAction("Index");
            }
            return View(genders);
        }

        /// <summary>
        /// Delete View Gender
        /// </summary>
        /// <param name="id">GenderID</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genders genders = db.Genders.Find(id);
            if (genders == null)
            {
                return HttpNotFound();
            }
            return View(genders);
        }

        /// <summary>
        /// Delete Gender in DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Genders genders = db.Genders.Find(id);
            db.Genders.Remove(genders);


            try {
                db.SaveChanges();

            } catch (DbUpdateException ex) {
                var ErrorCode = ex.InnerException.HResult;
                ModelState.AddModelError("GenderID", "Sie können dieses Geschlecht nicht löschen!");
                return View(genders);
            }


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
