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

namespace Exam.Controllers
{
    public class EventLogsController : Controller
    {
        private readonly LoggerDbContext _context;

        public EventLogsController(LoggerDbContext context)
        {
            _context = context;
        }

        // GET: EventLogs
        [AuthorizeByRoles(RoleEnum.admin)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventLog.ToListAsync());
        }

        // GET: EventLogs/Details/5
        [AuthorizeByRoles(RoleEnum.admin)]
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
        // GET: EventLogs/Delete/5
        [AuthorizeByRoles(RoleEnum.admin)]
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

        // POST: EventLogs/Delete/5
        [AuthorizeByRoles(RoleEnum.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventLog = await _context.EventLog.FindAsync(id);
            _context.EventLog.Remove(eventLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeByRoles(RoleEnum.admin)]
        private bool EventLogExists(int id)
        {
            return _context.EventLog.Any(e => e.id == id);
        }
    }
}
