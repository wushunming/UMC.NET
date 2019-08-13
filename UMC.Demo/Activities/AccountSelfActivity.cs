using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;
using UMC.Web;
using UMC.Security;

namespace UMC.Demo.Activities
{
    public class AccountSelfActivity : WebActivity
    {

        public override void ProcessActivity(WebRequest request, WebResponse response)
        {
            var user = UMC.Security.Identity.Current;
            if (user.IsAuthenticated == false)
            {
                response.Redirect(request.Model, "Login");
            }
            var Model = this.AsyncDialog("Model", gkey =>
              {
                  WebMeta form = request.SendValues ?? new UMC.Web.WebMeta();

                  if (form.ContainsKey("limit") == false)
                  {
                      this.Context.Send(new UISectionBuilder(request.Model, request.Command)
                              .RefreshEvent("UI.Setting", "image", "Email", "Mobile")
                              .Builder(), true);

                  }

                  //    var rel = Utility.Database.ObjectEntity<Entities.VIPMember>().Where.And().Equal(relt).Entities.Single() ?? relt;


                  var account = Security.Account.Create(user.Id.Value);
                  var dic = UMC.Web.UISection.Create(new UITitle("�˻���Ϣ"));
                  dic.AddImageTextValue(Data.WebResource.Instance().ImageResolve(user.Id.Value, "1", 4), "ͷ��", 100, new UIClick("id", user.Id.ToString(), "seq", "1")
                  { Model = "Design", Command = "Image" });

                  dic.AddCell("�ǳ�", user.Alias, new UIClick("Alias") { Command = "Setting", Model = "Account" });//.Put("Model", "Account").Put("Command", "Setting").Put("SendValue", "Alias");


                  //dic.Title.Right(new UIEventText("����").Click(new UIClick() { Model = "Account", Command = "Password" }));


                  var name = user.Name;
                  dic.AddCell('\uf084', "��¼�˺�", name, new UIClick() { Model = "Account", Command = "Password" });



                  var ac = account[Security.Account.EMAIL_ACCOUNT_KEY];
                  if (ac != null && String.IsNullOrEmpty(ac.Name) == false)
                  {

                      name = ac.Name;

                      int c = name.IndexOf('@');
                      if (c > 0)
                      {
                          var cname = name.Substring(0, c);
                          name = name.Substring(0, 2) + "***" + name.Substring(c);
                      }
                      if ((ac.Flags & Security.UserFlags.UnVerification) == Security.UserFlags.UnVerification)
                      {
                          name = name + "(δ��֤)";

                      }
                  }
                  else
                  {
                      name = "�����";
                  }

                  var cui = dic.NewSection().AddCell('\uf199', "����", name, new UIClick() { Command = "Email", Model = "Account" });
                  //.Put("Model", "Account").Put("Command", "Email")

                  //.Put("Name", "Email");

                  ac = account[Security.Account.MOBILE_ACCOUNT_KEY];
                  if (ac != null && String.IsNullOrEmpty(ac.Name) == false)
                  {
                      name = ac.Name;
                      if (name.Length > 3)
                      {
                          name = name.Substring(0, 3) + "****" + name.Substring(name.Length - 3);
                      }
                      if ((ac.Flags & Security.UserFlags.UnVerification) == Security.UserFlags.UnVerification)
                      {
                          name = name + "(δ��֤)";
                      }
                  }
                  else
                  {
                      name = "�����";
                  }

                  cui.AddCell('\ue91a', "�ֻ�����", name, new UIClick() { Command = "Mobile", Model = "Account" });

                  UICell cell = UICell.Create("UI", new UMC.Web.WebMeta().Put("text", "�˳���¼").Put("Icon", "\uf011").Put("click", new UIClick() { Model = "Account", Command = "Close" }));
                  cell.Style.Name("text", new UIStyle().Color(0xf00));
                  dic.NewSection().NewSection().Add(cell);



                  response.Redirect(dic);
                  return this.DialogValue("none");
              });
            switch (Model)
            {
                case "Alias":
                    String Alias = this.AsyncDialog("Alias", a => new UITextDialog() { Title = "�޸ı���" });
                    Membership.Instance().ChangeAlias(user.Name, Alias);
                    this.Prompt(String.Format("�����˻��ı������޸ĳ�{0}", Alias), false);
                    this.Context.Send("Setting", true);


                    break;
            }

        }
    }
}