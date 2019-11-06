using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Xml;
using Core;

namespace DAL.Xml
{
    internal class EntryRepository : RepositoryBase<Entry>
    {
        private List<Entry> _Entries;

        public override void Add(Entry entity)
        {
            Load();
            _Entries.Add(entity);
            Save();
        }

        public override void Delete(Entry entity)
        {
            if (_Entries == null)
                throw new InvalidOperationException();

            _Entries.Remove(entity);
            Save();
        }

        public override void Edit(Entry entity)
        {
            if (_Entries == null)
                throw new InvalidOperationException();

            int idx = _Entries.IndexOf(entity);
            _Entries.RemoveAt(idx);
            _Entries.Insert(idx, entity);
            Save();
        }

        public override Entry GetById(Guid uid)
        {
            Load();
            return _Entries.FirstOrDefault(o => o.Uid == uid);
        }

        public override IEnumerable<Entry> List()
        {
            Load();
            return _Entries;
        }

        public override IEnumerable<Entry> List(Expression<Func<Entry, bool>> predicate)
        {
            Load();
            return _Entries.Where(predicate.Compile());
        }

        private void Load()
        {
            if (_Entries != null)
                return;

            Data.Entry[] dalEntries = null;
            using (Stream stream = _Storage.Load(Path.Combine(_Folder, _EntriesFileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Data.Entry[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    dalEntries = (Data.Entry[])serializer.ReadObject(reader);
                }
            }

            var categories = new CategoryRepository().List().ToArray();
            _Entries = dalEntries.Select(o =>
            {
                var entry = o.Cast();
                entry.Categories = o.CategoriesUids.Select(cuid => categories.First(c => c.Uid == cuid)).ToArray();
                return entry;
            }).ToList();
        }

        private void Save()
        {
            if (_Entries == null)
                throw new InvalidOperationException();

            var dalEntries = _Entries.Select(o => new Data.Entry(o));
            using (Stream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Data.Entry[]));
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    serializer.WriteObject(writer, dalEntries);
                }
                _Storage.Save(stream, Path.Combine(_Folder, _EntriesFileName));
            }
        }
    }
}
