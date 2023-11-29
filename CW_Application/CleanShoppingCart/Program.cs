using ShoppingCartRepositoryPattern;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CleanShoppingCart
{
    class MainClass
    {
        static void Main()
        {
            Category clothingCategory = new Category("Clothing");
            Category shoesCategory = new Category("Shoes");
            Category bagsCategory = new Category("Bags");
            Category jewelryCategory = new Category("Jewelry");
            Category watchesCategory = new Category("Watches");
            Category accessoriesCategory = new Category("Accessories");
            Category stationeryCategory = new Category("Stationery");

            // Creating an array of sample Categories
            Category[] categoryNames = {clothingCategory, shoesCategory, bagsCategory, jewelryCategory, watchesCategory, accessoriesCategory, stationeryCategory};

            // Iterating through each categorying in order to populate them with sample products
            foreach(Category category in categoryNames)
            {
                for (int i = 1; i <= 10; i++)
                {
                    var price = Math.Round(20.42 * i,2);
                    category.AddProduct(new Product($"{category.Name} Item {i}", price));
                }
            }
            string filePath = "ProductXML.xml";
            IRepository repository = new ProductRepository(filePath);
            Console.WriteLine($"=== Online Shop 'One Piece' ===");
            Console.WriteLine($"=== Welcome to our Online Shop. Here you can find many things related to One Piece ===");
            while (true)
            {
                ShoppingCart cart = new ShoppingCart(repository);
                foreach (Category category in categoryNames)
                {
                    cart.AddCategory(category);
                }
                Console.WriteLine("\n---------------------------------------------------------");
                Console.WriteLine("One Piece Menu:");
                Console.WriteLine("1.  To List Categories, Enter Keyword: Categories or 1");
                Console.WriteLine("2.  To List All Products, Enter Keyword: Products or 2");
                Console.WriteLine("3.  To List All items in my Shopping Cart, Enter Keyword: Cart or 3");
                Console.WriteLine("4.  To Close the application, Enter Keyword: Close or 4");
                Console.WriteLine("---------------------------------------------------------\n");
                var MenuResponse = Console.ReadLine();
                if (MenuResponse == "1" || MenuResponse.ToLower() == "categories")
                {
                    bool IsCategoryActive = true;
                    Console.WriteLine("Categories section was selected");
                    Console.WriteLine("\n----------------------------------------------");
                    foreach(Category category in categoryNames)
                    {
                        Console.WriteLine($"{category.CategoryId}.  {category.Name}");
                    }
                    Console.WriteLine("----------------------------------------------\n");
                    while (IsCategoryActive)
                    {
                        try
                        {
                            Console.WriteLine("Enter a category number or category name to list products that it contains");
                            string userInput = Console.ReadLine();
                            Console.WriteLine("\n---------------------------------------------------------");
                            int categoryNumber;
                            bool isNumeric = int.TryParse(userInput, out categoryNumber);
                            List<Product> SelectedCategoryProducts = new List<Product>();
                            var i = 1;
                            if (isNumeric)
                            {
                                SelectedCategoryProducts = cart.GetProductsByCategoryId(Convert.ToInt32(userInput));
                            }
                            else
                            {
                                SelectedCategoryProducts = cart.GetProductsByCategoryName(userInput);
                            }
                            foreach (Product product in SelectedCategoryProducts)
                            {
                                Console.WriteLine($"{i}.  {product.Name} - Price ${product.Price}");
                                i++;
                            };
                            Console.WriteLine("---------------------------------------------------------\n");
                            while (true)
                            {
                                Console.WriteLine("Enter a product number to add into shopping cart");
                                var SelectedProduct = Console.ReadLine();
                                Console.WriteLine("\n---------------------------------------------------------");
                                int selectedProductIndex;
                                bool isValidInput = int.TryParse(SelectedProduct, out selectedProductIndex);

                                if (isValidInput && selectedProductIndex > 0 && selectedProductIndex <= SelectedCategoryProducts.Count)
                                {
                                    // Subtract 1 from the selected index to match the zero-based index of the list
                                    Product selectedProduct = SelectedCategoryProducts[selectedProductIndex - 1];
                                    cart.AddProduct(selectedProduct);
                                    Console.WriteLine($"{selectedProduct.Name} has been added to the shopping cart.");
                                    Console.WriteLine("Do you want to continue adding products to the shopping cart? (yes/no)");
                                    var IsAdditionalProduct = Console.ReadLine();
                                    if (IsAdditionalProduct == "yes")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        CartMenu(cart);
                                        IsCategoryActive = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\n---------------------------------------------------------");
                                    Console.WriteLine("Invalid product number. Please try again.");
                                    Console.WriteLine("---------------------------------------------------------\n");
                                    continue;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n---------------------------------------------------------");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("---------------------------------------------------------\n");
                        }
                    }

                }
                else if(MenuResponse == "2" || MenuResponse.ToLower() == "products")
                {
                    
                    Console.WriteLine("Products section was selected");
                    Console.WriteLine("\n----------------------------------------------");
                    var i = 1;
                    var AllProducts = new List<Product>();
                    foreach(Category category in cart.GetCategories())
                    {
                        foreach(Product product in category.Products)
                        {
                            AllProducts.Add(product);
                            Console.WriteLine($"{i}.  {product.Name} - Price ${product.Price}");
                            i++;
                        }
                    }
                    Console.WriteLine("---------------------------------------------------------\n");
                    while (true)
                    {
                        Console.WriteLine("Enter a product number to add into shopping cart");
                        var SelectedProduct = Console.ReadLine();
                        Console.WriteLine("\n---------------------------------------------------------");
                        int selectedProductIndex;
                        bool isValidInput = int.TryParse(SelectedProduct, out selectedProductIndex);

                        if (isValidInput && selectedProductIndex > 0 && selectedProductIndex <= AllProducts.Count)
                        {
                            // Subtract 1 from the selected index to match the zero-based index of the list
                            Product selectedProduct = AllProducts[selectedProductIndex - 1];
                            cart.AddProduct(selectedProduct);
                            Console.WriteLine($"{selectedProduct.Name} has been added to the shopping cart.");
                            Console.WriteLine("Do you want to continue adding products to the shopping cart? (yes/no)");
                            var IsAdditionalProduct = Console.ReadLine();
                            if (IsAdditionalProduct == "yes")
                            {
                                continue;
                            }
                            else
                            {
                                CartMenu(cart);
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n---------------------------------------------------------");
                            Console.WriteLine("Invalid product number. Please try again.");
                            Console.WriteLine("---------------------------------------------------------\n");
                            continue;
                        }
                    }
                }
                else if(MenuResponse == "3" || MenuResponse.ToLower() == "cart")
                {
                    CartMenu(cart);
                }
                else if(MenuResponse == "4" || MenuResponse.ToLower() == "close")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\n---------------------------------------------------------");
                    Console.WriteLine("I don't understand you response :(");
                    Console.WriteLine("---------------------------------------------------------\n");
                }
            }
        }
        public static void CartMenu(ShoppingCart cart)
        {
            while (true)
            {
                Console.WriteLine("Your Shopping Car List:");
                foreach (Product product in cart.Products)
                {
                    Console.WriteLine($"{product.ProductId}.  {product.Name} - Price ${product.Price}");
                }
                Console.WriteLine($"Total Price: ${Math.Round(cart.TotalCost,2)}");
                Console.WriteLine("\n---------------------------------------------------------");
                Console.WriteLine("Enter keyword, \"Menu\" to go to Main Menu");
                Console.WriteLine("Enter keyword Remove, to remove a product from shopping cart");
                var UserAnswer = Console.ReadLine();
                if (UserAnswer.ToLower() == "menu")
                {
                    break;
                }
                else if (UserAnswer.ToLower() == "remove")
                {
                    while (true)
                    {
                        Console.WriteLine("Enter keyword id of a product");
                        var ProductByUser = Console.ReadLine();
                        Product SelectedProduct = cart.GetProductByProductId(Convert.ToInt32(ProductByUser));
                        if(SelectedProduct != null)
                        {
                            cart.RemoveProduct(SelectedProduct);
                            Console.WriteLine("Succesfully Removed");
                            break;

                        }
                        else
                        {
                            Console.WriteLine("\n---------------------------------------------------------");
                            Console.WriteLine("Wrong input.Try again");
                            Console.WriteLine("---------------------------------------------------------\n");
                        }
                    }

                }
                else
                {
                    Console.WriteLine("\n---------------------------------------------------------");
                    Console.WriteLine("Wrong input. Please try again");
                    Console.WriteLine("---------------------------------------------------------\n");
                }
            }
        }
    }
}