using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.DashboardDto
{
    public class OverViewChartDto
    {
        public string? ProductName { get; set; }
        public int EmployeeCount { get; set; }
        public int CustomerCount { get; set; }
        public int ProductRevenue {get;set;}
            
    }
}