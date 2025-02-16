using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using NotesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesWebApp.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var notes = _context.Notes.Where(n => n.UserId == userId && n.IsArchived == false).ToList();
            return View(notes);
        }

        public ActionResult Archived()
        {
            var userId = User.Identity.GetUserId();
            var notes = _context.Notes.Where(n => n.UserId == userId && n.IsArchived == true).ToList();
            return View(notes);
        }

        [HttpPost]
        public ActionResult ToggleArchive(int? id)
        {
            var userId = User.Identity.GetUserId();
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return HttpNotFound();

            note.IsArchived = !note.IsArchived;
            _context.SaveChanges();
            return RedirectToAction("Archived");
        }

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Content))
            {
                ModelState.AddModelError("Content", "Content is required.");
                return View(note);
            }

            var userId = User.Identity.GetUserId();
            note.UserId = userId;
            note.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(note.Title))
            {
                note.Title = $"Untitled - {_context.Notes.Count() + 1}";
            }

            _context.Notes.Add(note);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return HttpNotFound();

            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            var userId = User.Identity.GetUserId();
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == note.Id && n.UserId == userId);
            if (existingNote == null) return HttpNotFound();

            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.Color = note.Color;
            _context.Entry(existingNote).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return HttpNotFound();

            return View(note);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return HttpNotFound();

            _context.Notes.Remove(note);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}