using System;
using System.Data.SqlClient;

namespace DAL.Sql.Data
{
    public class Entry : IEntityProvider<Core.Entry>, IDataObject
    {
        public Guid Uid { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public Core.Entry Cast()
        {
            return new Core.Entry(Uid)
            {
                Price = this.Price,
                Date = this.Date,
                Comment = this.Comment
            };
        }

        public void From(Core.Entry source)
        {
            Uid = source.Uid;
            Price = source.Price;
            Date = source.Date;
            Comment = source.Comment;
        }

        public void Read(SqlDataReader reader)
        {
            Uid = Guid.Parse(reader["uid"].ToString());
            Price = (decimal)reader["price"];
            Date = (DateTime)reader["date"];
            Comment = (string)reader["comment"];
        }
    }
}
