using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerminalServer.Domain.Entities;

namespace TerminalServer.Domain.Abstract
{
    public interface ITerminalRepository
    {
        IQueryable<Product> Products { get; }
        void AddProduct(Product product);
        void RemoveProduct(Product product);
        Product Update(Product product);
        Product FindProduct(string name);
    }
}
