using Autofac;
using Core;
using Core.Dto;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class CategoriesEditorVM : ViewModelBase,
        IOutputPort<AddCategoryResponse>,
        IOutputPort<GetCategoriesResponse>,
        IOutputPort<UpdateCategoryRelationsResponse>,
        IOutputPort<DeleteCategoriesResponse>
    {
        private CategoryRelation[] _Relations;

        #region Commands

        private RelayCommand _AddCategoryCommand;

        public ICommand AddCategoryCommand
        {
            get
            {
                if (_AddCategoryCommand == null)
                {
                    _AddCategoryCommand = new RelayCommand((param) => AddCategory(), (param) => CanAddCategory());
                }
                return _AddCategoryCommand;
            }
        }

        private RelayCommand _ApplyParentsCommand;

        public ICommand ApplyParentsCommand
        {
            get
            {
                if (_ApplyParentsCommand == null)
                {
                    _ApplyParentsCommand = new RelayCommand((param) => SaveSelectedParents(), (param) => CanSaveSelectedParents());
                }
                return _ApplyParentsCommand;
            }
        }

        private RelayCommand _DeleteCategoryCommand;

        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (_DeleteCategoryCommand == null)
                {
                    _DeleteCategoryCommand = new RelayCommand((param) => DeleteCategory(), (param) => CanDeleteCategory());
                }
                return _DeleteCategoryCommand;
            }
        }

        #endregion Commands

        #region Public properties

        private ObservableCollection<Category> _Categories;

        public ObservableCollection<Category> Categories
        {
            get => _Categories;
            set
            {
                if (_Categories != value)
                {
                    _Categories = value;
                    OnPropertyChanged();
                }
            }
        }

        private IList _ParentCategories;

        public IList ParentCategories
        {
            get => _ParentCategories;
            set
            {
                if (_ParentCategories != value)
                {
                    _ParentCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        private Category _SelectedCategory;

        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                if (_SelectedCategory != value)
                {
                    _SelectedCategory = value;
                    OnSelectedCategoryChanged();
                    OnPropertyChanged();
                }
            }
        }

        private string _CategoryName;

        public string CategoryName
        {
            get => _CategoryName;
            set
            {
                if (_CategoryName != value)
                {
                    _CategoryName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _CategoryDescription;

        public string CategoryDescription
        {
            get => _CategoryDescription;
            set
            {
                if (_CategoryDescription != value)
                {
                    _CategoryDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Public properties

        public CategoriesEditorVM()
        {
            _Categories = new ObservableCollection<Category>();
            ReloadCategories();
        }

        private async void ReloadCategories()
        {
            SelectedCategory = null;
            _Relations = null;
            await ReloadCategoriesAsync();
        }

        private async Task ReloadCategoriesAsync()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IGetCategoriesUseCase>().Handle(new GetCategoriesRequest(), this);
            }
        }

        private bool CanAddCategory()
        {
            return !string.IsNullOrEmpty(CategoryName);
        }

        private async void AddCategory()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IAddCategoryUseCase>().Handle(new AddCategoryRequest(CategoryName, CategoryDescription), this);
            }
            CategoryName = null;
            CategoryDescription = null;
        }

        private bool CanDeleteCategory()
        {
            return SelectedCategory != null;
        }

        private async void DeleteCategory()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IDeleteCategoriesUseCase>().Handle(new DeleteCategoriesRequest(new[] { SelectedCategory }), this);
            }
        }

        private async void OnSelectedCategoryChanged()
        {
            ParentCategories?.Clear();
            if (_Relations == null)
            {
                await ReloadCategoriesAsync();
            }
            SelectParents();
        }

        private void SelectParents()
        {
            if (SelectedCategory == null || _Relations == null)
                return;

            Guid[] parentUids = _Relations
                .Where(o => o.ChildUid == SelectedCategory.Uid)
                .Select(o => o.ParentUid)
                .ToArray();
            foreach (var cat in Categories.Where(o => parentUids.Contains(o.Uid)))
            {
                ParentCategories.Add(cat);
            }
        }

        private bool CanSaveSelectedParents()
        {
            return SelectedCategory != null && ParentCategories != null;
        }

        private async void SaveSelectedParents()
        {
            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                await scope.Resolve<IUpdateCategoryRelationsUseCase>().Handle(
                    new UpdateCategoryRelationsRequest(SelectedCategory.Uid, ParentCategories.OfType<Category>().Select(o => o.Uid).ToArray()), this);
            }
        }

        public void Handle(AddCategoryResponse response)
        {
            if (response.Success)
            {
                Category cat = response.AddedCategory;
                Categories.Add(cat);
                SelectedCategory = cat;
            }
        }

        public void Handle(GetCategoriesResponse response)
        {
            if (response.Success)
            {
                Categories.Clear();
                foreach (Category cat in response.Categories)
                {
                    Categories.Add(cat);
                }
                _Relations = response.Relations;
            }
        }

        public void Handle(UpdateCategoryRelationsResponse response)
        {
            if (response.Success)
            {
                ReloadCategories();
            }
        }

        public void Handle(DeleteCategoriesResponse response)
        {
            if (response.Success)
            {
                ReloadCategories();
            }
        }
    }
}
