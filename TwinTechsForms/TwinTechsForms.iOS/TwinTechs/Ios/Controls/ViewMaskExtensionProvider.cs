using System;
using TwinTechs.Controls;
using System.Drawing;
using CoreAnimation;
using System.Runtime.InteropServices;
using CoreGraphics;
using UIKit;
using TwinTechs.Ios.Extensions;
using Xamarin.Forms;

namespace TwinTechs.Ios.Controls
{
	public class ViewMaskExtensionProvider : IViewEffectExtensionProvider
	{
		public ViewMaskExtensionProvider ()
		{
		}

		#region IViewMaskExtensionProvider implementation

		public void ApplyMaskToView (Xamarin.Forms.View view, ViewMaskerType maskType)
		{
			var renderer = view.GetRenderer ();
			var nativeView = renderer.NativeView;
			if (maskType == ViewMaskerType.None) {
				view.SizeChanged -= OnSizeChanged;
				nativeView.Layer.Mask = null;
			} else {
				var maskLayer = GetMaskShape (maskType, new Xamarin.Forms.Size (view.Width, view.Height));
				maskLayer.Frame = nativeView.Bounds;
				nativeView.Layer.Mask = maskLayer;
				view.SizeChanged += OnSizeChanged;
			}
			nativeView.Tag = (int)maskType;
			
		}


		public void ToggleViewShadow (Xamarin.Forms.View view, bool isOn)
		{
			var renderer = view.GetRenderer ();
			var nativeView = renderer.NativeView;
			nativeView.Layer.ShadowColor = UIColor.Black.CGColor;
			nativeView.Layer.ShadowOpacity = isOn ? 1 : 0;
			nativeView.Layer.ShadowOffset = isOn ? new CGSize (10, 10) : CGSize.Empty;
		}

		#endregion

		CAShapeLayer GetMaskShape (ViewMaskerType maskType, Xamarin.Forms.Size size)
		{
			var layer = new CAShapeLayer ();
			layer.FillColor = UIColor.White.CGColor;
			layer.StrokeColor = UIColor.White.CGColor;
			layer.LineWidth = 0;
			UIBezierPath path = null;
			var bounds = new CGRect (0, 0, size.Width, size.Height);

			switch (maskType) {
			case ViewMaskerType.Circle:
				path = UIBezierPath.FromRoundedRect (bounds, (nfloat)Math.Max (size.Width, size.Height));
				break;
			case ViewMaskerType.Triangle:
				var point1 = new CGPoint (0, size.Height);
				var point2 = new CGPoint (size.Width, size.Height);
				var point3 = new CGPoint (size.Width / 2, 0);
				path = new UIBezierPath ();
				path.MoveTo (point1);
				path.AddLineTo (point2);
				path.AddLineTo (point3);
				path.AddLineTo (point1);
				path.ClosePath ();
				path.Fill ();
				break;
			case ViewMaskerType.Square:
				var smallRectangle = UIBezierPath.FromRect (bounds.Inset (50, 50));
				path = UIBezierPath.FromRoundedRect (bounds, 20);
				break;
			default:
				throw new ArgumentOutOfRangeException ();
			}
			layer.Path = path.CGPath;
			return layer;
		}

		void OnSizeChanged (object sender, EventArgs e)
		{
			var view = sender as Xamarin.Forms.View;
			var renderer = view.GetRenderer ();
			var nativeView = renderer.NativeView;
			var maskLayer = nativeView.Layer.Mask;
			if (maskLayer != null) {
				var currentType = (ViewMaskerType)((int)nativeView.Tag);
				Device.BeginInvokeOnMainThread (() => ApplyMaskToView (view, currentType));
			}
		}
	}
}

