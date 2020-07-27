// Decompiled with JetBrains decompiler
// Type: MB.WebAuthenticationTester.WebTestRequest
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using MB.Authorization;
using MB.Authorization.Basic;
using MB.Authorization.Kerberos;
using MB.Authorization.Ntlm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Xml.Serialization;

namespace MB.WebAuthenticationTester
{
  [Serializable]
  public class WebTestRequest
  {
    [XmlIgnore]
    public Dictionary<string, string> RequestHeadersDictionary = new Dictionary<string, string>();
    [XmlIgnore]
    public Dictionary<string, string> ResponseHeadersDictionary = new Dictionary<string, string>();
    [XmlIgnore]
    public HttpWebRequest Request;
    [XmlIgnore]
    public HttpWebResponse Response;
    public string Url;
    public string HttpResult;
    public string ErrorMessage;
    public DateTime RequestDate;
    [XmlIgnore]
    public string UserName;
    [XmlIgnore]
    public string Domain;
    [XmlIgnore]
    public string SPN;
    public AuthorizationType AuthorizationType;
    public AuthorizationMessage AuthorizationMessage;
    protected ICredentials _requestCredentials;
    protected IWebProxy _proxy;

    public List<string> RequestHeaders
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (string key in this.RequestHeadersDictionary.Keys)
          stringList.Add(string.Format("{0}: {1}", (object) key, (object) this.RequestHeadersDictionary[key]));
        return stringList;
      }
    }

    public List<string> ResponseHeaders
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (string key in this.ResponseHeadersDictionary.Keys)
          stringList.Add(string.Format("{0}: {1}", (object) key, (object) this.ResponseHeadersDictionary[key]));
        return stringList;
      }
    }

    private WebTestRequest()
    {
    }

    public WebTestRequest(string url, ICredentials requestCredentials, IWebProxy proxy)
    {
      this.Url = url;
      this._requestCredentials = requestCredentials;
      this._proxy = proxy;
    }

    internal WebTestRequest(string authorizationHeader)
    {
      this.ParseAuthorizationHeaderValue(authorizationHeader);
    }

    public bool DoRequest()
    {
      this.RequestDate = DateTime.Now;
      try
      {
        ServicePointManager.SecurityProtocol =  (SecurityProtocolType)(
          (int)SecurityProtocolType.Ssl3 
          | 3072 // SecurityProtocolType.Tls12 
          | 768 // SecurityProtocolType.Tls11 
          | 192 // SecurityProtocolType.Tls;
          );

        this.Request = WebRequest.Create(this.Url) as HttpWebRequest;
        this.Request.Credentials = this._requestCredentials;
        this.Request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        if (this._proxy != null)
          this.Request.Proxy = this._proxy;
        this.Request.AllowAutoRedirect = false;
        this.Response = this.Request.GetResponse() as HttpWebResponse;
        StreamReader streamReader = new StreamReader(this.Response.GetResponseStream());
        streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch (WebException ex)
      {
        if (ex.Response != null)
        {
          this.Response = ex.Response as HttpWebResponse;
        }
        else
        {
          this.ErrorMessage = ex.ToString();
          return false;
        }
      }
      catch (Exception ex)
      {
        this.ErrorMessage = ex.ToString();
        return false;
      }
      if (this.Response != null)
        this.ProcessResponse();
      return true;
    }

    private void ProcessResponse()
    {
      this.SetHttpResult();
      this.SetHeaders();
      this.ParseAuthorizationHeader();
    }

    private void SetHeaders()
    {
      foreach (string allKey in this.Request.Headers.AllKeys)
        this.RequestHeadersDictionary.Add(allKey, this.Request.Headers[allKey]);
      foreach (string allKey in this.Response.Headers.AllKeys)
        this.ResponseHeadersDictionary.Add(allKey, this.Response.Headers[allKey]);
    }

    private void ParseAuthorizationHeader()
    {
      if (this.RequestHeadersDictionary.ContainsKey("Authorization"))
        this.ParseAuthorizationHeaderValue(this.RequestHeadersDictionary["Authorization"]);
      else
        this.AuthorizationType = AuthorizationType.None;
    }

    private void ParseAuthorizationHeaderValue(string headerValue)
    {
      try
      {
        this.AuthorizationMessage = AuthorizationMessageFactory.Build(headerValue);
        this.AuthorizationType = this.AuthorizationMessage.AuthorizationType;
        if (this.AuthorizationMessage is BasicAuthorizationMessage)
          this.UserName = ((BasicAuthorizationMessage) this.AuthorizationMessage).UserName;
        else if (this.AuthorizationMessage is NtlmAuthorizeMessage)
          this.SetNtlmValues(this.AuthorizationMessage as NtlmAuthorizeMessage);
        else if (this.AuthorizationMessage is KerberosAuthorizeMessage)
          this.SetKerberosValues(this.AuthorizationMessage as KerberosAuthorizeMessage);
        else
          this.ErrorMessage = "Unexpected authorization header";
      }
      catch (Exception ex)
      {
        this.ErrorMessage = "Unexpected authorization header";
      }
    }

    private void SetKerberosValues(KerberosAuthorizeMessage kerberosAuthorizeMessage)
    {
      try
      {
        if (kerberosAuthorizeMessage.NegotiationToken == null)
          return;
        NegTokenInit negotiationToken = kerberosAuthorizeMessage.NegotiationToken as NegTokenInit;
        this.SPN = negotiationToken.MechToken.InnerContextToken.Ticket.ServiceName.ToString();
        this.Domain = negotiationToken.MechToken.InnerContextToken.Ticket.Realm;
      }
      catch (Exception ex)
      {
      }
    }

    private void SetNtlmValues(NtlmAuthorizeMessage ntlmAuthorizeMessage)
    {
      this.UserName = ntlmAuthorizeMessage.UserName;
      this.Domain = ntlmAuthorizeMessage.DomainName;
    }

    private void SetHttpResult()
    {
      this.HttpResult = string.Format("{0} {1}", (object) (int) this.Response.StatusCode, (object) this.Response.StatusCode.ToString());
    }
  }
}
