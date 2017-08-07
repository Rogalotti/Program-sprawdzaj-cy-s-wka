using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordsFromZ3R0.Pages
{
    /// <summary>
    /// Interaction logic for BadConnectionPage.xaml
    /// </summary>
    public partial class BadConnectionPage : Page
    {
        public BadConnectionPage()
        {
            InitializeComponent();
        }

        private void badConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new MainPage();
        }
    }
}
