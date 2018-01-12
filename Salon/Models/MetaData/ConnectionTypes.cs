using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{

    public class ConnectionTypesMetaData
    {
        [Key]
        [Display(Name = "Kontakttyp")]
        public int ConnectionTypeId { get; set; }

        [Display(Name = "Kontakttyp")]
        public string Title { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }


        public virtual ICollection<Connections> Connections { get; set; }
    }

    [MetadataType(typeof(ConnectionTypesMetaData))]
    public partial class ConnectionTypes
    {

    }


}