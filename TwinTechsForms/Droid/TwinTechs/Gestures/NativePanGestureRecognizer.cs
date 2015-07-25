using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Dalvik.SystemInterop;
using Android.Support.V4.View;

namespace TwinTechs.Gestures
{
	
	
	public class NativePanGestureRecognizer : BaseNativeGestureRecognizer, INativePanGestureRecognizer
	{
		public NativePanGestureRecognizer ()
		{
			

		}

		Xamarin.Forms.Point _velocity;
		Xamarin.Forms.Point _previousPoint;

		Xamarin.Forms.Point _currentPoint;
		Xamarin.Forms.Point _rawTranslation;
		Xamarin.Forms.Point _startPoint;

		#region implemented abstract members of BaseNativeGestureRecognizer

		protected override bool IsMotionEventCancelled {
			get {
				//State == GestureRecognizerState.Began || State == GestureRecognizerState.Recognized ||
				return Recognizer.CancelsTouchesInView && (State == GestureRecognizerState.Changed);
			}
		}

		public Xamarin.Forms.Point GetVelocityInView (Xamarin.Forms.VisualElement view)
		{
			return _velocity;
//			throw new NotImplementedException ("need to write velocity");
		}

		public Xamarin.Forms.Point GetTranslationInView (Xamarin.Forms.VisualElement view)
		{
			return new Xamarin.Forms.Point (_rawTranslation.X, _rawTranslation.Y);
		}


		public override Xamarin.Forms.Point LocationInView (Xamarin.Forms.VisualElement view)
		{
			return GetLocationInAncestorView (_currentPoint, view);
		}

		public void SetTranslationInView (Xamarin.Forms.Point translation, Xamarin.Forms.VisualElement view)
		{
			//NOT really supported in Android
			//TODO take account of view
			_rawTranslation = translation;
//			_currentPoint = translation;
//			_previousPoint = translation;
//			_startPoint = translation;
		}


		protected override bool ProcessMotionEvent (MotionEvent e)
		{
			Console.WriteLine ("pan gesture state - {0} e.action {1}", State, e.Action);
			if (e.Action == MotionEventActions.Down && PointerId == -1) {
				OnDown (e);
				return State == GestureRecognizerState.Possible;
			}

			if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed) {
				return false;
			} else {
				if (e.Action == MotionEventActions.Up && PointerId == e.GetPointerId (0)) {
					OnUp (e);
					return true;
				} else if (e.Action == MotionEventActions.Move && PointerId == e.GetPointerId (0)) {
					OnMove (e);
					return State != GestureRecognizerState.Cancelled && State != GestureRecognizerState.Ended && State != GestureRecognizerState.Failed;
				}
			}
			return false;
		}

		#endregion

		void OnUp (MotionEvent e)
		{
			
			var notifyGestureEnded = false;
			if (e.ActionMasked == MotionEventActions.Cancel) {
				State = GestureRecognizerState.Cancelled;
				Console.WriteLine ("GESTURE CANCELLED");
				notifyGestureEnded = true;
			} else if (e.ActionMasked == MotionEventActions.Up) {
				State = GestureRecognizerState.Ended;
				Console.WriteLine ("GESTURE ENDED");
				notifyGestureEnded = true;
			}
			if (notifyGestureEnded) {
				PointerId = -1;
				SendGestureEvent ();
			}
			//TODO think about if we're going to capture this
			return;
		}


		void OnDown (MotionEvent e)
		{
			//TODO - really should be possible at this point - min distance should trigger began
			State = GestureRecognizerState.Possible;
			PointerId = e.GetPointerId (0);
			Console.WriteLine ("PAN POSSIBLE");
			_rawTranslation = new Xamarin.Forms.Point (0, 0);
			_currentPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			_previousPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			_startPoint = new Xamarin.Forms.Point (NativeView.GetX (), NativeView.GetY ());
			SendGestureEvent ();
		}

