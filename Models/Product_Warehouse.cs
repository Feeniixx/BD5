using System;
using System.ComponentModel.DataAnnotations;

namespace CW_5.Models
{
    public class Product_Warehouse
    {
        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdWarehouse { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "give a number greater than zero")]
        public int Amount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return "IdProduct: " + IdProduct + "\n" +
                   "IdWarehouse: " + IdWarehouse + "\n" +
                   "Amount: " + Amount + "\n" +
                   "CreatedAt: " + CreatedAt + "\n";
        }
    }
}