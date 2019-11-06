using System.IO;
using System.IO.IsolatedStorage;

namespace DAL.Xml
{
    public class IsolatedStorage : IFileStorage
    {
        public Stream Load(string name)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null))
            {
                return isoStore.OpenFile(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
        }

        public void Save(Stream stream, string name)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User, null, null))
            {
                isoStore.OpenFile(name, FileMode.Create, FileAccess.Write, FileShare.None);
            }
        }
    }
}
