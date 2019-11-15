using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GUI.Controls
{
    public class CategoriesListBox : ListBox
    {
        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => SetValue(SelectedItemsListProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CategoriesListBox));

        public CategoriesListBox()
        {
            SelectionChanged += EntriesDataGrid_SelectionChanged;
            Loaded += CategoriesListBox_Loaded;
        }

        private void CategoriesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }

        private void EntriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }
    }
}
