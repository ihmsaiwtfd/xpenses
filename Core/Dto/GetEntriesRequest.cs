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

        public GetEntriesRequest(DateTime fromDate, DateTime toDate)
        {
            Query = o => o.Date > fromDate && o.Date <= toDate;
        }

        public GetEntriesRequest(decimal fromPrice, decimal toPrice)
        {
            Query = o => o.Price > fromPrice && o.Price <= toPrice;
        }

        public GetEntriesRequest(Expression<Func<Entry, bool>> query)
        {
            Query = query;
        }
    }
}
