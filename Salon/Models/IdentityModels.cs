using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    // Sie können Profildaten für den Benutzer durch Hinzufügen weiterer Eigenschaften zur ApplicationUser-Klasse hinzufügen. Weitere Informationen finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=317594".
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Vorname")]
        public string firstName { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Nachname")]
        public string lastName { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Klasse")]
        public string Class { get; set; }

        [Display(Name = "Eintrittsdatum")]
        public DateTime? entryDate { get; set; }

        [Display(Name = "Austrittdatum")]
        public DateTime? resignationDate { get; set; }

        [StringLength(20)]
        [Display(Name = "Schülerkennzahl")]
        public string studentNumber { get; set; }

        public bool ChangedPassword { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("IdentityConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Salon.Models.CustomerViewModel> CustomerViewModels { get; set; }

        /*public System.Data.Entity.DbSet<Salon.Models.CustomerViewModel> CustomerViewModels { get; set; }

        public System.Data.Entity.DbSet<Salon.Models.VisitViewModel> VisitViewModels { get; set; }*/
    }
}