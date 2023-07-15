using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Contract
{
    public interface ICompositionDataOperation
    {
        /// <summary>
        /// Update Product Composition
        /// </summary>
        /// <param name="mediaCollection"></param>
        /// <returns></returns>
        Task<ICollection<Media>?> UpdateCompositionData(IEnumerable<UpdateMediaRequest>? mediaCollection);
        /// <summary>
        /// Delete range of Composition Data
        /// </summary>
        /// <param name="compositionsId">List of Composition Id to be deleted</param>
        /// <returns>Returns false if nothing was deleted otherwise true</returns>
        Task<bool> DeleteCompositionData(IEnumerable<int> compositionIdCollection);
    }
}
