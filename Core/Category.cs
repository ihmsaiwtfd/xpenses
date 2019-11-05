using System;
using System.Collections.Generic;

namespace Core
{
    public class Category
    {
        public Guid Uid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Category[] Parents { get; set; }

        public Category[] Children { get; set; }
    }
}
