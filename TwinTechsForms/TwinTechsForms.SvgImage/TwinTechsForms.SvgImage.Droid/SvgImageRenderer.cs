using System;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NGraphics;
using TwinTechs;
using TwinTechs.Droid;

using Size = NGraphics.Size;

[assembly: ExportRenderer (typeof(SvgImage), typeof(SvgImageRenderer))]
namespace TwinTechs.Droid
{
	[Preserve (AllMembers = true)]
	public class SvgImageRenderer : ViewRenderer<SvgImage, ImageView>
	{
		public new static void Init ()
		{
			var temp = DateTime.Now;
		}

		public SvgImageRenderer ()
		{
			// Offer to do our own drawing so Android will actually call `Draw`.
			SetWillNotDraw (willNotDraw: false);
		}

		private SvgImage _formsControl {
			get {
				return Element as SvgImage;
			}
		}

		// Don't need to deal with screen scaling on Android.
		const double ScreenScale = 1.0;

		public override void Draw (Android.Graphics.Canvas canvas)
		{
			base.Draw (canvas);

			if (_formsControl != null) {
				var outputSize = new Size (canvas.Width, canvas.Height);
				var finalCanvas = _formsControl.RenderSvgToCanvas (outputSize, ScreenScale, CreatePlatformImageCanvas);
				var image = (BitmapImage)finalCanvas.GetImage ();

				Control.SetImageBitmap (image.Bitmap);
			}
		}

		protected override void OnElementChanged (ElementChangedEventArgs<SvgImage> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				(e.OldElement as SvgImage).OnInvalidate -= HandleInvalidate;
			}

			if (e.NewElement != null) {
				(e.NewElement as SvgImage).OnInvalidate += HandleInvalidate;
			}

			Invalidate ();

			if (_formsControl != null) {
				Device.BeginInvokeOnMainThread (() => {
					var imageView = new ImageView (Context);

					imageView.SetScaleType (ImageView.ScaleType.FitXy);

					// TODO: ?Reuse existing Control instead?
					SetNativeControl (imageView);
					Invalidate ();
				});
			}
		}

		public override SizeRequest GetDesiredSize (int widthConstraint, int heightConstraint)
		{
			return new SizeRequest (new Xamarin.Forms.Size (_formsControl.WidthRequest, _formsControl.WidthRequest));
		}

		static Func<Size, double, IImageCanvas> CreatePlatformImageCanvas = (size, scale) => new AndroidPlatform ().CreateImageCanvas (size, scale);

		/// <summary>
		/// Handles view invalidate.
		/// </summary>
		void HandleInvalidate (object sender, System.EventArgs args)
		{
			Invalidate ();
		}

		/// <summary>
		/// http://stackoverflow.com/questions/24465513/how-to-get-detect-screen-size-in-xamarin-forms
		/// </summary>
		/// <param name="pixel"></param>
		/// <returns></returns>
		private int PixelToDP (int pixel)
		{
			var scale = Resources.DisplayMetrics.Density;
			return (int)((pixel * scale) + 0.5f);
		}
	}
}