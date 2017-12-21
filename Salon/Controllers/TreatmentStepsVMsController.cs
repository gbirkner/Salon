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


        // Submit and add or update database
        public ActionResult CreatEditTreatments()
        {
            return View(db.Treatments.ToList());
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

        public ActionResult CreatEditStepOptions(int? id = null)
        {

            return PartialView("_CreatEditStepOptions", db.StepOptions.Where(sid => sid.StepId == id).ToList());
        }

        // POST: Create / Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTreatments(List<Salon.Models.Treatments> treatments)
        {
            foreach (Treatments tr in treatments)
            {
                if (tr.TreatmentId != 0)
                    db.Entry(tr).State = EntityState.Modified;
                else
                    db.Entry(tr).State = EntityState.Added;
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
            return RedirectToAction("CreatEditTreatments", treatments);
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