
using Microsoft.AspNetCore.Mvc;
using Projekt_DT102G.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Components
{
    //This class is called from layout and it's responsible
    //for passing cart to the View
    public class CartShortSummary : ViewComponent
    {
        private Cart cart;

        //Cart is dependent on DI
        //We create dependency handling for Cart in Startup
        public CartShortSummary(Cart cart)
        {
            this.cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
