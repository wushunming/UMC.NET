using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// sql������
    /// </summary>
    public interface ISqler : IScript
    {
        /// <summary>
        ///  ִ�� SQL ��䡣
        /// </summary>
        /// <param name="sqlText">������ʽ���ű�</param>
        /// <param name="paramers">����</param>
        /// <returns>����Ӱ������</returns>
        int ExecuteNonQuery(string sqlText, params object[] paramers);
        /// <summary>
        ///  ִ�� SQL ��䡣
        /// </summary>
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        /// <returns>����Ӱ������</returns>
        int ExecuteNonQuery(string sqlText, System.Collections.IDictionary paramKeys);
        /// <summary>
        ///  ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С������������к��н������ԡ�
        /// </summary>
        /// <param name="sqlText">������ʽ���ű�</param>
        /// <param name="paramers">����</param>
        /// <returns>������е�һ�еĵ�һ�С�</returns>
        object ExecuteScalar(string sqlText, params object[] paramers);

        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        object ExecuteScalar(string sqlText, System.Collections.IDictionary paramKeys);
        /// <summary>
        ///  �� System.Data.DataSet ����ӻ�ˢ������ƥ��ʹ�� System.Data.DataSet ���Ƶ�����Դ�е��У�������һ�� System.Data.DataTable��
        /// </summary>
        /// <param name="sqlText">������ʽ���ű�</param>
        /// <param name="paramers">����</param>
        /// <returns>����������Դ���صļܹ���Ϣ�� System.Data.DataTable ����</returns>
        System.Data.DataTable ExecuteTable(string sqlText, params object[] paramers);
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        System.Data.DataTable ExecuteTable(string sqlText, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// ���ط�ҳ�����ݱ�
        /// </summary>
        /// <param name="sqlText">sql��ѯ�ı�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        System.Data.DataTable Execute(string sqlText, int start, int limit, params object[] paramers);
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="paramKeys">�ֵ����</param>
        System.Data.DataTable Execute(string sqlText, int start, int limit, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// ����DataSet,ע���ѯ���ܰ���20����
        /// </summary>
        /// <param name="sqlText">������ʽ���ű�</param>
        /// <param name="paramers">����</param>
        /// <returns>����һ��DataSet</returns>
        System.Data.DataSet ExecuteDataSet(string sqlText, params object[] paramers);
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        System.Data.DataSet ExecuteDataSet(string sqlText, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// ���ز�ѯ�ĵ�һ�У��������ֵ�
        /// </summary>
        /// <param name="sqlText">������ʽ���ű�</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        System.Collections.IDictionary ExecuteSingle(string sqlText, params object[] paramers);
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        System.Collections.IDictionary ExecuteSingle(string sqlText, System.Collections.IDictionary paramKeys);

        /// <summary>
        /// �Ѳ�ѯ�����ֶ�ת����Ӧ�ĵ���ʵ��
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="sqlText">SQL��ѯ�ı�</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        T ExecuteSingle<T>(string sqlText, params object[] paramers);
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        T ExecuteSingle<T>(string sqlText, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// �Ѳ�ѯ�����ֶ�ת����Ӧ��ʵ�弯
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="sqlText">SQL��ѯ�ı�</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        List<T> Execute<T>(string sqlText, params object[] paramers);
        /// <summary>
        /// �Ѳ�ѯ�����ֶ�ת����Ӧ��ʵ�弯
        /// </summary>
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="paramKeys">�ֵ����</param>
        List<T> Execute<T>(string sqlText, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// �Զ��崦��һ����ѯֻ���Ľ����
        /// </summary>
        /// <param name="sqlText">SQL��ѯ�ı�</param>
        /// <param name="reader">�������ݼ�����</param>
        /// <param name="paramers">����</param>
        void Execute<T>(string sqlText, DataReader<T> reader, params object[] paramers);
        /// <summary>
        /// ���ط�ҳ�����ݱ�
        /// </summary>
        /// <param name="sqlText">sql��ѯ�ı�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="reader">�������ݼ�����</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        void Execute<T>(string sqlText, int start, int limit, DataReader<T> reader, params object[] paramers);

        /// <summary>
        /// ���ط�ҳ�����ݱ�
        /// </summary>
        /// <param name="sqlText">sql��ѯ�ı�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        List<T> Execute<T>(string sqlText, int start, int limit, params object[] paramers);
        /// <summary>
        /// ���ط�ҳ�����ݱ�
        /// </summary>
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="paramKeys">�ֵ����</param>
        List<T> Execute<T>(string sqlText, int start, int limit, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// �Զ��崦��һ����ѯֻ���Ľ����
        /// </summary>
        /// <param name="sqlText">SQL��ѯ�ı�</param>
        /// <param name="reader">�������ݼ�����</param>
        /// <param name="paramers">����</param>
        void Execute(string sqlText, DataReader reader, params object[] paramers);


        /// <summary>
        /// �����ҳ�����ݼ���ע�⣺��DataReader���Ѿ�Read���ģ�����reader�в�Ҫ����Read����
        /// </summary>
        /// <param name="sqlText">sql��ѯ�ı�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="reader">�������ݼ�����</param>
        /// <param name="paramers">����</param>
        /// <returns></returns>
        void Execute(string sqlText, int start, int limit, DataRecord reader, params object[] paramers);
        /// <summary>
        /// �����ҳ�����ݼ���ע�⣺��DataReader���Ѿ�Read���ģ�����reader�в�Ҫ����Read����
        /// </summary>
        /// <param name="sqlText">�ֵ��ʽ���ű�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="limit">��¼��</param>
        /// <param name="reader">�������ݼ�����</param>
        /// <param name="paramKeys">�ֵ����</param>
        void Execute(string sqlText, int start, int limit, DataRecord reader, System.Collections.IDictionary paramKeys);

        /// <summary>
        /// �Զ��崦��һ����ѯֻ���Ľ����
        /// </summary>
        /// <param name="sqlText">SQL��ѯ�ı�</param>
        /// <param name="reader">�������ݼ�����</param>
        /// <param name="paramKeys">�����ֵ�</param>
        void Execute(string sqlText, DataReader reader, System.Collections.IDictionary paramKeys);
        /// <summary>
        /// ����ִ�нű�������ÿ���ű��ķ���Ӱ������
        /// </summary>
        /// <param name="paramKeys"></param>
        /// <param name="sqlTexts"></param>
        /// <returns></returns>
        int[] ExecuteNonQuery(System.Collections.IDictionary paramKeys, params string[] sqlTexts);
        /// <summary>
        /// ����ִ�нű�������ÿ���ű��ķ���Ӱ������
        /// </summary>
        /// <param name="scripts"></param>
        /// <returns></returns>
        int[] ExecuteNonQuery(params Script[] scripts);

        /// <summary>
        /// ����ִ�нű� �� ��predicate����true��ִ��action����������action������script����Ľű�����
        /// </summary>
        /// <param name="predicate">��predicate����Ϊtrueʱ�����ص���Script��������ʼ��cmdexec</param>
        /// <param name="cmdexec">ִ�еĽű�</param>
        void Execute(System.Predicate<Script> predicate, System.Action<System.Data.Common.DbCommand> cmdexec);
    }

}
