using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.UseCases
{
    internal class UpdateCategoryRelationsUseCase : IUpdateCategoryRelationsUseCase
    {
        private readonly IRepository<Category> _CatRepo;
        private readonly IRepository<CategoryRelation> _RelRepo;

        public UpdateCategoryRelationsUseCase(IRepository<Category> catRepo, IRepository<CategoryRelation> relRepo)
        {
            _CatRepo = catRepo;
            _RelRepo = relRepo;
        }

        public async Task<bool> Handle(UpdateCategoryRelationsRequest message, IOutputPort<UpdateCategoryRelationsResponse> outputPort)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (_CatRepo.GetById(message.Category) == null)
                        throw new ArgumentException("No category with such id.");

                    if (_CatRepo.List(o => message.Parents.Contains(o.Uid)).Count() != message.Parents.Length)
                        throw new ArgumentException("Not all parent categories exist.");

                    CategoryRelation[] relations = _RelRepo.List(o => o.ChildUid == message.Category).ToArray();
                    foreach (CategoryRelation rel in relations.Where(o => !message.Parents.Contains(o.ParentUid)))
                    {
                        _RelRepo.Delete(rel);
                    }
                    foreach (Guid newParent in message.Parents.Where(o => !relations.Any(r => r.ParentUid == o)))
                    {
                        _RelRepo.Add(new CategoryRelation() { ChildUid = message.Category, ParentUid = newParent });
                    }
                });
                outputPort.Handle(new UpdateCategoryRelationsResponse(true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new UpdateCategoryRelationsResponse(false, ex.ToString()));
                return false;
            }
        }
    }
}
