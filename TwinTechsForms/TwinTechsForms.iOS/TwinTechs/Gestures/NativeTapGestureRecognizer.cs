using System;
using UIKit;

namespace TwinTechs.Gestures
{
	public class NativeTapPressGestureRecgonizer : BaseNativeGestureRecognizer<UITapGestureRecognizer,TapGestureRecognizer>, ITapGestureRecognizer
	{
		public NativeTapPressGestureRecgonizer ()
		{
		}

		protected override void ConfigureNativeGestureRecognizer ()
		{
			base.ConfigureNativeGestureRecognizer ();
			NativeRecognizer.NumberOfTouchesRequired = (nuint)Recognizer.NumberOfTouchesRequired;
			NativeRecognizer.NumberOfTapsRequired = (nuint)Recognizer.NumberOfTapsRequired;
		}
	}
}

