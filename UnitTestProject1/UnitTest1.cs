using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salon.Controllers;
using System.Web.Mvc;

namespace UnitTestProject1 {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestIndex() {
            var controller = new VisitsController();
            var res = controller.Index(null, null) as ViewResult;

        }
    }
}
