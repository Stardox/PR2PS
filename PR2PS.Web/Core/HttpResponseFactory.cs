using Newtonsoft.Json;
using PR2PS.Common.Constants;
using PR2PS.Web.Core.JsonModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PR2PS.Web.Core
{
    public static class HttpResponseFactory
    {
        public static HttpResponseMessage Response200Plain(String key, String value)
        {
            return Response200Plain(String.Concat(key, Separators.EQ_CHAR, value));
        }

        public static HttpResponseMessage Response200Plain(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.MIME_TEXT_PLAIN);
            return msg;
        }

        public static HttpResponseMessage Response200Json(IJsonModel model)
        {
            return Response200Json(JsonConvert.SerializeObject(model));
        }

        public static HttpResponseMessage Response200Json(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.MIME_TEXT_JSON);
            return msg;
        }

        public static HttpResponseMessage Response200Xml(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.MIME_TEXT_XML);
            return msg;
        }

        public static HttpResponseMessage Response500Plain(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.MIME_TEXT_PLAIN);
            return msg;
        }
    }
}
