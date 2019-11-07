using Core.Interfaces;
using System;

namespace Core.Dto
{
    public class AddEntryRequest : IUseCaseRequest<AddEntryResponse>
    {
        public decimal Price { get; }

        public DateTime Date { get; }

        public string Comment { get; }

        public Guid[] Categories { get; }

        public AddEntryRequest(decimal price, DateTime date, string comment, Guid[] categories)
        {
            Price = price;
            Date = date;
            Comment = comment;
            Categories = categories;
        }
    }
}
