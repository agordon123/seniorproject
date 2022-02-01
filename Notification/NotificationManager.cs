using Hangfire;
using Hangfire.Server;
using Hangfire.Storage;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PanOpticon.Data;
using PanOpticon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = PanOpticon.Models.Task;

namespace PanOpticon.Notification
{
    public class NotificationManager : INotificationManager
    {

        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public NotificationManager(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
      
        public JobData GetJob(string id)
        {
            return JobStorage.Current.GetConnection().GetJobData(id);
        }

        public List<JobTask> GetJobTasks(Task task)
        {
            return _context.JobTasks.Include(p=>p.JobList).ThenInclude(p=>p.Task.PanopticonUser).Include(y=>y.JobList.Task.TaskNotificationSchedules).Where(t => t.JobList.Task == task).ToList();
            

        }

        public List<JobTask> GetJobTasksId(int taskid)
        {
            return GetJobTasks(_context.Tasks.Find(taskid));


        }

   

        public void SendNotification(string email, string subject, string htmlMessage, int taskid, string propertyname)
        {
            Debug.WriteLine("oh baby we sendin");
            var z = GetJobTasksId(taskid).Where(t => t.PropertyName == propertyname);
            var task = _context.Tasks.Include(t=>t.TaskNotificationSchedules).FirstOrDefault(t => t.Id == taskid);
            var shouldSend = task.TaskNotificationSchedules.GetNotificationProperty(propertyname).GetValue(task.TaskNotificationSchedules);
            var not = task.TaskNotificationSchedules.GetNotificationProperty("None").GetValue(task.TaskNotificationSchedules);
            if ((bool)shouldSend && (bool)not != true )
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(task.TaskNotificationSchedules.GetNotificationProperty(propertyname).Name);
                builder.AppendLine(shouldSend.ToString());

                _emailSender.SendEmailAsync(email, subject, htmlMessage + " <br/> " + builder.ToString() );
            }
        }
    }
}
