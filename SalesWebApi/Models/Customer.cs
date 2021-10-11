using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebApi.Models
{
    public class Customer
    {

        public int Id { get; set; }
        [StringLength(30), Required]
        public String Name { get; set; }
        [Column(TypeName = "decimal(11,2)")]
        public decimal Sales { get; set; }

        public Customer() { }
    }
}
