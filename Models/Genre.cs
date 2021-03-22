using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
	public class Genre
	{
		public int GenreId { get; set; }

		[Required(ErrorMessage = "Please enter a category name")]
		[Display(Name="Category Name")]
		public string GenreName { get; set; }

		//Navigation collection
		public virtual ICollection<Book> Books { get; set; }
	}
}
