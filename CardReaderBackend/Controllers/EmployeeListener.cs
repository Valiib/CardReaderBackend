using CardReaderBackend.HubConfig;
using CardReaderBackend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace CardReaderBackend.Controllers
{
    public class EmployeeListener
    {
        private readonly IConfiguration _configuration;
        private IHubContext<EmployeeHub> _hubContext;
        private IMemoryCache _cache;
        public EmployeeListener(IHubContext<EmployeeHub> hubContext, IMemoryCache cache, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _cache = cache;
            _configuration = configuration;
        }

        public void ListenForAlarmNotifications()
        {
          
            NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            conn.StateChange += Conn_StateChange;
            conn.Open();
            var listenCommand = conn.CreateCommand();
            listenCommand.CommandText = $"listen employee_changes;";
            listenCommand.ExecuteNonQuery();
            conn.Notification += PostgresNotificationRecieved;
            _hubContext.Clients.All.SendAsync(this.GetAlarmList());
            while (true)
            {
                conn.Wait();
            }
        }

        private void PostgresNotificationRecieved(object sender, NpgsqlNotificationEventArgs e)
        {
            string actionName = e.Payload.ToString();
            string actionType = "";

            if (actionName.Contains("DELETE"))
            {
                actionType = "Delete";
            }
            if (actionName.Contains("UPDATE"))
            {
                actionType = "Update";
            }
            if (actionName.Contains("INSERT"))
            {
                actionType = "Insert";
            }
            _hubContext.Clients.All.SendAsync("ReceiveMessage", this.GetAlarmList());
        }

        public string GetAlarmList()
        {
            var employeeList = new List<EmployeeModel>();
            using (NpgsqlCommand sqlCmd = new NpgsqlCommand())
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "select * from \"Employees\"";
     
                NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
                NpgsqlCommand customCMD = new NpgsqlCommand("SELECT * FROM \"Employee\" ORDER BY \"created\" DESC", conn);
                conn.Open();
                using (NpgsqlDataReader reader = customCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EmployeeModel model = new EmployeeModel();
                        model.id = reader.GetInt64(0);
                        model.Name = reader.GetString(1);
                        model.Surname = reader.GetString(2);
                        model.Created = reader.GetDateTime(3);
                      
                        employeeList.Add(model);
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            _cache.Set("employeeCache", SerializeObjectToJson(employeeList));
            return _cache.Get("employeeCache").ToString();
        }

        public String SerializeObjectToJson(Object alarmspeed)
        {
            try
            {
                var jss = new JavaScriptSerializer();
                return jss.Serialize(alarmspeed);
            }
            catch (Exception) { return null; }
        }


        private void Conn_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {

            _hubContext.Clients.All.SendAsync("Current State: " + e.CurrentState.ToString()
                + " Original State: " + e.OriginalState.ToString(), "connection state changed");
        }
    }
}
