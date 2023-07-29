﻿using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class CompositionOperation : ICompositionOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ICompositionDataOperation _compositionDataOperation;

        public CompositionOperation(IUnitOfWork unitOfWork, IMapper mapper, ICompositionDataOperation compositionDataOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _compositionDataOperation = compositionDataOperation;
        }

        public async Task<bool> DeleteProductCompositions(int productId)
        {
            try
            {
                IEnumerable<Composition>? productCompositions = await _unitOfWork.Composition.GetAll(composition => composition.ProductId == productId);
                if (productCompositions is not null && productCompositions.Any())
                {
                    // delete product composition data
                    await _compositionDataOperation.DeleteCompositionData(productCompositions.Select(comp => comp.Id));

                    _unitOfWork.Composition.RemoveRange(productCompositions);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateComposition(int productId, IEnumerable<UpdateCompositionRequest>? compositionCollection, IEnumerable<Composition>? existingCompositions)
        {
            try
            {
                if (compositionCollection is not null && compositionCollection.Any())
                {
                    List<Composition> compositionsToAdd = new();
                    foreach (var compositionRequest in compositionCollection)
                    {
                        // Find existing composition with the same ID in the database
                        Composition? existingComposition = existingCompositions?.FirstOrDefault(em => em.Id == compositionRequest.Id);
                        if (existingComposition != null)
                        {
                            // If the composition already exists, update its properties
                            _mapper.Map(compositionRequest, existingComposition);
                        }
                        else
                        {
                            // If the composition does not exist, create a new composition object and map the properties
                            Composition newComposition = _mapper.Map<Composition>(compositionRequest);
                            newComposition.ProductId = productId;
                            compositionsToAdd?.Add(newComposition);
                        }
                    }
                    if (compositionsToAdd.Any())
                    {
                        await _unitOfWork.Composition.AddRange(compositionsToAdd);
                    }
                }

            }catch(Exception)
            {
                throw;
            }
        }
    }
}