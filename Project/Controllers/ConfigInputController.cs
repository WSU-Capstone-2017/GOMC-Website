using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Project.ConfigInput;
using Project.Core;
using Project.Latex;
using Project.LoginSystem;
using Project.Models;
using Project.Models.Gomc;

namespace Project.Controllers
{
	public class ConfigInputController : ApiController
	{
		private const string ApplicationOctetStream = "application/octet-stream";

		private static readonly Dictionary<Guid, ConfigInputModel> tempModelMap = new Dictionary<Guid, ConfigInputModel>();
	    private static readonly Dictionary<Guid, string> tempXmlMap = new Dictionary<Guid, string>();


        [HttpPost]
	    public async Task<string> UploadConfForm()
	    {
	        var root = HttpContext.Current.Server.MapPath("~/temp/uploads");
	        Directory.CreateDirectory(root);
	        var provider = new MultipartFormDataStreamProvider(root);
	        await Request.Content.ReadAsMultipartAsync(provider);

	        var confForm = provider.FormData["confForm"];

	        if (string.IsNullOrEmpty(confForm))
	        {
	            return "MissingConfForm";
	        }

	        if (provider.FileData.Count == 0 || !File.Exists(provider.FileData[0].LocalFileName))
	        {
	            return "MissingFile";
            }

	        var ln = File.ReadAllLines(provider.FileData[0].LocalFileName, Encoding.Default);
	        File.Delete(provider.FileData[0].LocalFileName);
	        var ic = ln
	            .Where(j => j.Length > 0 && j[0] != '#')
	            .Select(j => j.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
	            .ToArray();

            var xm = ic
	            .Select(j =>
	                $"<{j[0]}>{string.Join(";;", j.Skip(1).ToArray())}</<{j[0]}>"
	            ).ToArray();
            var xml = $"<ConfigSetup>\n{xm.Aggregate("", (a, b) => a += (b + "\n"))}</ConfigSetup>";
	        var guid = Guid.NewGuid();
	        tempXmlMap.Add(guid, xml);
            return guid.ToString();
	    }
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
	    public HttpResponseMessage DownloadXmlFromGuid(Guid guid)
	    {
	        var xml = tempXmlMap.GetValue(guid);

	        if (xml == null)
	        {

	            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
	                "The input guid is invalid."
	            ));
	        }
	        var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)); 

	        var result = new HttpResponseMessage(HttpStatusCode.OK)
	        {
	            Content = new ByteArrayContent(ms.ToArray())
	        };
	        result.Content.Headers.ContentDisposition =
	            new ContentDispositionHeaderValue("attachment")
	            {
	                FileName = "in.conf.xml"
	            };

	        result.Content.Headers.ContentType =
	            new MediaTypeHeaderValue(ApplicationOctetStream);

	        return result;
	    }
 
		[HttpPost]
		public Guid PostConfForm(string xml)
		{
			var gd = Guid.NewGuid();
			tempXmlMap.Add(gd, xml);
			return gd;
		}

	    [HttpGet]
	    public HttpResponseMessage DownloadXml(string xml)
	    {
	        if (xml == null)
	        {
	            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
	                "The input xml is invalid."
	            ));
	        }
	        var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));

	        var result = new HttpResponseMessage(HttpStatusCode.OK)
	        {
	            Content = new ByteArrayContent(ms.ToArray())
	        };
	        result.Content.Headers.ContentDisposition =
	            new ContentDispositionHeaderValue("attachment")
	            {
	                FileName = "in.conf.xml"
	            };

	        result.Content.Headers.ContentType =
	            new MediaTypeHeaderValue(ApplicationOctetStream);

	        return result;
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