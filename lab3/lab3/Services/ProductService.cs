using lab3.Models;
using lab3.Services;
using System.Xml.Linq;

namespace lab3.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 450000
            },

            new Product
            {
                Id = 2,
                Name = "Smartphone",
                Price = 250000
            },

            new Product
            {
                Id = 3,
                Name = "Headphones",
                Price = 50000
            }
        };

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product Add(Product product)
        {
            int newId = _products.Count == 0
                ? 1
                : _products.Max(p => p.Id) + 1;

            product.Id = newId;

            _products.Add(product);

            return product;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);

            if (product == null)
            {
                return false;
            }

            _products.Remove(product);

            return true;
        }
    }
}