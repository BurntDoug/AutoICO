using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AutoICO.Services
{
    /// <summary>
    /// Service responsible for converting images to .ico format with multiple resolutions.
    /// </summary>
    public class ImageConverterService
    {
        // Standard icon sizes for Windows
        private static readonly int[] StandardIconSizes = { 256, 128, 64, 48, 32, 16 };

        /// <summary>
        /// Converts an image file to .ico format with multiple resolutions.
        /// </summary>
        /// <param name="sourceImagePath">Path to the source image file.</param>
        /// <param name="outputPath">Path where the .ico file will be saved.</param>
        /// <param name="customSizes">Optional custom sizes to include in the .ico file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sourceImagePath or outputPath is null.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the source image file does not exist.</exception>
        public async Task ConvertToIcoAsync(string sourceImagePath, string outputPath, int[]? customSizes = null)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(sourceImagePath))
                throw new ArgumentNullException(nameof(sourceImagePath));
            
            if (string.IsNullOrEmpty(outputPath))
                throw new ArgumentNullException(nameof(outputPath));
            
            if (!File.Exists(sourceImagePath))
                throw new FileNotFoundException("Source image file not found.", sourceImagePath);

            // Create directory for output file if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? string.Empty);

            // Use custom sizes if provided, otherwise use standard sizes
            var iconSizes = customSizes ?? StandardIconSizes;

            // Create a list to hold all the resized images
            var iconImages = new List<Image>();

            try
            {
                // Load the source image
                using var sourceImage = await Image.LoadAsync(sourceImagePath);
                
                // Create resized versions for each size
                foreach (var size in iconSizes)
                {
                    var resizedImage = sourceImage.Clone(ctx => ctx.Resize(size, size));
                    iconImages.Add(resizedImage);
                }

                // Write the ICO file
                using var outputStream = new FileStream(outputPath, FileMode.Create);
                await WriteIcoFileAsync(outputStream, iconImages);
            }
            finally
            {
                // Dispose all resized images
                foreach (var image in iconImages)
                {
                    image.Dispose();
                }
            }
        }

        /// <summary>
        /// Writes the ICO file to the specified stream.
        /// </summary>
        /// <param name="outputStream">The output stream to write to.</param>
        /// <param name="images">The images to include in the ICO file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task WriteIcoFileAsync(Stream outputStream, List<Image> images)
        {
            using var writer = new BinaryWriter(outputStream);
            
            // Write ICO header (6 bytes)
            writer.Write((short)0); // Reserved, must be 0
            writer.Write((short)1); // Type: 1 = ICO, 2 = CUR
            writer.Write((short)images.Count); // Number of images
            
            // Calculate the offset to the first image data
            int headerSize = 6; // 6 bytes for ICO header
            int directorySize = 16 * images.Count; // 16 bytes per directory entry
            int currentOffset = headerSize + directorySize;
            
            // Temporarily store image data in memory
            var imageDataList = new List<byte[]>();
            
            // Write directory entries
            for (int i = 0; i < images.Count; i++)
            {
                var image = images[i];
                
                // Convert image to PNG
                using var ms = new MemoryStream();
                await image.SaveAsPngAsync(ms);
                byte[] imageData = ms.ToArray();
                
                // Store the image data
                imageDataList.Add(imageData);
                
                // Write directory entry (16 bytes)
                byte width = (byte)(image.Width >= 256 ? 0 : image.Width); // 0 means 256
                byte height = (byte)(image.Height >= 256 ? 0 : image.Height); // 0 means 256
                writer.Write(width); // Width
                writer.Write(height); // Height
                writer.Write((byte)0); // Color palette size (0 for PNG)
                writer.Write((byte)0); // Reserved, must be 0
                writer.Write((short)0); // Color planes (0 for PNG)
                writer.Write((short)32); // Bits per pixel (32 for RGBA PNG)
                writer.Write(imageData.Length); // Size of image data in bytes
                writer.Write(currentOffset); // Offset of image data from start of file
                
                // Update the offset for the next image
                currentOffset += imageData.Length;
            }
            
            // Write the image data
            for (int i = 0; i < images.Count; i++)
            {
                writer.Write(imageDataList[i]);
            }
            
            await outputStream.FlushAsync();
        }
    }
} 