using System;
using System.Runtime.Serialization;
using Core;

namespace DAL.Xml.Data
{
    [DataContract]
    internal class Category : IEntityProvider<Core.Category>
    {
        [DataMember(Name = "uid", Order = 0)]
        public Guid Uid { get; set; }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "description", Order = 2)]
        public string Description { get; set; }

        public Core.Category Cast()
        {
            return new Core.Category(Uid)
            {
                Name = Name,
                Description = Description
            };
        }

        public void From(Core.Category entity)
        {
            Uid = entity.Uid;
            Name = entity.Name;
            Description = entity.Description;
        }
    }
}
