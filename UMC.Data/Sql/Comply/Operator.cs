using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace UMC.Data.Sql
{
    class Operator<T> : IOperator<T> where T : class
    {
        IWhere<T> constr;
        Operator opor;
        public Operator(Operator c, IWhere<T> cons)
        {
            this.opor = c;
            this.constr = cons;
        }
        #region IOperator<T> Members

        IWhere<T> IOperator<T>.Unequal(string field, object value)
        {
            this.opor.Unequal(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.Equal(string field, object value)
        {
            this.opor.Equal(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.Greater(string field, object value)
        {
            this.opor.Greater(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.Smaller(string field, object value)
        {
            this.opor.Smaller(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.GreaterEqual(string field, object value)
        {
            this.opor.GreaterEqual(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.SmallerEqual(string field, object value)
        {
            this.opor.SmallerEqual(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.Like(string field, string value)
        {

            this.opor.Like(field, value);
            return constr;
        }

        IWhere<T> IOperator<T>.In(string field, params object[] values)
        {
            this.opor.In(field, values);
            return constr;
        }

        IWhere<T> IOperator<T>.NotIn(string field, params object[] values)
        {
            this.opor.NotIn(field, values);
            return constr;
        }

        #endregion

        #region IOperator<T> Members

        IWhere<T> IOperator<T>.Unequal(T value)
        {
            var dic = CBO.GetProperty(value);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                this.opor.Unequal(em.Current.Key, em.Current.Value);
            }
            return constr;
        }

        IWhere<T> IOperator<T>.Equal(T value)
        {
            var dic = CBO.GetProperty(value);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                this.opor.Equal(em.Current.Key, em.Current.Value);
            }
            return constr;
        }

        IWhere<T> IOperator<T>.Greater(T value)
        {
            var dic = CBO.GetProperty(value);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                this.opor.Greater(em.Current.Key, em.Current.Value);
            }
            return constr;
        }

        IWhere<T> IOperator<T>.Smaller(T value)
        {
            var dic = CBO.GetProperty(value);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                this.opor.Smaller(em.Current.Key, em.Current.Value);
            }
            return constr;
        }

        IWhere<T> IOperator<T>.GreaterEqual(T value)
        {
            var dic = CBO.GetProperty(value);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                this.opor.GreaterEqual(em.Current.Key, em.Current.Value);
            }
            return constr;
        }

        IWhere<T> IOperator<T>.SmallerEqual(T value)
        {
            var dic = CBO.GetProperty(value);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                this.opor.SmallerEqual(em.Current.Key, em.Current.Value);
            }
            return constr;
        }



        IWhere<T> IOperator<T>.In(string field, Script script)
        {
            this.opor.In(field, script);
            return constr;
        }

        #endregion

        #region IOperator<T> Members


        IWhere<T> IOperator<T>.In(T field, params object[] values)
        {
            var dic = CBO.GetProperty(field);
            if (dic.Count > 1)
            {
                throw new ArgumentException("ʵ��In��ֻ����һ���ֶ�", "field");
            }
            var em = dic.GetEnumerator();
            if (em.MoveNext())
            {
                var list = new List<object>();
                list.AddRange(values);
                list.Add(em.Current.Value);
                this.opor.In(em.Current.Key, list.ToArray());
            }
            return constr;
        }

        IWhere<T> IOperator<T>.NotIn(T field, params object[] values)
        {
            var dic = CBO.GetProperty(field);
            var em = dic.GetEnumerator();
            if (em.MoveNext())
            {
                var list = new List<object>();
                list.AddRange(values);
                list.Add(em.Current.Value);
                this.opor.NotIn(em.Current.Key, list.ToArray());
            }
            return constr;
        }

        IWhere<T> IOperator<T>.NotIn(string field, Script script)
        {
            this.opor.NotIn(field, script);
            return constr;
        }

        #endregion

        #region IOperator Members

        //IWhere IOperator.Unequal(string field, object value)
        //{
        //    return opor.Unequal(field, value);
        //}

        //IWhere IOperator.Equal(string field, object value)
        //{
        //    return opor.Equal(field, value);
        //}

        //IWhere IOperator.Greater(string field, object value)
        //{
        //    return opor.Greater(field, value);
        //}

        //IWhere IOperator.Smaller(string field, object value)
        //{
        //    return opor.Smaller(field, value);
        //}

        //IWhere IOperator.GreaterEqual(string field, object value)
        //{
        //    return opor.GreaterEqual(field, value);
        //}

        //IWhere IOperator.SmallerEqual(string field, object value)
        //{
        //    return opor.SmallerEqual(field, value);
        //}

        //IWhere IOperator.Like(string field, string value)
        //{
        //    return opor.Like(field, value);
        //}

        //IWhere IOperator.In(string field, params object[] values)
        //{
        //    return opor.In(field, values);
        //}

        //IWhere IOperator.NotIn(string field, params object[] values)
        //{

        //    return opor.NotIn(field, values);
        //}

        #endregion

        #region IOperator<T> Members


        //IWhere<T> IOperator<T>.Contains()
        //{
        //    var p = (Conditions)this.opor.Contains();

        //    return new Conditions<T>(p, this.constr.Entities);
        //    //return new Operator<T>(p, con);
        //}

        #endregion

        #region IOperator Members


        //IWhere IOperator.Contains()
        //{
        //    return opor.Contains();
        //}


        IWhere<T> IOperator<T>.NotLike(string field, string value)
        {
            this.opor.NotLike(field, value);
            return constr;
        }



        //IWhere IOperator.NotLike(string field, string value)
        //{
        //    return this.opor.NotLike(field, value);
        //}



        IWhere<T> IOperator<T>.Like(T field, bool schar)
        {

            var dic = CBO.GetProperty(field);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                string from = "{0}";
                if (schar)
                {
                    from = "{0}%";
                }
                this.opor.Like(em.Current.Key, String.Format(from, em.Current.Value));
            }
            return constr;
        }
        IWhere<T> IOperator<T>.Like(T field)
        {

            var dic = CBO.GetProperty(field);
            var em = dic.GetEnumerator();
            while (em.MoveNext())
            {
                string from = "%{0}%";
                this.opor.Like(em.Current.Key, String.Format(from, em.Current.Value));
            }
            return constr;
        }

        #endregion


        IWhere<T> IOperator<T>.Contains()
        {
            var t = (Conditions<T>)this.constr.Contains();
            var op = (Operator)this.opor;
            t.Wherer.FristJoin = op.IsOr ? UMC.Data.Sql.Conditions.JoinVocable.Or : UMC.Data.Sql.Conditions.JoinVocable.And;
            return t;
        }
    }
    class Operator
    {
        public bool IsOr;
        public Conditions condit;
        public Operator(Conditions q, bool IsOr)
        {
            this.condit = q;
            this.IsOr = IsOr;
        }
        #region IOperator Members

        public Conditions Unequal(string field, object value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.Unequal, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.Unequal, value);
            }
        }

        public Conditions Equal(string field, object value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.Equal, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.Equal, value);
            }
        }

        public Conditions Greater(string field, object value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.Greater, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.Greater, value);
            }
        }

        public Conditions Smaller(string field, object value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.Smaller, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.Smaller, value);
            }
        }

        public Conditions GreaterEqual(string field, object value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.GreaterEqual, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.GreaterEqual, value);
            }
        }

        public Conditions SmallerEqual(string field, object value)
        {
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.SmallerEqual, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.SmallerEqual, value);
            }
        }

        public Conditions Like(string field, string value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.Like, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.Like, value);
            }
        }

        public Conditions In(string field, params object[] values)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (values.Length == 0)
            {
                throw new ArgumentException("values�ĳ��Ȳ���Ϊ0");
            }

            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.In, values);
            }
            else
            {
                return this.condit.And(field, DbOperator.In, values);
            }
        }


        #endregion

        #region IOperator Members


        public Conditions NotIn(string field, params object[] values)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (values.Length == 0)
            {
                throw new ArgumentException("values�ĳ��Ȳ���Ϊ0");
            }

            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.NotIn, values);
            }
            else
            {
                return this.condit.And(field, DbOperator.NotIn, values);
            }
        }

        #endregion

        #region IOperator Members


        //IWhere IOperator.Contains()
        //{

        //    var query = new Conditions();
        //    query.GroubId = this.condit.GroubId;
        //    query.IsContainFrist = true;
        //    query.Arguments = this.condit.Arguments;
        //    return query;
        //    //return new Operator(query, this.IsOr);

        //}

        #endregion

        #region IOperator Members


        public Conditions NotLike(string field, string value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field");
            }
            if (IsOr)
            {
                return this.condit.Or(field, DbOperator.NotLike, value);
            }
            else
            {
                return this.condit.And(field, DbOperator.NotLike, value);
            }
        }

        #endregion
    }
}
