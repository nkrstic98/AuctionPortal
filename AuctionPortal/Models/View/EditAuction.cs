using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPortal.Models.View {
    public class EditAuction {
        [Required]
        [Remote(controller: "Auction", action: "ValidateAuctionName", AdditionalFields = nameof(name))]
        public int id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Remote(controller: "Auction", action: "ValidateAuctionName", AdditionalFields = nameof(id))]
        public string name { get; set; }
        
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }
        
        [Display(Name = "Image")]
        public IFormFile file { get; set; }
        
        [Required]
        [Display(Name = "Price")]
        public double startingPrice { get; set; }
        [Required]
        [Display(Name = "Opening date and time")]
        public DateTime openingDateTime { get; set; }
        
        [Required]
        [Display(Name = "Closing date and time")]
        public DateTime closingDateTime { get; set; }
    }
}