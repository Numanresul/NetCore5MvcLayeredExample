using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FirsatBul.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FirsatBul.DataAccess
{
    public class FirsatBulIdentityDbContext: IdentityDbContext<AppUser>
    {
        //public FirsatBulIdentityDbContext(DbContextOptions<FirsatBulIdentityDbContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=FirsatBulNLayered;UserName=postgres;Password=123456");
        }

    }
}
