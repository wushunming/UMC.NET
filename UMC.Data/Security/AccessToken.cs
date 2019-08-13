using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace UMC.Security
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    public class AccessToken
    {
        //public const String CONTENT_TYPE = "LOGIN/AUTH";
        public String ContentType
        {
            get;
            set;
        }
        private AccessToken()
        {
            this.Data = new Hashtable();
        }
        /// <summary>
        /// 过期时间，单位分钟，0为不过期
        /// </summary>
        public int Timeout
        {
            get;
            private set;
        }
        public AccessToken(Guid tmpId)
            : this()
        {
            this.Id = tmpId;
        }
        public string Username
        {
            get;
            private set;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? SId
        {
            get;
            private set;
        }
        /// <summary>
        /// 关联的ID
        /// </summary>
        public Guid? Id
        {
            get;
            set;
        }
        /// <summary>
        /// 角色
        /// </summary>
        public string Roles
        {
            get;
            private set;
        }
        /// <summary>
        /// 最后一次活动时间
        /// </summary>
        public int? ActiveTime
        {
            get;
            private set;
        }
        [UMC.Data.JSON]
        public Hashtable Data
        {
            get;
            private set;
        }
        /// <summary>
        /// 客户端的唯一标识
        /// </summary>
        public static Guid? Token
        {
            get
            {
                var data = System.Threading.Thread.CurrentPrincipal as UMC.Security.Principal;
                if (data != null)
                {
                    var ticket = data.SpecificData as AccessToken;
                    if (ticket != null)
                    {
                        return ticket.Id;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void SignOut()
        {
            var data = System.Threading.Thread.CurrentPrincipal as UMC.Security.Principal;
            var ticket = data.SpecificData;//as AccessToken;
            if (ticket != null)
            {
                Login(UMC.Security.Identity.Create(ticket.Id.Value, "?", String.Empty), ticket.Id.Value, ticket.ContentType);
            }
        }
        public static AccessToken Activation(string Username, AccessToken auth, string client)
        {
            switch (Username)
            {
                case "#":
                case "?":
                    var Token = Create(auth.Identity(), auth.Id.Value, client, auth.Timeout);
                    UMC.Security.Membership.Instance().Activation(Token);
                    return Token;
                default:
                    Identity Id = UMC.Security.Membership.Instance().Identity(Username);
                    var Token2 = Create(Id, auth.Id.Value, client, auth.Timeout);
                    UMC.Security.Membership.Instance().Activation(Token2);
                    return Token2;

            }

        }
        public AccessToken Put(string key, string value)
        {
            if (String.IsNullOrEmpty(key) == false)
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.Data.Remove(key);
                }
                else
                {
                    this.Data[key] = value;
                }
            }
            return this;
        }
        public static AccessToken Create(Identity Id, Guid tmpId, String contentType, int timeout)
        {
            var auth = new AccessToken();
            auth.ContentType = contentType;
            auth.Timeout = timeout;
            auth.Id = tmpId;
            auth.Username = Id.Name;
            auth.SId = Id.Id;
            auth.ActiveTime = UMC.Data.Utility.TimeSpan();
            auth.Roles = null;

            switch (Id.Name)
            {
                case "#":
                case "?":
                    if (String.IsNullOrEmpty(Id.Alias) == false)
                    {
                        auth.Data["#"] = Id.Alias;
                    }
                    break;
                default:
                    ;
                    auth.Data["#"] = Id.Alias;
                    if (Id.Roles != null)
                    {
                        auth.Roles = String.Join(",", Id.Roles);
                    }
                    break;
            }
            return auth;
        }
        public AccessToken Put(System.Collections.Specialized.NameValueCollection NameValue)
        {
            for (var i = 0; i < NameValue.Count; i++)
            {
                var key = NameValue.GetKey(i);
                if (String.IsNullOrEmpty(key) == false)
                {
                    var value = NameValue.Get(i);
                    if (String.IsNullOrEmpty(value))
                    {
                        this.Data.Remove(key);
                    }
                    else
                    {
                        this.Data[key] = value;
                    }
                }
            }
            return this;
        }
        /// <summary>
        /// 提交修改访问票据
        /// </summary>
        public void Commit()
        {
            this.ActiveTime = UMC.Data.Utility.TimeSpan();///
            UMC.Security.Membership.Instance().Activation(this);
        }
        public UMC.Security.Identity Identity()
        {

            var Alias = this.Data["#"] as string ?? String.Empty;
            int cuttime = UMC.Data.Utility.TimeSpan();
            if (this.Timeout > 0 && ((this.ActiveTime ?? 0) + this.Timeout) <= cuttime)
            {
                return UMC.Security.Identity.Create(this.Id.Value, "?", Alias);
            }
            if (String.IsNullOrEmpty(this.Username))
            {
                return UMC.Security.Identity.Create(this.Id.Value, "?", Alias);
            }
            switch (this.Username)
            {
                case "?":
                    return UMC.Security.Identity.Create(this.SId ?? this.Id.Value, "?", Alias);
                case "#":
                    if (this.SId.HasValue)
                    {
                        return UMC.Security.Identity.Create(this.SId.Value, "#", Alias);
                    }
                    else
                    {
                        return UMC.Security.Identity.Create(this.Id.Value, "?", Alias);
                    }
                default:
                    if (this.SId.HasValue)
                    {
                        if (String.IsNullOrEmpty(this.Roles))
                        {
                            return UMC.Security.Identity.Create(this.SId.Value, this.Username, Alias);

                        }
                        else
                        {
                            return UMC.Security.Identity.Create(this.SId.Value, this.Username
                                , Alias, this.Roles.Split(new String[] { "," }, StringSplitOptions.None));
                        }
                    }
                    else
                    {
                        return UMC.Security.Identity.Create(this.Id.Value, "?", Alias);
                    }
                    break;
            }
        }
        /// <summary>
        /// 登录，默认30分钟过期
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="tmpId"></param>
        /// <returns></returns>
        public static AccessToken Login(Identity Id, Guid tmpId, string client)
        {
            return Login(Id, tmpId, 30, client);

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="tmpId"></param>
        /// <param name="contentType"></param>
        /// <param name="unqiue"></param>
        /// <returns></returns>
        public static AccessToken Login(Identity Id, Guid tmpId, string contentType, bool unqiue)
        {
            if (unqiue)
            {
                var token = Create(Id, tmpId, contentType, 0);

                var sesion = new Configuration.Session<UMC.Security.AccessToken>(token, token.Id.ToString());

                sesion.ContentType = contentType;

                sesion.Commit(Id, contentType);

                UMC.Security.Principal.Create(Id, token);
                return token;
            }
            else
            {
                return Login(Id, tmpId, 30, contentType);
            }

        }
        public static AccessToken Login(Identity Id, Guid tmpId, int timeout, string contentType)
        {
            UMC.Security.Principal.Create(Id);

            var auth = Create(Id, tmpId, contentType, timeout);// new AccessToken();

            UMC.Security.Membership.Instance().Activation(auth);
            return auth;

        }
        public static string Get(string key)
        {
            var data = System.Threading.Thread.CurrentPrincipal as UMC.Security.Principal;
            if (data != null)
            {
                var ticket = data.SpecificData as AccessToken;
                if (ticket != null)
                {
                    return ticket.Data[key] as string;
                }
            }
            return null;
        }
        public static void Set(string key, string value)
        {
            AccessToken.Current.Put(key, value).Commit();

        }
        public static AccessToken Current
        {
            get
            {
                var data = System.Threading.Thread.CurrentPrincipal as UMC.Security.Principal;
                if (data == null)
                {
                    return null;
                }
                return data.SpecificData as AccessToken;
            }
        }
        public static void Set(System.Collections.Specialized.NameValueCollection NameValue)
        {
            var data = System.Threading.Thread.CurrentPrincipal as UMC.Security.Principal;
            if (data == null)
            {
                return;
            }
            var ticket = data.SpecificData as AccessToken;
            if (ticket == null) { return; }
            for (var i = 0; i < NameValue.Count; i++)
            {
                var key = NameValue.GetKey(i);
                if (String.IsNullOrEmpty(key) == false)
                {
                    var value = NameValue.Get(i);
                    if (String.IsNullOrEmpty(value))
                    {
                        ticket.Data.Remove(key);
                    }
                    else
                    {
                        ticket.Data[key] = value;
                    }
                }
            }
            var iden = data.Identity as UMC.Security.Identity;
            if (iden.Id.HasValue)
            {
                UMC.Security.Membership.Instance().Activation(ticket);
            }
        }

    }
}
