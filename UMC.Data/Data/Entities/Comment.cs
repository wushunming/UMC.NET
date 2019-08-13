using System;

namespace UMC.Data.Entities
{
    /// <summary>
    /// ��ע����
    /// </summary>
    public enum AttentionType
    {
        /// <summary>
        /// ������
        /// </summary>
        Follow = 2,
        /// <summary>
        /// ����
        /// </summary>
        Friend = 1,
        /// <summary>
        /// ������
        /// </summary>
        Black = -1,
        /// <summary>
        /// ����
        /// </summary>
        Atten = 0,
    }
    /// <summary>
    /// �ظ�����
    /// </summary>
    public enum ReplyType
    {
        /// <summary>
        /// �ظ�
        /// </summary>
        Reply = 0,
        /// <summary>
        ///����
        /// </summary>
        Request = 2,
        /// <summary>
        /// ˽��
        /// </summary>
        Mail = 1,
        /// <summary>
        /// ת��
        /// </summary>
        Farwork = 3,
        /// <summary>
        /// ����
        /// </summary>
        Blog = 4
    }
    /// <summary>
    /// ��Ա��ϵ
    /// </summary>
    public class Relation
    {
        public Guid? Id
        {
            get;
            set;
        }
        /// <summary>
        /// ������
        /// </summary>
        public int? Attention
        {
            get;
            set;
        }
        /// <summary>
        /// ������
        /// </summary>
        public int? Friends
        {
            get;
            set;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public int? BlackList
        {
            get;
            set;
        }
        /// <summary>
        /// �ղ���
        /// </summary>
        public int? Favs
        {
            get;
            set;
        }
        /// <summary>
        /// ������
        /// </summary>
        public int? Fans
        {
            get;
            set;
        }
        /// <summary>
        /// ������
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
    /// ����
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
        ///����ʱ��
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
        /// �Ƿ���ʾ
        /// </summary>
        public int? Visible
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ч������
        /// </summary>
        public bool? IsInvalidScore
        {
            get;
            set;
        }

    }
    /// <summary>
    /// �ظ�
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
        /// �ظ�����
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
        /// ����
        /// </summary>
        Comment = 2,
        /// <summary>
        /// ����
        /// </summary>
        Release = 0,
        /// <summary>
        /// ת��
        /// </summary>
        Farwork = 1
    }

}
