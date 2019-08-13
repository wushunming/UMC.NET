using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UMC.Net
{
    public abstract class Message : UMC.Configuration.DataProvider
    {
        static Message _Instance;
        private class Messager : Message
        {
            public override void Send(String type, Hashtable content, string number)
            {

            }

            public override void Send(string content, params string[] to)
            {

            }
        }

        public static Message Instance()
        {
            if (_Instance == null)
            {
                _Instance = UMC.Data.Reflection.CreateObject("Msg") as Message;
                if (_Instance == null)
                {
                    _Instance = new Messager();
                    UMC.Data.Reflection.SetProperty(_Instance, "Provider", Data.Provider.Create("WebResource", "UMC.Data.WebResource"));
                }
            }
            return _Instance;
        }

        #region IMessage Members

        public abstract void Send(String type, System.Collections.Hashtable content, string number);
        public abstract void Send(string content, params string[] to);
        public virtual void Send(System.Net.Mail.MailMessage message)
        {
            var client = new System.Net.Mail.SmtpClient(this.Provider.Attributes["Host"]
                , UMC.Data.Utility.IntParse(this.Provider.Attributes["Port"], 25));
            if (!String.IsNullOrEmpty(this.Provider.Attributes["Username"]) && !String.IsNullOrEmpty(this.Provider.Attributes["Password"]))
            {
                client.Credentials = new System.Net.NetworkCredential(this.Provider.Attributes["Username"], this.Provider.Attributes["Password"]);
            }
            client.EnableSsl = this.Provider.Attributes["EnableSsl"] == "true";
            message.From = new System.Net.Mail.MailAddress(this.Provider["From"]);
            client.Send(message);
        }

        #endregion
    }
}
