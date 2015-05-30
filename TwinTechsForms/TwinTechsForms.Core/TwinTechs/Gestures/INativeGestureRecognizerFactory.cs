using System;

namespace TwinTechs.Gestures
{
	public interface INativeGestureRecognizerFactory
	{
		INativeGestureRecognizer CreateNativeGestureRecognizer<T> (T recognizer);
	}
}

