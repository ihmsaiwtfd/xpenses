using System.Data.SqlClient;

namespace DAL.Sql
{
    public interface IDataObject
    {
        void Read(SqlDataReader reader);
    }
}
