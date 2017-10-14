using Project.Models;
using Project.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Project.Controllers
{
    [TestClass]
    public class HomeControllersTest
    {
        [TestMethod]
        public void GetDownloadModelNullTest()
        {
            var hc = new HomeController();
            var model = hc.GetDownloadModel();
            Assert.IsNotNull(model);
        }
        [TestMethod]
        public void DownloadsActionDoesNotReturnNullModel()
        {
            var hc = new HomeController();
            var actionResult = hc.Downloads();           
            var model2 = Utils.ModelFromActionResult<DownloadsModel>(actionResult);
            Assert.IsNotNull(model2);
        }      
        [TestMethod]
        public void ContactActionDoesNotReturnNullModel()
        {
            var hc = new HomeController();
            var actionResult = hc.Contact();
            var model3 = Utils.ModelFromActionResult<HomeModel>(actionResult);
            Assert.IsNotNull(model3);
        }
    }
}
