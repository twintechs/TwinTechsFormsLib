using System;
using Android.Views;

namespace TwinTechs.Gestures
{
	public class NativeSwipeGestureRecognizer: BaseNativeGestureRecognizer
	{
		public NativeSwipeGestureRecognizer ()
		{
		}

		#region gesture stuff

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			Console.WriteLine ("OnFling");
			OnGesture ();
			return true;
		}

		#endregion

		protected override GestureDetector CreateGestureDetector ()
		{
			throw new NotImplementedException ();
		}
	}
}

