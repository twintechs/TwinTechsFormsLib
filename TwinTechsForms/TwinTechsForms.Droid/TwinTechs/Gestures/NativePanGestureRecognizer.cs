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
		Xamarin.Forms.Point _viewStartLocation;

		//TODO gesture property?
		const double _minimumMoveDistance = 5;

		#region implemented abstract members of BaseNativeGestureRecognizer and overrides

		protected override bool IsGestureCotinuous {
			get {
				return true;
			}
		}

		public Xamarin.Forms.Point GetVelocityInView (Xamarin.Forms.VisualElement view)
		{
			return _velocity;
		}

		public Xamarin.Forms.Point GetTranslationInView (Xamarin.Forms.VisualElement view)
		{
			return new Xamarin.Forms.Point (_rawTranslation.X, _rawTranslation.Y);
		}


		public override Xamarin.Forms.Point LocationInView (Xamarin.Forms.VisualElement view)
		{
			return GetLocationInAncestorView (_currentScreenPoint, view);
		}

		public void SetTranslationInView (Xamarin.Forms.Point translation, Xamarin.Forms.VisualElement view)
		{
			//Experimental android support
			_rawTranslation = translation;
		}


		internal override void ProcessMotionEvent (GestureMotionEvent e)
		{
//			Console.WriteLine ("pan gesture state - {0} e.action {1}", State, e.Action);
			if (e.Action == MotionEventActions.Down && PointerId == -1) {
				OnDown (e);
//				e.IsConsumed = State == GestureRecognizerState.Possible;
			} else if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed) {
				return;
			} else {
				if (PointerId == e.GetPointerId (0)) {
					if (e.ActionMasked == MotionEventActions.Cancel || e.ActionMasked == MotionEventActions.Up) {
						OnUp (e);
//						e.IsConsumed = true;
					} else if (e.Action == MotionEventActions.Move) {
						OnMove (e);
//						e.IsConsumed = State != GestureRecognizerState.Cancelled && State != GestureRecognizerState.Ended && State != GestureRecognizerState.Failed;
					}
				} 
			}
		}

		#endregion

		Xamarin.Forms.Point _currentScreenPoint;

		void OnUp (GestureMotionEvent e)
		{
			if (State == GestureRecognizerState.Possible) {
				State = GestureRecognizerState.Failed;
			} else if (e.ActionMasked == MotionEventActions.Cancel) {
				State = GestureRecognizerState.Cancelled;
			} else if (e.ActionMasked == MotionEventActions.Up) {
				State = GestureRecognizerState.Ended;
			}
		}


		void OnDown (GestureMotionEvent e)
		{
			//TODO - really should be possible at this point - min distance should trigger began
			State = GestureRecognizerState.Possible;
			PointerId = e.GetPointerId (0);
			Console.WriteLine ("PAN POSSIBLE");
			_rawTranslation = new Xamarin.Forms.Point (0, 0);
			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ()); 
			_currentPoint = FirstTouchPoint;
			_currentScreenPoint = FirstTouchPoint;
			_previousPoint = FirstTouchPoint;
			_viewStartLocation = new Xamarin.Forms.Point (NativeView.GetX (), NativeView.GetY ());
		}

		void OnMove (GestureMotionEvent e)
		{
			_currentScreenPoint = new Xamarin.Forms.Point (e.GetX (), e.GetY ());
			var currentViewPosition = new Xamarin.Forms.Point (NativeView.GetX (), NativeView.GetY ());
			var currentViewOffset = new Xamarin.Forms.Point (currentViewPosition.X - _viewStartLocation.X, currentViewPosition.Y - _viewStartLocation.Y);
			var eventPoint = new Xamarin.Forms.Point (e.GetX () + currentViewOffset.X, e.GetY () + currentViewOffset.Y);
			var coords = new MotionEvent.PointerCoords ();
			e.GetPointerCoords (0, coords);
//			Console.WriteLine ("CHANGED e2{0} coords {1},{2}", eventPoint, coords.X, coords.Y);
			if (State == GestureRecognizerState.Possible || State == GestureRecognizerState.Began || State == GestureRecognizerState.Changed) {

				if (e.GetX () < 0 || e.GetX () > NativeView.Width || e.GetY () < 0 || e.GetY () > NativeView.Height) {
					Console.WriteLine ("Gesture exited from view - it's over");
					State = GestureRecognizerState.Ended;
				} else if (e.ActionMasked == MotionEventActions.Move || e.ActionMasked == MotionEventActions.Scroll) {
					_previousPoint = _currentPoint;
					_currentPoint = eventPoint;
					_velocity = new Xamarin.Forms.Point (_currentPoint.X - _previousPoint.X, _currentPoint.Y - _previousPoint.Y);

					if (State == GestureRecognizerState.Possible && !IsMoveSufficientToBegin (_currentScreenPoint)) {
						Console.WriteLine ("not moved enough - not starting");
						return;
					}
					//TODO proper conversion
					_rawTranslation.X += _velocity.X / 2;
					_rawTranslation.Y += _velocity.Y / 2;
					State = State == GestureRecognizerState.Possible ? GestureRecognizerState.Began : GestureRecognizerState.Changed;
					e.IsCancelled = Recognizer.CancelsTouchesInView;
				}
//			Console.WriteLine ("State " + State + "_previousPoint " + _previousPoint + " _currentPoint" + _currentPoint);
			}
		}

		bool IsMoveSufficientToBegin (Xamarin.Forms.Point newLocation)
		{
			var absoluteDistance = new Xamarin.Forms.Point (FirstTouchPoint.X - newLocation.X, FirstTouchPoint.Y - newLocation.Y);
			var absX = Math.Abs (absoluteDistance.X);
			var absY = Math.Abs (absoluteDistance.Y);
			var biggestValue = Math.Max (absX, absY);
			return biggestValue >= _minimumMoveDistance;
		}

	}
}

