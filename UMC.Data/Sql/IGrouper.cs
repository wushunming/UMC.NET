using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ʵ�����������
    /// </summary>
    public interface IGrouper<T> : IScript where T : class
    {
        T Single();
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns></returns>
        System.Data.DataTable Query();
        /// <summary>
        /// ��ѯ����
        /// </summary>
        //void Query(DataReader reader);
        /// <summary>
        /// ��ѯ����
        /// </summary>
        void Query(DataReader<T> reader);
        /// <summary>
        /// ����
        /// </summary>
        IGroupOrder<T> Order { get; }

        IGrouper<T> Count(String asName);
        /// <summary>
        /// ���¼��,��Ӧ���ֶ�Ϊ"G"+(i+1)��iΪͳ�ƴ���,���磺e.GroupBy("field'}).Count(),��Count������Ϊ"G1"
        /// </summary>
        /// <returns></returns>
        IGrouper<T> Count(T field);
        /// <summary>
        /// ���¼��,��Ӧ���ֶ�Ϊ"G"+(i+1)��iΪͳ�ƴ���,���磺e.GroupBy("field'}).Count(),��Count������Ϊ"G1"
        /// </summary>
        /// <returns></returns>
        IGrouper<T> Count();
        /// <summary>
        /// ���,��Ӧ���ֶ�Ϊ"G"+(i+1)��iΪͳ�ƴ���,���磺e.GroupBy("field'}).Sum(),��Sum������Ϊ"G1"
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Sum(string field);
        IGrouper<T> Sum(string field, string asName);
        /// <summary>
        /// ��ƽ��,��Ӧ���ֶ�Ϊ"G"+(i+1)��iΪͳ�ƴ���,���磺e.GroupBy("field'}).Sum(field),��Sum������Ϊ"G1"
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Avg(string field);
        IGrouper<T> Avg(string field, string asName);
        /// <summary>
        /// �����ֵ,��Ӧ���ֶ�Ϊ"G"+(i+1)��iΪͳ�ƴ���,���磺e.GroupBy("field'}).Max(field),��Max������Ϊ"G1"
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Max(string field);
        IGrouper<T> Max(string field, string asName);
        /// <summary>
        /// ����Сֵ,,��Ӧ���ֶ�Ϊ"G"+(i+1)��iΪͳ�ƴ���,���磺e.GroupBy("field'}).Min(field),��Min������Ϊ"G1"
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Min(string field);
        IGrouper<T> Min(string field, string asName);
        /// <summary>
        /// ���,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Sum(T field);
        /// <summary>
        /// ��ƽ��,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Avg(T field);
        /// <summary>
        /// �����ֵ,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Max(T field);
        /// <summary>
        /// ����Сֵ,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        IGrouper<T> Min(T field);
        ///// <summary>
        ///// ���,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        ///// </summary>
        ///// <param name="field">�ֶ�</param>
        ///// <returns></returns>
        //IGrouper<T> Sum(T field, T asField);
        ///// <summary>
        ///// ��ƽ��,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        ///// </summary>
        ///// <param name="field">�ֶ�</param>
        ///// <returns></returns>
        //IGrouper<T> Avg(T field, T asField);
        ///// <summary>
        ///// �����ֵ,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        ///// </summary>
        ///// <param name="field">�ֶ�</param>
        ///// <returns></returns>
        //IGrouper<T> Max(T field, T asField);
        ///// <summary>
        ///// ����Сֵ,��Ӧ���ֶ�ֵΪʵ��ǿ��ֶ�
        ///// </summary>
        ///// <param name="field">�ֶ�</param>
        ///// <returns></returns>
        //IGrouper<T> Min(T field, T asField);
    }
}
