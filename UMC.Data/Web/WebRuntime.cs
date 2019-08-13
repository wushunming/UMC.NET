using System;
using System.Collections.Generic;
using System.Text;
using UMC.Data;
using System.IO;
using UMC.Configuration;
using System.Threading.Tasks;
using System.Collections;

namespace UMC.Web
{
    class WebRuntime : IDisposable
    {
        public Hashtable Items
        {
            get
            {
                return Client.Items;
            }
        }
        public WebRuntime(WebClient client, System.Collections.IDictionary header)
        {
            this.Request = client.Session.Request();
            this.Request.OnInit(client, header);
            this.Response = client.Session.Response();
            this.Response.OnInit(client);
            this.Client = client;
        }
        ~WebRuntime()
        {
            GC.SuppressFinalize(this);
        }
        public string UserHostAddress
        {
            get;
            internal set;

        }
        public WebClient Client
        {
            get;
            set;
        }
        public static WebContext Start(WebClient client)
        {
            WebRuntime runtime = new WebRuntime(client, new Hashtable());
            Current = runtime;
            runtime.Context = client.Session.Context();
            try
            {
                runtime.Context.Init(runtime);
                runtime.Context.OnInit(client);
            }
            catch (UMC.Web.WebAbortException)
            {
            }

            return runtime.Context;

        }
        public static WebContext ProcessRequest(System.Collections.IDictionary header, WebClient client)
        {
            WebRuntime runtime = new WebRuntime(client, header);

            var value = System.Threading.Thread.CurrentPrincipal;
            runtime.DoFactory(value);
            return runtime.Context;

        }
        [ThreadStatic]
        static WebRuntime _Current;
        /// <summary>
        /// 当前的处理工厂
        /// </summary>
        public static WebRuntime Current
        {
            get
            {
                return _Current;
            }
            private set
            {
                _Current = value;
            }
        }


        internal WebFlow CurrentFlow
        {
            get;
            set;
        }
        internal IWebFactory CurrentFlowFactory
        {
            get;
            set;
        }

        /// <summary>
        /// POS交易上下文
        /// </summary>
        public WebContext Context
        {
            get;
            set;
        }

        public WebActivity CurrentActivity
        {
            get;
            set;
        }
        static WebRuntime()
        {
            var als = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in als)//mscorlib, 
            {
                var mpps = a.GetCustomAttributes(typeof(MappingAttribute), false);
                if (mpps.Length > 0)
                {

                    Register(a);
                }
            }
        }
        static void Register(System.Reflection.Assembly assembly)
        {

            var types = assembly.GetTypes();
            foreach (var t in types)
            {
                var mpps = t.GetCustomAttributes(typeof(MappingAttribute), false);
                foreach (var m in mpps)
                {
                    var mp = m as MappingAttribute;
                    if (String.IsNullOrEmpty(mp.Command) == false && String.IsNullOrEmpty(mp.Model) == false)
                    {
                        if (typeof(WebActivity).IsAssignableFrom(t))
                        {
                            if (activities.ContainsKey(mp.Model) == false)
                            {
                                activities.Add(mp.Model, new Dictionary<string, Type>());
                            }
                            activities[mp.Model][mp.Command] = t;

                            authKeys[String.Format("{0}.{1}", mp.Model, mp.Command)] = mp.Auth;
                        }
                    }
                    else if (String.IsNullOrEmpty(mp.Model) == false)
                    {
                        if (typeof(WebFlow).IsAssignableFrom(t))
                        {
                            if (flows.ContainsKey(mp.Model) == false)
                            {
                                flows.Add(mp.Model, new List<Type>()); ;
                            }
                            var list = flows[mp.Model];
                            if (list.Contains(t) == false)
                            {
                                list.Add(t);
                                authKeys[mp.Model] = mp.Auth;
                            }
                        }

                    }
                    else
                    {
                        if (typeof(IWebFactory).IsAssignableFrom(t))
                        {
                            if (webFactorys.Contains(t) == false)
                            {
                                if (typeof(WebFactory).IsAssignableFrom(t))
                                {
                                    webFactorys.Insert(0, t);
                                }
                                else
                                {
                                    webFactorys.Add(t);
                                }
                            }
                        }

                    }
                }
            }
        }

