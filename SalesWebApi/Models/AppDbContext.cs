using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebApi.Controllers;

namespace SalesWebApi.Models
{
    public class AppDbContext : DbContext
    {

        //DbSet collections go here (or anywhere, but here is where greg puts them)
        //don't need default constructor like in library
        public virtual DbSet<Customer> Customers { get; set; }
    
        public virtual DbSet<Order> Orders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) { }

        public DbSet<SalesWebApi.Controllers.OrderLine> OrderLine { get; set; }

    }
}
