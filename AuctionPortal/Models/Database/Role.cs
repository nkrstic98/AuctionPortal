using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionPortal.Models.Database {
    public class Roles {
        public static IdentityRole administrator = new IdentityRole() {
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR"
        };
        public static IdentityRole user = new IdentityRole() {
            Name = "User",
            NormalizedName = "USER"
        };
    }
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                Roles.user,
                Roles.administrator
            );
        }
    }
}