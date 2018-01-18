using Salon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;

namespace Salon.Controllers
{
    public class TreatmentStepsVMsController : Controller
    {
        private SalonEntities db = new SalonEntities();
        // GET: TreatmentStepsVMs
        public ActionResult Index()
        {
            return View (db.Treatments.ToList());
        }

        public ActionResult TreatmentSteps(int? id = null)
        {
            var tsteps = db.TreatmentSteps.Include(y => y.Steps);
            IEnumerable<StepsVM>  TreatmentSteps = (from t in tsteps
                                                    where t.TreatmentId == id
                                                    select new StepsVM
                                                    {
                                                        StepsId = t.StepId,
                                                        Title = t.Steps.Title,
                                                        Description = t.Steps.Description,
                                                        isSensitive = t.Steps.isSensitive,
                                                        isActive = t.Steps.isActive,
                                                        Duration = t.Duration,
                                                        Order = t.StepOrder
                                                    }).ToList();
            return PartialView("_TreatmentSteps", TreatmentSteps);
        }

        //GET: Stepoptions Index Page
        public ActionResult TreatmentStepOptions(int? id = null)
        {
            return PartialView("_TreatmentStepOptions", db.StepOptions.Where( sid => sid.StepId == id).ToList());
        }

        // GET: Treatment/Create
        public ActionResult CreateTreatment()
        {
            return View();
        }

        // POST: Treatment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTreatment([Bind(Include = "Title,Description,isActive")] Treatments treatments)
        {
            if (ModelState.IsValid)
            {
                db.Treatments.Add(treatments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(treatments);
        }

        // GET: EditTreatment/Edit/5
        public ActionResult EditTreatment(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatments treatments = db.Treatments.Find(id);
            if (treatments == null)
            {
                return HttpNotFound();
            }
            return View(treatments);
        }

        // POST: EditTreatment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTreatment([Bind(Include = "TreatmentId,Title,Description,isActive")] Treatments treatments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treatments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TreatmentId = new SelectList(db.Treatments, "TreatmentId", "Title", treatments.TreatmentId);
            return View(treatments);
        }

        // GET: CreateEditSteps/Edit/5
        public ActionResult CreateEditSteps(int? id = null)
        {
            var tsteps = db.TreatmentSteps.Include(y => y.Steps);
            List<StepsVM> TreatmentSteps = (from t in tsteps
                                            where t.TreatmentId == id
                                            select new StepsVM
                                            {
                                                TreatmentId = t.TreatmentId,
                                                StepsId = t.StepId,
                                                Title = t.Steps.Title,
                                                Description = t.Steps.Description,
                                                isSensitive = t.Steps.isSensitive,
                                                isActive = t.Steps.isActive,
                                                Duration = t.Duration,
                                                Order = t.StepOrder
                                            }).ToList();
            ViewBag.Name = db.Treatments.Find(id).Title;
            return View("CreateEditSteps", TreatmentSteps);
        }

        // POST: Create / Edit Steps
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSteps(List<StepsVM> svm)
        {
            List<Steps> steps = (from s in svm
                                 select new Steps
                                 {
                                     StepId = s.StepsId,
                                     Title = s.Title,
                                     Description = s.Description,
                                     isSensitive = s.isSensitive,
                                     isActive = s.isActive
                                 }).ToList();
            foreach (Steps s in steps)
            {
                if (s.StepId != 0)
                    db.Entry(s).State = EntityState.Modified;
                else
                    db.Entry(s).State = EntityState.Added;
            }

            bool SaveFailed = false;
            do
            {
                SaveFailed = false;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    SaveFailed = true;

                    // Update original values from the database 
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (SaveFailed == true);

            List<TreatmentSteps> tsteps = (from st in svm
                                            select new TreatmentSteps
                                            {
                                                TreatmentId = st.TreatmentId,
                                                StepId = st.StepsId,
                                                StepOrder = st.Order,
                                                Duration = st.Duration
                                            }).ToList();

            foreach (TreatmentSteps s in tsteps)
            {
                if (s.StepId != 0)
                    db.Entry(s).State = EntityState.Modified;
                else
                    db.Entry(s).State = EntityState.Added;
            }

            for (int i = 0; i < steps.Count(); i++)
            {
                if (tsteps[i].StepId == 0)
                {
                    tsteps[i].StepId = steps[i].StepId;
                }
            }

            SaveFailed = false;
            do
            {
                SaveFailed = false;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    SaveFailed = true;

                    // Update original values from the database 
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (SaveFailed == true);

            return RedirectToAction("Index");
        }

        public ActionResult CreatEditStepOptions(int? id = null)
        {
            ViewBag.Name = db.Steps.Find(id).Title;
            ViewBag.StepId = db.Steps.Find(id).StepId;
            return PartialView("_CreatEditStepOptions", db.StepOptions.Where(sid => sid.StepId == id).ToList());
        }


        // POST: Create / Edit Steps
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOptions(List<StepOptions> options)
        {
            foreach (StepOptions o in options)
            {
                if (o.StepOptionId != 0)
                    db.Entry(o).State = EntityState.Modified;
                else
                    db.Entry(o).State = EntityState.Added;
            }

            //if (ModelState.IsValid)
            //{
                bool SaveFailed = false;
                do
                {
                    SaveFailed = false;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        SaveFailed = true;

                        // Update original values from the database 
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }
                } while (SaveFailed == true);
            //}

            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }

    }
}