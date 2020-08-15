using AuctionPortal.Models.Database;
using static AuctionPortal.Models.Database.Auction;

namespace AuctionPortal.Models.View {
    public class AuctionView {
        public string base64Data { get; set; }
        public Auction auction { get; set; }
    }
}