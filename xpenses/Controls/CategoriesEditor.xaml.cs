using GUI.ViewModels;
using System.Windows;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for CategoriesEditor.xaml
    /// </summary>
    public partial class CategoriesEditor : Window
    {
        public CategoriesEditor()
        {
            InitializeComponent();
            DataContext = new CategoriesEditorVM();
        }
    }
}
