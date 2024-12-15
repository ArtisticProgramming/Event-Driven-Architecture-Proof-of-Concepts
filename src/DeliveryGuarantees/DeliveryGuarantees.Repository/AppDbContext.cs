using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryGuarantees.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<OutBoxMessage> OutBoxMessages { get; set; }
        public DbSet<InBoxMessage> InBoxMessages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BoxDb");
        }
    }
}
