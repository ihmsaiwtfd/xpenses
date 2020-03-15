using System;
using System.Data.SqlClient;

namespace DAL.Sql.Data
{
    class EntryCategoryRelationship : IEntityProvider<Core.EntryCategoryRelation>, IDataObject
    {
        public Guid EntryUid { get; set; }

        public Guid CategoryUid { get; set; }

        public Core.EntryCategoryRelation Cast()
        {
            return new Core.EntryCategoryRelation()
            {
                EntryUid = this.EntryUid,
                CategoryUid = this.CategoryUid
            };
        }

        public void From(Core.EntryCategoryRelation source)
        {
            EntryUid = source.EntryUid;
            CategoryUid = source.CategoryUid;
        }

        public void Read(SqlDataReader reader)
        {
            EntryUid = Guid.Parse(reader["entry_uid"].ToString());
            CategoryUid = Guid.Parse(reader["category_uid"].ToString());
        }
    }
}
