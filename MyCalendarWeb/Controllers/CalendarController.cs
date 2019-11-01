using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCalendarWeb.Models;


namespace MyCalendarWeb.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly MyCalendarWebContext _context;
        static string userId;
        static string userName;
        static string userEmail;

        public CalendarController(MyCalendarWebContext context)
        {
            _context = context;
            //userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            //userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName
            //userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's Email
        }

        // GET: Events
        public ActionResult Index()
        {
            var groupedEvents = _context.Events.Include(u => u.User).ToList()
                .GroupBy(e => (new DateTime(e.StartTime.Value.Year, e.StartTime.Value.Month, e.StartTime.Value.Day)).Date)
                .OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.ToList());
            return View(groupedEvents);
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var @event = _context.Events
                .Include(u => u.User)
                .FirstOrDefault(m => m.EventID == id);
            if(@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Username");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("EventID,Subject,Description,StartTime,EndTime,UserID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Username", @event.UserID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _context.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Username", @event.UserID);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("EventID,Subject,Description,StartTime,EndTime,UserID")] Event @event)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Username", @event.UserID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = _context.Events
                .Include(u => u.User)
                .FirstOrDefault(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var @event = _context.Events.Find(id);
            _context.Events.Remove(@event);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }
}