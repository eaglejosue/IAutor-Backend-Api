using System.Drawing;

namespace IAutor.Api.Helpers.Extensions;

public static class ImageExtensions
{
    public static Stream ResizeImage(IFormFile file, int maxWidth, int maxHeight)
    {
        using var stream = file.OpenReadStream();
        byte[] imageBytes;
        using (Image img = Image.FromStream(stream))
        {
            var size = ResizeKeepAspect(new Size(img.Width, img.Height), img.Width > maxWidth ? maxWidth : img.Width, img.Height > maxHeight ? maxHeight : img.Height);
            using (Bitmap b = new Bitmap(img, size))
            {
                using (MemoryStream ms2 = new MemoryStream())
                {
                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageBytes = ms2.ToArray();

                    Stream streamRet = new MemoryStream(imageBytes);
                    return streamRet;

                }
            }
        }
    }

    public static Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight, bool enlarge = false)
    {
        maxWidth = enlarge ? maxWidth : Math.Min(maxWidth, src.Width);
        maxHeight = enlarge ? maxHeight : Math.Min(maxHeight, src.Height);

        decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
        return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
    }
}
