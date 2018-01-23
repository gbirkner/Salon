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
    public class PicturesController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: Pictures
        public ActionResult Index()
        {
            var pictures = db.Pictures.Include(p => p.Visits);
            return View(pictures.ToList());
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pictures pictures = db.Pictures.Find(id);
            if (pictures == null)
            {
                return HttpNotFound();
            }
            return View(pictures);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            ViewBag.VisitId = new SelectList(db.Visits, "VisitId", "Created");
            return View();
        }

        // POST: Pictures/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase Photo, [Bind(Include = "PictureId,isSketch,VisitId,Description")] Pictures pictures)
        {

            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    pictures.Photo = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(pictures.Photo, 0, Photo.ContentLength);
                    pictures.VisitId = null;
                    db.Pictures.Add(pictures);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.VisitId = new SelectList(db.Visits, "VisitId", "ModifiedBy", pictures.VisitId);
            return View(pictures);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pictures pictures = db.Pictures.Find(id);
            if (pictures == null)
            {
                return HttpNotFound();
            }
            ViewBag.VisitId = new SelectList(db.Visits, "VisitId", "ModifiedBy", pictures.VisitId);
            return View(pictures);
        }

        // POST: Pictures/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase Photo, [Bind(Include = "PictureId,isSketch,VisitId,Description")] Pictures pictures)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    pictures.Photo = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(pictures.Photo, 0, Photo.ContentLength);
                    db.Entry(pictures).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.VisitId = new SelectList(db.Visits, "VisitId", "ModifiedBy", pictures.VisitId);
            return View(pictures);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pictures pictures = db.Pictures.Find(id);
            if (pictures == null)
            {
                return HttpNotFound();
            }
            return View(pictures);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pictures pictures = db.Pictures.Find(id);
            db.Pictures.Remove(pictures);
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

        [HttpGet]
        public ActionResult DrawSketch()
        {
            List<Pictures> pictures = db.Pictures.Where(e => e.PictureId >= 6 && e.PictureId <= 11).ToList();
            return PartialView("DrawSketch", pictures);
        }

        [HttpGet]
        public ActionResult AddPhoto()
        {
            return PartialView("AddPhoto");
        }

        // upload photo
        [HttpPost]
        public ActionResult AddPhoto(HttpPostedFileBase Photo, [Bind(Include = "PictureId,Description,visitId")] Pictures pictures)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    int visitId = Int32.Parse(Request["photoVisitId"]);

                    pictures.Photo = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(pictures.Photo, 0, Photo.ContentLength);
                    pictures.isSketch = false;
                    pictures.VisitId = visitId;
                    db.Pictures.Add(pictures);
                    db.SaveChanges();
                    return RedirectToAction("VisitCreate", "Visits", new { id = visitId });
                }
            }

            ViewBag.VisitId = new SelectList(db.Visits, "VisitId", "ModifiedBy", pictures.VisitId);
            return View(pictures);
        }

        // upload sketches
        [HttpPost]
        public ActionResult AddSketches()
        {
            string imageDataDreieck = Request["sketchDataDreieck"];
            string imageDataOval = Request["sketchDataOval"];
            string imageDataRund = Request["sketchDataRund"];
            string imageDataViereck = Request["sketchDataViereck"];
            string imageDataSchmal = Request["sketchDataSchmal"];
            string imageDataSeite = Request["sketchDataSeite"];

            string imageDescriptionDreieck = Request["descriptionDreieck"];
            string imageDescriptionOval = Request["descriptionOval"];
            string imageDescriptionRund = Request["descriptionRund"];
            string imageDescriptionViereck = Request["descriptionViereck"];
            string imageDescriptionSchmal = Request["descriptionSchmal"];
            string imageDescriptionSeite = Request["descriptionSeite"];

            string visitId = Request["sketchVisitId"];

            Pictures pictures;
            
            if (!imageDataDreieck.Equals(""))
            {
                pictures = new Pictures();
                pictures.Photo = Convert.FromBase64String(imageDataDreieck);
                pictures.Description = imageDescriptionDreieck;
                pictures.isSketch = true;
                pictures.VisitId = Int32.Parse(visitId);
                db.Pictures.Add(pictures);
                db.SaveChanges();
            }

            if (!imageDataOval.Equals(""))
            {
                pictures = new Pictures();
                pictures.Photo = Convert.FromBase64String(imageDataOval);
                pictures.Description = imageDescriptionOval;
                pictures.isSketch = true;
                pictures.VisitId = Int32.Parse(visitId);
                db.Pictures.Add(pictures);
                db.SaveChanges();
            }

            if (!imageDataRund.Equals(""))
            {
                pictures = new Pictures();
                pictures.Photo = Convert.FromBase64String(imageDataRund);
                pictures.Description = imageDescriptionRund;
                pictures.isSketch = true;
                pictures.VisitId = Int32.Parse(visitId);
                db.Pictures.Add(pictures);
                db.SaveChanges();
            }

            if (!imageDataViereck.Equals(""))
            {
                pictures = new Pictures();
                pictures.Photo = Convert.FromBase64String(imageDataViereck);
                pictures.Description = imageDescriptionViereck;
                pictures.isSketch = true;
                pictures.VisitId = Int32.Parse(visitId);
                db.Pictures.Add(pictures);
                db.SaveChanges();
            }

            if (!imageDataSchmal.Equals(""))
            {
                pictures = new Pictures();
                pictures.Photo = Convert.FromBase64String(imageDataSchmal);
                pictures.Description = imageDescriptionSchmal;
                pictures.isSketch = true;
                pictures.VisitId = Int32.Parse(visitId);
                db.Pictures.Add(pictures);
                db.SaveChanges();
            }

            if (!imageDataSeite.Equals(""))
            {
                pictures = new Pictures();
                pictures.Photo = Convert.FromBase64String(imageDataSeite);
                pictures.Description = imageDescriptionSeite;
                pictures.isSketch = true;
                pictures.VisitId = Int32.Parse(visitId);
                db.Pictures.Add(pictures);
                db.SaveChanges();
            }

            return RedirectToAction("VisitCreate", "Visits", new { id = visitId });
        }

        // delete picture
        public ActionResult DeletePicture(int? id)
        {
            Pictures pictures = db.Pictures.Find(id);

            int visitId = pictures.VisitId.Value;

            db.Pictures.Remove(pictures);
            db.SaveChanges();

            return RedirectToAction("VisitCreate", "Visits", new { id = visitId });
        }
    }
}
