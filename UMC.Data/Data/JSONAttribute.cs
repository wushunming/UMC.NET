using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data
{

    /// <summary>
    /// ��ʾ������Կ�����JSON���л�
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class JSONAttribute : System.Attribute
    {
    }
}
