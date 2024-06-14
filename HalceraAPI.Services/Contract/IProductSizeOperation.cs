using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Composition;
using HalceraAPI.Services.Dtos.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Contract
{
    public interface IProductSizeOperation
    {
        Task DeleteSizeByListOfCompositionIdAsync(List<int> compositionIds);
        void UpdateProductSize(IEnumerable<UpdateProductSizeRequest>? productSizeRequests,
            ICollection<ProductSize>? existingProductSizesFromDb);
    }
}
