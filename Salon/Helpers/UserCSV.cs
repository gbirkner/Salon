using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.Helpers
{
    public class UserCSV
    {
        public void userData( string nClass, string nFirstName, string nLastName, string nEntryDate, string nResignationDate, string nStudentNumber)
        {
            Class = nClass;
            FirstName = nFirstName;
            LastName = nLastName;
            EntryDate = nEntryDate;
            ResignationDate = nResignationDate;
            StudentNumber = nStudentNumber;
        }

        public string Class { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EntryDate { get; set; }
        public string ResignationDate { get; set; }
        public string StudentNumber { get; set; }
    }
}