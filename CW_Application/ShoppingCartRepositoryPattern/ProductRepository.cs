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
        public void AddProduct(Product product)
        {
            var products = GetProducts();
            products.Add(product);
            AddProducts(products);
        }

        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            if (File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
            {
                using (var reader = XmlReader.Create(_filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && reader.Name == "Product")
                        {
                            var product = new Product { };
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    switch (reader.Name)
                                    {
                                        case "ProductId":
                                            product.ProductId = reader.ReadElementContentAsInt();
                                            break;
                                        case "Name":
                                            product.Name = reader.ReadElementContentAsString();
                                            break;
                                        case "Price":
                                            product.Price = reader.ReadElementContentAsDouble();
                                            break;
                                    }
                                }
                                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Product")
                                {
                                    break;
                                }
                            }
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public void RemoveProduct(int Id)
        {
            var products = GetProducts();
            products.RemoveAll(x => x.ProductId == Id);
            AddProducts(products);
        }
        public void AddProducts(List<Product> products)
        {
            using (
                var writer = XmlWriter.Create(
                    _filePath,
                    new XmlWriterSettings
                    {
                        OmitXmlDeclaration = true,
                        Indent = true,
                    }
                )
            )
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Products");

                foreach (var product in products)
                {
                    writer.WriteStartElement("Product");

                    writer.WriteElementString("ProductId", product.ProductId.ToString());
                    writer.WriteElementString("Name", product.Name);
                    writer.WriteElementString("Price", product.Price.ToString());

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
