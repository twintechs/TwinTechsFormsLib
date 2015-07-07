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
		Xamarin.Forms.Point _translation;
		Xamarin.Forms.Point _previousPoint;

		Xamarin.Forms.Point _currentPoint;
		Xamarin.Forms.Point _currentTranslation;

		public Xamarin.Forms.Point GetVelocityInView (Xamarin.Forms.VisualElement view)
		{
			return _velocity;
//			throw new NotImplementedException ("need to write velocity");
		}

		public Xamarin.Forms.Point GetTranslationInView (Xamarin.Forms.VisualElement view)
		{
			//not supported like in iOS right now.
			return new Xamarin.Forms.Point (-_currentTranslation.X, -_currentTranslation.Y);
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
			_currentTranslation = translation;
			_currentPoint = translation;
			_previousPoint = translation;
		}

		#region IOnGestureListener impl

		public bool OnDown (MotionEvent e)
		{
			State = GestureRecognizerState.Began;
			_currentTranslation = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			_currentPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			_previousPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			OnGesture ();
			return true;
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
			if (e2.ActionMasked == MotionEventActions.Move || e2.ActionMasked == MotionEventActions.Scroll) {
				_previousPoint = _currentPoint;
				_currentPoint = new Xamarin.Forms.Point (e2.GetX (), e2.GetY ());
				_currentTranslation = new Xamarin.Forms.Point (distanceX, distanceY);
				State = GestureRecognizerState.Changed;
			}
//			Console.WriteLine ("_currentTranslation " + _currentTranslation);
//			Console.WriteLine ("State " + State + "_previousPoint " + _previousPoint + " _currentPoint" + _currentPoint);
			OnGesture ();
			return true;
		}

		#endregion
	}
}

