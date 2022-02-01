using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Models
{
    public class CalendarPartialViewModel
    {
      public int Month { get; set; }
      public int Year { get; set; }
        public List<Models.Task> Tasks { get; set; }
    }
}
