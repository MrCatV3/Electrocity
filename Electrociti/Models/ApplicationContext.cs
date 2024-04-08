using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Electrociti.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString = "Data Source=DESKTOP-8ES4I02\\SQLEXPRESS;Database=ElectroCity;Integrated Security=sspi;Encrypt=False;TrustServerCertificate=true";

        public DbSet<Service> Service { get; set; }
        public DbSet<Employee> Employee2 { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Purchase> Purchases2 { get; set; }
        public DbSet<EmployeeService> EmployeeService { get; set; }
        public DbSet<CommentEmployee> CommentEmployees2 { get; set; }

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
            //Database.EnsureCreated();
        }
        
    }
}
