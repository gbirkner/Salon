using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Salon.Models
{

    public class SettingsMetaData
    {
        [Key, Display(Name ="EinstellungsID")]
        public string SettingID { get; set; }

        [Display(Name = "Auto. Anonymisierung Benutzer", Description = "Benutzer, die älter sind als x Tage werden automatisch anonymisiert")]
        [Range(90, 3650)]
        public short AnonymizeUserByDays { get; set; }

        [Display(Name = "Auto. Anonymisierung Kunden", Description = "Kunden, die seit x Tagen keine Besuche mehr haben werden automatisch anonymisiert")]
        [Range(90, 3650)]
        public short AnonymizeCustomerByDays { get; set; }

        [Display(Name = "Auto. Löschung Benutzer", Description = "Benutzer, die älter sind als x Tage werden automatisch gelöscht")]
        [Range(90, 3650)]
        [DeleteUserValidation]
        public short DeleteUserByDays { get; set; }

        [Display(Name = "Auto. Löschung Kunden", Description = "Kunden, die seit x Tagen keine Besuche mehr haben werden automatisch gelöscht")]
        [Range(90, 3650)]
        [DeleteCustomerValidation]
        public short DeleteCustomerByDays { get; set; }
    }

    [MetadataType(typeof(SettingsMetaData))]
    public partial class Settings
    {

    }

    internal class DeleteUserValidation : ValidationAttribute //, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Settings Setting = (Settings)validationContext.ObjectInstance;

            if (Setting.AnonymizeUserByDays > Setting.DeleteUserByDays)
            {
                return new ValidationResult("Das automatische Löschen darf nicht vor dem Anonymisieren erfolgen.");
            }

            return ValidationResult.Success;
        }
    }

    internal class DeleteCustomerValidation : ValidationAttribute //, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Settings Setting = (Settings)validationContext.ObjectInstance;

            if (Setting.AnonymizeCustomerByDays > Setting.DeleteCustomerByDays)
            {
                return new ValidationResult("Das automatische Löschen darf nicht vor dem Anonymisieren erfolgen.");
            }

            return ValidationResult.Success;
        }
    }
}