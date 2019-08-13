using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Collections;
using UMC.Data.Entities;

namespace UMC.Security
{
    /// <summary>
    /// ��Ȩ����
    /// </summary>
    public class AuthManager
    {
        System.Security.Principal.IPrincipal principal;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        private AuthManager(System.Security.Principal.IPrincipal principal)
        {
            this.principal = principal;
        }

        /// <summary>
        /// ��֤�û��Ƿ���ͨ��ͨ���
        /// </summary>
        /// <param name="wildcard">ͨ���</param>
        /// <returns></returns>
        public static Boolean IsAuthorization(string wildcard)
        {
            return IsAuthorization(System.Threading.Thread.CurrentPrincipal, wildcard);
        }
        /// <summary>
        /// ������֤Ȩ��
        /// </summary>
        public static bool[] IsAuthorization(params string[] wildcards)
        {
            return IsAuthorization(System.Threading.Thread.CurrentPrincipal, wildcards);
        }
        /// <summary>
        /// ������֤Ȩ��
        /// </summary>
        /// <param name="wildcards"></param>
        /// <returns></returns>
        public static bool[] IsAuthorization(System.Security.Principal.IPrincipal princ, params string[] wildcards)
        {
            bool[] rerValue = new bool[wildcards.Length];
            if (wildcards.Length > 0)
            {
                if (princ.IsInRole(UMC.Security.Membership.AdminRole))
                {
                    for (var i = 0; i < rerValue.Length; i++)
                    {
                        rerValue[i] = true;
                    }
                    return rerValue;
                }
                var list = new List<String>();
                foreach (var wildcard in wildcards)
                {
                    list.Add(wildcard);
                    int l = wildcard.Length - 1;

                    while (l > -1)
                    {
                        switch (wildcard[l])
                        {
                            case '.':
                                list.Add(wildcard.Substring(0, l) + ".*");
                                break;
                        }
                        l--;
                    }
                }
                var wMger = new AuthManager(princ);
                var vs = wMger.Check(list.ToArray());
                int start = 0, end = 0;

                for (int i = 1; i < wildcards.Length; i++)
                {
                    end = list.FindIndex(w => wildcards[i] == w);
                    rerValue[i - 1] = IsAuthorization(vs, start, end);
                    start = end;


                }
                rerValue[wildcards.Length - 1] = IsAuthorization(vs, start, vs.Length);
            }
            return rerValue;
        }
        static bool IsAuthorization(int[] vs, int start, int end)
        {
            for (var c = start; c < end; c++)
            {
                if (vs[c] != 0)
                {
                    return vs[c] > 0;
                }
            }

            return true;
        }

        /// <summary>
        /// ��֤Ȩ�޷�SId
        /// </summary>
        /// <param name="sid">Ȩ�޷�SId</param>
        public static bool IsAuthorization(Guid sid)
        {
            return IsAuthorization(sid.ToString());
        }
        /// <summary>
        /// ��֤Ȩ�޷�SId,������ش���0��ͨ�����������0��ʾû�����ù������С��0��ʾû��ͨ��
        /// </summary>
        /// <param name="sid">Ȩ�޷�SId</param>
        /// <param name="principal">�û����</param>
        /// <returns></returns>
        public static int IsAuthorization(Guid sid, System.Security.Principal.IPrincipal principal)
        {
            if (principal == null)
            {
                throw new System.ArgumentNullException("principal");
            }
            if (!principal.IsInRole(UMC.Security.Membership.AdminRole))
            {
                AuthManager wMger = new AuthManager(principal);//as WildcardManager;
                return wMger.Check(sid.ToString())[0];
            }
            else
            {
                return 1;
            }
        }


        /// <summary>
        /// ����Ȩ��
        /// </summary>
        /// <param name="sourceKey">ԴȨ��SID</param>
        /// <param name="destKey">Ŀ��Ȩ��SID</param>
        public static bool CopyAuthorize(string sourceKey, string destKey)
        {
            AuthManager wMger = new AuthManager(System.Threading.Thread.CurrentPrincipal);
            return wMger.Copy(sourceKey, destKey);
        }
        /// <summary>
        /// ���Ȩ������
        /// </summary>
        /// <param name="wildcardKey"></param>
        public static void Unauthorize(string wildcardKey)
        {
            AuthManager wMger = new AuthManager(System.Threading.Thread.CurrentPrincipal);
            wMger.Delete(wildcardKey);
        }
        /// <summary>
        /// ��֤�û��Ƿ���ͨ��ͨ���
        /// </summary>
        /// <param name="wildcard">ͨ���</param>
        /// <param name="principal">�û����</param>
        /// <returns></returns>
        public static bool IsAuthorization(System.Security.Principal.IPrincipal principal, string wildcard)
        {

            return IsAuthorization(System.Threading.Thread.CurrentPrincipal, new string[] { wildcard })[0];

        }


