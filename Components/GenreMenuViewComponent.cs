using Microsoft.AspNetCore.Mvc;
using Projekt_DT102G.Data;
using Projekt_DT102G.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Components
{
    public class GenreMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public GenreMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public IViewComponentResult Invoke(bool horizontalmenu)
        {
            ViewBag.SelectedGenre = RouteData?.Values["genre"];
            
            NavGenre navGenre = new NavGenre()
            {
                Genre = _context.Books
                .Select(x => x.Genres.GenreName)
                .Distinct()
                .OrderBy(x => x),
                Home = string.IsNullOrEmpty(ViewBag.SelectedGenre) ? true : false
            };

            if (horizontalmenu)
               return View("HorizontalMenu", navGenre);
            else
               return View(navGenre);
        }
    }
}
