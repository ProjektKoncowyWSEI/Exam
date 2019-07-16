using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Logger.Data;
using Logger.Model;
using Helpers;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin)]
    public class EventLogsController : Controller
    {
        private readonly LoggerDbContext _context;

        public EventLogsController(LoggerDbContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventLog.OrderByDescending(o=>o.CreatedTime).ToArrayAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLog = await _context.EventLog
                .FirstOrDefaultAsync(m => m.id == id);
            if (eventLog == null)
            {
                return NotFound();
            }

            return View(eventLog);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLog = await _context.EventLog
                .FirstOrDefaultAsync(m => m.id == id);
            if (eventLog == null)
            {
                return NotFound();
            }

            return View(eventLog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventLog = await _context.EventLog.FindAsync(id);
            _context.EventLog.Remove(eventLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       
        private bool EventLogExists(int id)
        {
            return _context.EventLog.Any(e => e.id == id);
        }
    }
}
