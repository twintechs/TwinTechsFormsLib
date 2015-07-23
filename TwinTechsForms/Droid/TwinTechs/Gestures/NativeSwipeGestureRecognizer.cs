using System;
using Android.Views;

namespace TwinTechs.Gestures
{
	public class NativeSwipeGestureRecognizer :  BaseNativeGestureRecognizer ,GestureDetector.IOnGestureListener
	{
		//TODO introuduce minimum distance

		public NativeSwipeGestureRecognizer ()
		{
		}

		#region gesture stuff

		public void OnLongPress (MotionEvent e)
		{
		}

		protected override GestureDetector CreateGestureDetector ()
		{
			return new GestureDetector (this);
		}

		#endregion

		public bool OnDown (MotionEvent e)
		{
			return false;
		}

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
//			Console.WriteLine ("did fling! vx {0} vy {1}", velocityX, velocityY);
			var direction = GetSwipeDirection (velocityX, velocityY);
			var expectedDirection = (Recognizer as SwipeGestureRecognizer).Direction;
			if (direction == expectedDirection) {
				State = GestureRecognizerState.Recognized;
				OnGesture ();
			} else {
				State = GestureRecognizerState.Failed;
				Console.WriteLine ("failed gesture was expecting {0} got {1}", expectedDirection, direction);
			}
			return false;
		}

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			return false;
		}

		public void OnShowPress (MotionEvent e)
		{

		}

		public bool OnSingleTapUp (MotionEvent e)
		{
			
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

