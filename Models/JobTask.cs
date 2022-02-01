using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PanOpticon.Models
{
    public class JobTask
    {
        public int Id { get; set; }
        public string JobId { get; set; }
        public string PropertyName { get; set; }
        public JobList JobList { get; set; }

    }


    public class JobList
    { 
    public int Id { get; set; }
    public Task Task { get; set; }
    }

}
