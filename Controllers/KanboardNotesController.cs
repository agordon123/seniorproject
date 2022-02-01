using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PanOpticon.Data;
using PanOpticon.Models;
using PanOpticon.UserRoles;

namespace PanOpticon.Controllers
{
    public class KanboardNotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PanopticonUser> _userManager;
        public KanboardNotesController(ApplicationDbContext context, UserManager<PanopticonUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: KanboardNotes
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Todo)); //View(await _context.KanboardNote.ToListAsync());
        }

        // GET: panopticon.ilus.space/KanboardNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kanboardNote = await _context.KanboardNote
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kanboardNote == null)
            {
                return NotFound();
            }

            return View(kanboardNote);
        }

        // GET: KanboardNotes/Search
        public async Task<IActionResult> Search()
        {
            return View();
        }

        // POST: KanboardNotes/SearchResult
        public async Task<IActionResult> SearchResult(String SearchPhrase)
        {
            return View("Index", await _context.KanboardNote.Where(j => j.Note.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: KanboardNotes/Create

    

        [Authorize]
        public IActionResult Create()
        {


            var list = _context.Todos.Include(p => p.PanopticonUser).Where(t => t.PanopticonUser.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToList();
            ViewBag.List = new SelectList(list, "Id", "Name");
            return View();
        }

        // POST: KanboardNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Note,Title,Todo")] KanboardNote kanboardNote)
        {

           // var list = _context.Todos.Include(p => p.PanopticonUser).Where(t => t.PanopticonUser.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToList();
          //  ViewBag.List = new SelectList(list, "Id", "Name");

            if (ModelState.IsValid)
            {
                kanboardNote.PanopticonUser = await _userManager.GetUserAsync(HttpContext.User);

                var x = _context.Todos.Include(p => p.PanopticonUser).FirstOrDefault(i => i.Id == kanboardNote.Todo.Id);

                kanboardNote.Todo = x;
               
                _context.Add(kanboardNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kanboardNote);
        }

        // GET: KanboardNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

       

            var kanboardNote = _context.KanboardNote.Include(p=>p.PanopticonUser).FirstOrDefault(t => t.Id == id);


            if (kanboardNote == null)
            {
                return NotFound();
            }

            if (kanboardNote.PanopticonUser.Id != _userManager.GetUserAsync(HttpContext.User).Result.Id)
                return Forbid();

            var list = _context.Todos.Include(p => p.PanopticonUser).Where(t => t.PanopticonUser.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToList();
            ViewBag.List = new SelectList(list, "Id", "Name");

            return View(kanboardNote);
        }

        // POST: KanboardNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Note,Title, Todo")] KanboardNote kanboardNote)
        {
            if (id != kanboardNote.Id)
            {
                return NotFound();
            }


            kanboardNote.PanopticonUser = await _userManager.GetUserAsync(HttpContext.User);
            if (kanboardNote.PanopticonUser.Id != _userManager.GetUserAsync(HttpContext.User).Result.Id)
            {
                return Forbid();
            }

            var list = _context.Todos.Include(p => p.PanopticonUser).Where(t => t.PanopticonUser.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToList();
            ViewBag.List = new SelectList(list, "Id", "Name");

            if (ModelState.IsValid)
            {
                try
                {

                    var x = _context.Todos.Include(p => p.PanopticonUser).FirstOrDefault(i => i.Id == kanboardNote.Todo.Id);

                    kanboardNote.Todo = x;

                    _context.Update(kanboardNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KanboardNoteExists(kanboardNote.Id))
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
            return View(kanboardNote);
        }

        // GET: KanboardNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kanboardNote = await _context.KanboardNote
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kanboardNote == null)
            {
                return NotFound();
            }

            return View(kanboardNote);
        }

        // POST: KanboardNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kanboardNote = await _context.KanboardNote.FindAsync(id);
            _context.KanboardNote.Remove(kanboardNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KanboardNoteExists(int id)
        {
            return _context.KanboardNote.Any(e => e.Id == id);
        }

// create a new ActionResult for the PanOpticon.Models.Todo class that will return all the Todo objects including the KanboardNote objects.
[Authorize]
        public async Task<IActionResult> Todo()
        {
            return View(await _context.Todos.Include(t => t.KanboardNotes).Include(p=>p.PanopticonUser).Where(p=>p.PanopticonUser.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToListAsync());
        }




        

        // create a new Get and Post method for KanboardNotes that creates a new PanOpticon.Models.Todo object and adds it to the Todo table in the database.
        // GET: KanboardNotes/CreateTodo
        [Authorize]
        public IActionResult CreateTodo()
        {
            return View();
        }

        // POST: KanboardNotes/CreateTodo
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTodo([Bind("Id,Name")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                todo.PanopticonUser = await _userManager.GetUserAsync(HttpContext.User);
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Todo));
            }
            return View(todo);
        }
       
       


   


    }
  
}
