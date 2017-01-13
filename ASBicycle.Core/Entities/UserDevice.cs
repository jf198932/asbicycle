using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("tb_user_statistics")]
    public class UserDevice : Entity
    {
        public DateTime create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? user_id { get; set; }
        public string mobile_brand { get; set; }
        public string mobile_model { get; set; }
        public string os_version { get; set; }
        public string app_version { get; set; }
        public DateTime? last_use_date { get; set; }
        public string device_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User User { get; set; }
    }
}