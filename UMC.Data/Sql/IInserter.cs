using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ʵ�����������
    /// </summary>
    public interface IInserter
    {
        /// <summary>
        /// ��������ʵ�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ages"></param>
        /// <returns></returns>
        bool Execute<T>(params T[] ages);

        /// <summary>
        /// �������ݣ����ñ������ֵ��������
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="values">���µ��ֶζ�</param>
        /// <returns>������Ӱ�������</returns>
        int Execute(string name, params System.Collections.IDictionary[] values);

        /// <summary>
        /// ���뵥һʵ����󣬷��ص�ǰ���@@identity
        /// </summary>
        /// <typeparam name="T">�򵥶�������</typeparam>
        /// <param name="obj">����</param>
        /// <returns></returns>
        int ExecuteSingle(object obj);
        /// <summary>
        /// �����ݱ���в������ݿ����
        /// </summary>
        /// <param name="table">��</param>
        /// <returns></returns>
        bool Execute(System.Data.DataTable table);
        /// <summary>
        /// �����ݱ���в������ݿ����
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        int Execute(System.Data.IDataReader reader, string table);
    }
}
