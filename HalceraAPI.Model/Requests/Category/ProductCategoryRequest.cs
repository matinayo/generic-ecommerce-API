using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Category
{
    /// <summary>
    /// Category Id for Products
    /// </summary>
    public class ProductCategoryRequest
    {
        [Required]
        public int CategoryId { get; set; }
    }
}
