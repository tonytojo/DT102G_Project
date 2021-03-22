using Microsoft.AspNetCore.Mvc;
using Projekt_DT102G.Data;
using Projekt_DT102G.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Components
{
    //This class is called from layout and it's responsible
    //for filter out all the category that exist
    public class GenreMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public GenreMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        //If the argument horizontalmenu is true we call another View to handle screen < 768
        public IViewComponentResult Invoke(bool horizontalmenu)
        {
            ViewBag.SelectedGenre = RouteData?.Values["genre"];
            
            //Fiter out out all the categories that we have
            NavGenre navGenre = new NavGenre()
            {
                Genre = _context.Books
                .Select(x => x.Genres.GenreName)
                .Distinct()
                .OrderBy(x => x),
                Home = string.IsNullOrEmpty(ViewBag.SelectedGenre) ? true : false
            };

            //If horizontalmenu is true we have screen < 768 so we call a View to handle that
            //otherwise we call Default View to handele screen > 768
            if (horizontalmenu)
               return View("HorizontalMenu", navGenre);
            else
               return View(navGenre);
        }
    }
}
