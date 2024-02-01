using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Electrociti.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString = "Server=DESKTOP-8ES4I02;Database=ElectroCity;Integrated Security = sspi; Encrypt=False;";

        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<RoleEmployee> Roles { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<CommentEmployee> CommentEmployees { get; set; } 

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
            base.OnModelCreating(modelBuilder);
        }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
    }
}
