using DAL.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OurDBContext : DbContext
    {
        public DbSet<bl_UserAccount> UserAccount { get; set; }
    }
}
