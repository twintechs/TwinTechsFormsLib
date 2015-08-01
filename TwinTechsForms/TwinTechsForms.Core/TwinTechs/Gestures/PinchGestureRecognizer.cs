using System;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{

	public interface INativePinchGestureRecognizer : INativeGestureRecognizer
	{
		float Scale ();

		float Velocity ();
	}


	public class PinchGestureRecognizer : BaseGestureRecognizer
	{
		public float Scale ()
		{
			return (NativeGestureRecognizer as INativePinchGestureRecognizer).Scale ();
		}

		public float Velocity ()
		{
			return (NativeGestureRecognizer as INativePinchGestureRecognizer).Velocity ();
		}

		public PinchGestureRecognizer ()
		{
		}


	}
}

