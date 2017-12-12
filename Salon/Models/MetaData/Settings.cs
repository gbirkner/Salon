using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{

    public class SettingsMetaData
    {
        [Key, Display(Name ="Einstellungsname")]
        public int SettingID { get; set; }

        [Display(Name = "Einstellungswert")]
        public short AnonymizeUserByDays { get; set; }

        [Display(Name = "Beschreibung"), DataType(DataType.MultilineText)]
        public short AnonymizeCustomerByDays { get; set; }
    }

    [MetadataType(typeof(SettingsMetaData))]
    public partial class Settings
    {

    }
}