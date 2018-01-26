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
    /**
     * Controller to Handle the Views Visits/Index, Visits/VisitCreate, Visits/_CustomerPicker, Visits/_TreatmentView, CustomerVisit
     */
    public class VisitsController : Controller
    {
        private SalonEntities db = new SalonEntities();

        /**
         * Returns the View for the Index page, selects 50 visits and skips 'skip' visits if given
         * @param int? skip: if given, skip a number of entries
         * @param bool? success: if given, display a notification that the save was successful or not (Redirect after visit save)
         * @return /Visits/Index.cshtml View
         */
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

        /**
         * returns partial view filled w/ a given visit
         * @param int? id: id of the visit to display
         * @return PartialView Visits/VisitDetails 
         */
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
            
            foreach(Pictures p in visit.Pictures) {
                visitDetails.images.Add(p.Photo);
            }
            
            return PartialView(visitDetails);
        }

        /**
         * gets the VisitTreatments of a visit (since they arent just saved in the friggin' database)
         * @param Visit v: Visit to get treatments of
         * @return List of VisitTreatments
         */
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
                    visitTreatment.allowSensitive = v.Customers.allowSensitive;
                    visitTreatment.treatmentID = vt.TreatmentId;
                    visitTreatment.name = treatmentTemplate.Title;
                    visitTreatment.tasks.Add(vt);
                    visitTreatment.possibleTasks = treatmentTemplate.TreatmentSteps.ToList();
                    treatments.Add(visitTreatment);
                }
            }
            return treatments;
        }

        [Authorize]
        public ActionResult VisitCreate(int? id, int? cusId) {
            Customers customer;
            if (cusId != null) {
                customer = db.Customers.Find(cusId);
            } else {
                customer = null;
            }
            
            //AspNetUsers stylist = db.AspNetUsers.Find("33abf8c7-5ae1-4ed6-819f-9d325e57d7bb");
            AspNetUsers stylist = db.AspNetUsers.Find(User.Identity.GetUserId());

            
            //TODO: read session variables 4 teacher & room!
            AspNetUsers teacher = db.AspNetUsers.Find("33abf8c7-5ae1-4ed6-819f-9d325e57d7bb");
            Rooms room = db.Rooms.Find(2);
            if (Session["room"] != null) {
                room = db.Rooms.Find(Int32.Parse(Session["room"].ToString()));
            }
            if(Session["teacher"] != null) {
                teacher = db.AspNetUsers.Find(Session["teacher"]);
            }
            int duration = 0;

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
                duration = visit.Duration;
            }

            ViewBag.teachers = (from t in db.AspNetUsers
                                where t.AspNetRoles.Any(r => r.Id == "3") select t).ToList();
            ViewBag.rooms = db.Rooms.ToList();
            
            VisitCreateViewModel model = new VisitCreateViewModel();
            model.visitId = id;
            model.created = DateTime.Now;
            model.customer = customer;
            model.stylist = stylist;
            model.availableTreatments = sortTheGoddamnedListThxDaniel(db.Treatments.ToList());
            model.selectedTreatments = visitTreatments;
            model.teacher = new KeyValuePair<string, string>(teacher.Id, teacher.lastName);
            model.room = new KeyValuePair<int, string>(room.RoomId, room.Title);
            model.duration = duration;
            return View(model);
        }

        private List<Treatments> sortTheGoddamnedListThxDaniel(List<Treatments> fckingUnsorted) {
            return fckingUnsorted.OrderBy(x => x.Title).ToList();
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
                if(i < 5) {
                    i++;
                }else {
                    if(key == "btn_save") {
                        break;
                    }
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
                if (i < 5) {
                    i++;
                } else {
                    if(key == "btn_save") {
                        break;
                    }
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

        public ActionResult deleteVisit(int id) {
            Visits visit = db.Visits.Find(id);

            var delete = db.VisitTasks.Where(x => x.VisitId == id);
            foreach (var row in delete) {
                db.VisitTasks.Remove(row);
            }

            db.Visits.Remove(visit);
            db.SaveChanges();

            return Redirect("/Visits/Index");
        }

        /**
         * loads the partial view to select a customer
         * @return PartialView 
         */
        public ActionResult _CustomerPicker() {
            var customers = db.Customers;
            IEnumerable<CustomerPicker> picker = (from c in customers
                                                  where c.isActive == true
                                                  orderby c.LName
                                                  select new CustomerPicker {
                                                      lName = c.LName,
                                                      fName = c.FName,
                                                      customerId = c.CustomerId
                                                  }).ToList();
            return PartialView(picker);
        }

        /**
         * loads partial view for a treatment
         * @param int id: Id of the treatment to load
         * @param bool sensitive: include sensitive steps 
         * @return PartialView
         */
        public ActionResult _TreatmentForm(int id, bool sensitive) {
            Treatments treatment = db.Treatments.Find(id);
            VisitTreatment model = new VisitTreatment();
            model.name = treatment.Title;
            model.treatmentID = treatment.TreatmentId;
            model.allowSensitive = sensitive;
            VisitTasks vt;
            foreach(TreatmentSteps s in treatment.TreatmentSteps) {
                if ((s.Steps.isSensitive && sensitive) || !s.Steps.isSensitive) {
                    vt = new VisitTasks();
                    vt.StepId = s.StepId;
                    vt.TreatmentSteps = s;
                    vt.TreatmentId = treatment.TreatmentId;
                    model.tasks.Add(vt);
                    model.possibleTasks.Add(vt.TreatmentSteps);
                }
            }
            return PartialView(model);
        }

        /**
         * checks if given customer allows sensitive data
         * @param int customerId: id of the customer
         * @return bool 
         */
        public bool getCustomerSensitive(int customerId) {
            Customers c = db.Customers.Find(customerId);
            return c.allowSensitive;
        }

        /**
         * checks the sensitive data settings of 2 customers, returns message including the treatment steps that have to be removed/added
         * in case the settings are different ( ͡° ͜ʖ ͡°)
         * @param int cusId1: id of the first customer
         * @param int cusId2: id of the second customer
         * @param int visitId: id of the visit (visit has to be saved to change customer)
         */
        public string checkCustomerSwitch(int cusId1, int cusId2, int visitId) {
            if(visitId == 0) {
                return makeAlert("Der Besuch mus vor der &Auml;nderung des Kunden abgespeichert werden!<br /> Bitte speichern Sie den Besuch ab und veruchen es erneut!", "Speichern!", "danger");
            }
            Customers c1 = db.Customers.Find(cusId1);
            Customers c2 = db.Customers.Find(cusId2);
            string btn = String.Format("<button type='button' class='btn btn-primary' onclick=\"changeCustomer('{0}', '{1}', '{2}', true)\">Kunden &auml;ndern!</button>",
                c2.CustomerId, c2.FName, c2.LName);
            Visits v = db.Visits.Find(visitId);
            string res = "";

            if(c1.allowSensitive != c2.allowSensitive) {
                res = makeAlert("Die Freigabe der Sensitiven Daten zwischen den Kunden ist unterschiedlich!", "Achtung!", "danger");
                List<TreatmentSteps> diff = new List<TreatmentSteps>();
                foreach (VisitTreatment vt in getVisitTreatments(v)) {
                    diff.AddRange(vt.getSensitiveTasks());
                }
                if (c1.allowSensitive && !c2.allowSensitive) {
                    res += "Wenn Sie den Kunden &auml;ndern, werden folgende Schritte aus den Formularen entfernt:<br /><ul>";
                    
                }else {
                    res += "Wenn Sie den Kunden &auml;ndern, werden folgende Schritte zu dem Formularen hinzugef&uuml;gt:<br /><ul>";
                }
                foreach (TreatmentSteps ts in diff) {
                    res += "<li>" + ts.Steps.Title + "</li>";
                }
                res += "</ul>";
            }else {
                return "ok";
            }
            
            res += btn;
            return res;
        }

        private string makeAlert(string msg, string label, string type) {
            string res = String.Format("<div class='alert alert-{0}'><strong>{1}</strong> {2}</div>", type, label, msg);
            return res;
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

        [ChildActionOnly]
        public ActionResult GraphicsView(int? id)
        {
            PicturesVM pics = new PicturesVM();
            if (id.HasValue)
            {
                pics.visitId = id.Value;
                pics.Pictures = db.Pictures.Where(x => x.VisitId == id).ToList();

            }
            return PartialView(pics);
        }
    }
}
