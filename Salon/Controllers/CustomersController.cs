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

        /* Beschreibung Customer Page
         
            Seitenaufbau:

            Suche:
                - Bei der Suche wird ein Actionlink verwendet der eine Controllerfunktion aufruft
                  und die CustomerOverview zurückgibt, diese wird ein Div eingefügt. Die Suche wird
                  bei Keyupdate ausgelöst. Controllerfunktion: _CustomerOverview
                  Der Code wird unten auf der Index Seite gefunden.


            Kundenübersicht mit folgenden zusätzlichen Links:
                - Besuche (Masterdetail) -> _CustomerConnection
                - Kontaktdaten (Masterdetail) -> _VisitShort

            Masterdetail:

            Kontaktdaten:
                - Kontaktdaten hinzufügen im Modal (_Create im Controller)
                - Kontaktdaten löschen im Modal (_Delete im Controller)
                - Kontaktdaten editieren im Modal (_Edit im Controller)
            
            Funktionen zu den Modalen befinden sich in der _CustomerConnection View.
        

            Besuche:
                - Link zur Detail-Ansicht vom Besuch, die Besuche können da editiert werden

             
             */

        /// <summary>
        /// Delete Customer by Procedure
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
        public ActionResult Delete (int id)
        {
            db.DeleteCustomerByID(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Anonymize Customer by Procedure
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
        public ActionResult Anonymisieren(int id)
        {
            db.AnonymizeCustomerByID(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Index Customers
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Overview for Search
        /// </summary>
        /// <param name="searchstring">Searchstring</param>
        /// <returns></returns>
        public ActionResult CustomerOverview(string searchstring = null)
        {
            var cust = db.Customers.Include(p => p.Cities).Include(g => g.Genders).Include(k => k.Cities.Countries);
            IEnumerable<CustomerViewModel> CustomerViewModels = (
                from c in cust
                //find Searchstring in Columns
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

        /// <summary>
        /// MasterDetail Visits
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
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

        /// <summary>
        /// MasterDetail Connections
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create new Connection (View)
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
        public ActionResult _Create(int? id)
        {
            ViewBag.ConnectionTypeId = new SelectList(db.ConnectionTypes, "ConnectionTypeId", "Title");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "NameFull", id);

            return PartialView();
        }

        /// <summary>
        /// Create Connection in DB
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "ConnectionId,ConnectionTypeId,CustomerId,Title,Description")] Connections connections)
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
        /// Edit Connection View
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit Connection in DB
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Edit([Bind(Include = "ConnectionId,ConnectionTypeId,CustomerId,Title,Description")] Connections connections)
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
        /// Delete Connection View
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete Connection DB
        /// </summary>
        /// <param name="id">ConnectionId</param>
        /// <returns></returns>
        [HttpPost, ActionName("_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteConfirmed(int id)
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


        /// <summary>
        /// Detail View Customer
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            customers.ModifiedBy = db.AspNetUsers.FirstOrDefault(a => a.Id == customers.ModifiedBy).UserName;
            customers.CreatedBy = db.AspNetUsers.FirstOrDefault(a => a.Id == customers.CreatedBy).UserName;


            //var cust = db.Customers.Include(p => p.Cities).Include(k => k.Cities.Countries).Include(k => k.Genders);
            //IEnumerable<CustomerViewModel> CustomerViewModels = (
            //    from c in cust
            //    orderby c.LName
            //    where c.CustomerId == id
            //    select new CustomerViewModel {
            //        CustomerId = c.CustomerId,
            //        FName = c.FName,
            //        LName = c.LName,
            //        GenderTitle = c.Genders.GenderTitle,
            //        PostalCode = c.Cities.PostalCode,
            //        CityName = c.Cities.Title,
            //        Country = c.Cities.Countries.Title,
            //        Street = c.Street,
            //        Description = c.Description,
            //        isActive = c.isActive,
            //        allowImages = c.allowImages,
            //        allowSensitive = c.allowSensitive,
            //        Modified = c.Modified,
            //        ModifiedBy = c.AspNetUsers.UserName,
            //        Created = c.Created,
            //        CreatedBy = c.AspNetUsers.UserName
            //    }
            //    );

            if (customers == null)
            {
                return HttpNotFound();
            }

            //if (CustomerViewModels == null) {
            //    return HttpNotFound();
            //}
            return View(customers);
        }

        /// <summary>
        /// Create Customer View
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle");
            return View();
        }

        /// <summary>
        /// Create Customer in DB
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
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
                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("CustomerId", "Es ist ein Fehler aufgetreten!");

                    ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
                    ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
                    ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
                    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
                    return View(customers);
                }
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
            return View(customers);
        }

        /// <summary>
        /// Edit Customer View
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit Customer in DB
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,FName,LName,Street,isActive,Description,allowImages,allowSensitive,ModifiedBy,Modified,CreatedBy,Created,GenderID,CityId")] Customers customers)
        {
            customers.Modified = DateTime.Now;
            customers.ModifiedBy = "0255c5b9-6cad-40a8-a5b7-bd916832519a"; //User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                try {
                    db.SaveChanges();

                } catch (Exception ex) {
                    var ErrorCode = ex.InnerException.HResult;
                    ModelState.AddModelError("CustomerId", "Es ist ein Fehler aufgetreten!");

                    ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
                    ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
                    ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
                    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
                    return View(customers);
                }
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", customers.ModifiedBy);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Title", customers.CityId);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderTitle", customers.GenderID);
            return View(customers);
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
