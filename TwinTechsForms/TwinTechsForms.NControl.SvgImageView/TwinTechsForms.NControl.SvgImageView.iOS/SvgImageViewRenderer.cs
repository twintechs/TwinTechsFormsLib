using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NGraphics;
using TwinTechsForms.NControl;
using TwinTechsForms.NControl.iOS;
using Size = NGraphics.Size;

[assembly: ExportRenderer(typeof(SvgImageView), typeof(SvgImageViewRenderer))]
namespace TwinTechsForms.NControl.iOS {
    [Preserve(AllMembers = true)]
    public class SvgImageViewRenderer : ImageRenderer {
        /// <summary>
        ///   Used for registration with dependency service
        /// </summary>
        public new static void Init() {
            var temp = DateTime.Now;
        }

        SvgImageView FormsControl {
            get {
                return Element as SvgImageView;
            }
        }

        static Func<Size, double, IImageCanvas> CreatePlatformImageCanvas = (size, scale) => new ApplePlatform().CreateImageCanvas(size, scale);

        public override void Draw(CGRect rect) {
            base.Draw(rect);

            if (FormsControl != null) {
                using (CGContext context = UIGraphics.GetCurrentContext()) {
                    context.SetAllowsAntialiasing(true);
                    context.SetShouldAntialias(true);
                    context.SetShouldSmoothFonts(true);

                    var finalCanvas = FormsControl.RenderSvgToCanvas(new Size(rect.Width, rect.Height), UIScreen.MainScreen.Scale, CreatePlatformImageCanvas);
                    var image = finalCanvas.GetImage();
                    var uiImage = image.GetUIImage();
                    Control.Image = uiImage;
                }
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

            SetNeedsDisplay();
        }

        /// <summary>
        /// Handles view invalidate.
        /// </summary>
        void HandleInvalidate(object sender, EventArgs args) {
            SetNeedsDisplay();
        }
    }
}
