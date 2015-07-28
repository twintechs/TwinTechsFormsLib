using System;

namespace TwinTechs.Gestures
{
	public interface INativeGestureRecognizerFactory
	{
		/// <summary>
		/// Creates the native gesture recognizer.
		/// </summary>
		/// <param name="recognizer">Recognizer.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void AddNativeGestureRecognizerToRecgonizer<T> (T recognizer) where T : BaseGestureRecognizer;

		/// <summary>
		/// Removes the recognizer.
		/// </summary>
		/// <param name="recognizer">Recognizer.</param>
		void RemoveRecognizer (BaseGestureRecognizer recognizer);
	}
}

