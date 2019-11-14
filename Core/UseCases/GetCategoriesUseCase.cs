using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.UseCases
{
    internal class GetCategoriesUseCase : IGetCategoriesUseCase
    {
        private readonly IRepository<Category> _CatRepo;
        private readonly IRepository<CategoryRelation> _RelRepo;

        public GetCategoriesUseCase(IRepository<Category> catRepo, IRepository<CategoryRelation> relRepo)
        {
            _CatRepo = catRepo;
            _RelRepo = relRepo;
        }

        public async Task<bool> Handle(GetCategoriesRequest message, IOutputPort<GetCategoriesResponse> outputPort)
        {
            Category[] categories = null;
            CategoryRelation[] relations = null;
            try
            {
                await Task.Run(() =>
                {
                    categories = _CatRepo.List(message.Query).ToArray();
                    if (message.IncludeRelations)
                    {
                        Guid[] catUids = categories.Select(o => o.Uid).ToArray();
                        relations = _RelRepo.List(o => catUids.Contains(o.ParentUid) || catUids.Contains(o.ChildUid)).ToArray();
                    }
                });
                outputPort.Handle(new GetCategoriesResponse(categories, relations, true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new GetCategoriesResponse(null, null, false, ex.ToString()));
                return false;
            }
        }
    }
}
