using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionPortal.Models.View;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuctionPortal.Models.Database {
    public class User : IdentityUser {
        [Required]
        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Required]
        [Display(Name = "Number of tokens")]
        public int tokens { get; set; }
        
        [Display(Name = "Orders")]
        public ICollection<Order> orderList { get; set; }

        [Display(Name = "Auctions")]
        public ICollection<Auction> auctionList { get; set; }
        
        [Required]
        public bool active { get; set; }
    }

    public class UserProfile : Profile {
        public UserProfile() {
            base.CreateMap<RegisterModel, User> ()
                .ForMember(
                    destination => destination.Email,
                    options => options.MapFrom(data => data.email)
                )
                .ForMember(
                    destination => destination.UserName,
                    options => options.MapFrom(data => data.username)
                )
                .ReverseMap();
        }
    }
}