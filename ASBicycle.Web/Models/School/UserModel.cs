using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ASBicycle.Web.Models.School
{
    public class UserModel
    {
        public UserModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            SchoolList = new List<SelectListItem>();
            CertificationList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Weixacc { get; set; }
        public string Email { get; set; }
        public int? Certification { get; set; }
        public int? Textmsg { get; set; }
        public DateTime? Textmsg_time { get; set; }
        public string Remember_token { get; set; }
        public int? Credits { get; set; }
        public int? Balance { get; set; }
        public int? School_id { get; set; }
        public string School_name { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
        public List<SelectListItem> CertificationList { get; set; }
    }
}