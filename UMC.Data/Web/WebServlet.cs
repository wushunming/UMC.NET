using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Collections;
using UMC.Configuration;
using System.Collections.Generic;


namespace UMC.Web
{
    /// <summary>
    /// POS处理
    /// </summary>
    public class WebServlet : UMC.Net.INetHandler
    {


        static void Process(UMC.Net.NetContext context, String model, string cmd)
        {
            if (String.IsNullOrEmpty(context.UserAgent))
            {
                return;
            }
            if (context.UrlReferrer != null)
            {
                context.AddHeader("Access-Control-Allow-Origin", String.Format("{0}://{1}", context.UrlReferrer.Scheme, context.UrlReferrer.Host));
            }
            else
            {

                context.AddHeader("Access-Control-Allow-Origin", "*");
            }
            context.AddHeader("Access-Control-Allow-Credentials", "true");
            context.AddHeader("Access-Control-Allow-Methods", "OPTIONS, GET, POST");
            context.AddHeader("Access-Control-Allow-Headers", "x-requested-with,content-type");

            var QueryString = new NameValueCollection(context.QueryString);
            QueryString.Add(context.Form);
            if (string.IsNullOrEmpty(model))
            {
                model = QueryString["_model"];
                cmd = QueryString["_cmd"];

            }
            var jsonp = context.QueryString.Get("jsonp");
            if ("Upload".Equals(model) && "Command".Equals(cmd))
            {
                try
                {

                    var url = new Uri(QueryString["src"]);
                    var wb = new Net.HttpClient();
                    var hash = UMC.Data.JSON.Deserialize<Hashtable>(wb.GetAsync(url).Result.Content.ReadAsStringAsync().Result).GetEnumerator();

                    QueryString.Clear();
                    while (hash.MoveNext())
                    {
                        QueryString.Add(hash.Key.ToString(), hash.Value.ToString());
                    }
                    model = QueryString["_model"];
                    cmd = QueryString["_cmd"];
                    if (String.IsNullOrEmpty(jsonp) == false)
                    {
                        QueryString.Add("jsonp", jsonp);
                    }
                }
                catch
                {
                    return;
                }
            }
            context.ContentType = "text/javascript;charset=utf-8";

            var Url = context.Url;
            var ip = context.Headers.Get("X-Real-IP");
            if (String.IsNullOrEmpty(ip))
            {
                ip = context.UserHostAddress; ;
            }
            var cahash = context.Headers.Get("CA-Host");
            if (String.IsNullOrEmpty(cahash) == false)
            {

                Url = new Uri(String.Format("https://{1}{0}", context.Url.PathAndQuery, cahash));
            }


            Process(QueryString, context.InputStream, context.OutputStream, Url, context.UrlReferrer, ip, context.UserAgent, model, cmd, url =>
               {
                   context.Redirect(url.AbsoluteUri);
               });
        }

