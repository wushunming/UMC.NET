using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Entities
{

    public enum Visibility
    {
        Hidden = 0,
        Collapse = 1,
        Visible = 2,
        Draft = 4
    }

    /// <summary>
    /// 类别
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id
        {
            get;
            set;
        }
        /// <summary>
        /// 样式
        /// </summary>
        public string CssClass
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int? Sequence
        {
            get;
            set;
        }
        /// <summary>
        /// 可见标签
        /// </summary>
        public Visibility? Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 版主
        /// </summary>
        public Guid? user_id
        {
            get;
            set;
        }
        public Guid? AppId
        {
            get;
            set;
        }
        public int? Count
        {
            get;
            set;
        }
        public int? Attentions
        {
            get;
            set;
        }
    }
    //public class CategoryUser
    //{
    //    public Guid? category_id
    //    {
    //        get; set;
    //    }
    //    public Guid? user_id
    //    {
    //        get; set;
    //    }
    //}
}
