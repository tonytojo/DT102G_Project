using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_DT102G.Data;
using Projekt_DT102G.Models;

namespace Projekt_DT102G.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Cart cart;

        //Ctor
        public OrdersController(ApplicationDbContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;
        }

        //Is called on checkout. We pass the Cart and an empty form to fill in to the View
        public IActionResult Checkout()
        {
            OrderCartViewModel orderCartViewModel = new OrderCartViewModel();
            orderCartViewModel.Carts = cart;
            orderCartViewModel.Orders = new Order();

            return View(orderCartViewModel);
        }

        //Call finish to say thanks for the order
		// GET: Orders
		public IActionResult Index(int? id)
        {
            cart.ClearCart();
            return View("Finish");
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Carts,Orders")] OrderCartViewModel orderCartViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderCartViewModel.Orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = orderCartViewModel.Orders.OrderId });
            }

            return View(orderCartViewModel);
        }
    }
}
