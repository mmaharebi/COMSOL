﻿using System;
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
        // Data management class instance
        private Data data = new Data();

        // to know wich "Select Parameter" button clicked
        private string targetTextBox;
        private Shape selectedShape;
        private int regionIdCounter = 1;

        public MainWindow()
        {
            InitializeComponent();
            primarySettings();
        }

        private void primarySettings()
        {
            // set parameters_listView
            parameters_listView.ItemsSource = data.GetParameters();
            parameters_listView.Items.Refresh();

            // set newParameterUnitScale_comboBox
            List<string> unitNames = new List<string>();
            foreach (Unit unit in data.GetUnits())
            {
                unitNames.Add(unit.name);
            }
            newParameterUnitScale_comboBox.ItemsSource = unitNames;
            newParameterUnitScale_comboBox.Items.Refresh();
            newParameterUnitScale_comboBox.SelectedItem = unitNames[10];

            // set geometry_comboBox
            geometry_comboBox.ItemsSource = data.GetShapesNames();
            geometry_comboBox.SelectedIndex = 0;
            geometry_comboBox.Items.Refresh();

            // set material_comboBox
            material_comboBox.ItemsSource = data.GetReegionTypes();
            material_comboBox.SelectedIndex = 0;
            material_comboBox.Items.Refresh();

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
                string newParameterUnitScaleName = newParameterUnitScale_comboBox.SelectedItem.ToString();
                double newParameterUnitScaleNumber = data.GetUnits()[data.GetUnits().FindIndex(unit => unit.name == newParameterUnitScaleName)].number;

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
                newParameterValue *= newParameterUnitScaleNumber;
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
            //drawLine_grid.Visibility = Visibility.Hidden;
            drawRectangle_grid.Visibility = Visibility.Hidden;
            drawEllipse_grid.Visibility = Visibility.Hidden;
            drawPolygon_grid.Visibility = Visibility.Hidden;

            switch (geometry_comboBox.SelectedIndex)
            {
                //case 1:
                //    drawLine_grid.Visibility = Visibility.Visible;
                //    break;
                case 1:
                    drawRectangle_grid.Visibility = Visibility.Visible;
                    break;
                case 2:
                    drawEllipse_grid.Visibility = Visibility.Visible;
                    break;
                case 3:
                    drawPolygon_grid.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void selectParameter_button_Clicked(object sender, RoutedEventArgs e)
        {
            selectParameter_ToggleVisiblity();
            //selectParameter_grid.Visibility = Visibility.Visible;

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
            try
            {
                // check if shape's data is valid
                bool isValid = true;
                shapeName = shapeName[0].ToString().ToUpper() + shapeName.Substring(1, shapeName.Length - 1);
                isValid &= (data.GetShapesNames().Contains(shapeName));

                SolidColorBrush fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                RotateTransform rotateTransform = new RotateTransform(shapeAngle);

                if (isValid)
                {
                    switch (shapeName)
                    {
                        //case "Line":
                        //    Line line = new Line();
                        //    line.X1 = (5 / 9) * this.Width + Convert.ToDouble(line_X1_textBox.Text);
                        //    line.X2 = (5 / 9) * this.Width + Convert.ToDouble(line_X2_textBox.Text);
                        //    line.Y1 = 20 + Convert.ToDouble(line_Y1_textBox.Text);
                        //    line.Y2 = 20 + Convert.ToDouble(line_Y2_textBox.Text);

                        //    line.Fill = fill;

                        //    shapes_canvas.Children.Add(line);

                        //    break;

                        case "Rectangle":
                            Rectangle rectangle = new System.Windows.Shapes.Rectangle();
                            // here
                            rectangle.Width = Convert.ToDouble(rectangle_width_textBox.Text);
                            rectangle.Height = Convert.ToDouble(rectangle_height_textBox.Text);
                            Canvas.SetLeft(rectangle, Convert.ToDouble(rectangle_X_textBox.Text));
                            Canvas.SetTop(rectangle, Convert.ToDouble(rectangle_Y_textBox.Text));

                            rectangle.RenderTransform = rotateTransform;

                            rectangle.Fill = fill;

                            shapes_canvas.Children.Add(rectangle);
                            
                            break;

                        case "Ellipse":
                            Ellipse ellipse = new Ellipse();
                            ellipse.Width = Convert.ToDouble(ellipse_D_textBox.Text);
                            ellipse.Height = Convert.ToDouble(ellipse_d_textBox.Text);
                            Canvas.SetLeft(ellipse, Convert.ToDouble(ellipse_X_textBox.Text) - ellipse.Width / 2);
                            Canvas.SetTop(ellipse, Convert.ToDouble(ellipse_Y_textBox.Text) - ellipse.Height / 2);

                            ellipse.RenderTransform = rotateTransform;

                            ellipse.Fill = fill;

                            shapes_canvas.Children.Add(ellipse);

                            break;
                        case "Polygon":
                            if (data.polygon.Points.Count > 2)
                            {
                                Polygon polygon = data.polygon;
                                
                                polygon.RenderTransform = rotateTransform;

                                polygon.Fill = fill;

                                shapes_canvas.Children.Add(polygon);

                                data.polygon = new Polygon();
                            }
                            else
                            {
                                MessageBox.Show("Polygon should have more than 2 points.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Shape is not valid", "", MessageBoxButton.OK, MessageBoxImage.Error);
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
            SolidColorBrush unselectedShapeFill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush selectedShapeFill = new SolidColorBrush(Color.FromRgb(100, 100, 100));

            selectedShape = null;
            pec_assign_button.IsEnabled = false;
            dielectric_assign_button.IsEnabled = false;


            foreach (System.Windows.Shapes.Shape shape in shapes_canvas.Children)
            {
                if (shape.IsMouseOver)
                {
                    shape.Fill = selectedShapeFill;
                    selectedShape = shape;
                    pec_assign_button.IsEnabled = true;
                    dielectric_assign_button.IsEnabled = true;
                }
                else
                {
                    shape.Fill = unselectedShapeFill;
                }
            }
        }

        private void add_polygon_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.polygon.Points.Add(new Point { X = Convert.ToDouble(polygon_X_textBox.Text), Y = Convert.ToDouble(polygon_Y_textBox.Text) });
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something is wrong\n" + exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

        }

        private void cancel_polygon_button_Click(object sender, RoutedEventArgs e)
        {
            polygon_X_textBox.Text = "";
            polygon_Y_textBox.Text = "";
            data.polygon = new Polygon();

            MessageBox.Show("Data has been reseted\n You can define a new polygon.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void material_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dielectric_groupBox.Visibility = Visibility.Hidden;
            pec_groupBox.Visibility = Visibility.Hidden;

            switch (material_comboBox.SelectedIndex)
            {
                case 1:
                    pec_groupBox.Visibility = Visibility.Visible;
                    break;
                case 2:
                    dielectric_groupBox.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void material_assign_button_Click(object sender, RoutedEventArgs e)
        {
            Region region = new Region { };
            region.id = regionIdCounter++;

            switch (material_comboBox.SelectedIndex)
            {
                case 1:
                    region.material = new Material { epsr = 0, mur = 0, sigma = -1 };
                    data.addRegion(region);
                    break;

                case 2:
                    try
                    {
                        region.material = new Material { epsr = Convert.ToDouble(dielectric_epsr_textBox.Text), mur = Convert.ToDouble(dielectric_mur_textBox.Text), sigma = Convert.ToDouble(dielectric_sigma_textBox.Text) };
                        data.addRegion(region);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Something is wrong\n" + exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
