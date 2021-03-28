using FirsatBul.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace FirsatBul.DataAccess
{
    public class FirsatBulDbContext:DbContext
    {
        //public FirsatBulDbContext(DbContextOptions<FirsatBulDbContext> options) : base(options)
        //{

        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=FirsatBulNLayered;UserName=postgres;Password=123456");
        }
        public DbSet<Firsatlar> Firsatlar { get; set; }
    }
}
