using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPortal.Models.View {
    public class LoginModel {
        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }
        [HiddenInput]
        public string returnUrl { get; set; }
    }
}