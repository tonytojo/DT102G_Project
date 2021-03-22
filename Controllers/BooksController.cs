using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projekt_DT102G.Data;
using Projekt_DT102G.Models;
using X.PagedList;

namespace Projekt_DT102G.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public BooksController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index(string genre, int? genrePage)
        {
            int NrOfBooksOnPage = int.Parse(_config.GetValue<string>("NrOfBooksOnPage"));

            IEnumerable<Book> list = _context.Books.Where(p => genre == null || p.Genres.GenreName == genre)
                .ToList().ToPagedList(genrePage ?? 1, NrOfBooksOnPage);

            ViewBag.StockCount = _context.Books.Where(p => genre == null || p.Genres.GenreName == genre).Count();
            ViewBag.currentGenre = genre;
            return View(list);
        }

        public async Task<IActionResult> Search(string search)
        {
            var bookContent = _context.Books.Include(r => r.Genres).Where(x => x.Name.Contains(search));
            ViewBag.search = search;

            return View("Search", await bookContent.ToListAsync());
        }
    }
}
