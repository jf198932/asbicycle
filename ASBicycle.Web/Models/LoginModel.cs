using System.Web.Mvc;

namespace ASBicycle.Web.Models
{
    public class LoginModel
    {
        public bool CheckoutAsGuest { get; set; }

        [AllowHtml]
        public string Email { get; set; }

        [AllowHtml]
        public string UserNameOrEmail { get; set; }
        [AllowHtml]
        public string TenancyName { get; set; }

        [AllowHtml]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}