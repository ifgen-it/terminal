using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalClient.Entities
{
    class Product
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Name == product.Name &&
                   Amount == product.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Amount);
        }

        public override string ToString()
        {
            return $"Name={Name}, Amount={Amount}";
        }
        
    }
}
