using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Salon.Models;
using System.Collections.Specialized;
using Microsoft.AspNet.Identity;

namespace Salon.Controllers
{
    public class VisitsController : Controller
    {
        private SalonEntities db = new SalonEntities();

        // GET: Visits
        public ActionResult Index(int? skip, bool? success)
        {
            if (skip == null || skip < 0)
                skip = 0;

            if(success != null) {
                ViewBag.success = success;
            }
            ViewBag.skip = skip;
            var visits = db.Visits.Include(v => v.AspNetUsers).Include(v => v.AspNetUsers1).Include(v => v.Customers);
            IEnumerable<VisitShortViewModel> shortVisitViewModels = (from v in visits
                                                                     orderby v.Created descending 
                                                                     select new VisitShortViewModel {
                                                                         visitId = v.VisitId,
                                                                         created = v.Created,
                                                                         customer = v.Customers,
                                                                         stylist = v.AspNetUsers1
                                                                     }).Skip((int)skip * 50).Take(50).ToList();
            return View(shortVisitViewModels);
        }

        public ActionResult VisitShort() {
            var visits = db.Visits.Include(v => v.AspNetUsers).Include(v => v.AspNetUsers1).Include(v => v.Customers);
            IEnumerable<VisitShortViewModel> shortVisitViewModels = (from v in visits
                                                                     orderby v.Created
                                                                     select new VisitShortViewModel {
                                                                         visitId = v.VisitId,
                                                                         created = v.Created,
                                                                         customer = v.Customers,
                                                                         stylist = v.AspNetUsers1
                                                                     }).ToList();
            return View(shortVisitViewModels);
        }

        public ActionResult VisitDetails(int? id) {
            var visit = db.Visits.Find(id);
            VisitDetailViewModel visitDetails = new VisitDetailViewModel();
            visitDetails.visitId = visit.VisitId;
            visitDetails.created = visit.Created;
            visitDetails.customer = visit.Customers;
            visitDetails.stylist = visit.AspNetUsers1;
            visitDetails.modifiedBy = visit.AspNetUsers;
            visitDetails.modified = visit.Modified;
            visitDetails.duration = visit.Duration;
            visitDetails.treatments = getVisitTreatments(visit);
            
            return PartialView(visitDetails);
        }

        private List<VisitTreatment> getVisitTreatments(Visits v) {
            List<VisitTreatment> treatments = new List<VisitTreatment>();

            bool ok;
            Treatments treatmentTemplate;
            foreach(VisitTasks vt in v.VisitTasks) {
                ok = false;
                foreach(VisitTreatment t in treatments) {
                    if(t.treatmentID == vt.TreatmentId) {
                        t.tasks.Add(vt);
                        ok = true;
                        break;
                    }
                }
                if (!ok) {
                    treatmentTemplate = vt.getTreatment();
                    VisitTreatment visitTreatment = new VisitTreatment();
                    visitTreatment.treatmentID = vt.TreatmentId;
                    visitTreatment.name = treatmentTemplate.Title;
                    visitTreatment.tasks.Add(vt);
                    visitTreatment.possibleTasks = treatmentTemplate.TreatmentSteps.ToList();
                    treatments.Add(visitTreatment);
                }
            }
            return treatments;
        }

        public ActionResult VisitCreate(int? id) {
            Customers customer = null;
            //TEST VALUES TODO CHANGE
            //AspNetUsers stylist = db.AspNetUsers.Find("33abf8c7-5ae1-4ed6-819f-9d325e57d7bb");
            AspNetUsers stylist = db.AspNetUsers.Find(User.Identity.GetUserId());
            AspNetUsers teacher = db.AspNetUsers.Find("33abf8c7-5ae1-4ed6-819f-9d325e57d7bb");
            Rooms room = db.Rooms.Find(2);

            Visits visit;

            DateTime created = DateTime.Now;
            List<VisitTreatment> visitTreatments = new List<VisitTreatment>();
            if(id != null) {
                visit = db.Visits.Find(id);
                created = visit.Created;
                stylist = visit.AspNetUsers1;
                customer = visit.Customers;
                visitTreatments = getVisitTreatments(visit);
                teacher = visit.AspNetUsers2;
                room = visit.Rooms;
            }

            ViewBag.teachers = (from t in db.AspNetUsers
                                where t.AspNetRoles.Any(r => r.Id == "3") select t).ToList();
            ViewBag.rooms = db.Rooms.ToList();
            
            VisitCreateViewModel model = new VisitCreateViewModel();
            model.visitId = id;
            model.created = DateTime.Now;
            model.customer = customer;
            model.stylist = stylist;
            model.availableTreatments = db.Treatments.ToList();
            model.selectedTreatments = visitTreatments;
            model.teacher = new KeyValuePair<string, string>(teacher.Id, teacher.lastName);
            model.room = new KeyValuePair<int, string>(room.RoomId, room.Title);
            return View(model);
        }

