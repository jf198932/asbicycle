using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
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
        [MaxLength(32)]
        public string Phone { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Nickname { get; set; }
        [MaxLength(32)]
        public string Weixacc { get; set; }
        [MaxLength(32)]
        public string Email { get; set; }
        public int? Certification { get; set; }
        public int? Textmsg { get; set; }
        public DateTime? Textmsg_time { get; set; }
        [MaxLength(100)]
        public string Remember_token { get; set; }
        public int? Credits { get; set; }
        public int? Balance { get; set; }

        public string Img { get; set; }

        public string HeadImg { get; set; }

        public int? User_type { get; set; }
        public int? Device_os { get; set; }
        public string Device_id { get; set; }

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