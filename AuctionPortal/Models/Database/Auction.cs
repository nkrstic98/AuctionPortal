using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionPortal.Models.Database {
    public class Auction {
        public enum AuctionState
        {
            DRAFT, READY, OPEN, SOLD, EXPIRED, DELETED
        };

        [Key]
        [Remote(controller: "User", action: "ValidateAuctionName", AdditionalFields = nameof(name))]
        public int id { get; set; }

        [Required]
        [Display(Name = "Auction")]
        [Remote(controller: "Book", action: "ValidateAuctionName", AdditionalFields = nameof(id))]
        public string name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Image")]
        public int imageId { get; set; }

        [Required]
        [Display(Name = "Image")]
        public Image image { get; set; }

        [Required]
        [Display(Name = "User")]
        public string userId { get; set; }
    
        [Required]
        [Display(Name = "User")]
        public User user { get; set; }
        
        [Required]
        [Display(Name = "Starting price")]
        public double startingPrice { get; set; }

        [Required]
        [Display(Name = "Creation date")]
        public DateTime creationDateTime { get; set; }

        [Required]
        [Display(Name = "Opening date and time")]
        public DateTime openingDateTime { get; set; }

        [Required]
        [Display(Name = "Closing date and time")]
        public DateTime closingDateTime { get; set; }

        [Required]
        [Display(Name = "Auction state")]
        public AuctionState state { get; set; }

        [Display(Name = "Licitations")]
        public ICollection<Bid> biddingList { get; set; }

        [Display(Name = "Total accession ammout")]
        public double accession { get; set; } //ukupno povecanje cene
    }

    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.Property(auction => auction.id).ValueGeneratedOnAdd();
        }
    }
}