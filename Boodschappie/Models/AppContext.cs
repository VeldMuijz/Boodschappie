using Boodschappie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Boodschappie.Models
{
    public class AppContext : DbContext{
    
         public AppContext() : base("DefaultConnection") { }
         public DbSet<ItemList> ItemList { get; set; }
         public DbSet<Items> Items { get; set; }
         public DbSet<UserProfile> UserProfile { get; set; }
         public DbSet<SharedWith> SharedWith { get; set; }

    }
}