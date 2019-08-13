using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ʵ�����͵�Where��ѯ�ӿڣ�ע�����и��Ӽ�����
    /// </summary>

    public interface IWhere<T> where T : class
    {
        /// <summary>
        /// ���û��ȡ���õ��ֶ�����ֵ
        /// </summary>
        /// <param name="name">�ֶ���</param>
        /// <returns></returns>
        object this[string name] { get; set; }
        /// <summary>
        /// ������е���������
        /// </summary>
        //IWhere Reset();
        /// <summary>
        /// �Ƴ���������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int Remove(string name);

        /// <summary>
        /// ��ǰ��������
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// ����Sql���ʽ�Ĳ�ѯ����
        /// </summary>
        /// <param name="expression">sql���ʽ</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        //IWhere Or(string expression, params Object[] paramers);

        /// <summary>
        /// ����Sql���ʽ�Ĳ�ѯ����
        /// </summary>
        /// <param name="expression">sql���ʽ</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        //IWhere And(string expression, params Object[] paramers);
        /// <summary>
        ///��ȡ Or����
        /// </summary>
        /// <returns></returns>
        //IOperator Or();
        ///// <summary>
        /////��ȡ And����
        ///// </summary>
        ///// <returns></returns>
        //IOperator And();

        /// <summary>
        /// ������С���� SQL WHERE���������� ��(field1=1 AND field2=2)
        /// </summary>
        //IWhere Contains();
        //}
        ///// <summary>
        ///// ʵ��ʵ���ѯ
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //public interface IWhere<T> : IWhere where T : class
        //{
        IObjectEntity<T> Entities
        {
            get;
        }

        /// <summary>
        /// �������е���������
        /// </summary>
        IWhere<T> Reset();

        /// <summary>
        /// �Ƴ�
        /// </summary>
        /// <param name="field">ʵ������</param>
        /// <returns></returns>
        IWhere<T> Remove(T field);

        /// <summary>
        /// �滻����
        /// </summary>
        /// <param name="field">ʵ������</param>
        /// <returns></returns>
        IWhere<T> Replace(T field);

        /// <summary>
        /// ����Sql���ʽ�Ĳ�ѯ����
        /// </summary>
        /// <param name="expression">sql���ʽ</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        IWhere<T> Or(string expression, params Object[] paramers);


        /// <summary>
        /// ����Sql���ʽ�Ĳ�ѯ����
        /// </summary>
        /// <param name="expression">sql���ʽ</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        IWhere<T> And(string expression, params Object[] paramers);
        /// <summary>
        ///��ȡ Or����
        /// </summary>
        /// <returns></returns>
        IOperator<T> Or();
        /// <summary>
        ///��ȡ And����
        /// </summary>
        /// <returns></returns>
        IOperator<T> And();

        /// <summary>
        ///ʵ������Or
        /// </summary>
        /// <returns></returns>
        IWhere<T> Or(T field);
        /// <summary>
        ///ʵ������And
        /// </summary>
        /// <returns></returns>
        IWhere<T> And(T field);

        /// <summary>
        /// ������С���� SQL WHERE���������� ��(field1=1 AND field2=2)
        /// </summary>
        /// <returns></returns>
        IWhere<T> Contains();
    }
}
