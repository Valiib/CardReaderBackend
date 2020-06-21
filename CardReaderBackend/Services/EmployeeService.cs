using CardReaderBackend.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CardReaderBackend.Services
{
    public class EmployeeService
    {
        private IConfiguration _configuration;
        public EmployeeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal IDbConnection Connection {
            get { return new NpgsqlConnection(_configuration.GetConnectionString("DatabaseConnection")); }
        }


        public async Task<List<EmployeeModel>> GetAll()
        {
            var employeeList = new List<EmployeeModel>() { new EmployeeModel { } };
            return employeeList;
        }

        public async Task<EmployeeModel> Get()
        {
            return new EmployeeModel() { id = 1 };
        }
        public async Task<EmployeeModel> Create()
        {
            return new EmployeeModel { id = 1 };
        }
        public async Task<EmployeeModel> Update()
        {
            return new EmployeeModel { id = 1 };
        }
   
        public async Task<string> Delete()
        {
            return "Succesfuly deleted employee: 2";
        }
    }
}
