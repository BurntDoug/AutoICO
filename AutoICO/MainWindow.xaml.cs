using AutoICO.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace AutoICO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
        }

        /// <summary>
        /// Event handler for the SelectFile button click.
        /// </summary>
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
                Title = "Select an Image File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.SelectedImagePath = openFileDialog.FileName;
                ConvertButton.IsEnabled = true;
                
                // In a full implementation, we would show a preview of the image here
            }
        }

        /// <summary>
        /// Event handler for the Convert button click.
        /// </summary>
        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertButton.IsEnabled = false;
            SelectFileButton.IsEnabled = false;
            
            try
            {
                bool success = await _viewModel.ConvertAsync();
                
                if (success)
                {
                    MessageBox.Show($"Successfully created ICO file at: {_viewModel.OutputPath}", 
                        "Conversion Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to convert the image to ICO format.", 
                        "Conversion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ConvertButton.IsEnabled = true;
                SelectFileButton.IsEnabled = true;
            }
        }
    }
}