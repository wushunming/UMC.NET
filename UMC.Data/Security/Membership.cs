using System;
using System.Collections.Generic;
using System.Text;


namespace UMC.Security
{

    /// <summary>
    /// �û���ȫ��ǩ
    /// </summary>
    public enum UserFlags
    {
        /// <summary>
        /// ����
        /// </summary>
        Normal = 0,
        /// <summary>
        /// ����
        /// </summary>
        Lock = 1,
        /// <summary>
        /// Ҫ��������
        /// </summary>
        Changing = 2,
        /// <summary>
        /// ���ܸ�������
        /// </summary>
        UnChangePasswork = 4,
        /// <summary>
        /// û��ͨ����֤
        /// </summary>
        UnVerification = 8,
        /// <summary>
        /// ����
        /// </summary>
        Disabled = 16

    }
    /// <summary> 
    /// �û�������
    /// </summary>
    public abstract class Membership : UMC.Configuration.DataProvider
    {
        public const string SessionCookieName = ".WDKTemporary";
        
        /// <summary>
        /// ����Ա��ɫ
        /// </summary>
        public const String AdminRole = "Administrators";
        /// <summary>
        /// һ���û���ɫ
        /// </summary>
        public const String UserRole = "Users";
        /// <summary>
        /// ������ɫ
        /// </summary>
        public const String GuestRole = "Guest";
        /// <summary>
        /// ʵ��
        /// </summary>
        /// <returns></returns>
        public static Membership Instance()
        {
            return UMC.Data.Reflection.CreateObject("Membership") as Membership ?? new UMC.Configuration.Membership();
        }

        /// <summary>
        /// ��ȡ�û����
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public abstract Identity Identity(string username);
        /// <summary> 
        /// �����û�����
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="maxtimes">ʧ��������</param>
        /// <returns>ʧ�ܴ���</returns>
        public abstract int Password(string username, string password, int maxtimes);

        /// <summary> 
        /// ��ȡ�û�������ͽ�ɫ
        /// </summary>
        /// <param name="username">�û���</param>
        /// <returns>�������룬�����null�����ʾ�޴��û�</returns>
        public abstract string Password(string username);
        /// <summary>
        /// ��ȡ�û����
        /// </summary>
        /// <param name="SessionKey">�û���ʾ</param>
        /// <param name="contentType">�û��ն�</param>
        /// <returns></returns>
        public virtual UMC.Security.Principal Authorization(Guid SessionKey, String contentType)
        {
            if (SessionKey != Guid.Empty)
            {

                var session = new Configuration.Session<Security.AccessToken>(SessionKey.ToString(), false);
                if (session.Value != null)
                {
                    var auth = session.Value;
                    auth.Id = SessionKey;
                    auth.ContentType = session.ContentType;
                    return Authorization(auth, auth.Identity());

                }
                var user = UMC.Security.Identity.Create(SessionKey, "?", String.Empty);
                return UMC.Security.Principal.Create(user, Security.AccessToken.Create(user, SessionKey, contentType, 0));

            }
            var id = UMC.Security.Identity.Create("?", String.Empty);
            return UMC.Security.Principal.Create(id, Security.AccessToken.Create(id, Guid.Empty, contentType, 0));

        }
        /// <summary>
        /// ͨ��ID��ȡ�û����
        /// </summary>
        /// <returns></returns>
        public abstract UMC.Security.Identity Identity(Guid id);
        /// <summary>
        /// ͨ���󶨵ĵ������˻���ȡ�û����
        /// </summary>
        /// <param name="name"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public abstract UMC.Security.Identity Identity(string name, int accountType);
        /// <summary>
        /// ��¼���߳�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public virtual UMC.Security.Principal Authorization(Security.AccessToken auth, UMC.Security.Identity id)
        {
            var passDate = Data.Utility.TimeSpan();// DateTime.Now.AddMinutes(-10));
            if (auth.ActiveTime < passDate - 600)
            {
                Data.Reflection.SetProperty(auth, "ActiveTime", passDate);

                this.Activation(auth);
            }
            return UMC.Security.Principal.Create(id, auth);
        }
        /// <summary>
        /// �任����
        /// </summary>
        /// <param name="username">�û�����</param>
        /// <param name="password"></param>
        public abstract bool Password(string username, string password);
        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public abstract Identity CreateUser(Guid id, string username, string password, string alias);
        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public abstract Guid CreateUser(string username, string password, string alias);
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="alias">����</param>
        /// <returns></returns>
        public abstract bool ChangeAlias(string username, string alias);
        /// <summary>
        /// �Ƿ��������û���
        /// </summary>
        /// <returns></returns>
        public abstract bool Exists(string username);
        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public abstract bool DeleteUser(string username);
        /// <summary>
        /// ����״̬��־
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="flags">�û�״̬��ʾ</param>
        /// <returns></returns>
        public abstract bool ChangeFlags(string username, UserFlags flags);

        /// <summary>
        /// �Ƴ��û���ɫ
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="role">��ɫ</param>
        /// <returns></returns> 
        public abstract bool RemoveRole(string username, string role);
        /// <summary>
        /// �����û���ɫ
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="role">�û���</param>
        /// <returns></returns> 
        public abstract bool AddRole(string username, params string[] roles);

        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="token">��¼���Ʊ��</param>
        public abstract void Activation(UMC.Security.AccessToken token);


    }

}
