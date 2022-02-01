using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PanOpticon.Data;
using PanOpticon.Email;
using PanOpticon.Models;
using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender email;
        private readonly UserManager<PanopticonUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient jobClient;
        
        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, ApplicationDbContext context, IBackgroundJobClient backgroundJobClient, UserManager<PanopticonUser> userManager)
        {
            _logger = logger;
            email = emailSender;
            _context = context;
            jobClient = backgroundJobClient;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_userManager.GetUserId(HttpContext.User) != null)
            {
                var x = await _context.Tasks.Where(m => m.PanopticonUser == _userManager.GetUserAsync(HttpContext.User).Result).ToListAsync();
                if (x.Count > 0)
                    return View(x);
            }



            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Job()
        {
            
           // var x = jobClient.Schedule(() => Debug.WriteLine("woah"), TimeSpan.FromMinutes(2));
            Debug.WriteLine("woah");
            return View("Index");
        }
        
  



     // panopticon.ilus.space/Home/Mail
        public IActionResult Mail()
        {
            try
            {
               
                email.SendEmailAsync("notjohnbrandt@gmail.com", "test", "herllo");
            }
            catch(Exception)
            {
                
            }
            return View("Index");
        }

        public IActionResult FAQ()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
