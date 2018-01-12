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
    public class SettingsController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: Settings
        public ActionResult Index()
        {
            return View(db.Settings.ToList());
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SettingID,AnonymizeUserByDays,AnonymizeCustomerByDays")] Settings settings)
        {
            if (ModelState.IsValid)
            {
                db.Settings.Add(settings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(settings);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                id = 1;
            }
            Settings settings = db.Settings.Find(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // POST: Settings/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Settings settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(settings);
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
