using System;
using System.Collections.Generic;
using System.Text;
using UMC.Data.Sql;
using UMC.Data.Entities;

namespace UMC.Configuration
{
    /// <summary>
    /// 会话对象
    /// </summary>
    /// <typeparam name="T">会员类型</typeparam>
    public class Session<T>
    {
        /// <summary>
        /// 会话值
        /// </summary>
        public T Value
        {
            get;
            private set;
        }
        /// <summary>
        /// 会话Key
        /// </summary>
        public string Key
        {
            get;
            private set;
        }
        public string ContentType
        {
            get;
            set;
        }
        Guid _user_id;
        public Session(string sessionKey, UMC.Security.Identity id)
        {
            this.Key = sessionKey;
            var se = GSession(sessionKey, id, false);
            if (se != null)
            {
                this.ContentType = se.ContentType;

                if (typeof(T) == typeof(string))
                {
                    object obj = se.Content;
                    this.Value = (T)obj;
                }
                else
                {
                    this.Value = UMC.Data.JSON.Deserialize<T>(se.Content);
                }
                _user_id = se.user_id ?? Guid.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="IsDefault">如果IsDefault为true，如果不存在当前当前用户的会话值 ，则以WebADNuke用户的值，作为默认值以</param>
        public Session(string sessionKey, bool IsDefault)
        {
            //this.Value = value;
            this.Key = sessionKey;
            var se = GSession(sessionKey, UMC.Security.Identity.Current, IsDefault);
            if (se != null)
            {
                this.ContentType = se.ContentType;

                if (typeof(T) == typeof(string))
                {
                    object obj = se.Content;
                    this.Value = (T)obj;
                }
                else
                {
                    this.Value = UMC.Data.JSON.Deserialize<T>(se.Content);
                }
                this.ModifiedTime = se.UpdateTime ?? DateTime.MinValue;
            }
        }

        public DateTime ModifiedTime
        {
            get;
            private set;
        }
        public Session(string sessionKey)
            : this(sessionKey, true)
        {
        }
        static Session GSession(string SessionKey, UMC.Security.Identity user, bool IsIsDefault)
        {
            if (String.IsNullOrEmpty(SessionKey) == false)
            {
                var sessionEneity = UMC.Data.Database.Instance().ObjectEntity<Session>();
                sessionEneity.Where.And(new Session { SessionKey = SessionKey });
                //if (user != null)
                //{
                //    if (IsIsDefault)
                //    {
                //        sessionEneity.Where.And().In(new Data.Entities.Session { user_id = user.Id }, Guid.Empty);
                //    }
                //    else
                //    {
                //        sessionEneity.Where.And().Equal(new Data.Entities.Session { user_id = user.Id });//, WebADNuke.Security.Membership.Sharename);
                //    }

                //}
                //else if (IsIsDefault)
                //{
                //    sessionEneity.Where.And().In(new Data.Entities.Session { user_id = Guid.Empty });
                //}

                var sess = sessionEneity.Query(0, 3);
                switch (sess.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return sess[0];
                    default:
                        if (user == null)
                        {
                            return sess[0];
                        }
                        else
                        {
                            if (sess[0].user_id == user.Id)
                            {
                                return sess[0];
                            }
                            else
                            {
                                return sess[1];
                            }
                        }
                }
            }
            return null;
        }
        public Session(T value, string sessionKey)
        {
            this.Value = value;
            this.Key = sessionKey;
        }
        /// <summary>
        /// 提交更改
        /// </summary>
        public void Commit()
        {
            this.Commit(this._user_id);
        }
        /// <summary>
        /// 提交更改
        /// </summary>
        public void Commit(UMC.Security.Identity id)
        {
            this.Commit(id.Id ?? Guid.Empty);
        }
        /// <summary>
        /// 提交更改,且消除用户contentType类型的Sesion
        /// </summary>
        public void Commit(UMC.Security.Identity id, string contentType)
        {
            this.ContentType = contentType;
            var sessionEneity = UMC.Data.Database.Instance().ObjectEntity<Session>();

            sessionEneity.Where.And().Equal(new Session { ContentType = contentType, user_id = id.Id });
            sessionEneity.Delete();
            this.Commit(Guid.Empty, id.Id ?? Guid.Empty);
        }
        public void Commit(T value, UMC.Security.Identity id)
        {
            this.Value = value;
            this.Commit(id, "app/json");
        }

        public void Commit(T value, params Guid[] ids)
        {
            this.Commit(ids);
        }
        /// <summary>
        /// 提交更改
        /// </summary>
        public void Commit(params Guid[] ids)
        {


            var session = new Session
            {
                UpdateTime = DateTime.Now,
                user_id = ids[ids.Length - 1],
                ContentType = this.ContentType ?? "text/javascript",
                SessionKey = this.Key
            };
            if (this.Value is string)
            {
                session.Content = this.Value as string;
            }
            else
            {
                session.Content = UMC.Data.JSON.Serialize(this.Value, "ts");
            }
            this.ModifiedTime = DateTime.Now;
            //System.Web.HttpRuntime.Cache.Insert(this.Key, session);
            UMC.Data.Database database = UMC.Data.Database.Instance();
            database.BeginTransaction();
            try
            {
                var sessionEneity = database.ObjectEntity<Session>();
                sessionEneity.Where.And().Equal(new Session
                {
                    SessionKey = this.Key,
                });
                if (ids[0] == Guid.Empty)
                {
                    sessionEneity.Delete();
                    if (session.user_id.Value != Guid.Empty)
                    {
                        sessionEneity.Insert(session);
                    }
                }
                else
                {
                    if (ids.Length > 1)
                    {
                        sessionEneity.Delete();
                        if (session.user_id.Value != Guid.Empty)
                        {
                            sessionEneity.Insert(session);
                        }
                    }
                    else
                    {
                        if (session.user_id.Value != Guid.Empty)
                        {
                            sessionEneity.Where.And().In(new Session { user_id = ids[0] });
                            if (sessionEneity.Update(session) == 0)
                            {
                                if (session.user_id.Value != Guid.Empty)
                                {
                                    sessionEneity.Insert(session);
                                }
                            }
                        }
                        else
                        {
                            sessionEneity.Delete();
                        }
                    }
                }
                database.Commit();
            }
            catch
            {
                database.Rollback();
            }
        }
    }

}
