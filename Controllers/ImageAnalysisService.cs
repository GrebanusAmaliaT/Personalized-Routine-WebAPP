using System.Drawing;

namespace AplicatieRutina.Services
{
    public class ImageAnalysisService
    {
        public (string redness, string brightness, string darkness) Analyze(string imagePath)
        {
            using var bitmap = new Bitmap(imagePath);

            int redPixels = 0, brightPixels = 0, darkPixels = 0;
            int total = bitmap.Width * bitmap.Height;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);

                    if (pixel.R > 150 && pixel.R > pixel.G + 30 && pixel.R > pixel.B + 30)
                        redPixels++;

                    if (pixel.R > 220 && pixel.G > 220 && pixel.B > 220)
                        brightPixels++;

                    if (pixel.R < 40 && pixel.G < 40 && pixel.B < 40)
                        darkPixels++;
                }
            }

            string redness = redPixels * 100 / total > 15 ? "High" : "Normal";
            string brightness = brightPixels * 100 / total > 10 ? "High" : "Normal";
            string darkness = darkPixels * 100 / total > 10 ? "Visible" : "None";

            return (redness, brightness, darkness);
        }
    }
}
