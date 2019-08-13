using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ≈≈–Ú
    /// </summary>
    //interface IOrder
    //{
    //    /// <summary>
    //    /// Desc
    //    /// </summary>
    //    /// <param name="fieldName">≈≈–Ú◊÷∂Œ</param>
    //    /// <returns></returns>
    //    IOrder Desc(string fieldName);
    //    /// <summary>
    //    ///  Asc
    //    /// </summary>
    //    /// <param name="fieldName">≈≈–Ú◊÷∂Œ</param>
    //    /// <returns></returns>
    //    IOrder Asc(string fieldName);

    //    /// <summary>
    //    /// «Âø’≈≈–Ú…Ë÷√
    //    /// </summary>
    //    void Clear();
    //}

    public interface IOrder<T> where T : class
    {
        /// <summary>
        /// Desc
        /// </summary>
        /// <param name="fieldName">≈≈–Ú◊÷∂Œ</param>
        /// <returns></returns>
        IOrder<T> Desc(string fieldName);
        /// <summary>
        ///  Asc
        /// </summary>
        /// <param name="fieldName">≈≈–Ú◊÷∂Œ</param>
        /// <returns></returns>
        IOrder<T> Asc(string fieldName);

        /// <summary>
        /// «Âø’≈≈–Ú…Ë÷√
        /// </summary>
        IOrder<T> Clear();
        IOrder<T> Asc(T field);
        IOrder<T> Desc(T field);
        IObjectEntity<T> Entities
        {
            get;
        }
    }

    public interface IGroupOrder<T> where T : class
    {
        /// <summary>
        /// Desc
        /// </summary>
        /// <param name="fieldName">≈≈–Ú◊÷∂Œ</param>
        /// <returns></returns>
        IGroupOrder<T> Desc(string fieldName);
        /// <summary>
        ///  Asc
        /// </summary>
        /// <param name="fieldName">≈≈–Ú◊÷∂Œ</param>
        /// <returns></returns>
        IGroupOrder<T> Asc(string fieldName);

        /// <summary>
        /// «Âø’≈≈–Ú…Ë÷√
        /// </summary>
        IGroupOrder<T> Clear();
        IGroupOrder<T> Asc(T field);
        IGroupOrder<T> Desc(T field);
        IGrouper<T> Entities
        {
            get;
        }
    }
}
