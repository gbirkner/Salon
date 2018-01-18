using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    public class UserCSVViewModel
    {
        [DataType(DataType.Upload)]
       public HttpPostedFileBase CSVUpload { get; set; }
    }
}