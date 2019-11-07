using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Xml;
using Core;
using Core.Interfaces;

namespace DAL.Xml
{
    internal abstract class RepositoryBase<T, TData> : IRepository<T>
        where T : EntityBase
        where TData : IEntityProvider<T>, new()
    {
        protected const string _Folder = "xpenses";
        protected const string _EntriesFileName = "Entries.xml";
        protected const string _CategoriesFileName = "Categories.xml";
        protected const string _CatRelationshipFileName = "CategoriesRelationship.xml";
        protected const string _EntryCatRelationshipFileName = "EntryCategoryRelationship.xml";

        protected IFileStorage _Storage;
        protected List<T> _List;

        protected abstract string FileName { get; }

        protected RepositoryBase()
        {
            _Storage = new IsolatedStorage();
        }

        public virtual void Add(T entity)
        {
            Load();
            _List.Add(entity);
            Save();
        }

        public virtual void Delete(T entity)
        {
            if (_List == null)
                throw new InvalidOperationException();

            _List.Remove(entity);
            Save();
        }

        public virtual void Edit(T entity)
        {
            if (_List == null)
                throw new InvalidOperationException();

            int idx = _List.IndexOf(entity);
            _List.RemoveAt(idx);
            _List.Insert(idx, entity);
            Save();
        }

        public virtual T GetById(Guid uid)
        {
            Load();
            return _List.FirstOrDefault(o => o.Uid == uid);
        }

        public virtual IEnumerable<T> List()
        {
            Load();
            return _List;
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            Load();
            return _List.Where(predicate.Compile());
        }

        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        protected virtual void Load()
        {
            if (_List != null)
                return;

            TData[] dalEntities = null;
            using (Stream stream = _Storage.Load(Path.Combine(_Folder, FileName)))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(TData[]));
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    dalEntities = (TData[])serializer.ReadObject(reader);
                }
            }
            _List = dalEntities.Select(o => o.Cast()).ToList();
        }

        protected virtual void Save()
        {
            if (_List == null)
                throw new InvalidOperationException();

            IEnumerable<TData> dalEntities = _List.Select(o =>
            {
                TData d = new TData();
                d.From(o);
                return d;
            });
            using (Stream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(TData[]));
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    serializer.WriteObject(writer, dalEntities);
                }
                _Storage.Save(stream, Path.Combine(_Folder, FileName));
            }
        }
    }
}
