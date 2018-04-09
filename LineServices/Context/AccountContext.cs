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
    public class AccountContext : BaseContext
    {

        public AccountContext() : base() { }
        public AccountContext(String connectionString) : base(connectionString) { }

        public decimal SaveEvents(string merchantId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken)
        {

            ////EncryptURLContext EncryptURL = new EncryptURLContext();
            //String password = string.Empty;
            //using (Encryption.Coding encryption = new Encryption.Coding())
            //{
            //    password = encryption.Encrypt(entity.Password);
            //}
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("MerchantId", merchantId);
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
            this.Database.ExecuteSqlCommand("sp_InsertEvents @MerchantId,@EventType,@SourceType,@SourceId,@Sender,@MessageType,@MessageText,@ReplyToken,@Result out", para);

            return !string.IsNullOrEmpty(para[8].Value.ToString()) ? Convert.ToDecimal(para[8].Value) : 0;
        }

        public List<Friends> GetFriends(string merchantId)
        {
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter("MerchantId", merchantId);

            List<Friends> list = this.Database.SqlQuery<Friends>("sp_GetFriends @MerchantId", para).ToList<Friends>();

            return list;
        }
    }
}