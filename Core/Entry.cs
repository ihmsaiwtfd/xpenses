using System;
using System.Collections.Generic;

namespace Core
{
    public class Entry : EntityBase
    {
        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public Entry(Guid uid)
            : base(uid)
        {
        }
    }
}
