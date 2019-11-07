using System;
using System.Runtime.Serialization;

namespace DAL.Xml.Data
{
    [DataContract]
    internal class CategoryRelationship : IEntityProvider<Core.CategoryRelation>
    {
        [DataMember(Name = "parent_uid", Order = 0)]
        public Guid ParentUid { get; set; }

        [DataMember(Name = "child_uid", Order = 1)]
        public Guid ChildUid { get; set; }

        public Core.CategoryRelation Cast()
        {
            return new Core.CategoryRelation()
            {
                ParentUid = ParentUid,
                ChildUid = ChildUid
            };
        }

        public void From(Core.CategoryRelation entity)
        {
            ParentUid = entity.ParentUid;
            ChildUid = entity.ChildUid;
        }
    }
}
