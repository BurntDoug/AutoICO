using AutoICO.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
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
            DataContext = _viewModel; // Set ViewModel as DataContext
            
            // Set initial values for the checkbox
            PresetSizesCheckBox.IsChecked = true;
            CustomSizesTextBox.IsEnabled = false;
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
                    StatusTextBlock.Text = _viewModel.StatusMessage;
                    MessageBox.Show($"Successfully created ICO file at: {_viewModel.OutputPath}", 
                        "Conversion Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    StatusTextBlock.Text = _viewModel.StatusMessage;
                    MessageBox.Show("Failed to convert the image to ICO format.", 
                        "Conversion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error: {ex.Message}";
                MessageBox.Show($"An error occurred: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ConvertButton.IsEnabled = true;
                SelectFileButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Event handler for the browse output button click.
        /// </summary>
        private void BrowseOutputButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Icon Files|*.ico|All Files|*.*",
                Title = "Save ICO File",
                DefaultExt = ".ico"
            };

            // Set initial directory if we have a selected image path
            if (!string.IsNullOrEmpty(_viewModel.SelectedImagePath))
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(_viewModel.SelectedImagePath);
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(_viewModel.SelectedImagePath);
            }

            if (saveFileDialog.ShowDialog() == true)
            {
                _viewModel.OutputPath = saveFileDialog.FileName;
            }
        }

        /// <summary>
        /// Event handler for the preset sizes checkbox check.
        /// </summary>
        private void PresetSizesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _viewModel.UseStandardSizes = true;
            CustomSizesTextBox.IsEnabled = false;
        }

        /// <summary>
        /// Event handler for the preset sizes checkbox uncheck.
        /// </summary>
        private void PresetSizesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewModel.UseStandardSizes = false;
            CustomSizesTextBox.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for drag over.
        /// </summary>
        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                if (files.Length > 0 && IsValidImageFile(files[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Event handler for drop.
        /// </summary>
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                if (files.Length > 0 && IsValidImageFile(files[0]))
                {
                    _viewModel.SelectedImagePath = files[0];
                }
                else
                {
                    MessageBox.Show("Please drop a valid image file (JPG, PNG, BMP, or GIF).", 
                        "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Checks if a file is a valid image file.
        /// </summary>
        private bool IsValidImageFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

            return validExtensions.Contains(extension);
        }
    }
}