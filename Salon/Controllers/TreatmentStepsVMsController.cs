using Salon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Salon.Controllers
{
    public class TreatmentStepsVMsController : Controller
    {
        private SalonEntities db = new SalonEntities();
        // GET: TreatmentStepsVMs
        public ActionResult Index()
        {
            IEnumerable<TreatmentsVM> Treatments = (from t in db.Treatments
                                                    select new TreatmentsVM
                                                    {
                                                        TreatmentId = t.TreatmentId,
                                                        Title = t.Title,
                                                        Description = t.Description,
                                                        isActive = t.isActive
                                                    }).ToList();
            return View (Treatments);
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
            var topt = db.StepOptions.Include(z => z.Steps);
            IEnumerable<OptionsVM> TreatmentStepOptions = (from s in topt
                                                   where s.StepId == id
                                                   select new OptionsVM
                                                   {
                                                      StepOptionId = s.StepOptionId,
                                                      Position = s.Position,
                                                      Option = s.Option,
                                                      Description = s.Description,
                                                      isActive = s.isActive
                                                   }).ToList();
            return PartialView("_TreatmentStepOptions", TreatmentStepOptions);
        }

        public ActionResult CreateTreatment()
        {
            return View("CreateEditTreatments", new Treatments());
        }
        public ActionResult EditTreatment(int id)
        {
            return View("CreateEditTreatments", db.Treatments.Find(id));
        }

        // Submit and add or update database
        public ActionResult CreatEditTreatments()
        {
            return View(db.Treatments.ToList());
        }


        public ActionResult CreateStep()
        {
            return View("CreateEditTreatments", new Steps());
        }
        public ActionResult EditStep(int id)
        {
            return View("CreateEditTreatments", db.Steps.Find(id));
        }

        // Submit and add or update database
        [HttpPost]
        public ActionResult CreateEditStep(Steps model)
        {
            if (ModelState.IsValid)
            {
                // No id so we add it to database
                if (model.StepId <= 0)
                {
                    db.Steps.Add(model);
                }
                // Has Id, therefore it's in database so we update
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("CreateEditTreatments");
            }

            return View(model);
        }

        public ActionResult CreateOption()
        {
            return View("CreateEditTreatments", new StepOptions());
        }
        public ActionResult EditOption(int id)
        {
            return View("CreateEditTreatments", db.Steps.Find(id));
        }

        // Submit and add or update database
        [HttpPost]
        public ActionResult CreateEditOption(StepOptions model)
        {
            if (ModelState.IsValid)
            {
                // No id so we add it to database
                if (model.StepId <= 0)
                {
                    db.StepOptions.Add(model);
                }
                // Has Id, therefore it's in database so we update
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("CreateEditTreatments");
            }

            return View(model);
        }
    }
}