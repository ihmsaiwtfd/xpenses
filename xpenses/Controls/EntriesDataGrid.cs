using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Controls
{
    public class EntriesDataGrid : DataGrid
    {
        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => SetValue(SelectedItemsListProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(EntriesDataGrid));

        public EntriesDataGrid()
        {
            SelectionChanged += EntriesDataGrid_SelectionChanged;
        }

        private void EntriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }
    }
}
