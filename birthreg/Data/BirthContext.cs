using birthreg.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Data
{ 
    public class BirthContext : IdentityDbContext<User>
    {
        public BirthContext(DbContextOptions<BirthContext> option) : base(option)
        {

        }
        public new DbSet<User> Registrars { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Parent> Parents { get; set; }
    }
}
