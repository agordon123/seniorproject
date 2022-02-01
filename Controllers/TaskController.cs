using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PanOpticon.Data;
using PanOpticon.Email;
using PanOpticon.Models;
using PanOpticon.Notification;
using PanOpticon.UserRoles;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PanOpticon.Controllers
{
    public class TaskController : Controller
    {
        // GET: TaskNotificationController

         private readonly ApplicationDbContext _context;
        private readonly UserManager<PanopticonUser> _userManager;
        private readonly IBackgroundJobClient jobClient;
        private readonly IEmailSender email;
        private readonly ISmsSender sender;
        private readonly INotificationManager _notificationManager;
         
        public TaskController(ApplicationDbContext context, 
            UserManager<PanopticonUser> userManager, 
            IEmailSender emailSender, 
            IBackgroundJobClient backgroundJobClient, 
            ISmsSender _sender,
            INotificationManager notificationManager)
        {
            _context = context;
            _userManager = userManager;
            email = emailSender;
            jobClient = backgroundJobClient;
            sender = _sender;
            _notificationManager = notificationManager;
        }


        [Authorize]

        public async Task<ActionResult> Index()
        {
 
            List<TaskViewModel> taskViewModels = new List<TaskViewModel>();

              _context.Tasks.Include(p => p.PanopticonUser).Include(p=>p.TaskNotificationSchedules).Where(m => m.PanopticonUser == _userManager.GetUserAsync(HttpContext.User).Result).ToList().ForEach(
                x =>
                {
                    taskViewModels.Add(new TaskViewModel { Task = x, TaskNotificationSchedule = x.TaskNotificationSchedules });
                }
                );

            return View(taskViewModels);
        }

        // GET: TaskNotificationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaskNotificationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskNotificationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(TaskViewModel taskViewModel)
        {


            if(ModelState.IsValid)
            {
                Debug.WriteLine(taskViewModel.Task.TaskName);
                taskViewModel.Task.PanopticonUser = await _userManager.GetUserAsync(HttpContext.User);
                taskViewModel.Task.CreationDate = DateTime.Now;
                taskViewModel.TaskNotificationSchedule.Task = taskViewModel.Task;
                _context.Add(taskViewModel.Task);
                _context.Add(taskViewModel.TaskNotificationSchedule);
                  _context.SaveChanges();

                
                JobList jobList = new JobList { Task = taskViewModel.Task };
                
                _context.Add(jobList);

                typeof(TaskNotificationSchedule).GetProperties().Where(x => x.PropertyType == typeof(bool)).ToList().ForEach( z => 
                {
                    PropertyInfo x = taskViewModel.TaskNotificationSchedule.GetNotificationProperty(z.Name);
                         var offset = (taskViewModel.TaskNotificationSchedule.GetNotificationTimeOffset(z.Name)).getOffset();
                    

                    if( offset != 0)
                    {
                        #region Debuggies
                        Debug.WriteLine("offset");
                        Debug.WriteLine(offset);
                        Debug.WriteLine(TimeSpan.FromMinutes(offset));
                        Debug.WriteLine(taskViewModel.Task.DueDate.ToString());
                        Debug.WriteLine(taskViewModel.Task.DueDate.Subtract(TimeSpan.FromMinutes(offset)));

                        #endregion

                        var job =  jobClient.Schedule(() => _notificationManager.SendNotification(
                            taskViewModel.Task.PanopticonUser.Email, 
                            taskViewModel.Task.TaskName,
                            taskViewModel.Task.TaskDescription,
                            taskViewModel.Task.Id,
                            x.Name), taskViewModel.Task.DueDate.Subtract(TimeSpan.FromMinutes(offset)));
                    JobTask jobTask = new JobTask { JobId = job, PropertyName = x.Name, JobList = jobList };

                        _context.Add(jobTask);
                    }
                        
                  
                    
                     _context.SaveChanges();
                 
                });

                #region  booger
                //  var y =  taskViewModel.TaskNotificationSchedule.GetType().GetProperties();

                //var y = typeof(Models.TaskNotificationSchedule).GetProperties();
                //if((bool)typeof(Models.TaskNotificationSchedule).GetProperty("None").GetValue(taskViewModel.TaskNotificationSchedule,null) == true )
                //{

                //    // jobClient.Schedule(() => 
                //    //, ))));

                //    Debug.WriteLine("THEY selected none, no notifications");

                //    return RedirectToAction(nameof(Index));
                //}
                //typeof(TaskNotificationSchedule)
                //    .GetProperties()
                //    .ToList()
                //    .ForEach(z => 
                //    {

                //        typeof(TaskNotificationSchedule).GetProperties().ToList().ForEach(p =>
                //        {
                //            if (p.Name != z.Name)
                //                return;
                //            if (p.PropertyType != typeof(bool))
                //                return;

                //            if ( z.GetCustomAttributes(false).OfType<TimeOffset>().SingleOrDefault() == null )
                //            {
                //                return;
                //            }

                //            if((bool)p.GetValue(taskViewModel.TaskNotificationSchedule,null) == false)
                //            {
                //                return;
                //            }
                //            Debug.WriteLine("name of property {0}", p.Name);
                //            Debug.WriteLine("value of property{0}", p.GetValue(taskViewModel.TaskNotificationSchedule));
                //            Debug.WriteLine("How many minutes from now");
                //            Debug.WriteLine(taskViewModel.Task.DueDate.Subtract(TimeSpan.FromMinutes((z.GetCustomAttributes(false).OfType<TimeOffset>().SingleOrDefault().offset))));

                //           jobClient.Schedule(() => email.SendEmailAsync(taskViewModel.Task.PanopticonUser.Email, taskViewModel.Task.TaskName, taskViewModel.Task.TaskDescription), taskViewModel.Task.DueDate.Subtract(TimeSpan.FromMinutes((z.GetCustomAttributes(false).OfType<TimeOffset>().SingleOrDefault().offset))));
                //            if(_userManager.GetUserAsync(HttpContext.User).Result.CellProvider == CellProvider.Tmobile)
                //            {

                //            //    //sendSMSAsync(taskViewModel.Task.PanopticonUser.PhoneNumber);
                //                jobClient.Schedule(() =>  sender.SendSMS(taskViewModel.Task.PanopticonUser.PhoneNumber, taskViewModel.Task.TaskDescription)   , taskViewModel.Task.DueDate.Subtract(TimeSpan.FromMinutes((z.GetCustomAttributes(false).OfType<TimeOffset>().SingleOrDefault().offset))));

                //            }
                //        });
                //    });

                #endregion

                return RedirectToAction(nameof(Index));

            }
            return View(taskViewModel);
        }


        public ActionResult Test(int? id)
       {
            var task = _context.Tasks.Include(t => t.TaskNotificationSchedules).FirstOrDefault(t => t.Id == id);
            var z = _notificationManager.GetJobTasks(task);
            
            var noti = task.TaskNotificationSchedules;

            typeof(TaskNotificationSchedule).GetProperties().Where(x => x.PropertyType == typeof(bool)).ToList().ForEach(z =>
            {
                if (z.Name == "None")
                    return;
                PropertyInfo x = noti.GetNotificationProperty(z.Name);
                Debug.WriteLine("Notification info: ");
                Debug.WriteLine(x.Name);
                _notificationManager.GetJobTasksId(task.Id).ForEach(q => {
                    Debug.WriteLine(q.PropertyName);
                    Debug.WriteLine(noti.GetNotificationProperty(q.PropertyName).GetValue(noti));
                    var z = JobStorage.Current.GetConnection().GetJobData(q.JobId);
                    Debug.WriteLine(z);
                    Debug.WriteLine(z.State);
                    foreach (var arg in z.Job.Args)
                    {
                        Debug.WriteLine("args");
                        Debug.WriteLine(arg.ToString());
                    }
                    
                  
                });
                //Debug.WriteLine(g.Count);
                //Debug.WriteLine(g.Id);
            });

                
            //Debug.WriteLine(z);

            return Content( "" );
        }
    

        // GET: TaskNotificationController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var task = _context.Tasks.Include(p => p.PanopticonUser).FirstOrDefault(t => t.Id == id);
            var tasknotif = _context.TaskNotificationSchedules.Include(t => t.Task).Include(p => p.Task.PanopticonUser).FirstOrDefault(t => t.TaskId == task.Id);
            if (task == null || tasknotif == null)
                return NotFound();

            if (task.PanopticonUser.Id != _userManager.GetUserAsync(HttpContext.User).Result.Id)
                return Forbid();

           

                return View(new TaskViewModel{ Task = task, TaskNotificationSchedule = tasknotif });
        }

        // POST: TaskNotificationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, TaskViewModel taskViewModel)
        {
            if(id != taskViewModel.Task.Id)
            {
                return NotFound();
            }

            taskViewModel.Task.PanopticonUser = await _userManager.GetUserAsync(HttpContext.User);
            taskViewModel.Task.TaskNotificationSchedules = taskViewModel.TaskNotificationSchedule;
            if (taskViewModel.Task.PanopticonUser.Id != _userManager.GetUserAsync(HttpContext.User).Result.Id)
            {
                return Forbid();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Tasks.Update(taskViewModel.Task);
                    _context.TaskNotificationSchedules.Update(taskViewModel.TaskNotificationSchedule);
                   // _context.Entry(taskViewModel.Task).Property(u => u.TaskNotificationSchedules).IsModified = false;
                   // _context.Entry(taskViewModel.TaskNotificationSchedule).Property(u => u.Task).IsModified = false;
                    await _context.SaveChangesAsync();

                   var jobs = _context.JobTasks.Where(j => j.JobList == _context.JobLists.FirstOrDefault(t => t.Task.Id == id)).ToList();
                     jobs.ForEach(z =>
                    {
                      BackgroundJob.Delete(z.JobId);

                        
                    });

                    jobs.ForEach(g =>
                    {
                        PropertyInfo x = taskViewModel.TaskNotificationSchedule.GetNotificationProperty(g.PropertyName);
                        var offset = (taskViewModel.TaskNotificationSchedule.GetNotificationTimeOffset(g.PropertyName)).getOffset();

                        var job = jobClient.Schedule(() => _notificationManager.SendNotification(
                          taskViewModel.Task.PanopticonUser.Email,
                          taskViewModel.Task.TaskName,
                          taskViewModel.Task.TaskDescription,
                          taskViewModel.Task.Id,
                          g.PropertyName), taskViewModel.Task.DueDate.Subtract(TimeSpan.FromMinutes(offset)));

                        g.JobId = job;

                    });

                    await _context.SaveChangesAsync();





                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(taskViewModel.Task.Id) || !TaskNotificationExists(taskViewModel.TaskNotificationSchedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: TaskNotificationController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var task = _context.Tasks.Include(t=>t.TaskNotificationSchedules).FirstOrDefault(t => t.Id == id);
          
            TaskViewModel vm = new TaskViewModel { Task = task, TaskNotificationSchedule = task.TaskNotificationSchedules };



            return View(vm);
        }

        // POST: TaskNotificationController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var task = await _context.Tasks.Include(t => t.TaskNotificationSchedules).FirstOrDefaultAsync(t => t.Id == id);
            var taskNotif = await _context.TaskNotificationSchedules.FindAsync(task.TaskNotificationSchedules.Id);
            var joblist = await _context.JobLists.FirstOrDefaultAsync(j => j.Task.Id == task.Id);
            if (joblist != null)
            {
                var jobs = await _context.JobTasks.Where(j => j.JobList.Id == joblist.Id).ToListAsync();
                jobs.ForEach(i => jobClient.Delete(i.JobId));

                jobs.ForEach(j => _context.JobTasks.Remove(j));

                _context.JobLists.Remove(joblist);
            }
            _context.TaskNotificationSchedules.Remove(taskNotif);
            _context.Tasks.Remove(task);
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));



        }

        [HttpPost]
        public async Task<ActionResult> Complete(int id)
        {
            // Marks task as complete by setting Priority to Completed and CompletedDate to now
            var task = await _context.Tasks.FindAsync(id);
            task.TaskPriority = TaskPriority.Completed;
            task.CompletedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }

        private bool TaskNotificationExists(int id)
        {
            return _context.TaskNotificationSchedules.Any(e => e.Id == id);
        }

        // Return view model for metrics
        [HttpGet]
        public async Task<IActionResult> Metrics()
        {

            //List<MetricsViewModel> metricsViewModel = new List<MetricsViewModel>();

            var tasks = await _context.Tasks.Where(t => t.PanopticonUser == _userManager.GetUserAsync(HttpContext.User).Result).ToListAsync();
            var completedTasks = tasks.Where(t => t.TaskPriority == TaskPriority.Completed).ToList();
            var completedTasksCount = completedTasks.Count();
            var totalTasksCount = tasks.Count();
            double completedTasksPercentage = 0;
            double newTotal = totalTasksCount;
            double newCompleted = completedTasksCount;
            if (totalTasksCount > 0)
            {
                completedTasksPercentage = (newCompleted / newTotal) * 100;
            } else if (totalTasksCount == 0)
            {
                completedTasksPercentage = 0;
            }
            
            // Calculates the total time between creation and completion of tasks
            var totalTime = new TimeSpan();
            completedTasks.ForEach(t =>
            {
                var creationDate = t.CreationDate.TimeOfDay;


                var completedDate = ((DateTime)t.CompletedDate).TimeOfDay;
                totalTime += completedDate.Subtract(creationDate);
            });

            // Calculates the average time between creation and completion of tasks
            var averageTime = new TimeSpan();
            if (completedTasksCount > 0)
            {
                averageTime = totalTime.Divide(completedTasksCount);
            } else if (completedTasksCount == 0)
            {
                averageTime = new TimeSpan(0);
            }

            var metricsViewModel = new MetricsViewModel
            {
                CompletedTasksCount = completedTasksCount,
                TotalTasksCount = totalTasksCount,
                CompletedTasksPercentage = Math.Round(completedTasksPercentage, 1),
                TotalTimeDays = totalTime.Days,
                TotalTimeHours = totalTime.Hours,
                TotalTimeMinutes = totalTime.Minutes,
                AverageTimeDays = averageTime.Days,
                AverageTimeHours = averageTime.Hours,
                AverageTimeMinutes = averageTime.Minutes
            };


            return View("Metrics",metricsViewModel);
        }


    }
}


