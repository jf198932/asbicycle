using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 会员
    /// </summary>
    [Table("User")]
    public class User : Entity
    {
        public User()
        {
            Bikes = new List<Bike>();
            Creditss = new List<Credit>();
            Messages = new List<Message>();
            Recharges = new List<Recharge>();
            Refounds = new List<Refound>();
        }

        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(32)]
        public string Phone { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(32)]
        public string Nickname { get; set; }
        /// <summary>
        /// 微信账号
        /// </summary>
        [MaxLength(32)]
        public string Weixacc { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(32)]
        public string Email { get; set; }
        /// <summary>
        /// 实名认证状态，1未申请，2已申请，3已认证，4认证失败
        /// </summary>
        public int? Certification { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public int? Textmsg { get; set; }
        /// <summary>
        /// 短信验证码发送时间
        /// </summary>
        public DateTime? Textmsg_time { get; set; }
        /// <summary>
        /// 记住登录状态的token
        /// </summary>
        [MaxLength(100)]
        public string Remember_token { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int? Credits { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public int? Balance { get; set; }
        /// <summary>
        /// 用户认证图片
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }
        /// <summary>
        /// 身份类型 0：非校园用户  1：在校学生  2：教职工
        /// </summary>
        public int? User_type { get; set; }
        /// <summary>
        /// 1.IOS,2.Android
        /// </summary>
        public int? Device_os { get; set; }
        /// <summary>
        /// 用户设备串号
        /// </summary>
        public string Device_id { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string Id_no { get; set; }
        /// <summary>
        /// 所属学校
        /// </summary>
        public int? School_id { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }


        public virtual ICollection<Bike> Bikes { get; set; }
        public virtual ICollection<Credit> Creditss { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Recharge> Recharges { get; set; }
        public virtual ICollection<Refound> Refounds { get; set; }
    }
}