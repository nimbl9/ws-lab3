using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> products = new()
        {
            new Product { Id = 1, Name = "Ноутбук", Price = 350000 },
            new Product { Id = 2, Name = "Мышь", Price = 15000 },
            new Product { Id = 3, Name = "Клавиатура", Price = 25000 }
        };

        public IEnumerable<Product> GetAll()
        {
            return products;
        }

        public Product? GetById(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }

        public Product Add(Product product)
        {
            product.Id = products.Count == 0 ? 1 : products.Max(p => p.Id) + 1;
            products.Add(product);

            return product;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);

            if (product == null)
            {
                return false;
            }

            products.Remove(product);
            return true;
        }
    }
}