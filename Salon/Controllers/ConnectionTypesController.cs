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
    public class ConnectionTypesController : Controller
    {
        private SalonEntities db = new SalonEntities();

        /// <summary>
        /// Index View ConnectionType
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.ConnectionTypes.ToList());
        }

        /// <summary>
        /// Detail View ConnectionType
        /// </summary>
        /// <param name="id">ConnectionTypeId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create View ConnectionType
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create ConnectionType in DB
        /// </summary>
        /// <param name="connectionTypes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConnectionTypeId,Title,Description")] ConnectionTypes connectionTypes)
        {
            if (ModelState.IsValid)
            {
                db.ConnectionTypes.Add(connectionTypes);
                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("ConnectionTypeId", "Es ist ein Fehler aufgetreten!");
                    return View(connectionTypes);
                }
                return RedirectToAction("Index");
            }

            return View(connectionTypes);
        }

        /// <summary>
        /// Edit View ConnectionType
        /// </summary>
        /// <param name="id">ConnectionTypeId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit ConnectionType in DB
        /// </summary>
        /// <param name="connectionTypes">ConnectionTypeId</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConnectionTypeId,Title,Description")] ConnectionTypes connectionTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionTypes).State = EntityState.Modified;
                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("ConnectionTypeId", "Es ist ein Fehler aufgetreten!");
                    return View(connectionTypes);
                }
                return RedirectToAction("Index");
            }
            return View(connectionTypes);
        }

        /// <summary>
        /// Delete ConnectionType View
        /// </summary>
        /// <param name="id">ConnectionTypeId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete View ConnectionType
        /// </summary>
        /// <param name="id">ConnectionTypeId</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConnectionTypes connectionTypes = db.ConnectionTypes.Find(id);
            db.ConnectionTypes.Remove(connectionTypes);


            try {
                db.SaveChanges();

            } catch (DbUpdateException ex) {
                var ErrorCode = ex.InnerException.HResult;
                ModelState.AddModelError("ConnectionTypeId", "Sie können diesen Kontakttyp nicht löschen!");
                return View(connectionTypes);
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
