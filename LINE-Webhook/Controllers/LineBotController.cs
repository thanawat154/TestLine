using Line.Messaging.Webhooks;
using Line.Messaging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LINE_Webhook.Filter;
using LINE_Webhook.Utilities;
using LINE_Webhook.Helper;
using LINE_Webhook.Logging;
using System.Configuration;
using LINE_Webhook.CloudStorage;
using LINE_Webhook.ZortServices;

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

    
        public async Task<HttpResponseMessage> WebHook(string merchantId, string channelId, HttpRequestMessage request)
        {
            var mer = new LineServices.Merchant();

            using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
            {
                mer = await ws.GetMerchantAsync(channelId, merchantId);
            }

            if (!string.IsNullOrEmpty(mer.ChannelId) && !string.IsNullOrEmpty(mer.ZortId) && !string.IsNullOrEmpty(mer.ChannelSecret) && !string.IsNullOrEmpty(mer.ChannelAccessToken))
            {
                var events = await request.GetWebhookEventsAsync(mer.ChannelSecret);
                var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                var blobStorage = await BlobStorage.CreateAsync(connectionString, mer.ChannelId);
                //var eventSourceState = await TableStorage<EventSourceState>.CreateAsync(connectionString, "eventsourcestate");
                var client = new LineMessagingClient(mer.ChannelAccessToken);
                var merInfo = new LineIntegrationModel {
                    channel_id = mer.ChannelId,
                    channel_secret = mer.ChannelSecret,
                    channel_accesstoken = mer.ChannelAccessToken
                };

                var app = new LineBotApp(merInfo, client, blobStorage);
                await app.RunAsync(events);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                Logger.LogError("Invalid merchant info!: ChannelId="+ mer.ChannelId);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        //public async Task<HttpResponseMessage> SendMessageAsync(string merchantId, string channelId, HttpRequestMessage request)
        //{
        //    var mer = new LineServices.Merchant();

        //    using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
        //    {
        //        mer = await ws.GetMerchantAsync(channelId, merchantId);
        //    }

        //    if (!string.IsNullOrEmpty(mer.ChannelId) && !string.IsNullOrEmpty(mer.ZortId) && !string.IsNullOrEmpty(mer.ChannelSecret) && !string.IsNullOrEmpty(mer.ChannelAccessToken))
        //    {
        //        var events = await request.GetWebhookEventsAsync(mer.ChannelSecret);
        //        var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
        //        var blobStorage = await BlobStorage.CreateAsync(connectionString, mer.ChannelId);
        //        //var eventSourceState = await TableStorage<EventSourceState>.CreateAsync(connectionString, "eventsourcestate");
        //        var client = new LineMessagingClient(mer.ChannelAccessToken);
        //        var merInfo = new LineIntegrationModel
        //        {
        //            channel_id = mer.ChannelId,
        //            channel_secret = mer.ChannelSecret,
        //            channel_accesstoken = mer.ChannelAccessToken
        //        };

        //        var app = new LineChat(merInfo, client, blobStorage);
        //        await app.RunAsync(events);

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        Logger.LogError("Invalid merchant info!: ChannelId=" + mer.ChannelId);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}
        ////[ResponseType(typeof(string))]
        ////[HttpPost]
        //public async Task<HttpResponseMessage> GetFriends()
        //{
        //    try
        //    {
        //        //get credential from login session.
        //        var merchantId = "1567098166";
        //        var channelAccessToken = "FT1trntaKs5+hEAG9Bj+3ispLhNSNBxk1RO9w9q5kDQEumXe5r4Nbqj9UB2RynlYtNCv6sWLR+Atk2KU/91AfzidbqfBk08NaRYtuaprfOi6QYNxWe58tHXzDvnOs1qNn2s+YQ9bEshpiZaJpyFkAAdB04t89/1O/w1cDnyilFU=";
        //        var client = new LineMessagingClient(channelAccessToken);
        //        var ctn = new HttpResponseMessage
        //        {
        //            Content = new StringContent((await new LineChat().GetFriends(merchantId, client).JsonSerializeAsync()))
        //        };

        //        return ctn;

        //    }
        //    catch(Exception ex)
        //    {
        //        return PageRequest.Get_GenericError(ex);
        //    }                           
        //}
    }
}
