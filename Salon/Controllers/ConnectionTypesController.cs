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
    public class ConnectionTypesController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: ConnectionTypes
        public ActionResult Index()
        {
            return View(db.ConnectionTypes.ToList());
        }

        // GET: ConnectionTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionTypes connectionTypes = db.ConnectionTypes.Find(id);
            if (connectionTypes == null)
            {
                return HttpNotFound();
            }
            return View(connectionTypes);
        }

        // GET: ConnectionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConnectionTypes/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConnectionTypeId,Title,Description")] ConnectionTypes connectionTypes)
        {
            if (ModelState.IsValid)
            {
                db.ConnectionTypes.Add(connectionTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(connectionTypes);
        }

        // GET: ConnectionTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionTypes connectionTypes = db.ConnectionTypes.Find(id);
            if (connectionTypes == null)
            {
                return HttpNotFound();
            }
            return View(connectionTypes);
        }

        // POST: ConnectionTypes/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConnectionTypeId,Title,Description")] ConnectionTypes connectionTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(connectionTypes);
        }

        // GET: ConnectionTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionTypes connectionTypes = db.ConnectionTypes.Find(id);
            if (connectionTypes == null)
            {
                return HttpNotFound();
            }
            return View(connectionTypes);
        }

        // POST: ConnectionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConnectionTypes connectionTypes = db.ConnectionTypes.Find(id);
            db.ConnectionTypes.Remove(connectionTypes);
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
