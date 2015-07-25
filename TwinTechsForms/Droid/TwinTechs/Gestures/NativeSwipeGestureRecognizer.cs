using System;
using Android.Views;

namespace TwinTechs.Gestures
{
	public class NativeSwipeGestureRecognizer :  BaseNativeGestureRecognizer
	{
		//TODO introuduce minimum distance

		public NativeSwipeGestureRecognizer ()
		{
		}

		#region implemented abstract members of BaseNativeGestureRecognizer

		protected override bool IsMotionEventCancelled {
			get {
				return Recognizer.CancelsTouchesInView && (State == GestureRecognizerState.Began || State == GestureRecognizerState.Recognized);
			}
		}

		protected override bool ProcessMotionEvent (MotionEvent e)
		{
			//NOT IMPLEMENTED YET
			return false;
		}

		#endregion

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
//			Console.WriteLine ("did fling! vx {0} vy {1}", velocityX, velocityY);
			var direction = GetSwipeDirection (velocityX, velocityY);
			var expectedDirection = (Recognizer as SwipeGestureRecognizer).Direction;
			if (direction == expectedDirection) {
				State = GestureRecognizerState.Recognized;
				SendGestureEvent ();
			} else {
				State = GestureRecognizerState.Failed;
				Console.WriteLine ("failed gesture was expecting {0} got {1}", expectedDirection, direction);
			}
			return false;
		}

		SwipeGestureRecognizerDirection GetSwipeDirection (float velocityX, float velocityY)
		{
			var isHorizontalSwipe = Math.Abs (velocityX) > Math.Abs (velocityY);
			if (isHorizontalSwipe) {
				return velocityX > 0 ? SwipeGestureRecognizerDirection.Right : SwipeGestureRecognizerDirection.Left;
			} else {
				return velocityY > 0 ? SwipeGestureRecognizerDirection.Down : SwipeGestureRecognizerDirection.Up;
				
			}
		}
	}
}

