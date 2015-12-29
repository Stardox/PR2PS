using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PR2PS.Web.Core
{
    public static class HttpResponseFactory
    {
        public static HttpResponseMessage Response200Plain(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.MIME_TEXT_PLAIN);
            return msg;
        }

        public static HttpResponseMessage Response200JSON(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.MIME_TEXT_JSON);
            return msg;
        }

        public static HttpResponseMessage Response200XML(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.MIME_TEXT_XML);
            return msg;
        }

        public static HttpResponseMessage Response500Plain(String content)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            msg.Content = new StringContent(content);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.MIME_TEXT_PLAIN);
            return msg;
        }
    }
}
