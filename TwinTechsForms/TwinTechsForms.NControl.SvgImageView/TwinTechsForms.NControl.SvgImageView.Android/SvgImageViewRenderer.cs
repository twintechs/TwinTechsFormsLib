using System;
using Android.Graphics;
using Android.Runtime;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NGraphics;
using TwinTechsForms.NControl;
using TwinTechsForms.NControl.Android;
using Size = NGraphics.Size;

[assembly: ExportRenderer(typeof(SvgImageView), typeof(SvgImageViewRenderer))]
namespace TwinTechsForms.NControl.Android {
    [Preserve(AllMembers = true)]
    public class SvgImageViewRenderer : ImageRenderer {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init() {
            var temp = DateTime.Now;
        }

        public SvgImageViewRenderer() {
            // Offer to do our own drawing so Android will actually call `Draw`.
            SetWillNotDraw(willNotDraw: false);
        }

        SvgImageView FormsControl {
            get {
                return Element as SvgImageView;
            }
        }

        static Func<Size, double, IImageCanvas> CreatePlatformImageCanvas = (size, scale) => new AndroidPlatform().CreateImageCanvas(size, scale);

        public override void Draw(Canvas canvas) {
            base.Draw(canvas);

            if (FormsControl != null) {
                const double screenScale = 1.0; // Don't need to deal with screen scaling on Android.

                var finalCanvas = FormsControl.RenderSvgToCanvas(new Size(canvas.Width, canvas.Height), screenScale, CreatePlatformImageCanvas);
                var image = (BitmapImage)finalCanvas.GetImage();

                Control.SetImageBitmap(image.Bitmap);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e) {
            base.OnElementChanged(e);

            if (e.OldElement != null) {
                (e.OldElement as SvgImageView).OnInvalidate -= HandleInvalidate;
            }

            if (e.NewElement != null) {
                (e.NewElement as SvgImageView).OnInvalidate += HandleInvalidate;
            }
            
            Invalidate();
        }

        /// <summary>
        /// Handles view invalidate.
        /// </summary>
        void HandleInvalidate(object sender, System.EventArgs args) {
            Invalidate();
        }
    }
}
