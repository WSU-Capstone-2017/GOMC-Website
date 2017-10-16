using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Project.ConfigInput;

namespace Project.Controllers
{
	public class ConfigInputController : ApiController
	{
		private const string ApplicationOctetStream = "application/octet-stream";

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