using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class AddEntryResponse : UseCaseResponse
    {
        public Entry AddedEntry { get; }

        public AddEntryResponse(Entry newEntry, bool success, string message)
        {
            AddedEntry = newEntry;
        }
    }
}
