using System;
using UIKit;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TwinTechs.Ios.Extensions;
using CoreGraphics;

namespace TwinTechs.Gestures
{
	public class NativePanGestureRecognizer : BaseNativeGestureRecognizer<UIPanGestureRecognizer,PanGestureRecognizer>
	, INativePanGestureRecognizer
	{
		public NativePanGestureRecognizer ()
		{
		}

		#region overridden

		protected override void ConfigureNativeGestureRecognizer ()
		{
			base.ConfigureNativeGestureRecognizer ();
			NativeRecognizer.MinimumNumberOfTouches = (nuint)Recognizer.MinimumNumberOfTouches;
			NativeRecognizer.MaximumNumberOfTouches = (nuint)Recognizer.MaximumNumberOfTouches;
		}

		#endregion

		#region INativePanGestureRecognizer impl

		public Xamarin.Forms.Point GetVelocityInView (VisualElement view)
		{
			var renderer = view.GetRenderer ();
			return NativeRecognizer.VelocityInView (renderer.NativeView).ToPoint ();
		}

		public Xamarin.Forms.Point GetTranslationInView (VisualElement view)
		{
			var renderer = view.GetRenderer ();
			return NativeRecognizer.TranslationInView (renderer.NativeView).ToPoint ();
		}

		public void SetTranslationInView (Xamarin.Forms.Point translation, VisualElement view)
		{
			var renderer = view.GetRenderer ();
			NativeRecognizer.SetTranslation (new CGPoint (translation.X, translation.Y), renderer.NativeView);
		}

		#endregion


	}
}

