using practice_3.Models;

namespace practice_3.Services;

public interface IProductService
{
    IEnumerable<Product> GetAll();

    Product? GetById(int id);
}