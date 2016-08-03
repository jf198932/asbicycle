using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Refound")]
    public class Refound : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public int? Refound_count { get; set; }


        public int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}