
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
		[HttpPost]
	    public RegistrationResult Input(FormDataCollection formData)
	    {
		    var dict = formData.ToDictionary(j => j.Key, j => j.Value);

		    var name = dict.GetValue("gomc_downloads_registration_name");
		    var email = dict.GetValue("gomc_downloads_registration_email");
		    var affiliation = dict.GetValue("gomc_downloads_registration_affiliation");
		    var title = dict.GetValue("gomc_downloads_registration_tile");
		    var text = dict.GetValue("gomc_downloads_registration_text");

		    if(name == null)
		    {
			    return RegistrationResult.ErrorResult(RegistrationErrorType.MissingName);
			}
		    if (email == null)
		    {
			    return RegistrationResult.ErrorResult(RegistrationErrorType.MissingEmail);
			}
		    if (affiliation == null)
		    {
			    return RegistrationResult.ErrorResult(RegistrationErrorType.MissingAffiliation);
			}
		    if (title == null)
		    {
			    return RegistrationResult.ErrorResult(RegistrationErrorType.MissingTitle);
			}
		    if (text == null)
		    {
			    return RegistrationResult.ErrorResult(RegistrationErrorType.MissingText);
			}
		    if (!LoginSystem.LoginManager.IsValidEmail(email))
		    {
			    return RegistrationResult.ErrorResult(RegistrationErrorType.EmailInvalidFormat);
		    }

		    var model = new RegistrationModel
		    {
			    Name = name,
			    Email = email,
			    Affiliation = affiliation,
			    Title = title,
			    Text = text
		    };

		    var result = new RegistrationResult
		    {
			    Model = model,
			    ErrorType = RegistrationErrorType.Success
		    };

		    using(var db = new ProjectDbContext())
		    {
			    db.Registrations.Add(model);
			    db.SaveChanges();
		    }

		    return result;
	    }
    }
}