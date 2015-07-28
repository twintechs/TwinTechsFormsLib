using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Java.Lang;
using System.Collections.Generic;
using System.Threading;
using Android.Text.Method;
using Android.Gestures;

namespace TwinTechs.Gestures
{
	public abstract class BaseNativeGestureRecognizer : Java.Lang.Object, INativeGestureRecognizer
	{
		/// <summary>
		/// The pointer identifier. - TODO track more touches, and provide a mechanism for a subclass to easily indicate a touch as being actively tracked
		/// </summary>
		protected int PointerId = -1;

		internal BaseGestureRecognizer Recognizer { get; set; }

		internal View NativeView { get; set; }

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
		/// Processes the motion event.
		/// Overriding methods should updat the GestureMotionEvent state properties, marking for cancels, delays or consumption
		/// </summary>
		/// <param name="e">E.</param>
		internal abstract void ProcessMotionEvent (GestureMotionEvent e);
		//
		//		Dictionary<MotionEventActions, GestureRecognizerState> _stateMap = new Dictionary<MotionEventActions, GestureRecognizerState> () {
		//			[ MotionEventActions.Cancel ]	= GestureRecognizerState.Cancelled,
		//			[ MotionEventActions.Move ]	= GestureRecognizerState.Changed,
		//		};
		//
		//
		//		protected virtual void UpdateState (MotionEvent e)
		//		{
		//			if (_stateMap.ContainsKey (e.Action)) {
		//				State = _stateMap [e.Action];
		//			}
		//		}

		//TODO work out how to do this..
		public virtual Xamarin.Forms.Point LocationInView (Xamarin.Forms.VisualElement view)
		{
			return GetLocationInAncestorView (FirstTouchPoint, view);
		}

		public virtual  Xamarin.Forms.Point LocationOfTouch (int touchIndex, Xamarin.Forms.VisualElement view)
		{
			return GetLocationInAncestorView (FirstTouchPoint, view);
		}

		public int NumberOfTouches { get; protected set; }

		public GestureRecognizerState State { get; protected set; }

		protected void SendGestureEvent ()
		{
			Recognizer.SendAction ();
		}

		protected void SendGestureUpdate ()
		{
			Recognizer.SendUpdate ();
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

		//
		//		protected Xamarin.Forms.Point GetLocationInAncestorView (Xamarin.Forms.Point location, Xamarin.Forms.VisualElement view)
		//		{
		//			var targetViewRenderer = view.GetRenderer ();
		//			var targetView = targetViewRenderer.ViewGroup;
		//			var parent = NativeView;
		//			var returnPoint = location;
		//			while (parent != null && parent != targetView) {
		//				returnPoint.X += parent.Left;
		//				returnPoint.Y += parent.Top;
		//				parent = NativeView.Parent as View;
		//			}
		//			return returnPoint;
		//		}

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


