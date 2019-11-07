using Core;
using Core.Interfaces.UseCases;
using DAL.Xml;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI
{
    namespace Test
    {
        class Controller
        {
            IUseCaseInput input;

            Controller(IUseCaseInput input)
            {
                this.input = input;
            }

            public void Search(int clientId)
            {
                input.Search(clientId);
            }
        }

        interface IUseCaseInput
        {
            void Search(int clientId);
        }

        class UseCaseInteractor : IUseCaseInput
        {
            IUseCaseOutput output;

            UseCaseInteractor(IUseCaseOutput output)
            {
                this.output = output;
            }

            public void Search(int clientId)
            {
                object client = null;
                if (client == null)
                {
                    output.SetError("Argh!.");
                }
                else
                {
                    output.SetClient(client);
                }
            }
        }

        interface IUseCaseOutput
        {
            void SetError(string message);
            void SetClient(object client);
        }

        class Presenter : IUseCaseOutput
        {
            string Error { get; set; }
            string Name { get; set; }
            public void SetError(string message)
            {
                Error = message;
            }
            public void SetClient(object client)
            {
                Name = client.ToString();
            }
        }
    }

    public class MainVM : INotifyPropertyChanged
    {
        private ICommand _AddEntryCommand;

        public ICommand AddEntryCommand
        {
            get
            {
                if (_AddEntryCommand == null)
                {
                    _AddEntryCommand = new RelayCommand(o => AddEntry());
                }
                return _AddEntryCommand;
            }
        }

        private IAddEntryUseCase _AddEntryUseCase;

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

        public MainVM(IAddEntryUseCase addEntryUseCase)
        {
            _AddEntryUseCase = addEntryUseCase;
        }

        public async Task Initialize()
        {
            await Task.Run(() =>
            {
                //XmlDataAdapter adapter = new XmlDataAdapter(new IsolatedStorage());
                //adapter.Load();
                //Entries = new ObservableCollection<Entry>(adapter.Entries);
            });
        }

        private async void AddEntry()
        {
            // test
            await _AddEntryUseCase.Handle(new Core.Dto.AddEntryRequest(100.0m, DateTime.Parse("2019-12-12"), "a comment", new[] { Guid.NewGuid() }), null);
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
