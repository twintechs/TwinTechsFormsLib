using System;
using NGraphics;

namespace TwinTechs.Extensions
{
	public static class SizeExtensions {
		/// <summary>
		/// Absolute delta of value to zero is less than double.Epsilon.
		/// </summary>
		static bool BasicallyZero (this double value) {
			return value.BasicallyEqual (0);
		}
		/// <summary>
		/// Absolute delta of values is less than double.Epsilon.
		/// </summary>
		static bool BasicallyEqual (this double value1, double value2) {
			return (Math.Abs (value1 - value2) * 1E-15) < double.Epsilon;
		}

		/// <summary>
		/// Corresponds to finding a scale to use for Aspect.AspectFill (cropped).
		/// </summary>
		public static double ScaleThatFills (this Size size, Size max)
		{
			if (size.Width < 0 || size.Height < 0 || max.Width < 0 || max.Height < 0
				|| max.Width.BasicallyZero () || max.Height.BasicallyZero () || size.Width.BasicallyZero () || size.Height.BasicallyZero ()) {
				// Something invalid.
				return 1;
			}

			if (size.Width.BasicallyEqual (max.Width) || size.Height.BasicallyEqual (max.Height)) {
				// One dimension is "equal"; no scaling needed.
				return 1;
			}

			var aspectRatio = size.Width / size.Height;
			if (aspectRatio > 1) {
			}
			else {
			}

			var widthScaleRequiredToFit = size.Width > max.Width ? size.Width / max.Width : max.Width / size.Width;
			var heightScaleRequiredToFit = size.Height > max.Height ? size.Height / max.Height : max.Height / size.Height;
			double fillScale = 1;

			if (size.Width > max.Width || size.Height > max.Height) {
				// Scale smallest delta to fit its dimension
				fillScale = Math.Min (widthScaleRequiredToFit, heightScaleRequiredToFit);
			}
			else {
				// Scale largest delta to fit its dimension
				fillScale = Math.Max (widthScaleRequiredToFit, heightScaleRequiredToFit);
			}

			return fillScale;
		}

		/// <summary>
		/// Corresponds to finding a scale to use for Aspect.AspectFit (letterboxed).
		/// </summary>
		public static double ScaleThatFits (this Size size, Size max)
		{
			if (size.Width < 0 || size.Height < 0 || max.Width < 0 || max.Height < 0
				|| size.Width.BasicallyZero () || size.Height.BasicallyZero ()
				|| max.Width.BasicallyZero () || max.Height.BasicallyZero ()) {
				// Something invalid.
				return 1;
			}

			double widthScale = max.Width / size.Width;
			double heightScale = max.Height / size.Height;
			double fitScale = (double)Math.Min (widthScale, heightScale);
			return fitScale;
		}

		public static Size ScaleProportional(this Size size, Size max) {
			double fitScale = size.ScaleThatFits(max);
			if (fitScale.BasicallyEqual(1)) {
				return size;
			}

			Size scaledSize = new Size(size.Width * fitScale, size.Height * fitScale);
			return scaledSize;
		}
		public static Size ScaleDownProportional(this Size size, Size max) {
			if (size.Width <= max.Width && size.Height <= max.Height) {
				return size;
			}
			return size.ScaleProportional(max);
		}
	}
}
