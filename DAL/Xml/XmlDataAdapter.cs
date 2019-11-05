using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Core;

namespace DAL.Xml
{
    public class XmlDataAdapter : IDataAdapter
    {
        private const string _Folder = "xpenses";
        private const string _EntriesFileName = "Entries.xml";
        private const string _CategoriesFileName = "Categories.xml";
        private const string _CatRelationshipFileName = "CategoriesRelationship.xml";

        private IStorage _Storage;
        private List<Core.Entry> _Entries;
        private List<Core.Category> _Categories;
        private bool _IsInitialized;

        public IEnumerable<Entry> Entries => _Entries;

        public IEnumerable<Category> Categories => _Categories;

        public XmlDataAdapter(IStorage storage)
        {
            _Storage = storage;
        }

        public void Load()
        {
            DAL.Xml.Data.Category[] dalCategories = null;
            DAL.Xml.Data.Entry[] dalEntries = null;
            DAL.Xml.Data.CategoryRelationship[] releations = null;

            using (Stream stream = _Storage.Load(Path.Combine(_Folder, _CategoriesFileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DAL.Xml.Data.Category[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    dalCategories = (DAL.Xml.Data.Category[])serializer.ReadObject(reader);
                }
            }
            using (Stream stream = _Storage.Load(Path.Combine(_Folder, _EntriesFileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DAL.Xml.Data.Entry[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    dalEntries = (DAL.Xml.Data.Entry[])serializer.ReadObject(reader);
                }
            }
            using (Stream stream = _Storage.Load(Path.Combine(_Folder, _CatRelationshipFileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DAL.Xml.Data.CategoryRelationship[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    releations = (DAL.Xml.Data.CategoryRelationship[])serializer.ReadObject(reader);
                }
            }

            _Categories = dalCategories.Select(o => o.GetData()).ToList();
            Dictionary<Category, List<Category>> parentMap = new Dictionary<Category, List<Category>>();
            Dictionary<Category, List<Category>> childMap = new Dictionary<Category, List<Category>>();
            foreach (var rel in releations)
            {
                Category parent = _Categories.First(o => o.Uid == rel.ParentID);
                Category child = _Categories.First(o => o.Uid == rel.ChildID);
                if (parentMap[parent] == null)
                {
                    parentMap[parent] = new List<Category>();
                }
                if (childMap[child] == null)
                {
                    childMap[child] = new List<Category>();
                }
                parentMap[parent].Add(child);
                childMap[child].Add(parent);
            }
            foreach (var cat in _Categories)
            {
                cat.Children = parentMap[cat].ToArray();
                cat.Parents = childMap[cat].ToArray();
            }

            _Entries = dalEntries.Select(o =>
            {
                var entry = o.GetData();
                entry.Categories = o.CategoriesUids.Select(cuid => _Categories.First(c => c.Uid == cuid)).ToArray();
                return entry;
            }).ToList();
            _IsInitialized = true;
        }

        public void AddEntry(Entry entry)
        {
            if (!_IsInitialized)
                throw new InvalidOperationException("Call Load() first.");

            if (entry == null)
                throw new ArgumentNullException("Entry should not be empty.");

            _Entries.Add(entry);
            var dalEntries = _Entries.Select(o => new Xml.Data.Entry(entry));
            using (Stream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DAL.Xml.Data.Entry[]));
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    serializer.WriteObject(writer, dalEntries);
                }
                _Storage.Save(stream, Path.Combine(_Folder, _EntriesFileName));
            }
        }

        public void AddCategory(Category category)
        {
            if (!_IsInitialized)
                throw new InvalidOperationException("Call Load() first.");

            if (category == null)
                throw new ArgumentNullException("Category should not be empty.");

            _Categories.Add(category);
            var dalCats = _Categories.Select(o => new Xml.Data.Category(category));
            using (Stream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Data.Category[]));
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    serializer.WriteObject(writer, dalCats);
                }
                _Storage.Save(stream, Path.Combine(_Folder, _CategoriesFileName));
            }

            List<Data.CategoryRelationship> relations = new List<Data.CategoryRelationship>();
            foreach (var cat in _Categories)
            {
                foreach (var child in cat.Children)
                {
                    relations.Add(new Data.CategoryRelationship() { ParentID = cat.Uid, ChildID = child.Uid });
                }
            }
            using (Stream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Data.CategoryRelationship[]));
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    serializer.WriteObject(writer, relations);
                }
                _Storage.Save(stream, Path.Combine(_Folder, _CatRelationshipFileName));
            }
        }
    }
}
