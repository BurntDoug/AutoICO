# AutoICO - Detailed Documentation

## Architecture Overview

AutoICO is built as a .NET 9 WPF (Windows Presentation Foundation) application with a clean separation between UI and business logic. The application follows MVVM (Model-View-ViewModel) pattern to maintain a clear separation of concerns.

### Core Components

1. **UI Layer (Views)**
   - MainWindow.xaml - The main application window with drag-and-drop support
   - Converters - Value converters for UI binding

2. **Business Logic (ViewModels & Services)**
   - MainViewModel - Handles the UI state and operations
   - ImageConverterService - Core service for image processing and ICO generation

3. **Image Processing**
   - Uses SixLabors.ImageSharp for image manipulation
   - Handles resizing to multiple resolutions
   - Packages images into ICO format

## Technical Details

### Image Processing Workflow

1. User selects or drags an image
2. Application validates the image type
3. If using standard sizes, the app generates icons at 256x256, 128x128, 64x64, 48x48, 32x32, and 16x16 resolutions
4. If using custom sizes, the app parses user input and generates icons at specified resolutions
5. The app combines all resolutions into a single .ico file

### Custom Sizing Logic

The application supports both standard Windows icon sizes and custom sizes defined by the user:
- Standard sizes: 256, 128, 64, 48, 32, and 16 pixels
- Custom sizes: User can specify comma-separated values

## Advanced Usage

### Command Line Operation

While primarily a GUI application, you can run AutoICO from the command line:

```powershell
# Build the project
dotnet build

# Run the application
dotnet run --project AutoICO/AutoICO.csproj
```

### Troubleshooting

Common issues and solutions:

1. **Problem**: Application fails to load images
   **Solution**: Ensure the image format is supported (JPG, PNG, BMP, GIF)

2. **Problem**: Custom sizes not working
   **Solution**: Ensure sizes are comma-separated and contain only numbers

## Future Roadmap

Planned features for future releases:

1. Batch processing of multiple images
2. Preview of all icon sizes being generated
3. Additional icon formats support (SVG, ICNS)
4. Command-line interface for automation
5. Color depth options for icon generation

## Development Notes

### Building from Source

1. Clone the repository
2. Open the solution in Visual Studio 2022 or later
3. Restore NuGet packages
4. Build the solution

### Testing

Unit tests focus on the core image conversion functionality:
- Image format validation
- Resizing logic
- ICO file generation

## Deployment

### MSIX Packaging (Future)

The application may be packaged using MSIX for easier distribution and potential Microsoft Store listing.

Requirements for MSIX packaging:
- Windows 10/11 SDK
- Digital certificate for signing (for production use) 