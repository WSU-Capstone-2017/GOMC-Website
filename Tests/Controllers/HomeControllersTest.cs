
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Project.Controllers
{
    [TestClass]
    class HomeControllersTest
    {
        [TestMethod]
        public void GetDownloadModelNullTest()
        {
            var homeController = new HomeController();
            var model = homeController.GetDownloadModel();
            Assert.IsNotNull(model);
        }
    }
}