        //protected abstract int[] Check(params string[] wildcards);
        ///// <summary>
        ///// ����
        ///// </summary>
        ///// <param name="wildcard"></param>
        //protected abstract void Unauthorization(string wildcard);

        ///// <summary>
        ///// ����Ȩ��
        ///// </summary>
        ///// <param name="sourceKey"></param>
        ///// <param name="destKey"></param>
        //protected abstract bool Copy(string sourceKey, string destKey);

        int Check(UMC.Security.Authorize[] Authorizes)
        {
            int isAllowRoles = 0;
            int isAllowUser = 0;
            var username = this.principal.Identity.Name;

            foreach (var dr in Authorizes)
            {

                var sValue = dr.Value;
                switch (dr.Type)
                {
                    case AuthorizeType.UserAllow:
                        switch (sValue)
                        {
                            case "*":
                                if (isAllowUser == 0)
                                {
                                    isAllowUser = 1;
                                };
                                break;
                            case "?":
                                if (this.principal.Identity.IsAuthenticated == false)
                                {
                                    isAllowUser = 1;
                                }
                                break;
                            default:
                                if (String.Equals(username, sValue, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    isAllowUser = 1;
                                }
                                break;
                        }
                        break;
                    case AuthorizeType.RoleAllow:
                        if (sValue == "*")
                        {
                            if (isAllowRoles == 0)
                            {
                                isAllowRoles = 1;
                            }
                        }
                        else if (isAllowRoles != 1)
                        {
                            if (principal.IsInRole(sValue))
                            {
                                isAllowRoles = 1;
                            }
                            else
                            {
                                isAllowRoles = -1;
                            }
                        }
                        break;
                    case AuthorizeType.UserDeny:
                        if (isAllowUser > -1)
                        {
                            switch (sValue)
                            {
                                case "*":
                                    if (isAllowUser == 0)
                                    {
                                        isAllowUser = -1;
                                    };
                                    break;
                                case "?":
                                    if (this.principal.Identity.IsAuthenticated == false)
                                    {
                                        isAllowUser = -1;
                                    }
                                    break;
                                default:
                                    if (String.Equals(username, sValue, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        isAllowUser = -1;
                                    }
                                    break;
                            }
                        }
                        break;
                    case AuthorizeType.RoleDeny:
                        if (sValue == "*")
                        {
                            if (isAllowRoles == 0)
                            {
                                isAllowRoles = -1;
                            }
                        }
                        else if (isAllowRoles != -1 && principal.IsInRole(sValue))
                        {
                            isAllowRoles = -1;

                        }
                        break;
                }
            }
            if (isAllowUser == 0)
            {
                return isAllowRoles;
            }
            return isAllowUser;
        }
        protected virtual void Delete(string wildcard)
        {
            var entityWDKWildcards = UMC.Data.Database.Instance().ObjectEntity<Wildcard>();

            entityWDKWildcards.Where.And(new Wildcard { WildcardKey = wildcard });
            entityWDKWildcards.Delete();
        }
        protected virtual bool Copy(string sourceKey, string destKey)
        {
            var entityWDKWildcards = UMC.Data.Database.Instance().ObjectEntity<Wildcard>();

            entityWDKWildcards.Where.And(new Wildcard { WildcardKey = sourceKey });

            var wildcard = entityWDKWildcards.Single();
            if (wildcard != null)
            {
                wildcard.WildcardKey = destKey;
                entityWDKWildcards.Where.Replace(new Wildcard { WildcardKey = wildcard.WildcardKey });
                if (entityWDKWildcards.Count() == 0)
                {
                    entityWDKWildcards.Insert(wildcard);
                }
            }

            return true;
        }

        /// <summary>
        /// ��֤�û��Ƿ���ͨ��ͨ���,0����û���ҵ�ͨ�ã�1��ʾͨ����-1��ʾû��ͨ��
        /// </summary>
        /// <param name="wildcards">ͨ���</param>
        /// <returns></returns>
        protected virtual int[] Check(params string[] wildcards)
        {

            var vs = new List<int>();
            if (wildcards.Length > 0)
            {
                var entityWDKWildcards = UMC.Data.Database.Instance().ObjectEntity<Wildcard>();

                entityWDKWildcards.Where.And().In("WildcardKey", wildcards);
                var authorizes = new List<Wildcard>(entityWDKWildcards.Query());
                foreach (var w in wildcards)
                {
                    var au = authorizes.Find(a => a.WildcardKey == w);
                    if (au != null)
                    {
                        if (String.IsNullOrEmpty(au.Authorizes))
                        {
                            vs.Add(0);
                        }
                        else
                        {
                            vs.Add(Check(UMC.Data.JSON.Deserialize<UMC.Security.Authorize[]>(au.Authorizes)));

                        }
                    }
                    else
                    {
                        vs.Add(0);
                    }
                }
            }
            return vs.ToArray();

        }
    }
}
