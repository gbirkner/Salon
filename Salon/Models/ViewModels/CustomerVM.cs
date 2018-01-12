using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Salon.Models
{
    public class CustomerViewModel
    {
        //[Display(Name = "Land")]
        //[StringLength(255, MinimumLength = 1)]
        //public string CountryId { get; set; }

        [Display(Name = "PLZ")]
        [StringLength(255, MinimumLength = 1)]
        public string PostalCode { get; set; }

        [Key]
        [Required]
        public int CustomerId { get; set; }

        [Display(Name = "Vorname")]
        [StringLength(255)]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        [StringLength(255, MinimumLength = 1)]
        public string LName { get; set; }

        [Display(Name = "Straße")]
        [StringLength(255)]
        public string Street { get; set; }

        [Display(Name = "Stadt")]
        [StringLength(255, MinimumLength = 1)]
        public string CityName { get; set; }

        [Display(Name = "Land")]
        [StringLength(255, MinimumLength = 1)]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Aktiv")]
        public bool isActive { get; set; }

        [Required]
        [Display(Name = "Geschlecht")]
        [StringLength(20, MinimumLength = 1)]
        public string GenderTitle { get; set; }

        [Required]
        [Display(Name = "Geschlecht")]
        public int GenderID { get; set; }

        [Display(Name = "Stadt")]
        public int? CityId { get; set; }

        [Display(Name = "Beschreibung")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Bilder")]
        public bool allowImages { get; set; }

        [Required]
        [Display(Name = "sensible Daten")]
        public bool allowSensitive { get; set; }

        [Display(Name = "geändert von")]
        [StringLength(128)]
        public string ModifiedBy { get; set; }

        [Required]
        [Display(Name = "geändert")]
        public System.DateTime Modified { get; set; }

        [StringLength(128)]
        [Display(Name = "erstellt von")]
        public string CreatedBy { get; set; }

        [Required]
        [Display(Name = "erstellt")]
        public System.DateTime Created { get; set; }



        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }



        public virtual List<Cities> Cities { get; set; }

        public virtual List<Genders> Genders { get; set; }

        public virtual List<Countries> Countries { get; set; }


        public List<Genders> fillGender()
        {
            List<Genders> genderList = new List<Genders>();

            using (var context = new SalonEntities())
            {
                var genders = context.Genders;

                foreach(var item in genders)
                {
                    genderList.Add(new Genders() { GenderID = item.GenderID, Customers = item.Customers, GenderTitle = item.GenderTitle });
                }
            }
            return genderList;
        }

        public List<Countries> fillCountries()
        {
            List<Countries> countryList = new List<Countries>();

            using (var context = new SalonEntities())
            {
                var countries = context.Countries;

                foreach (var item in countries)
                {
                    countryList.Add(new Countries() { CountryId = item.CountryId,  Title = item.Title });
                }
            }
            return countryList;
        }

        public List<Cities> fillCities()
        {
            List<Cities> cityList = new List<Cities>();

            using (var context = new SalonEntities())
            {
                var cities = context.Cities;

                foreach (var item in cities)
                {
                    cityList.Add(new Cities() { CityId = item.CityId, Title = item.Title, CountryId = item.CountryId });
                }
            }
            return cityList;
        }
    }
}