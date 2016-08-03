using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Message")]
    public class Message : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(1000)]
        public string Message_info { get; set; }
        public DateTime? Send_Time { get; set; }


        public int? Bikesite_id { get; set; }
        public int? User_id { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}