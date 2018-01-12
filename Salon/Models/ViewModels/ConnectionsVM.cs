using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class ConnectionViewModel
    {
        [Key]
        public int ConnectionId { get; set; }

        public int ConnectionTypeId { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Kontakttyp")]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        [Display(Name = "Beschreibung")]
        [StringLength(500, MinimumLength = 1)]
        public string Description { get; set; }

        [Display(Name = "Kontaktdaten")]
        [StringLength(255, MinimumLength = 1)]
        public string Value { get; set; }


        public virtual ConnectionTypes ConnectionTypes { get; set; }

        public virtual Customers Customers { get; set; }
    }
}