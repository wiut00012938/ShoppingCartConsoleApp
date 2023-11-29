using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CleanShoppingCart
{
    public class XmlRepository
    {
        string _filePath;

        public XmlRepository(string _filePath)
        {
            this._filePath = _filePath;
        }
        public void SaveProducts(List<Product> products)
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
            ){
                writer.WriteStartDocument();
                writer.WriteStartElement("Products");

                foreach (var product in products)
                {
                    writer.WriteStartElement("Product");

                    writer.WriteElementString("ProductId", product.ProductId.ToString());
                    writer.WriteElementString("Name", product.Name);
                    writer.WriteElementString("Price", product.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        public List<Product> LoadProducts()
        {
            var products = new List<Product>();
            if(File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
            {
                using (var reader = XmlReader.Create(_filePath))
                {
                    while (reader.Read())
                    {
                        if(reader.IsStartElement() && reader.Name  == "Product")
                        {
                            var product = new Product{};
                            while (reader.Read())
                            {
                                if(reader.NodeType == XmlNodeType.Element)
                                {
                                    switch(reader.Name)
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
    }
}
