using System;
using System.Runtime.Serialization;

namespace DAL.Xml.Data
{
    [DataContract]
    internal class CategoryRelationship
    {
        [DataMember(Name = "parent_uid", Order = 0)]
        public Guid ParentID { get; set; }

        [DataMember(Name = "child_uid", Order = 1)]
        public Guid ChildID { get; set; }
    }
}
