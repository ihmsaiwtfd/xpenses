using Core.Interfaces;

namespace Core.Dto
{
    public class AddCategoryRequest : IUseCaseRequest<AddCategoryResponse>
    {
        public string Name { get; }

        public string Description { get; set; }

        public AddCategoryRequest(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
