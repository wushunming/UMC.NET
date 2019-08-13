using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using UMC.Data.Sql;
using UMC.Security;
namespace UMC.Data
{
    /// <summary>
    ///ʵ�����ݿ�����ṩ��
    /// </summary>
    public sealed class Database : IDisposable
    {
        [ThreadStaticAttribute]
        static System.Collections.Hashtable _items;
        bool IsTran = false;

        /// <summary>
        /// ���ݿ����������
        /// </summary>
        public DbCommonProvider DbProvider
        {
            get
            {
                return DbCommonFactory.Provider;
            }
        }

        DbCommonFactory DbCommonFactory;
        System.Data.Common.DbTransaction tran;
        /// <summary>
        /// ����ǲ��������ʼ�������ύ����
        /// </summary>
        public void Commit()
        {
            if (IsTran)
            {
                if (this.DbCommonFactory.TranCmd.Transaction != null)
                {
                    this.tran.Commit();
                }
                this.DbCommonFactory.TranCmd.Connection.Close();
                this.tran.Dispose();
                this.IsTran = false;
                if (this.DbCommonFactory.Provider.Provider != null)
                {
                    if (_items != null)
                    {
                        _items.Remove(this.DbCommonFactory.Provider.Provider.Name);
                    }
                }
            }
        }
        /// <summary>
        /// �ύ�������threedAllΪ�棬���ύ�߳������е�����
        /// </summary>
        /// <param name="threedAll"></param>
        public void Commit(bool threedAll)
        {
            if (threedAll)
            {
                if (_items != null)
                {
                    var d = new Database[_items.Count];
                    _items.Values.CopyTo(d, 0);
                    var m = d.GetEnumerator();
                    while (m.MoveNext())
                    {
                        var db = m.Current as Database;
                        db.Commit();
                    }
                }

            }
            else
            {
                this.Commit();
            }
        }

        /// <summary>
        /// �����ݿ����
        /// </summary>
        public void Open()
        {
            this.DbCommonFactory.Open();
        }
        /// <summary>
        /// �ر����ݿ����
        /// </summary>
        public void Close()
        {
            this.DbCommonFactory.Close();
        }
        /// <summary>
        /// �����������threedAllΪ�棬������߳������е�����
        /// </summary>
        /// <param name="threedAll"></param> 
        public void Rollback(bool threedAll)
        {
            if (threedAll)
            {
                if (_items != null)
                {
                    var d = new Database[_items.Count];
                    _items.Values.CopyTo(d, 0);
                    var m = d.GetEnumerator();
                    while (m.MoveNext())
                    {
                        var db = m.Current as Database;
                        db.Rollback();
                    }
                }

            }
            else
            {
                this.Rollback();
            }
        }
        /// <summary>
        /// ����ǲ��������ʼ�������������
        /// </summary>
        public void Rollback()
        {
            if (IsTran)
            {
                if (this.DbCommonFactory.TranCmd.Transaction != null)
                {
                    this.tran.Rollback();
                }
                this.DbCommonFactory.TranCmd.Connection.Close();
                this.tran.Dispose();

                this.IsTran = false;
                if (_items != null)
                {
                    _items.Remove(this.DbCommonFactory.Provider.Provider.Name);
                }
            }
        }
        private Database(DbCommonFactory DbFactory)
        {
            this.DbCommonFactory = DbFactory;
        }
        /// <summary> 
        /// ����Ĭ��DbProviderʵ���ʵ��,Ĭ�����ýڵ���defaultDbProvider
        /// </summary>
        /// <returns></returns>
        public static Database Instance()
        {
            return Instance("defaultDbProvider"); ;
        }
        /// <summary>
        /// ����DataBase
        /// </summary>
        /// <param name="Factor"></param>
        /// <returns></returns>
        public static Database Instance(DbCommonFactory Factor)
        {
            return new Database(Factor);
        }
        public Database For(Guid appKey)
        {
            if (this.DbCommonFactory.Provider != null && this.DbCommonFactory.Provider.Provider != null)
            {
                var provider = this.DbCommonFactory.Provider.Provider;
                var provider2 = Reflection.CreateObject(Reflection.Instance().DatabaseProvider(this.DbCommonFactory.Provider.Provider.Name, appKey)) as DbCommonProvider;
                return Instance(new DbCommonFactory(provider2));
            }
            return this;
        }
        /// <summary>
        /// ����Ĭ�����ݿ�ʵ���ʵ��
        /// </summary>
        /// <param name="providerName">���ýڵ���</param>
        /// <returns></returns>
        public static Database Instance(string providerName)
        {
            if (_items != null)
            {
                if (_items.ContainsKey(providerName))
                {
                    return _items[providerName] as Database;
                }
            }
            var pKey = UMC.Security.Principal.Current.AppKey ?? Guid.Empty;
            var provider = Reflection.CreateObject(Reflection.Instance().DatabaseProvider(providerName, pKey)) as DbCommonProvider;
            return new Database(new DbCommonFactory(provider));
        }


