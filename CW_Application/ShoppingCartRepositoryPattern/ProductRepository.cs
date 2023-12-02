using CleanShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ShoppingCartRepositoryPattern
{
    public class ProductRepository : IRepository
    {
        string _filePath;
        XmlRepository _repository;
        public ProductRepository(string filePath)
        {
            _filePath = filePath;
            _repository = new XmlRepository(filePath);
        }

        public void AddProduct(Product product)
        {
            var products = GetProducts();
            products.Add(product);
            _repository.SaveProducts(products);
        }

        public List<Product> GetProducts()
        {
            return _repository.LoadProducts();
        }

        public void RemoveProduct(Product product)
        {
            var products = GetProducts(); // Create a new list based on the original list
            products.Remove(products.FirstOrDefault(p => p.ProductId == product.ProductId));
            for (int i = 0; i < products.Count; i++)
            {
                products[i].ProductId = i + 1;
            }
            _repository.SaveProducts(products);
        }
    }
}
