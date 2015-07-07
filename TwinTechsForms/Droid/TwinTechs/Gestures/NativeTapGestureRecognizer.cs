using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;

namespace TwinTechs.Gestures
{
	public class NativeTapGestureRecognizer : BaseNativeGestureRecognizer,GestureDetector.IOnGestureListener
	{
		public NativeTapGestureRecognizer ()
		{
		}

		#region gesture stuff

		public void OnLongPress (MotionEvent e)
		{
//			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
//			Console.WriteLine ("OnLongPress");
//			OnGesture ();
//			base.OnLongPress (e);
		}


		protected override GestureDetector CreateGestureDetector ()
		{
			return new GestureDetector (this);
		}

		#endregion

		public bool OnDown (MotionEvent e)
		{
			return false;
		}

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			return false;
		}

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			return false;
		}

		public void OnShowPress (MotionEvent e)
		{
			
		}

		public bool OnSingleTapUp (MotionEvent e)
		{
			State = GestureRecognizerState.Recognized;
			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));
			Console.WriteLine ("On Tap");

			OnGesture ();
			return true;
		}
	}
}

