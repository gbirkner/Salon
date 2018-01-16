using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    [MetadataType(typeof(ConnectionTypes))]
    public class ConnectionTypesMetaData
    {
        [Key]
        public int ConnectionTypeId;

        [Required]
        [Display(Name = "Connectiontype", ShortName = "C-Type")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "")]
        public string Title;

        [StringLength(500, ErrorMessage = "Die Beschreibung darf max. 500 Zeichen lang sein.")]
        public string Description;
    }

    public partial class ConnectionTypes
    {
    }
}