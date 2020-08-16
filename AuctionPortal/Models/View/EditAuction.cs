using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AuctionPortal.Models.View {
    public class EditAuction {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
        
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }
        
        [Display(Name = "Image")]
        public IFormFile file { get; set; }
        
        [Required]
        [Display(Name = "Price")]
        public int startingPrice { get; set; }
        [Required]
        [Display(Name = "Opening date and time")]
        public DateTime openingDateTime { get; set; }
        
        [Required]
        [Display(Name = "Closing date and time")]
        public DateTime closingDateTime { get; set; }
    }
}