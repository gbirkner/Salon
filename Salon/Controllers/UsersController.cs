using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Salon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System.Text;
using Salon.Helpers;
using System.IO;
using System.Web.Security;

namespace Salon.Controllers
{
    [Authorize(Roles ="Admin, Lehrer")]
    public class UsersController : Controller
    {

        /// <summary>
        /// Manage user and add them to roles
        /// </summary>

        private ApplicationDbContext db = new ApplicationDbContext();
        private SalonEntities salondb = new SalonEntities();

        public UsersController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }

        // GET: User
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.Include(x => x.Roles).ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        public ActionResult _ResetPassword(string id)
        {
            ApplicationUser user = UserManager.FindById(id);
            if (user != null)
            {
                string NewPassword = System.Web.Security.Membership.GeneratePassword(8, 3);
                ViewBag.NewPassword = NewPassword;

                var provider = new DpapiDataProtectionProvider("Salon");
                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("Create"));

                string resetToken = UserManager.GeneratePasswordResetToken(user.Id);
                IdentityResult passwordChangeResult = UserManager.ResetPassword(user.Id, resetToken, NewPassword);

                user.ChangedPassword = false;
                UserManager.Update(user);

                if (passwordChangeResult == IdentityResult.Success)
                {
                    salondb.InsertLog("_ResetPassword", "UsersController", User.Identity.GetUserId());
                    return PartialView("_ResetPasswordSuccess");
                }
            }
            return PartialView("_ResetPasswordFailure");
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, string RoleId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = userViewModel.UserName;
                user.firstName = userViewModel.firstName;
                user.lastName = userViewModel.lastName;
                user.ChangedPassword = false;
                var userresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User Admin to Role
                if (userresult.Succeeded)
                {
                    if (!String.IsNullOrEmpty(RoleId))
                    {
                        //Find Role
                        var role = await RoleManager.FindByIdAsync(RoleId);
                        var result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().ToString());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", userresult.Errors.First().ToString());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                    return View();
                }
                salondb.InsertLog("Create", "UsersController", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            bool AllowEdit;
            AllowEdit = User.IsInRole("Admin");
            if (!AllowEdit)
                AllowEdit = User.IsInRole("Lehrer") && user.Roles.Count(x => x.RoleId == "2") == 1;

            if (AllowEdit)
            {
                var userRoles = UserManager.FindById(user.Id).Roles.ToList();
                if (userRoles != null && userRoles.Count() > 0)
                {
                    ViewBag.Roles = new SelectList(RoleManager.Roles, "Id", "Name", userRoles != null ? userRoles[0].RoleId : null);
                }
                else
                    ViewBag.Roles = new SelectList(RoleManager.Roles, "Id", "Name");

                Response.AddHeader("auth", "1");
                return View(user);
            }
            else
                return View("NotAuthorized");            
    }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserName,Id,firstName,lastName,Class,entryDate,resignationDate,Email")] ApplicationUser formuser, string id, string RoleId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
            var user = await UserManager.FindByIdAsync(id);
            user.UserName = formuser.UserName;
            if (ModelState.IsValid)
            {
                //Update the user details
                await UserManager.UpdateAsync(user);

                //If user has existing Role then remove the user from the role
                // This also accounts for the case when the Admin selected Empty from the drop-down and
                // this means that all roles for the user must be removed
                var rolesForUser = await UserManager.GetRolesAsync(id);
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser)
                    {
                        var result = await UserManager.RemoveFromRoleAsync(id, item);
                    }
                }

                if (!String.IsNullOrEmpty(RoleId))
                {
                    //Find Role
                    var role = await RoleManager.FindByIdAsync(RoleId);
                    //Add user to new role
                    var result = await UserManager.AddToRoleAsync(id, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                        return View();
                    }
                }
                salondb.InsertLog("Edit", "UsersController", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

        List<string> usernamelist = new List<string>();
        int succeeded = 0;

        // GET: Import
        [HttpGet]
        public ActionResult Import()
        {
            return View(new UserCSVViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Import(UserCSVViewModel model)
        {
            var validImportTypes = new string[]
           {
                "text/csv",
                "application/vnd.ms-excel"
           };

            if (model.CSVUpload == null || model.CSVUpload.ContentLength == 0)
            {
                ModelState.AddModelError("CSVUpload", "This field is required");
            }
            else if (!validImportTypes.Contains(model.CSVUpload.ContentType))
            {
                ModelState.AddModelError("CSVUpload", "Please choose a CSV file.");
            }

            if (ModelState.IsValid)
            {

                if (model.CSVUpload != null && model.CSVUpload.ContentLength > 0)
                {
                    StreamReader csvreader = new StreamReader(model.CSVUpload.InputStream, System.Text.Encoding.UTF8);
                    List<UserCSV> users = new List<UserCSV>();
                    string headerLine = csvreader.ReadLine();

                    var oldusernames = from u in db.Users
                                       where u.UserName != null
                                       select u.UserName;

                    if (oldusernames != null)
                    {
                        foreach (string oldusername in oldusernames)
                        {
                            usernamelist.Add(oldusername);
                        }
                    }

                    while (!csvreader.EndOfStream)
                    {
                        string line = csvreader.ReadLine();
                        string[] values = line.Split(';');
                        string username = GenerateUsername(values[2], values[1]);
                        string password = Membership.GeneratePassword(8, 3);
                        users.Add(new UserCSV(values[0], values[2], values[1], values[3], values[4], values[5], username, password));
                    }

                    foreach (UserCSV user in users)
                    {

                        ApplicationUser oldUser = db.Users.Where(x => x.studentNumber == user.StudentNumber).FirstOrDefault();

                        if (oldUser != null)
                        {
                            oldUser.Class = user.Class;
                            oldUser.entryDate = DateTime.Parse(user.EntryDate);
                            oldUser.resignationDate = DateTime.Parse(user.ResignationDate);
                            oldUser.ChangedPassword = false;

                            string resetToken = await UserManager.GeneratePasswordResetTokenAsync(oldUser.Id);
                            IdentityResult passwordChangeResult = await UserManager.ResetPasswordAsync(oldUser.Id, resetToken, user.Password);

                            if (passwordChangeResult.Succeeded)
                            {
                                var userResult = UserManager.Update(oldUser);

                                if (userResult.Succeeded)
                                {
                                    succeeded += 1;
                                }
                            }
                        }
                        else
                        {
                            var newUser = new ApplicationUser();
                            newUser.firstName = user.FirstName;
                            newUser.lastName = user.LastName;
                            newUser.UserName = user.UserName;
                            newUser.Class = user.Class;
                            newUser.studentNumber = user.StudentNumber;
                            newUser.ChangedPassword = false;
                            newUser.entryDate = DateTime.Parse(user.EntryDate);
                            newUser.resignationDate = DateTime.Parse(user.ResignationDate);
                            var userResult = UserManager.Create(newUser, user.Password);

                            //Add User Role Applicant
                            if (userResult.Succeeded)
                            {
                                var roleResult = UserManager.AddToRole(newUser.Id, "Schueler");
                                if (roleResult.Succeeded)
                                {
                                    succeeded += 1;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    Export(users);
                    return RedirectToAction("Index");

                }
            }

            return View(model);

        }
        [NonAction]
        private string GenerateUsername(string FirstName, string LastName)
        {

            int lengh = 0;
            bool found = false;
            string username = null;
            int index = 0;

            string FName = FirstName.ToLower();
            string LName = LastName.ToLower();

            FName = replaceGermanUmlauts(FName);
            LName = replaceGermanUmlauts(LName);

            if (LName.Length > 8)
            {
                LName = LName.Substring(0, 8);
            }

            do
            {
                lengh++;
                FName = FName.Substring(0, lengh);
                username = FName + LName;

                found = usernamelist.Contains(username);

            } while (found && lengh < FName.Length);

            if (found == true)
            {
                do
                {
                    index++;
                    username = FName + LName + index;
                    found = usernamelist.Contains(username);
                } while (found == true);
            }

            usernamelist.Add(username);

            return username;
        }

        [NonAction]
        private string replaceGermanUmlauts(string Text)
        {
            Text = Text.Replace("ä", "ae");
            Text = Text.Replace("ü", "ue");
            Text = Text.Replace("ö", "oe");
            Text = Text.Replace("ß", "ss");
            Text = Text.Replace(" ", "");
            Text = Text.Replace("é", "e");
            Text = Text.Replace("è", "e");
            return Text;
        }

        private void Export(List<UserCSV> exportUserList)
        {

            string userExportCsv = GetCsvString(exportUserList);

            // Return the file content with response body. 
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=Schuelerliste.csv");
            Response.Write(userExportCsv);
            Response.End();
        }

        private string GetCsvString(List<UserCSV> userExportCsvList)
        {
            StringBuilder csv = new StringBuilder();

            csv.AppendLine("Klasse;Familienname;Vorname;Benutzername;Passwort");

            foreach (UserCSV user in userExportCsvList)
            {
                csv.Append(user.Persist);
                csv.AppendLine();
            }

            return csv.ToString();
        }
    }
}