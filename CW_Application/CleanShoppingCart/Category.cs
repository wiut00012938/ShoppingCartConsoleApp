using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanShoppingCart
{
    public class Category 
    {
        private static int categoryCounter = 1;

        public int CategoryId { get; }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Category's name can not be null here");
                }
                name = value;
            }
        }
        public List<Product> Products { get; set; }
        public Category(string name)
        {
            CategoryId = categoryCounter++;
            Name = name;
            Products = new List<Product>();
        }
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

    }
}
