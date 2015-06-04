using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using Dalvik.SystemInterop;

namespace TwinTechs.Gestures
{
	
	
	public class NativePanGestureRecognizer : BaseNativeGestureRecognizer, INativePanGestureRecognizer
	{
		public NativePanGestureRecognizer ()
		{
			

		}

		#region gesture stuff

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			
			Console.WriteLine ("OnScroll");
			OnGesture ();
			return true;
		}

		#endregion

		Xamarin.Forms.Point _velocity;
		Xamarin.Forms.Point _translation;

		public Xamarin.Forms.Point GetVelocityInView (Xamarin.Forms.VisualElement view)
		{
			
			return 	Xamarin.Forms.Point.Zero;
		}

		public Xamarin.Forms.Point GetTranslationInView (Xamarin.Forms.VisualElement view)
		{
			return 	Xamarin.Forms.Point.Zero;
		}

		protected override GestureDetector CreateGestureDetector ()
		{
			//TODO wip
//			var listener = new ScaleGestureDetector.SimpleOnScaleGestureListener ();
//			var detector = new ScaleGestureDetector (NativeView.Context,listener);
			throw new NotImplementedException ();
		}

		public void SetTranslationInView (Xamarin.Forms.Point translation, Xamarin.Forms.VisualElement view)
		{
			throw new NotImplementedException ();
		}
	}
}

