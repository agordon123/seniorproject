using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Models
{
    public class MetricsViewModel
    {
        public int CompletedTasksCount { get; set; }

        public int TotalTasksCount { get; set; }

        public double CompletedTasksPercentage { get; set; }

        public int TotalTimeDays { get; set; }

        public int TotalTimeHours { get; set; }

        public int TotalTimeMinutes { get; set; }

        public int AverageTimeDays { get; set; }

        public int AverageTimeHours { get; set; }

        public int AverageTimeMinutes { get; set; }
    }
}
