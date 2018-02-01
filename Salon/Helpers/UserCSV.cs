using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.Helpers
{
    public class UserCSV
    {
        public UserCSV( string nClass, string nFirstName, string nLastName, string nEntryDate, string nResignationDate, string nStudentNumber, string nUserName, string nPassword)
        {
            Class = nClass;
            FirstName = nFirstName;
            LastName = nLastName;
            EntryDate = nEntryDate;
            ResignationDate = nResignationDate;
            StudentNumber = nStudentNumber;
            UserName = nUserName;
            Password = nPassword;
        }

        public string Class { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EntryDate { get; set; }
        public string ResignationDate { get; set; }
        public string StudentNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Persist {
            get {
                return Class + ";" + LastName + ";" + FirstName + ";" + UserName + ";" + Password + ";";
            }
        }
    }
}