        static void Process(NameValueCollection nvs, System.IO.Stream input, System.IO.Stream Output,
            Uri Url, Uri UrlReferrer, String UserHostAddress, String UserAgent, string model, string cmd, Action<Uri> redirec)
        {
            NameValueCollection QueryString = new NameValueCollection();

            if (String.IsNullOrEmpty(model))
            {
                if (Url.Segments.Length > 4)
                {
                    var paths = new System.Collections.Generic.List<string>(Url.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
                    if (paths.Count > 0)
                        paths.RemoveAt(0);
                    if (paths.Count > 0)
                        paths.RemoveAt(0);
                    if (paths.Count > 0)
                    {
                        if (paths[paths.Count - 1].IndexOf('.') > -1)
                        {
                            paths.RemoveAt(paths.Count - 1);
                        }
                    }

                    if (paths.Count > 1)
                    {
                        model = paths[0];
                        cmd = paths[1];
                    }
                    if (paths.Count > 2)
                    {
                        QueryString.Add(null, paths[2]);

                    }

                }
            }
            string start = nvs.Get("_start");
            for (var i = 0; i < nvs.Count; i++)
            {
                var key = nvs.GetKey(i);
                if (String.IsNullOrEmpty(key))
                {
                    QueryString.Add(null, nvs.Get(i));
                }
                else if (!key.StartsWith("_"))
                {
                    QueryString.Add(key, nvs.Get(i));
                }
            }
            var jsonp = QueryString.Get("jsonp");
            QueryString.Remove("jsonp");

            var zip = Output;
            var writer = new System.IO.StreamWriter(zip);
            switch (model)
            {
                case "System":
                    if (String.IsNullOrEmpty(jsonp) == false)
                    {
                        writer.Write(jsonp);
                        writer.Write('(');
                    }
                    switch (cmd)
                    {
                        case "TimeSpan":
                            writer.Write(Data.Utility.TimeSpan());
                            break;
                        case "Mapping":
                            Data.JSON.Serialize(Mapping(), writer);
                            break;
                        case "Debug":
                            if (String.IsNullOrEmpty(UMC.Security.AccessToken.Get("Debug")))
                            {
                                UMC.Security.AccessToken.Set("Debug", "OK");
                                writer.Write("{\"Text\":\"当前账户开启了调试模式\"}");
                            }
                            else
                            {

                                UMC.Security.AccessToken.Set("Debug", null);
                                writer.Write("{\"Text\":\"当前账户关闭了调试模式\"}");
                            }
                            break;
                    }
                    if (String.IsNullOrEmpty(jsonp) == false)
                    {
                        writer.Write(")");
                    }
                    writer.Flush();
                    return;
            }

            if (Url.Host.StartsWith("127.0"))
            {
                Url = new Uri(new Uri("https://api.365lu.cn"), Url.PathAndQuery);
            }

            var client = new WebClient(Url, UrlReferrer, UserHostAddress, UserAgent);
            client.InputStream = input;
            if (String.IsNullOrEmpty(jsonp) == false && jsonp.StartsWith("app"))
            {
                client.IsApp = true;
            }

            if (String.IsNullOrEmpty(start) == false)
            {
                client.Start(start);
            }
            else if (String.IsNullOrEmpty(model))
            {

                client.SendDialog(QueryString);
            }
            else
            {
                if (String.IsNullOrEmpty(cmd))
                {
                    if (model.StartsWith("[") == false)
                    {
                        throw new Exception("Command is empty");
                    }
                }
                else
                {
                    client.Command(model, cmd, QueryString);
                }
            }


            if (String.IsNullOrEmpty(model) == false && model.StartsWith("[") && String.IsNullOrEmpty(cmd))
            {
                client.JSONP(model, jsonp, writer);
            }
            else
            {
                if (String.IsNullOrEmpty(jsonp) == false)
                {
                    writer.Write(jsonp);
                    writer.Write('(');
                }
                client.WriteTo(writer, redirec);
                if (String.IsNullOrEmpty(jsonp) == false)
                {
                    writer.Write(")");
                }
            }
            writer.Flush();
            zip.Close();
        }
        protected virtual bool Authorization(UMC.Net.NetContext context)
        {

            var path = context.Url.LocalPath;
            var paths = new List<string>(path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            if (paths.Count > 0)
            {
                if (paths[paths.Count - 1].IndexOf('.') > -1)
                {
                    paths.RemoveAt(paths.Count - 1);
                }
            }
            else
            {
                return false;
            }

            var CookieKey = String.Empty;

            if (paths.Count == 2 || paths.Count >= 4)
            {
                CookieKey = paths[1];
            }
            var cookie = String.IsNullOrEmpty(CookieKey) ? context.Cookies[Membership.SessionCookieName] : CookieKey;
            var sessionKey = Guid.Empty;
            string contentType = "Client/" + context.UserHostAddress;
            if (UMC.Data.Utility.IsApp(context.UserAgent))
            {
                contentType = "App/" + context.UserHostAddress;
            }
            if (String.IsNullOrEmpty(cookie) == false)
            {
                sessionKey = UMC.Data.Utility.Guid(cookie, true) ?? Guid.Empty;
            }
            if (sessionKey == Guid.Empty)
            {

                sessionKey = Guid.NewGuid();
                context.AppendCookie(Membership.SessionCookieName, UMC.Data.Utility.Guid(sessionKey));

                var user = new UMC.Security.Guest(sessionKey);
                UMC.Security.Principal.Create(user, UMC.Security.AccessToken.Create(user, sessionKey, contentType, 0));
            }
            else
            {
                UMC.Security.Membership.Instance().Authorization(sessionKey, contentType);

            }
            var urf = context.UrlReferrer;
            if (urf != null)
            {
                if (String.IsNullOrEmpty(urf.Query) == false)
                {
                    var query = System.Web.HttpUtility.ParseQueryString(urf.Query.Substring(1));
                    var sp = UMC.Data.Utility.Guid(query["sp"]);
                    if (sp.HasValue)
                    {
                        if (String.Equals(UMC.Security.AccessToken.Get("Spread-Id"), sp.ToString()) == false)
                        {
                            UMC.Security.AccessToken.Set("Spread-Id", sp.ToString());
                        }
                    }
                }
            }
            return true;


        }
        public static List<WebMeta> Auths()
        {
            List<WebMeta> metas = new List<WebMeta>();
            if (WebRuntime.flows.Count > 0)
            {
                var em = WebRuntime.flows.GetEnumerator();
                while (em.MoveNext())
                {

                    MappingAttribute mapping = (MappingAttribute)em.Current.Value[0].GetCustomAttributes(typeof(MappingAttribute), false)[0];



                    WebAuthType authType = WebRuntime.authKeys[em.Current.Key];
                    if (authType == WebAuthType.Check || authType == WebAuthType.UserCheck)
                    {
                        metas.Add(new WebMeta().Put("key", em.Current.Key + ".*").Put("desc", mapping.Desc));


                    }


                }
            }

            if (WebRuntime.activities.Count > 0)
            {
                var em = WebRuntime.activities.GetEnumerator();
                while (em.MoveNext())
                {
                    var em3 = em.Current.Value.GetEnumerator();
                    while (em3.MoveNext())
                    {
                        MappingAttribute mapping = (MappingAttribute)em3.Current.Value.GetCustomAttributes(typeof(MappingAttribute), false)[0];

                        WebAuthType authType = WebRuntime.authKeys[em.Current.Key];
                        if (authType == WebAuthType.Check || authType == WebAuthType.UserCheck)
                        {
                            metas.Add(new WebMeta().Put("key", mapping.Model + "." + mapping.Command).Put("desc", mapping.Desc));


                        }

                    }



                }
            }
            return metas;

        }


        #region IMIMEHandler Members

        public virtual void ProcessRequest(UMC.Net.NetContext context)
        {
            if (Authorization(context))
            {

                string Prefix = UMC.Data.Utility.GetPrefix(context.Url);
                if (String.IsNullOrEmpty(Prefix))
                {

                    Process(context, string.Empty, string.Empty);
                }
                else
                {
                    UMC.Net.INetHandler handler = UMC.Data.Reflection.CreateObject(UMC.Data.Reflection.Configuration("payment"), Prefix)
                               as
                    UMC.Net.INetHandler;
                    handler.ProcessRequest(context);
                }
            }
            else
            {
                Data.JSON.Serialize(Mapping(), context.Output);
            }


        }

        #endregion
        static List<WebMeta> Mapping()
        {

            List<WebMeta> metas = new List<WebMeta>();
            if (WebRuntime.webFactorys.Count > 0)
            {
                foreach (Type t in WebRuntime.webFactorys)
                {
                    WebMeta meta = new WebMeta();
                    meta.Put("type", t.FullName);
                    meta.Put("name", "." + t.Name);
                    metas.Add(meta);

                    MappingAttribute mapping = (MappingAttribute)t.GetCustomAttributes(typeof(MappingAttribute), false)[0];
                    if (String.IsNullOrEmpty(mapping.Desc) == false)
                    {
                        meta.Put("desc", mapping.Desc);

                    }

                }

            }
            if (WebRuntime.flows.Count > 0)
            {
                var em = WebRuntime.flows.GetEnumerator();
                while (em.MoveNext())
                {
                    var tls = em.Current.Value;
                    foreach (Type t in tls)
                    {
                        WebMeta meta = new WebMeta();
                        meta.Put("type", t.FullName);
                        meta.Put("name", em.Current.Key + ".");
                        meta.Put("auth", WebRuntime.authKeys[em.Current.Key].ToString().ToLower());
                        meta.Put("model", em.Current.Key);//.getKey())
                        metas.Add(meta);

                        MappingAttribute mapping = (MappingAttribute)t.GetCustomAttributes(typeof(MappingAttribute), false)[0];
                        if (String.IsNullOrEmpty(mapping.Desc) == false)
                        {
                            meta.Put("desc", mapping.Desc);

                        }

                    }


                }
            }
            if (WebRuntime.activities.Count > 0)
            {
                var em = WebRuntime.activities.GetEnumerator();
                while (em.MoveNext())
                {
                    var em3 = em.Current.Value.GetEnumerator();
                    while (em3.MoveNext())
                    {
                        MappingAttribute mapping = (MappingAttribute)em3.Current.Value.GetCustomAttributes(typeof(MappingAttribute), false)[0];

                        WebAuthType authType = mapping.Auth;// WebRuntime.authKeys[em.Current.Key];

                        WebMeta meta = new WebMeta();
                        meta.Put("type", em3.Current.Value.FullName);
                        meta.Put("name", mapping.Model + "." + mapping.Command);
                        meta.Put("auth", authType.ToString().ToLower());
                        meta.Put("model", mapping.Model);//.getKey())
                        meta.Put("cmd", mapping.Command);//.getKey())
                        metas.Add(meta);

                        if (String.IsNullOrEmpty(mapping.Desc) == false)
                        {
                            meta.Put("desc", mapping.Desc);

                        }


                    }



                }
            }
            return metas;
        }

    }
}