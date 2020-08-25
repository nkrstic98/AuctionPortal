using System.Collections.Generic;
using AuctionPortal.Models.Database;

namespace AuctionPortal.Models.View {
    public class SearchModel {
        public string name { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string state { get; set; }
        public int numPages { get; set; }
        public int currPage { get; set; }
        public bool second { get; set; }
        public bool wonAuctions { get; set; }

        public IList<AuctionView> auctionList { get; set; }
        public IList<Order> orders { get; set; }
    }
}