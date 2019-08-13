using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Entities
{

    /// <summary>
    /// 主题
    /// </summary>
    public class Subject
    {
        public Guid? Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 查看数量
        /// </summary>
        public int? Look
        {
            get;
            set;
        }
        /// <summary>
        /// 回复数据
        /// </summary>
        public int? Reply
        {
            get;
            set;
        }
        /// <summary>
        /// 类别
        /// </summary>
        public Guid? category_id
        {
            get;
            set;
        }
        public int? Status
        {
            get;
            set;
        }
        public int? Visible
        {
            get;
            set;
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastDate
        {
            get;
            set;
        }
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime? ReleaseDate
        {
            get;
            set;
        }
        /// <summary>
        /// 主题项
        /// </summary>
        public string Items
        {
            get;
            set;
        }
        /// <summary>
        /// Url
        /// </summary>
        public string Url
        {
            get;
            set;
        }
        public string DataJSON
        {
            get;
            set;
        }
        /// <summary>
        /// 赞数
        /// </summary>
        public int? Favs
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public string ConfigXml
        {
            get;
            set;
        }
        public Guid? user_id
        {
            get;
            set;
        }
        public string Poster
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
        public bool? IsPicture
        {
            get;
            set;
        }
        public int? Seq
        {
            get;
            set;
        }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可以评论
        /// </summary>
        public bool? IsComment
        {
            get; set;
        }
        /// <summary>
        /// 复制的源主题
        /// </summary>
        public Guid? soure_id { get; set; }
        /// <summary>
        /// 提交审核的时间
        /// </summary>
        public DateTime? SubmitTime { get; set; }

        public int? Score
        {
            get; set;
        }
        /// <summary>
        /// 审核人
        /// </summary>
        public Guid? AppId
        {
            get;
            set;
        }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string AppDesc
        {
            get; set;
        }
    }
    //public class Keywords
    //{
    //    public Guid? Id
    //    {
    //        get;
    //        set;
    //    }
    //    public string Keyword
    //    {
    //        get;
    //        set;
    //    }
    //    public int? Priority
    //    {
    //        get;
    //        set;
    //    }
    //    public int? Count
    //    {
    //        get;
    //        set;
    //    }
    //    public DateTime? ModifiedDate
    //    {
    //        get;
    //        set;
    //    }

    //    public int? Type
    //    {
    //        get;
    //        set;
    //    }
    //    public Guid? AppId
    //    {
    //        get;
    //        set;
    //    }
    //}
}
