using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UMC.Data;
using UMC.Security;

namespace UMC.Web
{
    /// <summary>
    /// 网络请求
    /// </summary>
    public abstract class WebFactory : IWebFactory
    {
        private String[] models;
        private Uri uri;
        protected void RegisterModel(Uri uri, params String[] models)
        {
            this.models = models;
            this.uri = uri;
        }
        internal class XHRer : IJSON
        {
            public XHRer(String ex)
            {
                this.expression = ex;
            }
            string expression;
            #region IJSONConvert Members

            void IJSON.Write(System.IO.TextWriter writer)
            {
                writer.Write(expression);
            }

            void IJSON.Read(string key, object value)
            {
            }

            #endregion
        }
        protected IJSON Expression(string xhr)
        {
            return new XHRer(xhr);
        }
        class XHRFlow : WebFlow
        {
            private Uri uri;
            public XHRFlow(Uri uri)
            {
                this.uri = uri;

            }
            public override WebActivity GetFirstActivity()
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(new Uri(this.uri, "/").AbsoluteUri);
                WebRequest req = this.Context.Request;
                sb.Append(Data.Utility.GetRoot(req.Url));
                sb.Append("/");
                sb.Append(AccessToken.Token);
                sb.Append("/");
                if (req.Headers.ContainsKey(EventType.Dialog))
                {
                    WebMeta meta = req.Headers.GetMeta(EventType.Dialog);
                    if (meta != null)
                    {
                        var em = meta.GetDictionary().GetEnumerator();
                        var isOne = true;
                        while (em.MoveNext())
                        {
                            if (isOne)
                            {
                                sb.Append("?");
                                isOne = false;
                            }
                            else
                            {
                                sb.Append("&");
                            }
                            sb.Append(Uri.UnescapeDataString(em.Key.ToString()));
                            sb.Append("=");
                            sb.Append(Uri.UnescapeDataString(em.Value.ToString()));


                        }
                    }
                    else
                    {
                        String dg = req.Headers.Get(EventType.Dialog);
                        sb.Append("?");
                        sb.Append(Uri.UnescapeDataString(dg)); ;// em.Key.ToString()));


                    }
                }
                else
                {
                    sb.Append(req.Model);
                    sb.Append("/");
                    sb.Append(req.Command);
                    sb.Append("/");
                    WebMeta meta = req.SendValues;// ();
                    if (meta != null)
                    {
                        var em = meta.GetDictionary().GetEnumerator();
                        var isOne = true;
                        while (em.MoveNext())
                        {
                            if (isOne)
                            {
                                sb.Append("?");
                                isOne = false;
                            }
                            else
                            {
                                sb.Append("&");
                            }
                            sb.Append(Uri.UnescapeDataString(em.Key.ToString()));
                            sb.Append("=");
                            sb.Append(Uri.UnescapeDataString(em.Value.ToString()));


                        }

                    }
                    else
                    {

                        String dg = req.SendValue;
                        if (String.IsNullOrEmpty(dg) == false)
                        {
                            sb.Append("?");
                            sb.Append(Uri.UnescapeDataString(dg)); ;// em.Key.ToString()));

                        }

                    }

                }

                var http = new System.Net.Http.HttpClient();
                http.DefaultRequestHeaders.Add("user-agent", req.UserAgent);
                String xhr = http.GetStringAsync(sb.ToString()).Result;

                String eventPfx = "{\"ClientEvent\":";
                if (xhr.StartsWith(eventPfx))
                {

                    int index = xhr.IndexOf(",");
                    if (index > -1)
                    {
                        var webEvent = (WebEvent)(Utility.Parse(xhr.Substring(eventPfx.Length, index - eventPfx.Length), 0));
                        if ((webEvent & WebEvent.AsyncDialog) == WebEvent.AsyncDialog)
                        {

                            this.Context.Response.Redirect(new XHRer(xhr));
                        }
                        else
                        {
                            this.Context.Response.Redirect(Data.JSON.Expression(xhr));
                        }
                    }
                    else
                    {
                        this.Context.Response.Redirect(Data.JSON.Expression(xhr));
                    }
                }
                else
                {
                    this.Context.Response.Redirect(Data.JSON.Expression(xhr));
                }

                return WebActivity.Empty;
            }
        }
        public virtual WebFlow GetFlowHandler(string mode)
        {
            if (this.models != null && this.uri != null)
            {
                foreach (var m in this.models)
                {
                    if (m == mode)
                        return new XHRFlow(this.uri);
                }
            }

            return WebFlow.Empty;
        }
        /// <summary>
        /// 请在此方法中完成url与model的注册,即调用registerModel方法
        /// </summary>
        /// <param name="context"></param>
        public abstract void OnInit(WebContext context);
    }
}