using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Sitebeacon")]
    public class Sitebeacon : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Gps_point { get; set; }
        public int? Tx_power { get; set; }
        public int? Battery { get; set; }


        public int? Bikesite_id { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
    }
}