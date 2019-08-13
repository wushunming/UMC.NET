using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;
using UMC.Data;
using UMC.Security;
using UMC.Web;

namespace UMC.Demo.Activities
{
    class AccountMobileActivity : WebActivity
    {

        void SendMobileCode(string mobile)
        {

            var user = UMC.Security.Identity.Current;

            var req = this.Context.Request;

            var hask = new Hashtable();

            var session = new UMC.Configuration.Session<Hashtable>(mobile);
            if (session.ModifiedTime.AddMinutes(15) > DateTime.Now)
            {
                hask = session.Value;
            }
            else
            {
                hask["Code"] = UMC.Data.Utility.NumberCode(Guid.NewGuid().GetHashCode(), 6);
            }
            var times = UMC.Data.Utility.IntParse(String.Format("{0}", hask["Times"]), 0) + 1;
            if (times > 5)
            {
                var date = session.ModifiedTime;
                if (date.AddHours(3) > DateTime.Now)
                {
                    this.Prompt("���Ѿ�������5�Σ�������Сʱ������");
                }
                else
                {
                    times = 0;
                }
            }
            session.Commit(hask, user);


            UMC.Data.Reflection.PropertyToDictionary(UMC.Data.Database.Instance().ObjectEntity<UMC.Data.Entities.User>()
                .Where.And().Equal(new UMC.Data.Entities.User { Id = user.Id.Value }).Entities.Single(), hask);

            hask["DateTime"] = DateTime.Now;

            Net.Message.Instance().Send("Verify", hask, mobile);

        }


        void Remove()
        {
            var user = UMC.Security.Identity.Current;
            var act = Account.Create(user.Id.Value);

            var a = act[Account.MOBILE_ACCOUNT_KEY];
            var code = Web.UIDialog.AsyncDialog("Remove", d =>
                 {

                     var fm = new Web.UIFormDialog() { Title = "�����֤" };
                     fm.AddTextValue().Put("�ֻ�����", a.Name);
                     fm.AddVerify("��֤��", "Code", "���յ�����֤��")
                .Command("Account", "Mobile", new UMC.Web.WebMeta().Put("Mobile", a.Name).Put("Code", "Send"))
                    .Put("Start", "YES");

                     fm.Submit("ȷ����֤��", this.Context.Request, "Mobile");
                     return fm;
                 });
            var session = new UMC.Configuration.Session<Hashtable>(a.Name);
            if (session.Value != null)
            {
                if (String.Equals(session.Value["Code"] as string, code))
                {

                    Account.Post(a.Name, a.user_id, Security.UserFlags.UnVerification, Account.MOBILE_ACCOUNT_KEY);
                    this.Prompt("�ֻ�����󶨳ɹ�", false);
                    this.Context.Send(new UMC.Web.WebMeta().Put("type", "Mobile"), true);
                }
            }
            this.Prompt("���������֤�����");
        }
        public override void ProcessActivity(WebRequest request, WebResponse response)
        {

            var user = UMC.Security.Identity.Current;
            var act = Account.Create(user.Id.Value);


            var value = Web.UIDialog.AsyncDialog("Mobile", d =>
            {
                var acc = act[UMC.Security.Account.MOBILE_ACCOUNT_KEY];
                if (acc != null && (acc.Flags & UserFlags.UnVerification) != UserFlags.UnVerification)
                {
                    return new Web.UIConfirmDialog("��ȷ�Ͻ�����ֻ��İ���") { Title = "���ȷ��", DefaultValue = "Change" };
                }
                var fm = new Web.UIFormDialog() { Title = "�ֻ���" };
                fm.AddText("�ֻ�����", "Mobile", acc != null ? acc.Name : "");

                fm.Submit("��һ��", request, "Mobile");
                return fm;

            });
            switch (value)
            {
                case "Change":

                    Remove();
                    return;
            }
            if (Data.Utility.IsPhone(value) == false)
            {
                this.Prompt("�ֻ������ʽ����ȷ");
            }


            var entity = Data.Database.Instance().ObjectEntity<UMC.Data.Entities.Account>();
            entity.Where.And().Equal(new UMC.Data.Entities.Account { Name = value, Type = Account.MOBILE_ACCOUNT_KEY }).And().Unequal(new Data.Entities.Account { user_id = user.Id });
            if (entity.Count() > 0)
            {
                this.Prompt("���ֻ������ѱ������˺Ű�");

            }


            var Code = UMC.Web.UIDialog.AsyncDialog("Code", g =>
            {
                var fm = new Web.UIFormDialog() { Title = "�ֻ�������֤" };
                fm.AddTextValue().Put("�ֻ�����", value);
                fm.AddVerify("��֤��", "Code", "���յ�����֤��")
                .Command("Account", "Mobile", new UMC.Web.WebMeta().Put("Mobile", value).Put("Code", "Send"))
                    .Put("Start", "YES");

                fm.Submit("ȷ����֤��", request, "Mobile");
                return fm;
            });
            if (Code == "Send")
            {
                this.SendMobileCode(value);
                this.Prompt("��֤���ѷ���", false);
                this.Context.Send(new UMC.Web.WebMeta().UIEvent("VerifyCode", this.AsyncDialog("UI", "none"), new UMC.Web.WebMeta("Time", "100")), true);

            }
            var session = new UMC.Configuration.Session<Hashtable>(value);
            if (session.Value != null)
            {
                var code = session.Value["Code"] as string;
                if (String.Equals(code, Code))
                {

                    Account.Post(value, user.Id.Value, Security.UserFlags.Normal, Account.MOBILE_ACCOUNT_KEY);
                    this.Prompt("�ֻ�����󶨳ɹ�", false);
                    this.Context.Send(new UMC.Web.WebMeta().Put("type", "Mobile"), true);
                }
            }


            this.Prompt("���������֤�����");


        }

    }
}