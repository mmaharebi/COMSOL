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

using NumSharp;

using COMSOL.com;

namespace COMSOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Data management class instance
        private Data data = new Data();

        // to know wich "Select Parameter" button clicked
        private string targetTextBox;

        public MainWindow()
        {
            InitializeComponent();

            // set parameters_listView
            parameters_listView.ItemsSource = data.GetParameters();
            parameters_listView.Items.Refresh();

            // set newParameterUnitScale_comboBox
            newParameterUnitScale_comboBox.ItemsSource = data.GetUnits();
            newParameterUnitScale_comboBox.Items.Refresh();
            newParameterUnitScale_comboBox.SelectedIndex = data.GetUnits().IndexOf("--Select--");

            // set geometry_comboBox
            geometry_comboBox.ItemsSource = data.GetShapesNames();
            geometry_comboBox.SelectedIndex = 0;
            geometry_comboBox.Items.Refresh();

            // set selectParameter_listView
            selectParameter_listView.ItemsSource = data.GetParameters();
            selectParameter_listView.Items.Refresh();
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
            material_border.Visibility = Visibility.Hidden;

            if (navigation_treeView.SelectedItem == parameters_treeViewItem)
            {
                parameters_border.Visibility = Visibility.Visible;
            }

            if (navigation_treeView.SelectedItem == geometry_treeViewItem)
            {
                geometry_border.Visibility = Visibility.Visible;
            }

            if (navigation_treeView.SelectedItem == material_treeViewItem)
            {
                material_border.Visibility = Visibility.Visible;
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
            data.removeParameter(selectedParameter);
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
                if (data.GetParameters().Count > 0)
                {
                    foreach (Parameter parameter in data.GetParameters())
                    {
                        names.Add(parameter.name);
                    }
                }
                valid = valid && (names.IndexOf(newParameterName) == -1);
                double newParameterValue = Convert.ToDouble(newParameterValue_textBox.Text);
                string newParameterDescription = newParameterDescription_textBox.Text;
                if (valid)
                {
                    data.addParameter(new Parameter { name = newParameterName, value = newParameterValue, description = newParameterDescription });
                    parameters_listView.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Inputs are not valid.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something is wrong\n" + exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void selectParameter_button_Clicked(object sender, RoutedEventArgs e)
        {
            // selectParameter_ToggleVisiblity();
            selectParameter_grid.Visibility = Visibility.Visible;

            string senderName = (sender as Button).Name;

            List<int> endPositions = GetCharPositions(senderName, '_');

            targetTextBox = senderName[0..(endPositions[1])] + "_textBox";

        }

        private void selectParameter_listView_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // check if situation is valid
                bool isValid = true;
                isValid &= (selectParameter_listView.SelectedItem != null);

                if (isValid)
                {
                    Parameter selectedParameter = selectParameter_listView.SelectedItem as Parameter;
                    (this.FindName(targetTextBox) as TextBox).Text = selectedParameter.value.ToString();
                }
                else
                {
                    MessageBox.Show("Something is wrong", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Something is wrong\n" + exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            finally
            {
                selectParameter_grid.Visibility = Visibility.Hidden;
            }
        }

        private void drawShape_button_Clicked(object sender, RoutedEventArgs e)
        {
            string senderName = (sender as Button).Name;
            List<int> endPositions = GetCharPositions(senderName, '_');
            
            string shapeName = senderName.Substring(endPositions[0] + 1, (endPositions[1] - endPositions[0] - 1));

            string shapeAngleTextBox = shapeName + '_' + "angle" + '_' + "textBox";

            string shapeAngleText = (this.FindName(shapeAngleTextBox) as TextBox).Text;
            if (shapeAngleText == "")
            {
                shapeAngleText = Convert.ToString(0);
            }

            try
            {
                double shapeAngle = Convert.ToDouble(shapeAngleText);
                drawShape(shapeName, shapeAngle);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something is wrong\n" + exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

        }

        private void drawShape(string shapeName, double shapeAngle)
        {
            // this list should be gettable from "Data" class
            List<string> shapeNames = new List<string> { "line", "rectangle", "ellipse" };

            try
            {
                // check if shape's data is valid
                bool isValid = true;
                isValid &= (shapeNames.Contains(shapeName));

                SolidColorBrush stroke = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));
                SolidColorBrush fill = new SolidColorBrush(Color.FromArgb(20, 97, 97, 100));
                double strokeThickness = 0.5;

                RotateTransform rotateTransform = new RotateTransform(shapeAngle);

                if (isValid)
                {
                    switch (shapeName)
                    {
                        case "line":
                            Line line = new Line();
                            // here
                            line.X1 = (5 / 9) * this.Width + Convert.ToDouble(line_X1_textBox.Text);
                            line.X2 = (5 / 9) *this.Width + Convert.ToDouble(line_X2_textBox.Text);
                            line.Y1 = 20 + Convert.ToDouble(line_Y1_textBox.Text);
                            line.Y2 = 20 + Convert.ToDouble(line_Y2_textBox.Text);

                            //line.RenderTransform = rotateTransform;

                            line.Stroke = stroke;
                            line.StrokeThickness = strokeThickness;
                            line.Fill = fill;

                            shapes_canvas.Children.Add(line);

                            break;
                        
                        case "rectangle":
                            Rectangle rectangle = new System.Windows.Shapes.Rectangle();
                            // here
                            rectangle.Width = Convert.ToDouble(rectangle_width_textBox.Text);
                            rectangle.Height = Convert.ToDouble(rectangle_height_textBox.Text);
                            Canvas.SetLeft(rectangle, Convert.ToDouble(rectangle_X_textBox.Text));
                            Canvas.SetTop(rectangle, Convert.ToDouble(rectangle_Y_textBox.Text));

                            rectangle.RenderTransform = rotateTransform;

                            rectangle.Stroke = stroke;
                            rectangle.StrokeThickness = strokeThickness;
                            rectangle.Fill = fill;

                            shapes_canvas.Children.Add(rectangle);
                            
                            break;

                        case "ellipse":
                            Ellipse ellipse = new Ellipse();
                            ellipse.Width = Convert.ToDouble(ellipse_D_textBox.Text);
                            ellipse.Height = Convert.ToDouble(ellipse_d_textBox.Text);
                            Canvas.SetLeft(ellipse, Convert.ToDouble(ellipse_X_textBox.Text) - ellipse.Width / 2);
                            Canvas.SetTop(ellipse, Convert.ToDouble(ellipse_Y_textBox.Text) - ellipse.Height / 2);

                            ellipse.RenderTransform = rotateTransform;

                            ellipse.Stroke = stroke;
                            ellipse.StrokeThickness = strokeThickness;
                            ellipse.Fill = fill;

                            shapes_canvas.Children.Add(ellipse);

                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Something is wrong\n" + exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void selectParameter_ToggleVisiblity()
        {
            selectParameter_listView.Items.Refresh();
            if (selectParameter_grid.Visibility == Visibility.Hidden)
            {
                selectParameter_grid.Visibility = Visibility.Visible;
            }
            else
            {
                selectParameter_grid.Visibility = Visibility.Hidden;
            }
        }

        private List<int> GetCharPositions(string str, char chr)
        {
            List<int> chrPositions = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == chr)
                {
                    chrPositions.Add(i);
                }
            }
            return new List<int>(chrPositions);
        }

        private void shapes_canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush unselectedShapeFill = new SolidColorBrush(Color.FromArgb(20, 97, 97, 100));
            SolidColorBrush selectedShapeFill = new SolidColorBrush(Color.FromArgb(20, 97, 10, 100));

            foreach (System.Windows.Shapes.Shape shape in shapes_canvas.Children)
            {
                if (shape.IsMouseOver)
                {
                    shape.Fill = selectedShapeFill;
                }
                else
                {
                    shape.Fill = unselectedShapeFill;
                }
            }
        }
    }
}
