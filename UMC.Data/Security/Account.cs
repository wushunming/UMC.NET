using System;
using System.Collections;
using System.Collections.Generic;
namespace UMC.Security
{

    /// <summary>
    /// 账户
    /// </summary>
    public class Account
    {

        public string ForId
        {
            get;
            private set;
        }

        /// <summary>
        /// 用户Mail
        /// </summary>
        public const int EMAIL_ACCOUNT_KEY = 1;
        /// <summary>
        /// 移动电话
        /// </summary>
        public const int MOBILE_ACCOUNT_KEY = 2;
        /// <summary>
        /// 手机MAC地址
        /// </summary>
        public const int MAC_ACCOUNT_KEY = 5;
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid user_id
        {
            get;
            private set;
        }
        public int Type
        {
            get;
            private set;
        }
        /// <summary>
        /// 账户名
        /// </summary>
        public string Name
        {
            get;
            private set;
        }
        /// <summary>
        /// 账户标签
        /// </summary>
        public UMC.Security.UserFlags Flags
        {
            get;
            private set;
        }
        private Account(UMC.Data.Entities.Account acc)
        {
            this.Name = acc.Name;
            this.Flags = acc.Flags ?? UMC.Security.UserFlags.Normal;
            this.ForId = acc.ForId;
            this.user_id = acc.user_id.Value;
            this.Type = acc.Type ?? 0;
            this.Items = UMC.Data.JSON.Deserialize<Hashtable>(acc.ConfigData) ?? new Hashtable();
        }

