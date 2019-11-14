using Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Core.Dto
{
    public class GetEntriesRequest : IUseCaseRequest<GetEntriesResponse>
    {
        public Expression<Func<Entry, bool>> Query { get; }

        public GetEntriesRequest()
        {
            Query = o => true;
        }

        public GetEntriesRequest(Expression<Func<Entry, bool>> query)
        {
            Query = query;
        }
    }
}
