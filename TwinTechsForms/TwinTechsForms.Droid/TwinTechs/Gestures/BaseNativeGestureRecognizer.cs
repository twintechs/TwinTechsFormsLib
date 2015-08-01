using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using System.Linq;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public abstract class BaseNativeGestureRecognizer : Java.Lang.Object, INativeGestureRecognizer
	{
		/// <summary>
		/// The pointer identifier. - TODO track more touches, and provide a mechanism for a subclass to easily indicate a touch as being actively tracked
		/// </summary>
		protected int PointerId = -1;

		internal BaseGestureRecognizer Recognizer { get; set; }

		internal Android.Views.View NativeView { get; set; }

		/// <summary>
		/// Gets or sets the first touch point - for convenience
		/// </summary>
		/// <value>The first touch point.</value>
		protected Xamarin.Forms.Point FirstTouchPoint { get; set; }


		/// <summary>
		/// Gets or sets the second touch point - for convenience
		/// </summary>
		/// <value>The second touch point.</value>
		protected Xamarin.Forms.Point SecondTouchPoint { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this gesture is continuous or a one off.
		/// If it's not continuous, then only recognized events get dispatched
		/// override if your gesture does things like pinch/pan/zoom/rotate, which have discrete active phases.
		/// </summary>
		/// <value><c>true</c> if this instance is a continuous gesture; otherwise, <c>false</c>.</value>
		protected virtual bool IsGestureCotinuous { get { return false; } }

		/// <summary>
		/// Processes the motion event.
		/// Overriding methods should updat the GestureMotionEvent state properties, marking for cancels, delays or consumption
		/// </summary>
		/// <param name="e">E.</param>
		internal abstract void ProcessMotionEvent (GestureMotionEvent e);

		#region INativeGestureRecognizer impl


		public void UpdateCancelsTouchesInView (bool _cancelsTouchesInView)
		{
			//does nothign on android
		}

		public void UpdateDelaysTouches (bool _delaysTouches)
		{
			//does nothign on android
		}


		public virtual Xamarin.Forms.Point LocationInView (Xamarin.Forms.VisualElement view)
		{
			return GetLocationInAncestorView (FirstTouchPoint, view);
		}

		public virtual  Xamarin.Forms.Point LocationOfTouch (int touchIndex, Xamarin.Forms.VisualElement view)
		{
			return GetLocationInAncestorView (FirstTouchPoint, view);
		}

		public int NumberOfTouches { get; protected set; }

		bool _gestureDidBegin;

		GestureRecognizerState _state;

		public GestureRecognizerState State {
			get {
				return _state;
			}
			protected set {
				var oldState = _state;
				_state = value; 
				if (oldState == GestureRecognizerState.Possible && value == GestureRecognizerState.Began) {
					if (Recognizer.OnGestureShouldBeginDelegate != null && !Recognizer.OnGestureShouldBeginDelegate (Recognizer)) {
						_state = GestureRecognizerState.Failed;
					}	
				}

				if (_state == GestureRecognizerState.Cancelled || _state == GestureRecognizerState.Ended || _state == GestureRecognizerState.Failed) {
					PointerId = -1;
				}

				//we track if the gesture had begun at some point in processing this gesture, so we can elect which continuous events to send
				if (_state == GestureRecognizerState.Began) {
					_gestureDidBegin = true;
				}

				if (_state == GestureRecognizerState.Recognized || (IsGestureCotinuous && _gestureDidBegin)) {
					SendGestureEvent ();
				}

				if (GetIsFinishedState (_state)) {
					_gestureDidBegin = false;
				}
			}
		}

		bool GetIsFinishedState (GestureRecognizerState state)
		{
			return state == GestureRecognizerState.Ended || state == GestureRecognizerState.Cancelled || state == GestureRecognizerState.Recognized ||
			state == GestureRecognizerState.Failed;
		}

		private void SendGestureEvent ()
		{
			if (Recognizer != null) {
				Device.BeginInvokeOnMainThread (() => {
					Recognizer.SendAction ();
				});
			}
		}

		protected Xamarin.Forms.Point GetLocationInAncestorView (Xamarin.Forms.Point location, Xamarin.Forms.VisualElement view)
		{
			int[] nativeViewLocation = new int[2];
			NativeView.GetLocationOnScreen (nativeViewLocation);
			var nativeViewLocationOnScreen = new Xamarin.Forms.Point (nativeViewLocation [0], nativeViewLocation [1]);

			var offsetLocation = new Xamarin.Forms.Point (location.X + nativeViewLocationOnScreen.X, location.Y + nativeViewLocationOnScreen.Y);

			var targetViewRenderer = view.GetRenderer ();
			var targetView = targetViewRenderer.ViewGroup;
			int[] targetViewLocation = new int[2];
			targetView.GetLocationOnScreen (targetViewLocation);
			var nativeViewScreenLocation = new Xamarin.Forms.Point (targetViewLocation [0], targetViewLocation [1]);

			var returnPoint = offsetLocation;
			returnPoint.X -= nativeViewScreenLocation.X;
			returnPoint.Y -= nativeViewScreenLocation.Y;

			Console.WriteLine ("offsetLocation {0} nativeViewLocationOnScreen {1} returnPoint", offsetLocation, nativeViewLocationOnScreen);
			Console.WriteLine ("location {0} parentViewLoc {1} returnPoint {2}", location, nativeViewScreenLocation, returnPoint);
			return returnPoint;
		}

		#endregion


		#region handling gestures at a group level

		/**
		 * We have an issue with Android that makes it pretty much impossible to compose gestures. 
		 * Below is my stab at picking up gestures from the activity - the idea is that the main activity would pass all touches to this method
		 * the system will work, and will work very richly (with delay touch, cancel touches, delegate callback for should 
		 * recognize in parallel, etc)
		 * However, my time is limited, so this is crude for now.
		 */
		public void ProcessGestureMotionEvent (GestureMotionEvent gestureEvent)
		{
			var ev = gestureEvent.MotionEvent;

			//TODO work out if it's our view in here, then update the coordinates
			int[] location = new int[2];
			NativeView.GetLocationOnScreen (location);
			var nativeViewScreenLocation = new Xamarin.Forms.Point (location [0], location [1]);
			var offset = Xamarin.Forms.Point.Zero;
			var touchPoint = new Xamarin.Forms.Point (ev.GetX (), ev.GetY ());
			var mainPointerId = ev.GetPointerId (0);
			//1. is it inside the view? 
			Console.WriteLine ("touch point {0} view bounds {1} size {2},{3}", touchPoint, nativeViewScreenLocation, NativeView.Width, NativeView.Height);
			var isInsideOfView = touchPoint.X >= nativeViewScreenLocation.X && touchPoint.Y >= nativeViewScreenLocation.Y &&
			                     touchPoint.X <= (NativeView.Width + nativeViewScreenLocation.X) && touchPoint.Y <= (NativeView.Height + nativeViewScreenLocation.Y);

			//2. report touches inside, or outside but tracked? (so cancels can occur)
			//TODO track more touches
			if (isInsideOfView || PointerId == mainPointerId) {
				//if letting the view know, translate the coords into local view coords (apply the offsets to the touch)
				offset.X = -nativeViewScreenLocation.X;
				offset.Y = -nativeViewScreenLocation.Y;
				ev.OffsetLocation ((float)offset.X, (float)offset.Y);
				var offsetLocation = new Xamarin.Forms.Point (ev.GetX (), ev.GetY ());
				if (isInsideOfView) {
					Console.WriteLine ("INSIDE " + ev.Action + " offset " + offset + " results in " + offsetLocation);
				} else {
					Console.WriteLine ("touch outside view, but was tracked " + offset);
				}

				//TODO - ask the view if it's happy to process this touch at the same time as another gesture - I see no way to make it work for views.. (without 
				//an entire Touch dispatching mechanism:/)
				//that will be done by 2 parses - one to discover all *gestures* that want the touch, then another parse to go back through and either cancel
				//or pass the touches long
				//that's not implemented yet though (time)
				ProcessMotionEvent (gestureEvent);

				//remove the offset
				ev.OffsetLocation ((float)-offset.X, (float)-offset.Y);
			}
//			Console.WriteLine ("location " + ev.GetX () + ", " + ev.GetY () + " offset " + offset);
		}


		#endregion

	}
}


