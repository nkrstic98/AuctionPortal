using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionPortal.Models.Database {
    public class Order {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Buyer")]
        public string userId { get; set; }

        [Required]
        [Display(Name = "Buyer")]
        public User user { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        [Required]
        [Display(Name = "Bought Ammount")]
        public int tokenAmmout { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int price { get; set; }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(order => order.id).ValueGeneratedOnAdd();
        }
    }
}