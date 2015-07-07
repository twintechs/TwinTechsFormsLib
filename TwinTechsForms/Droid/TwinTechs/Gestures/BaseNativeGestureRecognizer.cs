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

		BaseGestureRecognizer Recognizer { get; set; }

		protected View NativeView;

		protected Xamarin.Forms.Point FirstTouchPoint { get; set; }

		protected Xamarin.Forms.Point SecondTouchPoint { get; set; }

		public bool IsGestureInitialized { get; private set; }

		protected GestureDetector NativeGestureDetector { get; set; }

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
				NativeGestureDetector = null;
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
//			if (Recognizer.View is Xamarin.Forms.Layout<Xamarin.Forms.View>) {
//				GroupRecognizers.Add (Recognizer);
//			}
			if (NativeView == null) {
				throw new InvalidOperationException ("attempted to initialize a native gesture recognizers for a view before it had created it's renderer");
			}
			//check if we already have a multi-cast listener
			Listener = GetMultiCastListener ();
			if (Listener == null) {
				Listener = new MultiCastOnTouchListener ();
//				NativeView.SetOnTouchListener (Listener);
				NativeView.Touch += (sender, e) => {
					Console.WriteLine ("Touch " + e);
					
				};
				NativeView.GenericMotion += (object sender, View.GenericMotionEventArgs e) => {
					Console.WriteLine ("motion " + e);
				};
			}
			Listener.AddListener (this);
			NativeGestureDetector = CreateGestureDetector ();
			IsGestureInitialized = true;
		}

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
			PreProcessMotionEvent (e);
			var result = false;
			if (NativeGestureDetector != null) {
				result = NativeGestureDetector.OnTouchEvent (e);
			}
			PostProcessMotionEvent (e);
			return result;
		}

		/// <summary>
		/// Pre process motion event. Do anythign extra outside of the detector
		/// </summary>
		/// <param name="e">E.</param>
		protected virtual void PreProcessMotionEvent (MotionEvent e)
		{
		}

		/// <summary>
		/// Posts the process motion event. Do anythign extra outside of the detector
		/// </summary>
		/// <param name="e">E.</param>
		protected virtual void PostProcessMotionEvent (MotionEvent e)
		{
		}

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

		protected void OnGesture ()
		{
			Recognizer.SendAction ();
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

		/// <summary>
		/// Creates the gesture detector. Your view should set up a detector which is using the 
		/// Listener property to get touch events.
		/// </summary>
		protected abstract GestureDetector CreateGestureDetector ();

		//		public override bool OnDown (MotionEvent e)
		//		{
		//			return base.OnDown (e);
		//		}


		#region handling gestures at a group level

		/**
		 * We have an issue with Android that makes it pretty much impossible to compose gestures. 
		 * For now, this is all parked on android till I come up with a solution that works.
		 */
		/**
		 * Because we are composing, we have to do a bit of hackery to get touches from the main activity
		 */
		#endregion
		public bool ConsumesActivityTouch (MotionEvent ev)
		{
			//TODO work out if it's our view in here, then update the coordinates
			var offset = GetOffsetInNativeView (ev.GetX (), ev.GetY ());
			Console.WriteLine ("location " + ev.GetX () + ", " + ev.GetY () + " offset " + offset);
			ev.OffsetLocation (-offset.X, -offset.Y);
			Console.WriteLine ("ofseeted " + ev.GetX () + ", " + ev.GetY ());
			if (ev.GetX () < 0 || ev.GetY () < 0 || ev.GetX () > NativeView.Width || ev.GetY () > NativeView.Height) {
				return false;
			} else {
				//next check children
//				var isConsumedByChild = IsTouchConsumedByChildren (NativeView as ViewGroup, ev);
				//we need to check all views to see if they ate the touch
				OnTouch (this.NativeView, ev);
			}
			return false;
		}

		//		bool IsTouchConsumedByChildren (ViewGroup viewGroup, MotionEvent ev)
		//		{
		//			var isConsumed = false;
		//			if (viewGroup != null) {
		//
		//				for (int i = 0; i < viewGroup.ChildCount; i++) {
		//					var view = viewGroup.GetChildAt (i);
		//					//TODO offset the touch
		//					view.OnTouchEvent (ev);
		//					isConsumed = IsTouchConsumedByChildren (view as ViewGroup, ev);
		//					if (isConsumed) {
		////						break;
		//					}
		//				}
		//			}
		//			return isConsumed;
		//			
		//		}

		protected Point GetOffsetInNativeView (double X, double Y)
		{
			var parent = NativeView;
			var offset = new Point ();
			while (parent != null) {
				offset.X += parent.Left;
				offset.Y += parent.Top;
				parent = parent.Parent as View;
			} 
			return offset;
		}
	}
}

