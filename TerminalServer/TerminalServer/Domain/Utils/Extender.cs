using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerminalServer.Domain.Concrete;
using TerminalServer.Domain.Entities;

namespace TerminalServer.Domain.Utils
{
    public static class Extender
    {
        public static void InsertDataIntoDb(this TerminalDbContext db)
        {
            // Products
            Product p1 = new Product { Name = "bicycle", Amount = 3 };
            Product p2 = new Product { Name = "computer", Amount = 1 };
            Product p3 = new Product { Name = "laptop", Amount = 8 };
            Product p4 = new Product { Name = "tv", Amount = 7 };
            Product p5 = new Product { Name = "car", Amount = 2 };
            Product p6 = new Product { Name = "ball", Amount = 12 };
            
            db.Products.AddRange(p1, p2, p3, p4, p5, p6);
            db.SaveChanges();
        }
    }
}
