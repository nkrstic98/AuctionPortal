using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPortal.Models.View {
    public class AddAuctionModel {
        [Required]
        [Display(Name = "Name")]
        [Remote(controller: "Auction", action: "ValidateAuctionName")]
        public string name { get; set; }
        
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }
        
        [Required]
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