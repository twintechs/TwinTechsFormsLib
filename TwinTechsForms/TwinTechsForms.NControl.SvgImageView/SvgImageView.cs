using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using NGraphics;
using Point = NGraphics.Point;
using Size = NGraphics.Size;
using System.IO;

namespace TwinTechsForms.NControl
{
	public class SvgImageView : Image
	{
		public event EventHandler OnInvalidate;

		/// <summary>
		/// The path to the svg file
		/// </summary>
		public static readonly BindableProperty SvgPathProperty =
			BindableProperty.Create ("SvgPath", typeof(string), typeof(SvgImageView), default(string));

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
			BindableProperty.Create ("SvgAssembly", typeof(Assembly), typeof(SvgImageView), default(Assembly));

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
			BindableProperty.Create (nameof (SvgStretchableInsets), typeof(ResizableSvgInsets), typeof(SvgImageView), default(ResizableSvgInsets));

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

		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName == SvgImageView.SvgPathProperty.PropertyName
			             || propertyName == SvgImageView.SvgAssemblyProperty.PropertyName) {
				// Changed SVG resource or assembly for SVG resource; load new one.
				LoadSvgFromResource ();
				Invalidate ();
			} else if (propertyName == SvgImageView.SvgStretchableInsetsProperty.PropertyName) {
				Invalidate ();
			}
		}

		Graphic LoadedGraphic { get; set; }

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
						sliceInsets.ScaleSection (outputSize, section));
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
