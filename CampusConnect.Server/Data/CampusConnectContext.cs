using CampusConnect.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Server.Data
{
    public class CampusConnectContext : DbContext
    {
        public CampusConnectContext()
        {

        }

        public CampusConnectContext(DbContextOptions<CampusConnectContext> options) : base(options) 
        { 

        }

        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<Degree> Degrees { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<LearnGroup> LearnGroups { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
    }
}
