using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core;

namespace DAL.Xml
{
    internal abstract class RepositoryBase<T> : IRepository<T>
        where T : EntityBase
    {
        protected const string _Folder = "xpenses";
        protected const string _EntriesFileName = "Entries.xml";
        protected const string _CategoriesFileName = "Categories.xml";
        protected const string _CatRelationshipFileName = "CategoriesRelationship.xml";

        protected IFileStorage _Storage;

        protected RepositoryBase()
        {
            _Storage = new IsolatedStorage();
        }

        public abstract void Add(T entity);

        public abstract void Delete(T entity);

        public abstract void Edit(T entity);

        public abstract T GetById(Guid uid);

        public abstract IEnumerable<T> List();

        public abstract IEnumerable<T> List(Expression<Func<T, bool>> predicate);

        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }
    }
}
