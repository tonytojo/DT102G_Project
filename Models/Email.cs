using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
	public class Email
	{
		public int Id { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
	}
}
