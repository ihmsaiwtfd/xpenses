using Core;
using System.Collections.Generic;

namespace DAL
{
    public interface IDataAdapter
    {
        IEnumerable<Entry> Entries { get; }
        IEnumerable<Category> Categories { get; }

        void Load();
        void AddEntry(Entry entry);
        void AddCategory(Category category);
    }
}
