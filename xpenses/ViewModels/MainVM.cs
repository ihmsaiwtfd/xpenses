using Autofac;
using Core;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using DAL.Xml;
using GUI.Controls;
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

namespace GUI.ViewModels
{
    internal interface IMainResponseHandler : IOutputPort<GetEntriesResponse>, IOutputPort<DeleteEntriesResponse>
    {
    }

    public class MainVM : ViewModelBase, IMainResponseHandler
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

        private ICommand _EditCategoriesCommand;

        public ICommand EditCategoriesCommand
        {
            get
            {
                if (_EditCategoriesCommand == null)
                {
                    _EditCategoriesCommand = new RelayCommand(o => EditCategories());
                }
                return _EditCategoriesCommand;
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

        public string CurrentMonthTotal => Entries?.Sum(o => o.Price).ToString();

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
            OnPropertyChanged(nameof(CurrentMonthTotal));
        }

        private void AddEntry()
        {
            EntryEditor editor = new EntryEditor();
            if (editor.ShowDialog() == true)
            {
                ReloadEntries();
            }
        }

        private async void DeleteEntries()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IDeleteEntriesUseCase>().Handle(new DeleteEntriesRequest(SelectedEntries.OfType<Entry>().ToArray()), this);
            }
        }

        private void EditCategories()
        {
            CategoriesEditor editor = new CategoriesEditor();
            editor.ShowDialog();
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
                OnPropertyChanged(nameof(CurrentMonthTotal));
            }
        }

        public void Handle(DeleteEntriesResponse response)
        {
            ReloadEntries();
        }
    }
}
