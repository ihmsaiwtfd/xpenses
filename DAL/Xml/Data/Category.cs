using System;
using System.Runtime.Serialization;
using Core;

namespace DAL.Xml.Data
{
    [DataContract]
    internal class Category : IDataProvider<Core.Category>
    {
        [DataMember(Name = "uid", Order = 0)]
        public Guid Uid { get; set; }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "description", Order = 2)]
        public string Description { get; set; }

        public Core.Category GetData()
        {
            return new Core.Category()
            {
                Uid = Uid,
                Name = Name,
                Description = Description
            };
        }

        public Category()
        {
        }

        public Category(Core.Category source)
        {
            Uid = Uid;
            Name = Name;
            Description = Description;
        }
    }
}
