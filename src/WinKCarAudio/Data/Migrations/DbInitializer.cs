using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinKCarAudio.Models.AssetViewModels;

namespace WinKCarAudio.Data.Migrations
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();

                // Look for any students.
                if (context.Asset.Any() || context.Category.Any() || context.Make.Any())
                {
                    return;   // DB has been seeded
                }

                var assets = new Asset[]
                {
                //new Asset{addDate = DateTime.Parse("2005-09-01"),description="This will only work above 15 degree C", name="Bobcat", price=50000},
                //new Asset{addDate = DateTime.Parse("2015-09-01"),description="This will only work above 20 degree C", name="Tow Truck",  price=110000},
                //new Asset{addDate = DateTime.Parse("2005-09-01"),description="This will only work above 15 degree C", name="Bobcat", price=50000}
                };

                var category = new Category[]
                {
                    new Category {CategoryName = "Dash Camera" },
                    new Category { CategoryName = "Speakers" },
                    new Category { CategoryName = "Subwofer" },
                    new Category { CategoryName = "Other" },
                };

                var make = new Make[]
               {
                    new Make {Name = "Sony"},
                    new Make {Name = "Pioneer"},
                    new Make {Name = "Kenwood"},
                    new Make {Name = "Other"}
           };

                foreach (Asset s in assets)
                {
                    context.Asset.Add(s);
                }
                foreach (Category s in category)
                {
                    context.Category.Add(s);
                }
               
                foreach (Make s in make)
                {
                    context.Make.Add(s);
                }
                context.SaveChanges();
            }
            catch { }
        }
    }
}
