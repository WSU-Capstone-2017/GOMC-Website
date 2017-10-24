
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Project.Core;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class RegistrationController : ApiController
    {
	    public RegistrationResult Input(FormDataCollection formDataCollection)
	    {
		    var dict = formDataCollection.ToDictionary(j => j.Key, j => j.Value);

		    var name = dict.GetValue("userName");
		    var email = dict.GetValue("userEmail");
		    var affiliation = dict.GetValue("userAffliation");
		    // var title = dict.GetValue("gomc_downloads_registration_tile"); // Same as in other places, commented for now
		    var text = dict.GetValue("extraComment");

		    var result = new RegistrationResult();


			if (name == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingName);
			}
		    if (email == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingEmail);
			}

		    if (!LoginSystem.LoginManager.IsValidEmail(email))
		    {
			    result.ErrorResult(RegistrationErrorType.EmailInvalidFormat);
		    }

		    var model = new RegistrationModel
		    {
			    Name = name,
			    Email = email,
			    Affiliation = affiliation,
			   // Title = null,
			    Text = text
		    };

		    using(var db = new ProjectDbContext())
		    {
			    db.Registrations.Add(model);
			    db.SaveChanges();
		    }

		    result.Model = model;

		    return result;
	    }
    }
}