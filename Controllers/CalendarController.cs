using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PanOpticon.Data;
using PanOpticon.Models;
using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon.Controllers
{

    public class CalendarController : Controller
    {
        private UserManager<PanopticonUser> _userManager;

        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext dbContext, UserManager<PanopticonUser> manager)
        {
            _context = dbContext;
            _userManager = manager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult CalendarPartial(int month = 1, int year = 2021)

        {
            List<Models.Task> tasks = _context.Tasks.Select(s => s).Where(m => m.DueDate.Month == month && m.PanopticonUser == _userManager.GetUserAsync(HttpContext.User).Result).ToList<Models.Task>();


            if (month >= 13)
            {
                Debug.WriteLine("error: greater than 12");
                month = 1;
                year = year + 1;
            }
            if (month <= 0)
            {
                Debug.WriteLine("error: less  than 1");
                month = 12;
                year = year - 1;
            }
            Debug.WriteLine($"{month}, {year}");
            return PartialView("_Calendar", new CalendarPartialViewModel { Month = month, Year = year, Tasks = tasks });
        }

        public IActionResult Boop()
        {
            return View();
        }
    }
}
