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

namespace COMSOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exit_menuItem_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mainWindow_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to exit?", "Exit Application", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void about_menuItem_Clicked(object sender, RoutedEventArgs e)
        {
            string about = "About COMSOL";
            MessageBox.Show(about, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
