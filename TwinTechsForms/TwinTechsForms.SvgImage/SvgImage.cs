using System.Reflection;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using NGraphics;
using TwinTechs.Extensions;

using Point = NGraphics.Point;
using Size = NGraphics.Size;

namespace TwinTechs
{
	public enum ResizableSvgSection
	{
		TopLeft,
		TopCenter,
		TopRight,
		CenterLeft,
		CenterCenter,
		CenterRight,
		BottomLeft,
		BottomCenter,
		BottomRight,
	}

	public struct ResizableSvgInsets : IEquatable<ResizableSvgInsets>
	{
		public int Top { get; set; }

		public int Right { get; set; }

		public int Bottom { get; set; }

		public int Left { get; set; }

		public static ResizableSvgInsets Zero = new ResizableSvgInsets (0, 0, 0, 0);

		public ResizableSvgInsets (int top, int right, int bottom, int left)
		{
			Top = top;
			Right = right;
			Bottom = bottom;
			Left = left;
		}

		public ResizableSvgInsets (int vertical, int horizontal) : this (vertical, horizontal, vertical, horizontal)
		{
		}

		public ResizableSvgInsets (int allSides) : this (allSides, allSides, allSides, allSides)
		{
		}

		public Rect GetSection (Size parentSize, ResizableSvgSection section)
		{
			switch (section) {
			case ResizableSvgSection.TopLeft:
				return new Rect (Point.Zero, new Size (Left, Top));
			case ResizableSvgSection.TopCenter:
				return new Rect (new Point (Left, 0), new Size (parentSize.Width - Right - Left, Top));
			case ResizableSvgSection.TopRight:
				return new Rect (new Point (parentSize.Width - Right, 0), new Size (Right, Top));
			case ResizableSvgSection.CenterLeft:
				return new Rect (new Point (0, Top), new Size (Right, parentSize.Height - Bottom - Top));
			case ResizableSvgSection.CenterCenter:
				return new Rect (new Point (Left, Top), new Size (parentSize.Width - Right - Left, parentSize.Height - Bottom - Top));
			case ResizableSvgSection.CenterRight:
				return new Rect (new Point (parentSize.Width - Right, Top), new Size (Right, parentSize.Height - Bottom - Top));
			case ResizableSvgSection.BottomLeft:
				return new Rect (new Point (0, parentSize.Height - Bottom), new Size (Right, Bottom));
			case ResizableSvgSection.BottomCenter:
				return new Rect (new Point (Left, parentSize.Height - Bottom), new Size (parentSize.Width - Right - Left, Bottom));
			case ResizableSvgSection.BottomRight:
				return new Rect (new Point (parentSize.Width - Right, parentSize.Height - Bottom), new Size (Right, Bottom));
			default:
				throw new ArgumentOutOfRangeException ("section", "Invalid resizable SVG section");
			}
		}

		public override bool Equals (object obj)
		{
			if (obj.GetType () != typeof(ResizableSvgInsets)) {
				return false;
			}
			return Equals ((ResizableSvgInsets)obj);
		}

		public override int GetHashCode ()
		{
			return (Top + Right + Bottom + Left).GetHashCode ();
		}

		public static bool operator == (ResizableSvgInsets inset1, ResizableSvgInsets inset2)
		{
			return inset1.Equals (inset2);
		}

		public static bool operator != (ResizableSvgInsets inset1, ResizableSvgInsets inset2)
		{
			return !inset1.Equals (inset2);
		}

		#region IEquatable implementation

		public bool Equals (ResizableSvgInsets other)
		{
			return Top == other.Top
			&& Right == other.Right
			&& Bottom == other.Bottom
			&& Left == other.Left;
		}

		#endregion

		public override string ToString ()
		{
			return string.Format ("[{0}, {1}, {2}, {3}]", Top, Right, Bottom, Left);
		}
	}

	public class SvgImage : Image
	{
		public event EventHandler OnInvalidate;

		/// <summary>
		/// The path to the svg file
		/// </summary>
		public static readonly BindableProperty SvgPathProperty =
			BindableProperty.Create (nameof (SvgPath), typeof(string), typeof(SvgImage), default(string));

		/// <summary>
		/// The path to the svg file
		/// </summary>
		public string SvgPath {
			get { return (string)GetValue (SvgPathProperty); }
			set { SetValue (SvgPathProperty, value); }
		}

		/// <summary>
		/// The assembly containing the svg file
		/// </summary>
		public static readonly BindableProperty SvgAssemblyProperty =
			BindableProperty.Create (nameof (SvgAssembly), typeof(Assembly), typeof(SvgImage), default(Assembly));

		/// <summary>
		/// The assembly containing the svg file
		/// </summary>
		public Assembly SvgAssembly {
			get { return (Assembly)GetValue (SvgAssemblyProperty); }
			set { SetValue (SvgAssemblyProperty, value); }
		}

