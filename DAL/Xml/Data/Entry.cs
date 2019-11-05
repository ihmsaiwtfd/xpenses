using System;
using System.Runtime.Serialization;
using Core;

namespace DAL.Xml.Data
{
    [DataContract]
    internal class Entry : IDataProvider<Core.Entry>
    {
        [DataMember(Name = "uid", Order = 0)]
        public Guid Uid { get; set; }

        [DataMember(Name = "date", Order = 1)]
        public DateTime Date { get; set; }

        [DataMember(Name = "price", Order = 2)]
        public decimal Price { get; set; }

        [DataMember(Name = "comment", Order = 3)]
        public string Comment { get; set; }

        [DataMember(Name = "categories_uids", Order = 4)]
        public Guid[] CategoriesUids { get; set; }

        public Core.Entry GetData()
        {
            return new Core.Entry()
            {
                Uid = Uid,
                Date = Date,
                Price = Price,
                Comment = Comment
            };
        }

        public Entry()
        {
        }

        public Entry(Core.Entry source)
        {
            Uid = source.Uid;
            Date = source.Date;
            Price = source.Price;
            Comment = source.Comment;
        }
    }
}