using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerminalServer.Domain.Entities;
using TerminalServer.Domain.Utils;

namespace TerminalServer.Domain.Concrete
{
    public partial class TerminalDbContext : DbContext
    {
        private static int counter;
        public TerminalDbContext()
        {
            if (counter == 0)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
                this.InsertDataIntoDb();
            }
            ++counter;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TerminalDb;Trusted_connection=TRUE");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
