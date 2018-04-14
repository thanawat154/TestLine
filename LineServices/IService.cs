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
        decimal SaveEvents(string merchantId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken);

        [OperationContract]
        List<Friends> GetFriends(string merchantId);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Friends
    {

        [DataMember]
        public bool MerchantId { get; set; }
        [DataMember]
        public string SourceId { get; set; }
        [DataMember]
        public string SourceType { get; set; }
        [DataMember]
        public string EventId { get; set; }
        [DataMember]
        public string MessageText { get; set; }
    }
}
