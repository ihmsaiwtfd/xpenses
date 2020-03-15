using System;
using System.Data.SqlClient;
using Core;

namespace DAL.Sql.Data
{
    public class CategoryRelationship : IEntityProvider<Core.CategoryRelation>, IDataObject
    {
        public Guid ParentUid { get; set; }

        public Guid ChildUid { get; set; }

        public CategoryRelation Cast()
        {
            return new CategoryRelation()
            {
                ParentUid = this.ParentUid,
                ChildUid = this.ChildUid
            };
        }

        public void From(CategoryRelation source)
        {
            ParentUid = source.ParentUid;
            ChildUid = source.ChildUid;
        }

        public void Read(SqlDataReader reader)
        {
            ParentUid = Guid.Parse(reader["parent_uid"].ToString());
            ChildUid = Guid.Parse(reader["child_uid"].ToString());
        }
    }
}
