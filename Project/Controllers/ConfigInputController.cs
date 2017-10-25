using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Project.ConfigInput;
using Project.Core;
using Project.Models.Gomc;

namespace Project.Controllers
{
	public class ConfigInputController : ApiController
	{
		private const string ApplicationOctetStream = "application/octet-stream";

		private static readonly Dictionary<Guid, ConfigInputModel> tempModelMap = new Dictionary<Guid, ConfigInputModel>();

		public Guid FormPost(FormDataCollection formDataCollection)
		{
			var formDataConv = new ConfigFormDataConvertor(formDataCollection.ToDictionary(j => j.Key, j => j.Value));

			var model = formDataConv.Convert();

			if (model == null)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
					formDataConv.GetErrorMessage()
				));
			}

			var validator = new Validator(model);

			if (!validator.IsValid())
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
					validator.GetErrorMessage()
				));
			}

			var guid = Guid.NewGuid();

			tempModelMap.Add(guid, model);

			return guid;
		}

		[HttpGet]
		public HttpResponseMessage DownloadFromGuid(Guid guid)
		{
			var model = tempModelMap.GetValue(guid);

			if (model == null)
			{

				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
					"The input guid is invalid."
				));
			}
			var ms = new MemoryStream();
			model.ToXml(ms);


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