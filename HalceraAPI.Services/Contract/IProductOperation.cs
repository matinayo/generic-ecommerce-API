using HalceraAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Contract
{
    public interface IProductOperation
    {
        Task<IEnumerable<Product>?> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task<int> AddProductToCart(int productId);
    }
}
