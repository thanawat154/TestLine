using Line.Messaging.Webhooks;
using Line.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using LINE_Webhook.CloudStorage;
using LINE_Webhook.Models;

namespace LINE_Webhook.Controllers
{
    [RoutePrefix("api")]
    public class LineBotController : ApiController
    {

        private static LineMessagingClient lineMessagingClient;
        private string accessToken = ConfigurationManager.AppSettings["ChannelAccessToken"];
        private string channelSecret = ConfigurationManager.AppSettings["ChannelSecret"];
        public LineBotController()
        {

            lineMessagingClient = new LineMessagingClient(accessToken);
        }

    
        public async Task<HttpResponseMessage> WebHook(HttpRequestMessage request)
        {
            var events = await request.GetWebhookEventsAsync(channelSecret);

          
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            //var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");
            //var eventSourceState = await TableStorage<EventSourceState>.CreateAsync(connectionString, "eventsourcestate");

            //var app = new LineBotApp(lineMessagingClient, eventSourceState, blobStorage);
            var app = new LineMsgApp(lineMessagingClient);
            await app.RunAsync(events);

            return Request.CreateResponse(HttpStatusCode.OK);
        }      
    }
}
