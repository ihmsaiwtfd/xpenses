namespace Core.Dto
{
    public class DeleteCategoriesResponse : UseCaseResponse
    {
        public DeleteCategoriesResponse(bool success, string message)
            : base(success, message)
        {
        }
    }
}
