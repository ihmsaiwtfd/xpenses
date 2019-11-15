using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    internal class DeleteCategoriesUseCase : IDeleteCategoriesUseCase
    {
        private readonly IRepository<Category> _CatRepo;
        private readonly IRepository<EntryCategoryRelation> _EntryCatRelRepo;
        private readonly IRepository<CategoryRelation> _CatRelRepo;

        public DeleteCategoriesUseCase(IRepository<Category> catRepo, IRepository<CategoryRelation> catRelRepo, IRepository<EntryCategoryRelation> entryCatRelRepo)
        {
            _CatRepo = catRepo;
            _EntryCatRelRepo = entryCatRelRepo;
            _CatRelRepo = catRelRepo;
        }

        public async Task<bool> Handle(DeleteCategoriesRequest message, IOutputPort<DeleteCategoriesResponse> outputPort)
        {
            try
            {
                await Task.Run(() =>
                {
                    Guid[] uids = message.Categories.Select(o => o.Uid).ToArray();
                    foreach (CategoryRelation relation in _CatRelRepo.List(o => uids.Contains(o.ChildUid) || uids.Contains(o.ParentUid)).ToArray())
                    {
                        _CatRelRepo.Delete(relation);
                    }
                    foreach (EntryCategoryRelation relation in _EntryCatRelRepo.List(o => uids.Contains(o.CategoryUid)).ToArray())
                    {
                        _EntryCatRelRepo.Delete(relation);
                    }
                    foreach (Category cat in message.Categories)
                    {
                        _CatRepo.Delete(cat);
                    }
                });
                outputPort.Handle(new DeleteCategoriesResponse(true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new DeleteCategoriesResponse(false, ex.ToString()));
                return false;
            }
        }
    }
}