        /// <summary>
        /// ʹ����������Ѿ�ʹ���������򷵻�false�����û��ʹ��������ȡ�����񲢷���true
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            return this.BeginTransaction(System.Data.IsolationLevel.Unspecified);
        }
        /// <summary>
        /// ʹ����������Ѿ�ʹ���������򷵻�false�����û��ʹ��������ȡ�����񲢷���true
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction(System.Data.IsolationLevel lev)
        {
            if (IsTran == false)
            {
                this.tran = this.DbCommonFactory.UseTran(lev);
                this.IsTran = true;

                if (this.DbCommonFactory.Provider.Provider != null)
                {
                    if (String.IsNullOrEmpty(this.DbCommonFactory.Provider.Provider.Name) == false)
                    {
                        if (_items == null)
                        {
                            _items = new System.Collections.Hashtable();
                        }
                        _items[this.DbCommonFactory.Provider.Provider.Name] = this;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// ������ѯ�����Ͳ�ѯ���������������ͣ���Ԫ���ͣ�DateTime��Guid��
        /// </summary>
        //public IUpdater Update()
        //{
        //    return DbCommonFactory.Update();
        //}
        /// <summary>
        /// ����ɾ����������ͷ��������ǵ��������ͣ���Ԫ���ͣ�DateTime��Guid��
        /// </summary>
        /// <returns></returns>
        public IInserter Insert()
        {
            return DbCommonFactory.Insert();
        }
        /// <summary>
        /// ����ɾ��Sql������
        /// </summary>
        /// <returns></returns>
        //public IDeleter Delete()
        //{
        //    return DbCommonFactory.Delete();
        //}
        /// <summary>
        /// ������Sql���Ĳ�ѯ��
        /// </summary>
        /// <returns></returns>
        public ISqler Sqler()
        {
            return DbCommonFactory.Sqler();
        }
        /// <summary>
        /// ������Sql���Ĳ�ѯ��
        /// </summary>
        /// <param name="TimeOut">��ʱʱ��</param>
        /// <returns></returns>
        public ISqler Sqler(int TimeOut)
        {
            return DbCommonFactory.Sqler(TimeOut);
        }
        /// <summary>
        /// ������Sql���Ĳ�ѯ��
        /// </summary>
        /// <param name="pfx">�Ƿ��Զ���ӱ�ǰ׺</param>
        /// <returns></returns>
        public ISqler Sqler(bool pfx)
        {
            return DbCommonFactory.Sqler(pfx);
        }

        /// <summary>
        /// ����ʵ���ۺϹ���������
        /// </summary>
        /// <returns></returns>
        public IObjectEntity<T> ObjectEntity<T>() where T : class
        {
            return DbCommonFactory.ObjectEntity<T>();// (TimeOut);
        }

        /// <summary>
        /// ����ʵ���ۺϹ���������
        /// </summary>
        /// <returns></returns>
        public IObjectEntity<T> ObjectEntity<T>(string tabName, T anonymousType) where T : class
        {
            return DbCommonFactory.ObjectEntity<T>(tabName);
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            ((IDisposable)DbCommonFactory).Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}







