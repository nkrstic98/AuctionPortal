using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionPortal.Models.Database;
using Microsoft.AspNetCore.Authorization;
using AuctionPortal.Models.View;
using System.IO;
using Microsoft.AspNetCore.Identity;
using System.Timers;
using System.Threading;

namespace AuctionPortal.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly AuctionPortalContext _context;
        private UserManager<User> userManager;
        public AuctionController(AuctionPortalContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        
        //Get Image data to be shown on website
        public async Task<AuctionView> GetImageView(int index) {
            IList<Image> images = await this._context.images.ToListAsync();

            Image image = images.Where(i => i.id == index).FirstOrDefault();

            AuctionView model = new AuctionView() {
                base64Data = Convert.ToBase64String(image.data)
            };

            return model;
        }

        //Gets remaining time for open auctions
        public string GetRemainingTime(TimeSpan diff) {
            double seconds = Math.Floor((diff.TotalMilliseconds / 1000) % 60);
            double minutes = Math.Floor((diff.TotalMilliseconds / (1000 * 60)) % 60);
            double hours = Math.Floor((diff.TotalMilliseconds / (1000 * 60 * 60)));

            string h = (hours < 10) ? "0" + hours.ToString() : hours.ToString();
            string m = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
            string s = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();
            
            return h + ":" + m + ":" + s;
        }

        // GET: All open auctions in Index View
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var auctionPortalContext = this._context.auctions;

            IList<Auction> list = await auctionPortalContext
                .Where(a => a.state == Auction.AuctionState.OPEN)
                .OrderByDescending(a => a.creationDateTime)
                .Take(12)
                .Include(u => u.user)
                .Include(a => a.biddingList)
                .ThenInclude(a => a.user)
                .ToListAsync();

            IList<AuctionView> auctionList = new List<AuctionView>();

            foreach (var el in list)
            {
                AuctionView data = await this.GetImageView(el.imageId);

                if(DateTime.Compare(el.closingDateTime, DateTime.Now) < 0 && el.state == Auction.AuctionState.OPEN) {
                    await this.closeAuction(el.id);
                }
                else {
                    TimeSpan diff = el.closingDateTime - DateTime.Now;
                    data.remainingTime = this.GetRemainingTime(diff);
                }

                el.biddingList = el.biddingList.OrderByDescending(b => b.bidTime).ToList();

                data.auction = el;

                data.open = true;

                auctionList.Add(data);
            }

            SearchModel searchModel = new SearchModel() {
                auctionList = auctionList,
                numPages = Decimal.ToInt32(Math.Ceiling(Convert.ToDecimal(auctionPortalContext.Where(a => a.state == Auction.AuctionState.OPEN).Count() / 12.0))),
                currPage = 1
            };

            return View(searchModel);
        }

        //Search for Auctions using provided filters
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Search(SearchModel model)
        {
            IQueryable<Auction> query = this._context.auctions;

            IList<Auction> auctions;
            double numAuctions;

            if(!model.wonAuctions) {

                if(model.name != null) {
                    query = query.Where(auction => auction.name.ToLower().Contains(model.name));
                }

                if(model.minPrice != null) {
                    query = query.Where(auction => auction.startingPrice + auction.accession >= int.Parse(model.minPrice));
                }

                if(model.maxPrice != null) {
                    query = query.Where(auction => auction.startingPrice + auction.accession <= int.Parse(model.maxPrice));
                }

                if(model.state != null) {
                    switch(model.state)
                    {
                        case "1": query = query.Where(auction => auction.state == Auction.AuctionState.DRAFT);
                                break;
                        case "2": query = query.Where(auction => auction.state == Auction.AuctionState.READY);
                                break;
                        case "3": query = query.Where(auction => auction.state == Auction.AuctionState.OPEN);
                                break;
                        case "4": query = query.Where(auction => auction.state == Auction.AuctionState.SOLD);
                                break;
                        case "5": query = query.Where(auction => auction.state == Auction.AuctionState.EXPIRED);
                                break;
                        case "6": query = query.Where(auction => auction.state == Auction.AuctionState.DELETED);
                                break;
                    }
                }

                numAuctions = query.Count();

                auctions = await query
                    .OrderByDescending(auction => auction.openingDateTime)
                    .Skip((model.currPage - 1) * 12)
                    .Take(12)
                    .Include(u => u.user)
                    .Include(a => a.biddingList)
                    .ThenInclude(u => u.user)
                    .ToListAsync();

            }
            else {
                User loggedInUser = await this.userManager.GetUserAsync(base.User);

                IList<Auction> auctionList = this._context.auctions
                    .Where(a => a.state == Auction.AuctionState.SOLD)
                    .Include(u => u.user)
                    .Include(a => a.biddingList)
                    .ThenInclude(u => u.user)
                    .ToList();

                auctions = new List<Auction>();

                foreach (var item in auctionList)
                {
                    if(item.biddingList.Last().userId == loggedInUser.Id) {
                        auctions.Add(item);
                    }
                }

                numAuctions = auctions.Count;
            }

            IList<AuctionView> auctionViews = new List<AuctionView>();

            foreach (Auction a in auctions)
            {
                AuctionView av = await this.GetImageView(a.imageId);

                if(DateTime.Compare(a.closingDateTime, DateTime.Now) < 0 && a.state == Auction.AuctionState.OPEN) {
                    await this.closeAuction(a.id);
                }
                else {
                    TimeSpan diff = a.closingDateTime - DateTime.Now;
                    av.remainingTime = this.GetRemainingTime(diff);
                }

                if(a.state == Auction.AuctionState.OPEN) av.open = true;
                else av.open = false;

                a.biddingList = a.biddingList.OrderByDescending(b => b.bidTime).ToList();

                av.auction = a;

                auctionViews.Add(av);
            }

            SearchModel searchModel = new SearchModel() {
                auctionList = auctionViews,
                numPages = Decimal.ToInt32(Math.Ceiling(Convert.ToDecimal(numAuctions / 12.0))),
                currPage = model.currPage
            };

            if(model.second == true) {
                return PartialView("Pagination", searchModel);
            }
            return PartialView("AuctionList", searchModel);
        }

        // GET: Auction/AdminDetails/5
        //Different Views for Admin and regular User
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.auctions.Include(a => a.biddingList).ThenInclude(u => u.user).FirstOrDefaultAsync(m => m.id == id);

            if (auction == null)
            {
                return NotFound();
            }

            AuctionView auctionView = await this.GetImageView(auction.imageId);
            auctionView.auction = auction;

            return View(auctionView);
        }

        // GET: Auction Details for Regular users
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int? auctionId)
        {
            if (auctionId == null)
            {
                return NotFound();
            }

            var auction = await _context.auctions
                .Where(m => m.id == auctionId)
                .Include(u => u.user)
                .Include(a => a.biddingList)
                .ThenInclude(u => u.user)
                .FirstOrDefaultAsync();

            if (auction == null)
            {
                return NotFound();
            }

            auction.biddingList = auction.biddingList.OrderByDescending(b => b.bidTime).ToList();

            AuctionView auctionView = await this.GetImageView(auction.imageId);

            if(DateTime.Compare(auction.closingDateTime, DateTime.Now) < 0 && auction.state == Auction.AuctionState.OPEN) {
                await this.closeAuction(auction.id);
            }
            else {
                TimeSpan diff = auction.closingDateTime - DateTime.Now;
                auctionView.remainingTime = this.GetRemainingTime(diff);
            }

            if(auction.state == Auction.AuctionState.OPEN) auctionView.open = true;
            else auctionView.open = false;

            auctionView.auction = auction;

            return View(auctionView);
        }

        // GET: Gets called when Real-time Auction state changes
        [AllowAnonymous]
        public async Task<IActionResult> GetDetails(int auctionId) {
            Auction auction = await this._context.auctions
                .Where(a => a.id == auctionId)
                .Include(u => u.user)
                .Include(a => a.biddingList)
                .ThenInclude(u => u.user)
                .FirstOrDefaultAsync();

            AuctionView data = await this.GetImageView(auction.imageId);

            if(DateTime.Compare(auction.closingDateTime, DateTime.Now) < 0) {
                data.remainingTime = "00:00:00";
                if(auction.state != Auction.AuctionState.EXPIRED && auction.state != Auction.AuctionState.SOLD) {
                    await this.closeAuction(auction.id);
                }
                data.open = false;
            }
            else {
                TimeSpan diff = auction.closingDateTime - DateTime.Now;
                data.remainingTime = this.GetRemainingTime(diff);
                data.open = true;
            }

            auction.biddingList = auction.biddingList.OrderByDescending(b => b.bidTime).ToList();

            data.auction = auction;

            return PartialView("AuctionDetails", data);
        }

        // Check if Auction name is taken
        public IActionResult ValidateAuctionName(int? id, string name) {
            IQueryable<Auction> query = this._context.auctions.Where(auction => auction.name == name);

            if(id != null) {
                query = query.Where(auction => auction.id != id);
            }

            bool exists = query.Any();
            if(!exists) {
                return Json(true);
            }
            else {
                return Json("Auction name is already taken");
            }
        }

        // GET: Auction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddAuctionModel model)
        {
            if(!ModelState.IsValid) {
                return View(model);
            }

            Image image;

            using(BinaryReader reader = new BinaryReader(model.file.OpenReadStream())) {
                image = new Image() {
                    data = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length))
                };
            }

            DateTime creationDateTime = DateTime.Now;

            if(DateTime.Compare(model.openingDateTime, model.closingDateTime) >= 0) {
                ModelState.AddModelError("", "Opening and/or closing date are not valid");
                return View(model);
            }

            if(DateTime.Compare(creationDateTime, model.openingDateTime) >= 0 || DateTime.Compare(creationDateTime, model.closingDateTime) >= 0) {
                ModelState.AddModelError("", "Opening and/or closing date are not valid");
                return View(model);
            }

            User user = await this.userManager.GetUserAsync(base.User);

            Auction auction = new Auction() {
                name = model.name,
                description = model.description,
                imageId = image.id,
                image = image,
                startingPrice = model.startingPrice,
                creationDateTime = creationDateTime,
                openingDateTime = model.openingDateTime,
                closingDateTime = model.closingDateTime,
                state = Auction.AuctionState.DRAFT,
                accession = 0,
                userId = user.Id,
                user = user
            };

            if(user.auctionList == null) {
                user.auctionList = new List<Auction>();
            }

            user.auctionList.Add(auction);

            this._context.Users.Update(user);
            await this._context.images.AddAsync(image);
            await this._context.auctions.AddAsync(auction);
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Auction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(new EditAuction() {
                id = auction.id,
                name = auction.name,
                description = auction.description,
                startingPrice = auction.startingPrice,
                openingDateTime = auction.openingDateTime,
                closingDateTime = auction.closingDateTime
            });
        }

        // POST: Auction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAuction model)
        {
            if (ModelState.IsValid)
            {
                Auction auction = await this._context.auctions.FindAsync(model.id);

                auction.name = model.name;
                auction.description = model.description;
                auction.startingPrice = model.startingPrice;
                auction.openingDateTime = model.openingDateTime;
                auction.closingDateTime = model.closingDateTime;

                if(model.file != null) {
                    Image image = await this._context.images.FindAsync(auction.imageId);

                    using(BinaryReader reader = new BinaryReader(model.file.OpenReadStream())) {
                        image.data = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length));
                    }

                    auction.image = image;

                    this._context.images.Update(image);
                }

                this._context.auctions.Update(auction);
                await this._context.SaveChangesAsync();

                return RedirectToAction(nameof(AuctionController.MyAuctions), "Auction");
            }
            return View(model);
        }

        // Get View for Admin with all Auctions in DRAFT state
        public async Task<IActionResult> ManageAuctions()
        {
            var auctionPortalContext = _context.auctions;
            IList<Auction> list = await auctionPortalContext.ToListAsync();

            return View(list);
        }

        //Approve new auctions in DRAFT state -> set to READY state
        public async Task<IActionResult> Approve(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == id);
                    auction.state = Auction.AuctionState.READY;

                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageAuctions));
            }
            else {
                return RedirectToAction(nameof(ManageAuctions));
            }
        }

        //Delete auctions that are not approved -> set to DELETE state
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == id);

                    auction.state = Auction.AuctionState.DELETED;

                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            User user = await this.userManager.GetUserAsync(base.User);

            IList<string> roles = await this.userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if(role == "User") {
                    return RedirectToAction ( nameof ( AuctionController.MyAuctions ), "Auction" );
                }
                else {
                    if(role == "Administrator") {
                        return RedirectToAction ( nameof ( AuctionController.ManageAuctions ), "Auction" );
                    }
                }
            }

            return RedirectToAction ( nameof ( AuctionController.Index ), "Auction" );
        }

        private bool AuctionExists(int id)
        {
            return _context.auctions.Any(e => e.id == id);
        }

        //Show me all auctions I created
        public async Task<IActionResult> MyAuctions() {
            User user = await this.userManager.GetUserAsync(base.User);

            if(user == null) {
                return RedirectToAction(nameof(UserController.Login), "User");
            }

            IList<Auction> auctions = await this._context.auctions.Where(u => u.userId == user.Id)
            .Include(u => u.user)
            .Include(a => a.biddingList)
            .ThenInclude(u => u.user)
            .OrderByDescending(a => a.creationDateTime)
            .ToListAsync();

            IList<AuctionView> auctionViews = new List<AuctionView>();

            foreach (var item in auctions)
            {
                AuctionView auctionView = await GetImageView(item.id);

                if(DateTime.Compare(item.closingDateTime, DateTime.Now) < 0 && item.state == Auction.AuctionState.OPEN) {
                    await this.closeAuction(item.id);
                    auctionView.open = false;
                }
                else {
                    TimeSpan diff = item.closingDateTime - DateTime.Now;
                    auctionView.remainingTime = this.GetRemainingTime(diff);
                    auctionView.open = true;

                    auctionView.auction = item;
                }

                auctionViews.Add(auctionView);
            }

            return View(auctionViews);
        }

        //Show me details of my auction
        public async Task<IActionResult> MyAuctionDetails(int? auctionId)
        {
            if (auctionId == null)
            {
                return NotFound();
            }

            var auction = await _context.auctions.FirstOrDefaultAsync(m => m.id == auctionId);

            if (auction == null)
            {
                return NotFound();
            }

            AuctionView auctionView = await this.GetImageView(auction.imageId);
            auctionView.auction = auction;

            return View(auctionView);
        }

        //Gets called every 30s from client AJAX request
        //Check if any auction is ready to be opened
        [AllowAnonymous]
        public async Task openAuctions() {
            Console.WriteLine("Auction update " + DateTime.Now);

            IList<Auction> auctions = await this._context.auctions.ToListAsync();

            foreach (var a in auctions)
            {
                if(DateTime.Compare(a.openingDateTime, DateTime.Now) <= 0) {
                    if(a.state == Auction.AuctionState.READY) {
                        Console.WriteLine("I am opening auction " + a.id);
                        a.state = Auction.AuctionState.OPEN;
                        this._context.auctions.Update(a);
                    }
                    if(a.state == Auction.AuctionState.DRAFT) {
                        Console.WriteLine("I am making auction " + a.id + " expired");
                        a.state = Auction.AuctionState.EXPIRED;
                        this._context.auctions.Update(a);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        //Gets called when auction's time runs out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> closeAuction(int auctionId) {
            Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == auctionId);

            if(auction.accession != 0) {
                auction.state = Auction.AuctionState.SOLD;
            }
            else {
                auction.state = Auction.AuctionState.EXPIRED;
            }

            this._context.auctions.Update(auction);
            await this._context.SaveChangesAsync();

            return await this.GetAuction(auctionId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bid(int auctionId) {
            Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == auctionId);
            User user = await this.userManager.GetUserAsync(base.User);

            if(auction.user == user) {
                return Json("You cannot bid on your own auction");
            }

            if(user.tokens == 0) {
                return Json("You do not have tokens, so you cannot bid on this auction");
            }

            user.tokens--;

            auction.accession += auction.startingPrice * 0.1;

            TimeSpan diff = auction.closingDateTime - DateTime.Now;

            if(diff.TotalSeconds <= 10.0) {
                auction.closingDateTime = auction.closingDateTime.AddSeconds(10 - diff.TotalSeconds + 1);
            }

            Bid bid = new Bid() {
                userId = user.Id,
                user = user,
                auctionId = auction.id,
                auction = auction,
                bidTime = DateTime.Now
            };

            if(auction.biddingList == null) {
                auction.biddingList = new List<Bid>();
            }

            auction.biddingList.Add(bid);

            await this._context.bids.AddAsync(bid);
            this._context.auctions.Update(auction);
            this._context.Users.Update(user);

            await this._context.SaveChangesAsync();

            return Json(true);
        }

        //Gets called as a result of real-time change in auction state
        //Someone bids on auction, or changes it's state
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuction(int auctionId) {
            Auction auction = await this._context.auctions
                .Where(a => a.id == auctionId)
                .Include(u => u.user)
                .Include(a => a.biddingList)
                .ThenInclude(u => u.user)
                .FirstOrDefaultAsync();

            AuctionView data = await this.GetImageView(auction.imageId);

            if(auction.state != Auction.AuctionState.OPEN) {
                data.open = false;
            }
            else {
                TimeSpan diff = auction.closingDateTime - DateTime.Now;
                data.remainingTime = this.GetRemainingTime(diff);
                data.open = true;
            }

            auction.biddingList = auction.biddingList.OrderByDescending(b => b.bidTime).ToList();

            data.auction = auction;

            return PartialView("Auction", data);
        }
    }
}