using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Dalvik.SystemInterop;
using Android.Support.V4.View;

namespace TwinTechs.Gestures
{
	
	
	public class NativePanGestureRecognizer : BaseNativeGestureRecognizer, INativePanGestureRecognizer, Android.Views.GestureDetector.IOnGestureListener
	{
		public NativePanGestureRecognizer ()
		{
			

		}

		Xamarin.Forms.Point _velocity;
		Xamarin.Forms.Point _previousPoint;

		Xamarin.Forms.Point _currentPoint;
		Xamarin.Forms.Point _rawTranslation;
		Xamarin.Forms.Point _startPoint;

		public Xamarin.Forms.Point GetVelocityInView (Xamarin.Forms.VisualElement view)
		{
			return _velocity;
//			throw new NotImplementedException ("need to write velocity");
		}

		public Xamarin.Forms.Point GetTranslationInView (Xamarin.Forms.VisualElement view)
		{
			//not supported like in iOS right now.
			return new Xamarin.Forms.Point (_rawTranslation.X, _rawTranslation.Y);
		}

		protected override GestureDetector CreateGestureDetector ()
		{
			var detector = new GestureDetector (this);
			return detector;
		}

		public void SetTranslationInView (Xamarin.Forms.Point translation, Xamarin.Forms.VisualElement view)
		{
			//NOT really supported in Android
			//TODO take account of view
			_rawTranslation = translation;
			_currentPoint = translation;
			_previousPoint = translation;
		}

		#region IOnGestureListener impl

		public bool OnDown (MotionEvent e)
		{
			State = GestureRecognizerState.Began;
			Console.WriteLine ("ON DOWN");
			_rawTranslation = new Xamarin.Forms.Point (0, 0);
			_currentPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			_previousPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			_startPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			OnGesture ();
//			return true;
			return false;
		}

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			return false;
		}

		public void OnLongPress (MotionEvent e)
		{
		}

		public void OnShowPress (MotionEvent e)
		{
		}

		public bool OnSingleTapUp (MotionEvent e)
		{
			return false;
		}


		protected override void PostProcessMotionEvent (MotionEvent e)
		{
			var notify = false;
			if (e.ActionMasked == MotionEventActions.Cancel) {
				State = GestureRecognizerState.Cancelled;
				notify = true;
			} else if (e.ActionMasked == MotionEventActions.Up) {
				State = GestureRecognizerState.Ended;
				notify = true;
			}
			if (notify) {
				OnGesture ();
			}
		}

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			if (State == GestureRecognizerState.Failed || State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended) {
				return false;
			} 
			if (e2.GetX () < 0 || e2.GetX () > NativeView.Width || e2.GetY () < 0 || e2.GetY () > NativeView.Height) {
				Console.WriteLine ("Gesture exited from view - it's over");
				State = GestureRecognizerState.Failed;
				OnGesture ();
				return false;
			}

			if (e2.ActionMasked == MotionEventActions.Move || e2.ActionMasked == MotionEventActions.Scroll) {
				_previousPoint = _currentPoint;
				_currentPoint = new Xamarin.Forms.Point (e2.GetX (), e2.GetY ());
//				_currentTranslation = new Xamarin.Forms.Point (distanceX, distanceY);
				_velocity = new Xamarin.Forms.Point (distanceX, distanceY);
				_rawTranslation.X -= distanceX;
				_rawTranslation.Y -= distanceY;
				State = GestureRecognizerState.Changed;
			}
			Console.WriteLine ("ONSCROLL _currentTranslation {0} dx {1},{2}", _rawTranslation, distanceX, distanceY);
//			Console.WriteLine ("State " + State + "_previousPoint " + _previousPoint + " _currentPoint" + _currentPoint);
			OnGesture ();
//			return true;
			return false;
		}

		#endregion
	}
}

