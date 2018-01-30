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
    public class CitiesController : Controller
    {
        private SalonEntities db = new SalonEntities();

        /// <summary>
        /// Index View Cities
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var cities = db.Cities.Include(c => c.Countries);
            return View(cities.ToList());
        }

        /// <summary>
        /// Detail View Cities
        /// </summary>
        /// <param name="id">CityId</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cities cities = db.Cities.Find(id);
            if (cities == null)
            {
                return HttpNotFound();
            }
            return View(cities);
        }

        /// <summary>
        /// Create View City
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Title");
            return View();
        }

        /// <summary>
        /// Create City in DB
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityId,CountryId,PostalCode,Title")] Cities cities)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(cities);

                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("CityId", "Es ist ein Fehler aufgetreten!");
                    return View(cities);
                }
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Title", cities.CountryId);
            return View(cities);
        }

        /// <summary>
        /// Overview for Search
        /// </summary>
        /// <param name="searchstring">Searchstring</param>
        /// <returns></returns>
        public ActionResult CitiesOverview(string searchstring = null)
        {
            var city = db.Cities.Include(p => p.Countries);
            IEnumerable<CitiesViewModel> CitiesVM = (
                from c in city
                where c.PostalCode.Contains(searchstring) || c.Title.Contains(searchstring) || c.Countries.Title.Contains(searchstring)
                orderby c.Title
                select new CitiesViewModel
                {
                    CityId = c.CityId,
                    Title = c.Title,
                    PostalCode = c.PostalCode,
                    Country = c.Countries.Title
                }
                ).ToList();

            return PartialView("_CitiesOverview", CitiesVM);
        }

        /// <summary>
        /// Edit View City
        /// </summary>
        /// <param name="id">CityId</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cities cities = db.Cities.Find(id);
            if (cities == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Title", cities.CountryId);
            return View(cities);
        }

        /// <summary>
        /// Edit City in DB
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityId,CountryId,PostalCode,Title")] Cities cities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cities).State = EntityState.Modified;

                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("CityId", "Es ist ein Fehler aufgetreten!");
                    return View(cities);
                }
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Title", cities.CountryId);
            return View(cities);
        }

        /// <summary>
        /// Delete City View
        /// </summary>
        /// <param name="id">CityId</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cities cities = db.Cities.Find(id);
            
            if (cities == null)
            {
                return HttpNotFound();
            }
            return View(cities);           
            }

        /// <summary>
        /// Delete City in DB
        /// </summary>
        /// <param name="id">CityId</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cities cities = db.Cities.Find(id);
            db.Cities.Remove(cities);

            try {
                db.SaveChanges();

            } catch (DbUpdateException ex) {
                var ErrorCode = ex.InnerException.HResult;
                //FK for City -> can not be deleted
                ModelState.AddModelError("CityId", "Sie können diese Stadt nicht löschen!");
                return View(cities);
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
