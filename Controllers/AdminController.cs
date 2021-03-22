using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projekt_DT102G.Data;
using Projekt_DT102G.Models;

namespace Projekt_DT102G.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment hostEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

       [Authorize(Roles = "Admin")]
        public IActionResult Index(int id)
        {
            var list = _context.Books.OrderBy(x => x.Genres.GenreName).ThenBy(x=>x.Name).Include(b => b.Genres).ToList();

            //If id != 0 then you have update this id. So we swap this element to be first 
            //in the list so that you can see that's ok in the view
            if (id != 0)
            {
                var index = list.FindIndex(x => x.BookId == id);
                var item = list[index];
                list[index] = list[0];
                list[0] = item;

                ViewData["id"] = id;
            }

            ViewBag.StockCount = list.Count;
            return View("Index", list);
        }

       [Authorize(Roles = "Admin")]
         public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    
       [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (vmodel.Photo != null)
				{
                   string uploadsDir = Path.Combine(hostEnvironment.WebRootPath, "image");
                   uniqueFileName = Guid.NewGuid().ToString() + "_" + vmodel.Photo.FileName;
                   string filepath = Path.Combine(uploadsDir, uniqueFileName);
                   vmodel.Photo.CopyTo(new FileStream(filepath, FileMode.Create));
                }

                Book newBook = new Book()
                {
                    Name= vmodel.Name,
                    Author = vmodel.Author,
                    Description = vmodel.Description,
                    Price = vmodel.Price,
                    GenreId = vmodel.GenreId,
                    Genres = vmodel.Genres,
                    PhotoPath = uniqueFileName
                };

                _context.Add(newBook);
                await _context.SaveChangesAsync();
                int Id = newBook.BookId;
                return RedirectToAction(nameof(Index), new { Id });
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            BookCreateViewModel model = new BookCreateViewModel()
            {
                 Name = book.Name,
                 Author=book.Author,
                 Description=book.Description,
                 Price=book.Price,
                 GenreId=book.GenreId,
                 Genres=book.Genres,
                 PhotoPath= book.PhotoPath
            };

            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", book.GenreId);
            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookCreateViewModel vmodel)
        {
            if (id != vmodel.Id)
                return NotFound();
            
            string uniqueFileName = null;
            if (vmodel.Photo != null)
            {
                string uploadsDir = Path.Combine(hostEnvironment.WebRootPath, "image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + vmodel.Photo.FileName;
                string filepath = Path.Combine(uploadsDir, uniqueFileName);

                using (var fs = new FileStream(filepath, FileMode.Create))
                {
                    vmodel.Photo.CopyTo(fs);
                }

                //remove old image file
                if (!string.IsNullOrEmpty(vmodel.PhotoPath))
                {
                    filepath = Path.Combine(uploadsDir, vmodel.PhotoPath);
                    System.IO.File.Delete(filepath);
                }
            }

            Book newBook = new Book()
            {
                BookId = vmodel.Id,
                Name = vmodel.Name,
                Author = vmodel.Author,
                Description = vmodel.Description,
                Price = vmodel.Price,
                GenreId = vmodel.GenreId,
                Genres = vmodel.Genres,
                PhotoPath = uniqueFileName
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(newBook.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id });
            }

            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", newBook.GenreId);
            return View(vmodel);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id, string PhotoPath)
        {
            if (PhotoPath != null)
            {
                string uploadsDir = Path.Combine(hostEnvironment.WebRootPath, "image");
                string filepath = Path.Combine(uploadsDir, PhotoPath);
                System.IO.File.Delete(filepath);
            }

            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
