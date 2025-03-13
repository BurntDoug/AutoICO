using AutoICO.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoICO.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
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
                if (_selectedImagePath != value)
                {
                    _selectedImagePath = value;
                    OnPropertyChanged();
                    
                    // Default output path is the same directory with .ico extension
                    if (!string.IsNullOrEmpty(value))
                    {
                        OutputPath = Path.ChangeExtension(value, ".ico");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the output path for the .ico file.
        /// </summary>
        public string? OutputPath
        {
            get => _outputPath;
            set
            {
                if (_outputPath != value)
                {
                    _outputPath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Converts the selected image to .ico format.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> ConvertAsync()
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
} 