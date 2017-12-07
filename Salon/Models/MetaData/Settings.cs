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
        public string SettingID { get; set; }

        [Display(Name = "Einstellungswert")]
        public string SettingValue { get; set; }

        [Display(Name = "Beschreibung"), DataType(DataType.MultilineText)]
        public string SettingDescription { get; set; }
    }

    [MetadataType(typeof(SettingsMetaData))]
    public partial class Settings
    {

    }
}