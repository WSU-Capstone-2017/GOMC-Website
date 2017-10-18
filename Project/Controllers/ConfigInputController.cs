using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Project.ConfigInput;

namespace Project.Controllers
{
	public class ConfigInputController : ApiController
	{
		private const string ApplicationOctetStream = "application/octet-stream";

		[HttpPost]
		public HttpResponseMessage PostInput(FormDataCollection formDataCollection)
		{
			var txt = formDataCollection.Select(j => $"{j.Key}={j.Value}").Aggregate("", (a, b) => a += b + "\n");
			var model = ConfigInputModel.FromFormData(formDataCollection.ToDictionary(j => j.Key, j => j.Value));

			if(model == null || !Validator.IsValid(model))
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			var ms = new MemoryStream();

			try
			{
				model.ToXml(ms);
			}
			catch
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			var result = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(ms.ToArray())
			};
			result.Content.Headers.ContentDisposition =
				new ContentDispositionHeaderValue("attachment")
				{
					FileName = "input.xml"
				};
			result.Content.Headers.ContentType =
				new MediaTypeHeaderValue(ApplicationOctetStream);

			return result;
		}

		[HttpGet]
		public HttpResponseMessage ValidateConfigInput(string jsonInput)
		{
			var model = ConfigInputModel.FromJson(jsonInput);

			if(model == null || !Validator.IsValid(model))
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			var ms = new MemoryStream();

			try
			{
				model.ToXml(ms);
			}
			catch
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			var result = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(ms.ToArray())
			};
			result.Content.Headers.ContentDisposition =
				new ContentDispositionHeaderValue("attachment")
				{
					FileName = "input.xml"
				};
			result.Content.Headers.ContentType =
				new MediaTypeHeaderValue(ApplicationOctetStream);

			return result;
		}
	}
}