using Line.Messaging;
using Line.Messaging.Webhooks;
using LINE_Webhook.CloudStorage;
using LINE_Webhook.Models;
using LINE_Webhook.ZortServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LINE_Webhook
{
    internal class LineBotApp : WebhookApplication
    {
        private LineMessagingClient messagingClient { get; }
        private BlobStorage blobStorage { get; }
        private LineIntegrationModel merInfo { get; }

        public LineBotApp(LineIntegrationModel mer, LineMessagingClient client, BlobStorage blob)
        {
            this.messagingClient = client;
            this.blobStorage = blob;
            this.merInfo = mer;
        }

        #region Handlers

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            string json = JsonConvert.SerializeObject(ev);
            //Save raw data in AutoBot DB.
            using (LineServices.ServiceClient ws = new LineServices.ServiceClient())
            {
                merInfo.eventid = await ws.SaveEventsAsync(merInfo.channel_id, ev.Type.ToString(), ev.Source.Type.ToString(), ev.Source.Id.ToString(), ev.Source.Id.ToString(), ev.Message.Type.ToString(), json, ev.ReplyToken);
            }

            UserProfile uInfo = await messagingClient.GetUserProfileAsync(ev.Source.UserId);
            using (ZortServices.LineAtServiceClient zort = new ZortServices.LineAtServiceClient())
            {
                switch (ev.Message.Type)
                {
                    case EventMessageType.Text:
                        await zort.AddMessageAsync(ev.Source.UserId, uInfo.DisplayName, uInfo.PictureUrl, ((TextEventMessage)ev.Message).Text, ev.ReplyToken, merInfo);
                        //await messagingClient.ReplyMessageAsync(ev.ReplyToken, new List<ISendMessage> { new TextMessage(((TextEventMessage)ev.Message).Text) });
                        break;
                    case EventMessageType.Image:
                    case EventMessageType.Audio:
                    case EventMessageType.Video:
                    case EventMessageType.File:
                        // Prepare blob directory name for binary object.
                        var path = ev.Source.Type.ToString();// merInfo.channel_id + "_" + ev.Source.Type + "_" + ev.Source.Id;
                        var stream = await messagingClient.GetContentStreamAsync(ev.Message.Id);
                        var ext = GetFileExtension(stream.ContentHeaders.ContentType.MediaType);
                        var uri = await blobStorage.UploadFromStreamAsync(stream, path, ev.Message.Id + ext);
                        await zort.AddPictureAsync(ev.Source.UserId, uInfo.DisplayName, uInfo.PictureUrl, uri.ToString(), ev.ReplyToken, merInfo);
                        await messagingClient.ReplyMessageAsync(ev.ReplyToken, new List<ISendMessage> { new ImageMessage(uri.ToString(),uri.ToString()) });
                        break;
                    case EventMessageType.Location:
                        var location = ((LocationEventMessage)ev.Message);
                        await zort.AddLocationAsync(ev.Source.UserId, uInfo.DisplayName, uInfo.PictureUrl, location.Latitude.ToString(), location.Longitude.ToString(), ev.ReplyToken, merInfo);
                        //await HandleLocationAsync(ev.ReplyToken, ev.Source.UserId, location);
                        break;
                    case EventMessageType.Sticker:
                        var sticker = (StickerEventMessage)ev.Message;
                        await zort.AddStickerAsync(ev.Source.UserId, uInfo.DisplayName, uInfo.PictureUrl, sticker.PackageId, sticker.StickerId, ev.ReplyToken, merInfo);
                        //await HandleStickerAsync(ev.ReplyToken, ev.Source.UserId, sticker);
                        break;
                }
            }
        }
        
        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            switch (ev.Postback.Data)
            {
                case "Date":
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "You chose the date: " + ev.Postback.Params.Date);
                    break;
                case "Time":
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "You chose the time: " + ev.Postback.Params.Time);
                    break;
                case "DateTime":
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "You chose the date-time: " + ev.Postback.Params.DateTime);
                    break;
                default:
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "Your postback is " + ev.Postback.Data);
                    break;
            }
        }
        
        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            string json = JsonConvert.SerializeObject(ev);
            // Store source information which follows the bot.
            //await sourceState.AddAsync(ev.Source.Type.ToString(), ev.Source.Id);

            var userName = "";
            if (!string.IsNullOrEmpty(ev.Source.Id))
            {
                var userProfile = await messagingClient.GetUserProfileAsync(ev.Source.Id);
                userName = userProfile?.DisplayName ?? "";
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"Hello {userName}! Thank you for following !");
        }
        
        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {
            string json = JsonConvert.SerializeObject(ev);
            // Remote source information which unfollows the bot.
            //await sourceState.DeleteAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            string json = JsonConvert.SerializeObject(ev);
            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your {ev.Source.Type.ToString().ToLower()}!");
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            string json = JsonConvert.SerializeObject(ev);
            //await sourceState.DeleteAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnBeaconAsync(BeaconEvent ev)
        {
            var message = "";
            switch (ev.Beacon.Type)
            {
                case BeaconType.Enter:
                    message = "You entered the beacon area!";
                    break;
                case BeaconType.Leave:
                    message = "You leaved the beacon area!";
                    break;
                case BeaconType.Banner:
                    message = "You tapped the beacon banner!";
                    break;
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"{message}(Dm:{ev.Beacon.Dm}, Hwid:{ev.Beacon.Hwid})");
        }

        #endregion

        private async Task HandleLocationAsync(string replyToken, string userId, LocationEventMessage location)
        {
            await messagingClient.ReplyMessageAsync(replyToken, new[]
            {
                new LocationMessage("Location", location.Address,location.Latitude, location.Longitude)
            });
        }
        private async Task HandleTextAsync(long evId, string userId, string displayName, MessageEvent ev, ZortServices.LineIntegrationModel line)
        {
            var userMessage = ((TextEventMessage)ev.Message).Text;
            var keyword = userMessage.ToLower().Replace(" ", "");
            ISendMessage replyMessage = null;

            #region "BOT keyword"
            if (keyword == "buttons")
            {
                replyMessage = new TemplateMessage("Button Template",
                    new ButtonsTemplate(text:"ButtonsTemplate", title:"Click Buttons.",
                    actions: new List<ITemplateAction> {
                        new MessageTemplateAction("Message Label", "sample data"),
                        new PostbackTemplateAction("Postback Label", "sample data", "sample data"),
                    new UriTemplateAction("Uri Label", "https://github.com/kenakamu")
                    }));
            }
            else if (keyword == "confirm")
            {
                replyMessage = new TemplateMessage("Confirm Template",
                    new ConfirmTemplate("ConfirmTemplate", new List<ITemplateAction> {
                        new MessageTemplateAction("Yes", "Yes"),
                        new MessageTemplateAction("No", "No")
                    }));
            }
            else if (keyword == "carousel")
            {
                List<ITemplateAction> actions1 = new List<ITemplateAction>();
                List<ITemplateAction> actions2 = new List<ITemplateAction>();

                // Add actions.
                actions1.Add(new MessageTemplateAction("Message Label", "sample data"));
                actions1.Add(new PostbackTemplateAction("Postback Label", "sample data", "sample data"));
                actions1.Add(new UriTemplateAction("Uri Label", "https://github.com/kenakamu"));

                // Add datetime picker actions
                actions2.Add(new DateTimePickerTemplateAction("DateTime Picker", "DateTime",
                    DateTimePickerMode.Datetime, "2017-07-21T13:00", null, null));
                actions2.Add(new DateTimePickerTemplateAction("Date Picker", "Date",
                    DateTimePickerMode.Date, "2017-07-21", null, null));
                actions2.Add(new DateTimePickerTemplateAction("Time Picker", "Time",
                    DateTimePickerMode.Time, "13:00", null, null));

                replyMessage = new TemplateMessage("Button Template",
                    new CarouselTemplate(new List<CarouselColumn> {
                        new CarouselColumn("Casousel 1 Text", "https://github.com/apple-touch-icon.png",
                        "Casousel 1 Title", actions1),
                        new CarouselColumn("Casousel 1 Text", "https://github.com/apple-touch-icon.png",
                        "Casousel 1 Title", actions2)
                    }));
            }
            else if (keyword == "imagecarousel")
            {
                UriTemplateAction action = new UriTemplateAction("Uri Label", "https://github.com/kenakamu");

                replyMessage = new TemplateMessage("ImageCarouselTemplate",
                    new ImageCarouselTemplate(new List<ImageCarouselColumn> {
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action)
                    }));
            }
            else if (keyword == "imagemap")
            {
                var url = HttpContext.Current.Request.Url;
                var imageUrl = $"{url.Scheme}://{url.Host}:{url.Port}/images/githubicon";
                replyMessage = new ImagemapMessage(
                    imageUrl,
                    "GitHub",
                    new ImagemapSize(1040, 1040), new List<IImagemapAction>
                    {
                        new UriImagemapAction(new ImagemapArea(0, 0, 520, 1040), "http://github.com"),
                        new MessageImagemapAction(new ImagemapArea(520, 0, 520, 1040), "I love LINE!")
                    });
            }
            else if (keyword == "addrichmenu")
            {
                // Create Rich Menu
                RichMenu richMenu = new RichMenu()
                {
                    Size = ImagemapSize.RichMenuLong,
                    Selected = false,
                    Name = "nice richmenu",
                    ChatBarText = "touch me",
                    Areas = new List<ActionArea>()
                    {
                        new ActionArea()
                        {
                            Bounds = new ImagemapArea(0,0 ,ImagemapSize.RichMenuLong.Width,ImagemapSize.RichMenuLong.Height),
                            Action = new PostbackTemplateAction("ButtonA", "Menu A", "Menu A")
                        }
                    }
                };

                var richMenuId = await messagingClient.CreateRichMenuAsync(richMenu);
                var image = new MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~\Images\richmenu.PNG")));
                // Upload Image
                await messagingClient.UploadRichMenuPngImageAsync(image, richMenuId);
                // Link to user
                await messagingClient.LinkRichMenuToUserAsync(userId, richMenuId);

                replyMessage = new TextMessage("Rich menu added");
            }
            else if (keyword == "deleterichmenu")
            {
                // Get Rich Menu for the user
                var richMenuId = await messagingClient.GetRichMenuIdOfUserAsync(userId);
                await messagingClient.UnLinkRichMenuFromUserAsync(userId);
                await messagingClient.DeleteRichMenuAsync(richMenuId);
                replyMessage = new TextMessage("Rich menu deleted");
            }
            else if (keyword == "deleteallrichmenu")
            {
                // Get Rich Menu for the user
                var richMenuList = await messagingClient.GetRichMenuListAsync();
                foreach (var richMenu in richMenuList)
                {
                    await messagingClient.DeleteRichMenuAsync(richMenu.RichMenuId);
                }
                replyMessage = new TextMessage("All rich menu added");
            }
            else
            {
                replyMessage = new TextMessage(userMessage);
            }
            #endregion

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, new List<ISendMessage> { replyMessage });
        }

        /// <summary>
        /// Replies random sticker
        /// Sticker ID of bssic stickers (packge ID =1)
        /// see https://devdocs.line.me/files/sticker_list.pdf
        /// </summary>
        private async Task HandleStickerAsync(string replyToken, string userId, StickerEventMessage sticker)
        {
            //using (ZortServices.LineAtServiceClient zort = new ZortServices.LineAtServiceClient())
            //{
            //    await zort.AddMessage_StickerAsync(userId, "", location.Address, location.Latitude.ToString(), location.Longitude.ToString(), replyToken, null);
            //}
            await messagingClient.ReplyMessageAsync(replyToken, new[]
            {
                new StickerMessage(sticker.PackageId, sticker.StickerId)
            });

            //var stickerids = Enumerable.Range(1, 17)
            //    .Concat(Enumerable.Range(21, 1))
            //    .Concat(Enumerable.Range(100, 139 - 100 + 1))
            //    .Concat(Enumerable.Range(401, 430 - 400 + 1)).ToArray();

            //var rand = new Random(Guid.NewGuid().GetHashCode());
            //var stickerId = stickerids[rand.Next(stickerids.Length - 1)].ToString();
            //await messagingClient.ReplyMessageAsync(replyToken, new[] {
            //            new StickerMessage("1", stickerId)
            //        });
        }

        private string GetFileExtension(string mediaType)
        {
            switch (mediaType)
            {
                case "image/jpeg":
                    return ".jpeg";
                case "audio/x-m4a":
                    return ".m4a";
                case "video/mp4":
                    return ".mp4";
                default:
                    return "";
            }
        }
    }
}