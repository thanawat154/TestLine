using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using LINE_Webhook.Logging;

namespace LINE_Webhook.Filter {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ApiExceptionFilter : ExceptionFilterAttribute {

        public override void OnException(HttpActionExecutedContext context) {
            var exception = context.Exception;
            if (exception != null) {
                Logger.LogError(exception.Message);
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message);
                base.OnException(context);
            }
        }
    }

}