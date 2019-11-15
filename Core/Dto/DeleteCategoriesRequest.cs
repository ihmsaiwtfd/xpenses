using Core.Interfaces;

namespace Core.Dto
{
    public class DeleteCategoriesRequest : IUseCaseRequest<DeleteCategoriesResponse>
    {
        public Category[] Categories { get; }

        public DeleteCategoriesRequest(Category[] categories)
        {
            Categories = categories;
        }
    }
}
