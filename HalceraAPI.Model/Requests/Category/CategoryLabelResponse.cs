using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Category
{
    /// <summary>
    /// Overview data of Category
    /// </summary>
    public class CategoryLabelResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
