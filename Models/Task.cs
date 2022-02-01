using PanOpticon.Attributes;
using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PanOpticon.Attributes;
using System.Reflection;

namespace PanOpticon.Models
{
    public class Task
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public PanopticonUser PanopticonUser { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Task Name Required")]
        public string TaskName { get; set; }


        // MaxLength(280)
        [Display(Name = "Description")]

        public string? TaskDescription { get; set; }


        //int 
        [Display(Name = "Priority")]

        public TaskPriority TaskPriority { get; set; } = TaskPriority.None;

        [Display(Name = "Due Date")]
        [Required(ErrorMessage = "Task Due Date Required")]
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }

        public DateTime? CompletedDate { get; set; }
        // ? means nullable
        public string? TaskCss { get; set; }
        public TaskNotificationSchedule TaskNotificationSchedules { get; set; }


    }

    public static class TaskExtension
        {

        public static string GetPriorityBadge(this TaskPriority taskPriority)
        {
            var type = taskPriority.GetType();
            var name = Enum.GetName(type, taskPriority);
            var attrib = type.GetField(name).GetCustomAttributes(false).OfType<PriorityCss>().FirstOrDefault();

            return attrib.CustomBadgeCSS ?? "";

        }

        public static DisplayAttribute GetDisplayName(this TaskPriority taskPriority)
        {
            var type = taskPriority.GetType();
            var name = Enum.GetName(type, taskPriority);
            var attrib = type.GetField(name).GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();

            return attrib ?? new DisplayAttribute();

        }

        public static PropertyInfo GetNotificationProperty(this TaskNotificationSchedule taskNotification, string property)
        {
            var type = taskNotification.GetType();
            PropertyInfo prop = type.GetProperty(property);

            return prop;
        }

        public static TimeOffset GetNotificationTimeOffset(this TaskNotificationSchedule taskNotification, string property)
        {
            var type = taskNotification.GetType();
            TimeOffset timeOffset = taskNotification.GetNotificationProperty(property).GetCustomAttributes(false).OfType<TimeOffset>().SingleOrDefault();

            if(timeOffset == null)
            {
                return new TimeOffset(0);
            }

            return timeOffset;
        }

    }

    
    public class TaskNotificationSchedule
    { 
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      //  public virtual Task Task { get; set; }

        [Description("Thirty minutes before")]
        [Display(Name ="Thirty Minutes Before")]
        [TimeOffset(30)]
        public bool Thirty { get; set; } = false;
        [Display(Name = "One Hour Before")]
        [TimeOffset(60)]
        public bool HourBefore { get; set; } = false;
        [Display(Name = "No Notification")]
        public bool None { get; set; } = true;

        [Display(Name = "Fifteen Minutes Before")]
        [TimeOffset(15)]
        public bool Fifteen { get; set; } = false;

        [Display(Name = "One Day Before")]
        [TimeOffset(1440)]
        public bool OneDay { get; set; } = false;

        [Display(Name = "Five Hours Before")]
        [TimeOffset(300)]
        public bool FiveHours { get; set; } = false;

        public int TaskId { get; set; }

        public Task Task { get; set; }
    }



    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TimeOffset : Attribute
    {
        public int offset {get;set; }
        public TimeOffset(int offset)
        {
            this.offset = offset;
        }

        public int getOffset()
        {
            return offset;
        }

    }


}
