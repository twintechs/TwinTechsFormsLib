using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;

namespace TwinTechs.Gestures
{
	public class NativeTapGestureRecognizer : BaseNativeGestureRecognizer,GestureDetector.IOnGestureListener
	{
		public NativeTapGestureRecognizer ()
		{
		}

		#region gesture stuff

		System.Timers.Timer _multiTapTimer;

		int _currentTapCount;

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
			NumberOfTouches = e.PointerCount;
			if (NumberOfTouches < (this.Recognizer as TapGestureRecognizer).NumberOfTouchesRequired) {
				return false;
			}
			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
			_currentTapCount++;
			Console.WriteLine ("Tapped current tap count " + _currentTapCount);

			var requiredTaps = (this.Recognizer as TapGestureRecognizer).NumberOfTapsRequired;
			if (requiredTaps == 1) {
				State = GestureRecognizerState.Recognized;
				OnGesture ();
			} else {
				
				if (_currentTapCount == requiredTaps) {
					Console.WriteLine ("did multi tap, required " + requiredTaps);
					NumberOfTouches = 1;
					State = GestureRecognizerState.Recognized;
					OnGesture ();
					_currentTapCount = 0;
				} else {
					ResetMultiTapTimer (true);
					Console.WriteLine ("incomplete multi tap, " + _currentTapCount + "/" + requiredTaps);
				}
			}
			
			return false;
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
				_multiTapTimer.Stop ();
			}
			if (isActive) {
				State = GestureRecognizerState.Possible;
				_multiTapTimer = new System.Timers.Timer ();
				_multiTapTimer.Interval = 300;
				_multiTapTimer.Elapsed += _multiTapTimer_Elapsed;
				_multiTapTimer.Start ();
			} else {
				_currentTapCount = 0;
				State = GestureRecognizerState.Failed;
			}
			
		}
	}
}

