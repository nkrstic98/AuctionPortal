using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionPortal.Models.Database {
    public class Bid {
        [Key]
        public int id { get; set; }
        public string userId { get; set; }
        public User user { get; set; }
        public int auctionId { get; set; }
        public Auction auction { get; set; }
        public DateTime bidTime { get; set; }
    }

    public class BidConfiguration : IEntityTypeConfiguration<Bid> {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.Property(b => b.id).ValueGeneratedOnAdd();
        }
    }
}