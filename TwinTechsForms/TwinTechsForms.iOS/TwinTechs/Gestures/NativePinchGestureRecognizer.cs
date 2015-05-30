using System;
using UIKit;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public class NativePinchGestureRecognizer : BaseNativeGestureRecognizer<UIPinchGestureRecognizer,PinchGestureRecognizer>
	, INativePinchGestureRecognizer
	{
		public Xamarin.Forms.Point GetVelocityInView (Xamarin.Forms.VisualElement view)
		{
			throw new NotImplementedException ();
		}

		public Xamarin.Forms.Point GetTranslationInView (Xamarin.Forms.VisualElement view)
		{
			throw new NotImplementedException ();
		}




		public NativePinchGestureRecognizer ()
		{
		}

		public float Velocity ()
		{
			return (float)NativeRecognizer.Velocity;
		}


		public float Scale ()
		{
			return (float)NativeRecognizer.Scale;
		}
	}
}

