using ShoppingCartRepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace CleanShoppingCart
{
    public class ShoppingCart: IShoppingCart
    {
        private static int ProductCounter = 1;
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public double TotalCost { get { return Products.Sum(p => p.Price); } }
        //private XmlRepository _repository;
        private IRepository _repository;

        /*public ShoppingCart(XmlRepository repository)
        {
            this._repository = repository;
            Products = repository.LoadProducts();
            Categories = new List<Category>();
        }*/
        /*public void AddProduct(Product product)
        {
            product.ProductId = ProductCounter++;
            Products.Add(product);
            _repository.SaveProducts(Products);
        }*/
        /*public void RemoveProduct(Product product)
        {
            Products.Remove(product);
            _repository.SaveProducts(Products);
        }*/

        public ShoppingCart(IRepository repository)
        {
            this._repository = repository;
            Products = repository.GetProducts();
            Categories = new List<Category>();
            if(Products.Count != 0)
            {
                ProductCounter = Products.Last().ProductId + 1;
            }
        }
        public void AddProduct(Product product)
        {
            product.ProductId = ProductCounter;
            ProductCounter++;
            Products.Add(product);
            _repository.AddProduct(product);
        }
        public void RemoveProduct(Product product)
        {
            _repository.RemoveProduct(product);
            Products = _repository.GetProducts();
        }
        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }
        public List<Category> GetCategories()
        {
            return Categories;
        }
        public List<Product> GetProductsByCategoryName(string name)
        {
            var NededCategory = Categories.Where(c => c.Name.ToLower() == name).FirstOrDefault();
            if(NededCategory == null)
            {
                throw new Exception("No Category with entered Name");
            }
            return NededCategory.Products;
        }
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            var NededCategory = Categories.Where(c => c.CategoryId == categoryId).FirstOrDefault();
            if (NededCategory == null)
            {
                throw new Exception("No Category with entered Name");
            }
            return NededCategory.Products;
        }
        public Product GetProductByProductId(int productId)
        {
            var NeededProduct = Products.Where(p => p.ProductId == productId).FirstOrDefault();
            return NeededProduct;
        }
    }

}
