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

namespace AuctionPortal.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly AuctionPortalContext _context;
        private UserManager<User> userManager;
        private Timer timer;

        public AuctionController(AuctionPortalContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;

            this.setTimer();
        }

        private void setTimer() {
            timer = new Timer(60 * 1000);
            timer.Elapsed += startAuctions;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public async Task<AuctionView> GetImageView(int index) {
            IList<Image> images = await this._context.images.ToListAsync();

            Image image = images.Where(i => i.id == index).FirstOrDefault();

            AuctionView model = new AuctionView() {
                base64Data = Convert.ToBase64String(image.data)
            };

            return model;
        }

        public string GetRemainingTime(TimeSpan diff) {
            double seconds = Math.Floor((diff.TotalMilliseconds / 1000) % 60);
            double minutes = Math.Floor((diff.TotalMilliseconds / (1000 * 60)) % 60);
            double hours = Math.Floor((diff.TotalMilliseconds / (1000 * 60 * 60)));

            string h = (hours < 10) ? "0" + hours.ToString() : hours.ToString();
            string m = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
            string s = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();
            
            return h + ":" + m + ":" + s;
        }

        // GET: Auction
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var auctionPortalContext = this._context.auctions;

            IList<Auction> list = await auctionPortalContext.OrderByDescending(a => a.creationDateTime).ToListAsync();
            IList<AuctionView> auctionList = new List<AuctionView>();

            foreach (var el in list)
            {

                AuctionView data = await this.GetImageView(el.imageId);

                if(DateTime.Compare(el.closingDateTime, DateTime.Now) < 0) {
                    data.remainingTime = "00:00:00";
                    await this.closeAuction(el.id);
                }
                else {
                    TimeSpan diff = el.closingDateTime - DateTime.Now;
                    data.remainingTime = this.GetRemainingTime(diff);
                }

                data.auction = el;
                
                auctionList.Add(data);
            }

            SearchModel searchModel = new SearchModel() {
                auctionList = auctionList
            };

            return View(searchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Search(SearchModel model)
        {
            IQueryable<Auction> query = this._context.auctions;

            if(model.name != null) {
                query = query.Where(auction => auction.name == model.name);
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

            IList<Auction> auctions = await query.OrderByDescending(auction => auction.openingDateTime).ToListAsync();

            IList<AuctionView> auctionViews = new List<AuctionView>();

            foreach (Auction a in auctions)
            {
                AuctionView av = await this.GetImageView(a.imageId);

                if(DateTime.Compare(a.closingDateTime, DateTime.Now) < 0) {
                    av.remainingTime = "00:00:00";
                    await this.closeAuction(a.id);
                }
                else {
                    TimeSpan diff = a.closingDateTime - DateTime.Now;
                    av.remainingTime = this.GetRemainingTime(diff);
                }

                av.auction = a;

                auctionViews.Add(av);
            }

            return PartialView("AuctionList", auctionViews);
        }

        // GET: Auction/AdminDetails/5
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.auctions.FirstOrDefaultAsync(m => m.id == id);

            if (auction == null)
            {
                return NotFound();
            }

            AuctionView auctionView = await this.GetImageView(auction.imageId);
            auctionView.auction = auction;

            return View(auctionView);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.auctions.FirstOrDefaultAsync(m => m.id == id);

            if (auction == null)
            {
                return NotFound();
            }

            AuctionView auctionView = await this.GetImageView(auction.imageId);
            auctionView.auction = auction;

            return View(auctionView);
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

            IList<Auction> auctions = await this._context.auctions.Where(a => a.name == model.name).ToListAsync();

            if(auctions != null) {
                foreach (Auction a in auctions)
                {
                    if(a.state != Auction.AuctionState.DELETED)  {
                        ModelState.AddModelError("", "Auction with that name already exists");
                        return View(model);
                    }
                }
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
                user = user,
                biddingList = null
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
                    Console.WriteLine("Fajl nije null");
                    Console.WriteLine(model.file);

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

        public async Task<IActionResult> ManageAuctions()
        {
            var auctionPortalContext = _context.auctions;
            IList<Auction> list = await auctionPortalContext.ToListAsync();

            return View(list);
        }

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

        public async Task<IActionResult> MyAuctions() {
            User user = await this.userManager.GetUserAsync(base.User);

            if(user == null) {
                return RedirectToAction(nameof(UserController.Login), "User");
            }

            IList<Auction> auctions = await this._context.auctions.Where(u => u.userId == user.Id).ToListAsync();

            user = this._context.Users.Where(u => u.Id == user.Id).Include(a => a.auctionList).FirstOrDefault();

            return View(user);
        }

        public void startAuctions(object sender, ElapsedEventArgs e) {
            Console.WriteLine("Auction update" + e.SignalTime);

            IList<Auction> auctions = this._context.auctions.ToList();

            DateTime now = DateTime.Now;

            foreach (var a in auctions)
            {
                if(DateTime.Compare(a.openingDateTime, now) <= 0) {
                    if(a.state == Auction.AuctionState.READY) {
                        a.state = Auction.AuctionState.OPEN;
                        this._context.auctions.Update(a);
                    }
                    if(a.state ==Auction.AuctionState.DRAFT) {
                        a.state = Auction.AuctionState.EXPIRED;
                        this._context.auctions.Update(a);
                    }
                }
            }

            _context.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task closeAuction(int auctionId) {
            Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == auctionId);

            if(auction.accession != 0) {
                auction.state = Auction.AuctionState.SOLD;
            }
            else {
                auction.state = Auction.AuctionState.EXPIRED;
            }

            this._context.auctions.Update(auction);
            this._context.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bid(int auctionId) {
            Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == auctionId);
            User user = await this.userManager.GetUserAsync(base.User);

            if(user.tokens == 0) {
                return Json("You do not have tokens, so you cannot bid on this auction");
            }

            user.tokens--;

            auction.accession += auction.startingPrice * 0.1;

            TimeSpan diff = DateTime.Now - auction.closingDateTime;
            if(diff.TotalSeconds <= 10) auction.closingDateTime.AddSeconds(10 - diff.TotalSeconds);

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

        [HttpGet]
        public async Task<IActionResult> GetAuction(int auctionId) {
            Auction auction = await this._context.auctions.FirstOrDefaultAsync(a => a.id == auctionId);

            AuctionView data = await this.GetImageView(auction.imageId);

            if(DateTime.Compare(auction.closingDateTime, DateTime.Now) < 0) {
                data.remainingTime = "00:00:00";
                await this.closeAuction(auction.id);
            }
            else {
                TimeSpan diff = auction.closingDateTime - DateTime.Now;
                data.remainingTime = this.GetRemainingTime(diff);
            }

            data.auction = auction;

            return PartialView("Auction", data);
        }
    }
}