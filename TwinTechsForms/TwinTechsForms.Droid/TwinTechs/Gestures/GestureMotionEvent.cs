using System;
using Android.Views;

namespace TwinTechs.Gestures
{
	/// <summary>
	/// Gesture Motion Event
	/// Allows us to decorate mtion events with processign state (such as canelled/cached)
	/// Includes a crude mechanism for restoring the motion event's state, after other views/gestures might've done some fooling around.
	/// This is all experimental - I suspect someone who's an android programmer (I'm an iOS guy) will give me a load of reasons
	/// why this is not a good idea. I look forward to the criticism and any ideas suggestions!
	/// </summary>
	public class GestureMotionEvent
	{


		MotionEventActions _cachedAction;
		float _cachedY;
		float _cachedX;

		public GestureMotionEvent (MotionEvent e)
		{
			_cachedAction = e.Action;
			_cachedX = e.GetX ();
			_cachedY = e.GetY ();
			MotionEvent = e;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this motionevent is marked for being 
		/// NOTE - once set to true, this flag remains true;
		/// </summary>
		/// <value><c>true</c> if this instance is cached; otherwise, <c>false</c>.</value>
		public bool IsMarkedForDelay { get { return _delayCount > 0; } }

		int _delayCount;

		public int DelayCount { get ; }

		public void ReleaseDelay ()
		{
			if (_delayCount >= 1) {
				_delayCount--;
			}
		}

		public void MarkDelay ()
		{
			_delayCount++;
		}

		bool _isCancelled;

		/// <summary>
		/// Gets or sets a value indicating whether this motionevent can be considered is cancelled.
		/// NOTE - once set to true, this flag remains true;
		/// </summary>
		/// <value><c>true</c> if this instance is cancelled; otherwise, <c>false</c>.</value>
		public bool IsCancelled {
			get { return _isCancelled; }
			set {
				if (!_isCancelled) {
					_isCancelled = value;
				}
			}
		}

		/// <summary>
		/// Gets the motion event.
		/// </summary>
		/// <value>The motion event.</value>
		public MotionEvent MotionEvent { get; private set; }

		bool _isConsumed;

		/// <summary>
		/// Gets or sets a value indicating whether this motion is consumed by a gesture recognizer.
		/// </summary>
		/// <value><c>true</c> if this instance is consumed; otherwise, <c>false</c>.</value>
		public bool IsConsumed {
			get { return _isConsumed; }
			set {
				if (!_isConsumed) {
					_isConsumed = value;
				}
			}
		}

		public MotionEvent GetCachedEvent ()
		{
			MotionEvent.Action = _cachedAction;
			MotionEvent.OffsetLocation (_cachedX - MotionEvent.GetX (), _cachedY - MotionEvent.GetY ());
			return MotionEvent;

		}

		#region proxy methods to assist my own refactoring, and aid readibility

		public MotionEventActions Action { get { return MotionEvent.Action; } }

		public MotionEventActions ActionMasked { get { return MotionEvent.ActionMasked; } }

		public int GetPointerId (int pointerIndex)
		{
			return MotionEvent.GetPointerId (pointerIndex);
		}

		public float GetX (int pointerIndex)
		{
			return MotionEvent.GetX (pointerIndex);
		}

		public float GetY (int pointerIndex)
		{
			return MotionEvent.GetY (pointerIndex);
		}


		public float GetX ()
		{
			return MotionEvent.GetX ();
		}

		public float GetY ()
		{
			return MotionEvent.GetY ();
		}


		public void GetPointerCoords (int pointerIndex, MotionEvent.PointerCoords outPointerCoords)
		{
			MotionEvent.GetPointerCoords (pointerIndex, outPointerCoords);
		}

		public int PointerCount { get { return MotionEvent.PointerCount; } }

		#endregion
	}
}

