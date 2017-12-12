using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{

    public class SettingsMetaData
    {
        [Key, Display(Name ="EinstellungsID")]
        public string SettingID { get; set; }

        [Display(Name = "Tage, nachdem User automatisch anonymisiert werden.")]
        public string AnonymizeUserByDays { get; set; }

        [Display(Name = "Tage, nachdem Kunden automatisch anonymisiert werden.")]
        public string AnonymizeCustomerByDays { get; set; }
    }

    [MetadataType(typeof(SettingsMetaData))]
    public partial class Settings
    {

    }
}