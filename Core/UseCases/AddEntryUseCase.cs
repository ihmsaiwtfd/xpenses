using System;
using System.Threading.Tasks;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class AddEntryUseCase : IAddEntryUseCase
    {
        private readonly IRepository<Entry> _EntryRepo;
        private readonly IRepository<EntryCategoryRelation> _RelationRepo;

        public AddEntryUseCase(IRepository<Entry> entryRepo, IRepository<EntryCategoryRelation> relationRepo)
        {
            _EntryRepo = entryRepo;
            _RelationRepo = relationRepo;
        }

        public async Task<bool> Handle(AddEntryRequest message, IOutputPort<AddEntryResponse> outputPort)
        {
            Entry entry = new Entry(_EntryRepo.NextIdentity())
            {
                Price = message.Price,
                Date = message.Date,
                Comment = message.Comment
            };
            try
            {
                await Task.Run(() =>
                {
                    _EntryRepo.Add(entry);
                    foreach (Guid cat in message.Categories)
                    {
                        EntryCategoryRelation rel = new EntryCategoryRelation()
                        {
                            EntryUid = entry.Uid,
                            CategoryUid = cat
                        };
                        _RelationRepo.Add(rel);
                    }
                });
                outputPort.Handle(new AddEntryResponse(entry, true, null));
                return true;
            }
            catch (Exception ex)
            {
                outputPort.Handle(new AddEntryResponse(null, false, ex.ToString()));
                return false;
            }
        }
    }
}
