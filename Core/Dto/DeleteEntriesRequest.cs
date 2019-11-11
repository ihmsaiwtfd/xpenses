using Core.Interfaces;
using System;

namespace Core.Dto
{
    public class DeleteEntriesRequest : IUseCaseRequest<DeleteEntriesResponse>
    {
        public Entry[] Entries { get; }

        public DeleteEntriesRequest(Entry[] entries)
        {
            Entries = entries;
        }
    }
}
