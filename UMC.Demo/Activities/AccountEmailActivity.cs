using System;
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
    class AccountEmailActivity : WebActivity
    {

        void SendEmail(string email)
        {
            var user = UMC.Security.Identity.Current;

            var uiattr = UMC.Data.Reflection.GetDataProvider("account", "Email");

            //account.Items[Account.KEY_VERIFY_FIELD] = WebUtility.NumberCode(Guid.NewGuid().GetHashCode(), 6);// session.ToString();
            //account.Items["Name"] = email;
            //account.Commit();

            var hask = new Hashtable();

            hask["Code"] = Utility.NumberCode(Guid.NewGuid().GetHashCode(), 6);// account.Items[Account.KEY_VERIFY_FIELD];// new Uri(this.Request.Url, url);
            hask["DateTime"] = DateTime.Now;

            var session = new UMC.Configuration.Session<Hashtable>(email);
            if (session.ModifiedTime.AddMinutes(15) > DateTime.Now)
            {
                hask = session.Value;
            }
            else
            {
                session.Commit(hask, user);
            }

            UMC.Data.Reflection.PropertyToDictionary(UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>()
                .Where.And().Equal(new UMC.Data.Entities.User { Id = user.Id.Value }).Entities.Single(), hask);


            var mail = new System.Net.Mail.MailMessage();
            mail.To.Add(email);
            mail.Subject = Utility.Format(uiattr["Subject"], hask);
            mail.Body = Utility.Format(uiattr["Body"], hask);
            mail.IsBodyHtml = true;
            Net.Message.Instance().Send(mail);

        }

        void Remove()
        {
            var user = UMC.Security.Identity.Current;
            var act = Account.Create(user.Id.Value);

            var a = act[Account.EMAIL_ACCOUNT_KEY];
            var code = Web.UIDialog.AsyncDialog("Remove", d =>
                 {

                     var fm = new Web.UIFormDialog() { Title = "�����֤" };
                     fm.AddTextValue().Put("����", a.Name);
                     fm.AddVerify("��֤��", "Code", "�������յ�����֤��")
                         .Put("Command", "Email").Put("Model", "Account").Put("SendValue", new UMC.Web.WebMeta().Put("Email", a.Name).Put("Code", "Send")).Put("Start", "YES");

                     fm.Submit("ȷ����֤��", this.Context.Request, "Email");
                     return fm;
                 });
            var session = new UMC.Configuration.Session<Hashtable>(a.Name);
            if (session.Value != null)
            {
                if (String.Equals(session.Value["Code"] as string, code))
                {

                    Account.Post(a.Name, a.user_id, Security.UserFlags.UnVerification, Account.EMAIL_ACCOUNT_KEY);
                    this.Prompt("�������󶨳ɹ�", false);
                    this.Context.Send(new UMC.Web.WebMeta().Put("type", "Email"), true);
                }
            }
            this.Prompt("���������֤�����");
        }
        public override void ProcessActivity(WebRequest request, WebResponse response)
        {

            var user = UMC.Security.Identity.Current;
            var act = Account.Create(user.Id.Value);


            var value = Web.UIDialog.AsyncDialog("Email", d =>
            {
                var acc = act[UMC.Security.Account.EMAIL_ACCOUNT_KEY];
                if (acc != null && (acc.Flags & UserFlags.UnVerification) != UserFlags.UnVerification)
                {
                    return new Web.UIConfirmDialog("��ȷ�Ͻ��������İ���") { Title = "���ȷ��", DefaultValue = "Change" };
                }


                var t = new Web.UITextDialog() { Title = "�����", DefaultValue = acc != null ? acc.Name : "" };
                t.Config["submit"] = "��һ��";
                return t;

            });
            switch (value)
            {
                case "Change":

                    Remove();
                    return;
            }

            if (Data.Utility.IsEmail(value) == false)
            {
                this.Prompt("�����ʽ����ȷ");
            }


            var entity = Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
            entity.Where.And().Equal(new UMC.Data.Entities.Account { Name = value, Type = Account.EMAIL_ACCOUNT_KEY }).And().Unequal(new Data.Entities.Account { user_id = user.Id });
            if (entity.Count() > 0)
            {
                this.Prompt("�������Ѵ��ڰ�");

            }


            var Code = UMC.Web.UIDialog.AsyncDialog("Code", g =>
            {
                var fm = new Web.UIFormDialog() { Title = "��֤��" };
                fm.AddTextValue().Put("����", value);
                fm.AddVerify("��֤��", "Code", "�������յ�����֤��")
                    .Put("Command", "Email").Put("Model", "Account").Put("SendValue", new UMC.Web.WebMeta().Put("Email", value).Put("Code", "Send")).Put("Start", "YES");

                fm.Submit("ȷ����֤��", request, "Email");
                return fm;
            });
            if (Code == "Send")
            {
                this.SendEmail(value);
                this.Prompt("��֤���ѷ���");
            }
            var session = new UMC.Configuration.Session<Hashtable>(value);
            if (session.Value != null)
            {
                var code = session.Value["Code"] as string;
                if (String.Equals(code, Code))
                {

                    Account.Post(value, user.Id.Value, Security.UserFlags.Normal, Account.EMAIL_ACCOUNT_KEY);
                    this.Prompt("����󶨳ɹ�", false);
                    this.Context.Send(new UMC.Web.WebMeta().Put("type", "Email"), true);
                }
            }


            this.Prompt("���������֤�����");


        }

    }
}