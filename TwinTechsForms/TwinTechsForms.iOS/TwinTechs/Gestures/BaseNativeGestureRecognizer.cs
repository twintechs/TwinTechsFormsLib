using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using TwinTechs.Ios.Extensions;

namespace TwinTechs.Gestures
{
	public abstract class BaseNativeGestureRecognizer<NativeGestureType,T> : INativeGestureRecognizer
		where NativeGestureType : UIGestureRecognizer 
		where T : BaseGestureRecognizer
	{

		protected T Recognizer { get; set; }

		protected UIView NativeView { get; set; }

		protected NativeGestureType NativeRecognizer { get; set; }

		protected virtual void ConfigureNativeGestureRecognizer ()
		{
			NativeRecognizer.CancelsTouchesInView = Recognizer.CancelsTouchesInView;
			NativeRecognizer.DelaysTouchesBegan = Recognizer.DelaysTouchesBegan;
			NativeRecognizer.DelaysTouchesEnded = Recognizer.DelaysTouchesEnded;
		}

		#region INativeGestureRecognizer impl

		public void AddRecognizer (BaseGestureRecognizer recognizer)
		{
			Recognizer = (T)recognizer;
			var renderer = Recognizer.View.GetRenderer ();
			if (renderer == null) {
				Recognizer.View.PropertyChanged += Recognizer_View_PropertyChanged;
			} else {
				InitializeNativeRecognizer ();
			}
		}

		void Recognizer_View_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer") {
				var renderer = Recognizer.View.GetRenderer ();
				if (renderer != null && NativeView == null) {
					InitializeNativeRecognizer ();
				} else if (renderer == null && NativeView != null && NativeRecognizer != null) {
					RemoveRecognizer (Recognizer);
				}
			}
		}

		void InitializeNativeRecognizer ()
		{
			var renderer = Recognizer.View.GetRenderer ();
			if (renderer == null) {
				throw new InvalidOperationException ("attempted to initialize a native gesture recognizers for a view before it had created it's renderer");
			}

			NativeView = renderer.NativeView;

			//workaroudn for irritating bugn which causes renderer to fail
			if (typeof(NativeGestureType).Equals (typeof(UIPinchGestureRecognizer))) {
				NativeRecognizer = new UIPinchGestureRecognizer (OnGesture) as NativeGestureType;
			} else {
				Action<NativeGestureType> action = OnGesture;
				NativeRecognizer = (NativeGestureType)Activator.CreateInstance (typeof(NativeGestureType), action);
			}

			ConfigureNativeGestureRecognizer ();

			NativeView.UserInteractionEnabled = true;
			NativeView.AddGestureRecognizer (NativeRecognizer);
		}

		public void RemoveRecognizer (BaseGestureRecognizer recognizer)
		{
			NativeView.RemoveGestureRecognizer (NativeRecognizer);
			NativeRecognizer = null;
			recognizer.NativeGestureRecognizer = null;
		}

		public GestureRecognizerState State {
			get { 
				return (GestureRecognizerState)(NativeRecognizer != null ? 
					GetGestureRecognizerStateFromUIState (NativeRecognizer.State) : GestureRecognizerState.Failed);
			}
		}

		public Point LocationInView (VisualElement view)
		{
			if (NativeRecognizer != null) {
				
				var renderer = view.GetRenderer ();
				return NativeRecognizer.LocationInView (renderer.NativeView).ToPoint ();
			} else {
				return Point.Zero;
			}
		}

		public Point LocationOfTouch (int touchIndex, VisualElement view)
		{
			if (NativeRecognizer != null) {
				var renderer = view.GetRenderer ();
				return NativeRecognizer.LocationOfTouch (touchIndex, renderer.NativeView).ToPoint ();
			} else {
				return Point.Zero;
			}
		}

		public int NumberOfTouches {
			get {
				return (int)NativeRecognizer.NumberOfTouches;
			}
		}


		GestureRecognizerState GetGestureRecognizerStateFromUIState (UIGestureRecognizerState state)
		{
			switch (state) {
			case UIGestureRecognizerState.Possible:
				return GestureRecognizerState.Possible;
			case UIGestureRecognizerState.Began:
				return GestureRecognizerState.Began;
			case UIGestureRecognizerState.Changed:
				return GestureRecognizerState.Changed;
			case UIGestureRecognizerState.Ended:
				return GestureRecognizerState.Ended;
			case UIGestureRecognizerState.Cancelled:
				return GestureRecognizerState.Cancelled;
			case UIGestureRecognizerState.Failed:
				return GestureRecognizerState.Failed;
			default:
				throw new ArgumentOutOfRangeException ();
			}
		}


		void OnGesture (UIGestureRecognizer recognizer)
		{
			Recognizer.SendAction ();
		}

		#endregion
	}
}

