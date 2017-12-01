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
    }
}