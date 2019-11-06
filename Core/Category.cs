using System;
using System.Collections.Generic;

namespace Core
{
    public class Category : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Category[] Parents { get; set; }

        public Category[] Children { get; set; }

        public Category(Guid uid)
            : base(uid)
        {
        }
    }
}