        internal static Dictionary<String, WebAuthType> authKeys = new Dictionary<String, WebAuthType>();
        internal static List<Type> webFactorys = new List<Type>();
        internal static Dictionary<String, List<Type>> flows = new Dictionary<string, List<Type>>();
        internal static Dictionary<String, Dictionary<String, Type>> activities = new Dictionary<String, Dictionary<string, Type>>();
        class MappingFLow : WebFlow
        {

            public override WebActivity GetFirstActivity()
            {
                var webRequest = this.Context.Request;
                var dic = activities[webRequest.Model];
                if (dic.ContainsKey(webRequest.Command))
                {
                    return Reflection.CreateInstance(dic[webRequest.Command]) as WebActivity;

                }
                else
                {
                    return WebActivity.Empty;
                }
            }
        }
        class MappingActivityFactory : IWebFactory
        {
            int index = 0;

            WebFlow IWebFactory.GetFlowHandler(string mode)
            {
                if (activities.ContainsKey(mode))
                {

                    return new MappingFLow();

                }
                return WebFlow.Empty;

            }

            void IWebFactory.OnInit(WebContext context)
            {

            }
        }
        class MappingFlowFactory : IWebFactory
        {
            int index = 0;
            public MappingFlowFactory(int i)
            {
                this.index = i;
            }
            WebFlow IWebFactory.GetFlowHandler(string mode)
            {
                if (flows.ContainsKey(mode))
                {
                    return Reflection.CreateInstance(flows[mode][index]) as WebFlow;
                }
                return WebFlow.Empty;

            }

            void IWebFactory.OnInit(WebContext context)
            {

            }
            public static IWebFactory[] GetFactory(String model)
            {
                if (flows.ContainsKey(model))
                {
                    var len = flows[model].Count;
                    var list = new List<IWebFactory>();
                    for (var i = 0; i < len; i++)
                    {
                        list.Add(new MappingFlowFactory(i));

                    }
                    return list.ToArray();
                }
                return new IWebFactory[0];

            }
        }
        void DoFactory()
        {

            Context = this.Client.Session.Context();

            Context.Init(this);
            Context.OnInit(this.Client);
            var factorys = new List<IWebFactory>();


            foreach (var ftype in webFactorys)
            {
                var flowFactory = Reflection.CreateInstance(ftype) as IWebFactory;
                if (flowFactory != null)
                {

                    factorys.Add(flowFactory);
                    flowFactory.OnInit(Context);
                }
            }
            factorys.AddRange(MappingFlowFactory.GetFactory(Context.Request.Model));

            factorys.Add(new MappingActivityFactory());
            foreach (var factory in factorys)
            {
                this.CurrentFlowFactory = factory;

                var flow = factory.GetFlowHandler(this.Request.Model);
                flow.Context = this.Context;

                this.CurrentFlow = flow;

                ProcessActivity(flow, flow.GetFirstActivity());
            }
            Context.Complete();
        }
        protected void DoFactory(System.Security.Principal.IPrincipal pi)
        {
            System.Threading.Thread.CurrentPrincipal = pi;
            WebRuntime.Current = this;

            if ((this.Response.ClientEvent & WebEvent.Error) == WebEvent.Error)
            {
                return;
            }
            if (String.IsNullOrEmpty(UMC.Security.AccessToken.Get("Debug")))
            {
                try
                {
                    DoFactory();

                }
                catch (UMC.Web.WebAbortException)
                {
                }
                catch (Exception ex)
                {
                    this.Response.ClientEvent = WebEvent.Error;
                    this.Response.Headers["Error"] = ex.Message;
                    UMC.Data.Utility.Error("POS", DateTime.Now, this.Request.Url.AbsoluteUri, ex.ToString());
                }

            }
            else
            {
                try
                {
                    DoFactory();

                }
                catch (UMC.Web.WebAbortException)
                {
                }
            }

        }


        void ProcessActivity(WebFlow flow, WebActivity active)
        {
            active.Flow = flow;
            active.Context = this.Context;
            this.CurrentActivity = active;
            active.ProcessActivity(this.Request, this.Response);
            if (active == WebActivity.Empty)
            {
            }
            else
            {
                ProcessActivity(flow, flow.GetNextActivity(active.Id));
            }
        }
        public WebRequest Request
        {
            get;
            private set;
        }
        public WebResponse Response
        {
            get;
            private set;
        }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
