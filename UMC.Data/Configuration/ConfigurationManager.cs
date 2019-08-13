using System;
using System.Collections.Generic;
using System.Text;
using UMC.Data.Sql;
using UMC.Data.Entities;

namespace UMC.Configuration
{
    /// <summary>
    /// ���ݻ������
    /// </summary>
    /// <param name="key"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public delegate System.Collections.Hashtable DataCacheCallback(Guid key, string name);
    /// <summary>
    /// ���ù���
    /// </summary>
    public sealed class ConfigurationManager
    {


        /// <summary>
        /// ��ȡ�µ���ˮ���
        /// </summary>
        /// <param name="codeKey"></param>
        public static string SerialNumber(string codeKey)
        {
            return SerialNumber(codeKey, string.Empty, "Y");
        }
        /// <summary>
        /// ��ȡ�µ���ˮ���
        /// </summary>
        /// <param name="format">������ʽ</param>
        public static string SerialNumber(string codeKey, string format)
        {
            return SerialNumber(codeKey, format, "Y");
        }
        /// <summary>
        /// ��ȡ�µ���ˮ���
        /// </summary>
        /// <param name="codeKey"></param>
        /// <param name="format">������ʽ</param>
        /// <param name="type">ֻ��Ϊ��Y������M������D��</param>
        public static string SerialNumber(string codeKey, string format, string type)
        {
            var codeEntity = UMC.Data.Database.Instance().ObjectEntity<Number>();
            codeEntity.Where.And(new Number { CodeKey = codeKey });
            var code = codeEntity.Single();
            var CurValue = 1;

            if (code == null)//.Count == 0)
            {
                if (String.IsNullOrEmpty(format))
                {
                    format = codeKey[0].ToString().ToUpper() + "{0:yy}-{1}";
                }
                try
                {
                    codeEntity.Insert(new Number
                    {
                        CodeKey = codeKey,
                        Type = type ?? "Y",
                        Value = CurValue,
                        Format = format,
                        UpdateDate = DateTime.Now
                    });
                }
                catch (DbException)
                {
                }
            }
            else
            {
                if (String.IsNullOrEmpty(format))
                {
                    format = code.Format;
                }
                CurValue = code.Value.Value;
                var UpdateDate = code.UpdateDate.Value;
                var Now = DateTime.Now;
                var IncType = type ?? code.Type;
                switch (IncType)
                {
                    case "Y":
                        if (UpdateDate.Year == Now.Year)
                        {
                            CurValue = CurValue + 1;
                        }
                        else
                        {
                            CurValue = 1;
                        }
                        break;
                    case "M":
                        if (UpdateDate.Year == Now.Year && UpdateDate.Month == Now.Month)
                        {
                            CurValue = CurValue + 1;
                        }
                        else
                        {
                            CurValue = 1;
                        }
                        break;
                    case "D":
                        if (UpdateDate.Year == Now.Year && UpdateDate.Month == Now.Month && UpdateDate.Day == Now.Day)
                        {
                            CurValue = CurValue + 1;
                        }
                        else
                        {
                            CurValue = 1;
                        }
                        break;
                    case "A":
                        CurValue = CurValue + 1;
                        break;
                    default:
                        goto case "Y";
                }
                codeEntity.Update(new Number
                {
                    Value = CurValue,
                    Type = type,
                    UpdateDate = DateTime.Now
                });
            }
            return String.Format(format, DateTime.Now, CurValue);
        }

        public static Cache DataCache(Guid key, string name, int timeout, DataCacheCallback callback)
        {
            var sqer = UMC.Data.Database.Instance().ObjectEntity<Cache>();
            var cache = sqer.Where.And().Equal(new Cache { CacheKey = name, Id = key }).Entities.Single();
            if (cache != null)
            {
                if (cache.ExpiredTime.Value < DateTime.Now)
                {
                    cache = new Cache()
                    {
                        Id = key,
                        CacheKey = name,
                        BuildDate = DateTime.Now,
                        ExpiredTime = DateTime.Now.AddSeconds(timeout),
                        CacheData = callback(key, name)
                    };
                    sqer.Update(cache);
                }
            }
            else
            {
                cache = new Cache()
                {
                    Id = key,
                    CacheKey = name,
                    BuildDate = DateTime.Now,
                    ExpiredTime = DateTime.Now.AddSeconds(timeout),
                    CacheData = callback(key, name)
                };
                sqer.Insert(cache);

            }
            return cache;

        }


        /// <summary>
        /// ������ݻ���
        /// </summary>
        /// <param name="cacheId"></param>
        /// <param name="CacheKey"></param>
        public static void ClearCache(Guid cacheId, string CacheKey)
        {
            var sqer = UMC.Data.Database.Instance().ObjectEntity<Cache>();
            sqer.Where.And().Equal(new Cache { CacheKey = CacheKey, Id = cacheId }).Entities.Delete();
        }
        public static System.Collections.Hashtable ParseDictionary(string format)
        {
            var strs = format.Split(',');
            var attr = new System.Collections.Hashtable();
            var c = 0;
            if (strs[0].IndexOf(':') == -1)
            {
                attr["text"] = strs[0];
                c = 1;
            }
            for (; c < strs.Length; c++)
            {
                var str = strs[c];
                var p = str.IndexOf(':');
                if (p > 0)
                {
                    var va = str.Substring(p + 1).Trim();
                    var na = str.Substring(0, p).Trim();
                    var value = (va.StartsWith("(") && va.EndsWith(")")) ? UMC.Data.JSON.Expression(va) : UMC.Data.Reflection.Parse(va);


                    var code = Convert.ToInt32(na[0]);
                    if ((code > 64 && code < 91) || (code > 96 && code < 123) || (code > 47 && code < 58))
                    {
                        attr[na] = value;
                    }
                    else
                    {
                        var cattr = attr[na[0]] as System.Collections.Hashtable;
                        if (cattr == null)
                        {
                            attr[na[0]] = cattr = new System.Collections.Hashtable();
                        }
                        cattr[na.Substring(1)] = value;
                    }
                }
                else
                {
                    attr[str] = true;
                }
            }
            return attr;
        }

    }

}
