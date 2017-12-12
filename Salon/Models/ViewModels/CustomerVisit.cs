using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class CustomerViewModel
    {
        [Key]
        public string CountryId { get; set; }

        [Display(Name = "PLZ")]
        public string PostalCode { get; set; }

        public int CustomerId { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        public string Street { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }

        public bool isActive { get; set; }
        public int GenderID { get; set; }

        public string Description { get; set; }

        public bool allowImages { get; set; }

        public bool allowSensitive { get; set; }

        public string ModifiedBy { get; set; }

        public System.DateTime Modified { get; set; }

        public string CreatedBy { get; set; }

        public System.DateTime Created { get; set; }



        /*public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        public virtual Cities Cities { get; set; }*/
    }

    public class VisitViewModel
    {
        [Key]
        public int VisitId { get; set; }

        public int Duration { get; set; }

        public Nullable<int> CustomerId { get; set; }

        public string ModifiedBy { get; set; }

        public System.DateTime Modified { get; set; }

        public string CreatedBy { get; set; }

        public System.DateTime Created { get; set; }
    }

    public class ConnectionViewModel
    {
        public int ConnectionId { get; set; }

        public int ConnectionTypeId { get; set; }

        public int CustomerId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }
        



        /*public virtual ConnectionTypes ConnectionTypes { get; set; }

        public virtual Customers Customers { get; set; }*/
    }
}