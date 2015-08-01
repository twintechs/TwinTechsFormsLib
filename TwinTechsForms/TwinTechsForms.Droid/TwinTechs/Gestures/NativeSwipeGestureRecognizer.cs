using System;
using Android.Views;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public class NativeSwipeGestureRecognizer :  BaseNativeGestureRecognizer
	{
		const int MinimumSwipeDistance = 5;
		const int MaxSwipeDuration = 1000;
		DateTime _startTime;


		public NativeSwipeGestureRecognizer ()
		{
		}

		SwipeGestureRecognizer SwipeGestureRecognizer { get { return Recognizer as SwipeGestureRecognizer; } }

		#region implemented abstract members of BaseNativeGestureRecognizer

		internal override void ProcessMotionEvent (GestureMotionEvent e)
		{
			if (e.Action == MotionEventActions.Down && PointerId == -1) {
				OnDown (e);

				//TODO - this should probably be possible at this point?
				if (State == GestureRecognizerState.Began) {
					//TODO track all pointers that are down.
					PointerId = e.GetPointerId (0);
//					e.IsConsumed = true;
					e.IsCancelled = Recognizer.CancelsTouchesInView;
				}
			} else if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed) {
				return;
			} else if (e.ActionMasked == MotionEventActions.Cancel) {
				State = GestureRecognizerState.Cancelled;
				Console.WriteLine ("GESTURE CANCELLED");
			} else if (e.ActionMasked == MotionEventActions.Up) {
				OnUp (e);
//				e.IsConsumed = State != GestureRecognizerState.Failed;
			}
		}

		#endregion

		void OnDown (GestureMotionEvent e)
		{
			//TODO - should really be possible until all taps/fingers are satisfied.
			State = GestureRecognizerState.Began;
//			State = (e.PointerCount == SwipeGestureRecognizer.NumberOfTouchesRequired) ? GestureRecognizerState.Began : GestureRecognizerState.Failed;

			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
			_startTime = DateTime.Now;
		}


		void OnUp (GestureMotionEvent e)
		{
			NumberOfTouches = e.PointerCount;
			var tookTooLong = (DateTime.Now - _startTime).Milliseconds > MaxSwipeDuration;
			var wrongAmountOfTouches = NumberOfTouches < SwipeGestureRecognizer.NumberOfTouchesRequired;
			if (tookTooLong || wrongAmountOfTouches) {
				State = GestureRecognizerState.Failed;
				return;
			}
			var endTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
			double velocityX = endTouchPoint.X - FirstTouchPoint.X;
			double velocityY = endTouchPoint.Y - FirstTouchPoint.Y;
			var direction = GetSwipeDirection (velocityX, velocityY);
			var expectedDirection = (Recognizer as SwipeGestureRecognizer).Direction;
			if (direction == expectedDirection) {
				State = GestureRecognizerState.Recognized;
			} else {
				State = GestureRecognizerState.Failed;
				Console.WriteLine ("failed gesture was expecting {0} got {1}", expectedDirection, direction);
			}
		}

		SwipeGestureRecognizerDirection GetSwipeDirection (double velocityX, double velocityY)
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

