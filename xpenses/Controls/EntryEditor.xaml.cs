using GUI.ViewModels;
using System.Windows;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for EntryEditor.xaml
    /// </summary>
    public partial class EntryEditor : Window
    {
        public EntryEditor()
        {
            InitializeComponent();
            DataContext = new EntryEditorVM();
        }
    }
}
