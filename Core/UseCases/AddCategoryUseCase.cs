using System;
using System.Threading.Tasks;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    internal class AddCategoryUseCase : IAddCategoryUseCase
    {
        private IRepository<Category> _CatRepo;

        public AddCategoryUseCase(IRepository<Category> catRepo)
        {
            _CatRepo = catRepo;
        }

        public async Task<bool> Handle(AddCategoryRequest message, IOutputPort<AddCategoryResponse> outputPort)
        {
            Category cat = new Category(_CatRepo.NextIdentity())
            {
                Name = message.Name,
                Description = message.Description
            };
            try
            {
                await Task.Run(() =>
                {
                    _CatRepo.Add(cat);
                });
                outputPort.Handle(new AddCategoryResponse(cat, true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new AddCategoryResponse(null, false, ex.ToString()));
                return false;
            }
        }
    }
}
