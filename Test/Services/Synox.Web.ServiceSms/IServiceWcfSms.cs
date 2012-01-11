using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Synox.Services.ServiceSMS.Entity;

namespace Synox.Web.ServiceSms
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceWcfSms" in both code and config file together.
    [ServiceContract]
    public interface IServiceWcfSms
    {
        [OperationContract]
        void SendSms(string numeroGsm, string message);

        [OperationContract]
        List<Sms> Get();

        [OperationContract]
        void SaveState(List<Sms> smsList);

        //        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)         {                 ServiceHost host = new ServiceHost(typeof(Service1),baseAddresses);            foreach(Uri uri in baseAddresses)             {                  WebHttpBinding webbinding=new WebHttpBinding(WebHttpSecurityMode.None);             webbinding.AllowCookies=true;             webbinding.CrossDomainScriptAccessEnabled=true;             EndpointAddress ea=new EndpointAddress(uri);                        WebScriptEnablingBehavior behavior = new WebScriptEnablingBehavior();             behavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json;            // behavior.DefaultBodyStyle = WebMessageBodyStyle.WrappedRequest;                      behavior.DefaultOutgoingRequestFormat = WebMessageFormat.Json;            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IService1), webbinding, uri);            endpoint.Behaviors.Add(behavior);                    }                         return host;                 } 
    }
}
