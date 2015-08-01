using System;
using Android.App;
using Android.Views;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	/// <summary>
	/// Helper class which provides a mechanism for android apps to dispatch touches to 
	/// multiple views which facilitates composing gesture recognition, 
	/// without having to have custom subclasses, while providing better orchestration of touches to gestures.
	/// 
	/// The class hooks into an activity, which is expected to contain all of the view groups with views with touches
	/// to coordinate
	/// </summary>
	public class GestureTouchDispatcher
	{
		Activity _activity { get; set; }

		Dictionary<MotionEvent,GestureMotionEvent> _delayedMotionEvents = new Dictionary<MotionEvent,GestureMotionEvent> ();

		public GestureTouchDispatcher (Activity activity)
		{
			_activity = activity;
		}


		public bool DispatchTouchEvent (MotionEvent ev)
		{
			if (_delayedMotionEvents.ContainsKey (ev)) {
				Console.WriteLine ("was delayed event - processing now " + ev);
				var gestureEvent = _delayedMotionEvents [ev];
				_delayedMotionEvents.Remove (ev);
				var restoredEvent = gestureEvent.GetCachedEvent ();
				_activity.DispatchTouchEvent (restoredEvent);
				var handled = _delayedMotionEvents.Remove (restoredEvent);
				return handled;
			}
			var gestureMotionEvent = new GestureMotionEvent (ev);
			//find if there's a view container with a gesture, which is currently on the screen.
			foreach (var recognizer in NativeGestureCoordinator.GroupRecognizers) {
				var nativeRecognizer = recognizer.NativeGestureRecognizer as BaseNativeGestureRecognizer;
				//				Console.WriteLine ("checkign gesture touch");
				nativeRecognizer.ProcessGestureMotionEvent (gestureMotionEvent);
				gestureMotionEvent.IsConsumed = GetIsConsumedState (nativeRecognizer.State);
			}

			if (gestureMotionEvent.IsConsumed && gestureMotionEvent.IsCancelled) {
				ev.Action = MotionEventActions.Cancel;
			}
			if (gestureMotionEvent.IsMarkedForDelay) {
				_delayedMotionEvents [ev] = gestureMotionEvent;
			}
			return gestureMotionEvent.IsConsumed;
		}


		bool GetIsConsumedState (GestureRecognizerState state)
		{
			return state == GestureRecognizerState.Ended || state == GestureRecognizerState.Began ||
			state == GestureRecognizerState.Recognized || state == GestureRecognizerState.Changed;
		}
	}
}

