using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AuctionPortal.Hubs {
    public class BiddingHub : Hub {
        public async Task Bid(int auctionId) {
            await base.Clients.All.SendAsync("UpdateAuction", auctionId);
        }
    }
}