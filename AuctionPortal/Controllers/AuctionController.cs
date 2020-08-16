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

        public async Task<AuctionView> GetImageView(int index) {
            IList<Image> images = await this._context.images.ToListAsync();

            Image image = images.Where(i => i.id == index).FirstOrDefault();

            AuctionView model = new AuctionView() {
                base64Data = Convert.ToBase64String(image.data)
            };

            return model;
        }

        // GET: Auction
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var auctionPortalContext = _context.auctions;

            IList<Auction> list = await auctionPortalContext.ToListAsync();
            IList<AuctionView> auctionList = new List<AuctionView>();

            int i = 0;

            foreach(var el in list) {
                AuctionView data = await this.GetImageView(el.id);
                i++;
                data.auction = el;
                auctionList.Add(data);
            }

            SearchModel searchModel = new SearchModel() {
                auctionList = auctionList
            };

            return View(searchModel);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Index (SearchModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }

            if(model.name == "" && model.state ==  && model.minPrice == "" && model.maxPrice == "") {
                return View(model);
            }

            IList<AuctionView> auctionList = new List<AuctionView>();

            if(model.name != "") {
                foreach (var a in model.auctionList)
                {
                    if(a.auction.name == model.name) {
                        auctionList.Add(a);
                    }
                }
            }
        }*/

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
                Console.WriteLine("Empty list");
            }
            else {
                Console.WriteLine("Not empty list");
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

        public async Task startAuctions() {
            IList<Auction> auctions = await this._context.auctions.ToListAsync();

            DateTime now = DateTime.Now;

            foreach (var a in auctions)
            {
                if(DateTime.Compare(a.openingDateTime, now) <= 0) {
                    if(a.state == Auction.AuctionState.READY) {
                        a.state = Auction.AuctionState.OPEN;
                    }
                    if(a.state ==Auction.AuctionState.DRAFT) {
                        a.state = Auction.AuctionState.EXPIRED;
                    }
                }
            }
        }
    }
}
