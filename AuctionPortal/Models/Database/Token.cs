using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionPortal.Models.Database {
    public class Token {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Package")]
        public string package { get; set; }
        
        [Required]
        [Display(Name = "Ammount")]
        public int ammount { get; set; }
        
        [Required]
        [Display(Name = "Price")]
        public int price { get; set; }
    }

    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.Property(token => token.id).ValueGeneratedOnAdd();

            builder.HasData(
                new Token() {
                    id = 1,
                    package = "Silver",
                    ammount = 5,
                    price = 5 * 2
                },
                new Token() {
                    id = 2,
                    package = "Gold",
                    ammount = 10,
                    price = 10 * 2
                },
                new Token() {
                    id = 3,
                    package = "Platinum",
                    ammount = 20,
                    price = 20 * 2
                }
            );
        }
    }
}