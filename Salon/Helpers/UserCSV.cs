using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.Helpers
{
    public class UserCSV
    {
        public userData( string nKlasse, string nFirstName, string nLastName)
        {
            Klasse = nKlasse;
        }

        public string Klasse { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MyProperty { get; set; }
    }
}