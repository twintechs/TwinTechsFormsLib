using System;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public interface INativeGestureRecognizer
	{
		void AddRecognizer (BaseGestureRecognizer recognizer);

		void RemoveRecognizer (BaseGestureRecognizer recognizer);

		int NumberOfTouches { get; }

		Point LocationInView (VisualElement view);

		Point LocationOfTouch (int touchIndex, VisualElement view);

		GestureRecognizerState State { get; }
	}
}

