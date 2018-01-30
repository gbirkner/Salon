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
    public class ConnectionsController : Controller
    {
        private SalonEntities db = new SalonEntities();

        /// <summary>
        /// Index View Connection
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var connections = db.Connections.Include(c => c.ConnectionTypes).Include(c => c.Customers);
            return View(connections.ToList());
            
        }

        /// <summary>
        /// Detail View Connection
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
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

        /// <summary>
        /// Create View Connection
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull");

            return View();
        }

        /// <summary>
        /// Create Connection in DB
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConnectionId,ConnectionTypeId,CustomerId,Title,Description")] Connections connections)
        {
            if (ModelState.IsValid)
            {
                db.Connections.Add(connections);

                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("ConnectionId", "Es ist ein Fehler aufgetreten!");
                    return View(connections);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title", connections.ConnectionTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", connections.CustomerId);
            return View(connections);
        }

        /// <summary>
        /// Edit View Connection
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
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

        /// <summary>
        /// Overview for Search
        /// </summary>
        /// <param name="searchstring">Searchstring</param>
        /// <returns></returns>
        public ActionResult ConnectionsOverview(string searchstring = null)
        {
            var cons = db.Connections.Include(p => p.Customers).Include(p => p.ConnectionTypes);
            IEnumerable<ConnectionViewModel> ConVM = (
                from c in cons
                where c.Customers.FName.Contains(searchstring) || c.Customers.LName.Contains(searchstring) || c.Title.Contains(searchstring) || c.ConnectionTypes.Title.Contains(searchstring) || c.Description.Contains(searchstring)
                orderby c.Title
                select new ConnectionViewModel
                {
                    ConnectionId = c.ConnectionId,
                    Title = c.Title,
                    TypTitle = c.ConnectionTypes.Title,
                    FName = c.Customers.FName,
                    LName = c.Customers.LName,
                    Description = c.Description
                }
                ).ToList();

            return PartialView("_ConnectionsOverview", ConVM);
        }

        /// <summary>
        /// Edit Connection in DB
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConnectionId,ConnectionTypeId,CustomerId,Title,Description")] Connections connections)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connections).State = EntityState.Modified;

                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("ConnectionId", "Es ist ein Fehler aufgetreten!");
                    return View(connections);
                }
                return RedirectToAction("Index");
            }
            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title", connections.ConnectionTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", connections.CustomerId);
            return View(connections);
        }

        /// <summary>
        /// Delete View Connecion
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
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

        /// <summary>
        /// Delete Connection in DB
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Connections connections = db.Connections.Find(id);
            db.Connections.Remove(connections);
            try {
                db.SaveChanges();

            } catch (Exception ex) {
                var ErrorCode = ex.InnerException.HResult;
                ModelState.AddModelError("ConnectionId", "Es ist ein Fehler aufgetreten!");
                return View(connections);
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
