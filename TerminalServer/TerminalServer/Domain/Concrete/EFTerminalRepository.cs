using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerminalServer.Domain.Abstract;
using TerminalServer.Domain.Entities;

namespace TerminalServer.Domain.Concrete
{
    public class EFTerminalRepository : ITerminalRepository
    {
        private readonly TerminalDbContext db;
        private static readonly object sync_obj = new object();
        public EFTerminalRepository(TerminalDbContext dbContext)
        {
            db = dbContext;
        }
        public IQueryable<Product> Products => db.Products;

        public void AddProduct(Product product)
        {
            db.Add(product);
            db.SaveChanges();
        }

        public Product FindProduct(string name)
        {
            return db.Products.Find(name);
        }

        public void RemoveProduct(Product product)
        {
            var foundProduct = db.Products.FirstOrDefault(p => p.Name == product.Name);
            db.Remove(foundProduct);
            db.SaveChanges();
        }

        public Product Update(Product product)
        {
            Product foundProduct = null;
            lock (sync_obj)
            {
                foundProduct = db.Products.FirstOrDefault(p => p.Name == product.Name);
                foundProduct.Amount += product.Amount;
                db.SaveChanges();
            }
            return foundProduct;
        }
    }
}
