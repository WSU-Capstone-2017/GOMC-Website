using System;
using System.Web.Http;
using Project.Data;
using Project.Latex;
using Project.Models;

namespace Project.Controllers
{
	public class LatexController : ApiController
	{
		public LatexConvertResult Convert(string file, string version, Guid session)
		{
			using(var db = new ProjectDbContext())
			{
				foreach(var s in db.AlreadyLoggedIns)
				{
					if(s.Session == session)
					{
						if(DateTime.Now >= s.Expiration)
						{
							db.AlreadyLoggedIns.Remove(s);
							db.SaveChanges();

							return LatexConvertResult.SessionExpired;
						}

						var conv = new LatexConvertor();

						var convRes = conv.Convert(file);

						if(convRes != ConversionResult.Success)
						{
							return LatexConvertResult.InvalidFormat;
						}

						var model = new LatexUploadModel
						{
							AuthorId = s.LoginId,
							Version = version,
							HtmlZip = conv.HtmlZip,
							Pdf = conv.Pdf
						};

						db.LatexUploads.Add(model);

						return LatexConvertResult.Success;
					}
				}

				return LatexConvertResult.BadSession;
			}
		}
	}
}