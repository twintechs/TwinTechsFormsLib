using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Java.Lang;
using System.Collections.Generic;
using System.Threading;

namespace TwinTechs.Gestures
{
	public abstract class BaseNativeGestureRecognizer : Java.Lang.Object, INativeGestureRecognizer,View.IOnTouchListener
	{
		public BaseNativeGestureRecognizer ()
		{
		}

		public static List<BaseGestureRecognizer> GroupRecognizers = new List<BaseGestureRecognizer> ();

		internal MultiCastOnTouchListener Listener { get; set; }

		/// <summary>
		/// The pointer identifier. - TODO track more touches, and provide a mechanism for a subclass to easily indicate a touch as being actively tracked
		/// </summary>
		protected int PointerId = -1;

		protected BaseGestureRecognizer Recognizer { get; set; }

		protected View NativeView;

		/// <summary>
		/// Gets a value indicating whether the motion event is cancelled for other touches
		/// </summary>
		protected abstract bool IsMotionEventCancelled { get; }

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

		public bool IsGestureInitialized { get; private set; }

		#region INativeGestureRecognizer implementation

		public void AddRecognizer (BaseGestureRecognizer recognizer)
		{
			Recognizer = recognizer;
			var renderer = Recognizer.View.GetRenderer ();
			if (renderer == null) {
				Recognizer.View.PropertyChanged += Recognizer_View_PropertyChanged;
			} else {
				InitializeNativeRecognizer ();
			}
		
		}

		public void RemoveRecognizer (BaseGestureRecognizer recognizer)
		{
			if (IsGestureInitialized) {
				Listener = GetMultiCastListener ();
				if (Listener == null) {
					throw new IllegalStateException ("the view's multi caster was removed before removing a listener");
				}

				Listener.RemoveListener (this);
				if (!Listener.HasListeners) {
					NativeView.SetOnTouchListener (null);
				}
				NativeView.SetOnTouchListener (null);
				IsGestureInitialized = false;
			}
		}

		#endregion


		void Recognizer_View_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer") {
				var renderer = Recognizer.View.GetRenderer ();
				if (renderer != null && NativeView == null) {
					InitializeNativeRecognizer ();
				} else if (renderer == null && NativeView != null && !IsGestureInitialized) {
					RemoveRecognizer (Recognizer);
				}
			}
		}

		void InitializeNativeRecognizer ()
		{
			NativeView = Recognizer.View.GetNativeView ();
			//NEED A MECHANISM TO GET TOUCHES FROM A PARENT VIEW OBSCURED BY ANOTHER VIEW
			//I was planning to have an activity level touch listner, which would check with a gesture recognizer registered
			// with GroupRecognizers, before posting to view
			// but I ran out of time.
			if (Recognizer.IsConsumingTouchesInParallel && Recognizer.View is Xamarin.Forms.Layout<Xamarin.Forms.View>) {
				GroupRecognizers.Add (Recognizer);
			}
			if (NativeView == null) {
				throw new InvalidOperationException ("attempted to initialize a native gesture recognizers for a view before it had created it's renderer");
			}
			Console.WriteLine ("splitting " + (NativeView as ViewGroup).MotionEventSplittingEnabled);
			//check if we already have a multi-cast listener
			//note - not actually using the multi cast listener right now -it'd be trivial to add it back in
			//but I've been doign a lot of experiments with simultaneous touches on android and wanted to reduce noise
			Listener = GetMultiCastListener ();
			if (Listener == null) {
				Listener = new MultiCastOnTouchListener ();
//				NativeView.SetOnTouchListener (Listener);
				NativeView.Touch += (sender, e) => {
					OnTouch (NativeView, e.Event);
//					Console.WriteLine ("Touch " + e.Event);
					
				};
//				NativeView.GenericMotion += (object sender, View.GenericMotionEventArgs e) => {
//					Console.WriteLine ("++motion " + e);
//				};
			}
			Listener.AddListener (this);
			IsGestureInitialized = true;
		}

		/// <summary>
		/// Gets the multi cast listener, if any - this allows us to put several gestures on one view, without over complicating
		/// the PCL GestureRecognizer class
		/// </summary>
		/// <returns>The multi cast listener.</returns>
		MultiCastOnTouchListener GetMultiCastListener ()
		{
			foreach (var recognizer in Recognizer.View.GestureRecognizers) {
				if (recognizer is BaseGestureRecognizer) {
					var nativeRecognizer = (recognizer as BaseGestureRecognizer).NativeGestureRecognizer as BaseNativeGestureRecognizer;
					if (nativeRecognizer != null && nativeRecognizer.Listener != null) {
						return nativeRecognizer.Listener;
					}
				}
			}
			return null;
		}


		#region IOnTouchListener impl

		public bool OnTouch (View nativeView, MotionEvent e)
		{
			var captured = ProcessMotionEvent (e);
			return captured;
		}

		/// <summary>
		/// Processes the motion event.
		/// </summary>
		/// <returns><c>true</c>, if motion event was processed/captured, <c>false</c> otherwise.</returns>
		/// <param name="e">E.</param>
		protected abstract bool ProcessMotionEvent (MotionEvent e);

		#endregion

		Dictionary<MotionEventActions, GestureRecognizerState> _stateMap = new Dictionary<MotionEventActions, GestureRecognizerState> () {
			[ MotionEventActions.Cancel ]	= GestureRecognizerState.Cancelled,
			[ MotionEventActions.Move ]	= GestureRecognizerState.Changed,
		};


		protected virtual void UpdateState (MotionEvent e)
		{
			if (_stateMap.ContainsKey (e.Action)) {
				State = _stateMap [e.Action];
			}
		}

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
			var targetViewRenderer = view.GetRenderer ();
			var targetView = targetViewRenderer.ViewGroup;
			var parent = NativeView;
			var returnPoint = location;
			while (parent != null && parent != targetView) {
				returnPoint.X += parent.Left;
				returnPoint.Y += parent.Top;
				parent = NativeView.Parent as View;
			} 
			return returnPoint;
		}

		#region handling gestures at a group level

		/**
		 * We have an issue with Android that makes it pretty much impossible to compose gestures. 
		 * Below is my stab at picking up gestures from the activity - the idea is that the main activity woudl pass all touches to this method
		 * the system will work, and will work very richly (with delay touch, cancel touches, delegate callback for should 
		 * recognize in parallel, etc)
		 * However, my time is limited, so this is crude for now.
		 */
		public bool ConsumesActivityTouch (MotionEvent ev)
		{
			var didConsume = false;
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

				//TODO - ask the view if it's happy to process this touch at the same time as another gesture - I see no way to make it work for views.. :/
				//that will be done by 2 parses - one to discover all *gestures* that want the touch, then another parse to go back through and either cancel
				//or pass the touches long
				didConsume = OnTouch (this.NativeView, ev);
				//TODO add OnCancel method

				//remove the offset
				ev.OffsetLocation ((float)-offset.X, (float)-offset.Y);
			}
			//TODO - we should consider that some gestures want to cancel touches.
			return didConsume;
//			Console.WriteLine ("location " + ev.GetX () + ", " + ev.GetY () + " offset " + offset);
		}

		/// <summary>
		/// reports if this gesture will cancel the touch for further (non gesture) processing.
		/// </summary>
		/// <returns><c>true</c>, if cancel activity touch was willed, <c>false</c> otherwise.</returns>
		/// <param name="ev">Ev.</param>
		public bool CancelsActivityTouch (MotionEvent ev)
		{
			return IsMotionEventCancelled;
		}

		#endregion

	}
}


