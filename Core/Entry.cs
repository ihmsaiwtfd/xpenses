using System;
using System.Collections.Generic;

namespace Core
{
    public class Entry
    {
        public Guid Uid { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public Category[] Categories { get; set; }
    }
}
