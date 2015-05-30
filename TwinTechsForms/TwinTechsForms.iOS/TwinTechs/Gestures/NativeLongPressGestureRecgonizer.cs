using System;
using UIKit;

namespace TwinTechs.Gestures
{
	public class NativeLongPressGestureRecgonizer : BaseNativeGestureRecognizer<UILongPressGestureRecognizer,LongPressGestureRecognizer>
	{
		public NativeLongPressGestureRecgonizer ()
		{
		}

		protected override void ConfigureNativeGestureRecognizer ()
		{
			base.ConfigureNativeGestureRecognizer ();
			NativeRecognizer.MinimumPressDuration = Recognizer.MinimumPressDuration;
		}
	}
}

