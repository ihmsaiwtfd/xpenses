using Autofac;
using Core;
using Core.Dto;
using Core.Interfaces;
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
    public class MainVM : INotifyPropertyChanged, IOutputPort<AddEntryResponse>
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            Initialize();
        }

        private void Initialize()
        {
        }

        private async void AddEntry()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IAddEntryUseCase>().Handle(new AddEntryRequest(100.0m, DateTime.Parse("2019-12-12"), "a comment", new[] { Guid.NewGuid() }), this);
            }
        }

        public void Handle(AddEntryResponse response)
        {
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
