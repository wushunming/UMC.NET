using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ���ݷ��ʹ����ṩ��
    /// </summary>
    public abstract class DbCommonProvider : UMC.Configuration.DataProvider
    {


        public virtual string Year(string feild)
        {
            return feild;

        }
        public virtual string Month(string feild)
        {

            return feild;
        }
        public virtual string Day(string feild)
        {

            return feild;
        }

        public virtual string Quarter(string feild)
        {

            return feild;
        }
        public virtual string Minute(string feild)
        {
            return feild;

        }
        public virtual string Hour(string feild)
        {

            return feild;
        }
        public virtual string Week(string feild)
        {

            return feild;
        }
        /// <summary>
        /// ���������ַ���
        /// </summary>
        public abstract string ConntionString
        {
            get;
        }
        /// <summary>
        /// ׷�Ӳ�����ͬʱ�������ݿ������
        /// </summary>
        /// <param name="key">�ؼ���</param>
        /// <param name="obj">����ֵ</param>
        /// <param name="cmd">����</param>
        /// <returns></returns>
        public virtual string AppendDbParameter(string key, object obj, DbCommand cmd)
        {
            if (String.IsNullOrEmpty(ParamsPrefix) == false)
            {
                if (key.StartsWith(ParamsPrefix) == false)
                {
                    key = ParamsPrefix + key;
                }
                if (cmd.Parameters.Contains(key))
                {
                    return key;
                }
            }
            var parm = this.DbFactory.CreateParameter();
            parm.ParameterName = key;
            if (obj == null)
            {
                parm.Value = DBNull.Value;
            }
            else
            {
                if (obj is DateTime)
                {
                    parm.DbType = System.Data.DbType.Date;
                    if (Convert.ToDateTime(obj) == DateTime.MinValue)
                    {
                        parm.Value = DBNull.Value;
                    }
                    else
                    {
                        parm.Value = obj;
                    }

                }
                else
                {

                    parm.Value = obj;
                }
            }
            cmd.Parameters.Add(parm);
            if (string.IsNullOrEmpty(this.ParamsPrefix))
            {
                return "?";
            }
            return key;

        }

        /// <summary>
        /// ����ǰ׺
        /// </summary>
        protected virtual string ParamsPrefix
        {
            get
            {
                return this.Provider.Attributes["ParamsPrefix"] ?? "@";
            }
        }
        /// <summary>
        /// ���ݷ���
        /// </summary>
        /// <returns></returns>
        public abstract System.Data.Common.DbProviderFactory DbFactory
        {
            get;
        }
        /// <summary>
        /// ʵ��ӳ�����ǰ׺
        /// </summary>
        public virtual string Prefixion
        {
            get
            {
                string Prefix = "";
                if (this.Provider != null)
                {
                    Prefix = this.Provider.Attributes["Prefix"];
                    if (String.IsNullOrEmpty(Prefix))
                    {
                        return "";
                    }
                }
                return Prefix;

            }
        }

        /// <summary>
        /// ʵ��ӳ�����ǰ׺�ָ��
        /// </summary>
        public virtual string Delimiter
        {
            get
            {
                if (this.Provider != null)
                {

                    return this.Provider.Attributes["delimiter"] ?? ".";

                }
                return ".";

            }
        }
        /// <summary>
        ///  ָ�������ư����ո������ǵ��ַ������ݿ�������磬����У�ʱʹ�õ���ʼ�ַ���
        /// </summary>
        public virtual string QuotePrefix
        {
            get
            {
                return this.Provider.Attributes["QuotePrefix"] ?? "";
            }
        }
        /// <summary>
        /// ָ�������ư����ո������ǵ��ַ������ݿ�������磬����У�ʱʹ�õĽ����ַ�
        /// </summary>
        public virtual string QuoteSuffix
        {
            get
            {
                return this.Provider.Attributes["QuoteSuffix"] ?? "";
            }
        }
        /// <summary>
        /// ȡ�������е�Sql�ű�
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public abstract string GetIdentityText(string tableName);
        /// <summary>
        /// �ϳɷ�ҳ��Sql��ʽ�ű���ע�Ȿ��ҳ�ǲ���Top��ȥ��ҳ������������Դ��֧��Top���㣬����д
        /// </summary>
        /// <param name="start">��ʼ��¼��)</param>
        /// <param name="limit">���������</param>
        /// <param name="selectText">��ѯ��Sql�ű�</param>
        /// <returns></returns>
        public abstract string GetPaginationText(int start, int limit, string selectText);
    }
}
