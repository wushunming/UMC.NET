using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace UMC.Data.Sql
{
    /// <summary>
    /// ����CommandRun�ĸ�������
    /// </summary>
    /// <param name="cmdRun">����Command�Ĵ���</param>
    /// <returns></returns>
    delegate object CommandProgress(CommandRun cmdRun);
    /// <summary>
    /// ����Command�Ĵ���
    /// </summary>
    /// <param name="cmd">��ʾҪ������Դִ�е� SQL ����洢���̡�Ϊ��ʾ����ġ����ݿ����е����ṩһ������</param>
    /// <returns></returns>
    delegate object CommandRun(DbCommand cmd);

    /// <summary>
    /// �������ݴ���
    /// </summary>
    /// <param name="dr"></param>
    public delegate void DataRecord(System.Data.IDataRecord dr);
    /// <summary>
    /// �������ݴ���
    /// </summary>
    /// <param name="dr"></param>
    public delegate void DataReader(System.Data.IDataReader dr);
    /// <summary>
    /// �������ݴ���
    /// </summary>
    /// <typeparam name="T">ʵ����</typeparam>
    /// <param name="item"></param>
    public delegate void DataReader<T>(T item);
    /// <summary>
    /// ʵ��ܹ����ݲ���ʲ㹤����֧�����������������
    /// </summary>
    public class DbCommonFactory : IDisposable
    {

        /// <summary>
        /// ���ݿ���������ṩ��
        /// </summary>
        public DbCommonProvider Provider
        {
            get;
            private set;
        }
        int TimeOut = 30000;
        //string conString;
        /// <summary>
        /// �����������ַ�����ʼ�����ʹ���
        /// </summary>
        /// <param name="dbProvider">���ʹ�����</param>
        public DbCommonFactory(DbCommonProvider dbProvider)
        {
            this.Provider = dbProvider;
            //this.conString = conString;
            this.Progress = this.ConnectionProgress;
        }
        public DbCommonFactory(DbCommonProvider dbProvider, string con) : this(dbProvider) { }
        CommandProgress Progress;
        internal DbCommand TranCmd
        {
            get;
            set;
        }

        object TrasactionProgress(CommandRun cmdAcs)
        {
            this.TranCmd.CommandTimeout = TimeOut;
            this.TranCmd.Parameters.Clear();
            this.TranCmd.CommandText = String.Empty;
            switch (this.TranCmd.Connection.State)
            {
                case ConnectionState.Closed:
                    return ConnectionProgress(cmdAcs);
                default:

                    try
                    {
                        return cmdAcs(this.TranCmd);
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        throw new DbException(ex, this.TranCmd);
                    }
            }


        }
        object ConnectionProgress(CommandRun cmdAcs)
        {

            using (DbConnection con = Provider.DbFactory.CreateConnection())
            {
                con.ConnectionString = this.Provider.ConntionString;
                DbCommand cmd = con.CreateCommand();
                cmd.CommandTimeout = TimeOut;
                cmd.Connection.Open();
                try
                {
                    return cmdAcs(cmd);
                }
                catch (System.Data.Common.DbException ex)
                {
                    throw new DbException(ex, cmd);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        DbConnection conOpen;
        /// <summary>
        /// ����������
        /// </summary>
        public void Open()
        {
            if (conOpen == null)
            {
                conOpen = Provider.DbFactory.CreateConnection();
                conOpen.ConnectionString = Provider.ConntionString;
            }
            conOpen.Open();
            this.Progress = CommandProgress;
        }
        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        public void Close()
        {

            if (conOpen != null)
            {
                conOpen.Close();
            }
        }
        object CommandProgress(CommandRun cmdAcs)
        {

            DbCommand cmd = conOpen.CreateCommand();
            cmd.CommandTimeout = TimeOut;
            try
            {
                return cmdAcs(cmd);
            }
            catch (System.Exception ex)
            {
                throw new DbException(ex, cmd);
            }
        }

        /// <summary>
        /// ����ʵ���ۺϹ���������
        /// </summary>
        /// <returns></returns>
        public IObjectEntity<T> ObjectEntity<T>() where T : class
        {
            ObjectEntity<T> selecter = new ObjectEntity<T>(new Sqler(Provider, Progress, true), String.Empty);
            return selecter;
        }
        /// <summary>
        /// ����ʵ���ۺϹ���������
        /// </summary>
        /// <returns></returns>
        public IObjectEntity<T> ObjectEntity<T>(string tabName) where T : class
        {
            ObjectEntity<T> selecter = new ObjectEntity<T>(new Sqler(Provider, Progress), tabName);
            return selecter;
        }
        /// <summary>
        /// ʹ������
        /// </summary>
        internal DbTransaction UseTran(IsolationLevel isolationLevel)
        {
            DbConnection con = Provider.DbFactory.CreateConnection();//
            con.ConnectionString = this.Provider.ConntionString;
            con.Open();
            DbTransaction tran = con.BeginTransaction(isolationLevel);
            this.Progress = this.TrasactionProgress;
            this.TranCmd = tran.Connection.CreateCommand();
            this.TranCmd.Transaction = tran;
            return tran;
        }

        /// <summary>
        /// ����ʵ�����������
        /// </summary>
        public IInserter Insert()
        {
            Inserter inserter = new Inserter(new Sqler(Provider, Progress));
            return inserter;
        }
        /// <summary>
        /// ����sql������
        /// </summary>
        /// <returns></returns>
        public ISqler Sqler()
        {
            Sqler sqler = new Sqler(Provider, Progress);
            return sqler;
        }
        /// <summary>
        /// ����sql������
        /// </summary>
        /// <returns></returns>
        public ISqler Sqler(bool pfx)
        {
            Sqler sqler = new Sqler(Provider, Progress, pfx);
            return sqler;
        }
        /// <summary>
        /// ����sql������
        /// </summary>
        /// <param name="timeout">��ʱʱ��</param>
        /// <returns></returns>
        public ISqler Sqler(int timeout)
        {
            return this.Sqler(timeout, true);
        }
        /// <summary>
        /// ����sql������
        /// </summary>
        /// <param name="timeout">��ʱʱ��</param>
        /// <param name="pfx">ǰ׺</param>
        /// <returns></returns>
        public ISqler Sqler(int timeout, bool pfx)
        {
            this.TimeOut = timeout;
            Sqler sqler = new Sqler(Provider, Progress, pfx);
            return sqler;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (TranCmd != null)
            {
                TranCmd.Dispose();
            }
            if (conOpen != null)
            {
                conOpen.Dispose();
            }
        }

        #endregion
    }
}
