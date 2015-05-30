using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Java.Lang;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	public abstract class BaseNativeGestureRecognizer : Java.Lang.Object, INativeGestureRecognizer, IOnTouchListener
	{
		public BaseNativeGestureRecognizer ()
		{
		}

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
			if (NativeView == null) {
				throw new InvalidOperationException ("attempted to initialize a native gesture recognizers for a view before it had created it's renderer");
			}
			//check if we already have a multi-cast listener
			Listener = GetMultiCastListener ();
			if (Listener == null) {
				Listener = new MultiCastOnTouchListener ();
				NativeView.SetOnTouchListener (Listener);
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
			NativeGestureDetector.OnTouchEvent (e);
			return true;
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
			return FirstTouchPoint;
		}

		public virtual  Xamarin.Forms.Point LocationOfTouch (int touchIndex, Xamarin.Forms.VisualElement view)
		{
			return FirstTouchPoint;
		}

		public int NumberOfTouches { get; protected set; }

		public GestureRecognizerState State { get; protected set; }

		protected void OnGesture ()
		{
			Recognizer.SendAction ();
		}

		/// <summary>
		/// Creates the gesture detector. Your view should set up a detector which is using the 
		/// Listener property to get touch events.
		/// </summary>
		protected abstract GestureDetector CreateGestureDetector ();
	}
}

