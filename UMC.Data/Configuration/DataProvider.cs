using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Configuration
{
    /// <summary>
    /// �������ý�
    /// </summary>
    public abstract class DataProvider
    {
        /// <summary>
        /// ���õĽڵ�
        /// </summary>
        public UMC.Data.Provider Provider
        {
            internal set;
            get;
        }
    }
}
