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

        //private static LineMessagingClient lineMessagingClient;
        //private string accessToken = ConfigurationManager.AppSettings["ChannelAccessToken"];
        //private string channelSecret = ConfigurationManager.AppSettings["ChannelSecret"];
        public LineBotController()
        {

            //lineMessagingClient = new LineMessagingClient(accessToken);
        }

    
        public async Task<HttpResponseMessage> WebHook(string merchantId, HttpRequestMessage request)
        {
            //TODO: need to detect channel access token in DB instead with Async.
            string channelSecret = "";
            string channelAccessToken = "";
            
            switch (merchantId)
            {
                case "1567098166":
                    channelSecret = "9756bda3cef7ec92557e3ed86c347133";
                    channelAccessToken = "FT1trntaKs5+hEAG9Bj+3ispLhNSNBxk1RO9w9q5kDQEumXe5r4Nbqj9UB2RynlYtNCv6sWLR+Atk2KU/91AfzidbqfBk08NaRYtuaprfOi6QYNxWe58tHXzDvnOs1qNn2s+YQ9bEshpiZaJpyFkAAdB04t89/1O/w1cDnyilFU=";
                    break;
            }

            if (string.IsNullOrEmpty(channelSecret) || string.IsNullOrEmpty(channelAccessToken))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var events = await request.GetWebhookEventsAsync(channelSecret);
            //var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            //var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");
            //var eventSourceState = await TableStorage<EventSourceState>.CreateAsync(connectionString, "eventsourcestate");
            var client = new LineMessagingClient(channelAccessToken);
            var app = new LineBotApp(merchantId, client);
            //var app = new LineMsgApp(lineMessagingClient);
            await app.RunAsync(events);

            return Request.CreateResponse(HttpStatusCode.OK);
        }      
    }
}
