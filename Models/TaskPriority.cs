using PanOpticon.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Models
{
    public enum TaskPriority
    {
        [PriorityCss("bg-dark")]
        Completed = 0,
        [Display(Name = "On Hold")]
        [PriorityCss("bg-info")]
        On_Hold,
        [Display(Name = "In Progress")]
        [PriorityCss("bg-light")]
        In_Progress,
        [PriorityCss("bg-light")]
        None,
        [PriorityCss("bg-primary")]
        Low,
        [PriorityCss("bg-secondary")]
        Medium,
        [PriorityCss("bg-warning")]
        High,
        [PriorityCss("bg-danger")]
        Critical

        
    }


    public static class TaskPriorityStyle
    {
        public static readonly Dictionary<TaskPriority, string> CSS = new()
        {
            { TaskPriority.Low,"link-primary"},
            { TaskPriority.Medium, "link-secondary" },
            { TaskPriority.High, "link-danger" },
            { TaskPriority.Critical, "link-info bg-danger" },
            { TaskPriority.Completed, "link-success"},
            {TaskPriority.In_Progress, "link-light" },
            { TaskPriority.On_Hold, "link-dark"}
        };
    }
     
}
