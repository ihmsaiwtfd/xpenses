
namespace Core.Dto
{
    public class GetCategoriesResponse : UseCaseResponse
    {
        public Category[] Categories { get; set; }

        public CategoryRelation[] Relations { get; set; }

        public GetCategoriesResponse(Category[] categories, CategoryRelation[] relations, bool success, string message)
            : base(success, message)
        {
            Categories = categories;
            Relations = relations;
        }
    }
}
