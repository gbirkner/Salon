using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salon.Controllers
{

    public class ImportController : Controller
    {
        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportUser(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                StreamReader csvreader = new StreamReader(file.InputStream);

                while (!csvreader.EndOfStream)
                {
                    var line = csvreader.ReadLine();
                    var values = line.Split(';');

                    foreach(var value in values)
                    {

                    }
                }

            }

            return View();

        }
    }
}