using Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Core.Dto
{
    public class GetCategoriesRequest : IUseCaseRequest<GetCategoriesResponse>
    {
        public Expression<Func<Category, bool>> Query { get; }

        public bool IncludeRelations { get; }

        public GetCategoriesRequest(bool includeRelations = true)
        {
            Query = o => true;
            IncludeRelations = includeRelations;
        }

        public GetCategoriesRequest(Expression<Func<Category, bool>> query, bool includeRelations = true)
        {
            Query = query;
            IncludeRelations = includeRelations;
        }
    }
}
