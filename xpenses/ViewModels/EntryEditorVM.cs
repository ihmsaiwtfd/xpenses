using Autofac;
using Core;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class EntryEditorVM : ViewModelBase,
        IDataErrorInfo,
        IOutputPort<AddEntryResponse>,
        IOutputPort<GetCategoriesResponse>
    {
        private RelayCommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand((param) => SaveAndExit((Window)param), (param) => CanSaveAndExit((Window)param));
                }
                return _SaveCommand;
            }
        }

        private decimal _Price;

        public decimal Price
        {
            get => _Price;
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _Date = DateTime.Today;

        public DateTime Date
        {
            get => _Date;
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _Comment;

        public string Comment
        {
            get => _Comment;
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Category> _AllCategories;

        public ObservableCollection<Category> AllCategories
        {
            get => _AllCategories;
            set
            {
                if (_AllCategories != value)
                {
                    _AllCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        private IList _SelectedCategories;

        public IList SelectedCategories
        {
            get => _SelectedCategories;
            set
            {
                if (_SelectedCategories != value)
                {
                    _SelectedCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Price):
                        if (Price < 0)
                        {
                            error = "Price should be a positive number";
                        }
                        break;
                    default:
                        break;
                }
                return error;
            }
        }

        public EntryEditorVM()
        {
            AllCategories = new ObservableCollection<Category>();
            ReloadCategories();
        }

        private async void ReloadCategories()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IGetCategoriesUseCase>().Handle(new GetCategoriesRequest(), this);
            }
        }

        private bool CanSaveAndExit(Window dialog)
        {
            bool isValid = dialog == null ? true : IsValid(dialog);
            bool hasCategories = (SelectedCategories?.Count ?? 0) > 0;
            return isValid && hasCategories;
        }

        private async void SaveAndExit(Window dialog)
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IAddEntryUseCase>().Handle(new AddEntryRequest(Price, Date, Comment, SelectedCategories.OfType<Category>().Select(o => o.Uid).ToArray()), this);
            }
            dialog.DialogResult = true;
            dialog.Close();
        }

        private bool IsValid(DependencyObject obj)
        {
            return !Validation.GetHasError(obj) &&
                LogicalTreeHelper.GetChildren(obj)
                    .OfType<DependencyObject>()
                    .All(IsValid);
        }

        public void Handle(AddEntryResponse response)
        {
        }

        public void Handle(GetCategoriesResponse response)
        {
            if (response.Success)
            {
                AllCategories.Clear();
                foreach (Category cat in response.Categories)
                {
                    AllCategories.Add(cat);
                }
            }
        }
    }
}
