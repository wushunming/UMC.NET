using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Configuration
{
    public class Membership : UMC.Security.Membership
    {
        class User : UMC.Data.Entities.User
        {
            public string Password
            {
                get;
                set;
            }
        }
        public override int Password(string username, string password, int max)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");

            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.And(new User { Username = username });
            var user = userEntiy.Single(new User
            {
                VerifyTimes = 0,
                Password = String.Empty,
                Id = Guid.Empty,
                Flags = Security.UserFlags.Normal
            });
            if (user == null)
            {
                return -1;
            }
            else
            {
                if (((user.Flags ?? Security.UserFlags.Lock) & Security.UserFlags.Lock) == Security.UserFlags.Lock)
                {
                    return -2;
                }
                else if (((user.Flags ?? Security.UserFlags.Disabled) & Security.UserFlags.Disabled) == Security.UserFlags.Disabled)
                {
                    return -3;
                }
                else if (String.IsNullOrEmpty(user.Password))
                {
                    return 0;
                }
                var destPwd = UMC.Data.Utility.DES(Convert.FromBase64String(user.Password), user.Id.Value);
                StringComparison comparison = StringComparison.CurrentCulture;
                var spIndex = password.IndexOf(':');
                if (spIndex > 0)
                {
                    var md5pwd = password.Substring(spIndex + 1);
                    var sp = password.Substring(0, spIndex);
                    if (md5pwd.Length == 32)
                    {
                        password = password.Substring(spIndex + 1);
                        destPwd = UMC.Data.Utility.MD5(sp + destPwd);
                        comparison = StringComparison.CurrentCultureIgnoreCase;
                    }
                }
                if (String.Equals(destPwd, password, comparison))
                {
                    userEntiy.Update(new User { VerifyTimes = 0, ActiveTime = DateTime.Now });
                    return 0;
                }
                else if (max > 0)
                {
                    var s = new User { VerifyTimes = (user.VerifyTimes ?? 0) + 1 };
                    if (max <= user.VerifyTimes)
                    {
                        s.Flags = (user.Flags ?? Security.UserFlags.Normal) | Security.UserFlags.Lock;
                        userEntiy.Update(s);
                    }
                    else
                    {
                        userEntiy.Update(s);
                    }
                    return s.VerifyTimes ?? 0;
                }
                else
                {
                    return 1;
                }
            }
            return -2;
        }
        public override UMC.Security.Identity Identity(string name, int accountType)
        {
            var acount = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>()
                .Where.And(new UMC.Data.Entities.Account { Type = accountType, Name = name })
                .Entities.Single();
            if (acount == null)
            {
                return null;
            }

            var user = this.Identity(acount.user_id.Value);
            if (user == null)
            {
                var a = UMC.Security.Account.Create(acount);
                return UMC.Security.Identity.Create(acount.user_id.Value, "#", (a.Items["Alias"] as string) ?? " 关联用户");
            }
            return user;
        }

        public override bool Password(string Username, string password)
        {
            if (String.IsNullOrEmpty(Username)) throw new ArgumentNullException("username");

            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.And(new User { Username = Username });
            var user = userEntiy.Single(new User { Id = Guid.Empty });
            if (user != null)
            {
                return userEntiy.Update(new User { Password = Convert.ToBase64String(UMC.Data.Utility.DES(password, user.Id.Value)) }) > 0;
            }
            return false;

        }


        public override bool DeleteUser(string username)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>();
            userEntiy.Where.And(new UMC.Data.Entities.User { Username = username });
            var user = userEntiy.Single();
            if (user != null)
            {
                var WDKRole = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Role>();
                WDKRole.Where.And(new UMC.Data.Entities.Role { Rolename = AdminRole });
                var role = WDKRole.Single();
                if (role != null)
                {
                    var WDKUserToRole = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.UserToRole>();
                    WDKUserToRole.Where.And().Equal(new UMC.Data.Entities.UserToRole { role_id = role.Id });

                    if (WDKUserToRole.Count() == 1)
                    {
                        WDKUserToRole.Where.And().Equal(new UMC.Data.Entities.UserToRole { user_id = user.Id });
                        if (WDKUserToRole.Count() == 1)
                        {
                            return false;
                        }
                    }
                    WDKUserToRole.Where.Reset();
                    WDKUserToRole.Where.And().Equal(new UMC.Data.Entities.UserToRole { user_id = user.Id });
                    WDKUserToRole.Delete();
                    UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>()
                        .Where.And(new UMC.Data.Entities.Account { user_id = user.Id }).Entities.Delete(); ;

                }
            }
            userEntiy.Delete();

            return true;
        }


        public override Guid CreateUser(string username, string password, string alias)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.And(new User { Username = username });
            if (userEntiy.Count() == 0)
            {
                var sn = NewSN();
                userEntiy.Insert(new User
                {
                    Alias = alias,
                    Flags = UMC.Security.UserFlags.Normal,
                    Id = sn,
                    RegistrTime = DateTime.Now,
                    IsMember = false,
                    Username = username,
                    Password = Convert.ToBase64String(UMC.Data.Utility.DES(password, sn))
                });
                return sn;
            }

            return Guid.Empty;
        }
        public virtual Guid NewSN()
        {
            return Guid.NewGuid();
        }

        public override bool ChangeFlags(string username, UMC.Security.UserFlags flags)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.And(new User { Username = username });
            return userEntiy.Update(new User { Flags = flags }) > 0;
        }

        public override bool Exists(string Username)
        {
            if (String.IsNullOrEmpty(Username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.And(new User { Username = Username });
            return userEntiy.Count() == 1;
        }

        public override UMC.Security.Identity Identity(string username)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");

            var user = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>()
                .Where.And().Equal(new UMC.Data.Entities.User
            {
                Username = username
            }).Entities.Single(new UMC.Data.Entities.User
            {
                Id = Guid.Empty,
                Alias = String.Empty,
                Username = String.Empty,
            });
            if (user == null)
            {
                return null;
            }
            var flags = user.Flags ?? Security.UserFlags.Normal;
            if ((flags & Security.UserFlags.Lock) == Security.UserFlags.Lock)
            {
                return UMC.Security.Identity.Create(user.Id.Value, user.Username, user.Alias);
            }
            var roles = new List<string>();
            var sqlScript = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.UserToRole>()
                       .Where.And().Equal(new UMC.Data.Entities.UserToRole { user_id = user.Id.Value })
                       .Entities.Script(new UMC.Data.Entities.UserToRole
                       {
                           role_id = Guid.Empty
                       });


            UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Role>()
                .Where.And().In("Id", sqlScript)
                .Entities.Query(new UMC.Data.Entities.Role { Rolename = String.Empty }, dr =>
                {
                    roles.Add(dr.Rolename);
                });

            return UMC.Security.Identity.Create(user.Id.Value, user.Username, user.Alias, roles.ToArray());

        }


        public override bool RemoveRole(string Username, string rolename)
        {
            if (String.IsNullOrEmpty(Username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>();
            userEntiy.Where.And(new UMC.Data.Entities.User { Username = Username });
            var user = userEntiy.Single(new UMC.Data.Entities.User { Id = Guid.Empty });
            if (user != null)
            {
                var WDKRole = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Role>();
                WDKRole.Where.And(new UMC.Data.Entities.Role { Rolename = rolename });
                var role = WDKRole.Single();
                if (role != null)
                {
                    var uTRole = new UMC.Data.Entities.UserToRole { role_id = role.Id };
                    var WDKUserToRole = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.UserToRole>();

                    WDKUserToRole.Where.And().Equal(uTRole);
                    if (rolename == AdminRole)
                    {
                        if (WDKUserToRole.Count() == 1)
                        {
                            return false;
                        }
                        else
                        {
                            WDKUserToRole.Where.And().Equal(new UMC.Data.Entities.UserToRole { user_id = user.Id }); ;
                        }
                    }
                    else
                    {

                        WDKUserToRole.Where.And().Equal(new UMC.Data.Entities.UserToRole { user_id = user.Id }); ;
                    }
                    WDKUserToRole.Delete();
                    return true;
                }
            }

            return false;
        }

        public override bool AddRole(string Username, params string[] roles)
        {
            if (String.IsNullOrEmpty(Username)) throw new ArgumentNullException("username");
            if (roles.Length > 0)
            {
                var userEntiy = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>();
                userEntiy.Where.And(new UMC.Data.Entities.User { Username = Username });
                var user = userEntiy.Single();
                if (user != null)
                {
                    var WDKRole = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Role>();
                    WDKRole.Where.And().In(new UMC.Data.Entities.Role { Rolename = roles[0] }, roles);
                    var ids = new List<Guid>();
                    var tds = new List<UMC.Data.Entities.UserToRole>();
                    WDKRole.Query(dr =>
                    {
                        ids.Add(dr.Id.Value);
                        tds.Add(new Data.Entities.UserToRole { user_id = user.Id, role_id = dr.Id });
                    });
                    if (ids.Count > 0)
                    {
                        UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.UserToRole>()
                                .Where.And().Equal(new Data.Entities.UserToRole { user_id = user.Id }).And().In(new Data.Entities.UserToRole { role_id = ids[0] }, ids.ToArray())
                                .Entities.IFF(e => e.Delete() >= 0, e => e.Insert(tds.ToArray()));

                        return true;
                    }
                }
            }

            return false;
        }

        public override bool ChangeAlias(string username, string alias)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            return UMC.Data.Database.Instance().ObjectEntity<User>()
                   .Where.And(new User { Username = username })
              .Entities.Update(new User { Alias = alias }) > 0;
        }
        public override void Activation(Security.AccessToken token)
        {
            var sesion = new Configuration.Session<UMC.Security.AccessToken>(token, token.Id.ToString());
            sesion.ContentType = token.ContentType;

            switch (token.Username ?? "?")
            {
                case "?":
                    if (token.SId.HasValue)
                    {
                        sesion.Commit(Guid.Empty, token.SId ?? Guid.Empty);
                    }
                    else
                    {
                        sesion.Commit(Guid.Empty, token.Id ?? Guid.Empty);
                    }
                    break;
                case "#":
                    sesion.Commit(Guid.Empty, token.Identity().Id ?? Guid.Empty);
                    break;
                default:
                    sesion.Commit(Guid.Empty, token.Identity().Id ?? Guid.Empty);

                    UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>()
                        .Where.And(new UMC.Data.Entities.User { Username = token.Username }).Entities
                        .Update(new User { ActiveTime = DateTime.Now, SessionKey = token.Id.Value });
                    break;
            }

        }

        public override string Password(string username)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.And(new User { Username = username });
            var user = userEntiy.Single(new User
            {
                Password = String.Empty,
                Id = Guid.Empty
            });
            if (user != null)
            {

                return UMC.Data.Utility.DES(Convert.FromBase64String(user.Password), user.Id.Value);
            }
            return null;
        }

        public override Security.Identity Identity(Guid id)
        {


            var user = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>()
                .Where.And().Equal(new UMC.Data.Entities.User
                {
                    Id = id
                }).Entities.Single(new UMC.Data.Entities.User
                {
                    Id = Guid.Empty,
                    Alias = String.Empty,
                    Username = String.Empty,
                });
            if (user == null)
            {
                return null;
            }
            var roles = new List<string>();
            var sqlScript = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.UserToRole>()
                       .Where.And().Equal(new UMC.Data.Entities.UserToRole { user_id = user.Id.Value })
                       .Entities.Script(new UMC.Data.Entities.UserToRole
                       {
                           role_id = Guid.Empty
                       });


            UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Role>()
                .Where.And().In("Id", sqlScript)
                .Entities.Query(new UMC.Data.Entities.Role { Rolename = String.Empty }, dr =>
                {
                    roles.Add(dr.Rolename);
                });

            return UMC.Security.Identity.Create(user.Id.Value, user.Username, user.Alias, roles.ToArray());
        }
        public override Security.Identity CreateUser(Guid id, string username, string password, string alias)
        {

            if (String.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            var userEntiy = UMC.Data.Database.Instance().ObjectEntity<User>();
            userEntiy.Where.Or(new User { Username = username, Id = id });
            if (userEntiy.Count() == 0)
            {
                userEntiy.Insert(new User
                {
                    Alias = alias,
                    Flags = UMC.Security.UserFlags.Normal,
                    Id = id,
                    RegistrTime = DateTime.Now,
                    IsMember = true,
                    Username = username,
                    Password = Convert.ToBase64String(UMC.Data.Utility.DES(password, id))
                });
                return UMC.Security.Identity.Create(id, username, alias, GuestRole);
            }

            return null;

        }
    }
}
