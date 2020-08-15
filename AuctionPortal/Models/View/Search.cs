using System.Collections.Generic;
using static AuctionPortal.Models.Database.Auction;

namespace AuctionPortal.Models.View {
    public class SearchModel {
        public string name { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
        public AuctionState state { get; set; }

        public IList<AuctionView> auctionList { get; set; }
    }
}