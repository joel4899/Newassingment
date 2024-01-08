using Domain.Models;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.DataContext
{
    public class AirlineDbContext: IdentityDbContext<CustomUser> //it gives you the ability to use the IdentityManagement module
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options)
           : base(options)
        {
        }

       

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<Flight>()
                .Property(f => f.WholesalePrice)
                .HasColumnType("decimal(18, 2)"); 
            modelBuilder.Entity<Ticket>()
                .Property(t => t.PricePaid)
                .HasColumnType("decimal(18, 2)");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
