using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace DAL.Sql.Data
{
    public class Category : IEntityProvider<Core.Category>, IDataObject
    {
        public Guid Uid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Core.Category Cast()
        {
            return new Core.Category(Uid)
            {
                Name = this.Name,
                Description = this.Description
            };
        }

        public void From(Core.Category source)
        {
            Uid = source.Uid;
            Name = source.Name;
            Description = source.Description;
        }

        public void Read(SqlDataReader reader)
        {
            Uid = Guid.Parse(reader["uid"].ToString());
            Name = (string)reader["name"];
            Description = (string)reader["desciption"];
        }
    }
}
