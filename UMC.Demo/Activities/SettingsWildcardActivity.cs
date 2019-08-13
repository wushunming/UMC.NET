using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using UMC.Web;

namespace UMC.Demo.Activities
{
    class SettingsWildcardActivity : WebActivity
    {
        public override void ProcessActivity(WebRequest request, WebResponse response)
        {


            var roleEntity = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Role>();
            var RoleType = UMC.Web.UIDialog.AsyncDialog("Type", d =>
            {
                if (roleEntity.Count() < 4)
                {
                    return Web.UIDialog.ReturnValue("User");
                }
                var rd = new Web.UIRadioDialog() { Title = "选择设置账户类型" };
                rd.Options.Add("角色", "Role");
                rd.Options.Add("用户", "User");
                return rd;
            });

            var setValue = UMC.Web.UIDialog.AsyncDialog("Value", d =>
            {
                if (RoleType == "Role")
                {
                    var rd = new Web.UIRadioDialog() { Title = "请选择设置权限的角色" };
                    roleEntity.Where.Reset().And().NotIn(new Data.Entities.Role
                    {
                        Rolename = UMC.Security.Membership.GuestRole
                    }, UMC.Security.Membership.AdminRole);

                    roleEntity.Query(dr => rd.Options.Add(dr.Rolename, dr.Rolename));
                    return rd;
                }
                else
                {
                    return new UserDialog() { Title = "请选择设置权限的账户" };
                }
            });

            var wdcks = Web.WebServlet.Auths();
            if (wdcks.Count == 0)
            {
                this.Prompt("现在的功能不需要设置权限");
            }
            var wdks = new List<UMC.Data.Entity<UMC.Data.Entities.Wildcard, List<UMC.Security.Authorize>>>();

            var wddEntity = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Wildcard>();
            wddEntity.Where.And().In("WildcardKey", wdcks.ToArray()).Entities.Query(dr =>
            {
                wdks.Add(new Data.Entity<Data.Entities.Wildcard, List<Security.Authorize>>(dr, dr.Authorizes));
            });


            var Wildcard = Web.UIDialog.AsyncDialog("Wildcards", d =>
            {
                var fmdg = new Web.UICheckboxDialog();
                fmdg.Title = "收银权限设置";
                fmdg.DefaultValue = "None";


                foreach (var cm in wdcks)
                {
                    var id = cm.Get("key");// String.Format("{0}.{1}.POS", cm.Command, cm.Model);

                    var wdk = wdks.Find(w => String.Equals(w.Value.WildcardKey, id, StringComparison.CurrentCultureIgnoreCase));
                    if (wdk != null)
                    {
                        if (wdk.Config != null)
                        {
                            var isS = false;
                            if (RoleType == "Role")
                            {
                                isS = wdk.Config.Exists(a => a.Type == Security.AuthorizeType.RoleDeny
                                    && String.Equals(a.Value, setValue, StringComparison.CurrentCultureIgnoreCase));
                            }
                            else
                            {
                                isS = wdk.Config.Exists(a => a.Type == Security.AuthorizeType.UserDeny
                                    && String.Equals(a.Value, setValue, StringComparison.CurrentCultureIgnoreCase));
                            }
                            fmdg.Options.Add(cm.Get("desc"), id, !isS);
                        }
                        else
                        {
                            fmdg.Options.Add(cm.Get("desc"), id, true);
                        }
                    }
                    else
                    {
                        fmdg.Options.Add(cm.Get("desc"), id, true);
                    }
                }

                return fmdg;

            });
            foreach (var cm in wdcks)
            {
                var id = cm.Get("key");
                var wdk = wdks.Find(w => String.Equals(w.Value.WildcardKey, id, StringComparison.CurrentCultureIgnoreCase));

                List<Security.Authorize> authorizes;
                if (wdk != null)
                {
                    authorizes = wdk.Config;
                }
                else
                {
                    authorizes = new List<Security.Authorize>();
                }
                if (RoleType == "Role")
                {
                    authorizes.RemoveAll(a => (a.Type == Security.AuthorizeType.RoleDeny || a.Type == Security.AuthorizeType.RoleAllow)
                        && String.Equals(a.Value, setValue, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    authorizes.RemoveAll(a => (a.Type == Security.AuthorizeType.UserAllow || a.Type == Security.AuthorizeType.UserDeny)
                        && String.Equals(a.Value, setValue, StringComparison.CurrentCultureIgnoreCase));
                }
                if (Wildcard.IndexOf(id) == -1)
                {

                    if (RoleType == "Role")
                    {
                        authorizes.Add(new Security.Authorize { Value = setValue, Type = Security.AuthorizeType.RoleDeny });

                    }
                    else
                    {
                        authorizes.Add(new Security.Authorize { Value = setValue, Type = Security.AuthorizeType.UserDeny });
                    }
                }

                var widcard = new UMC.Data.Entities.Wildcard
                {
                    Authorizes = UMC.Data.JSON.Serialize(authorizes),
                    WildcardKey = id,
                    Description = cm.Get("desc")
                };
                wddEntity.Where.Reset().And().Equal(new Data.Entities.Wildcard { WildcardKey = id })
                    .Entities.IFF(e => e.Count() == 0, e => e.Insert(widcard), e => e.Update(widcard));

            }
            this.Prompt("收银权限设置成功");

        }

    }
}