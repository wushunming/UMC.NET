using System;

namespace UMC.Data.Entities
{
    /// <summary>
    /// 关注类型
    /// </summary>
    public enum AttentionType
    {
        /// <summary>
        /// 收听的
        /// </summary>
        Follow = 2,
        /// <summary>
        /// 好友
        /// </summary>
        Friend = 1,
        /// <summary>
        /// 黑名单
        /// </summary>
        Black = -1,
        /// <summary>
        /// 听众
        /// </summary>
        Atten = 0,
    }
    /// <summary>
    /// 回复类型
    /// </summary>
    public enum ReplyType
    {
        /// <summary>
        /// 回复
        /// </summary>
        Reply = 0,
        /// <summary>
        ///请求
        /// </summary>
        Request = 2,
        /// <summary>
        /// 私信
        /// </summary>
        Mail = 1,
        /// <summary>
        /// 转发
        /// </summary>
        Farwork = 3,
        /// <summary>
        /// 博客
        /// </summary>
        Blog = 4
    }
    /// <summary>
    /// 会员关系
    /// </summary>
    public class Relation
    {
        public Guid? Id
        {
            get;
            set;
        }
        /// <summary>
        /// 收听数
        /// </summary>
        public int? Attention
        {
            get;
            set;
        }
        /// <summary>
        /// 好友数
        /// </summary>
        public int? Friends
        {
            get;
            set;
        }
        /// <summary>
        /// 黑名单数
        /// </summary>
        public int? BlackList
        {
            get;
            set;
        }
        /// <summary>
        /// 收藏数
        /// </summary>
        public int? Favs
        {
            get;
            set;
        }
        /// <summary>
        /// 听众数
        /// </summary>
        public int? Fans
        {
            get;
            set;
        }
        /// <summary>
        /// 评论数
        /// </summary>
        public int? Comments
        {
            get;
            set;
        }
        public int? Gender
        {
            get;
            set;
        }
        public DateTime? Birthday
        {
            get;
            set;
        }
        public string Nation
        {
            get;
            set;
        }
        public string Province
        {
            get;
            set;
        }
        public string Region
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
        public string Summary
        {
            get;
            set;
        }
        public string Alias
        {
            get;
            set;
        }


        public string Latlng { get; set; }
    }

    /// <summary>
    /// 评论
    /// </summary>
    public class Comment
    {
        public Guid? Id
        {
            get;
            set;
        }
        public Guid? ref_id
        {
            get;
            set;
        }
        public Guid? for_id
        {
            get;
            set;
        }
        public Guid? user_id
        {
            get;
            set;
        }
        public int? Score
        {
            get;
            set;
        }
        public int? Effective
        {
            get;
            set;
        }
        public int? Invalid
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        /// <summary>
        ///评论时间
        /// </summary>
        public DateTime? CommentDate
        {
            get;
            set;
        }
        public int? Unhealthy
        {
            get;
            set;
        }
        public int? Reply
        {
            get;
            set;
        }
        public int? Farworks
        {
            get;
            set;
        }

        public string OuterId
        {
            get;
            set;
        }
        public string Poster
        {
            get;
            set;
        }

        public bool? IsPicture
        {
            get;
            set;
        }
        public Guid? AppId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public int? Visible
        {
            get;
            set;
        }
        /// <summary>
        /// 无效的评分
        /// </summary>
        public bool? IsInvalidScore
        {
            get;
            set;
        }

    }
    /// <summary>
    /// 回复
    /// </summary>
    public class Reply
    {
        public Guid? Id
        {
            get;
            set;
        }
        public Guid? ref_id
        {
            get;
            set;
        }
        public Guid? user_id
        {
            get;
            set;
        }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content
        {
            get;
            set;
        }
        public string ContentType
        {
            get;
            set;
        }
        public int? Effective
        {
            get;
            set;
        }
        public int? Invalid
        {
            get;
            set;
        }
        public DateTime? ReplyDate
        {
            get;
            set;
        }
        public string Poster
        {
            get;
            set;
        }
        public Guid? AppId
        {
            get;
            set;
        }
        public bool? IsPicture
        {
            get;
            set;
        }
    }
    public class Release
    {
        public Guid? Id
        {
            get;
            set;
        }
        public Guid? user_id
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        public Guid? ref_id
        {
            get;
            set;
        }
        public int? Farworks
        {
            get;
            set;
        }
        public int? Effective
        {
            get;
            set;
        }
        public int? Invalid
        {
            get;
            set;
        }
        public DateTime? ReleaseDate
        {
            get;
            set;
        }
        public ReleaseType? Type
        {
            get;
            set;
        }
        public string Poster
        {
            get;
            set;
        }
        public string OuterId
        {
            get;
            set;
        }
        public bool? IsPicture
        {
            get;
            set;
        }

        public Guid? AppId
        {
            get;
            set;
        }
    }
    public enum ReleaseType
    {
        /// <summary>
        /// 评论
        /// </summary>
        Comment = 2,
        /// <summary>
        /// 发布
        /// </summary>
        Release = 0,
        /// <summary>
        /// 转发
        /// </summary>
        Farwork = 1
    }

}
