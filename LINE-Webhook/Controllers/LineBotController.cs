﻿using Line.Messaging.Webhooks;
using Line.Messaging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LINE_Webhook.Filter;
using LINE_Webhook.Utilities;
using LINE_Webhook.Helper;

namespace LINE_Webhook.Controllers
{
    [RoutePrefix("api")]
    [ApiExceptionFilter]
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

        //[ResponseType(typeof(string))]
        //[HttpPost]
        public async Task<HttpResponseMessage> GetFriends()
        {
            try
            {
                //get credential from login session.
                var merchantId = "1567098166";
                var channelAccessToken = "FT1trntaKs5+hEAG9Bj+3ispLhNSNBxk1RO9w9q5kDQEumXe5r4Nbqj9UB2RynlYtNCv6sWLR+Atk2KU/91AfzidbqfBk08NaRYtuaprfOi6QYNxWe58tHXzDvnOs1qNn2s+YQ9bEshpiZaJpyFkAAdB04t89/1O/w1cDnyilFU=";
                var client = new LineMessagingClient(channelAccessToken);
                var ctn = new HttpResponseMessage
                {
                    Content = new StringContent((await new LineChat().GetFriends(merchantId, client).JsonSerializeAsync()))
                };

                return ctn;

            }
            catch(Exception ex)
            {
                return PageRequest.Get_GenericError(ex);
            }                           
        }
    }
}
