using CardReaderBackend.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CardReaderBackend.HubConfig
{
    public class EmployeeHub: Hub
    {
        protected readonly IConfiguration _configuration;
        private IMemoryCache _cache;
        private IHubContext<EmployeeHub> _hubContext;

        public EmployeeHub(IMemoryCache cache, IHubContext<EmployeeHub> hubContext, IConfiguration configuration) 
        {
            _configuration = configuration;
            _cache = cache;
            _hubContext = hubContext;
        }

        public async Task SendMessage()
        {
            if (!_cache.TryGetValue("employeeCache", out string response))
            {
                EmployeeListener employeeList = new EmployeeListener(_hubContext, _cache, _configuration);
                employeeList.ListenForAlarmNotifications();
                string jsonEmployeeAlarm = employeeList.GetAlarmList();
                _cache.Set("employeeCache", jsonEmployeeAlarm);
                await Clients.All.SendAsync("ReceiveMessage", _cache.Get("employeeCache").ToString());
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", _cache.Get("employeeCache").ToString());
            }
        }

    }
}
