using System.Collections.Generic;
using ASBicycle.Web.Models.Common;

namespace ASBicycle.Web.Models.Authen
{
    public class PermissionModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderSort { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }

    public class ButtonModel
    {
        public ButtonModel()
        {
            ButtonList = new List<KeyValueModel>();
            SelectedButtonList = new List<int>();
        }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public ICollection<KeyValueModel> ButtonList { get; set; }

        [KeyValue(DisplayProperty = "ButtonList")]
        public ICollection<int> SelectedButtonList { get; set; }
    }

    public class PermissionButtonModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}