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

        // GET: Countries
        public ActionResult Index()
        {
            return View(db.Countries.ToList());
        }

        // GET: Countries/Details/5
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

        // GET: Countries/Create
        public ActionResult Create()
        {
            return View();
        }



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




        // POST: Countries/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    //ViewBag.exceptionmessage = "Der Länderkürzel ist schon vorhanden!";
                    ModelState.AddModelError("CountryID", "Das Länderkürzel ist schon vorhanden!");
                    return View(countries);
                }




                return RedirectToAction("Index");
            }

            return View(countries);
        }

        // GET: Countries/Edit/5
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

        // POST: Countries/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryId,Title")] Countries countries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(countries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(countries);
        }

        // GET: Countries/Delete/5
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

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Countries countries = db.Countries.Find(id);
            db.Countries.Remove(countries);
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
