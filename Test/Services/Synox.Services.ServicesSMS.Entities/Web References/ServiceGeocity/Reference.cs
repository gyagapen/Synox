﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.235.
// 
#pragma warning disable 1591

namespace Synox.Services.ServiceSMS.Entity.ServiceGeocity {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "SyncMobileServiceSoap", Namespace = "http://geocity.symexo.com/")]
    public partial class SyncMobileService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback SupervisionSaasOperationCompleted;

        private System.Threading.SendOrPostCallback SendSmsOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public SyncMobileService()
        {
            this.Url = global::Synox.Services.ServiceSMS.Entity.Properties.Settings.Default.Synox_Services_ServicesSMS_Entity_ServiceGeocity_SyncMobileService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }



        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://geocity.symexo.com/SupervisionSaas", RequestNamespace = "http://geocity.symexo.com/", ResponseNamespace = "http://geocity.symexo.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SupervisionSaas()
        {
            object[] results = this.Invoke("SupervisionSaas", new object[0]);
            return ((string)(results[0]));
        }




        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://geocity.symexo.com/SendSms", RequestNamespace = "http://geocity.symexo.com/", ResponseNamespace = "http://geocity.symexo.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> SendSms(string numeroGsm, string message)
        {
            object[] results = this.Invoke("SendSms", new object[] {
                        numeroGsm,
                        message});
            return ((System.Nullable<System.DateTime>)(results[0]));
        }


        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }
}


#pragma warning restore 1591