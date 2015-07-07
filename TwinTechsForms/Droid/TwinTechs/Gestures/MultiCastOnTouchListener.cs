using System;
using Android.Views;
using Android.Graphics;
using TwinTechs.Droid.Extensions;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	public class MultiCastOnTouchListener :  Java.Lang.Object, View.IOnTouchListener
	{
		private List<View.IOnTouchListener> Listeners { get; set; }

		public MultiCastOnTouchListener ()
		{
			Listeners = new List<View.IOnTouchListener> ();
		}

		public void AddListener (View.IOnTouchListener listener)
		{
			Listeners.Add (listener);
		}

		public void RemoveListener (View.IOnTouchListener listener)
		{
			Listeners.Remove (listener);
		}

		public bool HasListeners {
			get {
				return Listeners.Count > 0;
			}

		}

		#region IOnTouchListener implementation

		public bool OnTouch (View v, MotionEvent e)
		{
			bool isHandled = false;
			foreach (var listener in Listeners) {
				isHandled |= listener.OnTouch (v, e);
			}
			return isHandled;
		}

		#endregion

	}
}

