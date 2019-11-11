using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    internal class DeleteEntriesUseCase : IDeleteEntriesUseCase
    {
        private readonly IRepository<Entry> _EntryRepo;
        private readonly IRepository<EntryCategoryRelation> _RelationRepo;

        public DeleteEntriesUseCase(IRepository<Entry> entryRepo, IRepository<EntryCategoryRelation> relationRepo)
        {
            _EntryRepo = entryRepo;
            _RelationRepo = relationRepo;
        }

        public async Task<bool> Handle(DeleteEntriesRequest message, IOutputPort<DeleteEntriesResponse> outputPort)
        {
            try
            {
                await Task.Run(() =>
                {
                    Guid[] uids = message.Entries.Select(o => o.Uid).ToArray();
                    foreach (var relation in _RelationRepo.List(o => uids.Contains(o.EntryUid)).ToArray())
                    {
                        _RelationRepo.Delete(relation);
                    }
                    foreach (var entry in message.Entries)
                    {
                        _EntryRepo.Delete(entry);
                    }
                });
                outputPort.Handle(new DeleteEntriesResponse(true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new DeleteEntriesResponse(false, ex.ToString()));
                return false;
            }
        }
    }
}
