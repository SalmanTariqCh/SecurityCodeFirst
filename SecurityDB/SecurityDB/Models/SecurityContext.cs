using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecurityDB.Models
{
    public class SecurityContext : DbContext
    {
        public DbSet <User> Users { get; set; }
        public DbSet <Role> Roles { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Shift> Shifts { get; set; }

    }
}