using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Core;
using Core.Interfaces;

namespace DAL.Sql
{
    internal abstract class RepositoryBase<T, TData> : IRepository<T>, IDisposable
        where T : EntityBase
        where TData : IEntityProvider<T>, IDataObject, new()
    {
        private string _ConnectionString;

        protected RepositoryBase()
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = @"WIN-BDSMKLTJAFM\SQLEXPRESS";
            connectionString.InitialCatalog = "xpensesdb";
            connectionString.Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
            _ConnectionString = connectionString.ToString();
        }

        public void Add(T entity)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "INSERT INTO "
                }
            }
        }

        public void Delete(T entity)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                }
            }
        }

        public void Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetById(Guid uid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public void Dispose()
        {
        }
    }
}
