using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UMC.Data.Sql;
using System.Collections;
using UMC.Configuration;
namespace UMC.Data
{
    /// <summary>
    /// 数据引用
    /// </summary>
    public class Reflection : UMC.Configuration.DataProvider
    {
        private static readonly Reflection reflection;
        static Reflection()
        {
            var path = AppDataPath(String.Format("WebADNuke\\assembly.config"));
            if (System.IO.File.Exists(path))
            {
                var rps = ProviderConfiguration.GetProvider(path);

                var providerName = "Reflection";
                if (rps == null || rps.Providers.Contains(providerName) == false)
                {
                    reflection = new Reflection();
                }
                else
                {
                    reflection = CreateObject(rps, providerName) as Reflection;
                }
            }
            else
            {
                reflection = new Reflection();
            }

        }
        public static Reflection Instance()
        {
            return reflection;

        }

        /// <summary>
        /// 是否为数据库空值，如果是就返回defaultValue
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object IsDBNull(object obj, object defaultValue)
        {
            if (obj == DBNull.Value)
            {
                return defaultValue;
            }
            else
            {
                return obj;
            }
        }


        public static UMC.Security.RuntimeModel RuntimeModel
        {
            get
            {
                return reflection.GetRuntimeModel();
            }
        }

        protected virtual UMC.Security.RuntimeModel GetRuntimeModel()
        {
            return Security.RuntimeModel.Single;
        }
        internal protected virtual Data.Provider GetProviderNode(string configKey, String Key)
        {
            var t = Configuration(configKey);

            if (t != null)
            {

                return t[Key];
            }
            else
            {

                Data.Utility.Debug("Provider", configKey, Key);
                return null;
            }

        }


        internal protected virtual Provider DatabaseProvider(string providerKey, Guid appKey)
        {
            return GetProviderNode("database", providerKey);
        }
        /// <summary>
        /// 获取平台配置
        /// </summary>
        /// <param name="configKey">配置名</param>
        /// <returns></returns>
        public static ProviderConfiguration Configuration(string configKey)
        {
            var path = AppDataPath(String.Format("WebADNuke\\{0}.config", configKey));
            if (System.IO.File.Exists(path))
            {
                return ProviderConfiguration.GetProvider(path);

            }
            else
            {
                path = AppDataPath(String.Format("WebADNuke\\{0}.xml", configKey));
                if (System.IO.File.Exists(path))
                {
                    return ProviderConfiguration.GetProvider(path);

                }
            }
            return null;
        }

        public static Provider GetDataProvider(string config, String key)
        {
            return reflection.GetProviderNode(config, key);
        }


