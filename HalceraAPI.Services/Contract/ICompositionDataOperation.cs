using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalceraAPI.Models.Requests.Composition.CompositionData;

namespace HalceraAPI.Services.Contract
{
    public interface ICompositionDataOperation
    {
        /// <summary>
        /// Update Product Composition Data
        /// </summary>
        void UpdateCompositionData(IEnumerable<UpdateCompositionDataRequest>? compositionDataRequests, ICollection<CompositionData>? existingCompositionDataFromDb);
        /// <summary>
        /// Delete range of Composition Data
        /// </summary>
        /// <param name="compositionsId">List of Composition Id to be deleted</param>
        /// <returns>Returns false if nothing was deleted otherwise true</returns>
        Task<bool> DeleteCompositionData(IEnumerable<int> compositionIdCollection);
    }
}
