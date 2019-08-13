using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// 国家
        /// </summary>
        Nation = 1,
        /// <summary>
        /// 省份
        /// </summary>
        Province = 2,
        /// <summary>
        /// 城市
        /// </summary>
        City = 3,
        /// <summary>
        /// 县区
        /// </summary>
        Region = 4,
        /// <summary>
        /// 商区
        /// </summary>
        Area = 0,
        /// <summary>
        /// 品类
        /// </summary>
        Category = 5,
        /// <summary>
        /// 子类
        /// </summary>
        Subclass = 6
    }
    public class Location
    {
        public long? Id
        {
            get;
            set;
        }
        public LocationType? Type
        {
            get;
            set;
        }
        public string ZipCode
        {
            get;
            set; 
        }
        public string Name
        {
            get;
            set;
        }
        public long? ParentId
        {
            get;
            set;
        }
        public decimal? Lng
        {
            get;
            set;
        }
        public decimal? Lat
        {
            get;
            set;
        }
        public bool? Visible
        {
            get;
            set;
        }


    }
    public enum ProposalType
    {
        /// <summary>
        /// 有效
        /// </summary>
        Effective = 0,
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 1,
        /// <summary>
        /// 不良信息
        /// </summary>
        Unhealthy = 3
    }
    public class Proposal
    {
        public Guid? ref_id
        {
            get;
            set;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
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
        public ProposalType? Type
        {
            get;
            set;
        }
        public DateTime? CreationDate
        {
            get;
            set;
        }
        public string Poster
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 关注
    /// </summary>
    public class Attention
    {
        /// <summary>
        /// ME
        /// </summary>
        public Guid? Id
        {
            get;
            set;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? user_id
        {
            get;
            set;
        }
        /// <summary>
        /// 关注类型
        /// </summary>
        public AttentionType? Type
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationDate
        {
            get;
            set;
        }
        /// <summary>
        /// 是否互听
        /// </summary>
        public bool? IsEachOther
        {
            get;
            set;
        }
        /// <summary>
        /// 是否好友
        /// </summary>
        public bool? IsFriend
        {
            get;
            set;
        }
    }
}
