using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Xamarin.Forms;
using System.Runtime.InteropServices;

namespace TwinTechs.Gestures
{
	public class NativeTapGestureRecognizer : BaseNativeGestureRecognizer
	{
		public NativeTapGestureRecognizer ()
		{
		}

		System.Timers.Timer _multiTapTimer;

		int _currentTapCount;
		DateTime _startTime;

		TapGestureRecognizer TapGestureRecognizer { get { return Recognizer as TapGestureRecognizer; } }

		#region implemented abstract members of BaseNativeGestureRecognizer

		internal override void ProcessMotionEvent (GestureMotionEvent e)
		{
			_startTime = DateTime.Now;
			if (e.Action == MotionEventActions.Down && PointerId == -1) {
				OnDown (e);
				if (State == GestureRecognizerState.Began) {
					e.IsConsumed = true;
					e.IsCancelled = true;
				}
			} else if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed) {
				return;
			} else if (e.ActionMasked == MotionEventActions.Cancel) {
				State = GestureRecognizerState.Cancelled;
				Console.WriteLine ("GESTURE CANCELLED");
				PointerId = -1;
				SendGestureUpdate ();
			} else if (e.ActionMasked == MotionEventActions.Up) {
				OnUp (e);
				if (PointerId == e.GetPointerId (0)) {
					e.IsConsumed = State != GestureRecognizerState.Failed;
				}
			}
		}

		#endregion

		void OnDown (GestureMotionEvent e)
		{
			//TODO - should really be possible until all taps/fingers are satisfied.
			State = (e.PointerCount == TapGestureRecognizer.NumberOfTouchesRequired) ? GestureRecognizerState.Began : GestureRecognizerState.Failed;
			_currentTapCount = 0;
			//TODO track all pointers that are down.
			PointerId = e.GetPointerId (0);
			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
			if (TapGestureRecognizer.NumberOfTapsRequired > 1 && State == GestureRecognizerState.Began) {
				ResetMultiTapTimer (true);
			}
		}


		void OnUp (GestureMotionEvent e)
		{
			NumberOfTouches = e.PointerCount;
			var tooLongBetweenTouches = (DateTime.Now - _startTime).Milliseconds > 400;
			var wrongNumberOfTouches = NumberOfTouches < (this.Recognizer as TapGestureRecognizer).NumberOfTouchesRequired;
			if (tooLongBetweenTouches || wrongNumberOfTouches) {
				State = GestureRecognizerState.Failed;
				SendGestureEvent ();
				PointerId = -1;
				return;
			}
			_currentTapCount++;
			Console.WriteLine ("Tapped current tap count " + _currentTapCount);

			var requiredTaps = (this.Recognizer as TapGestureRecognizer).NumberOfTapsRequired;
			if (requiredTaps == 1) {
				State = GestureRecognizerState.Recognized;
				SendGestureEvent ();
				PointerId = -1;
			} else {
				
				if (_currentTapCount == requiredTaps) {
					Console.WriteLine ("did multi tap, required " + requiredTaps);
					NumberOfTouches = 1;
					State = GestureRecognizerState.Recognized;
					ResetMultiTapTimer (false);
					SendGestureEvent ();
					_currentTapCount = 0;
					PointerId = -1;
				} else {
					Console.WriteLine ("incomplete multi tap, " + _currentTapCount + "/" + requiredTaps);
				}
			}
			
			return;
		}

		void _multiTapTimer_Elapsed (object sender, System.Timers.ElapsedEventArgs e)
		{
			Console.WriteLine ("didn't finish multi tap gesture");
			_currentTapCount = 0;
			ResetMultiTapTimer (false);
		}

		void ResetMultiTapTimer (bool isActive)
		{
			if (_multiTapTimer != null) {
				_multiTapTimer.Elapsed -= _multiTapTimer_Elapsed;
				_multiTapTimer.AutoReset = true;
				_multiTapTimer.Stop ();
				_multiTapTimer.Close ();
			}
			if (isActive) {
				State = GestureRecognizerState.Possible;
				_multiTapTimer = new System.Timers.Timer ();
				_multiTapTimer.Interval = 300 * (TapGestureRecognizer.NumberOfTapsRequired - 1);
				_multiTapTimer.AutoReset = false;
				_multiTapTimer.Elapsed += _multiTapTimer_Elapsed;
				_multiTapTimer.Start ();
			} else {
				_currentTapCount = 0;
				if (State == GestureRecognizerState.Possible) {
					
					State = GestureRecognizerState.Failed;
					PointerId = -1;
					Device.BeginInvokeOnMainThread (() => {
						SendGestureUpdate ();
					});
				}
			}
			
		}
	}
}

