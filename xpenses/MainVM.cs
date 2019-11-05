using Core;
using DAL.Xml;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace xpenses
{
    public class MainVM : INotifyPropertyChanged
    {
        private ObservableCollection<Entry> _Entries;

        public ObservableCollection<Entry> Entries
        {
            get => _Entries;
            set
            {
                if (_Entries != value)
                {
                    _Entries = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainVM()
        {
        }

        public async Task Initialize()
        {
            await Task.Run(() =>
            {
                XmlDataAdapter adapter = new XmlDataAdapter(new IsolatedStorage());
                adapter.Load();
                Entries = new ObservableCollection<Entry>(adapter.Entries);
            });
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
#if DEBUG
                throw new ArgumentNullException(nameof(propertyName));
#else
                return;
#endif
            }
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
