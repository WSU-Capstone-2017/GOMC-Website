using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Formatting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Controllers;
using Project.Data;
using Project.LoginSystem;
using Project.Models.LoginSystem;

namespace Project.Controller
{
	[TestClass]
	public class LogincControllerTests
	{
        [TestMethod]
        public void ValidEmailAndPassword()
        {
            var mk = new ProjectDbContext.MockType
            {
                UserLogins = new[]
                {
                    new UserLoginModel
                    {
                        Email = "test@email.com", PasswordHash = LoginManager.GetHash("password")
                    }
                }
            };

            var lm = new LoginController(() => ProjectDbContext.Mock(mk));
            var map = new Dictionary<string, string>
            {
                {"uName", "test@email.com" },
                {"pCode", "password" }
            };

            var form = new FormDataCollection(map);
            var ret = lm.ValidateLogin(form);
            Debug.Assert(ret?.ResultType == LoginResultType.Success);
        }
	}
}
