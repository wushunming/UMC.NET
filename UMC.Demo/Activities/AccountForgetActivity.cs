﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;
using UMC.Security; 
using UMC.Web;
using UMC.Data;

namespace UMC.Demo.Activities
{
    class AccountForgetActivity : WebActivity
    {
        bool Send(string mobile, UMC.Data.Provider provider)
        {
            var d = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
            if (Utility.IsPhone(mobile))
            {
                d.Where.And().Equal(new Data.Entities.Account { Name = mobile, Type = Account.MOBILE_ACCOUNT_KEY });
            }
            else
            {
                d.Where.And().Equal(new Data.Entities.Account { Name = mobile, Type = Account.EMAIL_ACCOUNT_KEY });
            }
            var acc = d.Single();
            if (acc != null)
            {
                var ac = Account.Create(acc);
                var user = UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>()
                .Where.And().Equal(new UMC.Data.Entities.User { Id = acc.user_id }).Entities.Single();
                var hask = new Hashtable();
                switch (acc.Type)
                {
                    case Account.MOBILE_ACCOUNT_KEY:
                        var times = UMC.Data.Utility.IntParse(String.Format("{0}", ac.Items["Times"]), 0) + 1;
                        if (times > 5)
                        {
                            //ac.Items["Date"] = WebADNuke.Utils.Utility.TimeSpan();
                            var date = UMC.Data.Utility.TimeSpan(Data.Utility.IntParse(String.Format("{0}", ac.Items["Date"]), UMC.Data.Utility.TimeSpan()));
                            if (date.AddHours(3) > DateTime.Now)
                            {
                                this.Prompt("您已经超过了5次，请您三小时后再试");
                            }
                            else
                            {
                                times = 0;
                            }
                        }
                        //var code = Utility.NumberCode(Guid.NewGuid().GetHashCode(), 6);
                        ac.Items["Date"] = UMC.Data.Utility.TimeSpan();
                        ac.Items["UserHostAddress"] = this.Context.Request.UserHostAddress;
                        ac.Items[Account.KEY_VERIFY_FIELD] = hask["Code"] = Utility.NumberCode(Guid.NewGuid().GetHashCode(), 6);
                        ac.Commit();
                        //WebADNuke.Data.Reflection.PropertyToDictionary(user, hask);
                        //string body = Utility.Format(provider["Sms"], hask);

                        Net.Message.Instance().Send("Forget", hask, ac.Name);

                        //WebResource.Instance().Send(body, ac.Name);
                        return true;
                    //this.Prompt("短信验证码已经发送，请注意查收");
                    //break;
                    case Account.EMAIL_ACCOUNT_KEY:

                        ac.Items[Account.KEY_VERIFY_FIELD] = hask["Code"] = Utility.NumberCode(Guid.NewGuid().GetHashCode(), 6);
                        ac.Commit();
                        UMC.Data.Reflection.PropertyToDictionary(user, hask);

                        hask["DateTime"] = DateTime.Now;

                        var mail = new System.Net.Mail.MailMessage();
                        mail.To.Add(mobile);
                        mail.Subject = Utility.Format(provider["Subject"], hask);
                        mail.Body = Utility.Format(provider["Body"], hask);
                        mail.IsBodyHtml = true;
                        UMC.Net.Message.Instance().Send(mail);
                        return true;

                        //this.Prompt("邮箱已经发送，请查收邮箱获取验证码");
                        break;
                }
            }

            return false;
            //this.Prompt("没有此绑定账户");
        }
        public override void ProcessActivity(WebRequest request, WebResponse response)
        {

            var username = Web.UIDialog.AsyncDialog("Username", d =>
            {
                var fd = new UMC.Web.UIFormDialog();
                fd.Title = "找回密码";
                fd.AddText("", "Username").Put("placeholder", "手机号码或邮箱");

                fd.Submit("下一步", request, "Forget");
                return fd;
            });
            var entity = Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
            UMC.Data.Entities.Account ac = new UMC.Data.Entities.Account { Name = username };
            if (Data.Utility.IsEmail(username))
            {
                ac.Type = UMC.Security.Account.EMAIL_ACCOUNT_KEY;
                entity.Where.And().Equal(ac);

            }
            else if (Data.Utility.IsPhone(username))
            {
                ac.Type = UMC.Security.Account.MOBILE_ACCOUNT_KEY;
                entity.Where.And().Equal(ac);
            }
            if (ac.Type.HasValue == false)
            {
                this.Prompt("只支持手机号和邮箱找回密码");
            }
            var acct = entity.Single();
            if (acct == null)
            {
                switch (ac.Type.Value)
                {
                    case UMC.Security.Account.EMAIL_ACCOUNT_KEY:
                        this.Prompt("没有找到此邮箱绑定账户");
                        break;
                    default:
                        this.Prompt("没有找到此手机号绑定账户");
                        break;
                }
            }
            var Code = UMC.Web.UIDialog.AsyncDialog("Code", g =>
            {
                var ts = ac.Type.Value == UMC.Security.Account.EMAIL_ACCOUNT_KEY ? "邮箱" : "手机";
                var fd = new UMC.Web.UIFormDialog();
                fd.AddTextValue().Put(ts, username);

                fd.AddVerify("验证码", "Code", String.Format("{0}收到的验证码", ts))
                 .Command(request.Model, request.Command, new UMC.Web.WebMeta().Put("Username", username).Put("Code", "Reset"));
                fd.Title = "验证" + ts;
                fd.Submit("验证", request, "Password");
                this.Context.Send(new UMC.Web.WebMeta().Put("type", "Forget"), false);

                return fd;
            });
            if (String.Equals(Code, "Reset"))
            {
                ;
                if (this.Send(username, UMC.Data.Reflection.GetDataProvider("account", "Forget")))
                {
                    this.Prompt("验证码已经发送，请注意查收", false);
                    this.Context.Send(new UMC.Web.WebMeta().UIEvent("VerifyCode", this.AsyncDialog("UI", "none"), new UMC.Web.WebMeta().Put("text", "验证码已发送")), true);
                }
                else
                {
                    switch (ac.Type.Value)
                    {
                        case UMC.Security.Account.EMAIL_ACCOUNT_KEY:
                            this.Prompt("没有找到此邮箱绑定账户");
                            break;
                        default:
                            this.Prompt("没有找到此手机号绑定账户");
                            break;
                    }
                }
            }
            var account = Account.Create(acct);
            var VerifyCode = account.Items[Account.KEY_VERIFY_FIELD] as string;

            if (String.Equals(VerifyCode, Code, StringComparison.CurrentCultureIgnoreCase))
            {
                WebMeta print = new UMC.Web.WebMeta();
                print["AccountType"] = acct.Type.ToString();
                print["VerifyCode"] = Code;
                print["user_id"] = acct.user_id.ToString();

                this.Context.Send(new UMC.Web.WebMeta().Put("type", "Forget"), false);
                response.Redirect(request.Model, "Password", print, true);
            }
            else
            {
                this.Prompt("您输入的验证码错误");
            }


        }

    }
}