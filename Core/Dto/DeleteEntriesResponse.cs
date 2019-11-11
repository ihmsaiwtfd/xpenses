using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class DeleteEntriesResponse : UseCaseResponse
    {
        public DeleteEntriesResponse(bool success, string message)
            : base(success, message)
        {
        }
    }
}
