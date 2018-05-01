using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LineServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        decimal SaveEvents(string channelId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken);

        //[OperationContract]
        //List<Friends> GetChats(string channelId);

        [OperationContract]
        bool RegisterMerchant(string merchantId, string channelId, string channelSecret, string channelAccessToken, string descriptions);

        [OperationContract]
        Merchant GetMerchant(string channelId, string zortId);
    }

    #region "DataContract"

 
    //// Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class Friends
    //{
    //    [DataMember]
    //    public string MerchantId { get; set; }
    //    [DataMember]
    //    public string SourceId { get; set; }
    //    [DataMember]
    //    public string SourceType { get; set; }
    //    [DataMember]
    //    public Int64 EventId { get; set; }
    //    [DataMember]
    //    public string MessageText { get; set; }
    //}

    [DataContract]
    public class Merchant
    {
        [DataMember]
        public string ChannelId { get; set; }
        [DataMember]
        public string ZortId { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string MerchantName { get; set; }
        [DataMember]
        public string ChannelSecret { get; set; }
        [DataMember]
        public string ChannelAccessToken { get; set; }
        [DataMember]
        public string Remark { get; set; }
    }

    #endregion
}
