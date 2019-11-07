using System;

namespace Core
{
    public class CategoryRelation : EntityBase
    {
        public Guid ParentUid { get; set; }

        public Guid ChildUid { get; set; }

        public CategoryRelation()
            : base(Guid.Empty)
        {
        }
    }
}
