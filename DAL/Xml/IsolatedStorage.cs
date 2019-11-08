using System.IO;
using System.IO.IsolatedStorage;

namespace DAL.Xml
{
    public class IsolatedStorage : IFileStorage
    {
        public Stream Load(string name)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine, null, null))
            {
                return isoStore.OpenFile(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
        }

        public void Save(Stream stream, string name)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine, null, null))
            {
                string dir = Path.GetDirectoryName(name);
                if (!isoStore.DirectoryExists(dir))
                    isoStore.CreateDirectory(dir);

                using (var file = isoStore.OpenFile(name, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(file);
                    file.Flush();
                }
            }
        }
    }
}
