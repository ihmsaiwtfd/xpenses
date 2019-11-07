using System;
using Core;

namespace DAL.Xml
{
    internal class EntryRepository : RepositoryBase<Entry, Data.Entry>
    {
        protected override string FileName => _EntriesFileName;
    }
}