        public static Account Create(UMC.Data.Entities.Account acc)
        {
            return new Account(acc);
        }
        /// <summary>
        /// 更改数据配置
        /// </summary>
        public void Commit()
        {
            var entity = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
            entity.Where.And().Equal(new UMC.Data.Entities.Account { user_id = this.user_id, Name = this.Name });
            var data = new UMC.Data.Entities.Account { ConfigData = Data.JSON.Serialize(this.Items), ForId = this.ForId };
            if (entity.Update(data) == 0)
            {
                data.user_id = this.user_id;
                data.Name = this.Name;
                data.Type = this.Type;
                entity.Insert(data);
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public Hashtable Items
        {
            get;
            private set;
        }
        /// <summary>
        /// 获得关系
        /// </summary>
        /// <param name="main"></param>
        /// <param name="relations"></param>
        public static void GetRelation(Data.Entities.Account main, params Data.Entities.Account[] relations)
        {
            if (String.IsNullOrEmpty(main.Name) || main.Type.HasValue == false)
            {
                throw new ArgumentException("main 中的属性name和Type必须有值");
            }
            var names = new List<String>();
            names.Add(main.Name);
            var types = new List<int>();
            types.Add((int)main.Type.Value);
            foreach (var r in relations)
            {
                if (r.Type.HasValue)
                    types.Add(r.Type.Value);
                if (String.IsNullOrEmpty(r.Name) == false)
                {
                    names.Add(r.Name);
                }
            }
            var entity = UMC.Data.Database.Instance().ObjectEntity<Data.Entities.Account>();

            entity.Where.And().In(new Data.Entities.Account
            {
                Name = names[0]
            }, names.ToArray()).And().In(new Data.Entities.Account { Type = types[0] }, types.ToArray());

            var mids = new List<Guid>();
            Guid mid = Guid.NewGuid();
            var rels = new List<Data.Entities.Account>();

            entity.Order.Asc(new Data.Entities.Account { user_id = Guid.Empty });
            entity.Query(g =>
            {
                rels.Add(g);
                if (g.user_id.Value != mid)
                {
                    mid = g.user_id.Value;
                    mids.Add(mid);
                }

            });

            if (mids.Count > 0)
            {
                entity.Where.Reset().And().In(new Data.Entities.Account
                {
                    user_id = mids[0]
                }, mids.ToArray()).And().In(new Data.Entities.Account
                {
                    Type = main.Type
                }, types.ToArray());
                entity.Query(g =>
                {
                    rels.Add(g);
                });
            }
            var memberId = Guid.Empty;
            var orel = rels.FindAll(g => g.Type == main.Type);

            var om = orel.Find(g => String.Equals(main.Name, g.Name) && g.Type == main.Type);
            if (om == null)
            {
                if (relations.Length > 0)
                {
                    var rel = new List<Data.Entities.Account>(relations).Find(g => String.IsNullOrEmpty(g.Name) == false);
                    if (rel != null)
                    {
                        var v = rels.Find(g => String.Equals(g.Name, rel.Name) && rel.Type == g.Type);
                        if (v != null)
                        {
                            main.user_id = v.user_id;
                        }
                    }
                }
                if ((main.user_id ?? Guid.Empty) == Guid.Empty)
                {
                    main.user_id = Guid.NewGuid();
                }
                main.Flags = UMC.Security.UserFlags.Normal;

                entity.Where.Reset().And().Equal(new Data.Entities.Account
                {
                    Type = main.Type,
                    user_id = main.user_id
                }).Entities.IFF(e => e.Update(main) == 0, e => e.Insert(main));
            }
            else
            {
                if (main.user_id.HasValue)
                {
                    if (main.user_id.Value != om.user_id.Value)
                    {

                        entity.Where.Reset().And().Equal(new Data.Entities.Account { Type = main.Type, user_id = main.user_id })
                            .Entities.IFF(e => e.Update(main) == 0, e => e.Update(main));
                    }
                }
                else
                {
                    main.user_id = om.user_id;
                }
            }
            memberId = main.user_id.Value;
            foreach (var r in relations)
            {
                var mrel = rels.FindAll(g => g.Type == r.Type && memberId == g.user_id);
                if (String.IsNullOrEmpty(r.Name) == false)
                {
                    var mcards = mrel.FindAll(g => String.Equals(r.Name, g.Name, StringComparison.CurrentCultureIgnoreCase));

                    switch (mcards.Count)
                    {
                        case 0:
                            r.user_id = memberId;
                            if (mrel.Count > 0)
                            {
                                entity.Where.Reset().And().Equal(new Data.Entities.Account { Type = r.Type })
                                    .And().In(new Data.Entities.Account
                                    {
                                        user_id = memberId
                                    }, mids.ToArray()).Entities.Delete();
                            }
                            entity.Insert(r);
                            mrel.Add(r);
                            break;
                        default:

                            r.Flags = UMC.Security.UserFlags.Normal;
                            r.user_id = mcards[0].user_id;
                            r.Name = mcards[0].Name;
                            r.user_id = mcards[0].user_id;
                            r.ConfigData = mcards[0].ConfigData;
                            break;
                    }
                }
                else
                {
                    var m = mrel.Find(g => r.Type == g.Type && memberId == g.user_id);
                    if (m != null)
                    {
                        r.user_id = m.user_id;
                        r.Name = m.Name;
                        r.Flags = m.Flags;
                        r.ConfigData = m.ConfigData;
                    }
                }

            }

        }
        /// <summary>
        /// 验证Key
        /// </summary>
        public const string KEY_VERIFY_FIELD = "VerifyCode";

        /// <summary>
        /// 提交新账户，如果不存在，则添加，如果存在则修改
        /// </summary>
        /// <param name="name">账户名</param>
        /// <param name="userid">用户Id</param>
        /// <param name="flags">账户标示</param>
        /// <param name="accountType">账户类型</param>
        /// <returns></returns>
        public static Account Post(string name, Guid userid, UMC.Security.UserFlags flags, int accountType)
        {
            var entity = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();

            var acc = new UMC.Data.Entities.Account
            {
                Name = name,
                Flags = flags
            };
            entity.Where.And().Equal(new Data.Entities.Account { Type = accountType, Name = name })
                .And().Unequal(new Data.Entities.Account { user_id = userid })
                .Entities.Delete();

            entity.Where.Reset().And().Equal(new UMC.Data.Entities.Account
            {
                Type = accountType,
                user_id = userid
            });
            var acc2 = entity.Single();
            if (acc2 == null)
            {
                acc.user_id = userid;
                acc.Type = accountType;
                entity.Insert(acc);
                return new Account(acc);
            }
            else
            {
                entity.Update(acc);
                acc2.Name = acc.Name;
                acc2.Flags = acc.Flags;
                return new Account(acc2);
            }

        }
        /// <summary>
        /// 验证外围账户
        /// </summary>
        /// <param name="userid">外围帐户</param>
        /// <param name="type">外围账户类型</param>
        /// <param name="code">验证的值</param>
        /// <param name="verifyField">值所在的字段</param>
        /// <returns></returns>
        public static bool Valid(Guid userid, int type, string code, string verifyField)
        {
            var entity = Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
            entity.Where.And().Equal(new UMC.Data.Entities.Account { Type = type, user_id = userid, });

            var dic = entity.Single();
            if (dic != null)
            {
                var config = UMC.Data.JSON.Deserialize<Hashtable>(dic.ConfigData) ?? new Hashtable();

                var session = config[verifyField ?? KEY_VERIFY_FIELD] as string;
                if (String.Equals(code, session))
                {
                    entity.Update(new UMC.Data.Entities.Account
                    {
                        Name = (config["Name"] as string) ?? dic.Name,
                        Flags = UMC.Security.UserFlags.Normal,
                        ConfigData = String.Empty
                    });
                    entity.Where.Reset().And().Equal(new Data.Entities.Account { Type = type, Name = (config["Name"] as string) ?? dic.Name })
                        .And().Unequal(new Data.Entities.Account { user_id = userid })
                        .Entities.Update(new UMC.Data.Entities.Account { Flags = UMC.Security.UserFlags.UnVerification });
                    return true;
                }
            }
            return false;
        }
        System.Collections.Generic.List<UMC.Data.Entities.Account> accounts;

        public Account this[int accountType]
        {
            get
            {
                if (accounts == null)
                {
                    var aEntity = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
                    aEntity.Where.And(new UMC.Data.Entities.Account { user_id = this.user_id });
                    this.accounts = new System.Collections.Generic.List<Data.Entities.Account>(aEntity.Query());
                }

                for (var i = 0; i < this.accounts.Count; i++)
                {
                    var ac = this.accounts[i];
                    if (ac.Type == accountType && (ac.Flags & UMC.Security.UserFlags.Disabled) != UMC.Security.UserFlags.Disabled)
                    {
                        var act = new Account(ac);
                        act.accounts = this.accounts;
                        return act;
                    }
                }
                return null;
            }
        }
        public static Account Create(Guid userid)
        {
            return new Account(new Data.Entities.Account { user_id = userid });
        }

    }

}