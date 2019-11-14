namespace Core.Dto
{
    public class AddCategoryResponse : UseCaseResponse
    {
        public Category AddedCategory { get; }

        public AddCategoryResponse(Category newCategory, bool success, string message)
            : base(success, message)
        {
            AddedCategory = newCategory;
        }
    }
}
