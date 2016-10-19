using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Sitemonitor")]
    public class Sitemonitor : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public int? Bikesite_id { get; set; }
        public int? Status { get; set; }
        public bool Enabled { get; set; }

        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
    }
}