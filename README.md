# AutoICO

A .NET 9 WinUI 3 app to convert images into multi-resolution .ico files.

## Overview

AutoICO is designed to automate the process of converting images (PNG, JPG, etc.) into Windows-compatible multi-resolution .ico files. Our goal is to streamline the manual tasks of resizing images to multiple sizes (256x256, 128x128, 64x64, 32x32, and 16x16) and bundling them into a single .ico file.

## Features

- **Simple Drag-and-Drop or File Picker** for source images
- **Automatic Resizing** to standard Windows icon resolutions
- **Optional Custom Settings** (e.g., custom output directories, advanced sizing options)
- **User-Friendly UI** with modern WinUI 3 design principles

## Build Requirements

- .NET 9 (Preview)
- Windows App SDK
- Visual Studio 2022 or later with Windows development workload

## Building and Running

1. Clone the repository
2. Open the solution in Visual Studio
3. Build the solution
4. Run the application

## Using the CLI

```powershell
# Build the project
dotnet build

# Run the application
dotnet run
```

## Dependencies

- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) - For image processing

## License

This project is licensed under the MIT License - see the LICENSE file for details. 