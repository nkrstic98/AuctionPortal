using AuctionPortal.Models.Database;
using Microsoft.AspNetCore.Identity;

namespace AuctionPortal.Initializers {
    public class UserInitializer {
        public static string[][] users = new string[][] {
            new string[] { "Nikola","Krstic","Male","niki","krsta@gmail.com","nikolica98", "10" },
            new string[] { "Filip","Lukovic","Male","fica","kackac777@gmail.com","kackac777", "10" },
            new string[] { "Marko","Hajdukov","Male","hajduk","hajduk@gmail.com","hajduk123", "10" },
            new string[] { "Ivan","Rakonjac","Male","ivica","irak98@gmail.com","ivica998", "10" },
            new string[] { "Nikola","Milenovic","Male","romaleRomali","rom@gmail.com","romale123", "10" },
            new string[] { "Vladimir","Kundakovic","Male","vladica","kundak@gmail.com","vladica123", "10" },
            new string[] { "Aleksandar","Putica","Male","dupica","sale@gmail.com","sale12345", "10" },
            new string[] { "Jovan","Milosev","Male","jokisa","jokisa@gmail.com","jokisa123", "10" },
            new string[] { "Marko","Balcakovic","Male","balcak","balcak@gmail.com","balcak98", "10" },
            new string[] { "Luka","Matovic","Male","deteSveta","ml170722d@gmail.com","matke123", "10" },
            new string[] { "Katarina","Raic","Female","ketySaY","kety@gmail.com","ketysay123", "10" },
            new string[] { "Milica","Tucovic","Female","comi","comi@gmail.com","milica123", "10" },
            new string[] { "Ljubinka","Vasic","Female","bubily","buba@gmail.com","buby12345", "10" },
            new string[] { "Lara","Vukovic","Female","lakica","mackice@gmail.com","lakica123", "10" },
            new string[] { "Damir","Savic","Male","dakisa","daki@gmail.com","dakisa123", "10" },
            new string[] { "Aleksa","Zivkovic","Other","akiKamenjar","aki@gmail.com","aleksa123", "10" }
        };

        private static string[] administrator = new string [] {
            "Admin","Admin","Other","admin","admin@microsoft.com","admin12345", "0"
        };

        private static bool addUser ( string[] row, UserManager<User> userManager, string role ) {
            string firstName = row[0];
            string lastName = row[1];
            string gender = row[2];
            string username = row[3];
            string email = row[4];
            string password = row[5];
            int tokens = int.Parse(row[6]);

            if ( userManager.FindByNameAsync ( username ).Result != null ) {
                return false;
            }

            User user = new User ( ) {
                firstName = firstName,
                lastName = lastName,
                gender = gender,
                UserName = username,
                Email = email,
                tokens = tokens,
                active = true
            };

            IdentityResult result = userManager.CreateAsync ( user, password ).Result;
            if ( !result.Succeeded ) {
                return false;
            }

            result = userManager.AddToRoleAsync ( user, role ).Result;

            if ( !result.Succeeded ) {
                return false;
            }

            return true;
        }

        public static void initialize  ( UserManager<User> userManager ) {
            foreach ( string[] row in UserInitializer.users ) {
                bool result = addUser ( row, userManager, Roles.user.Name );
                if ( !result ) {
                    return;
                }
            }

            addUser ( UserInitializer.administrator, userManager, Roles.administrator.Name );
        }
    }
}