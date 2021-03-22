using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
	public class Book
	{
        public int BookId { get; set; }

		[Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a author name")]
        public string Author { get; set; }


		[Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Price { get; set; }

        public string PhotoPath { get; set; }

        //Navigation collection
        public virtual ICollection<OrderLine> OrderLines { get; set; }

        //Navigation reference, Foreign key
        public virtual Genre Genres { get; set; }
        public int GenreId { get; set; }
    }
}
