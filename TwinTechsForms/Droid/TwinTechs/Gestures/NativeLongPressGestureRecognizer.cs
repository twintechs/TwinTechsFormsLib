using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;

namespace TwinTechs.Gestures
{
	public class NativeLongPressGestureRecognizer : BaseNativeGestureRecognizer,GestureDetector.IOnGestureListener
	{
		public NativeLongPressGestureRecognizer ()
		{
		}

		#region gesture stuff

		public void OnLongPress (MotionEvent e)
		{
			NumberOfTouches = e.PointerCount;
			if (NumberOfTouches < (this.Recognizer as LongPressGestureRecognizer).NumberOfTouchesRequired) {
				return;
			}
			FirstTouchPoint = new Xamarin.Forms.Point (e.GetX (0), e.GetY (0));

			Console.WriteLine ("OnLongPress");
			State = GestureRecognizerState.Recognized;
			OnGesture ();
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
			return false;
		}
	}
}

