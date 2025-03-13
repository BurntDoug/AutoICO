using AutoICO.Services;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoICO.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public class MainViewModel
    {
        private readonly ImageConverterService _imageConverterService;
        private string? _selectedImagePath;
        private string? _outputPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            _imageConverterService = new ImageConverterService();
        }

        /// <summary>
        /// Gets or sets the path to the selected image.
        /// </summary>
        public string? SelectedImagePath
        {
            get => _selectedImagePath;
            set 
            { 
                _selectedImagePath = value;
                // Default output path is the same directory with .ico extension
                if (!string.IsNullOrEmpty(value))
                {
                    OutputPath = Path.ChangeExtension(value, ".ico");
                }
            }
        }

        /// <summary>
        /// Gets or sets the output path for the .ico file.
        /// </summary>
        public string? OutputPath
        {
            get => _outputPath;
            set => _outputPath = value;
        }

        /// <summary>
        /// Converts the selected image to .ico format.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> ConvertAsync(ContentDialog? progressDialog = null)
        {
            if (string.IsNullOrEmpty(SelectedImagePath) || string.IsNullOrEmpty(OutputPath))
            {
                return false;
            }

            try
            {
                await _imageConverterService.ConvertToIcoAsync(SelectedImagePath, OutputPath);
                return true;
            }
            catch (Exception)
            {
                // Handle exceptions
                return false;
            }
        }
    }
} 