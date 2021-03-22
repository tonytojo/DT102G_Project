using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter adress")]
        public string Adress { get; set; }
      
        [MaxLength(12)]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only numbers ex. 1965061463762")]
        [Required(ErrorMessage = "Please Enter SSN number")]
        public string SSN { get; set; }

        //Navigation collection
        public ICollection<OrderLine> OrderLines { get; set; }
    }
}
