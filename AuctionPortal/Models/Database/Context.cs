using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionPortal.Models.Database {
    public class AuctionPortalContext : IdentityDbContext<User> {
        public DbSet<Token> tokens { get; set; }
        public DbSet<Auction> auctions { get; set; }
        public DbSet<Bid> bids { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<Order> orders { get; set; }

        public AuctionPortalContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new TokenConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new AuctionConfiguration());
            builder.ApplyConfiguration(new BidConfiguration());
        }
    }
}