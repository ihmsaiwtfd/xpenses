namespace Core.Dto
{
    public class UpdateCategoryRelationsResponse : UseCaseResponse
    {
        public UpdateCategoryRelationsResponse(bool success, string message)
            : base(success, message)
        {
        }
    }
}
