using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    internal class GetEntriesUseCase : IGetEntriesUseCase
    {
        private readonly IRepository<Entry> _Repo;

        public GetEntriesUseCase(IRepository<Entry> repo)
        {
            _Repo = repo;
        }

        public async Task<bool> Handle(GetEntriesRequest message, IOutputPort<GetEntriesResponse> outputPort)
        {
            Entry[] entries = null;
            try
            {
                await Task.Run(() =>
                {
                    entries = _Repo.List(message.Query).ToArray();
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
