using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
	public class Cart
	{
		public List<OrderItem> OrderItems = new List<OrderItem>();

		public virtual void AddOrderItem(Book book, int quantity)
		{
			OrderItem orderItem = OrderItems.Where(b => b.Book.BookId == book.BookId).FirstOrDefault();

			if (orderItem == null)
				OrderItems.Add(new OrderItem() { Book = book, Quantity = quantity });
			else
				orderItem.Quantity++;
		}

		public virtual void RemoveOrderItem(Book book)
		{
			OrderItems.RemoveAll(b => b.Book.BookId == book.BookId);
		}

		public decimal ComputeOrderValue()
		{
			return OrderItems.Sum(b => b.Book.Price * b.Quantity);
		}
		public virtual void ClearCart()
		{
			OrderItems.Clear();
		}
	}
}
