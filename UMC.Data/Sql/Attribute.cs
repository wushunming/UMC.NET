using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ��ָ�����ֶΣ����û�ж���Ĭ�϶�Ӧ��������
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : System.Attribute
    {
        /// <summary>
        /// Ĭ�϶�Ӧ��������
        /// </summary>
        public FieldAttribute()
        {
            this.Insert = true;
            this.Select = true;
            this.Update = true;
            this.AutoField = false;
        }
        /// <summary>
        /// ��ʾ���Զ�Ӧ���ֶ�
        /// </summary>
        /// <param name="fName">��Ӧ�ֶ���</param>
        public FieldAttribute(string fName)
            : base()
        {
            if (string.IsNullOrEmpty(fName))
            {
                throw new System.ArgumentNullException("fName");
            }
            this.Field = fName;
        }
        /// <summary>
        /// ��ʾ���Զ�Ӧ���ֶ�
        /// </summary>
        /// <param name="fName">��Ӧ�ֶ���</param>
        /// <param name="isExpression">�Ƿ��Ǳ�ʾʾ</param>
        /// <remarks>ע�⣺�����ʹ�ñ��ʽ�ͰѶԾ͵�ת������������ͬ�ֶ���</remarks>
        public FieldAttribute(string fName, bool isExpression)
            : this(fName)
        {
            this.IsExpression = isExpression;
            if (isExpression)
            {
                this.Update = false;
                this.Insert = false;
            }
        }

        /// <summary>
        /// ����Ϊ�Զ������У�����в��ɸ��£�Ҳ���ɲ���
        /// </summary>
        /// <param name="auto">�Ƿ����Զ�������</param>
        public FieldAttribute(bool auto)
        {
            this.Select = true;
            this.AutoField = true;
            if (auto)
            {
                this.Insert = false;
                this.Update = false;
            }
            else
            {
                this.Insert = true;
                this.Select = true;
            }
        }
        /// <summary>
        /// �Ƿ��Ǳ��ʽ
        /// </summary>
        public bool IsExpression
        {
            get;
            private set;
        }
        /// <summary>
        /// �󶨵��ֶ���
        /// </summary>
        public string Field
        {
            get;
            set;
        }
        /// <summary>
        /// ���������ݿ�ʱ���Ƿ���´��ֶ�
        /// </summary>
        public bool Update
        {
            get;
            set;
        }
        /// <summary>
        /// ����ѯ���ݿ�ʱ���ǲ��ǰ��ֶ�ֵ���ڵ�ǰ����
        /// </summary>
        public bool Select
        {
            get;
            set;
        }
        /// <summary>
        /// ���������ݿ�ʱ���ǲ��ǰ�����ֵ��䵽��Ӧ���ֶ���
        /// </summary>
        public bool Insert
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ����Զ�������
        /// </summary>
        public bool AutoField
        {
            get;
            private set;
        }
    }
    /// <summary>
    /// ����ʵ���Ӧ�ı���
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : System.Attribute
    {
        public TableAttribute()
        {
        }
        public TableAttribute(string tabName)
        {
            this.Name = tabName;
        }
        /// <summary>
        /// ���������û�ж��壬Ĭ�϶����������
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
