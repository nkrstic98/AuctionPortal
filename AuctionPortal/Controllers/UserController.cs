using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionPortal.Models.Database;
using AuctionPortal.Models.View;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionPortal.Controllers {
    public class UserController : Controller {
        private AuctionPortalContext context;
        private UserManager<User> userManager;
        private IMapper mapper;
        private SignInManager<User> signInManager;

        public UserController(AuctionPortalContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager) {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }

        public IActionResult isEmailUnique(string email)
        {
            bool exists = this.context.Users.Where(user => user.Email == email).Any();

            if (exists)
            {
                return Json("Email already taken");
            }
            else
            {
                return Json(true);
            }
        }

        public IActionResult isUsernameUnique(string username)
        {
            bool exists = this.context.Users.Where(user => user.UserName == username).Any();

            if (exists)
            {
                return Json("Username already taken");
            }
            else
            {
                if(username.Length < 6) {
                    return Json("Username must be at least 6 characters long");
                }
                else {
                    return Json(true);
                }
            }
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register ( RegisterModel model ) {
            if ( !ModelState.IsValid ) {
                return View ( model );
            }

            User user = this.mapper.Map<User> ( model );
            user.active = true;
            
            IdentityResult result = await this.userManager.CreateAsync ( user, model.password );

            if ( !result.Succeeded ) {
                foreach ( IdentityError error in result.Errors ) {
                    ModelState.AddModelError ( "", error.Description );
                }

                return View ( model );
            }

            result = await this.userManager.AddToRoleAsync ( user, Roles.user.Name );

            if ( !result.Succeeded ) {
                foreach ( IdentityError error in result.Errors ) {
                    ModelState.AddModelError ( "", error.Description );
                }

                return View ( model );
            }

            return RedirectToAction ( nameof ( UserController.Login ) );
        }

        public IActionResult Login(string returnUrl) {
            LoginModel model = new LoginModel() {
                returnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }

            var result = await this.signInManager.PasswordSignInAsync(model.username, model.password, false, false);

            if(!result.Succeeded) {
                ModelState.AddModelError("", "Username or password not valid!");
                return View(model);
            }

            if(model.returnUrl != null) {
                return Redirect(model.returnUrl);
            }

            User user = this.context.Users.Where(user => user.UserName == model.username).FirstOrDefault();

            if(user.active == false) {
                ModelState.AddModelError("", "Unfortunatelly, your account is suspended");
                return View();
            }

            IList<string> roles = await this.userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if(role == "User") {
                    return RedirectToAction ( nameof ( AuctionController.Index ), "Auction" );
                }
                else {
                    if(role == "Administrator") {
                        return RedirectToAction ( nameof ( AuctionController.ManageAuctions ), "Auction" );
                    }
                }
            }

            return RedirectToAction ( nameof ( UserController.Login ));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await this.signInManager.SignOutAsync();
            return RedirectToAction ( nameof ( AuctionController.Index ), "Auction" );
        }

        public async Task<IActionResult> AccDetails() {
            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            return View(loggedInUser);
        }

        public async Task<IActionResult> ChangeDetails() {
            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            RegisterModel registerModel = new RegisterModel() {
                firstName = loggedInUser.firstName,
                lastName = loggedInUser.lastName,
                gender = loggedInUser.gender,
                email = loggedInUser.Email,
                username = loggedInUser.UserName,
                password = "",
                confirmPassword = ""
            };

            return View(registerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeDetails ( RegisterModel model ) {
            if ( !ModelState.IsValid ) {
                return View ( model );
            }

            User user = await userManager.FindByEmailAsync(model.email);

            if(user != null) {
                user.firstName = model.firstName;
                user.lastName = model.lastName;
                user.gender = model.gender;
                user.Email = model.email;
                user.UserName = model.username;

                IdentityResult result = await this.userManager.UpdateAsync ( user );

                if ( !result.Succeeded ) {
                    foreach ( IdentityError error in result.Errors ) {
                        ModelState.AddModelError ( "", error.Description );
                    }

                    return View ( model );
                }
            }

            return RedirectToAction ( nameof ( UserController.AccDetails ) );
        }

        public async Task<IActionResult> ManageUsers() {
            var userContext = this.context.Users;

            IList<User> users = await userContext.ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> DeleteUser(string id) {
            if (ModelState.IsValid) {
                User user = this.context.Users.Where(user => user.Id == id).First();

                user.active = false;

                //odraditi brisanje svih aukcija koje je korisnik napravio

                IList<Auction> userAuctions = await this.context.auctions.Where(uid => user.Id == uid.userId).ToListAsync();

                foreach (var item in userAuctions)
                {
                    item.state = Auction.AuctionState.DELETED;
                    this.context.Update(item);
                    await this.context.SaveChangesAsync();
                }

                IdentityResult result = await this.userManager.UpdateAsync(user);

                if(!result.Succeeded) {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return RedirectToAction ( nameof ( UserController.ManageUsers ) );
        }

        public async Task<IActionResult> PurchaseTokens() {
            IList<Token> tokens = await this.context.tokens.ToListAsync();

            return View(tokens);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseTokens(Purchase model) {
            if(!ModelState.IsValid) {
                return View(model);
            }

            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            Token token = this.context.tokens.Where(t => t.ammount == model.amount).FirstOrDefault();

            DateTime today = DateTime.Now;

            Order order = new Order() {
                userId = loggedInUser.Id,
                user = loggedInUser,
                date = today,
                tokenAmmout = model.amount,
                price = token.price
            };

            if(loggedInUser.orderList == null) {
                loggedInUser.orderList = new List<Order>();
            }

            loggedInUser.orderList.Add(order);

            loggedInUser.tokens += model.amount;

            this.context.Users.Update(loggedInUser);
            this.context.orders.Update(order);
            await this.context.SaveChangesAsync();

            OrderConfirm confirm = new OrderConfirm() {
                order = order,
                newAmount = loggedInUser.tokens
            };

            return PartialView("OrderConfirmation", confirm);
        }
    }
}