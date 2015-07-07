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
			throw new NotImplementedException ();
		}

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			throw new NotImplementedException ();
		}

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			throw new NotImplementedException ();
		}

		public void OnShowPress (MotionEvent e)
		{
			throw new NotImplementedException ();
		}

		public bool OnSingleTapUp (MotionEvent e)
		{
			throw new NotImplementedException ();
		}
	}
}

