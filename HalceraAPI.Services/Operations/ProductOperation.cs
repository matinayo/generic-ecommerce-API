using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Model;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class ProductOperation : IProductOperation
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductOperation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> AddProductToCart(int productId)
        {
            try
            {
                ShoppingCart? cart = await _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(cart => cart.ProductId == productId);
                if (cart == null)
                {
                    Product? productItem = await _unitOfWork.ProductRepository.GetFirstOrDefault(product => product.Id == productId);

                    cart = new ShoppingCart() { ProductId = productId, ProductTitle = productItem?.Title, ImageURL = productItem?.ImageURL, Price = productItem?.Price };
                    // if product item does not exist in cart, add new item
                    await _unitOfWork.ShoppingCartRepository.Add(cart);
                }
                else
                {
                    // update existing item count in cart
                    cart.Count++;
                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Gets All products
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<IEnumerable<Product>?> GetAllProducts()
        {
            IEnumerable<Product>? listOfProducts = await _unitOfWork.ProductRepository.GetAll();
            return listOfProducts;
        }

        /// <summary>
        /// Get a single product by Id
        /// </summary>
        /// <param name="id">Id of product to be returned</param>
        /// <returns>Product</returns>
        public async Task<Product?> GetProductById(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.GetFirstOrDefault(product => product.Id == id);
            return product;
        }
    }
}
