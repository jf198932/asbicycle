using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Bikesite")]
    public class Bikesite : Entity
    {
        public Bikesite()
        {
            Bikes = new List<Bike>();
            Messages = new List<Message>();
            Sitebeacons = new List<Sitebeacon>();
            Sitemonitors = new List<Sitemonitor>();
            Logs = new List<Log>();
        }

        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public int? Type { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int? Rent_charge { get; set; }
        public int? Return_charge { get; set; }
        [MaxLength(100)]
        public string Gps_point { get; set; }
        public int? Radius { get; set; }
        public int? Bike_count { get; set; }
        public int? Available_count { get; set; }

        public int? School_id { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }


        public virtual ICollection<Bike> Bikes { get; set; } 
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Sitebeacon> Sitebeacons { get; set; }
        public virtual ICollection<Sitemonitor> Sitemonitors { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}