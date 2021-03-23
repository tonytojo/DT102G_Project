using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_DT102G.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt_DT102G.Data;
using Microsoft.EntityFrameworkCore;

namespace Projekt_DT102G.Controllers
{
	//This class represent the Cart where all the boks you have bought is stored
	//Because I use DI will the Cart be delivered as soon as I specified one
	public class CartController : Controller
	{
		private ApplicationDbContext _context;
		private Cart cart;

		//Ctor
		public CartController(ApplicationDbContext dbContext, Cart cartService)
		{
			_context = dbContext;
			cart = cartService;
		}


		public IActionResult Index(string currentUrl)
		{
			CartViewModel cartViewModel = new CartViewModel()
			{
				Cart = cart,
				CurrentUrl = currentUrl
			};

			return View(cartViewModel);
		}


		//Add a new boook to the Cart. Find the Id in db and then add item to order
		public RedirectToActionResult AddToCart(int bookId, string currentUrl)
		{
			Book book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
			if (book != null)
			{
				cart.AddOrderItem(book, 1);
			}

			return RedirectToAction("Index", new { currentUrl });
		}

		//Passed id is used to remove an Item from Cart
		public RedirectToActionResult RemoveFromCart(int bookId, string currentUrl)
		{
			Book book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
			if (book != null)
			{
				cart.RemoveOrderItem(book);
			}
			return RedirectToAction("Index", new { currentUrl });
		}
	}
}
