using Line.Messaging.Webhooks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LineServices.Context
{
    public class LineContext : BaseContext
    {

        public LineContext() : base() { }
        public LineContext(String connectionString) : base(connectionString) { }

        public decimal SaveEvents(string channelId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken)
        {

            ////EncryptURLContext EncryptURL = new EncryptURLContext();
            //String password = string.Empty;
            //using (Encryption.Coding encryption = new Encryption.Coding())
            //{
            //    password = encryption.Encrypt(entity.Password);
            //}
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("ChannelId", channelId);
            para[1] = new SqlParameter("EventType", eventType);
            para[2] = new SqlParameter("SourceType", sourceType);
            para[3] = new SqlParameter("SourceId", sourceId);
            para[4] = new SqlParameter("Sender", sender);
            para[5] = new SqlParameter("MessageType", messageType);
            para[6] = new SqlParameter("MessageText", messageText);
            para[7] = new SqlParameter("ReplyToken", replyToken);
            para[8] = new SqlParameter("Result", 0);

            para[8].Direction = ParameterDirection.Output;

            //auditIDParameter.Direction = System.Data.ParameterDirection.Output;

            //ws_InsertAuditLogins @TCO, @Password, @ipaddress, @ipaddress2, @_Status,@recordURL , @StaffLoginFlag,  @StaffID, @BrowserType, @_SuccessFlag, @AuditID output, @Referrer, @ASPSessionID
            this.Database.ExecuteSqlCommand("sp_InsertEvents @ChannelId,@EventType,@SourceType,@SourceId,@Sender,@MessageType,@MessageText,@ReplyToken,@Result out", para);

            return !string.IsNullOrEmpty(para[8].Value.ToString()) ? Convert.ToDecimal(para[8].Value) : 0;
        }

        //public List<Friends> GetChats(string channelId)
        //{
        //    SqlParameter[] para = new SqlParameter[1];
        //    para[0] = new SqlParameter("ChannelId", channelId);

        //    List<Friends> list = this.Database.SqlQuery<Friends>("sp_GetChats @ChannelId", para).ToList<Friends>();

        //    return list;
        //}

        public bool RegisterMerchant(string channelId, string zortId, string userId, string merchantName, string channelAccessToken, string remark)
        {
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("ChannelId", channelId);
            para[1] = new SqlParameter("ZortId", zortId);
            para[2] = new SqlParameter("UserId", userId);
            para[3] = new SqlParameter("MerchantName", merchantName);
            para[4] = new SqlParameter("ChannelAccessToken", channelAccessToken);
            para[5] = new SqlParameter("Remark", remark);
            para[6] = new SqlParameter("Result", 0);

            para[7].Direction = ParameterDirection.Output;

            this.Database.ExecuteSqlCommand("sp_RegisterMerchant @ChannelId,@ZortId,@UserId,@MerchantName,@ChannelAccessToken,@Remark,@Result out", para);

            var result = !string.IsNullOrEmpty(para[7].Value.ToString()) ? Convert.ToInt16(para[7].Value) : 0;

            return result > 0;
        }

        public Merchant GetMerchant(string channelId, string zortId)
        {
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("ChannelId", channelId);
            para[1] = new SqlParameter("ZortId", zortId);

            return this.Database.SqlQuery<Merchant>("sp_GetMerchant @ChannelId,@ZortId", para).FirstOrDefault();
        }
    }
}