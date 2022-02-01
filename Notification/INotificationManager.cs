using Hangfire;
using Hangfire.Server;
using Hangfire.Storage;
using PanOpticon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Notification
{
  public interface INotificationManager
    {
        JobData GetJob(string id);
        List<JobTask> GetJobTasks(Models.Task task);

        List<JobTask> GetJobTasksId(int taskid);
        void SendNotification(string email, string subject, string htmlMessage, int taskid, string propertyname);

    };
}
