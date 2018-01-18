using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Salon.Helpers;
using Salon.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Salon.Controllers
{

    public class ImportController : Controller
    {
        List<string> usernamelist = new List<string>();
        private ApplicationDbContext db = new ApplicationDbContext();

        public ImportController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        public ImportController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }

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
                "text/csv"
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
                    StreamReader csvreader = new StreamReader(model.CSVUpload.InputStream);
                    List<UserCSV> users = new List<UserCSV>();
                    string headerLine = csvreader.ReadLine();

                    while (!csvreader.EndOfStream)
                    {
                        string line = csvreader.ReadLine();
                        string[] values = line.Split(';');
                        string username = GenerateUsername(values[1], values[2]);
                        string password = Membership.GeneratePassword(8, 3);
                        users.Add(new UserCSV(values[0], values[1], values[2], values[3], values[4], values[5], username, password));

                        foreach (UserCSV user in users)
                        {

                            ApplicationUser oldUser = db.Users.Where(x => x.studentNumber == user.StudentNumber).First();

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
                                    UserManager.Update(oldUser);
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
                                    var result = UserManager.AddToRole(newUser.Id, "Schüler");
                                }
                            }
                        }
                    }

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

           string FName = replaceGermanUmlauts(FirstName);
           string LName = replaceGermanUmlauts(LastName);

            FName = FName.ToLower();
            LName = LName.ToLower();

            if (LName.Length > 8){
               LName = LName.Substring(0, 8);
              }

            do
            {
                lengh++;
                FName = FName.Substring(0, lengh);
                username = FName + LName;

                found = usernamelist.Contains(username);

            } while (found && lengh < FName.Length);

            if (found == true){
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
            return Text;
        }
    }
}