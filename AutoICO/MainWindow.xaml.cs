using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace AutoICO;

/// <summary>
/// Main window for the AutoICO application.
/// </summary>
public sealed partial class MainWindow : Window
{
    private string? _selectedImagePath;

    public MainWindow()
    {
        this.InitializeComponent();
        Title = "AutoICO";
    }

    /// <summary>
    /// Event handler for the SelectFile button click.
    /// </summary>
    private async void SelectFileButton_Click(object sender, RoutedEventArgs e)
    {
        var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
        
        // Initialize the file picker
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
        
        filePicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        filePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        filePicker.FileTypeFilter.Add(".jpg");
        filePicker.FileTypeFilter.Add(".jpeg");
        filePicker.FileTypeFilter.Add(".png");
        filePicker.FileTypeFilter.Add(".bmp");
        filePicker.FileTypeFilter.Add(".gif");

        var file = await filePicker.PickSingleFileAsync();
        if (file != null)
        {
            _selectedImagePath = file.Path;
            ConvertButton.IsEnabled = true;
            
            // In a full implementation, we would show a preview of the image here
            // and enable the convert button
        }
    }
}