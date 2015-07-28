using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public class NativeLongPressGestureRecognizer : BaseNativeGestureRecognizer
	{
		public NativeLongPressGestureRecognizer ()
		{
		}

		LongPressGestureRecognizer LongPressGestureRecognizer { get { return Recognizer as LongPressGestureRecognizer; } }

		System.Timers.Timer _longPressTimer;

		#region implemented abstract members of BaseNativeGestureRecognizer

		internal override void ProcessMotionEvent (GestureMotionEvent e)
		{
			if (e.ActionMasked == MotionEventActions.Down && PointerId == -1) {
				OnDown (e);
				e.IsConsumed = true;
				e.IsCancelled = Recognizer.CancelsTouchesInView;
			} else if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed) {
				return;
			} else {
				
				var xMovement = Math.Abs (e.GetX (0) - FirstTouchPoint.X);
				var yMovement = Math.Abs (e.GetY (0) - FirstTouchPoint.Y);
				var isMovedBeyondMaxDistance = xMovement > LongPressGestureRecognizer.MaxDistanceTolerance || yMovement > LongPressGestureRecognizer.MaxDistanceTolerance;
				Console.WriteLine ("isMovedBeyondMaxDistance {0} xd {1} yd{2}", isMovedBeyondMaxDistance, xMovement, yMovement);
				if (e.ActionMasked == MotionEventActions.Cancel || isMovedBeyondMaxDistance) {
					State = GestureRecognizerState.Cancelled;
					Console.WriteLine ("LONG PRESS CANCELLED");
					PointerId = -1;
					SendGestureUpdate ();
				} else if (e.ActionMasked == MotionEventActions.Up) {
					OnUp (e);
					e.IsConsumed = true;
				}
			}
		}

		#endregion


		void OnDown (GestureMotionEvent e)
		{
			//TODO - should really be possible until all taps/fingers are satisfied.
			if (e.PointerCount == LongPressGestureRecognizer.NumberOfTouchesRequired) {
				State = GestureRecognizerState.Began;
				PointerId = e.GetPointerId (0);
				FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
				ResetLongPressTimer (true);
			} else {
				State = GestureRecognizerState.Failed;
				PointerId = -1;
				SendGestureUpdate ();
				
			}
		}

		void OnUp (GestureMotionEvent e)
		{
			ResetLongPressTimer (false);
			//TODO track the correct fingers
			if (State == GestureRecognizerState.Began) {
				PointerId = -1;
				State = GestureRecognizerState.Failed;
				SendGestureUpdate ();
				Console.WriteLine ("LONG PRESS CANCELLED");
			}
		}

		void _longPressTapTimer_Elapsed (object sender, System.Timers.ElapsedEventArgs e)
		{
			Console.WriteLine ("LONG PRESS RECOGNIZED");
			ResetLongPressTimer (false);
			State = GestureRecognizerState.Recognized;
			Device.BeginInvokeOnMainThread (() => SendGestureEvent ());

			PointerId = -1;
			//TODO better improve the trackign of touches
		}

		void ResetLongPressTimer (bool isActive)
		{
			if (_longPressTimer != null) {
				_longPressTimer.Elapsed -= _longPressTapTimer_Elapsed;
				_longPressTimer.Stop ();
			}
			if (isActive) {
				State = GestureRecognizerState.Possible;
				_longPressTimer = new System.Timers.Timer ();
				_longPressTimer.Interval = LongPressGestureRecognizer.MinimumPressDuration * 1000;
				_longPressTimer.Elapsed += _longPressTapTimer_Elapsed;
				_longPressTimer.Start ();
			}

		}
	}
}

