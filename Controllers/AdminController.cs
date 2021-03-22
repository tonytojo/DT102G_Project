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
    //Only the person with role attribute is Authorize for action methods in this class
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostEnvironment;

        //Ctor
        //ApplicationDbContext is the channel to the database and IWebHostEnvironment is used for
        //have a link to wwwroot
        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        //Get all books sorted on category and then by name include category
   //    [Authorize(Roles = "Admin")]
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

            //Get the number of books in stock
            ViewBag.StockCount = list.Count;
            return View("Index", list);
        }

        //Get the book with the specified id to be updated
        //Return NotFound if the book can't be found
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
    

        //We call this method so the user can input data for a new Book in the View
       [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName");
            return View();
        }

        //This method is called when the user has input all data for a new book
        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      // [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateViewModel vmodel)
        {
            //Check if the data is valid. If the user has input an image make sure that this image
            //is added to wwwroot
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

                //Prepare the Book object with data from input. This Book object will be
                //added to thé database
                Book newBook = new Book()
                {
                    Name= vmodel.Name,
                    Author = vmodel.Author,
                    Description = vmodel.Description,
                    Price = vmodel.Price,
                    GenreId = vmodel.GenreId,
                    Genres = vmodel.Genres,
                    Alt = vmodel.Alt,
                    PhotoPath = uniqueFileName
                };

                _context.Add(newBook);
                await _context.SaveChangesAsync();

                //Call index to refresh the screen so we can see the new book
                //We pass the new id to Index methos so the new bok will be listed first in the
                //List of books
                int Id = newBook.BookId;
                return RedirectToAction(nameof(Index), new { Id });
            }

            //Here we have invalid data so pass back this data to the View
            return View();
        }

        //Here we have Edit so we can update a Book 
        //We pass in the id to update and this id is fetched from database
     //   [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            //We pass down the book to be updated
            BookCreateViewModel model = new BookCreateViewModel()
            {
                 Name = book.Name,
                 Author=book.Author,
                 Description=book.Description,
                 Price=book.Price,
                 GenreId=book.GenreId,
                 Genres=book.Genres,
                 Alt = book.Alt,
                 PhotoPath= book.PhotoPath
            };

            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", book.GenreId);
            return View(model);
        }

        //This method is called when we have updated the book and clicked on save
        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      //  [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookCreateViewModel vmodel)
        {
            if (id != vmodel.Id)
                return NotFound();
            
            //If we have a Photo for this Book make sure you add or update this image to wwwroot.
            //If there is an update for an image we remove the old imge first
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

            //Prepare the book object with new data from the update
            Book newBook = new Book()
            {
                BookId = vmodel.Id,
                Name = vmodel.Name,
                Author = vmodel.Author,
                Description = vmodel.Description,
                Price = vmodel.Price,
                GenreId = vmodel.GenreId,
                Genres = vmodel.Genres,
                Alt = vmodel.Alt,
                PhotoPath = uniqueFileName
            };

            //Check if the data is valid
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
                //We pass the id to the Indexd action method so this upadted book will be first in the list of books
                return RedirectToAction(nameof(Index), new { id });
            }

            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", newBook.GenreId);
            return View(vmodel);
        }

        //We get the id from the database from the passed id in the argument list
        // GET: Books/Delete/5
      //  [Authorize(Roles = "Admin")]
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

        //When we have clicked remove for a book in the View we call this delete method
        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id, string PhotoPath)
        {
            //Check if we have an image path. If we have remove the image in wwwroot
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
