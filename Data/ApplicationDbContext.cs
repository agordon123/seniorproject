using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PanOpticon.Models;
using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanOpticon.Data
{
    public class ApplicationDbContext : IdentityDbContext<PanopticonUser, IdentityRole, string>
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskNotificationSchedule> TaskNotificationSchedules { get; set; }
        public DbSet<KanboardNote> KanboardNote { get; set; }
        public DbSet<JobList> JobLists { get; set; }
        public DbSet<JobTask> JobTasks { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<FileUpload> Files { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Task>().HasOne(b => b.TaskNotificationSchedules).WithOne(i => i.Task).HasForeignKey<TaskNotificationSchedule>(b => b.TaskId);
            base.OnModelCreating(builder);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
         
     }
  
    }



}
