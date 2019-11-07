using System;
using System.Runtime.Serialization;

namespace DAL.Xml.Data
{
    [DataContract]
    internal class EntryCategoryRelationship : IEntityProvider<Core.EntryCategoryRelation>
    {
        [DataMember(Name = "entry_uid", Order = 0)]
        public Guid EntryUid { get; set; }

        [DataMember(Name = "category_uid", Order = 1)]
        public Guid CategoryUid { get; set; }

        public Core.EntryCategoryRelation Cast()
        {
            return new Core.EntryCategoryRelation()
            {
                EntryUid = EntryUid,
                CategoryUid = CategoryUid
            };
        }

        public void From(Core.EntryCategoryRelation source)
        {
            EntryUid = EntryUid;
            CategoryUid = CategoryUid;
        }
    }
}
