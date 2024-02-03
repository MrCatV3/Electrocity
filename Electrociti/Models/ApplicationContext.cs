using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Electrociti.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString = "Data Source=DESKTOP-8ES4I02\\SQLEXPRESS;Database=ElectroCity;Integrated Security=sspi;Encrypt=False;TrustServerCertificate=true";

        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<RoleEmployee> Roles { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<CommentEmployee> CommentEmployees { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        
    }
}
