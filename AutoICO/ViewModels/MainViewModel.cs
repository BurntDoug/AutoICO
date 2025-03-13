using AutoICO.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        private string _customSizes = "256,128,64,48,32,16";
        private bool _useStandardSizes = true;
        private BitmapImage? _imagePreviewSource;
        private string _statusMessage = string.Empty;

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
                    OnPropertyChanged(nameof(IsImageSelected));
                    
                    // Default output path is the same directory with .ico extension
                    if (!string.IsNullOrEmpty(value))
                    {
                        OutputPath = Path.ChangeExtension(value, ".ico");
                        LoadImagePreview(value);
                    }
                    else
                    {
                        ImagePreviewSource = null;
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
        /// Gets or sets the custom sizes for the icon.
        /// </summary>
        public string CustomSizes
        {
            get => _customSizes;
            set
            {
                if (_customSizes != value)
                {
                    _customSizes = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use standard sizes.
        /// </summary>
        public bool UseStandardSizes
        {
            get => _useStandardSizes;
            set
            {
                if (_useStandardSizes != value)
                {
                    _useStandardSizes = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the image preview source.
        /// </summary>
        public BitmapImage? ImagePreviewSource
        {
            get => _imagePreviewSource;
            set
            {
                if (_imagePreviewSource != value)
                {
                    _imagePreviewSource = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether an image is selected.
        /// </summary>
        public bool IsImageSelected => !string.IsNullOrEmpty(SelectedImagePath) && File.Exists(SelectedImagePath);

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
                // Parse custom sizes if necessary
                int[]? customSizes = null;
                if (!UseStandardSizes && !string.IsNullOrWhiteSpace(CustomSizes))
                {
                    var sizeStrings = CustomSizes.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var sizes = new System.Collections.Generic.List<int>();
                    
                    foreach (var sizeStr in sizeStrings)
                    {
                        if (int.TryParse(sizeStr.Trim(), out int size) && size > 0)
                        {
                            sizes.Add(size);
                        }
                    }
                    
                    if (sizes.Count > 0)
                    {
                        customSizes = sizes.ToArray();
                    }
                }

                // Ensure the directory exists
                string? directory = Path.GetDirectoryName(OutputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await _imageConverterService.ConvertToIcoAsync(SelectedImagePath, OutputPath, customSizes);
                StatusMessage = $"Successfully created ICO file at: {OutputPath}";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Loads an image preview.
        /// </summary>
        /// <param name="imagePath">The path to the image file.</param>
        private void LoadImagePreview(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                ImagePreviewSource = null;
                return;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load the image when initialized to avoid file lock
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();
                bitmap.Freeze(); // Optional: freeze the bitmap to make it accessible from other threads
                
                ImagePreviewSource = bitmap;
            }
            catch (Exception)
            {
                ImagePreviewSource = null;
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