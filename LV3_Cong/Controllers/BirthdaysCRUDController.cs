using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LV3_Cong.Data;
using LV3_Cong.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LV3_Cong.Controllers
{
    public class BirthdaysCRUD : Controller
    {
        private readonly AppDataBaseContext _appDataBaseContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BirthdaysCRUD(AppDataBaseContext appDataBaseContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDataBaseContext = appDataBaseContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _appDataBaseContext.Birthdays
                .OrderBy(b => b.Date.Month)
                .ThenBy(b => b.Date.Day)
                .ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> Upcoming(int days = 7)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var end = today.AddDays(days);
            var list = await _appDataBaseContext.Birthdays
                .Where(i =>
                    (i.Date.Month > today.Month

                    || (i.Date.Month == today.Month && i.Date.Day >= today.Day))

                    && (i.Date.Month < end.Month

                    || (i.Date.Month == end.Month && i.Date.Day <= end.Day))
                        )
                .OrderBy(i => i.Date.Month)
                .ThenBy(i => i.Date.Day)
                .ToListAsync();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Birthday birthday, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View(birthday);
            }
            if (formFile != null && formFile.Length > 0)
            {
                var upload = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(upload);

                var extention = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowed.Contains(extention))
                {
                    ModelState.AddModelError(nameof(formFile), "Wrong format");
                    return View(birthday);
                }
                var FileName = $"{Guid.NewGuid()}{extention}";
                var FilePath = Path.Combine(upload, FileName);
                await using var stream = new FileStream(FilePath, FileMode.Create);
                await formFile.CopyToAsync(stream);
                birthday.PhotoPath = $"/uploads/{FileName}";
            }
            _appDataBaseContext.Birthdays.Add(birthday);
            await _appDataBaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var birthady = await _appDataBaseContext.Birthdays.FindAsync(id);
            if (birthady == null)
            { 
                return NotFound(); 
            }
            return View(birthady);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Birthday model, IFormFile? formFile)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var entity = await _appDataBaseContext.Birthdays.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.FIO = model.FIO;
            entity.Date = model.Date;
            if (formFile != null && formFile.Length > 0)
            {
                var upload = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(upload);
                var extention = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowed.Contains(extention))
                {
                    ModelState.AddModelError(nameof(formFile), "Wrong format");
                    return View(model);
                }
                var FileName = $"{Guid.NewGuid()}{extention}";
                var FilePath = Path.Combine(upload, FileName);
                await using var stream = new FileStream(FilePath, FileMode.Create);
                await formFile.CopyToAsync(stream);
                entity.PhotoPath = $"/uploads/{FileName}";
            }
            await _appDataBaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var birthday = await _appDataBaseContext.Birthdays.FindAsync(id);
            if (birthday == null) return NotFound();
            return View(birthday);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birthday = await _appDataBaseContext.Birthdays.FindAsync(id);
            if (birthday != null)
            {
                _appDataBaseContext.Birthdays.Remove(birthday);
                await _appDataBaseContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