		bool OnMove (MotionEvent e)
		{
			var currentViewPosition = new Xamarin.Forms.Point (NativeView.GetX (), NativeView.GetY ());
			var currentViewOffset = new Xamarin.Forms.Point (currentViewPosition.X - _startPoint.X, currentViewPosition.Y - _startPoint.Y);
			var eventPoint = new Xamarin.Forms.Point (e.GetX () + currentViewOffset.X, e.GetY () + currentViewOffset.Y);
			var coords = new MotionEvent.PointerCoords ();
			e.GetPointerCoords (0, coords);
			Console.WriteLine ("CHANGED e2{0} coords {1},{2}", eventPoint, coords.X, coords.Y);
			if (State != GestureRecognizerState.Possible && State != GestureRecognizerState.Began && State != GestureRecognizerState.Changed) {
				return false;
			} 
			if (e.GetX () < 0 || e.GetX () > NativeView.Width || e.GetY () < 0 || e.GetY () > NativeView.Height) {
				Console.WriteLine ("Gesture exited from view - it's over");
				State = GestureRecognizerState.Ended;
				PointerId = -1;
				SendGestureEvent ();
				return false;
			}

			if (e.ActionMasked == MotionEventActions.Move || e.ActionMasked == MotionEventActions.Scroll) {
				_previousPoint = _currentPoint;
				_currentPoint = eventPoint;
				_velocity = new Xamarin.Forms.Point (_currentPoint.X - _previousPoint.X, _currentPoint.Y - _previousPoint.Y);
				//TODO proper conversion
				_rawTranslation.X += _velocity.X / 2;
				_rawTranslation.Y += _velocity.Y / 2;
				State = State == GestureRecognizerState.Possible ? GestureRecognizerState.Began : GestureRecognizerState.Changed;
			}
//			Console.WriteLine ("State " + State + "_previousPoint " + _previousPoint + " _currentPoint" + _currentPoint);
			SendGestureEvent ();
			return false;
		}

	}
}

/**
 * previous idea was onscroll
 * 
 * public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			var currentViewPosition = new Xamarin.Forms.Point (NativeView.GetX (), NativeView.GetY ());
			var currentViewOffset = new Xamarin.Forms.Point (currentViewPosition.X - _startPoint.X, currentViewPosition.Y - _startPoint.Y);
			var e1Point = new Xamarin.Forms.Point (e1.GetX () + currentViewOffset.X, e1.GetY () + currentViewOffset.Y);
			var e2Point = new Xamarin.Forms.Point (e2.GetX () + currentViewOffset.X, e2.GetY () + currentViewOffset.Y);
			var coords = new MotionEvent.PointerCoords ();
			e2.GetPointerCoords (0, coords);
			Console.WriteLine ("CHANGED e2{0} coords {1},{2}", e2Point, coords.X, coords.Y);
			if (State != GestureRecognizerState.Began && State != GestureRecognizerState.Changed) {
				return false;
			} 
			if (e2.GetX () < 0 || e2.GetX () > NativeView.Width || e2.GetY () < 0 || e2.GetY () > NativeView.Height) {
				Console.WriteLine ("Gesture exited from view - it's over");
				State = GestureRecognizerState.Ended;
				PointerId = -1;
				OnGesture ();
				return false;
			}

			if (e2.ActionMasked == MotionEventActions.Move || e2.ActionMasked == MotionEventActions.Scroll) {
				_previousPoint = _currentPoint;
				_currentPoint = e2Point;
//				_currentTranslation = new Xamarin.Forms.Point (distanceX, distanceY);
//				_velocity = new Xamarin.Forms.Point (distanceX, distanceY);
				_velocity = new Xamarin.Forms.Point (_currentPoint.X - _previousPoint.X, _currentPoint.Y - _previousPoint.Y);
				//TODO proper conversion
				_rawTranslation.X += _velocity.X / 2;
				_rawTranslation.Y += _velocity.Y / 2;
				State = GestureRecognizerState.Changed;
//				Console.WriteLine ("CHANGED currentPoint {0} d {1},{2} e1 {3}", _currentPoint, distanceX, distanceY, );
//				Console.WriteLine ("CHANGED currentPoint {0} previousPoint {1}", _currentPoint, _previousPoint);
//				Console.WriteLine ("CHANGED currentPoint {0} veloocity {3} dx {1},{2}", _rawTranslation, distanceX, distanceY, _velocity);
//				Console.WriteLine ("CHANGED _currentTranslation {0} veloocity {3} dx {1},{2}", _rawTranslation, distanceX, distanceY, _velocity);
			}
//			Console.WriteLine ("State " + State + "_previousPoint " + _previousPoint + " _currentPoint" + _currentPoint);
			OnGesture ();
			return false;
		}
*/

