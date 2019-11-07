using System;

namespace Core
{
    public class EntryCategoryRelation : EntityBase
    {
        public Guid EntryUid { get; set; }

        public Guid CategoryUid { get; set; }

        public EntryCategoryRelation()
            : base(Guid.Empty)
        {
        }
    }
}
