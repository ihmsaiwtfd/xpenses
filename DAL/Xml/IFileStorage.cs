using System.IO;

namespace DAL.Xml
{
    internal interface IFileStorage
    {
        Stream Load(string name);
        void Save(Stream stream, string name);
    }
}