        public ActionResult SaveVisit() {
            Visits visit = new Visits();
            NameValueCollection nvc = Request.Form;

            int cusId;
            string stylistId;
            int duration;
            if (!string.IsNullOrEmpty(nvc["slc_customerId"]) && Int32.TryParse(nvc["slc_customerId"], out cusId)) {
                cusId = Int32.Parse(nvc["slc_customerId"]);
                
            } else {
                return Redirect("/Visits/Index?success=false");
            }
            if (!string.IsNullOrEmpty(nvc["inp_stylistId"])) {
                stylistId = nvc["inp_stylistId"];
            }else {
                return Redirect("/Visits/Index?success=false");
            }
            if (!string.IsNullOrEmpty(nvc["inp_approx_duration"]) && Int32.TryParse(nvc["inp_approx_duration"], out duration)) {
                visit.Duration = duration;
            }

            visit.AspNetUsers2 = db.AspNetUsers.Find(nvc["slc_teacher"]);
            visit.RoomId = Int32.Parse(nvc["slc_room"]);

            visit.AspNetUsers1 = db.AspNetUsers.Find(stylistId);
            visit.AspNetUsers = db.AspNetUsers.Find(stylistId);
            visit.Customers = db.Customers.Find(cusId);
            visit.Created = DateTime.Now;
            visit.Modified = DateTime.Now;
            db.Visits.Add(visit);
            db.SaveChanges();

            int i = 0;
            int treatmentId;
            int stepId;
            string inType;
            VisitTasks vt;
            foreach (string key in nvc.AllKeys) {
                if(i < 6) {
                    i++;
                }else {
                    treatmentId = Int32.Parse(key.Split('_').GetValue(2).ToString().Trim());
                    stepId = Int32.Parse(key.Split('_').GetValue(3).ToString());
                    inType = key.Split('_').GetValue(0).ToString();
                    vt = new VisitTasks();
                    vt.StepId = stepId;
                    vt.TreatmentId = treatmentId;

                    if(inType == "slc") {
                        vt.StepOptionId = Int32.Parse(nvc[key]);
                    }else {
                        vt.Description = nvc[key];
                    }
                    visit.VisitTasks.Add(vt);
                    db.VisitTasks.Add(vt);
                    Console.WriteLine(nvc[key]);
                }
            }
            db.SaveChanges();
            if (nvc["btn_save"] == "back") {
                return Redirect("/Visits/Index?success=true");
            }else {
                return Redirect("/Visits/VisitCreate/" + visit.VisitId);
            }
            
            //return Redirect("/Visits/VisitCreate/" + visit.VisitId);
            //return VisitCreate(visit.VisitId);
        }

