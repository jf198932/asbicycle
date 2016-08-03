using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ASBicycle.Web.Models.School
{
    public class BikesiteModel
    {
        public BikesiteModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            SchoolList = new List<SelectListItem>();
            TypeList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [Required(ErrorMessage = "桩点名称不能为空")]
        public string Name { get; set; }
        public int? Type { get; set; }
        [MaxLength(500,ErrorMessage ="不能超过500字")]
        public string Description { get; set; }
        public int? Rent_charge { get; set; }
        public int? Return_charge { get; set; }
        public string Gps_point { get; set; }
        public int? Radius { get; set; }
        public int? Bike_count { get; set; }
        public int? Available_count { get; set; }

        public int? School_id { get; set; }
        public string School_name { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
        public List<SelectListItem> TypeList { get; set; }
    }
}