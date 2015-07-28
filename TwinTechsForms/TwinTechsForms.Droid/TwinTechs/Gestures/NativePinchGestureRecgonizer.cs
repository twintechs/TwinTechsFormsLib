using System;
using Android.Views;

namespace TwinTechs.Gestures
{
	public class NativePinchGestureRecgonizer: BaseNativeGestureRecognizer, INativePinchGestureRecognizer
	{
		public NativePinchGestureRecgonizer ()
		{
		}

		float _scale = 1.0f;
		float _velocity = 0.0f;

		#region implemented abstract members of BaseNativeGestureRecognizer

		internal override void ProcessMotionEvent (GestureMotionEvent e)
		{
		}

		#endregion

		public float Scale ()
		{
			return _scale;
		}

		public float Velocity ()
		{
			return _velocity;
		}
	}
}

