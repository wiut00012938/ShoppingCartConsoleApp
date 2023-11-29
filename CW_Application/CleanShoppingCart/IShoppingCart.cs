using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanShoppingCart
{
    public interface IShoppingCart
    {
        void AddProduct(Product product);
        void RemoveProduct(Product product);
        void AddCategory(Category category);
        List<Category> GetCategories();
        double TotalCost { get; }
    }
}
