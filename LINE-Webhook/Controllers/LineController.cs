using Line.Messaging;
using Line.Messaging.Webhooks;
using LINE_Webhook.API.LINE;
using LINE_Webhook.CloudStorage;
using LINE_Webhook.Models;
//using LINE_Webhook.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LINE_Webhook.Controllers
{
    [RoutePrefix("api")]
    public class LineController : ApiController
    {
        private static LineMessagingClient lineMessagingClient;
        private string accessToken = ConfigurationManager.AppSettings["ChannelAccessToken"];
        private string channelSecret = ConfigurationManager.AppSettings["ChannelSecret"];
        public LineController()
        {
            lineMessagingClient = new LineMessagingClient(accessToken);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult Webhook([FromBody] LineWebhookModels data)
        {
            if (data == null) return BadRequest();
            if (data.events == null) return BadRequest();

            //foreach (Event e in data.events)
            //{
            //    if (e.type == EventType.message)
            //    {
            //        ReplyBody rb = new ReplyBody()
            //        {
            //            replyToken = e.replyToken,
            //            messages = procMessage(e.message)
            //        };
            //        Reply reply = new Reply(rb);
            //        reply.send();

            //    }
            //}
            return Ok(data);
        }
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post(HttpRequestMessage request)
        {
            //public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
            //{
            var events = await request.GetWebhookEventsAsync(channelSecret);
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");
            var eventSourceState = await TableStorage<EventSourceState>.CreateAsync(connectionString, "eventsourcestate");

            var app = new LineBotApp(lineMessagingClient, eventSourceState, blobStorage);
            await app.RunAsync(events);

            return Ok(); // Request.CreateResponse(HttpStatusCode.OK);
            //}
        }

        //private List<SendMessage> procMessage(ReceiveMessage m)
        //{
        //    List<SendMessage> msgs = new List<SendMessage>();
        //    SendMessage sm = new SendMessage()
        //    {
        //        type = Enum.GetName(typeof(MessageType), m.type)
        //    };
        //    switch (m.type)
        //    {
        //        case MessageType.sticker:
        //            sm.packageId = m.packageId;
        //            sm.stickerId = m.stickerId;
        //            break;
        //        case MessageType.text:
        //            sm.text = m.text;
        //            break;
        //        default:
        //            sm.type = Enum.GetName(typeof(MessageType), MessageType.text);
        //            sm.text = "很抱歉，我只是一隻鸚鵡機器人，目前只能回覆基本貼圖與文字訊息喔！";
        //            break;
        //    }
        //    msgs.Add(sm);
        //    return msgs;
        //}
    }
}
