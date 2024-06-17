using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Electrociti.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString = "Data Source=DESKTOP-4PAD45N\\SQLEXPRESS;Database=ElectroCity;Integrated Security=sspi;Encrypt=False;TrustServerCertificate=true";
        //private readonly string _connectionString = "Data Source=DESKTOP-545KIBV\\SQLEXPRESS;Database=ElectroCity;Integrated Security=sspi;Encrypt=False;TrustServerCertificate=true";

        public DbSet<Service> Service { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<EmployeeService> EmployeeService { get; set; }
        public DbSet<CommentEmployee> CommentEmployees { get; set; }
        public DbSet<EmployeeWork> EmployeeWork { get; set; }
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
