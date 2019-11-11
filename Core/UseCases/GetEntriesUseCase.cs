using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public class GetEntriesUseCase : IGetEntriesUseCase
    {
        private readonly IRepository<Entry> _EntryRepo;

        public GetEntriesUseCase(IRepository<Entry> entryRepo)
        {
            _EntryRepo = entryRepo;
        }

        public async Task<bool> Handle(GetEntriesRequest message, IOutputPort<GetEntriesResponse> outputPort)
        {
            Entry[] entries = null;
            try
            {
                await Task.Run(() =>
                {
                    entries = _EntryRepo.List(message.Query).ToArray();
                });
                outputPort.Handle(new GetEntriesResponse(entries, true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new GetEntriesResponse(null, false, ex.ToString()));
                return false;
            }
        }
    }
}
