using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreImage;
using Foundation;

namespace TwinTechs.Gestures
{
	public class NativeSwipeGestureRecognizer : BaseNativeGestureRecognizer<UISwipeGestureRecognizer,SwipeGestureRecognizer>
	{
		public NativeSwipeGestureRecognizer ()
		{
		}

		#region abstract impl

		protected override void ConfigureNativeGestureRecognizer ()
		{
			base.ConfigureNativeGestureRecognizer ();
			NativeRecognizer.Direction = (UISwipeGestureRecognizerDirection)Recognizer.Direction;
			NativeRecognizer.NumberOfTouchesRequired = (nuint)Recognizer.NumberOfTouchesRequired;
		}

		#endregion
	}
}

