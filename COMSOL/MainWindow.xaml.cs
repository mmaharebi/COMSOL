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

using COMSOL.com;

namespace COMSOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Parameter> parameters = new List<Parameter>();
        List<string> shapesNames = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            // set parameters_listView
            parameters_listView.ItemsSource = parameters;
            parameters_listView.Items.Refresh();

            // set geometry_comboBox
            shapesNames.Add("--Select--");
            shapesNames.Add("Line");
            shapesNames.Add("Rectangle");
            shapesNames.Add("Ellipse");
            geometry_comboBox.ItemsSource = shapesNames;
            geometry_comboBox.SelectedIndex = 0;
            geometry_comboBox.Items.Refresh();
            

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

        private void navigation_treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            parameters_border.Visibility = Visibility.Hidden;
            geometry_border.Visibility = Visibility.Hidden;

            if (navigation_treeView.SelectedItem == parameters_treeViewItem)
            {
                parameters_border.Visibility = Visibility.Visible;
            }

            if (navigation_treeView.SelectedItem == geometry_treeViewItem)
            {
                geometry_border.Visibility = Visibility.Visible;
            }
        }

        private void parameters_listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeParameter_button.IsEnabled = false;
            if (parameters_listView.SelectedItems != null)
            {
                removeParameter_button.IsEnabled = true;
            }
        }

        private void removeParameter_button_Clicked(object sender, RoutedEventArgs e)
        {
            Parameter selectedParameter = parameters_listView.SelectedItem as Parameter;
            parameters.Remove(selectedParameter);
            parameters_listView.Items.Refresh();
            removeParameter_button.IsEnabled = false;
        }

        private void addParameter_button_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool valid = true;
                string newParameterName = newParameterName_textBox.Text;
                valid = valid && (newParameterName != null);
                List<string> names = new List<string>();
                if (parameters.Count > 0)
                {
                    foreach (Parameter parameter in parameters)
                    {
                        names.Add(parameter.name);
                    }
                }
                valid = valid && (names.IndexOf(newParameterName) == -1);
                double newParameterValue = Convert.ToDouble(newParameterValue_textBox.Text);
                string newParameterDescription = newParameterDescription_textBox.Text;
                if (valid)
                {
                    parameters.Add(new Parameter { name = newParameterName, value = newParameterValue, description = newParameterDescription });
                    parameters_listView.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Inputs are not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something is wrong.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void geometry_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drawLine_grid.Visibility = Visibility.Hidden;
            drawRectangle_grid.Visibility = Visibility.Hidden;
            drawEllipse_grid.Visibility = Visibility.Hidden;

            switch (geometry_comboBox.SelectedIndex)
            {
                case 1:
                    drawLine_grid.Visibility = Visibility.Visible;
                    break;
                case 2:
                    drawRectangle_grid.Visibility = Visibility.Visible;
                    break;
                case 3:
                    drawEllipse_grid.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
