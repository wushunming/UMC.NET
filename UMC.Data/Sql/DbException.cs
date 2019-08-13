using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// SQL��ʽ������
    /// </summary>
    public class SqlFormatException : FormatException
    {
        string name;
        /// <summary>
        /// SQL��ʽ������
        /// </summary>
        public SqlFormatException(string name)
        {
            this.name = name;
        }
        public override string Message
        {
            get
            {
                return "��ʹ���Զ�ǰ׺ʱ�������" + this.name + "����.��";
            }
        }
    }
    /// <summary>
    /// ���ݴ����������쳣
    /// </summary>
    public class DbException : Exception
    {
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="ex">ԭʼ�쳣</param>
        /// <param name="cmd">�ϳ����ݿ��������</param>
        public DbException(Exception ex, System.Data.Common.DbCommand cmd)
            : base(ex.Message, ex)
        {
            this.cmd = cmd;
        }

        System.Data.Common.DbCommand cmd;
        /// <summary>
        /// ���ݴ������ϳɵĵ�Sql����
        /// </summary>
        public System.Data.Common.DbCommand DbCommand
        {
            get
            {
                return cmd;
            }
        }
    }

}
