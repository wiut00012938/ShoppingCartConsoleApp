using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanShoppingCart
{
    public class Product
    {
        private string? name;
        private double price;
        public int ProductId { get; set; }
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Name of a product can't be null or empty ");
                }
                name = value;
            }
        }
        public double Price
        {
            get => price;
            set
            {
                if(value< 0)
                {
                    throw new ArgumentException("Product price can't be negative");
                }
                price = value;
            }
        }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
        public Product()
        {

        }
    }
}
