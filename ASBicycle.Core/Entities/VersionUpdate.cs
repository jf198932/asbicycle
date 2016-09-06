using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("VersionUpdate")]
    public class VersionUpdate : Entity
    {
        public int device_os { get; set; }
        public int versionCode { get; set; }
        [MaxLength(100)]
        public string versionName { get; set; }
        public int upgrade { get; set; }
        [MaxLength(100)]
        public string versionUrl { get; set; }
    }
}