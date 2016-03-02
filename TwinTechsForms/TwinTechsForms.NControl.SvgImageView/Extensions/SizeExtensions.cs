using System;
using NGraphics;

namespace TwinTechsForms.NControl {
    public static class SizeExtensions
    {
        public static double ScaleThatFits (this Size size, Size max)
        {
            if (size == max || max.Width == 0 || max.Height == 0 || size.Width == 0 || size.Height == 0) {
                return 1;
            }

            double widthScale = max.Width / size.Width;
            double heightScale = max.Height / size.Height;
            double fitScale = (double)Math.Min (widthScale, heightScale);
            return fitScale;
        }

        public static Size ScaleProportional (this Size size, Size max)
        {
            double fitScale = size.ScaleThatFits (max);
            if (fitScale == 1) {
                return size;
            }

            Size scaledSize = new Size (size.Width * fitScale, size.Height * fitScale);
            return scaledSize;
        }

        public static Size ScaleDownProportional (this Size size, Size max)
        {
            if (size.Width <= max.Width && size.Height <= max.Height) {
                return size;
            }
            return size.ScaleProportional (max);
        }
    }
}
