using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	public class NativeGestureCoordinator : INativeGestureRecognizerCoordinator
	{
		public static List<BaseGestureRecognizer> GroupRecognizers = new List<BaseGestureRecognizer> ();

		List<BaseNativeGestureRecognizer> NativeRecognizers { get; set; }

		Xamarin.Forms.View TargetView { get; set; }

		Android.Views.View _nativeView;

		bool _isInitialized;

		public bool HasRecognizers { get { return NativeRecognizers.Count > 0; } }

		public NativeGestureCoordinator (Xamarin.Forms.View targetView)
		{
			NativeRecognizers = new List<BaseNativeGestureRecognizer> ();
			TargetView = targetView;
			TargetView.InputTransparent = false;
			var renderer = TargetView.GetRenderer ();
			if (renderer == null) {
				TargetView.PropertyChanged += Recognizer_View_PropertyChanged;
			} else {
				Initialize ();
			}
		}

		public void Destroy ()
		{
			TargetView.PropertyChanged -= Recognizer_View_PropertyChanged;
			UnInitialize ();

			//remove any remaining gesture recognizers;
			var recognizersToRemove = NativeRecognizers.ToArray ();
			foreach (var recognizer in recognizersToRemove) {
				RemoveRecognizer (recognizer);
			}
		}

		public void AddRecognizer (BaseNativeGestureRecognizer nativeRecognizer)
		{
			NativeRecognizers.Add (nativeRecognizer);
			if (nativeRecognizer.Recognizer.View != TargetView) {
				throw new InvalidOperationException ("tried to add recognizer to gesture coordinator that is associated with a different view");
			}
			nativeRecognizer.NativeView = _nativeView;
			var formsRecognizer = nativeRecognizer.Recognizer;
			if (formsRecognizer.IsConsumingTouchesInParallel) { // && formsRecognizer.View is Xamarin.Forms.Layout<Xamarin.Forms.View>) {
				GroupRecognizers.Add (formsRecognizer);
			}
		}

		public void RemoveRecognizer (BaseNativeGestureRecognizer recognizer)
		{
			NativeRecognizers.Remove (recognizer);
			if (GroupRecognizers.Contains (recognizer.Recognizer)) {
				GroupRecognizers.Remove (recognizer.Recognizer);
			}
		}

		void Initialize ()
		{
			if (_isInitialized) {
				throw new InvalidOperationException ("asked to initialize coorodinator that was already intialized");
			}
			_nativeView = TargetView.GetNativeView ();
			foreach (var recognizer in NativeRecognizers) {
				recognizer.NativeView = _nativeView;
			}
			_nativeView.Touch += _nativeView_Touch;
			_isInitialized = true;
		}

		void UnInitialize ()
		{
			if (_nativeView != null) {
				_nativeView.Touch -= _nativeView_Touch;
				_nativeView = null;
			}
			_isInitialized = false;
		}

		#region events

		void Recognizer_View_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer") {
				var renderer = TargetView.GetRenderer ();
				if (renderer != null && !_isInitialized) {
					Initialize ();
				} else if (renderer == null && _isInitialized) {
					UnInitialize ();
				}
			}
		}


		void _nativeView_Touch (object sender, View.TouchEventArgs e)
		{
			var gestureMotionEvent = new GestureMotionEvent (e.Event);
			foreach (var recognizer in NativeRecognizers) {
				recognizer.ProcessMotionEvent (gestureMotionEvent);
			}
		}

		#endregion
	}
}

