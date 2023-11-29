using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanShoppingCart;
namespace ShoppingCartRepositoryPattern
{
    public interface IRepository
    {
        List<Product> GetProducts();
        void AddProduct(Product product);
        void RemoveProduct(int Id);
    }
}
