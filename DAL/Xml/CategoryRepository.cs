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
    internal class CategoryRepository : RepositoryBase<Category>
    {
        private List<Category> _Categories;

        public override void Add(Category entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public override void Edit(Category entity)
        {
            throw new NotImplementedException();
        }

        public override Category GetById(Guid uid)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Category> List()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Category> List(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        private void Load()
        {
            if (_Categories != null)
                return;

            Data.Category[] dalCategories = null;
            Data.CategoryRelationship[] releations = null;

            using (Stream stream = _Storage.Load(Path.Combine(_Folder, _CategoriesFileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Data.Category[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    dalCategories = (Data.Category[])serializer.ReadObject(reader);
                }
            }
            using (Stream stream = _Storage.Load(Path.Combine(_Folder, _CatRelationshipFileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Data.CategoryRelationship[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    releations = (Data.CategoryRelationship[])serializer.ReadObject(reader);
                }
            }

            var categories = dalCategories.Select(o => o.Cast()).ToList();
            Dictionary<Category, List<Category>> parentMap = new Dictionary<Category, List<Category>>();
            Dictionary<Category, List<Category>> childMap = new Dictionary<Category, List<Category>>();
            foreach (var rel in releations)
            {
                Category parent = categories.First(o => o.Uid == rel.ParentUid);
                Category child = categories.First(o => o.Uid == rel.ChildUid);
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
            foreach (var cat in categories)
            {
                cat.Children = parentMap[cat].ToArray();
                cat.Parents = childMap[cat].ToArray();
            }
        }

        private void Save()
        {
            if (_Categories == null)
                throw new InvalidOperationException();

            var dalCats = _Categories.Select(o => new Xml.Data.Category(o));
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
                    relations.Add(new Data.CategoryRelationship() { ParentUid = cat.Uid, ChildUid = child.Uid });
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
