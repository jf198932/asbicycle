using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("School")]
    public class School : Entity
    {
        public School()
        {
            //Bikes = new List<Bike>();
            Bikesites = new List<Bikesite>();
            Users = new List<User>();
        }

        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Areacode { get; set; }
        [MaxLength(100)]
        public string Gps_point { get; set; }
        public int? Site_count { get; set; }
        public int? Bike_count { get; set; }
        public int? Time_charge { get; set; }
        public DateTime? Refresh_date { get; set; }

        //public virtual ICollection<Bike> Bikes { get; set; } 
        public virtual ICollection<Bikesite> Bikesites { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}