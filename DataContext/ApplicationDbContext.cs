using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Npgsql;
using Prezentomat.Models;
using Prezentomat.Classes;
using System.Data.Entity.Validation;

namespace Prezentomat.DataContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext():base(nameOrConnectionString: "ApplicationDbContext")
        {

        }
        public virtual DbSet<UserClass> UserDetails { get; set; }

        public virtual DbSet<GatheringClass> GatheringDetails { get; set; }

        

    }
}