        public ActionResult updateVisit(int id) {
            Visits visit = db.Visits.Find(id);
            NameValueCollection nvc = Request.Form;

            int cusId;
            int duration;
            string stylistId;

            if (!string.IsNullOrEmpty(nvc["slc_customerId"]) && Int32.TryParse(nvc["slc_customerId"], out cusId)) {
                cusId = Int32.Parse(nvc["slc_customerId"]);

            } else {
                return Redirect("/Visits/Index?success=false");
            }
            if (!string.IsNullOrEmpty(nvc["inp_stylistId"])) {
                stylistId = nvc["inp_stylistId"];
            } else {
                return Redirect("/Visits/Index?success=false");
            }
            if (!string.IsNullOrEmpty(nvc["inp_approx_duration"]) && Int32.TryParse(nvc["inp_approx_duration"], out duration)) {
                visit.Duration = duration;
            }

            
            visit.AspNetUsers2 = db.AspNetUsers.Find(nvc["slc_teacher"]);
            visit.RoomId = Int32.Parse(nvc["slc_room"]);
            visit.AspNetUsers = db.AspNetUsers.Find(stylistId);
            visit.Customers = db.Customers.Find(cusId);
            visit.Modified = DateTime.Now;
            db.SaveChanges();

            var delete = db.VisitTasks.Where(x => x.VisitId == id);
            foreach(var row in delete) {
                db.VisitTasks.Remove(row);
            }

            int i = 0;
            int treatmentId;
            int stepId;
            string inType;
            VisitTasks vt;
            foreach (string key in nvc.AllKeys) {
                if (i < 6) {
                    i++;
                } else {
                    treatmentId = Int32.Parse(key.Split('_').GetValue(2).ToString().Trim());
                    stepId = Int32.Parse(key.Split('_').GetValue(3).ToString());
                    inType = key.Split('_').GetValue(0).ToString();
                    vt = new VisitTasks();
                    vt.StepId = stepId;
                    vt.TreatmentId = treatmentId;
                    if (inType == "slc") {
                        vt.StepOptionId = Int32.Parse(nvc[key]);
                    } else {
                        vt.Description = nvc[key];
                    }
                    visit.VisitTasks.Add(vt);
                    db.VisitTasks.Add(vt);
                    Console.WriteLine(nvc[key]);
                }
            }
            db.SaveChanges();

            if (nvc["btn_save"] == "back") {
                return Redirect("/Visits/Index?success=true");
            } else {
                return Redirect("/Visits/VisitCreate/" + id);
            }
        }

        public ActionResult _CustomerPicker() {
            var customers = db.Customers;
            IEnumerable<CustomerPicker> picker = (from c in customers
                                                  orderby c.LName
                                                  select new CustomerPicker {
                                                      lName = c.LName,
                                                      fName = c.FName,
                                                      customerId = c.CustomerId
                                                  }).ToList();
            return PartialView(picker);
        }
        
        public ActionResult _TreatmentForm(int id) {
            Treatments treatment = db.Treatments.Find(id);
            VisitTreatment model = new VisitTreatment();
            model.name = treatment.Title;
            model.treatmentID = treatment.TreatmentId;
            VisitTasks vt;
            foreach(TreatmentSteps s in treatment.TreatmentSteps) {
                vt = new VisitTasks();
                vt.StepId = s.StepId;
                vt.TreatmentSteps = s;
                vt.TreatmentId = treatment.TreatmentId;
                model.tasks.Add(vt);
                model.possibleTasks.Add(vt.TreatmentSteps);
            }
            return PartialView(model);
        }

        /*public ActionResult _TreatmentForm(VisitTreatment vt) {
            return PartialView(vt);
        }*/



        // GET: Visits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits visits = db.Visits.Find(id);
            if (visits == null)
            {
                return HttpNotFound();
            }
            return View(visits);
        }

        // GET: Visits/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisitId,Duration,CustomerId,ModifiedBy,Modified,CreatedBy,Created")] Visits visits)
        {
            if (ModelState.IsValid)
            {
                db.Visits.Add(visits);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.ModifiedBy);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId", visits.CustomerId);
            return View(visits);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits visits = db.Visits.Find(id);
            if (visits == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.ModifiedBy);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId", visits.CustomerId);
            return View(visits);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisitId,Duration,CustomerId,ModifiedBy,Modified,CreatedBy,Created")] Visits visits)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visits).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.AspNetUsers, "Id", "Email", visits.ModifiedBy);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CountryId", visits.CustomerId);
            return View(visits);
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits visits = db.Visits.Find(id);
            if (visits == null)
            {
                return HttpNotFound();
            }
            return View(visits);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Visits visits = db.Visits.Find(id);
            db.Visits.Remove(visits);
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
