using System;
using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class TrackEntity : EntityDto
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public string Start_point { get; set; }
        public string End_point { get; set; }
        public int? Start_site_id { get; set; }
        public int? End_site_id { get; set; }
        public DateTime? Start_time { get; set; }
        public DateTime? End_time { get; set; }
        public double? Payment { get; set; }
        public double? Should_pay { get; set; }
        public int? Pay_status { get; set; }
        public string Pay_method { get; set; }
        public string Pay_docno { get; set; }
        public string Remark { get; set; }
        public int User_id { get; set; }
        public int? Bike_id { get; set; }
        public string Start_site_name { get; set; }
        public string End_site_name { get; set; }
        public int? School_id { get; set; }
        public string School_name { get; set; }
        public string Ble_name { get; set; }
        public double time_charge { get; set; }
        public string Lock_pwd { get; set; }

        public string Start_gps_point { get; set; }

        public int Free_time { get; set; }

        public double Fixed_amount { get; set; }

        public double Top_amount { get; set; }
        public double? Recharge_count { get; set; }

        public int Ble_type { get; set; }

        public string Ble_serial { get; set; }
    }
}