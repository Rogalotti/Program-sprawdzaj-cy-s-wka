using System.Windows;
using static WordsFromZ3R0.Utils.DatabaseUtils;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            InitializeDatabase();
            mainWindow.Content = new MainPage();
        }
    }
}
