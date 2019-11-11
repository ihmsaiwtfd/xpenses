using Autofac;
using Core;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using DAL.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI
{
    internal interface IMainResponseHandler : IOutputPort<AddEntryResponse>, IOutputPort<GetEntriesResponse>, IOutputPort<DeleteEntriesResponse>
    {
    }

    public class MainVM : INotifyPropertyChanged, IMainResponseHandler
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

        private ICommand _DeleteEntriesCommand;

        public ICommand DeleteEntriesCommand
        {
            get
            {
                if (_DeleteEntriesCommand == null)
                {
                    _DeleteEntriesCommand = new RelayCommand(o => DeleteEntries());
                }
                return _DeleteEntriesCommand;
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

        private IList _SelectedEntries;

        public IList SelectedEntries
        {
            get => _SelectedEntries;
            set
            {
                if (_SelectedEntries != value)
                {
                    _SelectedEntries = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainVM()
        {
            Entries = new ObservableCollection<Entry>();
            ReloadEntries();
        }

        private async void ReloadEntries()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IGetEntriesUseCase>().Handle(new GetEntriesRequest(), this);
            }
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void AddEntry()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IAddEntryUseCase>().Handle(new AddEntryRequest(100.0m, DateTime.Parse("2019-12-12"), "a comment", new[] { Guid.NewGuid() }), this);
            }
        }

        private async void DeleteEntries()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IDeleteEntriesUseCase>().Handle(new DeleteEntriesRequest(SelectedEntries.OfType<Entry>().ToArray()), this);
            }
        }

        public void Handle(AddEntryResponse response)
        {
            if (response.Success)
                Entries.Add(response.AddedEntry);
        }

        public void Handle(GetEntriesResponse response)
        {
            if (response.Success)
            {
                Entries.Clear();
                foreach (Entry entry in response.Entries)
                {
                    Entries.Add(entry);
                }
            }
        }

        public void Handle(DeleteEntriesResponse response)
        {
            ReloadEntries();
        }
    }
}
