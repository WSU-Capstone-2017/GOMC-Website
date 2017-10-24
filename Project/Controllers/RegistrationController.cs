
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

		    var name = dict.GetValue("gomc_downloads_registration_name");
		    var email = dict.GetValue("gomc_downloads_registration_email");
		    var affiliation = dict.GetValue("gomc_downloads_registration_affiliation");
		    var title = dict.GetValue("gomc_downloads_registration_title");
		    var text = dict.GetValue("gomc_downloads_registration_text");

		    var result = new RegistrationResult();


			if (name == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingName);
			}
		    if (email == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingEmail);
			}
		    if (affiliation == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingAffiliation);
			}
		    if (title == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingTitle);
			}
		    if (text == null)
		    {
			    result.ErrorResult(RegistrationErrorType.MissingText);
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
			    Title = title,
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