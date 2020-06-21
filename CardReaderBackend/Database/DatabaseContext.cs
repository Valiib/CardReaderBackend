using CardReaderBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardReaderBackend.Database
{
    public class DatabaseContext: DbContext
    {
        protected readonly IConfiguration _configuration;
        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection"));
        }
        public DbSet<EmployeeModel> Employees { get; set; }

      
    }
}
    