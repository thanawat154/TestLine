using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.ModelBinding;
//using LINE_Webhook.Utilities.String;
using LINE_Webhook.Logging;

namespace LINE_Webhook.Helper {

    public static class PageRequest {

        public static HttpResponseMessage Get_ModelError(ModelStateDictionary modelstate) {
            var exception = string.Join(" | ", modelstate.Values.SelectMany(v => v.Errors).Select(e => e.Exception.Message));
            var message = string.Join(" | ", modelstate.Values.SelectMany(v => v.Errors).Select(e => e.Exception.InnerException.Message));
            Logger.LogWarning(exception + Environment.NewLine + message);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(exception + Environment.NewLine + message) };
        }

        public static HttpResponseMessage Get_GenericError(Exception ex)
        {
            var exception = string.Join(" | ", ex.Message);
            var message = string.Join(" | ", ex.InnerException.Message);
            Logger.LogWarning(exception + Environment.NewLine + message);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(exception + Environment.NewLine + message) };
        }

        //public static object Get_AttributeValue<T>(HttpClient client, string route) {

        //    var item = (T)client
        //          .GetAsync(route)
        //          .Result.EnsureSuccessStatusCode()
        //          .Content.ReadAsStringAsync()
        //          .Result.JsonDeserialize<T>();

        //    return item;
        //}

    }
}