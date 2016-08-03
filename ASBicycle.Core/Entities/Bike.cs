using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Bike")]
    public class Bike : Entity
    {
        public Bike()
        {
            Logs = new List<Log>();
            Tracks = new List<Track>();
        }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public string Ble_serial { get; set; }
        [MaxLength(32)]
        public string Ble_name { get; set; }
        public int? Ble_type { get; set; }
        public int? Lock_status { get; set; }
        public int? Bike_status { get; set; }
        public int? Vlock_status { get; set; }
        public int? Insite_status { get; set; }
        [MaxLength(32)]
        public string Position { get; set; }
        public int? Battery { get; set; }

        public string Bike_img { get; set; }

        public int? User_id { get; set; }
        public int? School_id { get; set; }
        public int? Bikesite_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
        //[ForeignKey("School_id")]
        //public virtual School School { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }


        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}