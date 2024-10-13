using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Context
{
    public class DataContext: DbContext
    {
        public DbSet<Admin> Admins { get; private set; }
        public DbSet<Vehicle> Vehicles { get; private set; }

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(new Admin { Id = 1, Email = "fulanodetal@gmail.com", Password = "123456", UserName="fulano"});
            base.OnModelCreating(modelBuilder);
        }
    }
}