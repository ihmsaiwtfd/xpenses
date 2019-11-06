using System;

namespace Core
{
    public abstract class EntityBase
    {
        public Guid Uid { get; private set; }

        protected EntityBase(Guid uid)
        {
            Uid = uid;
        }
    }
}
