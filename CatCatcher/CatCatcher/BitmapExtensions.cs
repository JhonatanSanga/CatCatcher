using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;

namespace CatCatcher
{
    public static class BitmapExtensions
    {
        public static Bitmap EuclideanFilter(this Bitmap source, Color color, short radius = 100)
        {
            var image = (Bitmap)source.Clone();
            var filter =
                new EuclideanColorFiltering
                {
                    CenterColor = new RGB(Color.FromArgb(color.R, color.G, color.B)),
                    Radius = radius
                };

            filter.ApplyInPlace(image);
            return image;
        }
        public static Bitmap FindObjectsOnFiltered(this Bitmap source, Color penColor, int minObjectWidth = 5, int minObjectHeight = 5, bool multiple = false)
        {
            var image = (Bitmap)source.Clone();
            BlobCounter blobCounter = new BlobCounter
            {
                MinWidth = minObjectWidth,
                MinHeight = minObjectHeight,
                FilterBlobs = true,
                ObjectsOrder = ObjectsOrder.Size
            };

            var objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            var grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            image.UnlockBits(objectsData);

            blobCounter.ProcessImage(image);
            var rects = blobCounter.GetObjectsRectangles();

            if (rects.Length <= 0) return source;

            Graphics g = Graphics.FromImage(source);
            for (int i = 0; rects.Length > i; i++)
            {
                Rectangle objectRect = rects[i];
                using (Pen pen = new Pen(Color.FromArgb(penColor.R, penColor.G, penColor.B), 2))
                {
                    g.DrawRectangle(pen, objectRect);

                    if (multiple)
                        g.DrawString((i + 1).ToString(), new Font("Arial", 12), Brushes.Red, objectRect);
                }

                if (multiple) continue;
                g.Dispose();
                return source;
            }

            g.Dispose();
            return source;
        }
        public static Bitmap FindObjectsOnOriginal(this Bitmap source, Color penColor, Color filterColor, short filterRadius = 100, int minObjectWidth = 5, int minObjectHeight = 5, bool multiple = false)
        {
            var fake = (Bitmap)source.Clone();//copia la imagen
            var image = fake.EuclideanFilter(filterColor, filterRadius);//filtro euclidiano
            var orj = (Bitmap)source.Clone();//copia la imagen

            BlobCounter blobCounter = new BlobCounter
            {
                MinWidth = minObjectWidth,
                MinHeight = minObjectHeight,
                FilterBlobs = true,
                ObjectsOrder = ObjectsOrder.Size
            };//constructor

            /*var objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);//guarda la informacion
            var grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);//crear un filtro
            grayscaleFilter.Apply(new UnmanagedImage(objectsData));//aplica el filtro
            image.UnlockBits(objectsData);//desbloque la informacion
            */
            blobCounter.ProcessImage(image);//construye los blobs
            var rects = blobCounter.GetObjectsRectangles();//

            if (rects.Length <= 0) return orj;

            Graphics g = Graphics.FromImage(orj);

            Lista.Coords.Clear();

            for (int i = 0; rects.Length > i; i++)
            {
                Rectangle objectRect = rects[i];
                using (Pen pen = new Pen(Color.FromArgb(penColor.R, penColor.G, penColor.B), 8))
                {
                    //g.DrawRectangle(pen, objectRect);//dibuja el rectangulo
                    g.DrawImage(System.Drawing.Image.FromFile(@"C:\Users\PC-2\Downloads\fotos\focus2.png"), objectRect);

                    Lista.Coords.Add("X: " + objectRect.X + "Y: " + objectRect.Y);

                    NativeMethods.SendMouseInput(objectRect.X, objectRect.Y, 1360, 768, false);

                    if (multiple)
                        g.DrawString((i + 1).ToString(), new Font("Arial", 60), Brushes.Red, objectRect);//pone el numero
                }

                if (multiple) continue;
                g.Dispose();//borrable
                return orj;//borrable
            }

            g.Dispose();//libera el espacio en la memoria
            return orj;

        }
    }
}
