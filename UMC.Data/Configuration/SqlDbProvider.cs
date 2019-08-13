using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace UMC.Configuration.Sql
{
    public class SqlDbProvider : UMC.Data.Sql.DbCommonProvider
    {
        private static System.Data.Common.DbProviderFactory Instance;

        public override string Year(string feild)
        {
            return String.Format("DATEPART(yy,{0})", feild);
        }
        public override string Month(string feild)
        {
            return String.Format("DATEPART(mm,{0})", feild);
        }
        public override string Day(string feild)
        {
            return String.Format("DATEPART(dd,{0})", feild);
        }
        public override string Hour(string feild)
        {
            return String.Format("DATEPART(hh,{0})", feild);
        }
        public override string Week(string feild)
        {
            return String.Format("DATEPART(dw,{0})", feild);
        }
        public override string Minute(string feild)
        {
            return String.Format("DATEPART(mi,{0})", feild);
        }

        protected override string ParamsPrefix
        {
            get
            {
                return "@";
            }
        }
        public override string GetIdentityText(string tableName)
        {
            return "SELECT @@IDENTITY";
        }
        public override string QuotePrefix
        {
            get
            {
                return "[";
            }
        }
        public override string QuoteSuffix
        {
            get
            {
                return "]";
            }
        }
        public override string GetPaginationText(int start, int limit, string SelectText)
        {
            StringBuilder sb = new StringBuilder(SelectText);

            if (start > 0)
            {
                sb.Insert(SelectText.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6, String.Format(" TOP {0} ", start + limit));
                sb.Insert(0, "SELECT IDENTITY(INT,0,1) AS __WDK_Page_ID , WebADNukePagge.* INTO #__WebADNukePagges FROM(");

                sb.Append(") AS WebADNukePagge");
                sb.AppendLine();
                sb.AppendFormat("SELECT *FROM  #__WebADNukePagges  WHERE __WDK_Page_ID >={0}", start);

                sb.AppendLine();
                sb.Append("DROP TABLE #__WebADNukePagges");

            }
            else
            {
                sb.Insert(SelectText.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6, String.Format(" TOP {0}", limit));
            }
            return sb.ToString();


        }
        public override string ConntionString
        {
            get
            {
                return this.Provider.Attributes["conString"];
            }
        }


        public override System.Data.Common.DbProviderFactory DbFactory
        {
            get
            {

                if (Instance == null)
                {
                    var als = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var a in als)//mscorlib, 
                    {
                        var type = a.GetType("System.Data.SqlClient.SqlClientFactory");
                        if (type != null)
                        {
                            Instance = type.GetField("Instance").GetValue(null) as System.Data.Common.DbProviderFactory;

                            break;
                        }
                    }

                }
                return Instance;
            }
        }
    }
}
