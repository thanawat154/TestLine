﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LINE_Webhook.LineServices {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Friends", Namespace="http://schemas.datacontract.org/2004/07/LineServices")]
    [System.SerializableAttribute()]
    public partial class Friends : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EventIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool MerchantIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageTextField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SourceIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SourceTypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EventId {
            get {
                return this.EventIdField;
            }
            set {
                if ((object.ReferenceEquals(this.EventIdField, value) != true)) {
                    this.EventIdField = value;
                    this.RaisePropertyChanged("EventId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool MerchantId {
            get {
                return this.MerchantIdField;
            }
            set {
                if ((this.MerchantIdField.Equals(value) != true)) {
                    this.MerchantIdField = value;
                    this.RaisePropertyChanged("MerchantId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageText {
            get {
                return this.MessageTextField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageTextField, value) != true)) {
                    this.MessageTextField = value;
                    this.RaisePropertyChanged("MessageText");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SourceId {
            get {
                return this.SourceIdField;
            }
            set {
                if ((object.ReferenceEquals(this.SourceIdField, value) != true)) {
                    this.SourceIdField = value;
                    this.RaisePropertyChanged("SourceId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SourceType {
            get {
                return this.SourceTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.SourceTypeField, value) != true)) {
                    this.SourceTypeField = value;
                    this.RaisePropertyChanged("SourceType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LineServices.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SaveEvents", ReplyAction="http://tempuri.org/IService/SaveEventsResponse")]
        decimal SaveEvents(string merchantId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SaveEvents", ReplyAction="http://tempuri.org/IService/SaveEventsResponse")]
        System.Threading.Tasks.Task<decimal> SaveEventsAsync(string merchantId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetFriends", ReplyAction="http://tempuri.org/IService/GetFriendsResponse")]
        LINE_Webhook.LineServices.Friends[] GetFriends(string merchantId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetFriends", ReplyAction="http://tempuri.org/IService/GetFriendsResponse")]
        System.Threading.Tasks.Task<LINE_Webhook.LineServices.Friends[]> GetFriendsAsync(string merchantId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : LINE_Webhook.LineServices.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<LINE_Webhook.LineServices.IService>, LINE_Webhook.LineServices.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public decimal SaveEvents(string merchantId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken) {
            return base.Channel.SaveEvents(merchantId, eventType, sourceType, sourceId, sender, messageType, messageText, replyToken);
        }
        
        public System.Threading.Tasks.Task<decimal> SaveEventsAsync(string merchantId, string eventType, string sourceType, string sourceId, string sender, string messageType, string messageText, string replyToken) {
            return base.Channel.SaveEventsAsync(merchantId, eventType, sourceType, sourceId, sender, messageType, messageText, replyToken);
        }
        
        public LINE_Webhook.LineServices.Friends[] GetFriends(string merchantId) {
            return base.Channel.GetFriends(merchantId);
        }
        
        public System.Threading.Tasks.Task<LINE_Webhook.LineServices.Friends[]> GetFriendsAsync(string merchantId) {
            return base.Channel.GetFriendsAsync(merchantId);
        }
    }
}
