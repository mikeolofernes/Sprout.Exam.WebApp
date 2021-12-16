using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Data
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options)
        {
        }

        public DbSet<EmployeeDto> Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDto>().ToTable("Employee");
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=DESKTOP-AGN8MLD\\SQLEXPRESS;Database=SproutExamDb;Integrated Security=True");
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SproutExamDb;User Id=sa;Password=amp123!;");
            }
        }
        
  
    }
}