		/// <summary>
		/// Optional SVG 9-slice insets
		/// </summary>
		public static readonly BindableProperty SvgStretchableInsetsProperty =
			BindableProperty.Create (nameof (SvgStretchableInsets), typeof(ResizableSvgInsets), typeof(SvgImage), default(ResizableSvgInsets));

		/// <summary>
		/// Optional SVG 9-slice insets
		/// </summary>
		public ResizableSvgInsets SvgStretchableInsets {
			get { return (ResizableSvgInsets)GetValue (SvgStretchableInsetsProperty); }
			set {
				SetValue (SvgStretchableInsetsProperty, value);
			}
		}

		public void Invalidate ()
		{
			OnInvalidate?.Invoke (this, EventArgs.Empty);
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			LoadSvgFromResource ();
			Invalidate ();
		}

		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName == SvgImage.SvgPathProperty.PropertyName
			    || propertyName == SvgImage.SvgAssemblyProperty.PropertyName) {
				// Changed SVG resource or assembly for SVG resource; load new one.
				LoadSvgFromResource ();
				Invalidate ();
			} else if (propertyName == SvgImage.SvgStretchableInsetsProperty.PropertyName) {
				Invalidate ();
			}
		}

		public Graphic LoadedGraphic { get; set; }

		public void LoadSvgFromResource ()
		{
			if (SvgAssembly == null || SvgPath == null) {
				// Can be called by OnPropertyChanged as values are initially defined.
				// Don't load anything until we are sure we have enough to do so.
				return;
			}

			var svgStream = SvgAssembly.GetManifestResourceStream (SvgPath);

			if (svgStream == null) {
				throw new Exception (string.Format ("Error retrieving {0} make sure Build Action is Embedded Resource",
					SvgPath));
			}

			var r = new SvgReader (new StreamReader (svgStream));

			LoadedGraphic = r.Graphic;
		}

		public IImageCanvas RenderSvgToCanvas (Size outputSize, double finalScale, Func<Size, double, IImageCanvas> createPlatformImageCanvas)
		{
			var originalSvgSize = LoadedGraphic.Size;
			var finalCanvas = createPlatformImageCanvas (outputSize, finalScale);

			if (SvgStretchableInsets != ResizableSvgInsets.Zero) {
				// Doing a stretchy 9-slice manipulation on the original SVG.
				// Partition into 9 segments, based on _formsControl.Svg9SliceInsets, storing both original and scaled sizes.
				var sliceInsets = SvgStretchableInsets;
				var sliceFramePairs = new[] {
					ResizableSvgSection.TopLeft,
					ResizableSvgSection.TopCenter,
					ResizableSvgSection.TopRight,
					ResizableSvgSection.CenterLeft,
					ResizableSvgSection.CenterCenter,
					ResizableSvgSection.CenterRight,
					ResizableSvgSection.BottomLeft,
					ResizableSvgSection.BottomCenter,
					ResizableSvgSection.BottomRight,
				}.Select (section => {
					return Tuple.Create (
						sliceInsets.GetSection (originalSvgSize, section),
						sliceInsets.GetSection (outputSize, section));
				}).ToArray ();

				foreach (var sliceFramePair in sliceFramePairs) {
					var sliceImage = RenderSectionToImage (LoadedGraphic, sliceFramePair.Item1, sliceFramePair.Item2, finalScale, createPlatformImageCanvas);
					finalCanvas.DrawImage (sliceImage, sliceFramePair.Item2);
				}
			} else {
				// Typical approach to rendering an SVG; just draw it to the canvas.
				double proportionalOutputScale = originalSvgSize.ScaleThatFits (outputSize);
				// Make sure ViewBox is reset to a proportionally-scaled default in case it was previous set by slicing.
				LoadedGraphic.ViewBox = new Rect (Point.Zero, originalSvgSize / proportionalOutputScale);
				LoadedGraphic.Draw (finalCanvas);
			}
			return finalCanvas;
		}

		static IImage RenderSectionToImage (/*this*/ Graphic graphics, Rect sourceFrame, Rect outputFrame, double finalScale, Func<Size, double, IImageCanvas> createPlatformImageCanvas)
		{
			var originalSize = graphics.Size;
			var sectionCanvas = createPlatformImageCanvas (outputFrame.Size, finalScale);

			// Redraw into final version with any scaling between the original and the output slice.
			var sliceVerticalScale = outputFrame.Height / sourceFrame.Height;
			var sliceHorizontalScale = outputFrame.Width / sourceFrame.Width;
			// Potentially setting ViewBox size smaller to enlarge result.
			graphics.ViewBox = new Rect (sourceFrame.Position, new Size (originalSize.Width / sliceHorizontalScale, originalSize.Height / sliceVerticalScale));

			graphics.Draw (sectionCanvas);
			return sectionCanvas.GetImage ();
		}
	}
}