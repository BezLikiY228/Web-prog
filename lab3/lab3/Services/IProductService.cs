using lab3.Models;

namespace lab3.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();

        Product? GetById(int id);

        Product Add(Product product);

        bool Delete(int id);
    }
}