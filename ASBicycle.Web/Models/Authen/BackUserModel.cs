using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ASBicycle.Web.Models.Common;

namespace ASBicycle.Web.Models.Authen
{
    public class BackUserModel
    {
        public BackUserModel()
        {
            RoleList = new List<KeyValueModel>();
            SelectedRoleList = new List<int>();
            Enabled = true;
        }

        public int Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Enabled { get; set; }
        public int PwdErrorCount { get; set; }
        public int LoginCount { get; set; }
        public DateTime? RegisterTime { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public ICollection<KeyValueModel> RoleList { get; set; }
        [KeyValue(DisplayProperty = "RoleList")]
        public ICollection<int> SelectedRoleList { get; set; }
    }
}