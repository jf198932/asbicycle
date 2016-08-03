using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ASBicycle.Web.Models.School
{
    public class BikeModel
    {
        public BikeModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            
            TypeList = new List<SelectListItem>();
            LockStatusList = new List<SelectListItem>();
            BikeStatusList = new List<SelectListItem>();
            VlockStatusList = new List<SelectListItem>();
            InsiteStatusList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
            BikesiteList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public string Ble_serial { get; set; }
        public string Ble_name { get; set; }
        public int? Ble_type { get; set; }
        public int? Lock_status { get; set; }
        public int? Bike_status { get; set; }
        public int? Vlock_status { get; set; }
        public int? Insite_status { get; set; }
        public string Position { get; set; }
        public int? Battery { get; set; }
        public string Bike_img { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
        public int? Bikesite_id { get; set; }
        public string Bikesite_name { get; set; }

        public List<SelectListItem> TypeList { get; set; }
        public List<SelectListItem> LockStatusList { get; set; }
        public List<SelectListItem> BikeStatusList { get; set; }
        public List<SelectListItem> VlockStatusList { get; set; }
        public List<SelectListItem> InsiteStatusList { get; set; }
        public List<SelectListItem> UserList { get; set; }
        public List<SelectListItem> BikesiteList { get; set; }
    }
}