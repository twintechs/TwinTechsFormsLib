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


		#region gesture stuff

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			Console.WriteLine ("Pinch");
			OnGesture ();
			//TODO set the scale
			return true;
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

		protected override GestureDetector CreateGestureDetector ()
		{
			throw new NotImplementedException ();
		}
	}
}

