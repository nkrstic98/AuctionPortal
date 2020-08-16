using System.Collections.Generic;
using static AuctionPortal.Models.Database.Auction;

namespace AuctionPortal.Models.View {
    public class SearchModel {
        public string name { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string state { get; set; }

        public IList<AuctionView> auctionList { get; set; }
    }
}