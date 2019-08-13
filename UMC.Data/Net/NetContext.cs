using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace UMC.Net
{
    public abstract class NetContext
    {
        public abstract int StatusCode
        {
            get;
            set;
        }
        public abstract string ContentType
        {
            get;
            set;
        }
        public abstract string UserHostAddress
        {
            get;
        }
        public abstract string RawUrl
        {
            get;
        }
        public abstract string UserAgent
        {
            get;
        }
        public abstract void AddHeader(string name, string value);
        public abstract void AppendCookie(string name, string value);
        public abstract NameValueCollection Headers
        {
            get;
        }

        public abstract NameValueCollection QueryString
        {
            get;
        }
        public abstract NameValueCollection Cookies
        {
            get;
        }
        public abstract NameValueCollection Form
        {
            get;
        }

        public abstract System.IO.Stream InputStream
        {
            get;
        }
        public abstract System.IO.TextWriter Output
        {
            get;
        }

        public abstract System.IO.Stream OutputStream
        {
            get;
        }
        public abstract Uri UrlReferrer
        {
            get;
        }
        public abstract Uri Url
        {
            get;
        }
        public abstract string HttpMethod
        {
            get;
        }
        public abstract void Redirect(string url);
    }


}
