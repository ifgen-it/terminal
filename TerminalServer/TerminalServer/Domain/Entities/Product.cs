using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TerminalServer.Domain.Entities
{
    public class Product
    {
        [Key]
        [MaxLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"Name={Name}, Amount={Amount}";
        }
    }
}
