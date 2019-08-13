using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UMC.Demo
{
    public class WebContext : UMC.Net.NetContext
    {
        HttpContext _Context;
        public WebContext(HttpContext co)
        {
            this._Context = co;
        }


        public override void AddHeader(string name, string value)
        {
            this._Context.Response.AddHeader(name, value);
        }
        public override void AppendCookie(string name, string value)
        {
            _Context.Response.AppendCookie(new HttpCookie(name, value) { Expires = DateTime.Now.AddYears(100) });
        }

        public override NameValueCollection Cookies
        {
            get
            {
                if (_Cookies == null)
                {
                    _Cookies = new NameValueCollection();
                    for (var i = 0; i < _Context.Request.Cookies.Count; i++)
                    {
                        var c = _Context.Request.Cookies[i];
                        _Cookies.Add(c.Name, c.Value);

                    }

                }
                return _Cookies;
            }
        }
        NameValueCollection _Cookies;
        public override NameValueCollection Headers
        {
            get
            {
                return _Context.Request.Headers;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _Context.Request.QueryString;
            }
        }

        public override NameValueCollection Form
        {
            get
            {
                return this._Context.Request.Form;
            }
        }

        public override System.IO.Stream InputStream
        {
            get
            {
                if (_Context.Request.Files.Count > 0)
                {
                    return _Context.Request.Files[0].InputStream;
                }
                if (this.ContentType.IndexOf("www-form-urlencoded", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    return System.IO.Stream.Null;
                }

                return this._Context.Request.InputStream;
            }
        }

        public override System.IO.TextWriter Output
        {
            get
            {
                return this._Context.Response.Output;
            }
        }
        public override System.IO.Stream OutputStream
        {
            get
            {
                return this._Context.Response.OutputStream;
            }
        }

        public override string ContentType
        {
            get
            {
                return this._Context.Request.ContentType;
            }
            set
            {
                this._Context.Response.ContentType = value;
            }
        }

        public override string UserHostAddress
        {
            get { return this._Context.Request.UserHostAddress; }
        }

        public override string RawUrl
        {
            get { return this._Context.Request.RawUrl; }
        }

        public override string UserAgent
        {
            get { return this._Context.Request.UserAgent; }
        }

        public override Uri UrlReferrer
        {
            get { return this._Context.Request.UrlReferrer; }
        }

        public override Uri Url
        {
            get { return this._Context.Request.Url; }
        }

        public override void Redirect(string url)
        {
            this._Context.Response.Redirect(url, true);
        }

        public override int StatusCode
        {
            get
            {

                return this._Context.Response.StatusCode;
            }
            set
            {

                this._Context.Response.StatusCode = value;
            }
        }

        public override string HttpMethod
        {
            get { return this._Context.Request.HttpMethod; }
        }
    }
}
