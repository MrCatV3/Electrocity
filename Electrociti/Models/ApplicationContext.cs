using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Electrociti.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString = "";

        public DbSet<Service> Services { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<CommentMaster> MasterComments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CommentClient> ClientComments { get; set; }
        public DbSet<Check> Checks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }


    }
}
