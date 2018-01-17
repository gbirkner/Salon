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

        public ActionResult TreatmentStepOptions(int? id = null)
        {
            return PartialView("_TreatmentStepOptions", db.StepOptions.Where( sid => sid.StepId == id).ToList());
        }

        // GET: Countries/Edit/5
        public ActionResult EditTreatment(string id)
        {
            if (id == null)
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

        // GET: Countries/Edit/5
        public ActionResult CreateEditSteps(int? id = null)
        {
            var tsteps = db.TreatmentSteps.Include(y => y.Steps);
            List<StepsVM> TreatmentSteps = (from t in tsteps
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
            ViewBag.Name = db.Treatments.Find(id).Title;
            return View("CreateEditSteps", TreatmentSteps);
        }
        
        public ActionResult CreatEditSteps(int? id = null)
        {
            var tsteps = db.TreatmentSteps.Include(y => y.Steps);
            List<StepsVM> TreatmentSteps = (from t in tsteps
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
            return PartialView("_CreatEditSteps", TreatmentSteps);
        }

        public ActionResult CreatEditStepOptions(int? id = null, int? tId = null, int? sId = null)
        {
            ViewBag.TreatmentLoopId = tId;
            ViewBag.StepLoopId = sId;
            return PartialView("_CreatEditStepOptions", db.StepOptions.Where(sid => sid.StepId == id).ToList());
        }

        // POST: Create / Edit
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
            List<TreatmentSteps> tsteps = (from st in svm
                                 select new TreatmentSteps
                                 {
                                     TreatmentId = st.TreatmentId,
                                     StepId = st.StepsId,
                                     StepOrder = st.Order,
                                     Duration = st.Duration
                                 }).ToList();
            foreach (Steps s in steps)
            {
                if (s.StepId != 0)
                    db.Entry(s).State = EntityState.Modified;
                else
                    db.Entry(s).State = EntityState.Added;
            }
            foreach (TreatmentSteps st in tsteps)
            {
                if (st.StepId != 0)
                    db.Entry(st).State = EntityState.Modified;
                else
                    db.Entry(st).State = EntityState.Added;
            }
            if (ModelState.IsValid)
            {
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
            }
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