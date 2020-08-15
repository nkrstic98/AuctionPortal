using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionPortal.Models.Database {
    public class Image {
        [Key]
        public int id { get; set; }
        [Required]
        public byte[] data { get; set; }
    }

    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(image => image.id).ValueGeneratedOnAdd();
        }
    }
}