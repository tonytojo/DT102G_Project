using Microsoft.AspNetCore.Mvc;
using Projekt_DT102G.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Components
{
    public class CartShortSummary : ViewComponent
    {
        private Cart cart;

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
