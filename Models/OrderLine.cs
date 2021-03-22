using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
	public class OrderLine
	{
		public int OrderLineId { get; set; }
		public int Quantity { get; set; }
		public int UnitPrice { get; set; }


		//Navigation reference, Foreign key
		public virtual Book Books { get; set; }
		public int BookId { get; set; }

		//Navigation reference, Foreign key
		public virtual Order Orders { get; set; }
		public int OrderId { get; set; }
	}
}