        /// <summary>
        /// 获取配置路径
        /// </summary>
        /// <param name="vPath"></param>
        /// <returns></returns>
        public static string AppDataPath(string vPath)
        {
            return Data.Utility.MapPath(String.Format("~App_Data\\{0}", vPath));
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static object CreateInstance(Type type)
        {
            if (type != null)
            {
                return Activator.CreateInstance(type, true);
            }
            return null;
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static T CreateObject<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        /// <summary>
        /// 创建类型实例,把设置属性的默认值
        /// </summary>
        public static T CreateInstance<T>(params object[] args)
        {
            var otype = typeof(T);
            var obj = Activator.CreateInstance(otype, args);

            BindingFlags bindingAttr = BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] pros = otype.GetProperties(bindingAttr);
            for (int i = 0; i < pros.Length; i++)
            {

                var p = pros[i];

                var type = System.Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;

                var value = type.Equals(typeof(String)) ? String.Empty : CreateInstance(type);
                //object value = dic[pros[i].Name];
                SetProperty(obj, pros[i], value);
            }
            return (T)obj;
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <param name="TypeName">类型名</param>
        /// <param name="args">初始化参数</param>
        /// <returns></returns>
        public static object CreateInstance(string TypeName, params object[] args)
        {
            return Activator.CreateInstance(CreateType(TypeName, true), args);
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <param name="providerName">Provider配置节点名</param>
        /// <param name="providerConfig">Provider配置节</param>
        /// <param name="args">初始化参数</param>
        /// <returns></returns>
        public static object CreateObject(ProviderConfiguration providerConfig, string providerName, params object[] args)
        {

            Provider provider = (Provider)providerConfig.Providers[providerName];
            if (provider == null)
            {
                throw new ArgumentException(String.Format("{0} 没有找到{1}配置", providerConfig.ProviderType, providerName), "providerName");
            }
            object obj = null;
            obj = CreateObject(provider, args);
            return obj;
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <param name="provider">Provider配置节点</param>
        /// <param name="args">初始化参数</param>
        /// <returns></returns>
        public static object CreateObject(UMC.Data.Provider provider, params object[] args)
        {
            if (String.IsNullOrEmpty(provider.Type))
            {
                throw new ArgumentException("UMC.Data.Provider在创建类型实例事type不能为空");
            }
            object obj = null;
            obj = CreateInstance(provider.Type, args);
            if (obj != null)
            {
                UMC.Data.Reflection.SetProperty(obj, "Provider", provider);
            }
            return obj;
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <param name="provider">Provider配置节点</param>
        /// <param name="args">初始化参数</param>
        /// <returns></returns>
        public static object CreateObject(Type type, UMC.Data.Provider provider, params object[] args)
        {
            var obj = Activator.CreateInstance(type, args);
            if (obj != null)
            {
                UMC.Data.Reflection.SetProperty(obj, "Provider", provider);
            }
            return obj;
        }


        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <param name="providerName">配置节点名</param>
        /// <param name="args">初始化参数</param>
        /// <returns></returns>
        public static object CreateObject(string providerName, params object[] args)
        {
            var pc = Configuration("assembly");
            if (pc == null)
            {
                return null;
            }
            else if (pc.Providers.ContainsKey(providerName))
            {
                return CreateObject(pc[providerName], args);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="TypeName">类型名</param>
        /// <returns></returns>
        public static Type CreateType(string TypeName)
        {
            return CreateType(TypeName, false);
        }
        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="TypeName">类型名</param>
        /// <param name="IgnoreErrors"></param>
        /// <returns></returns>
        public static Type CreateType(string TypeName, bool IgnoreErrors)
        {

            var als = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in als)//mscorlib, 
            {
                var type2 = a.GetType(TypeName);
                if (type2 != null)
                {
                    return type2;
                }
            }
            return System.Type.GetType(TypeName, true, IgnoreErrors);
        }

        /// <summary>
        /// 得到指定的属性值
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="Target"></param>
        /// <returns></returns>
        public static object GetProperty(Type Type, string PropertyName, object Target)
        {
            if (Type != null)
            {
                return Type.InvokeMember(PropertyName, BindingFlags.GetProperty, null, (Target), null);
            }
            return null;
        }
        /// <summary>
        /// 反射调用指定的方法
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="methodName">方法名</param>
        /// <param name="Target">类型实例</param>
        /// <param name="Args">方法的参数</param>
        public static object InvokeMethod(Type Type, string methodName, object Target, params object[] Args)
        {
            try
            {
                return Type.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Instance, null, (Target), Args);
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

        }
        /// <summary>
        /// 反射调用指定的方法
        /// </summary>
        /// <param name="Target">类型实例</param>
        /// <param name="methodName">方法名</param>
        /// <param name="Args">方法的参数</param>
        public static object InvokeMethod(object Target, string methodName, params object[] Args)
        {
            return Target.GetType().InvokeMember(methodName, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Instance, null, (Target), Args);
        }
        /// <summary>
        /// 把对象属性转化为字典
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static IDictionary PropertyToDictionary(object obj)
        {
            Hashtable hash = new Hashtable();
            return PropertyToDictionary(obj, hash);

        }
        /// <summary>
        /// 把对象属性转化为字典
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static IDictionary PropertyToDictionary(object obj, IDictionary dic)
        {
            if (obj == null)
            {
                return dic;
            }
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            BindingFlags bindingAttr = BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] pros = obj.GetType().GetProperties(bindingAttr);
            for (int i = 0; i < pros.Length; i++)
            {
                if (pros[i].GetIndexParameters().Length == 0)
                {
                    var va = pros[i].GetValue(obj, null); ;
                    if (va != null)
                    {
                        dic[pros[i].Name] = va;
                    }
                }
            }
            return dic;

        }

        /// <summary>
        /// 通过健/值，来取配设置对应属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nv"></param>
        public static void SetProperty(object obj, System.Collections.Specialized.NameValueCollection nv)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (nv == null)
            {
                throw new ArgumentNullException("nv");
            }
            Hashtable table = new Hashtable();
            for (int i = 0; i < nv.Count; i++)
            {
                if (!String.IsNullOrEmpty(nv.GetKey(i)))
                    table[nv.GetKey(i)] = nv[i];
            }
            SetProperty(obj, table);
        }
        /// <summary>
        /// 根据字典对对象的属性赋值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="dic">字典</param>
        public static void SetProperty(object obj, IDictionary dic)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            BindingFlags bindingAttr = BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] pros = obj.GetType().GetProperties(bindingAttr);
            for (int i = 0; i < pros.Length; i++)
            {
                object value = dic[pros[i].Name];
                SetProperty(obj, pros[i], value);
            }

        }

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="node">节点</param>
        /// <param name="typeName">是否创建类型名父节点</param>
        /// <returns></returns>
        public static System.Xml.XmlNode Serialize(object obj, System.Xml.XmlNode node, bool typeName)
        {
            var type = obj.GetType();

            var fnode = node;
            if (typeName)
            {
                fnode = node.OwnerDocument.CreateElement(type.Name);
                node.AppendChild(fnode);
            }
            var pros = type.GetProperties();//(xtertor.Current.Name, bindingAttr);
            foreach (var pro in pros)
            {
                if (pro.GetIndexParameters().Length == 0)
                {
                    var value = pro.GetValue(obj, null);
                    if (value != null)
                    {
                        var cnode = node.OwnerDocument.CreateElement(pro.Name);
                        fnode.AppendChild(cnode);
                        var s = value.ToString();
                        cnode.AppendChild(node.OwnerDocument.CreateCDataSection(s));
                    }
                }
            }
            return fnode;
        }
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="node">节点</param>
        public static System.Xml.XmlNode Serialize(object obj, System.Xml.XmlNode node)
        {
            return Serialize(obj, node, true);
        }
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="node">节点</param>
        public static System.Xml.XmlNode Serialize(Array array, System.Xml.XmlNode node)
        {
            if (array.Length > 0)
            {
                var type = array.GetValue(0).GetType();
                var fnode = node.OwnerDocument.CreateElement(type.Name);
                node.AppendChild(fnode);
                foreach (object obj in array)
                {
                    Serialize(obj, fnode);
                }
                return fnode;
            }
            return node;
        }
        /// <summary>
        /// 根据XPathNavigator文档子结点来取配的属性，并赋值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="xPath">XPathNavigator</param>
        /// <returns></returns>
        public static void SetProperty(object obj, System.Xml.XPath.XPathNavigator xPath)
        {
            System.Xml.XPath.XPathNodeIterator xtertor = xPath.SelectChildren(System.Xml.XPath.XPathNodeType.Element);
            while (xtertor.MoveNext())
            {

                if (!String.IsNullOrEmpty(xtertor.Current.Value))
                {
                    BindingFlags bindingAttr = BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

                    PropertyInfo pro = obj.GetType().GetProperty(xtertor.Current.Name, bindingAttr);
                    if (pro != null && pro.CanWrite)
                    {
                        pro.SetValue(obj, Parse(xtertor.Current.Value, pro.PropertyType), null);
                    }
                }

            }

        }
        /// <summary>
        /// 向非public属性赋值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名 </param>
        /// <param name="propertyValue">属性值 </param>
        /// <returns></returns>
        public static void SetProperty(object obj, string propertyName, object propertyValue)
        {
            if (!String.IsNullOrEmpty(propertyName))
            {
                BindingFlags bindingAttr = BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.NonPublic;

                PropertyInfo pro = obj.GetType().GetProperty(propertyName, bindingAttr);
                if (pro != null && pro.CanWrite)
                {
                    SetProperty(obj, pro, propertyValue);
                }
            }
        }

        public static void SetProperty(object obj, PropertyInfo prototype, object propertyValue)
        {
            try
            {
                if (prototype.CanWrite == false || propertyValue == null)
                {
                    return;
                }
                if (prototype.PropertyType.IsInstanceOfType(propertyValue))
                {
                    prototype.SetValue(obj, propertyValue, null);
                }
                else
                {
                    if (propertyValue == DBNull.Value)
                    {
                    }
                    else
                    {
                        string str = propertyValue.ToString();
                        if (!String.IsNullOrEmpty(str))
                        {
                            prototype.SetValue(obj, Reflection.Parse(str, prototype.PropertyType), null);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 从字典流格式中，求的字典的集合
        /// </summary>
        /// <param name="reader">满足ini文件格式的流</param>
        /// <returns></returns>
        public static System.Collections.Specialized.NameValueCollection[] GetNameValues(System.IO.TextReader reader)
        {
            var list = new System.Collections.Generic.List<System.Collections.Specialized.NameValueCollection>();
            var hash = new System.Collections.Specialized.NameValueCollection();
            while (true)
            {
                string str = reader.ReadLine();
                if (str == null)
                {
                    break; ;
                }
                else
                {
                    str = str.Trim();
                    if (string.IsNullOrEmpty(str))
                    {
                        if (hash.Count > 0)
                        {
                            list.Add(hash);
                            hash = new System.Collections.Specialized.NameValueCollection();
                        }
                    }
                    else
                    {
                        int i = str.IndexOf(':');
                        if (i > -1)
                        {
                            hash[str.Substring(0, i)] = str.Substring(i + 1).Trim();

                        }
                        else
                        {
                            hash["content"] = str;
                        }
                    }
                }

            }
            if (hash.Count > 0)
            {
                list.Add(hash);
            }
            return list.ToArray();
        }
        /// <summary>
        /// 把字符串转化与defaultValue类型相同对应的类型实例
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static object Parse(string str, object defaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            else
            {
                return Parse(str, defaultValue.GetType());
            }
        }


        /// <summary>
        /// 根据字符串格式，转化对应的类型,支持bool、Guid,DateTime,其它类型返回字符串
        /// </summary>
        /// <param name="str">带格式字符串</param>
        /// <returns></returns>
        public static object Parse(String str)
        {
            try
            {
                if (str == "true" || str == "True")
                {
                    return true;
                }
                else if (str == "false" || str == "False")
                {
                    return false;
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^\d+$"))
                {
                    return Convert.ToInt32(str);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^\d+\.\d+$"))
                {
                    return Convert.ToDecimal(str);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^\w{8}-(\w{4}-){3}\w{12}$"))
                {
                    return new Guid(str);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^\d{4}-\d{1,2}-\d{1,2}"))
                {
                    return Convert.ToDateTime(str);
                }
            }
            catch
            {
                return str;
            }

            return str;
        }
        static readonly DateTime UtcStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static int TimeSpan(DateTime time)
        {
            int intResult = 0;

            intResult = (int)(time.ToUniversalTime() - UtcStart).TotalSeconds;
            return intResult;
        }
        public static DateTime TimeSpan(int time)
        {

            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(time).ToLocalTime();// TimeZone.CurrentTimeZone.ToLocalTime(UtcStart);

        }
        /// <summary>
        /// 把字符串转化对应的类型实例，支持基元类型、GUID、DateTime和Enum
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static object Parse(string str, Type type)
        {
            if (String.IsNullOrEmpty(str))
            {
                if (typeof(string).Equals(type))
                {
                    return str;
                }
                return null;
            }
            var f = Nullable.GetUnderlyingType(type);
            if (f != null)
            {
                type = f;
            }
            switch (type.FullName)
            {
                case "System.SByte":
                    return Convert.ToSByte(str);
                case "System.Byte":
                    return Convert.ToByte(str);
                case "System.Int16":
                    return Convert.ToInt16(str);
                case "System.UInt16":
                    return Convert.ToUInt16(str);
                case "System.Int32":
                    return Convert.ToInt32(str);
                case "System.UInt32":
                    return Convert.ToUInt32(str);
                case "System.Int64":
                    return Convert.ToInt64(str);
                case "System.UInt64":
                    return Convert.ToUInt64(str);
                case "System.Char":
                    return Convert.ToChar(str);
                case "System.Single":
                    return Convert.ToSingle(str);
                case "System.Double":
                    return Convert.ToDouble(str);
                case "System.Decimal":
                    return Convert.ToDecimal(str);
                case "System.Boolean":
                    return Convert.ToBoolean(str);
                case "System.Guid":
                    if (String.IsNullOrEmpty(str))
                    {
                        if (f != null)
                        {
                            return null;
                        }
                        return Guid.Empty;
                    }
                    else if (str.Length == 22)
                    {
                        return UMC.Data.Utility.Guid(str).Value;
                    }
                    else
                    {
                        return new Guid(str);
                    }
                case "System.DateTime":
                    if (String.IsNullOrEmpty(str))
                    {
                        if (f != null)
                        {
                            return null;
                        }
                        return DateTime.MinValue;
                    }
                    try
                    {
                        return Convert.ToDateTime(str);
                    }
                    catch
                    {
                        int i = 0;
                        if (Int32.TryParse(str, out i))
                        {
                            return TimeSpan(i);
                        }
                        else if (f != null)
                        {
                            return null;
                        }
                        return DateTime.MinValue;
                    }
                case "System.String":
                    return str;
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, str, true);
            }
            return null;

        }
    }
}


