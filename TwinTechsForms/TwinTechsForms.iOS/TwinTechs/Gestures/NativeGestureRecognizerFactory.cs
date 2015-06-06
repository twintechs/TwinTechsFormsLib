using System;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	/// <summary>
	/// Creates gesture recognizers
	/// </summary>
	public class NativeGestureRecognizerFactory : INativeGestureRecognizerFactory
	{
		#region INativeGestureRecognizerFactory implementation

		Dictionary<Type,Type> TypeDictionary = new Dictionary<Type, Type> () {
			[typeof(SwipeGestureRecognizer) ] = typeof(NativeSwipeGestureRecognizer),
			[typeof(PanGestureRecognizer) ] = typeof(NativePanGestureRecognizer),
			[typeof(PinchGestureRecognizer) ] = typeof(NativePinchGestureRecognizer),
			[typeof(LongPressGestureRecognizer) ] = typeof(NativeLongPressGestureRecgonizer),
			[typeof(TwinTechs.Gestures.TapGestureRecognizer) ] = typeof(NativeTapPressGestureRecgonizer),
		};

		public INativeGestureRecognizer CreateNativeGestureRecognizer<T> (T recognizer)
		{
			if (!TypeDictionary.ContainsKey (recognizer.GetType ())) {
				throw new ArgumentException ("no native gesture recognizer for this forms recognizer " + recognizer.GetType ());
			}
			var targetType = TypeDictionary [recognizer.GetType ()];
			return (INativeGestureRecognizer)Activator.CreateInstance (targetType);
		}

		#endregion
		
	}
}

