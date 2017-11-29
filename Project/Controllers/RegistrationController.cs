using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Project.Core;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class RegistrationController : ApiController
    {
		public Func<ProjectDbContext> DbGetter { get; }

	    public RegistrationController() : this(null)
	    {
		    
	    }
	    public RegistrationController(Func<ProjectDbContext> dbGetter)
	    {
		    DbGetter = dbGetter ?? (() => new ProjectDbContext());
	    }

	    public static bool CaptchaCheck(string gRecaptchaResponse)
	    {
		    var userIp = HttpContext.Current.Request.UserHostAddress;

		    var content = new FormUrlEncodedContent(new Dictionary<string, string>
		    {
			    {"secret", "6LdqvTUUAAAAACCEPmAEy_a-p_vt60UYrLMhVaFO"},
			    {"response", gRecaptchaResponse},
			    {"remoteip", userIp}
		    });

		    var msg = MvcApplication.Client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content)
			    .Result.Content.ReadAsStringAsync()
			    .Result;

		    var result = JsonConv.ToObject<Dictionary<string, string>>(msg);

		    var success = result.GetValue("success");

		    return success == "true";
	    }

	    public RegistrationResult Input(FormDataCollection formDataCollection, Func<string, bool> captchaCheckFn = null)
	    {
		    captchaCheckFn = captchaCheckFn ?? CaptchaCheck;

		    var dict = formDataCollection.ToDictionary(j => j.Key, j => j.Value);
		    var result = new RegistrationResult();

			if (!captchaCheckFn(dict.GetValue("g-recaptcha-response")))
			{
				result.ErrorResult(RegistrationErrorType.CaptchaInvalid);
				return result;
			}

		    var name = dict.GetValue("userName");
		    var email = dict.GetValue("userEmail");
		    var affiliation = dict.GetValue("userAffliation");
		    var text = dict.GetValue("extraComment");



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
			    Text = text,
				Created = DateTime.Now
		    };

	        if (result.Success)
	        {
	            using (var db = DbGetter())
	            {
	                db.Registrations.Add(model);
	                db.SaveChanges();
	            }
	        }

	        result.Model = model;

		    return result;
	    }
    }
}