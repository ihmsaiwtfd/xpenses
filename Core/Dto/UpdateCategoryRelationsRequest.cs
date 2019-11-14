using Core.Interfaces;
using System;

namespace Core.Dto
{
    public class UpdateCategoryRelationsRequest : IUseCaseRequest<UpdateCategoryRelationsResponse>
    {
        public Guid Category { get; }

        public Guid[] Parents { get; }

        public UpdateCategoryRelationsRequest(Guid category, Guid[] parents)
        {
            Category = category;
            Parents = parents;
        }
    }
}
