using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using System.Web.UI;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Salon.Controllers
{
    public class CountriesController : Controller
    {
        private SalonEntities db = new SalonEntities();

        /// <summary>
        /// Index View Country
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Countries.ToList());
        }

        /// <summary>
        /// Detail View Country
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Countries countries = db.Countries.Find(id);
            if (countries == null)
            {
                return HttpNotFound();
            }
            return View(countries);
        }

        /// <summary>
        /// Create View Countr
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Overview Search
        /// </summary>
        /// <param name="searchstring">Searchstring</param>
        /// <returns></returns>
        public ActionResult CountriesOverview(string searchstring = null)
        {
            var country = db.Countries;
            IEnumerable<CountriesViewModel> CountriesVM = (
                from c in country
                where c.CountryId.Contains(searchstring) || c.Title.Contains(searchstring)
                orderby c.CountryId
                select new CountriesViewModel
                {
                    CountryId = c.CountryId,
                    Title = c.Title,
                }
                ).ToList();

            return PartialView("_CountriesOverview", CountriesVM);
        }


        /// <summary>
        /// Create Country in DB
        /// </summary>
        /// <param name="countries"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryId,Title")] Countries countries)
        {
            if (ModelState.IsValid)
            {
                db.Countries.Add(countries);
                try
                {
                    db.SaveChanges();

                } catch (DbUpdateException ex)
                {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("CountryID", "Das Länderkürzel ist schon vorhanden!");
                    return View(countries);
                }

                return RedirectToAction("Index");
            }

            return View(countries);
        }

        /// <summary>
        /// Edit View Country
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Countries countries = db.Countries.Find(id);
            if (countries == null)
            {
                return HttpNotFound();
            }
            return View(countries);
        }

        /// <summary>
        /// Edit Country in DB
        /// </summary>
        /// <param name="countries"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryId,Title")] Countries countries)
        {
            if (ModelState.IsValid) {
                db.Entry(countries).State = EntityState.Modified;

                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("CountryId", "Es ist ein Fehler aufgetreten!");
                    return View(countries);
                }
                
                return RedirectToAction("Index");
            }
            return View(countries);
        }

        /// <summary>
        /// Delete View Country
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Countries countries = db.Countries.Find(id);
            if (countries == null)
            {
                return HttpNotFound();
            }
            return View(countries);
        }

        /// <summary>
        /// Delete Country in DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Countries countries = db.Countries.Find(id);
            db.Countries.Remove(countries);
            try {
                db.SaveChanges();

            } catch (Exception ex) {
                var ErrorCode = ex.InnerException.HResult;
                ModelState.AddModelError("CountryId", "Sie können dieses Land nicht löschen!");
                return View(countries);
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
